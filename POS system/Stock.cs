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
        public partial class Stock: Form
        {
            public Stock()
            {
                InitializeComponent();
                LoadStock();
            }

            private void label2_Click(object sender, EventArgs e)
            {

            }

        StockDBDataSet db = new StockDBDataSet();

            private void LoadStock()
            {
                var stockAdapter = new POS_system.StockDBDataSetTableAdapters.StockTableAdapter();
                db.Stock.Clear();
                stockAdapter.Fill(db.Stock);
                dgtStock.DataSource = db.Stock;
            }

            private void btnAdd_Click(object sender, EventArgs e)
            {
                var adapter = new POS_system.StockDBDataSetTableAdapters.sp_StockTableAdapter();
                var result = adapter.GetData(
                    txtProductname.Text,
                    txtCategory.Text,
                    decimal.Parse(txtunitPrice.Text),
                    txtMaterial.Text,
                    DateTime.Now);
                MessageBox.Show("Product saved. NewStockID = " + result[0].NewStockID);
                LoadStock();
            }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
    }
