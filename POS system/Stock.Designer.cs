namespace POS_system
{
    partial class StockForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtProductname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtunitPrice = new System.Windows.Forms.TextBox();
            this.txtMaterial = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.dgtStock = new System.Windows.Forms.DataGridView();
            this.dtpDateAdded = new System.Windows.Forms.DateTimePicker();
            this.txtS_search = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgtStock)).BeginInit();
            this.SuspendLayout();
            // 
            // txtProductname
            // 
            this.txtProductname.Location = new System.Drawing.Point(264, 231);
            this.txtProductname.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProductname.Multiline = true;
            this.txtProductname.Name = "txtProductname";
            this.txtProductname.Size = new System.Drawing.Size(303, 29);
            this.txtProductname.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(68, 231);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Product Name :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(243, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 54);
            this.label2.TabIndex = 2;
            this.label2.Text = "Stock";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtunitPrice
            // 
            this.txtunitPrice.Location = new System.Drawing.Point(264, 353);
            this.txtunitPrice.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtunitPrice.Multiline = true;
            this.txtunitPrice.Name = "txtunitPrice";
            this.txtunitPrice.Size = new System.Drawing.Size(303, 29);
            this.txtunitPrice.TabIndex = 4;
            // 
            // txtMaterial
            // 
            this.txtMaterial.Location = new System.Drawing.Point(264, 417);
            this.txtMaterial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMaterial.Multiline = true;
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Size = new System.Drawing.Size(303, 29);
            this.txtMaterial.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(129, 353);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 29);
            this.label3.TabIndex = 6;
            this.label3.Text = "Unit Price:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(136, 293);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 29);
            this.label4.TabIndex = 7;
            this.label4.Text = "Category:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(149, 417);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 29);
            this.label5.TabIndex = 8;
            this.label5.Text = "Material:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(103, 484);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 29);
            this.label6.TabIndex = 9;
            this.label6.Text = "Date Added:";
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(191, 565);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(140, 49);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(403, 565);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(140, 49);
            this.btnBack.TabIndex = 12;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // dgtStock
            // 
            this.dgtStock.AllowUserToAddRows = false;
            this.dgtStock.AllowUserToDeleteRows = false;
            this.dgtStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgtStock.Location = new System.Drawing.Point(599, 95);
            this.dgtStock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgtStock.Name = "dgtStock";
            this.dgtStock.ReadOnly = true;
            this.dgtStock.RowHeadersWidth = 51;
            this.dgtStock.RowTemplate.Height = 24;
            this.dgtStock.Size = new System.Drawing.Size(957, 491);
            this.dgtStock.TabIndex = 13;
            this.dgtStock.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgtStock_CellClick);
            this.dgtStock.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgtStock_CellContentClick);
            // 
            // dtpDateAdded
            // 
            this.dtpDateAdded.Location = new System.Drawing.Point(264, 489);
            this.dtpDateAdded.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpDateAdded.Name = "dtpDateAdded";
            this.dtpDateAdded.Size = new System.Drawing.Size(303, 22);
            this.dtpDateAdded.TabIndex = 14;
            // 
            // txtS_search
            // 
            this.txtS_search.Location = new System.Drawing.Point(800, 38);
            this.txtS_search.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtS_search.Name = "txtS_search";
            this.txtS_search.Size = new System.Drawing.Size(503, 22);
            this.txtS_search.TabIndex = 15;
            this.txtS_search.TextChanged += new System.EventHandler(this.txtS_search_TextChanged);
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "Garments",
            "School Supplies",
            "Footwear",
            "RTW"});
            this.cmbCategory.Location = new System.Drawing.Point(264, 293);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(303, 24);
            this.cmbCategory.TabIndex = 3;
            // 
            // StockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1580, 811);
            this.Controls.Add(this.txtS_search);
            this.Controls.Add(this.dtpDateAdded);
            this.Controls.Add(this.dgtStock);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMaterial);
            this.Controls.Add(this.txtunitPrice);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtProductname);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "StockForm";
            this.Text = "Stock";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgtStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProductname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtunitPrice;
        private System.Windows.Forms.TextBox txtMaterial;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.DataGridView dgtStock;
        private System.Windows.Forms.DateTimePicker dtpDateAdded;
        private System.Windows.Forms.TextBox txtS_search;
        private System.Windows.Forms.ComboBox cmbCategory;
    }
}