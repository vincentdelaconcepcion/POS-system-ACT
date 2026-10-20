using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_system
{
    public partial class StockDisplay : Form
    {
        private decimal total = 0;
        private readonly SP_StockDataContext db = new SP_StockDataContext();
        private List<Panel> productCards;
        private Dictionary<Panel, Point> cardLocations;

        private static readonly Color InStockColor = Color.FromArgb(46, 125, 50);
        private static readonly Color OutOfStockColor = Color.FromArgb(198, 40, 40);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        public StockDisplay()
        {
            InitializeComponent();
            ApplyRoleAccess();
            Session.AddLogoutButton(this, panel2);
            InitializeSearch();
            LoadStockStatus();
        }

        private void ApplyRoleAccess()
        {
            Session.HideUnless(btnHome, Session.CanSeeHome);
            Session.HideUnless(button6, Session.CanManageStock);
            Session.HideUnless(button12, Session.CanOpenSettings);
        }

        private void LoadStockStatus()
        {
            try
            {
                var quantities = db.sp_Search(null)
                    .GroupBy(s => s.ProductName, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.Sum(s => s.Quantity), StringComparer.OrdinalIgnoreCase);

                foreach (Panel card in productCards)
                {
                    Button buyButton = card.Controls.OfType<Button>().FirstOrDefault();
                    string productName = GetProductName(card);

                    if (buyButton == null || string.IsNullOrEmpty(productName))
                        continue;

                    int quantity;
                    bool available = quantities.TryGetValue(productName, out quantity) && quantity > 0;

                    ApplyStockStatus(buyButton, available);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load stock availability:\n\n" + ex.Message,
                    "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ApplyStockStatus(Button button, bool available)
        {
            button.Tag = available;
            button.Text = available ? "BUY" : "OUT OF STOCK";
            button.BackColor = available ? InStockColor : OutOfStockColor;
            button.ForeColor = Color.White;
            button.Cursor = available ? Cursors.Hand : Cursors.No;
            button.Enabled = available;
            button.TabStop = available;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Form1 homeForm = new Form1();
            homeForm.Show();
            this.Hide();
        }

        private void btnStockDisplay_Click(object sender, EventArgs e)
        {
            StockForm product = new StockForm();
            product.Show();
            this.Hide();
        }

        private void BuyButton_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var card = btn.Parent;   // the panel holding this product

            // Labels sorted top to bottom: first = name, second = price
            var labels = card.Controls.OfType<Label>()
                             .OrderBy(l => l.Top)
                             .ToList();

            string name = labels[0].Text;
            decimal price = decimal.Parse(
                labels[1].Text.Replace("₱", "").Trim(),
                CultureInfo.InvariantCulture);

            lstCart.Items.Add($"{name} - ₱{price:N2}");

            total += price;
            lblTotal.Text = $"Total: ₱{total:N2}";

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void InitializeSearch()
        {
            productCards = panel1.Controls.OfType<Panel>().ToList();
            cardLocations = productCards.ToDictionary(c => c, c => c.Location);
            SendMessage(txtsearch.Handle, EM_SETCUEBANNER, (IntPtr)1, "Search product...");
        }


        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtsearch.Text.Trim();
            bool filtering = search.Length > 0;

            button1.Visible = !filtering;
            button16.Visible = !filtering;
            button25.Visible = !filtering;
            button36.Visible = !filtering;

            int index = 0;

            foreach (Panel card in productCards)
            {
                string productName = GetProductName(card);
                bool matches = !filtering
                    || productName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;

                card.Visible = matches;

                if (!filtering)
                {
                    card.Location = cardLocations[card];
                }
                else if (matches)
                {
                    int column = index % 4;
                    int row = index / 4;
                    card.Location = new Point(24 + column * 174, 53 + row * 253);
                    index++;
                }
            }

            panel1.AutoScrollPosition = new Point(0, 0);
        }

        private static string GetProductName(Panel card)
        {
            var labels = card.Controls.OfType<Label>().OrderBy(l => l.Top).ToList();
            return labels.Count >= 2 ? labels[0].Text.Trim() : string.Empty;
        }

    }
}
