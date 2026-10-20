using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_system
{
    public partial class StockDisplay : Form, IMessageFilter
    {
        private const string Peso = "\u20B1";
        private readonly SP_StockDataContext db = new SP_StockDataContext();
        private readonly List<CartItem> cart = new List<CartItem>();
        private List<Panel> productCards;
        private Dictionary<Panel, Point> cardLocations;

        private static readonly Color InStockColor = Color.FromArgb(46, 125, 50);
        private static readonly Color OutOfStockColor = Color.FromArgb(198, 40, 40);

        private const int WM_MOUSEWHEEL = 0x020A;
        private readonly Timer smoothScrollTimer = new Timer { Interval = 15 };
        private int scrollTarget;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        public StockDisplay()
        {
            InitializeComponent();
            //made the form full screen 
            WindowState = FormWindowState.Maximized;
            smoothScrollTimer.Tick += SmoothScrollTimer_Tick;
            Application.AddMessageFilter(this);
            FormClosed += StockDisplay_FormClosed;
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
                    .GroupBy(s => (s.ProductName ?? string.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
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
            Button btn = sender as Button;
            if (btn == null || btn.Parent == null)
                return;

            string name = GetProductName(btn.Parent as Panel);
            if (string.IsNullOrWhiteSpace(name))
                return;

            sp_SearchResult stock = db.sp_Search(null)
                .Where(s => string.Equals((s.ProductName ?? string.Empty).Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.Quantity)
                .FirstOrDefault();

            if (stock == null || stock.Quantity <= 0)
            {
                MessageBox.Show("This product is no longer available.", "Out of Stock",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadStockStatus();
                return;
            }

            CartItem item = cart.FirstOrDefault(i => i.StockID == stock.StockID);
            int quantityInCart = item == null ? 0 : item.Quantity;

            if (quantityInCart >= stock.Quantity)
            {
                MessageBox.Show(
                    string.Format("Only {0} unit(s) of {1} are available.", stock.Quantity, stock.ProductName),
                    "Stock Limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    StockID = stock.StockID,
                    ProductName = stock.ProductName.Trim(),
                    Category = stock.Category,
                    UnitPrice = stock.UnitPrice,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            RefreshCart();
        }

        private void RefreshCart()
        {
            decimal total = cart.Sum(i => i.Subtotal);

            lstCart.BeginUpdate();
            try
            {
                lstCart.Items.Clear();

                foreach (CartItem item in cart)
                {
                    lstCart.Items.Add(string.Format(
                        CultureInfo.CurrentCulture,
                        "{0} x{1} - {2}{3:N2}",
                        item.ProductName,
                        item.Quantity,
                        Peso,
                        item.Subtotal));
                }
            }
            finally
            {
                lstCart.EndUpdate();
            }

            lblTotal.Text = string.Format(CultureInfo.CurrentCulture, "Total: {0}{1:N2}", Peso, total);
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

            scrollTarget = 0;
            smoothScrollTimer.Stop();
            panel1.AutoScrollPosition = new Point(0, 0);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL || !Visible || panel1 == null || !panel1.IsHandleCreated)
                return false;

            if (!panel1.RectangleToScreen(panel1.ClientRectangle).Contains(Cursor.Position))
                return false;

            if (GetMaxScrollOffset() <= 0)
                return false;

            int wheelDelta = (short)((long)m.WParam >> 16);
            int lines = SystemInformation.MouseWheelScrollLines;
            if (wheelDelta == 0 || lines == 0)
                return false;

            int distance = lines < 0
                ? Math.Max(1, panel1.ClientSize.Height / 2)
                : lines * 40;

            ScrollProducts(wheelDelta > 0 ? -distance : distance);
            return true;
        }

        private void ScrollProducts(int delta)
        {
            int max = GetMaxScrollOffset();
            if (max <= 0)
                return;

            if (!smoothScrollTimer.Enabled)
                scrollTarget = -panel1.AutoScrollPosition.Y;

            scrollTarget = Math.Max(0, Math.Min(max, scrollTarget + delta));
            smoothScrollTimer.Start();
        }

        private void SmoothScrollTimer_Tick(object sender, EventArgs e)
        {
            int max = GetMaxScrollOffset();
            if (max <= 0)
            {
                scrollTarget = 0;
                smoothScrollTimer.Stop();
                return;
            }

            scrollTarget = Math.Min(scrollTarget, max);
            int current = -panel1.AutoScrollPosition.Y;
            int distance = scrollTarget - current;

            if (Math.Abs(distance) <= 1)
            {
                panel1.AutoScrollPosition = new Point(0, scrollTarget);
                smoothScrollTimer.Stop();
                return;
            }

            int step = distance / 3;
            if (step == 0)
                step = Math.Sign(distance);
            if (Math.Abs(step) > 120)
                step = Math.Sign(step) * 120;

            panel1.AutoScrollPosition = new Point(0, current + step);
        }

        private int GetMaxScrollOffset()
        {
            return Math.Max(0, panel1.DisplayRectangle.Height - panel1.ClientSize.Height);
        }

        private void StockDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            smoothScrollTimer.Stop();
            smoothScrollTimer.Dispose();
            Application.RemoveMessageFilter(this);
        }

        private static string GetProductName(Panel card)
        {
            var labels = card.Controls.OfType<Label>().OrderBy(l => l.Top).ToList();
            return labels.Count >= 2 ? labels[0].Text.Trim() : string.Empty;
        }

        private void StockDisplay_Load(object sender, EventArgs e)
        {

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Add at least one product before checking out.", "Cart Empty",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal totalAmount = cart.Sum(i => i.Subtotal);
            int itemCount = cart.Sum(i => i.Quantity);

            DialogResult confirm = MessageBox.Show(
                string.Format(CultureInfo.CurrentCulture,
                    "Complete this sale?\n\nItems: {0}\nTotal: {1}{2:N2}",
                    itemCount, Peso, totalAmount),
                "Confirm Checkout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            btnCheckout.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                int orderId = SaveOrder();

                MessageBox.Show(
                    string.Format(CultureInfo.CurrentCulture,
                        "Sale completed successfully.\n\nOrder #: {0}\nTotal: {1}{2:N2}",
                        orderId, Peso, totalAmount),
                    "Checkout Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cart.Clear();
                RefreshCart();
                LoadStockStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Checkout failed. No changes were saved.\n\n" + ex.Message,
                    "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadStockStatus();
            }
            finally
            {
                btnCheckout.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private int SaveOrder()
        {
            decimal totalAmount = cart.Sum(i => i.Subtotal);

            using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
            {
                conn.Open();

                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        bool hasCashier = ColumnExists(conn, tx, "dbo.Orders", "Cashier");

                        string insertOrder = hasCashier
                            ? "INSERT INTO dbo.Orders (OrderDate, TotalAmount, Cashier) VALUES (GETDATE(), @TotalAmount, @Cashier); SELECT CAST(SCOPE_IDENTITY() AS INT);"
                            : "INSERT INTO dbo.Orders (OrderDate, TotalAmount) VALUES (GETDATE(), @TotalAmount); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int orderId;

                        using (SqlCommand cmd = new SqlCommand(insertOrder, conn, tx))
                        {
                            AddMoney(cmd, "@TotalAmount", totalAmount);

                            if (hasCashier)
                            {
                                cmd.Parameters.Add("@Cashier", SqlDbType.NVarChar, 100).Value =
                                    string.IsNullOrWhiteSpace(Session.Username)
                                        ? (object)DBNull.Value
                                        : Session.Username.Trim();
                            }

                            orderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        foreach (CartItem item in cart)
                        {
                            int available;

                            using (SqlCommand cmd = new SqlCommand(
                                "SELECT Quantity FROM dbo.Stock WITH (UPDLOCK, ROWLOCK) WHERE StockID = @StockID AND IsArchived = 0",
                                conn, tx))
                            {
                                cmd.Parameters.Add("@StockID", SqlDbType.Int).Value = item.StockID;
                                object value = cmd.ExecuteScalar();

                                if (value == null || value == DBNull.Value)
                                    throw new InvalidOperationException(item.ProductName + " is no longer available.");

                                available = Convert.ToInt32(value);
                            }

                            if (available < item.Quantity)
                            {
                                throw new InvalidOperationException(string.Format(
                                    CultureInfo.CurrentCulture,
                                    "Only {0} unit(s) of {1} remain in stock.",
                                    available, item.ProductName));
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                "INSERT INTO dbo.OrderItems (OrderID, StockID, ProductName, Category, UnitPrice, Quantity, Subtotal) " +
                                "VALUES (@OrderID, @StockID, @ProductName, @Category, @UnitPrice, @Quantity, @Subtotal)",
                                conn, tx))
                            {
                                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderId;
                                cmd.Parameters.Add("@StockID", SqlDbType.Int).Value = item.StockID;
                                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 200).Value = item.ProductName;
                                cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 100).Value =
                                    string.IsNullOrWhiteSpace(item.Category) ? (object)DBNull.Value : item.Category;
                                AddMoney(cmd, "@UnitPrice", item.UnitPrice);
                                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = item.Quantity;
                                AddMoney(cmd, "@Subtotal", item.Subtotal);

                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                "UPDATE dbo.Stock SET Quantity = Quantity - @Quantity " +
                                "WHERE StockID = @StockID AND IsArchived = 0 AND Quantity >= @Quantity",
                                conn, tx))
                            {
                                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = item.Quantity;
                                cmd.Parameters.Add("@StockID", SqlDbType.Int).Value = item.StockID;

                                if (cmd.ExecuteNonQuery() != 1)
                                    throw new InvalidOperationException("Unable to reserve stock for " + item.ProductName + ".");
                            }
                        }

                        tx.Commit();
                        return orderId;
                    }
                    catch
                    {
                        try
                        {
                            tx.Rollback();
                        }
                        catch
                        {
                        }

                        throw;
                    }
                }
            }
        }

        private static void AddMoney(SqlCommand cmd, string name, decimal value)
        {
            SqlParameter parameter = cmd.Parameters.Add(name, SqlDbType.Decimal);
            parameter.Precision = 9;
            parameter.Scale = 2;
            parameter.Value = value;
        }

        private static bool ColumnExists(SqlConnection conn, SqlTransaction tx, string tableName, string columnName)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(@TableName) AND name = @ColumnName",
                conn, tx))
            {
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 128).Value = tableName;
                cmd.Parameters.Add("@ColumnName", SqlDbType.NVarChar, 128).Value = columnName;
                return cmd.ExecuteScalar() != null;
            }
        }

        private sealed class CartItem
        {
            public int StockID { get; set; }
            public string ProductName { get; set; }
            public string Category { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal Subtotal { get { return UnitPrice * Quantity; } }
        }
    }
}
