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
    public partial class StockDisplay : Form, IMessageFilter
    {
        private decimal total = 0;
        private readonly SP_StockDataContext db = new SP_StockDataContext();
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

        }
    }
}
