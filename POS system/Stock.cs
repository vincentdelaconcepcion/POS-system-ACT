using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
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
            ApplyUiShapes();
            this.Resize += (s, e) => ApplyUiShapes();
            txtS_search.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    e.SuppressKeyPress = true;
            };
            LoadStock();
        }

        SP_StockDataContext db = new SP_StockDataContext();
        private List<sp_SearchResult> allStock; // displays all stock items in memory for filtering
        private int selectedStockId = -1;
        private bool isViewArchived = false;

        private readonly Font gridHeaderFont = new Font("Segoe UI", 12F, FontStyle.Bold);
        private readonly Font gridFont = new Font("Segoe UI", 11F);

        private void ApplyUiShapes()
        {
            ApplyShapes(this);
        }

        private void ApplyShapes(Control root)
        {
            foreach (Control c in root.Controls)
            {
                Button btn = c as Button;
                if (btn != null)
                    RoundShape(btn, 18);

                TextBox txt = c as TextBox;
                if (txt != null)
                {
                    RoundShape(txt, 15);
                    CenterTextBoxText(txt);
                }

                if (c == pictureBox7)
                    MakeCircle(c);

                if (c.HasChildren)
                    ApplyShapes(c);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref NativeRect lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private const int EM_SETRECT = 0xB3;

        private void CenterTextBoxText(TextBox txt)
        {
            txt.TextAlign = HorizontalAlignment.Center;

            if (!txt.IsHandleCreated || !txt.Multiline || txt.Width <= 0 || txt.Height <= 0)
                return;

            int lineHeight = txt.Font.Height;
            NativeRect area = new NativeRect();
            area.Left = 6;
            area.Right = txt.ClientSize.Width - 6;
            area.Top = Math.Max(0, (txt.ClientSize.Height - lineHeight) / 2);
            area.Bottom = txt.ClientSize.Height;

            SendMessage(txt.Handle, EM_SETRECT, IntPtr.Zero, ref area);
        }

        private void MakeCircle(Control c)
        {
            if (c.Width <= 0 || c.Height <= 0)
                return;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(new Rectangle(0, 0, c.Width, c.Height));

                Region previous = c.Region;
                c.Region = new Region(path);
                if (previous != null)
                    previous.Dispose();
            }
        }

        private void RoundShape(Control c, int radius)
        {
            if (c.Width <= 0 || c.Height <= 0)
                return;

            Rectangle bounds = new Rectangle(0, 0, c.Width, c.Height);
            int d = radius * 2;

            using (GraphicsPath path = new GraphicsPath())
            {
                if (d <= 0 || bounds.Width < d || bounds.Height < d)
                {
                    path.AddRectangle(bounds);
                }
                else
                {
                    path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
                    path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
                    path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
                    path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();
                }

                Region previous = c.Region;
                c.Region = new Region(path);
                if (previous != null)
                    previous.Dispose();
            }
        }

        private void ApplyGridFormatting()
        {
            if (dgtStock.Columns.Contains("StockID"))
                dgtStock.Columns["StockID"].Visible = false;
            if (dgtStock.Columns.Contains("ProductName"))
            {
                dgtStock.Columns["ProductName"].HeaderText = "Product Name";
                dgtStock.Columns["ProductName"].FillWeight = 150;
            }
            if (dgtStock.Columns.Contains("UnitPrice"))
            {
                dgtStock.Columns["UnitPrice"].HeaderText = "Unit Price";
                dgtStock.Columns["UnitPrice"].FillWeight = 90;
            }
            if (dgtStock.Columns.Contains("DateAdded"))
            {
                dgtStock.Columns["DateAdded"].HeaderText = "Date Added";
                dgtStock.Columns["DateAdded"].DefaultCellStyle.Format = "d";
                dgtStock.Columns["DateAdded"].FillWeight = 90;
            }
            if (dgtStock.Columns.Contains("Category"))
                dgtStock.Columns["Category"].FillWeight = 110;
            if (dgtStock.Columns.Contains("Material"))
                dgtStock.Columns["Material"].FillWeight = 110;
            if (dgtStock.Columns.Contains("Quantity"))
                dgtStock.Columns["Quantity"].FillWeight = 80;
            if (dgtStock.Columns.Contains("dgtQty"))
                dgtStock.Columns["dgtQty"].FillWeight = 60;

            dgtStock.ReadOnly = false;
            foreach (DataGridViewColumn col in dgtStock.Columns)
                col.ReadOnly = (col.Name != "dgtQty");

            dgtStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgtStock.Font = gridFont;
            dgtStock.BackgroundColor = Color.White;
            dgtStock.GridColor = Color.FromArgb(215, 215, 215);
            dgtStock.EnableHeadersVisualStyles = false;
            dgtStock.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 0, 128);
            dgtStock.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgtStock.ColumnHeadersDefaultCellStyle.Font = gridHeaderFont;
            dgtStock.DefaultCellStyle.BackColor = Color.White;
            dgtStock.DefaultCellStyle.ForeColor = Color.FromArgb(35, 35, 35);
            dgtStock.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 128);
            dgtStock.DefaultCellStyle.SelectionForeColor = Color.White;
            dgtStock.ClearSelection();
            dgtStock.CurrentCell = null;
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
                            DateAdded = Convert.ToDateTime(reader["DateAdded"]),
                            Quantity = Convert.ToInt32(reader["Quantity"])
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
                btnDelete.Hide();
                btnRestore.Show();
                btnViewArchived.Text = "View Stock";
            }
            else
            {
                btnAdd.Show();
                btnUpdate.Hide();
                btnDelete.Hide();
                btnArchive.Show();
                btnRestore.Hide();
                btnViewArchived.Text = "View Archived";
            }
        }

        private int ParseQuantity()
        {
            int qty;
            return int.TryParse(txtQuantity.Text.Trim(), out qty) && qty >= 0 ? qty : 0;
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
                    cmbCategory.Text,
                    Convert.ToDecimal(txtunitPrice.Text),
                    txtMaterial.Text,
                    dtpDateAdded.Value,
                    ParseQuantity()
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
            txtQuantity.Clear();
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
            if (e.RowIndex < 0 || !isViewArchived)
            {
                UpdateButtonVisibility();
                btnUpdate.Hide();
            }

            if (e.RowIndex < 0)
                return;

            sp_SearchResult selected = dgtStock.Rows[e.RowIndex].DataBoundItem as sp_SearchResult;
            if (selected == null)
                return;

            selectedStockId = selected.StockID;
            txtProductname.Text = selected.ProductName;

            SelectCategory(selected.Category);

            txtunitPrice.Text = selected.UnitPrice.ToString("0.00");
            txtMaterial.Text = selected.Material ?? string.Empty;
            txtQuantity.Text = selected.Quantity.ToString();
            dtpDateAdded.Value = selected.DateAdded;

            if (!isViewArchived)
            {
                btnUpdate.Show();
                btnDelete.Show();
                btnAdd.Hide();
            }
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
                    cmbCategory.Text,
                    Convert.ToDecimal(txtunitPrice.Text),
                    txtMaterial.Text,
                    dtpDateAdded.Value,
                    selectedStockId,
                    ParseQuantity()
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
            ApplyUiShapes();
            LoadCategories();
        }

        private void LoadCategories()
        {
            db = new SP_StockDataContext();

            cmbCategory.DataSource = null;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryID";
            cmbCategory.DataSource = db.sp_GetCategories().ToList();
        }

        private void SelectCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                cmbCategory.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < cmbCategory.Items.Count; i++)
            {
                sp_GetCategoriesResult item = cmbCategory.Items[i] as sp_GetCategoriesResult;
                if (item != null && item.CategoryName == categoryName)
                {
                    cmbCategory.SelectedIndex = i;
                    return;
                }
            }

            cmbCategory.SelectedIndex = -1;
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

            SelectCategory(selected.Category);

            txtunitPrice.Text = selected.UnitPrice.ToString("0.00");
            txtMaterial.Text = selected.Material ?? string.Empty;
            txtQuantity.Text = selected.Quantity.ToString();
            dtpDateAdded.Value = selected.DateAdded;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Form1 home = new Form1();
            home.Show();
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