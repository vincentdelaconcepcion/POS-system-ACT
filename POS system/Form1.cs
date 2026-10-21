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

           ;
            Session.AddLogoutButton(this, panel2);
            
           
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
