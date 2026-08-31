using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_system
{
    public partial class Form1 : Form
    {
        SP_StockDataContext db = new SP_StockDataContext();
        private List<Stock> allStock;

        public Form1()
        {
            InitializeComponent();
            LoadStock();
        }

        private void LoadStock()
        {
            db = new SP_StockDataContext();
            allStock = db.Stocks.ToList();
            dgtStock.DataSource = allStock;

            dgtStock.Columns["StockID"].Visible = false;
            dgtStock.Columns["ProductName"].HeaderText = "Product Name";
            dgtStock.Columns["UnitPrice"].HeaderText = "Unit Price";
            dgtStock.Columns["DateAdded"].HeaderText = "Date Added";
        }

        private void txtS_search_TextChanged(object sender, EventArgs e)
        {
            string search = txtS_search.Text.Trim().ToLower();

            var filtered = allStock.Where(s =>
                s.ProductName.ToLower().Contains(search) ||
                s.Category.ToLower().Contains(search) ||
                (s.Material != null && s.Material.ToLower().Contains(search)) ||
                s.UnitPrice.ToString().Contains(search) ||
                s.DateAdded.ToString().ToLower().Contains(search)
            ).ToList();

            dgtStock.DataSource = filtered;
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
    }
}
