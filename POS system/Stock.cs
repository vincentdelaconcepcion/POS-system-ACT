    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace POS_system
{
    public partial class stick : Form
    {
        public stick()

        {
            InitializeComponent();
            LoadStock();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

      
        SP_StockDataContext db = new SP_StockDataContext();
        private List<Stock> allStock; // keep the full, unfiltered list here

        private void LoadStock()
        {
            db = new SP_StockDataContext();
            allStock = db.Stocks.ToList(); // load full list once
            dgtStock.DataSource = allStock;

            dgtStock.Columns["StockID"].Visible = false;
            dgtStock.Columns["ProductName"].HeaderText = "Product Name";
            dgtStock.Columns["UnitPrice"].HeaderText = "Unit Price";
            dgtStock.Columns["DateAdded"].HeaderText = "Date Added";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            db = new SP_StockDataContext();
            db.sp_Stock(
                txtProductname.Text,
                txtCategory.Text,
                Convert.ToDecimal(txtunitPrice.Text),
                txtMaterial.Text,
                dtpDateAdded.Value          
            );
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void dgtStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtS_search_TextChanged(object sender, EventArgs e)
        {
            string search = txtS_search.Text.Trim().ToLower();

            var filtered = allStock.Where(s =>
                s.ProductName.ToLower().Contains(search) ||
                s.Category.ToLower().Contains(search) ||
                (s.Material != null && s.Material.ToLower().Contains(search))
            ).ToList();

            dgtStock.DataSource = filtered;
        }
    }
}
