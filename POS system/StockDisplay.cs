using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_system
{
    public partial class StockDisplay : Form
    {
        private decimal total = 0;
        private readonly SP_StockDataContext db = new SP_StockDataContext();

        private static readonly Color InStockColor = Color.FromArgb(46, 125, 50);
        private static readonly Color OutOfStockColor = Color.FromArgb(198, 40, 40);

        public StockDisplay()
        {
            InitializeComponent();
            LoadStockStatus();
        }

        private void LoadStockStatus()
        {
            try
            {
                var quantities = db.sp_Search(null)
                    .GroupBy(s => s.ProductName, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.Sum(s => s.Quantity), StringComparer.OrdinalIgnoreCase);

                foreach (Panel card in panel1.Controls.OfType<Panel>())
                {
                    Button buyButton = card.Controls.OfType<Button>().FirstOrDefault();
                    var labels = card.Controls.OfType<Label>().OrderBy(l => l.Top).ToList();

                    if (buyButton == null || labels.Count < 2)
                        continue;

                    string productName = labels[0].Text.Trim();
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
    }
}
