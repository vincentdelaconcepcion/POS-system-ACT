using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_system
{
    internal enum UserRole
    {
        Cashier,
        Manager,
        Admin
    }

    internal static class Session
    {
        public static string Username { get; private set; }
        public static UserRole Role { get; private set; }
        public static bool LogoutRequested { get; private set; }

        public static bool CanSeeHome { get { return Role == UserRole.Manager || Role == UserRole.Admin; } }
        public static bool CanManageStock { get { return CanSeeHome; } }
        public static bool CanOpenSettings { get { return Role == UserRole.Admin; } }

        public static void SignIn(string username, UserRole role)
        {
            Username = username;
            Role = role;
            LogoutRequested = false;
        }

        public static void SignOut()
        {
            Username = null;
            Role = UserRole.Cashier;
            LogoutRequested = true;
        }

        public static UserRole ParseRole(string roleName)
        {
            UserRole role;
            if (!Enum.TryParse(roleName, true, out role))
                return UserRole.Cashier;
            return role;
        }

        public static Form CreateLandingForm()
        {
            return CanSeeHome ? (Form)new Form1() : new StockDisplay();
        }

        public static void HideUnless(Control control, bool allowed)
        {
            if (control != null)
                control.Visible = allowed;
        }

        public static void AddLogoutButton(Form owner, Control container)
        {
            if (container == null || owner == null)
                return;

            if (container.Controls.ContainsKey("btnLogout"))
                return;

            var btn = new Button();
            btn.Name = "btnLogout";
            btn.Text = "Logout";
            btn.Size = new Size(248, 50);
            btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = SystemColors.MenuHighlight;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn.Location = new Point(26, Math.Max(20, container.ClientSize.Height - btn.Height - 20));

            btn.Click += (s, e) =>
            {
                SignOut();
                owner.Close();
            };

            container.Controls.Add(btn);
            btn.BringToFront();
        }
    }
}
