using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_system
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Resize += Form1_Resize;

            if (!Session.CanSeeHome)
            {
                Shown += (s, e) =>
                {
                    new StockDisplay().Show();
                    Close();
                };
                return;
            }

            ApplyRoleAccess();
            Session.AddLogoutButton(this, panel2);
            LayoutDashboard();
        }

        private void ApplyRoleAccess()
        {
            Session.HideUnless(button6, Session.CanManageStock);
            Session.HideUnless(btnAdminPanel, Session.CanOpenSettings);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                LayoutDashboard();
        }

        private void LayoutDashboard()
        {
            if (panel1 == null || panel2 == null || panel15 == null ||
                ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return;

            int width = ClientSize.Width;
            int height = ClientSize.Height;
            bool compact = height < 800;

            int headerHeight = compact ? 120 : 140;
            int footerHeight = compact ? 70 : 86;
            int statHeight = compact ? 126 : 146;
            int pageGap = compact ? 12 : 18;
            int pageMargin = compact ? 16 : 24;
            int cardGap = compact ? 12 : 18;

            SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel15.SuspendLayout();
            panel8.SuspendLayout();
            panel13.SuspendLayout();
            panel14.SuspendLayout();

            try
            {
                panel2.Width = 300;
                int contentLeft = panel2.Width;
                int contentWidth = Math.Max(320, width - contentLeft);

                panel1.Location = new Point(contentLeft, 0);
                panel1.Size = new Size(contentWidth, headerHeight);
                FitBannerImage(pictureBox1, panel1, compact ? 30 : 60);

                int statsTop = panel1.Bottom + pageGap;
                int usableWidth = contentWidth - (pageMargin * 2);
                int statWidth = Math.Max(150, (usableWidth - (cardGap * 3)) / 4);
                Panel[] statCards = { panel4, panel5, panel6, panel7 };

                for (int i = 0; i < statCards.Length; i++)
                {
                    Panel card = statCards[i];
                    card.Location = new Point(
                        contentLeft + pageMargin + i * (statWidth + cardGap),
                        statsTop);
                    card.Size = new Size(statWidth, statHeight);
                    LayoutMetricCard(card);
                }

                int footerTop = height - footerHeight;
                panel15.Location = new Point(contentLeft, footerTop);
                panel15.Size = new Size(contentWidth, footerHeight);

                label32.Top = Math.Max(6, (footerHeight - label32.Height - label31.Height - 8) / 2);
                label31.Top = label32.Bottom + 2;
                FitBannerImage(pictureBox2, panel15, compact ? 24 : 48);

                int largeTop = statsTop + statHeight + pageGap;
                int largeHeight = Math.Max(180, footerTop - pageGap - largeTop);
                int largeGap = cardGap;
                int leftWidth = (int)((usableWidth - largeGap) * 0.54);
                int rightWidth = usableWidth - largeGap - leftWidth;

                panel8.BackColor = BackColor;
                panel8.Location = new Point(contentLeft + pageMargin, largeTop);
                panel8.Size = new Size(leftWidth, largeHeight);
                panel13.BackColor = BackColor;
                panel13.Location = new Point(panel8.Right + largeGap, largeTop);
                panel13.Size = new Size(rightWidth, largeHeight);

                LayoutMetricGrid();
                LayoutTotalSalesCard();
                LayoutSidebar();
            }
            finally
            {
                panel14.ResumeLayout();
                panel13.ResumeLayout();
                panel8.ResumeLayout();
                panel15.ResumeLayout();
                panel1.ResumeLayout();
                panel2.ResumeLayout();
                ResumeLayout();
            }
        }

        private void LayoutMetricGrid()
        {
            int padding = panel8.Width < 500 ? 14 : 18;
            int gap = panel8.Width < 500 ? 10 : 14;
            int cellWidth = Math.Max(120, (panel8.ClientSize.Width - (padding * 2) - gap) / 2);
            int cellHeight = Math.Max(110, (panel8.ClientSize.Height - (padding * 2) - gap) / 2);

            panel9.Location = new Point(padding, padding);
            panel9.Size = new Size(cellWidth, cellHeight);

            panel11.Location = new Point(padding + cellWidth + gap, padding);
            panel11.Size = new Size(cellWidth, cellHeight);

            panel10.Location = new Point(padding, padding + cellHeight + gap);
            panel10.Size = new Size(cellWidth, cellHeight);

            panel12.Location = new Point(padding + cellWidth + gap, padding + cellHeight + gap);
            panel12.Size = new Size(cellWidth, cellHeight);

            LayoutMetricCard(panel9);
            LayoutMetricCard(panel10);
            LayoutMetricCard(panel11);
            LayoutMetricCard(panel12);
        }

        private static void LayoutMetricCard(Panel card)
        {
            var labels = card.Controls.OfType<Label>().OrderBy(l => l.Top).ToList();
            if (labels.Count < 3)
                return;

            int top = card.Height < 140 ? 12 : 18;
            int gap = card.Height < 140 ? 36 : 42;
            int left = Math.Max(16, Math.Min(24, card.Width / 12));

            labels[0].Location = new Point(left, top);
            labels[1].Location = new Point(left, top + gap);
            labels[2].Location = new Point(left, top + (gap * 2));
        }

        private void LayoutTotalSalesCard()
        {
            panel14.Location = new Point(18, 18);
            panel14.Size = new Size(
                Math.Max(120, panel13.ClientSize.Width - 36),
                Math.Max(120, panel13.ClientSize.Height - 36));

            int centerX = panel14.ClientSize.Width / 2;
            int centerY = panel14.ClientSize.Height / 2;

            label30.Location = new Point(Math.Max(8, centerX - (label30.Width / 2)), Math.Max(8, centerY - 66));
            label29.Location = new Point(Math.Max(8, centerX - (label29.Width / 2)), Math.Max(8, centerY - 22));
            label28.Location = new Point(Math.Max(8, centerX - (label28.Width / 2)), Math.Max(8, centerY + 22));
        }

        private void LayoutSidebar()
        {
            if (panel2 == null || panel3 == null)
                return;

            Control logout = panel2.Controls["btnLogout"];
            if (logout != null)
            {
                logout.Left = 26;
                logout.Top = Math.Max(panel3.Bottom + 80, panel2.ClientSize.Height - logout.Height - 20);
            }

            Button[] navButtons = { button1, btnStockDisplay, button6, btnAdminPanel };
            var visibleButtons = navButtons.Where(b => b != null && b.Visible).ToArray();
            if (visibleButtons.Length == 0)
                return;

            const int buttonHeight = 65;
            const int buttonGap = 24;
            int groupHeight = (visibleButtons.Length * buttonHeight) + ((visibleButtons.Length - 1) * buttonGap);
            int topLimit = panel3.Bottom;
            int bottomLimit = logout != null ? logout.Top - 24 : panel2.ClientSize.Height - 24;
            int startY = topLimit + Math.Max(20, (bottomLimit - topLimit - groupHeight) / 2);

            for (int i = 0; i < visibleButtons.Length; i++)
            {
                visibleButtons[i].Location = new Point(26, startY + i * (buttonHeight + buttonGap));
                visibleButtons[i].Size = new Size(248, buttonHeight);
            }
        }

        private static void FitBannerImage(PictureBox picture, Panel banner, int rightMargin)
        {
            if (picture == null || picture.Image == null)
                return;

            int maxWidth = Math.Max(1, (int)(banner.ClientSize.Width * 0.42));
            int maxHeight = Math.Max(1, banner.ClientSize.Height);
            double scale = Math.Min(
                (double)maxHeight / picture.Image.Height,
                (double)maxWidth / picture.Image.Width);

            int imageWidth = Math.Max(1, (int)(picture.Image.Width * scale));
            int imageHeight = Math.Max(1, (int)(picture.Image.Height * scale));

            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.Size = new Size(imageWidth, imageHeight);
            picture.Location = new Point(
                Math.Max(0, banner.ClientSize.Width - imageWidth - rightMargin),
                (banner.ClientSize.Height - imageHeight) / 2);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            StockForm productsForm = new StockForm();
            productsForm.Show();
            this.Hide();    

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void srchj_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LayoutDashboard();
        }

        private void flowLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        public class RoundedPanel : Panel
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            StockForm productsForm = new StockForm();
            productsForm.Show();
            this.Hide();
        }

        private void btnStockDisplay_Click(object sender, EventArgs e)
        {
            StockDisplay displaystock = new StockDisplay();
            displaystock.Show();
            this.Hide();
        }
    }
}
