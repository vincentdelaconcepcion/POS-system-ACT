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

            var btn = new Button();
            btn.Name = "btnLogout";
            btn.Text = "Logout";
            btn.Size = new Size(120, 35);
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = SystemColors.MenuHighlight;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;

            int bottom = 0;
            foreach (Control c in container.Controls)
            {
                if (c is Button && c.Visible && c.Bottom > bottom)
                    bottom = c.Bottom;
            }
            btn.Location = new Point(30, bottom == 0 ? 20 : bottom + 12);

            btn.Click += (s, e) =>
            {
                LogoutRequested = true;
                owner.Close();
            };

            container.Controls.Add(btn);
        }
    }
}
