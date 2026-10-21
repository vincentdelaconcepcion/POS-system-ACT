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
            WindowState = FormWindowState.Maximized;

            if (!Session.CanSeeHome)
            {
                Shown += (s, e) =>
                {
                    new StockDisplay().Show();
                    Close();
                };
                return;
            }

            Session.AddLogoutButton(this, panel2);
            LayoutDashboard();
            Resize += (s, e) => LayoutDashboard();
        }

        private void LayoutDashboard()
        {
            if (panel2 == null || panel1 == null || panel4 == null || panel5 == null ||
                panel6 == null || panel7 == null || dataGridView1 == null || label12 == null)
                return;

            int width = ClientSize.Width;
            int height = ClientSize.Height;
            if (width <= 0 || height <= 0)
                return;

            int sidebarWidth = Math.Max(200, Math.Min(300, width / 5));
            panel2.Location = Point.Empty;
            panel2.Size = new Size(sidebarWidth, height);
            LayoutSidebar();

            int headerHeight = Math.Max(90, Math.Min(150, height / 7));
            panel1.Location = new Point(sidebarWidth, 0);
            panel1.Size = new Size(Math.Max(1, width - sidebarWidth), headerHeight);

            label1.Location = new Point(28, 12);
            label2.Location = new Point(30, label1.Bottom + 4);
            FitBannerImage(pictureBox1, panel1, 24);

            int left = sidebarWidth + 20;
            int top = headerHeight + 20;
            int right = width - 20;
            int bottom = height - 20;
            int contentWidth = Math.Max(200, right - left);

            int gridHeight = Math.Max(80, Math.Min(220, (bottom - top) / 4));
            int gridTop = bottom - gridHeight;
            label12.Location = new Point(left, gridTop - 38);
            dataGridView1.Location = new Point(left, gridTop);
            dataGridView1.Size = new Size(contentWidth, gridHeight);

            int gap = 16;
            int cardWidth = Math.Max(160, (contentWidth - gap) / 2);
            int cardsBottom = label12.Top - 20;
            int cardHeight = Math.Max(90, (cardsBottom - top - gap) / 2);

            panel4.Location = new Point(left, top);
            panel4.Size = new Size(cardWidth, cardHeight);
            panel5.Location = new Point(left + cardWidth + gap, top);
            panel5.Size = new Size(cardWidth, cardHeight);
            panel6.Location = new Point(left, top + cardHeight + gap);
            panel6.Size = new Size(cardWidth, cardHeight);
            panel7.Location = new Point(left + cardWidth + gap, top + cardHeight + gap);
            panel7.Size = new Size(cardWidth, cardHeight);

            LayoutStatCard(panel4, label3, label8);
            LayoutStatCard(panel5, label5, label4);
            LayoutStatCard(panel6, label9, label13);
            LayoutStatCard(panel7, label11, label10);
        }

        private static void LayoutStatCard(Panel card, Label title, Label value)
        {
            if (card == null || title == null || value == null)
                return;

            title.Location = new Point(18, 14);
            value.Location = new Point(
                Math.Max(18, (card.ClientSize.Width - value.Width) / 2),
                Math.Max(title.Bottom + 4, (card.ClientSize.Height - value.Height) / 2 + 8));
        }

        private void LayoutSidebar()
        {
            if (panel2 == null || panel3 == null)
                return;

            int sidebarWidth = panel2.ClientSize.Width;
            if (sidebarWidth <= 0)
                return;

            int logoWidth = Math.Max(60, Math.Min(sidebarWidth - 56, 130));
            int logoHeight = (pictureBox7 != null && pictureBox7.BackgroundImage != null)
                ? (int)(logoWidth * (double)pictureBox7.BackgroundImage.Height / pictureBox7.BackgroundImage.Width)
                : (int)(logoWidth * 0.775);
            if (pictureBox7 != null)
            {
                pictureBox7.Size = new Size(logoWidth, logoHeight);
                pictureBox7.Location = new Point((sidebarWidth - logoWidth) / 2, 20);
            }

            panel3.Size = new Size(sidebarWidth, Math.Max(140, (pictureBox7 != null ? pictureBox7.Bottom : 113) + 24));

            int buttonWidth = Math.Max(120, sidebarWidth - 40);
            int buttonHeight = Math.Max(36, Math.Min(56, panel2.ClientSize.Height / 16));
            int buttonGap = Math.Max(10, Math.Min(20, buttonHeight / 3));

            Control logout = panel2.Controls["btnLogout"];
            if (logout != null)
            {
                logout.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                logout.Size = new Size(buttonWidth, 48);
                logout.Left = (sidebarWidth - buttonWidth) / 2;
                logout.Top = Math.Max(panel3.Bottom + 20, panel2.ClientSize.Height - logout.Height - 20);
            }

            Button[] navButtons = { button1, btnStockDisplay, button6, btnAdminPanel };
            var visibleButtons = navButtons.Where(b => b != null && b.Visible).ToArray();
            if (visibleButtons.Length == 0)
                return;

            int groupHeight = (visibleButtons.Length * buttonHeight) + ((visibleButtons.Length - 1) * buttonGap);
            int topLimit = panel3.Bottom + 20;
            int bottomLimit = logout != null ? logout.Top - 16 : panel2.ClientSize.Height - 16;
            int startY = topLimit + Math.Max(10, (bottomLimit - topLimit - groupHeight) / 2);

            for (int i = 0; i < visibleButtons.Length; i++)
            {
                visibleButtons[i].Location = new Point((sidebarWidth - buttonWidth) / 2, startY + i * (buttonHeight + buttonGap));
                visibleButtons[i].Size = new Size(buttonWidth, buttonHeight);
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
