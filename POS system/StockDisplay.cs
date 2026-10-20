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
        public StockDisplay()
        {
            InitializeComponent();
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void label48_Click(object sender, EventArgs e)
        {

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
