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
