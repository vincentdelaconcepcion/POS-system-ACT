using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
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
        private List<Panel> orderedProductCards;
        private Dictionary<Panel, Point> cardLocations;
        private Dictionary<Panel, Button> cardCategories;
        private List<Button> categoryHeaders;
        private Dictionary<Button, int> categoryOriginalY;
        private Label lblProductTitle;
        private Label lblNoProducts;
        private Button btnAddProduct;
        private Label lblPaymentMethod;
        private ComboBox cboPaymentMethod;

        private static readonly Color InStockColor = Color.FromArgb(46, 125, 50);
        private static readonly Color OutOfStockColor = Color.FromArgb(198, 40, 40);
        private static readonly Color CategoryColor = Color.FromArgb(193, 154, 107);
        private static readonly Font SearchLabelFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font ProductNameFont = new Font("Segoe UI", 10.5F);
        private static readonly Font ProductPriceFont = new Font("Segoe UI", 9.75F);
        private static readonly Font StockLabelFont = new Font("Segoe UI", 8.25F, FontStyle.Bold);
        private static readonly Font BuyButtonFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private static readonly Font BuyButtonCompactFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);

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
            Resize += (s, e) =>
            {
                if (WindowState != FormWindowState.Minimized)
                    LayoutStockDisplay();
            };
            btnAddProduct = new Button
            {
                Text = "+ Add Product",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = CategoryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnAddProduct.FlatAppearance.BorderSize = 0;
            btnAddProduct.Click += (s, e) =>
            {
                StockForm stockForm = new StockForm();
                stockForm.Show();
                Hide();
            };
            Controls.Add(btnAddProduct);
            btnAddProduct.BringToFront();

            lblPaymentMethod = new Label
            {
                Text = "Payment Method",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            cboPaymentMethod = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(35, 52, 70),
                FlatStyle = FlatStyle.Flat
            };
            panel42.Controls.Add(lblPaymentMethod);
            panel42.Controls.Add(cboPaymentMethod);
            lblPaymentMethod.BringToFront();
            cboPaymentMethod.BringToFront();

            ApplyRoleAccess();
            LoadPaymentMethods();
            Session.AddLogoutButton(this, panel2);
            btnAdminPanel.Click += (s, e) =>
            {
                var adminPanel = new AdminPanel { Owner = this };
                adminPanel.Show();
                Hide();
            };

            txtsearch.Multiline = false;
            txtsearch.Font = new Font("Segoe UI", 11F);
            txtsearch.BorderStyle = BorderStyle.FixedSingle;

            lblProductTitle = new Label
            {
                Text = "Product Catalog",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 52, 70),
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lblNoProducts = new Label
            {
                Text = "No products match your search.",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 110, 120),
                BackColor = Color.Transparent,
                AutoSize = true,
                Visible = false
            };

            Controls.Add(lblProductTitle);
            lblProductTitle.BringToFront();
            panel1.Controls.Add(lblNoProducts);
            lblNoProducts.BringToFront();

            InitializeSearch();
            LayoutStockDisplay();
            LoadStockStatus();
        }

        private void ApplyRoleAccess()
        {
            Session.HideUnless(btnHome, Session.CanSeeHome);
            Session.HideUnless(button6, Session.CanManageStock);
            Session.HideUnless(btnAdminPanel, Session.CanOpenSettings);
            Session.HideUnless(btnAddProduct, Session.CanManageStock);
        }

        private void LoadPaymentMethods()
        {
            try
            {
                cboPaymentMethod.Items.Clear();

                using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT Name FROM dbo.PaymentMethods WHERE IsActive = 1 ORDER BY PaymentMethodID",
                    conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                cboPaymentMethod.Items.Add(reader.GetString(0));
                        }
                    }
                }

                cboPaymentMethod.SelectedIndex = cboPaymentMethod.Items.Count > 0 ? 0 : -1;
                cboPaymentMethod.Enabled = cboPaymentMethod.Items.Count > 0;

                if (cboPaymentMethod.Items.Count == 0)
                {
                    MessageBox.Show(
                        "No active payment methods are configured.",
                        "Payment Methods",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                cboPaymentMethod.Items.Clear();
                cboPaymentMethod.Enabled = false;
                MessageBox.Show(
                    "Unable to load payment methods:\n\n" + ex.Message,
                    "Payment Methods",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadStockStatus()
        {
            try
            {
                List<sp_SearchResult> stockItems = db.sp_Search(null).ToList();

                AddMissingProductCards(stockItems);

                var quantities = stockItems
                    .GroupBy(s => (s.ProductName ?? string.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.Sum(s => s.Quantity), StringComparer.OrdinalIgnoreCase);

                var products = stockItems
                    .GroupBy(s => (s.ProductName ?? string.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Key.Length > 0)
                    .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

                foreach (Panel card in productCards)
                {
                    Button buyButton = card.Controls.OfType<Button>().FirstOrDefault();
                    string productName = GetProductName(card);

                    if (buyButton == null || string.IsNullOrEmpty(productName))
                        continue;

                    sp_SearchResult product;
                    if (products.TryGetValue(productName, out product))
                        ApplyProductPrice(card, product.UnitPrice);

                    int quantity;
                    quantities.TryGetValue(productName, out quantity);

                    ApplyStockStatus(buyButton, quantity > 0);
                    ApplyStockLabel(card, quantity);
                }

                LayoutProductGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load stock availability:\n\n" + ex.Message,
                    "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMissingProductCards(List<sp_SearchResult> stockItems)
        {
            if (stockItems == null || productCards == null || cardCategories == null)
                return;

            var existingNames = new HashSet<string>(
                productCards.Select(GetProductName).Where(n => !string.IsNullOrWhiteSpace(n)),
                StringComparer.OrdinalIgnoreCase);

            foreach (var product in stockItems
                .Where(s => !string.IsNullOrWhiteSpace(s.ProductName))
                .GroupBy(s => s.ProductName.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First()))
            {
                string productName = product.ProductName.Trim();
                if (!existingNames.Add(productName))
                    continue;

                Panel card = CreateProductCard(product);
                cardCategories[card] = EnsureCategoryHeader(product.Category);
                productCards.Add(card);
                orderedProductCards.Add(card);
                card.Visible = MatchesSearch(productName);
            }
        }

        private Panel CreateProductCard(sp_SearchResult product)
        {
            string productName = (product.ProductName ?? "Unnamed Product").Trim();

            var card = new Panel
            {
                Name = "panelProduct" + product.StockID,
                BackColor = Color.White,
                Size = new Size(139, 231),
                TabStop = false
            };

            var picture = new PictureBox
            {
                Name = "pictureBoxProduct" + product.StockID,
                BackColor = Color.FromArgb(247, 243, 237),
                Image = CreateProductImage(product.ImageData, productName),
                SizeMode = PictureBoxSizeMode.Zoom,
                TabStop = false
            };

            var nameLabel = new Label
            {
                Name = "lblProductName",
                Text = productName,
                Font = ProductNameFont,
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };

            var priceLabel = new Label
            {
                Name = "lblPrice",
                Text = FormatPrice(product.UnitPrice),
                Font = ProductPriceFont,
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };

            var stockLabel = new Label
            {
                Name = "lblStock",
                Font = StockLabelFont,
                ForeColor = OutOfStockColor,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            var buyButton = new Button
            {
                Name = "btnBuy",
                Text = "BUY",
                Font = BuyButtonFont,
                ForeColor = Color.White,
                BackColor = InStockColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            buyButton.FlatAppearance.BorderSize = 0;
            buyButton.Click += BuyButton_Click;

            card.Controls.Add(picture);
            card.Controls.Add(nameLabel);
            card.Controls.Add(priceLabel);
            card.Controls.Add(stockLabel);
            card.Controls.Add(buyButton);
            TagProductLabels(card);
            panel1.Controls.Add(card);

            return card;
        }

        private Button EnsureCategoryHeader(string category)
        {
            string headerText = string.IsNullOrWhiteSpace(category) ? "Other" : category.Trim();

            Button existing = categoryHeaders.FirstOrDefault(
                header => string.Equals(header.Text, headerText, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
                return existing;

            var newHeader = new Button
            {
                Name = "btnCategory" + categoryHeaders.Count,
                Text = headerText,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = CategoryColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 36),
                TabStop = false,
                UseVisualStyleBackColor = false,
                Visible = txtsearch.Text.Trim().Length == 0
            };
            newHeader.FlatAppearance.BorderSize = 0;

            panel1.Controls.Add(newHeader);
            categoryHeaders.Add(newHeader);
            categoryOriginalY[newHeader] = int.MaxValue;

            return newHeader;
        }

        private static Image CreateProductImage(byte[] imageData, string productName)
        {
            if (imageData == null || imageData.Length == 0)
                return CreatePlaceholderImage(productName);

            try
            {
                using (MemoryStream stream = new MemoryStream(imageData))
                using (Image source = Image.FromStream(stream))
                {
                    return new Bitmap(source);
                }
            }
            catch
            {
                return CreatePlaceholderImage(productName);
            }
        }

        private static Image CreatePlaceholderImage(string productName)
        {
            var image = new Bitmap(190, 132);

            using (Graphics graphics = Graphics.FromImage(image))
            using (var background = new SolidBrush(Color.FromArgb(247, 243, 237)))
            using (var border = new Pen(Color.FromArgb(218, 204, 185), 2))
            using (var textBrush = new SolidBrush(Color.FromArgb(168, 130, 84)))
            using (var font = new Font("Segoe UI", 42F, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.FillRectangle(background, 0, 0, image.Width, image.Height);
                graphics.DrawRectangle(border, 1, 1, image.Width - 3, image.Height - 3);
                graphics.DrawString(
                    GetInitials(productName),
                    font,
                    textBrush,
                    new RectangleF(0, 0, image.Width, image.Height),
                    format);
            }

            return image;
        }

        private static string GetInitials(string productName)
        {
            string[] parts = (productName ?? string.Empty)
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string initials = string.Concat(
                parts.Take(2).Where(part => part.Length > 0).Select(part => char.ToUpperInvariant(part[0])));

            return string.IsNullOrEmpty(initials) ? "?" : initials;
        }

        private static string FormatPrice(decimal price)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}{1:N2}", Peso, price);
        }

        private static void ApplyProductPrice(Panel card, decimal price)
        {
            Label priceLabel = GetPriceLabel(card);
            if (priceLabel != null)
                priceLabel.Text = FormatPrice(price);
        }

        private static void ApplyStockLabel(Panel card, int quantity)
        {
            Label stockLabel = EnsureStockLabel(card);
            if (stockLabel == null)
                return;

            stockLabel.Text = string.Format(CultureInfo.CurrentCulture, "{0} left", quantity);
            stockLabel.ForeColor = quantity > 0 ? InStockColor : OutOfStockColor;
        }

        private static Label EnsureStockLabel(Panel card)
        {
            Label stockLabel = GetStockLabel(card);
            if (stockLabel != null)
                return stockLabel;

            stockLabel = new Label
            {
                Name = "lblStock",
                AutoSize = false,
                Font = StockLabelFont,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };
            card.Controls.Add(stockLabel);

            return stockLabel;
        }

        private bool MatchesSearch(string productName)
        {
            string search = txtsearch.Text.Trim();
            return search.Length == 0
                || (productName ?? string.Empty).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
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
            foreach (Panel card in productCards)
                TagProductLabels(card);

            cardLocations = productCards.ToDictionary(c => c, c => c.Location);
            orderedProductCards = productCards
                .OrderBy(c => cardLocations[c].Y)
                .ThenBy(c => cardLocations[c].X)
                .ToList();

            categoryHeaders = new[] { button1, button16, button25, button36 }
                .Where(b => b != null)
                .OrderBy(b => b.Top)
                .ToList();
            categoryOriginalY = categoryHeaders.ToDictionary(b => b, b => b.Top);
            cardCategories = new Dictionary<Panel, Button>();

            foreach (Panel card in orderedProductCards)
            {
                int cardY = cardLocations[card].Y;
                Button category = categoryHeaders.LastOrDefault(
                    header => categoryOriginalY[header] <= cardY);

                if (category == null)
                    category = categoryHeaders.FirstOrDefault();

                cardCategories[card] = category;
            }

            SendMessage(txtsearch.Handle, EM_SETCUEBANNER, (IntPtr)1, "Search product...");
        }


        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtsearch.Text.Trim();
            bool filtering = search.Length > 0;

            foreach (Button header in categoryHeaders)
                header.Visible = !filtering;

            foreach (Panel card in productCards)
            {
                string productName = GetProductName(card);
                bool matches = !filtering
                    || productName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;

                card.Visible = matches;
            }

            LayoutProductGrid();
            scrollTarget = 0;
            smoothScrollTimer.Stop();
            panel1.AutoScrollPosition = new Point(0, 0);
        }

        private void LayoutStockDisplay()
        {
            if (panel1 == null || panel2 == null || panel42 == null ||
                txtsearch == null || label76 == null || lblProductTitle == null)
                return;

            const int sideWidth = 300;
            const int cartWidth = 360;
            const int margin = 24;
            const int panelTop = 72;

            int width = ClientSize.Width;
            int height = ClientSize.Height;

            panel2.Width = sideWidth;
            panel42.Width = cartWidth;

            int productLeft = panel2.Width + margin;
            int productRight = width - cartWidth - margin;
            int productWidth = Math.Max(300, productRight - productLeft);

            const int addWidth = 128;
            const int addGap = 10;
            bool showAddProduct = Session.CanManageStock && btnAddProduct != null && productWidth >= 680;

            if (btnAddProduct != null)
            {
                btnAddProduct.Visible = showAddProduct;
                btnAddProduct.Size = new Size(addWidth, 32);
                btnAddProduct.Location = new Point(productRight - addWidth, 18);
            }

            int searchRight = showAddProduct ? productRight - addWidth - addGap : productRight;
            int searchWidth = Math.Min(380, Math.Max(220, (searchRight - productLeft) / 3));
            int searchLeft = searchRight - searchWidth;

            lblProductTitle.Visible = productWidth >= (showAddProduct ? 820 : 560);
            lblProductTitle.Location = new Point(productLeft, 18);

            label76.AutoSize = false;
            label76.Font = SearchLabelFont;
            label76.ForeColor = Color.FromArgb(45, 62, 80);
            label76.BackColor = Color.Transparent;
            label76.TextAlign = ContentAlignment.MiddleLeft;
            label76.Size = new Size(76, 28);
            label76.Location = new Point(searchLeft - 84, 20);

            txtsearch.AutoSize = false;
            txtsearch.Location = new Point(searchLeft, 18);
            txtsearch.Size = new Size(searchWidth, 32);

            panel1.Anchor = AnchorStyles.None;
            panel1.Location = new Point(productLeft, panelTop);
            panel1.Size = new Size(
                productWidth,
                Math.Max(200, height - panelTop - margin));

            LayoutCartPanel();
            LayoutProductGrid();
        }

        private void LayoutCartPanel()
        {
            if (panel42 == null || lstCart == null || lblTotal == null ||
                btnCheckout == null || label77 == null ||
                lblPaymentMethod == null || cboPaymentMethod == null)
                return;

            const int margin = 18;
            const int listTop = 126;
            const int bottomArea = 100;

            label77.Location = new Point(24, 20);

            lblPaymentMethod.Location = new Point(margin, 58);
            cboPaymentMethod.Location = new Point(margin, 82);
            cboPaymentMethod.Size = new Size(
                Math.Max(180, panel42.ClientSize.Width - (margin * 2)),
                30);

            lstCart.Anchor = AnchorStyles.None;
            lstCart.Location = new Point(margin, listTop);
            lstCart.Size = new Size(
                Math.Max(180, panel42.ClientSize.Width - (margin * 2)),
                Math.Max(160, panel42.ClientSize.Height - listTop - bottomArea));

            int bottomY = panel42.ClientSize.Height - 58;
            lblTotal.Anchor = AnchorStyles.None;
            lblTotal.Location = new Point(margin, bottomY + 7);

            btnCheckout.Anchor = AnchorStyles.None;
            btnCheckout.Size = new Size(150, 44);
            btnCheckout.Location = new Point(
                Math.Max(margin, panel42.ClientSize.Width - margin - btnCheckout.Width),
                bottomY);
        }

        private void LayoutProductGrid()
        {
            if (panel1 == null || orderedProductCards == null ||
                categoryHeaders == null || cardCategories == null || txtsearch == null)
                return;

            const int margin = 18;
            const int gap = 12;
            const int cardHeight = 236;
            const int rowGap = 16;
            const int minimumCardWidth = 140;
            const int maximumColumns = 6;

            int scrollBarWidth = SystemInformation.VerticalScrollBarWidth;
            int available = Math.Max(
                120,
                panel1.ClientSize.Width - (margin * 2) - scrollBarWidth);
            int columns = Math.Max(
                2,
                Math.Min(maximumColumns, (available + gap) / (minimumCardWidth + gap)));
            int cardWidth = Math.Max(
                120,
                (available - ((columns - 1) * gap)) / columns);
            cardWidth = Math.Min(cardWidth, 220);

            int gridWidth = (columns * cardWidth) + ((columns - 1) * gap);
            int startX = margin + Math.Max(0, (available - gridWidth) / 2);
            int scrollY = -panel1.AutoScrollPosition.Y;
            int y = 10;
            bool filtering = txtsearch.Text.Trim().Length > 0;

            if (filtering)
            {
                List<Panel> matches = orderedProductCards
                    .Where(card => card.Visible)
                    .ToList();

                y = LayoutCards(matches, startX, y, columns, cardWidth, cardHeight, gap, rowGap);
                lblNoProducts.Location = new Point(startX, 12);
                lblNoProducts.Visible = matches.Count == 0;
            }
            else
            {
                lblNoProducts.Visible = false;

                foreach (Button header in categoryHeaders)
                {
                    header.Visible = true;
                    header.Location = new Point(startX, y);
                    header.Size = new Size(
                        Math.Min(240, Math.Max(180, cardWidth + 32)),
                        36);
                    y += 48;

                    List<Panel> cards = orderedProductCards
                        .Where(card => cardCategories[card] == header)
                        .ToList();

                    y = LayoutCards(cards, startX, y, columns, cardWidth, cardHeight, gap, rowGap);
                    y += 24;
                }
            }

            panel1.AutoScrollMinSize = new Size(0, Math.Max(0, y + 8));

            if (scrollY > 0)
            {
                int max = GetMaxScrollOffset();
                panel1.AutoScrollPosition = new Point(0, Math.Min(scrollY, max));
            }
        }

        private int LayoutCards(
            List<Panel> cards,
            int startX,
            int startY,
            int columns,
            int cardWidth,
            int cardHeight,
            int gap,
            int rowGap)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int column = i % columns;
                int row = i / columns;
                Panel card = cards[i];

                card.Location = new Point(
                    startX + (column * (cardWidth + gap)),
                    startY + (row * (cardHeight + rowGap)));
                LayoutProductCard(card, cardWidth, cardHeight);
            }

            int rows = (cards.Count + columns - 1) / columns;
            return startY + (rows == 0
                ? 0
                : (rows * cardHeight) + ((rows - 1) * rowGap));
        }

        private static void LayoutProductCard(Panel card, int width, int height)
        {
            card.Size = new Size(width, height);

            PictureBox picture = card.Controls.OfType<PictureBox>().FirstOrDefault();
            if (picture != null)
            {
                const int imageHeight = 132;
                int imageWidth = Math.Max(60, width - 24);
                picture.Location = new Point((width - imageWidth) / 2, 14);
                picture.Size = new Size(imageWidth, imageHeight);
                picture.SizeMode = PictureBoxSizeMode.Zoom;
            }

            Label name = GetNameLabel(card);
            if (name != null)
            {
                name.AutoSize = false;
                name.AutoEllipsis = true;
                name.Font = ProductNameFont;
                name.Location = new Point(10, 152);
                name.Size = new Size(Math.Max(20, width - 20), 22);
                name.TextAlign = ContentAlignment.MiddleLeft;
            }

            Label price = GetPriceLabel(card);
            if (price != null)
            {
                price.AutoSize = false;
                price.Font = ProductPriceFont;
                price.Location = new Point(10, 176);
                price.Size = new Size(Math.Max(20, width - 82), 20);
                price.TextAlign = ContentAlignment.MiddleLeft;
            }

            Label stock = GetStockLabel(card);
            if (stock != null)
            {
                stock.AutoSize = false;
                stock.Font = StockLabelFont;
                stock.Location = new Point(Math.Max(10, width - 68), 176);
                stock.Size = new Size(58, 20);
                stock.TextAlign = ContentAlignment.MiddleRight;
            }

            Button buyButton = card.Controls.OfType<Button>().FirstOrDefault();
            if (buyButton != null)
            {
                buyButton.AutoSize = false;
                buyButton.Font = width >= 160 ? BuyButtonFont : BuyButtonCompactFont;
                buyButton.Location = new Point(10, height - 33);
                buyButton.Size = new Size(Math.Max(20, width - 20), 28);
            }
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
            Label name = GetNameLabel(card);
            return name == null ? string.Empty : name.Text.Trim();
        }

        private static void TagProductLabels(Panel card)
        {
            List<Label> labels = card.Controls.OfType<Label>()
                .Where(label => label.Name != "lblStock")
                .OrderBy(label => label.Top)
                .ToList();

            if (labels.Count > 0 && labels[0].Tag == null)
                labels[0].Tag = "product-name";

            if (labels.Count > 1 && labels[1].Tag == null)
                labels[1].Tag = "product-price";
        }

        private static Label GetNameLabel(Panel card)
        {
            return card.Controls.OfType<Label>()
                .FirstOrDefault(label =>
                    label.Name == "lblProductName" ||
                    object.Equals(label.Tag, "product-name"));
        }

        private static Label GetPriceLabel(Panel card)
        {
            return card.Controls.OfType<Label>()
                .FirstOrDefault(label =>
                    label.Name == "lblPrice" ||
                    object.Equals(label.Tag, "product-price"));
        }

        private static Label GetStockLabel(Panel card)
        {
            return card.Controls.OfType<Label>()
                .FirstOrDefault(label => label.Name == "lblStock");
        }

        private void StockDisplay_Load(object sender, EventArgs e)
        {
            LayoutStockDisplay();
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
            string paymentMethod = Convert.ToString(cboPaymentMethod.SelectedItem);
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                MessageBox.Show("Select a payment method before checking out.", "Payment Method Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            paymentMethod = paymentMethod.Trim();

            DialogResult confirm = MessageBox.Show(
                string.Format(CultureInfo.CurrentCulture,
                    "Complete this sale?\n\nItems: {0}\nTotal: {1}{2:N2}\nPayment: {3}",
                    itemCount, Peso, totalAmount, paymentMethod),
                "Confirm Checkout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            btnCheckout.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                int orderId = SaveOrder(paymentMethod);

                MessageBox.Show(
                    string.Format(CultureInfo.CurrentCulture,
                        "Sale completed successfully.\n\nOrder #: {0}\nTotal: {1}{2:N2}\nPayment: {3}",
                        orderId, Peso, totalAmount, paymentMethod),
                    "Checkout Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cart.Clear();
                if (cboPaymentMethod.Items.Count > 0)
                    cboPaymentMethod.SelectedIndex = 0;
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

        private int SaveOrder(string paymentMethod)
        {
            decimal totalAmount = cart.Sum(i => i.Subtotal);
            paymentMethod = (paymentMethod ?? string.Empty).Trim();

            using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
            {
                conn.Open();

                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        bool hasCashier = ColumnExists(conn, tx, "dbo.Orders", "Cashier");
                        bool hasPaymentMethod = ColumnExists(conn, tx, "dbo.Orders", "PaymentMethod");

                        var columns = new List<string> { "OrderDate", "TotalAmount" };
                        var values = new List<string> { "GETDATE()", "@TotalAmount" };

                        if (hasCashier)
                        {
                            columns.Add("Cashier");
                            values.Add("@Cashier");
                        }

                        if (hasPaymentMethod)
                        {
                            columns.Add("PaymentMethod");
                            values.Add("@PaymentMethod");
                        }

                        string insertOrder =
                            "INSERT INTO dbo.Orders (" + string.Join(", ", columns) + ") " +
                            "VALUES (" + string.Join(", ", values) + "); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS INT);";

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

                            if (hasPaymentMethod)
                            {
                                cmd.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 20).Value = paymentMethod;
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
