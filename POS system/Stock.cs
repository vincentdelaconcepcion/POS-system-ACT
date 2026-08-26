    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace POS_system
    {
        public partial class Stock: Form
        {
            public Stock()
            {
                InitializeComponent();
            }

            private void label2_Click(object sender, EventArgs e)
            {

            }
            TEMPODataSet db = new TEMPODataSet();
            private void btnAdd_Click(object sender, EventArgs e)
            {
            
                db = new TEMPODataSet();
            
           
            }
        }
    }
