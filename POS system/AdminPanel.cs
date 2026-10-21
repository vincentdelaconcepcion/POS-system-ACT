using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_system
{
    public partial class AdminPanel : Form
    {
        private static readonly Color AccentColor = Color.FromArgb(193, 154, 107);
        private static readonly Color DarkButtonColor = Color.FromArgb(52, 73, 94);
        private static readonly Color DangerColor = Color.FromArgb(198, 40, 40);

        public AdminPanel()
        {
            InitializeComponent();

            Text = "Admin Panel - POS System";
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            lblCurrentUser.Text = "Signed in as " + Session.Username + " (" + Session.Role + ")";

            if (!Session.CanOpenSettings)
            {
                Shown += (s, e) =>
                {
                    new StockDisplay().Show();
                    Close();
                };
                return;
            }

            WireNavigation();
            Session.AddLogoutButton(this, panel2);
            UpdateActionState();
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

            btnAddUser.Click += (s, e) => AddUser();
            btnRefresh.Click += (s, e) => ReloadUsers();
            btnQuickStock.Click += (s, e) => OpenForm(new StockForm());
            btnQuickPos.Click += (s, e) => OpenForm(new StockDisplay());
            btnQuickHome.Click += (s, e) => OpenForm(new Form1());
            btnQuickRefresh.Click += (s, e) => ReloadUsers();
            btnUpdateRole.Click += (s, e) => UpdateSelectedUserRole();
            btnResetPassword.Click += (s, e) => ResetSelectedUserPassword();
            btnDeleteUser.Click += (s, e) => DeleteSelectedUser();
            dgvUsers.SelectionChanged += (s, e) => UpdateActionState();

            chkShowPassword.CheckedChanged += (s, e) => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    AddUser();
                }
            };
        }

        private void OpenForm(Form form)
        {
            form.Show();
            Hide();
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
