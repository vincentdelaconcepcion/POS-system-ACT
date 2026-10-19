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
    public partial class StockDisplay : Form
    {
        public StockDisplay()
        {
            InitializeComponent();
            ApplyRoleAccess();
            Session.AddLogoutButton(this, panel2);
        }

        private void ApplyRoleAccess()
        {
            Session.HideUnless(btnHome, Session.CanSeeHome);
            Session.HideUnless(button6, Session.CanManageStock);
            Session.HideUnless(button12, Session.CanOpenSettings);
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
    }
}
