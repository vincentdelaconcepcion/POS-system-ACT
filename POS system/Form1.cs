using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_system
{
    public partial class Form1 : Form
    {
        private const string Peso = "\u20B1";

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

            Session.HideUnless(btnAdminPanel, Session.CanOpenSettings);
            Session.AddLogoutButton(this, panel2);
            LayoutDashboard();
            Resize += (s, e) => LayoutDashboard();
        }

        // this code is for the forms for the common controls to be auto adjust to the masximum width of the form
        private void LayoutDashboard()
        {
            if (panel2 == null || panel1 == null || panel4 == null || panel5 == null ||
                panel6 == null || panel7 == null || panel8 == null || lbllowstock == null)
                return;

            int width = ClientSize.Width;
            int height = ClientSize.Height;
            if (width <= 0 || height <= 0)
                return;

            const int sidebarWidth = 300;
            panel2.Location = Point.Empty;
            panel2.Size = new Size(sidebarWidth, height);

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
            lbllowstock.Location = new Point(left, gridTop - 38);
            

            int gap = 16;
            int cardWidth = Math.Max(160, (contentWidth - gap) / 2);
            int cardsBottom = lbllowstock.Top - 20;
            int cardHeight = Math.Max(90, (cardsBottom - top - gap) / 2);

            panel4.Location = new Point(left, top);
            panel4.Size = new Size(cardWidth, cardHeight);
            panel5.Location = new Point(left + cardWidth + gap, top);
            panel5.Size = new Size(cardWidth, cardHeight);
            panel6.Location = new Point(left, top + cardHeight + gap);
            panel6.Size = new Size(cardWidth, cardHeight);
            panel7.Location = new Point(left + cardWidth + gap, top + cardHeight + gap);
            panel7.Size = new Size(cardWidth, cardHeight);
            panel8.Location = new Point(panel6.Left, panel6.Bottom + gap);
            panel8.Size = new Size(cardWidth, cardHeight);


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

        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(
                    "POS_system.Properties.Settings.StockDBConnectionString3")))
                {
                    conn.Open();

                    label8.Text = Convert.ToInt32(ExecuteScalar(conn,
                        "SELECT COUNT(*) FROM dbo.Stock WHERE IsArchived = 0"))
                        .ToString("N0", CultureInfo.CurrentCulture);

                    label4.Text = Convert.ToInt32(ExecuteScalar(conn,
                        "SELECT COUNT(*) FROM dbo.Orders"))
                        .ToString("N0", CultureInfo.CurrentCulture);

                    

                    decimal totalSales = Convert.ToDecimal(ExecuteScalar(conn,
                        "SELECT ISNULL(SUM(TotalAmount), 0) FROM dbo.Orders"));
                    label13.Text = Peso + totalSales.ToString("N2", CultureInfo.CurrentCulture);

                    DataTable sales = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT TOP (20) OrderID, OrderDate, Cashier, PaymentMethod, ItemCount, TotalAmount " +
                        "FROM dbo.vw_SalesReport ORDER BY OrderDate DESC, OrderID DESC",
                        conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(sales);
                    }

                    
                }
            }
            catch
            {
                label8.Text = "-";
                label4.Text = "-";
                label13.Text = "-";
                
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(
                    "LogInConnectionString")))
                {
                    conn.Open();
                    label10.Text = Convert.ToInt32(ExecuteScalar(conn,
                        "SELECT COUNT(*) FROM dbo.[User]"))
                        .ToString("N0", CultureInfo.CurrentCulture);
                }
            }
            catch
            {
                label10.Text = "-";
            }

            LayoutDashboard();
        }

        

        private static object ExecuteScalar(SqlConnection connection, string commandText)
        {
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {
                return cmd.ExecuteScalar();
            }
        }

        private static string GetConnectionString(string name)
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[name];
            if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
                throw new InvalidOperationException(name + " is missing from App.config");

            return setting.ConnectionString;
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
            LoadDashboardData();
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

        private void btnAdminPanel_Click(object sender, EventArgs e)
        {
            AdminPanel adminpanel = new AdminPanel();
            adminpanel.Show();
            this.Hide();
        }

        
    }
}
