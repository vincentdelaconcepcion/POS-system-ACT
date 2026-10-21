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
    public partial class Login : Form
    {
        private const string UsernamePlaceholder = "Enter UserName";
        private const string PasswordPlaceholder = "Enter Password";

        public Login()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Resize += Login_Resize;

            txtUsername.Text = UsernamePlaceholder;
            txtUsername.ForeColor = SystemColors.ScrollBar;
            txtPassword.Text = PasswordPlaceholder;
            txtPassword.ForeColor = SystemColors.ScrollBar;

            txtUsername.Enter += (s, e) =>
            {
                if (txtUsername.Text == UsernamePlaceholder)
                {
                    txtUsername.Clear();
                    txtUsername.ForeColor = SystemColors.WindowText;
                }
            };
            txtUsername.Leave += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    txtUsername.Text = UsernamePlaceholder;
                    txtUsername.ForeColor = SystemColors.ScrollBar;
                }
            };
            txtPassword.Enter += (s, e) =>
            {
                if (txtPassword.Text == PasswordPlaceholder)
                {
                    txtPassword.Clear();
                    txtPassword.ForeColor = SystemColors.WindowText;
                    txtPassword.UseSystemPasswordChar = true;
                }
            };
            txtPassword.Leave += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    txtPassword.Text = PasswordPlaceholder;
                    txtPassword.ForeColor = SystemColors.ScrollBar;
                    txtPassword.UseSystemPasswordChar = false;
                }
            };

            checkBox1.CheckedChanged += (s, e) => txtPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            LayoutLogin();
        }

        private void Login_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                LayoutLogin();
        }

        // this code is for the forms for the common controls to be auto adjust to the masximum width of the form
        private void LayoutLogin()
        {
            if (panel1 == null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return;

            int panelWidth = Math.Max(320, Math.Min(430, ClientSize.Width - 80));
            int panelHeight = Math.Max(360, Math.Min(400, ClientSize.Height - 60));

            panel1.SuspendLayout();
            try
            {
                panel1.Size = new Size(panelWidth, panelHeight);
                panel1.Location = new Point(
                    Math.Max(0, (ClientSize.Width - panelWidth) / 2),
                    Math.Max(0, (ClientSize.Height - panelHeight) / 2));

                const int left = 48;
                int contentWidth = panelWidth - (left * 2);

                label1.AutoSize = false;
                label1.TextAlign = ContentAlignment.MiddleCenter;
                label1.Location = new Point(0, 28);
                label1.Size = new Size(panelWidth, 44);

                label2.Location = new Point(left, 108);
                txtUsername.Location = new Point(left, 134);
                txtUsername.Width = contentWidth;

                label3.Location = new Point(left, 186);
                txtPassword.Location = new Point(left, 212);
                txtPassword.Width = contentWidth;

                checkBox1.Location = new Point(left, 258);

                btnLogIn.Location = new Point(left, 294);
                btnLogIn.Size = new Size(contentWidth, 44);
            }
            finally
            {
                panel1.ResumeLayout();
                panel1.PerformLayout();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            // this area of the code is where the actors are going ot input the text field for logging in to their given account
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (txtUsername.Text == UsernamePlaceholder || username.Length == 0)
            {
                MessageBox.Show("Please enter your username.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (txtPassword.Text == PasswordPlaceholder || password.Length == 0)
            {
                MessageBox.Show("Please enter your password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                string roleName;
                if (AuthService.TryLogin(username, password, out roleName))
                {
                    Session.SignIn(username, Session.ParseRole(roleName));
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Incorrect username or password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not reach the LogIn database.\n\n" + ex.Message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
