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
    public partial class AdminPanel : Form
    {
        private static readonly string[] RoleNames = { "Cashier", "Manager", "Admin" };
        private static readonly Color CardColor = Color.FromArgb(245, 247, 250);
        private static readonly Color AccentColor = Color.FromArgb(193, 154, 107);
        private static readonly Color DarkButtonColor = Color.FromArgb(52, 73, 94);
        private static readonly Color DangerColor = Color.FromArgb(198, 40, 40);

        private Panel panelContent;
        private Panel panelHeader;
        private Panel panelAddUser;
        private Panel panelQuickActions;
        private Panel panelInfo;
        private Panel panelUsers;
        private Panel[] statCards;

        private Label lblHeader;
        private Label lblSubheader;
        private Label lblCurrentUser;
        private Label lblStatus;
        private Label lblRole;
        private Label lblQuickTitle;
        private Label lblUsersTitle;
        private Label lblSelection;
        private Label lblInfoTitle;
        private Label lblInfoBody;
        private Label lblTotalUsers;
        private Label lblAdmins;
        private Label lblManagers;
        private Label lblCashiers;

        private ComboBox cboRole;
        private ComboBox cboRoleChange;
        private Button btnAddUser;
        private Button btnUpdateRole;
        private Button btnResetPassword;
        private Button btnDeleteUser;
        private Button btnRefresh;
        private Button btnQuickStock;
        private Button btnQuickPos;
        private Button btnQuickHome;
        private Button btnQuickRefresh;
        private CheckBox chkShowPassword;
        private DataGridView dgvUsers;
        private FlowLayoutPanel flowUserActions;

        public AdminPanel()
        {
            InitializeComponent();

            Text = "Admin Panel - POS System";
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            BackColor = Color.DarkSlateGray;
            Resize += AdminPanel_Resize;

            if (!Session.CanOpenSettings)
            {
                Shown += (s, e) =>
                {
                    new StockDisplay().Show();
                    Close();
                };
                return;
            }

            BuildContent();
            WireNavigation();
            Session.AddLogoutButton(this, panel2);
            LayoutAdminPanel();
            Shown += (s, e) => ReloadUsers();
            FormClosed += (s, e) =>
            {
                if (!Session.LogoutRequested &&
                    Owner != null &&
                    !Owner.IsDisposed &&
                    !Owner.Visible)
                {
                    Owner.Show();
                }
            };
        }

        private void BuildContent()
        {
            panelContent = new Panel
            {
                BackColor = Color.DarkSlateGray,
                Location = new Point(panel2.Width, 0),
                Size = new Size(Math.Max(320, ClientSize.Width - panel2.Width), ClientSize.Height)
            };
            Controls.Add(panelContent);
            panel2.BringToFront();

            BuildHeader();
            BuildStats();
            BuildAddUserPanel();
            BuildQuickActionsPanel();
            BuildInfoPanel();
            BuildUsersPanel();
        }

        private void BuildHeader()
        {
            panelHeader = new Panel { BackColor = Color.SteelBlue };

            lblHeader = new Label
            {
                Text = "ADMIN PANEL",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(22, 14)
            };

            lblSubheader = new Label
            {
                Text = "Manage user accounts, roles, and system access",
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(220, 235, 245),
                AutoSize = true,
                Location = new Point(24, 58)
            };

            lblCurrentUser = new Label
            {
                Text = "Signed in as " + Session.Username + " (" + Session.Role + ")",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Size = new Size(430, 24)
            };

            lblStatus = new Label
            {
                Text = "Ready",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(220, 235, 245),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Size = new Size(430, 22)
            };

            panelHeader.Controls.Add(lblHeader);
            panelHeader.Controls.Add(lblSubheader);
            panelHeader.Controls.Add(lblCurrentUser);
            panelHeader.Controls.Add(lblStatus);
            panelContent.Controls.Add(panelHeader);
        }

        private void BuildStats()
        {
            Panel totalCard = CreateStatCard("Total Users", out lblTotalUsers);
            Panel adminCard = CreateStatCard("Administrators", out lblAdmins);
            Panel managerCard = CreateStatCard("Managers", out lblManagers);
            Panel cashierCard = CreateStatCard("Cashiers", out lblCashiers);

            statCards = new[] { totalCard, adminCard, managerCard, cashierCard };

            foreach (Panel card in statCards)
                panelContent.Controls.Add(card);
        }

        private static Panel CreateStatCard(string title, out Label valueLabel)
        {
            Panel card = new Panel { BackColor = Color.LightSteelBlue };

            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(45, 62, 80),
                AutoSize = true,
                Location = new Point(18, 14)
            };

            valueLabel = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 42, 60),
                AutoSize = true,
                Location = new Point(18, 42)
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            return card;
        }

        private void BuildAddUserPanel()
        {
            panelAddUser = new Panel { BackColor = CardColor };

            label1.Text = "Add New User";
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(35, 52, 70);
            label1.AutoSize = true;

            label2.Text = "Username";
            label2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(70, 85, 100);
            label2.AutoSize = true;

            txtUsername.Font = new Font("Segoe UI", 10.5F);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;

            label3.Text = "Password";
            label3.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(70, 85, 100);
            label3.AutoSize = true;

            txtPassword.Font = new Font("Segoe UI", 10.5F);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.UseSystemPasswordChar = true;

            lblRole = new Label
            {
                Text = "Role",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 85, 100),
                AutoSize = true
            };

            cboRole = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboRole.Items.AddRange(RoleNames);
            cboRole.SelectedIndex = 0;

            btnAddUser = CreateButton("Add User", AccentColor, Color.White, 30);
            btnAddUser.Click += (s, e) => AddUser();

            chkShowPassword = new CheckBox
            {
                Text = "Show password",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(70, 85, 100),
                AutoSize = true
            };
            chkShowPassword.CheckedChanged += (s, e) => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    AddUser();
                }
            };

            panelAddUser.Controls.Add(label1);
            panelAddUser.Controls.Add(label2);
            panelAddUser.Controls.Add(txtUsername);
            panelAddUser.Controls.Add(label3);
            panelAddUser.Controls.Add(txtPassword);
            panelAddUser.Controls.Add(lblRole);
            panelAddUser.Controls.Add(cboRole);
            panelAddUser.Controls.Add(btnAddUser);
            panelAddUser.Controls.Add(chkShowPassword);

            panelContent.Controls.Add(panelAddUser);
        }

        private void BuildQuickActionsPanel()
        {
            panelQuickActions = new Panel { BackColor = CardColor };

            lblQuickTitle = new Label
            {
                Text = "Quick Actions",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 52, 70),
                AutoSize = true
            };

            btnQuickStock = CreateButton("Manage Stock", DarkButtonColor, Color.White, 40);
            btnQuickPos = CreateButton("Open POS", DarkButtonColor, Color.White, 40);
            btnQuickHome = CreateButton("Open Dashboard", DarkButtonColor, Color.White, 40);
            btnQuickRefresh = CreateButton("Refresh Users", AccentColor, Color.White, 40);

            btnQuickStock.Click += (s, e) => OpenForm(new StockForm());
            btnQuickPos.Click += (s, e) => OpenForm(new StockDisplay());
            btnQuickHome.Click += (s, e) => OpenForm(new Form1());
            btnQuickRefresh.Click += (s, e) => ReloadUsers();

            panelQuickActions.Controls.Add(lblQuickTitle);
            panelQuickActions.Controls.Add(btnQuickStock);
            panelQuickActions.Controls.Add(btnQuickPos);
            panelQuickActions.Controls.Add(btnQuickHome);
            panelQuickActions.Controls.Add(btnQuickRefresh);

            panelContent.Controls.Add(panelQuickActions);
        }

        private void BuildInfoPanel()
        {
            panelInfo = new Panel { BackColor = CardColor };

            lblInfoTitle = new Label
            {
                Text = "Access Rules",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 52, 70),
                AutoSize = true
            };

            lblInfoBody = new Label
            {
                Text = "\u2022 Only Administrators can open and use this panel.\n" +
                       "\u2022 You cannot change or delete the account you are signed in with.\n" +
                       "\u2022 The last remaining Administrator cannot be demoted or deleted.\n" +
                       "\u2022 Passwords are validated and stored as SHA-256 hashes.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(70, 85, 100),
                AutoSize = false
            };

            panelInfo.Controls.Add(lblInfoTitle);
            panelInfo.Controls.Add(lblInfoBody);
            panelContent.Controls.Add(panelInfo);
        }

        private void BuildUsersPanel()
        {
            panelUsers = new Panel { BackColor = CardColor };

            lblUsersTitle = new Label
            {
                Text = "User Accounts",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 52, 70),
                AutoSize = true
            };

            btnRefresh = CreateButton("Refresh", DarkButtonColor, Color.White, 30);
            btnRefresh.Click += (s, e) => ReloadUsers();

            dgvUsers = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersHeight = 38,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(220, 225, 230),
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                RowTemplate = { Height = 34 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = DarkButtonColor;
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvUsers.DefaultCellStyle.ForeColor = Color.FromArgb(40, 55, 70);
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colUsername",
                DataPropertyName = "Username",
                HeaderText = "Username",
                FillWeight = 60
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRole",
                DataPropertyName = "Role",
                HeaderText = "Role",
                FillWeight = 40
            });

            dgvUsers.SelectionChanged += (s, e) => UpdateActionState();

            lblSelection = new Label
            {
                Text = "Select a user to manage",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 85, 100),
                AutoSize = false,
                Height = 20
            };

            cboRoleChange = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Width = 120,
                Height = 30,
                Margin = new Padding(0, 0, 10, 6)
            };
            cboRoleChange.Items.AddRange(RoleNames);

            btnUpdateRole = CreateButton("Change Role", AccentColor, Color.White, 30);
            btnUpdateRole.Width = 120;
            btnUpdateRole.Margin = new Padding(0, 0, 10, 6);
            btnUpdateRole.Click += (s, e) => UpdateSelectedUserRole();

            btnResetPassword = CreateButton("Reset Password", DarkButtonColor, Color.White, 30);
            btnResetPassword.Width = 140;
            btnResetPassword.Margin = new Padding(0, 0, 10, 6);
            btnResetPassword.Click += (s, e) => ResetSelectedUserPassword();

            btnDeleteUser = CreateButton("Delete User", DangerColor, Color.White, 30);
            btnDeleteUser.Width = 110;
            btnDeleteUser.Margin = new Padding(0, 0, 10, 6);
            btnDeleteUser.Click += (s, e) => DeleteSelectedUser();

            flowUserActions = new FlowLayoutPanel
            {
                BackColor = CardColor,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = false
            };
            flowUserActions.Controls.Add(cboRoleChange);
            flowUserActions.Controls.Add(btnUpdateRole);
            flowUserActions.Controls.Add(btnResetPassword);
            flowUserActions.Controls.Add(btnDeleteUser);

            panelUsers.Controls.Add(lblUsersTitle);
            panelUsers.Controls.Add(btnRefresh);
            panelUsers.Controls.Add(dgvUsers);
            panelUsers.Controls.Add(lblSelection);
            panelUsers.Controls.Add(flowUserActions);

            panelContent.Controls.Add(panelUsers);
            UpdateActionState();
        }

        private void WireNavigation()
        {
            btnAdminPanel.BackColor = Color.FromArgb(160, 120, 72);

            button1.Click += (s, e) => OpenForm(new Form1());
            btnStockDisplay.Click += (s, e) => OpenForm(new StockDisplay());
            button6.Click += (s, e) => OpenForm(new StockForm());
            btnAdminPanel.Click += (s, e) =>
            {
                ReloadUsers();
                txtUsername.Focus();
            };
        }

        private void OpenForm(Form form)
        {
            form.Show();
            Hide();
        }

        private void AdminPanel_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                LayoutAdminPanel();
        }

        private void LayoutAdminPanel()
        {
            if (panelContent == null || panel2 == null || panelHeader == null)
                return;

            panel2.Width = 300;
            panelContent.Location = new Point(panel2.Width, 0);
            panelContent.Size = new Size(
                Math.Max(320, ClientSize.Width - panel2.Width),
                Math.Max(300, ClientSize.Height));

            LayoutSidebar();

            int width = panelContent.ClientSize.Width;
            int height = panelContent.ClientSize.Height;
            const int margin = 24;
            const int gap = 18;
            const int headerHeight = 96;
            const int statHeight = 96;

            panelHeader.Location = new Point(margin, margin);
            panelHeader.Size = new Size(Math.Max(320, width - (margin * 2)), headerHeight);

            lblCurrentUser.Location = new Point(
                Math.Max(250, panelHeader.Width - lblCurrentUser.Width - 24), 18);
            lblStatus.Location = new Point(
                Math.Max(250, panelHeader.Width - lblStatus.Width - 24), 52);

            int statsTop = panelHeader.Bottom + gap;
            int usableWidth = panelHeader.Width;
            const int statGap = 14;
            int statWidth = Math.Max(150, (usableWidth - (statGap * 3)) / 4);

            for (int i = 0; i < statCards.Length; i++)
            {
                statCards[i].Location = new Point(
                    margin + i * (statWidth + statGap), statsTop);
                statCards[i].Size = new Size(statWidth, statHeight);
            }

            int cardsTop = statsTop + statHeight + gap;
            int leftWidth = Math.Max(320, Math.Min(380, usableWidth / 4));

            panelAddUser.Location = new Point(margin, cardsTop);
            panelAddUser.Size = new Size(leftWidth, 280);

            panelQuickActions.Location = new Point(margin, panelAddUser.Bottom + gap);
            int quickHeight = Math.Min(
                190,
                Math.Max(154, height - panelAddUser.Bottom - gap - margin));
            panelQuickActions.Size = new Size(leftWidth, quickHeight);

            int infoTop = panelQuickActions.Bottom + gap;
            int infoHeight = height - infoTop - margin;
            bool showInfo = infoHeight >= 96;
            panelInfo.Visible = showInfo;
            if (showInfo)
            {
                panelInfo.Location = new Point(margin, infoTop);
                panelInfo.Size = new Size(leftWidth, infoHeight);
            }

            int usersLeft = margin + leftWidth + gap;
            panelUsers.Location = new Point(usersLeft, cardsTop);
            panelUsers.Size = new Size(
                Math.Max(400, width - usersLeft - margin),
                Math.Max(260, height - cardsTop - margin));

            LayoutAddUserPanel();
            LayoutQuickActionsPanel();
            LayoutInfoPanel();
            LayoutUsersPanel();
        }

        private void LayoutSidebar()
        {
            if (panel3 == null)
                return;

            Control logout = panel2.Controls["btnLogout"];
            if (logout != null)
            {
                logout.Left = 26;
                logout.Top = Math.Max(panel3.Bottom + 80, panel2.ClientSize.Height - logout.Height - 20);
            }

            Button[] navButtons = { button1, btnStockDisplay, button6, btnAdminPanel };
            var visibleButtons = navButtons.Where(b => b != null && b.Visible).ToArray();
            if (visibleButtons.Length == 0)
                return;

            const int buttonHeight = 65;
            const int buttonGap = 24;
            int groupHeight = (visibleButtons.Length * buttonHeight) + ((visibleButtons.Length - 1) * buttonGap);
            int topLimit = panel3.Bottom;
            int bottomLimit = logout != null ? logout.Top - 24 : panel2.ClientSize.Height - 24;
            int startY = topLimit + Math.Max(20, (bottomLimit - topLimit - groupHeight) / 2);

            for (int i = 0; i < visibleButtons.Length; i++)
            {
                visibleButtons[i].Location = new Point(26, startY + i * (buttonHeight + buttonGap));
                visibleButtons[i].Size = new Size(248, buttonHeight);
            }
        }

        private void LayoutAddUserPanel()
        {
            int innerWidth = panelAddUser.ClientSize.Width - 36;

            label1.Location = new Point(18, 14);
            label2.Location = new Point(18, 54);
            txtUsername.Location = new Point(18, 76);
            txtUsername.Width = innerWidth;
            label3.Location = new Point(18, 116);
            txtPassword.Location = new Point(18, 138);
            txtPassword.Width = innerWidth;
            lblRole.Location = new Point(18, 178);

            cboRole.Location = new Point(18, 200);
            cboRole.Width = 120;

            btnAddUser.Location = new Point(148, 200);
            btnAddUser.Size = new Size(Math.Max(120, innerWidth - 130), 30);

            chkShowPassword.Location = new Point(18, 244);
        }

        private void LayoutQuickActionsPanel()
        {
            int innerWidth = panelQuickActions.ClientSize.Width - 36;
            int buttonWidth = Math.Max(120, (innerWidth - 10) / 2);
            const int buttonHeight = 40;

            lblQuickTitle.Location = new Point(18, 14);

            btnQuickStock.Location = new Point(18, 50);
            btnQuickStock.Size = new Size(buttonWidth, buttonHeight);

            btnQuickPos.Location = new Point(18 + buttonWidth + 10, 50);
            btnQuickPos.Size = new Size(buttonWidth, buttonHeight);

            btnQuickHome.Location = new Point(18, 100);
            btnQuickHome.Size = new Size(buttonWidth, buttonHeight);

            btnQuickRefresh.Location = new Point(18 + buttonWidth + 10, 100);
            btnQuickRefresh.Size = new Size(buttonWidth, buttonHeight);
        }

        private void LayoutInfoPanel()
        {
            if (panelInfo == null)
                return;

            int innerWidth = panelInfo.ClientSize.Width - 36;
            lblInfoTitle.Location = new Point(18, 14);
            lblInfoBody.Location = new Point(18, 48);
            lblInfoBody.Size = new Size(
                innerWidth,
                Math.Max(40, panelInfo.ClientSize.Height - 62));
        }

        private void LayoutUsersPanel()
        {
            int width = panelUsers.ClientSize.Width;
            int height = panelUsers.ClientSize.Height;

            lblUsersTitle.Location = new Point(18, 14);
            btnRefresh.Location = new Point(Math.Max(18, width - btnRefresh.Width - 18), 14);

            dgvUsers.Location = new Point(18, 100);
            dgvUsers.Size = new Size(Math.Max(200, width - 36), Math.Max(100, height - 228));

            lblSelection.Location = new Point(18, Math.Max(100, height - 118));
            lblSelection.Width = Math.Max(200, width - 36);

            flowUserActions.Location = new Point(18, Math.Max(120, height - 90));
            flowUserActions.Size = new Size(Math.Max(200, width - 36), 76);
        }

        private void ReloadUsers()
        {
            if (dgvUsers == null)
                return;

            try
            {
                List<UserAccount> users = AuthService.GetUsers();

                dgvUsers.DataSource = null;
                dgvUsers.DataSource = users;
                dgvUsers.ClearSelection();

                lblUsersTitle.Text = "User Accounts (" + users.Count + ")";
                lblTotalUsers.Text = users.Count.ToString();
                lblAdmins.Text = users.Count(u => string.Equals(u.Role, "Admin", StringComparison.OrdinalIgnoreCase)).ToString();
                lblManagers.Text = users.Count(u => string.Equals(u.Role, "Manager", StringComparison.OrdinalIgnoreCase)).ToString();
                lblCashiers.Text = users.Count(u => string.Equals(u.Role, "Cashier", StringComparison.OrdinalIgnoreCase)).ToString();

                SetStatus(users.Count + " account(s) loaded at " + DateTime.Now.ToString("h:mm tt"));
                UpdateActionState();
            }
            catch (Exception ex)
            {
                ShowError("Unable to load user accounts:\n\n" + ex.Message, "Admin Panel");
            }
        }

        private void AddUser()
        {
            try
            {
                string username = txtUsername.Text.Trim();
                UserRole role = GetSelectedRole(cboRole);

                AuthService.CreateUser(username, txtPassword.Text, role);

                txtUsername.Clear();
                txtPassword.Clear();
                cboRole.SelectedIndex = 0;
                chkShowPassword.Checked = false;

                ReloadUsers();
                SelectUser(username);
                SetStatus("User \"" + username + "\" was added.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Add User");
            }
        }

        private void UpdateSelectedUserRole()
        {
            UserAccount user = GetSelectedUser();
            if (user == null)
                return;

            try
            {
                UserRole role = GetSelectedRole(cboRoleChange);
                if (string.Equals(user.Role, role.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    SetStatus("That user already has the " + role + " role.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    this,
                    "Change " + user.Username + "'s role from " + user.Role + " to " + role + "?",
                    "Change Role",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                AuthService.UpdateUserRole(user.Username, role);
                ReloadUsers();
                SelectUser(user.Username);
                SetStatus(user.Username + " is now a " + role + ".");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Change Role");
            }
        }

        private void ResetSelectedUserPassword()
        {
            UserAccount user = GetSelectedUser();
            if (user == null)
                return;

            using (var prompt = new PasswordResetDialog(user.Username))
            {
                if (prompt.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    AuthService.ResetPassword(user.Username, prompt.NewPassword);
                    SetStatus("Password updated for " + user.Username + ".");

                    MessageBox.Show(
                        this,
                        "Password updated for " + user.Username + ".",
                        "Reset Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message, "Reset Password");
                }
            }
        }

        private void DeleteSelectedUser()
        {
            UserAccount user = GetSelectedUser();
            if (user == null)
                return;

            DialogResult confirm = MessageBox.Show(
                this,
                "Delete the account \"" + user.Username + "\"? This cannot be undone.",
                "Delete User",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                AuthService.DeleteUser(user.Username);
                ReloadUsers();
                SetStatus("User \"" + user.Username + "\" was deleted.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Delete User");
            }
        }

        private void UpdateActionState()
        {
            UserAccount user = GetSelectedUser();
            bool hasUser = user != null;
            bool canEdit = hasUser && !IsCurrentUser(user.Username);

            cboRoleChange.Enabled = canEdit;
            btnUpdateRole.Enabled = canEdit;
            btnResetPassword.Enabled = hasUser;
            btnDeleteUser.Enabled = canEdit;

            btnUpdateRole.BackColor = canEdit ? AccentColor : Color.FromArgb(180, 180, 180);
            btnResetPassword.BackColor = hasUser ? DarkButtonColor : Color.FromArgb(180, 180, 180);
            btnDeleteUser.BackColor = canEdit ? DangerColor : Color.FromArgb(180, 180, 180);

            if (hasUser)
            {
                cboRoleChange.SelectedItem = user.Role;
                lblSelection.Text = "Selected: " + user.Username + " (" + user.Role + ")" +
                    (canEdit ? string.Empty : " - current account");
            }
            else
            {
                cboRoleChange.SelectedIndex = -1;
                lblSelection.Text = "Select a user to manage";
            }
        }

        private UserAccount GetSelectedUser()
        {
            if (dgvUsers == null || dgvUsers.CurrentRow == null)
                return null;

            return dgvUsers.CurrentRow.DataBoundItem as UserAccount;
        }

        private void SelectUser(string username)
        {
            if (string.IsNullOrEmpty(username))
                return;

            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                UserAccount user = row.DataBoundItem as UserAccount;
                if (user != null && string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    dgvUsers.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private static bool IsCurrentUser(string username)
        {
            return string.Equals(username, Session.Username, StringComparison.OrdinalIgnoreCase);
        }

        private static UserRole GetSelectedRole(ComboBox combo)
        {
            UserRole role;
            if (combo.SelectedItem == null ||
                !Enum.TryParse(Convert.ToString(combo.SelectedItem), true, out role) ||
                !Enum.IsDefined(typeof(UserRole), role))
            {
                throw new InvalidOperationException("Select a valid user role.");
            }

            return role;
        }

        private static Button CreateButton(string text, Color backColor, Color foreColor, int height)
        {
            var button = new Button
            {
                Text = text,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Height = height,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void SetStatus(string message)
        {
            if (lblStatus != null)
                lblStatus.Text = message;
        }

        private void ShowError(string message, string title)
        {
            SetStatus(message);
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private sealed class PasswordResetDialog : Form
        {
            private readonly TextBox txtPassword;
            private readonly TextBox txtConfirm;

            public string NewPassword
            {
                get { return txtPassword.Text; }
            }

            public PasswordResetDialog(string username)
            {
                Text = "Reset Password";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                ClientSize = new Size(410, 250);
                BackColor = Color.White;
                Font = new Font("Segoe UI", 9.5F);

                var title = new Label
                {
                    Text = "Reset password",
                    Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(35, 52, 70),
                    AutoSize = true,
                    Location = new Point(20, 18)
                };

                var subtitle = new Label
                {
                    Text = "New password for " + username,
                    ForeColor = Color.FromArgb(70, 85, 100),
                    AutoSize = true,
                    Location = new Point(22, 54)
                };

                var lblPassword = new Label
                {
                    Text = "New password",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(70, 85, 100),
                    AutoSize = true,
                    Location = new Point(20, 88)
                };

                txtPassword = new TextBox
                {
                    UseSystemPasswordChar = true,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, 110),
                    Width = 370
                };

                var lblConfirm = new Label
                {
                    Text = "Confirm password",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(70, 85, 100),
                    AutoSize = true,
                    Location = new Point(20, 144)
                };

                txtConfirm = new TextBox
                {
                    UseSystemPasswordChar = true,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, 166),
                    Width = 370
                };

                var btnOk = CreateButton("Update", Color.FromArgb(193, 154, 107), Color.White, 34);
                btnOk.Location = new Point(210, 206);
                btnOk.Size = new Size(85, 34);
                btnOk.Click += (s, e) => Confirm();

                var btnCancel = CreateButton("Cancel", Color.FromArgb(108, 122, 137), Color.White, 34);
                btnCancel.Location = new Point(305, 206);
                btnCancel.Size = new Size(85, 34);
                btnCancel.DialogResult = DialogResult.Cancel;

                AcceptButton = btnOk;
                CancelButton = btnCancel;

                Controls.Add(title);
                Controls.Add(subtitle);
                Controls.Add(lblPassword);
                Controls.Add(txtPassword);
                Controls.Add(lblConfirm);
                Controls.Add(txtConfirm);
                Controls.Add(btnOk);
                Controls.Add(btnCancel);
            }

            private void Confirm()
            {
                if (txtPassword.Text.Length < 4)
                {
                    MessageBox.Show(this, "Password must be at least 4 characters long.",
                        "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                if (!string.Equals(txtPassword.Text, txtConfirm.Text, StringComparison.Ordinal))
                {
                    MessageBox.Show(this, "The passwords do not match.",
                        "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirm.Focus();
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
