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
    public partial class StockForm : Form
    {
        public StockForm()

        {
            InitializeComponent();
            LoadStock();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

      
        SP_StockDataContext db = new SP_StockDataContext();
        private List<Stock> allStock; // displays all stock items in memory for filtering

        private void LoadStock()
        {
            db = new SP_StockDataContext();
            allStock = db.sp_Search(null).ToList(); // load all items at once
            dgtStock.DataSource = allStock;

            dgtStock.Columns["StockID"].Visible = false;
            dgtStock.Columns["ProductName"].HeaderText = "Product Name";
            dgtStock.Columns["UnitPrice"].HeaderText = "Unit Price";
            dgtStock.Columns["DateAdded"].HeaderText = "Date Added";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // declaring a new instance of the data context to ensure we have a fresh context for the operation
                db = new SP_StockDataContext();
                // calling the stored procedure to add a new stock item
                db.sp_Stock(
                    txtProductname.Text,
                    cmbCategory.SelectedItem.ToString(),
                    Convert.ToDecimal(txtunitPrice.Text),
                    txtMaterial.Text,
                    dtpDateAdded.Value
                );
                LoadStock();
                ClearInputs();
                MessageBox.Show("Stock added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding stock:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            //clears the textboxes and resets the date picker to the current date everytime you adding a new stock
            txtProductname.Clear();
            txtunitPrice.Clear();
            txtMaterial.Clear();
           
            dtpDateAdded.Value = DateTime.Now;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Navigates back to the main form when the Back button is clicked
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
        
        private void dgtStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            Stock selected = dgtStock.Rows[e.RowIndex].DataBoundItem as Stock;
            if (selected == null)
                return;

            txtProductname.Text = selected.ProductName;

            if (selected.Category != null && cmbCategory.Items.Contains(selected.Category))
                cmbCategory.SelectedItem = selected.Category;
            else
                cmbCategory.SelectedIndex = -1;

            txtunitPrice.Text = selected.UnitPrice.ToString("0.00");
            txtMaterial.Text = selected.Material ?? string.Empty;
            dtpDateAdded.Value = selected.DateAdded;
        }

        private void dgtStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtS_search_TextChanged(object sender, EventArgs e)
        {
            string search = txtS_search.Text.Trim();

            db = new SP_StockDataContext();
            dgtStock.DataSource = db.sp_Search(search).ToList();

            dgtStock.Columns["StockID"].Visible = false;
            dgtStock.Columns["ProductName"].HeaderText = "Product Name";
            dgtStock.Columns["UnitPrice"].HeaderText = "Unit Price";
            dgtStock.Columns["DateAdded"].HeaderText = "Date Added";
        }
    }
}
