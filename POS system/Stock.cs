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
        private List<sp_SearchResult> allStock; // displays all stock items in memory for filtering
        private int selectedStockId = -1;
        private bool isViewArchived = false;

        private void ApplyGridFormatting()
        {
            if (dgtStock.Columns.Contains("StockID"))
                dgtStock.Columns["StockID"].Visible = false;
            if (dgtStock.Columns.Contains("ProductName"))
                dgtStock.Columns["ProductName"].HeaderText = "Product Name";
            if (dgtStock.Columns.Contains("UnitPrice"))
                dgtStock.Columns["UnitPrice"].HeaderText = "Unit Price";
            if (dgtStock.Columns.Contains("DateAdded"))
                dgtStock.Columns["DateAdded"].HeaderText = "Date Added";
        }

        private List<sp_SearchResult> GetArchivedList()
        {
            List<sp_SearchResult> list = new List<sp_SearchResult>();

            using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_GetArchived", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new sp_SearchResult
                        {
                            StockID = Convert.ToInt32(reader["StockID"]),
                            ProductName = reader["ProductName"].ToString(),
                            Category = reader["Category"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Material = reader["Material"] == DBNull.Value ? null : reader["Material"].ToString(),
                            DateAdded = Convert.ToDateTime(reader["DateAdded"])
                        });
                    }
                }
            }

            return list;
        }

        private void RunStockAction(string procName, string paramName, int stockId)
        {
            using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(procName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(paramName, stockId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateButtonVisibility()
        {
            if (isViewArchived)
            {
                btnAdd.Hide();
                btnUpdate.Hide();
                btnArchive.Hide();
                btnRestore.Show();
                btnViewArchived.Text = "View Stock";
            }
            else
            {
                btnAdd.Show();
                btnUpdate.Hide();
                btnArchive.Show();
                btnRestore.Hide();
                btnViewArchived.Text = "View Archived";
            }
        }

        private void LoadStock()
        {
            db = new SP_StockDataContext();
            allStock = isViewArchived ? GetArchivedList() : db.sp_Search(null).ToList();
            dgtStock.DataSource = allStock;

            ApplyGridFormatting();
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

            sp_SearchResult selected = dgtStock.Rows[e.RowIndex].DataBoundItem as sp_SearchResult;
            if (selected == null)
                return;

            selectedStockId = selected.StockID;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedStockId < 0)
            {
                MessageBox.Show("Select a product from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to permanently delete this product?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                db = new SP_StockDataContext();
                using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("dbo.sp_DeleteStock", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StockID", selectedStockId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                selectedStockId = -1;
                LoadStock();
                ClearInputs();
                MessageBox.Show("Stock deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting stock:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtS_search_TextChanged(object sender, EventArgs e)
        {
            string search = txtS_search.Text.Trim();

            db = new SP_StockDataContext();

            if (isViewArchived)
            {
                dgtStock.DataSource = allStock
                    .Where(s => string.IsNullOrEmpty(search)
                        || (!string.IsNullOrEmpty(s.ProductName) && s.ProductName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                        || (!string.IsNullOrEmpty(s.Category) && s.Category.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                        || (!string.IsNullOrEmpty(s.Material) && s.Material.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();
            }
            else
            {
                dgtStock.DataSource = db.sp_Search(search).ToList();
            }

            ApplyGridFormatting();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedStockId < 0)
            {
                MessageBox.Show("Select a product from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                db = new SP_StockDataContext();
                db.sp_UpdateProduct(
                    txtProductname.Text,
                    cmbCategory.SelectedItem != null ? cmbCategory.SelectedItem.ToString() : string.Empty,
                    Convert.ToDecimal(txtunitPrice.Text),
                    txtMaterial.Text,
                    dtpDateAdded.Value,
                    selectedStockId
                );
                LoadStock();
                selectedStockId = -1;
                ClearInputs();
                UpdateButtonVisibility();
                MessageBox.Show("Successfully Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating stock:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            isViewArchived = false;
            UpdateButtonVisibility();
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            if (selectedStockId < 0)
            {
                MessageBox.Show("Select a product from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string productName = txtProductname.Text.Trim();
            DialogResult confirm = MessageBox.Show(
                string.IsNullOrEmpty(productName)
                    ? "Are you sure you want to archive this product?"
                    : "Are you sure you want to archive \"" + productName + "\"?",
                "Confirm Archive",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                db = new SP_StockDataContext();
                RunStockAction("dbo.sp_ArchiveStock", "@StockID", selectedStockId);

                selectedStockId = -1;
                LoadStock();
                ClearInputs();
                MessageBox.Show("Product archived successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error archiving stock:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (selectedStockId < 0)
            {
                MessageBox.Show("Select a product from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string productName = txtProductname.Text.Trim();
            DialogResult confirm = MessageBox.Show(
                string.IsNullOrEmpty(productName)
                    ? "Are you sure you want to restore this product?"
                    : "Are you sure you want to restore \"" + productName + "\"?",
                "Confirm Restore",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                db = new SP_StockDataContext();
                RunStockAction("dbo.sp_RestoreStock", "@StockID", selectedStockId);

                selectedStockId = -1;
                LoadStock();
                ClearInputs();
                MessageBox.Show("Product restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restoring stock:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewArchived_Click(object sender, EventArgs e)
        {
            isViewArchived = !isViewArchived;
            selectedStockId = -1;
            LoadStock();
            ClearInputs();
            UpdateButtonVisibility();
        }

        private void dgtStock_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (!isViewArchived)
            {
                btnAdd.Hide();
                btnUpdate.Show();
            }

            sp_SearchResult selected = dgtStock.Rows[e.RowIndex].DataBoundItem as sp_SearchResult;
            if (selected == null)
                return;

            selectedStockId = selected.StockID;
            txtProductname.Text = selected.ProductName;

            if (selected.Category != null && cmbCategory.Items.Contains(selected.Category))
                cmbCategory.SelectedItem = selected.Category;
            else
                cmbCategory.SelectedIndex = -1;

            txtunitPrice.Text = selected.UnitPrice.ToString("0.00");
            txtMaterial.Text = selected.Material ?? string.Empty;
            dtpDateAdded.Value = selected.DateAdded;
        }
    }
}
