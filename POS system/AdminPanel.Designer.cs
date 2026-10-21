namespace POS_system
{
    partial class AdminPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminPanel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAdminPanel = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnStockDisplay = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.lblSubheader = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.statsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelStatUsers = new System.Windows.Forms.Panel();
            this.lblTotalUsers = new System.Windows.Forms.Label();
            this.lblTotalUsersTitle = new System.Windows.Forms.Label();
            this.panelStatAdmins = new System.Windows.Forms.Panel();
            this.lblAdmins = new System.Windows.Forms.Label();
            this.lblAdminsTitle = new System.Windows.Forms.Label();
            this.panelStatManagers = new System.Windows.Forms.Panel();
            this.lblManagers = new System.Windows.Forms.Label();
            this.lblManagersTitle = new System.Windows.Forms.Label();
            this.panelStatCashiers = new System.Windows.Forms.Panel();
            this.lblCashiers = new System.Windows.Forms.Label();
            this.lblCashiersTitle = new System.Windows.Forms.Label();
            this.bodyLayout = new System.Windows.Forms.TableLayoutPanel();
            this.leftLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelAddUser = new System.Windows.Forms.Panel();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelQuickActions = new System.Windows.Forms.Panel();
            this.btnQuickRefresh = new System.Windows.Forms.Button();
            this.btnQuickHome = new System.Windows.Forms.Button();
            this.btnQuickPos = new System.Windows.Forms.Button();
            this.btnQuickStock = new System.Windows.Forms.Button();
            this.lblQuickTitle = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.lblInfoBody = new System.Windows.Forms.Label();
            this.lblInfoTitle = new System.Windows.Forms.Label();
            this.panelUsers = new System.Windows.Forms.Panel();
            this.flowUserActions = new System.Windows.Forms.FlowLayoutPanel();
            this.cboRoleChange = new System.Windows.Forms.ComboBox();
            this.btnUpdateRole = new System.Windows.Forms.Button();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.lblSelection = new System.Windows.Forms.Label();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.colUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblUsersTitle = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.panelContent.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.statsLayout.SuspendLayout();
            this.panelStatUsers.SuspendLayout();
            this.panelStatAdmins.SuspendLayout();
            this.panelStatManagers.SuspendLayout();
            this.panelStatCashiers.SuspendLayout();
            this.bodyLayout.SuspendLayout();
            this.leftLayout.SuspendLayout();
            this.panelAddUser.SuspendLayout();
            this.panelQuickActions.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panelUsers.SuspendLayout();
            this.flowUserActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.btnAdminPanel);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.btnStockDisplay);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.flowLayoutPanel9);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 900);
            this.panel2.TabIndex = 114;
            // 
            // btnAdminPanel
            // 
            this.btnAdminPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(154)))), ((int)(((byte)(107)))));
            this.btnAdminPanel.FlatAppearance.BorderSize = 0;
            this.btnAdminPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminPanel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnAdminPanel.ForeColor = System.Drawing.Color.White;
            this.btnAdminPanel.Location = new System.Drawing.Point(26, 544);
            this.btnAdminPanel.Name = "btnAdminPanel";
            this.btnAdminPanel.Size = new System.Drawing.Size(248, 65);
            this.btnAdminPanel.TabIndex = 113;
            this.btnAdminPanel.Text = "Admin Panel";
            this.btnAdminPanel.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(154)))), ((int)(((byte)(107)))));
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(26, 455);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(248, 65);
            this.button6.TabIndex = 114;
            this.button6.Text = "Stock";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // btnStockDisplay
            // 
            this.btnStockDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(154)))), ((int)(((byte)(107)))));
            this.btnStockDisplay.FlatAppearance.BorderSize = 0;
            this.btnStockDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockDisplay.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnStockDisplay.ForeColor = System.Drawing.Color.White;
            this.btnStockDisplay.Location = new System.Drawing.Point(26, 366);
            this.btnStockDisplay.Name = "btnStockDisplay";
            this.btnStockDisplay.Size = new System.Drawing.Size(248, 65);
            this.btnStockDisplay.TabIndex = 113;
            this.btnStockDisplay.Text = "Stock Display";
            this.btnStockDisplay.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(154)))), ((int)(((byte)(107)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(26, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(248, 65);
            this.button1.TabIndex = 112;
            this.button1.Text = "Home";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.pictureBox7);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(300, 236);
            this.panel3.TabIndex = 98;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.White;
            this.pictureBox7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox7.BackgroundImage")));
            this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox7.Location = new System.Drawing.Point(68, 28);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(165, 179);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox7.TabIndex = 83;
            this.pictureBox7.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(86, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 28);
            this.label7.TabIndex = 98;
            this.label7.Text = "POS System";
            this.label7.Visible = false;
            // 
            // flowLayoutPanel9
            // 
            this.flowLayoutPanel9.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel9.Location = new System.Drawing.Point(17, 150);
            this.flowLayoutPanel9.Name = "flowLayoutPanel9";
            this.flowLayoutPanel9.Size = new System.Drawing.Size(260, 54);
            this.flowLayoutPanel9.TabIndex = 110;
            this.flowLayoutPanel9.Visible = false;
            // 
            // panelContent
            // 
            this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.panelHeader);
            this.panelContent.Controls.Add(this.statsLayout);
            this.panelContent.Controls.Add(this.bodyLayout);
            this.panelContent.Location = new System.Drawing.Point(300, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(882, 900);
            this.panelContent.TabIndex = 115;
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = System.Drawing.Color.Navy;
            this.panelHeader.Controls.Add(this.lblStatus);
            this.panelHeader.Controls.Add(this.lblCurrentUser);
            this.panelHeader.Controls.Add(this.lblSubheader);
            this.panelHeader.Controls.Add(this.lblHeader);
            this.panelHeader.Location = new System.Drawing.Point(24, 24);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(834, 96);
            this.panelHeader.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(245)))));
            this.lblStatus.Location = new System.Drawing.Point(380, 54);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(430, 22);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentUser.ForeColor = System.Drawing.Color.White;
            this.lblCurrentUser.Location = new System.Drawing.Point(380, 20);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(430, 24);
            this.lblCurrentUser.TabIndex = 2;
            this.lblCurrentUser.Text = "Signed in as";
            this.lblCurrentUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubheader
            // 
            this.lblSubheader.AutoSize = true;
            this.lblSubheader.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblSubheader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(245)))));
            this.lblSubheader.Location = new System.Drawing.Point(24, 58);
            this.lblSubheader.Name = "lblSubheader";
            this.lblSubheader.Size = new System.Drawing.Size(302, 19);
            this.lblSubheader.TabIndex = 1;
            this.lblSubheader.Text = "Manage user accounts, roles, and system access";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(22, 14);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(241, 45);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "ADMIN PANEL";
            // 
            // statsLayout
            // 
            this.statsLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statsLayout.BackColor = System.Drawing.Color.White;
            this.statsLayout.ColumnCount = 4;
            this.statsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.statsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.statsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.statsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.statsLayout.Controls.Add(this.panelStatUsers, 0, 0);
            this.statsLayout.Controls.Add(this.panelStatAdmins, 1, 0);
            this.statsLayout.Controls.Add(this.panelStatManagers, 2, 0);
            this.statsLayout.Controls.Add(this.panelStatCashiers, 3, 0);
            this.statsLayout.Location = new System.Drawing.Point(24, 138);
            this.statsLayout.Name = "statsLayout";
            this.statsLayout.RowCount = 1;
            this.statsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.statsLayout.Size = new System.Drawing.Size(834, 96);
            this.statsLayout.TabIndex = 1;
            // 
            // panelStatUsers
            // 
            this.panelStatUsers.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelStatUsers.Controls.Add(this.lblTotalUsers);
            this.panelStatUsers.Controls.Add(this.lblTotalUsersTitle);
            this.panelStatUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatUsers.Location = new System.Drawing.Point(0, 0);
            this.panelStatUsers.Margin = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.panelStatUsers.Name = "panelStatUsers";
            this.panelStatUsers.Size = new System.Drawing.Size(194, 96);
            this.panelStatUsers.TabIndex = 0;
            // 
            // lblTotalUsers
            // 
            this.lblTotalUsers.AutoSize = true;
            this.lblTotalUsers.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotalUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblTotalUsers.Location = new System.Drawing.Point(18, 42);
            this.lblTotalUsers.Name = "lblTotalUsers";
            this.lblTotalUsers.Size = new System.Drawing.Size(33, 37);
            this.lblTotalUsers.TabIndex = 1;
            this.lblTotalUsers.Text = "0";
            // 
            // lblTotalUsersTitle
            // 
            this.lblTotalUsersTitle.AutoSize = true;
            this.lblTotalUsersTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalUsersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTotalUsersTitle.Location = new System.Drawing.Point(18, 14);
            this.lblTotalUsersTitle.Name = "lblTotalUsersTitle";
            this.lblTotalUsersTitle.Size = new System.Drawing.Size(76, 19);
            this.lblTotalUsersTitle.TabIndex = 0;
            this.lblTotalUsersTitle.Text = "Total Users";
            // 
            // panelStatAdmins
            // 
            this.panelStatAdmins.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelStatAdmins.Controls.Add(this.lblAdmins);
            this.panelStatAdmins.Controls.Add(this.lblAdminsTitle);
            this.panelStatAdmins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatAdmins.Location = new System.Drawing.Point(208, 0);
            this.panelStatAdmins.Margin = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.panelStatAdmins.Name = "panelStatAdmins";
            this.panelStatAdmins.Size = new System.Drawing.Size(194, 96);
            this.panelStatAdmins.TabIndex = 1;
            // 
            // lblAdmins
            // 
            this.lblAdmins.AutoSize = true;
            this.lblAdmins.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblAdmins.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblAdmins.Location = new System.Drawing.Point(18, 42);
            this.lblAdmins.Name = "lblAdmins";
            this.lblAdmins.Size = new System.Drawing.Size(33, 37);
            this.lblAdmins.TabIndex = 1;
            this.lblAdmins.Text = "0";
            // 
            // lblAdminsTitle
            // 
            this.lblAdminsTitle.AutoSize = true;
            this.lblAdminsTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAdminsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblAdminsTitle.Location = new System.Drawing.Point(18, 14);
            this.lblAdminsTitle.Name = "lblAdminsTitle";
            this.lblAdminsTitle.Size = new System.Drawing.Size(99, 19);
            this.lblAdminsTitle.TabIndex = 0;
            this.lblAdminsTitle.Text = "Administrators";
            // 
            // panelStatManagers
            // 
            this.panelStatManagers.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelStatManagers.Controls.Add(this.lblManagers);
            this.panelStatManagers.Controls.Add(this.lblManagersTitle);
            this.panelStatManagers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatManagers.Location = new System.Drawing.Point(416, 0);
            this.panelStatManagers.Margin = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.panelStatManagers.Name = "panelStatManagers";
            this.panelStatManagers.Size = new System.Drawing.Size(194, 96);
            this.panelStatManagers.TabIndex = 2;
            // 
            // lblManagers
            // 
            this.lblManagers.AutoSize = true;
            this.lblManagers.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblManagers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblManagers.Location = new System.Drawing.Point(18, 42);
            this.lblManagers.Name = "lblManagers";
            this.lblManagers.Size = new System.Drawing.Size(33, 37);
            this.lblManagers.TabIndex = 1;
            this.lblManagers.Text = "0";
            // 
            // lblManagersTitle
            // 
            this.lblManagersTitle.AutoSize = true;
            this.lblManagersTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblManagersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblManagersTitle.Location = new System.Drawing.Point(18, 14);
            this.lblManagersTitle.Name = "lblManagersTitle";
            this.lblManagersTitle.Size = new System.Drawing.Size(70, 19);
            this.lblManagersTitle.TabIndex = 0;
            this.lblManagersTitle.Text = "Managers";
            // 
            // panelStatCashiers
            // 
            this.panelStatCashiers.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelStatCashiers.Controls.Add(this.lblCashiers);
            this.panelStatCashiers.Controls.Add(this.lblCashiersTitle);
            this.panelStatCashiers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatCashiers.Location = new System.Drawing.Point(624, 0);
            this.panelStatCashiers.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatCashiers.Name = "panelStatCashiers";
            this.panelStatCashiers.Size = new System.Drawing.Size(210, 96);
            this.panelStatCashiers.TabIndex = 3;
            // 
            // lblCashiers
            // 
            this.lblCashiers.AutoSize = true;
            this.lblCashiers.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCashiers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblCashiers.Location = new System.Drawing.Point(18, 42);
            this.lblCashiers.Name = "lblCashiers";
            this.lblCashiers.Size = new System.Drawing.Size(33, 37);
            this.lblCashiers.TabIndex = 1;
            this.lblCashiers.Text = "0";
            // 
            // lblCashiersTitle
            // 
            this.lblCashiersTitle.AutoSize = true;
            this.lblCashiersTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCashiersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblCashiersTitle.Location = new System.Drawing.Point(18, 14);
            this.lblCashiersTitle.Name = "lblCashiersTitle";
            this.lblCashiersTitle.Size = new System.Drawing.Size(60, 19);
            this.lblCashiersTitle.TabIndex = 0;
            this.lblCashiersTitle.Text = "Cashiers";
            // 
            // bodyLayout
            // 
            this.bodyLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bodyLayout.BackColor = System.Drawing.Color.White;
            this.bodyLayout.ColumnCount = 2;
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bodyLayout.Controls.Add(this.leftLayout, 0, 0);
            this.bodyLayout.Controls.Add(this.panelUsers, 1, 0);
            this.bodyLayout.Location = new System.Drawing.Point(24, 252);
            this.bodyLayout.Name = "bodyLayout";
            this.bodyLayout.RowCount = 1;
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bodyLayout.Size = new System.Drawing.Size(834, 624);
            this.bodyLayout.TabIndex = 2;
            // 
            // leftLayout
            // 
            this.leftLayout.ColumnCount = 1;
            this.leftLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.leftLayout.Controls.Add(this.panelAddUser, 0, 0);
            this.leftLayout.Controls.Add(this.panelQuickActions, 0, 1);
            this.leftLayout.Controls.Add(this.panelInfo, 0, 2);
            this.leftLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftLayout.Location = new System.Drawing.Point(0, 0);
            this.leftLayout.Margin = new System.Windows.Forms.Padding(0, 0, 18, 0);
            this.leftLayout.Name = "leftLayout";
            this.leftLayout.RowCount = 3;
            this.leftLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.leftLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.leftLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.leftLayout.Size = new System.Drawing.Size(342, 624);
            this.leftLayout.TabIndex = 0;
            // 
            // panelAddUser
            // 
            this.panelAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelAddUser.Controls.Add(this.chkShowPassword);
            this.panelAddUser.Controls.Add(this.btnAddUser);
            this.panelAddUser.Controls.Add(this.cboRole);
            this.panelAddUser.Controls.Add(this.lblRole);
            this.panelAddUser.Controls.Add(this.txtPassword);
            this.panelAddUser.Controls.Add(this.label3);
            this.panelAddUser.Controls.Add(this.txtUsername);
            this.panelAddUser.Controls.Add(this.label2);
            this.panelAddUser.Controls.Add(this.label1);
            this.panelAddUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAddUser.Location = new System.Drawing.Point(0, 0);
            this.panelAddUser.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.panelAddUser.Name = "panelAddUser";
            this.panelAddUser.Size = new System.Drawing.Size(342, 262);
            this.panelAddUser.TabIndex = 0;
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkShowPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(100)))));
            this.chkShowPassword.Location = new System.Drawing.Point(18, 238);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(108, 19);
            this.chkShowPassword.TabIndex = 8;
            this.chkShowPassword.Text = "Show password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            // 
            // btnAddUser
            // 
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(154)))), ((int)(((byte)(107)))));
            this.btnAddUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddUser.FlatAppearance.BorderSize = 0;
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(148, 200);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(176, 30);
            this.btnAddUser.TabIndex = 7;
            this.btnAddUser.Text = "Add User";
            this.btnAddUser.UseVisualStyleBackColor = false;
            // 
            // cboRole
            // 
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboRole.FormattingEnabled = true;
            this.cboRole.Items.AddRange(new object[] {
            "Cashier",
            "Manager",
            "Admin"});
            this.cboRole.Location = new System.Drawing.Point(18, 200);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(120, 25);
            this.cboRole.TabIndex = 6;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(100)))));
            this.lblRole.Location = new System.Drawing.Point(18, 178);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(35, 17);
            this.lblRole.TabIndex = 5;
            this.lblRole.Text = "Role";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtPassword.Location = new System.Drawing.Point(18, 138);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(306, 26);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(100)))));
            this.label3.Location = new System.Drawing.Point(18, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtUsername.Location = new System.Drawing.Point(18, 76);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(306, 26);
            this.txtUsername.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(100)))));
            this.label2.Location = new System.Drawing.Point(18, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add New User";
            // 
            // panelQuickActions
            // 
            this.panelQuickActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelQuickActions.Controls.Add(this.btnQuickRefresh);
            this.panelQuickActions.Controls.Add(this.btnQuickHome);
            this.panelQuickActions.Controls.Add(this.btnQuickPos);
            this.panelQuickActions.Controls.Add(this.btnQuickStock);
            this.panelQuickActions.Controls.Add(this.lblQuickTitle);
            this.panelQuickActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelQuickActions.Location = new System.Drawing.Point(0, 280);
            this.panelQuickActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.panelQuickActions.Name = "panelQuickActions";
            this.panelQuickActions.Size = new System.Drawing.Size(342, 142);
            this.panelQuickActions.TabIndex = 1;
            // 
            // btnQuickRefresh
            // 
            this.btnQuickRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(154)))), ((int)(((byte)(107)))));
            this.btnQuickRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuickRefresh.FlatAppearance.BorderSize = 0;
            this.btnQuickRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuickRefresh.ForeColor = System.Drawing.Color.White;
            this.btnQuickRefresh.Location = new System.Drawing.Point(176, 92);
            this.btnQuickRefresh.Name = "btnQuickRefresh";
            this.btnQuickRefresh.Size = new System.Drawing.Size(148, 40);
            this.btnQuickRefresh.TabIndex = 4;
            this.btnQuickRefresh.Text = "Refresh Users";
            this.btnQuickRefresh.UseVisualStyleBackColor = false;
            // 
            // btnQuickHome
            // 
            this.btnQuickHome.BackColor = System.Drawing.Color.Navy;
            this.btnQuickHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuickHome.FlatAppearance.BorderSize = 0;
            this.btnQuickHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickHome.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuickHome.ForeColor = System.Drawing.Color.White;
            this.btnQuickHome.Location = new System.Drawing.Point(18, 92);
            this.btnQuickHome.Name = "btnQuickHome";
            this.btnQuickHome.Size = new System.Drawing.Size(148, 40);
            this.btnQuickHome.TabIndex = 3;
            this.btnQuickHome.Text = "Open Dashboard";
            this.btnQuickHome.UseVisualStyleBackColor = false;
            // 
            // btnQuickPos
            // 
            this.btnQuickPos.BackColor = System.Drawing.Color.Navy;
            this.btnQuickPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuickPos.FlatAppearance.BorderSize = 0;
            this.btnQuickPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickPos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuickPos.ForeColor = System.Drawing.Color.White;
            this.btnQuickPos.Location = new System.Drawing.Point(176, 46);
            this.btnQuickPos.Name = "btnQuickPos";
            this.btnQuickPos.Size = new System.Drawing.Size(148, 40);
            this.btnQuickPos.TabIndex = 2;
            this.btnQuickPos.Text = "Open POS";
            this.btnQuickPos.UseVisualStyleBackColor = false;
            // 
            // btnQuickStock
            // 
            this.btnQuickStock.BackColor = System.Drawing.Color.Navy;
            this.btnQuickStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuickStock.FlatAppearance.BorderSize = 0;
            this.btnQuickStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickStock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnQuickStock.ForeColor = System.Drawing.Color.White;
            this.btnQuickStock.Location = new System.Drawing.Point(18, 46);
            this.btnQuickStock.Name = "btnQuickStock";
            this.btnQuickStock.Size = new System.Drawing.Size(148, 40);
            this.btnQuickStock.TabIndex = 1;
            this.btnQuickStock.Text = "Manage Stock";
            this.btnQuickStock.UseVisualStyleBackColor = false;
            // 
            // lblQuickTitle
            // 
            this.lblQuickTitle.AutoSize = true;
            this.lblQuickTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblQuickTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.lblQuickTitle.Location = new System.Drawing.Point(18, 12);
            this.lblQuickTitle.Name = "lblQuickTitle";
            this.lblQuickTitle.Size = new System.Drawing.Size(134, 25);
            this.lblQuickTitle.TabIndex = 0;
            this.lblQuickTitle.Text = "Quick Actions";
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelInfo.Controls.Add(this.lblInfoBody);
            this.panelInfo.Controls.Add(this.lblInfoTitle);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(0, 440);
            this.panelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(342, 184);
            this.panelInfo.TabIndex = 2;
            // 
            // lblInfoBody
            // 
            this.lblInfoBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfoBody.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblInfoBody.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(100)))));
            this.lblInfoBody.Location = new System.Drawing.Point(18, 48);
            this.lblInfoBody.Name = "lblInfoBody";
            this.lblInfoBody.Size = new System.Drawing.Size(306, 122);
            this.lblInfoBody.TabIndex = 1;
            this.lblInfoBody.Text = resources.GetString("lblInfoBody.Text");
            // 
            // lblInfoTitle
            // 
            this.lblInfoTitle.AutoSize = true;
            this.lblInfoTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblInfoTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.lblInfoTitle.Location = new System.Drawing.Point(18, 14);
            this.lblInfoTitle.Name = "lblInfoTitle";
            this.lblInfoTitle.Size = new System.Drawing.Size(212, 25);
            this.lblInfoTitle.TabIndex = 0;
            this.lblInfoTitle.Text = "Rules for adding Users";
            // 
            // panelUsers
            // 
            this.panelUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelUsers.Controls.Add(this.flowUserActions);
            this.panelUsers.Controls.Add(this.lblSelection);
            this.panelUsers.Controls.Add(this.dgvUsers);
            this.panelUsers.Controls.Add(this.btnRefresh);
            this.panelUsers.Controls.Add(this.lblUsersTitle);
            this.panelUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUsers.Location = new System.Drawing.Point(360, 0);
            this.panelUsers.Margin = new System.Windows.Forms.Padding(0);
            this.panelUsers.Name = "panelUsers";
            this.panelUsers.Size = new System.Drawing.Size(474, 624);
            this.panelUsers.TabIndex = 1;
            // 
            // flowUserActions
            // 
            this.flowUserActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowUserActions.Controls.Add(this.cboRoleChange);
            this.flowUserActions.Controls.Add(this.btnUpdateRole);
            this.flowUserActions.Controls.Add(this.btnResetPassword);
            this.flowUserActions.Controls.Add(this.btnDeleteUser);
            this.flowUserActions.Location = new System.Drawing.Point(18, 534);
            this.flowUserActions.Name = "flowUserActions";
            this.flowUserActions.Size = new System.Drawing.Size(438, 76);
            this.flowUserActions.TabIndex = 4;
            // 
            // cboRoleChange
            // 
            this.cboRoleChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoleChange.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboRoleChange.FormattingEnabled = true;
            this.cboRoleChange.Items.AddRange(new object[] {
            "Cashier",
            "Manager",
            "Admin"});
            this.cboRoleChange.Location = new System.Drawing.Point(0, 0);
            this.cboRoleChange.Margin = new System.Windows.Forms.Padding(0, 0, 10, 6);
            this.cboRoleChange.Name = "cboRoleChange";
            this.cboRoleChange.Size = new System.Drawing.Size(120, 25);
            this.cboRoleChange.TabIndex = 0;
            // 
            // btnUpdateRole
            // 
            this.btnUpdateRole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(154)))), ((int)(((byte)(107)))));
            this.btnUpdateRole.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdateRole.FlatAppearance.BorderSize = 0;
            this.btnUpdateRole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdateRole.ForeColor = System.Drawing.Color.White;
            this.btnUpdateRole.Location = new System.Drawing.Point(130, 0);
            this.btnUpdateRole.Margin = new System.Windows.Forms.Padding(0, 0, 10, 6);
            this.btnUpdateRole.Name = "btnUpdateRole";
            this.btnUpdateRole.Size = new System.Drawing.Size(120, 30);
            this.btnUpdateRole.TabIndex = 1;
            this.btnUpdateRole.Text = "Change Role";
            this.btnUpdateRole.UseVisualStyleBackColor = false;
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnResetPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResetPassword.FlatAppearance.BorderSize = 0;
            this.btnResetPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnResetPassword.ForeColor = System.Drawing.Color.White;
            this.btnResetPassword.Location = new System.Drawing.Point(260, 0);
            this.btnResetPassword.Margin = new System.Windows.Forms.Padding(0, 0, 10, 6);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(140, 30);
            this.btnResetPassword.TabIndex = 2;
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.UseVisualStyleBackColor = false;
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnDeleteUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteUser.FlatAppearance.BorderSize = 0;
            this.btnDeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteUser.ForeColor = System.Drawing.Color.White;
            this.btnDeleteUser.Location = new System.Drawing.Point(0, 36);
            this.btnDeleteUser.Margin = new System.Windows.Forms.Padding(0, 0, 10, 6);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(110, 30);
            this.btnDeleteUser.TabIndex = 3;
            this.btnDeleteUser.Text = "Delete User";
            this.btnDeleteUser.UseVisualStyleBackColor = false;
            // 
            // lblSelection
            // 
            this.lblSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelection.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(100)))));
            this.lblSelection.Location = new System.Drawing.Point(18, 506);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(438, 20);
            this.lblSelection.TabIndex = 3;
            this.lblSelection.Text = "Select a user to manage";
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUsers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUsers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvUsers.ColumnHeadersHeight = 38;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUsername,
            this.colRole});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(55)))), ((int)(((byte)(70)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(230)))));
            this.dgvUsers.Location = new System.Drawing.Point(18, 100);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowTemplate.Height = 34;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(438, 394);
            this.dgvUsers.TabIndex = 2;
            // 
            // colUsername
            // 
            this.colUsername.DataPropertyName = "Username";
            this.colUsername.FillWeight = 60F;
            this.colUsername.HeaderText = "Username";
            this.colUsername.Name = "colUsername";
            this.colUsername.ReadOnly = true;
            // 
            // colRole
            // 
            this.colRole.DataPropertyName = "Role";
            this.colRole.FillWeight = 40F;
            this.colRole.HeaderText = "Role";
            this.colRole.Name = "colRole";
            this.colRole.ReadOnly = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.Navy;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(360, 14);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblUsersTitle
            // 
            this.lblUsersTitle.AutoSize = true;
            this.lblUsersTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblUsersTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.lblUsersTitle.Location = new System.Drawing.Point(18, 14);
            this.lblUsersTitle.Name = "lblUsersTitle";
            this.lblUsersTitle.Size = new System.Drawing.Size(161, 30);
            this.lblUsersTitle.TabIndex = 0;
            this.lblUsersTitle.Text = "User Accounts";
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(1182, 900);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "AdminPanel";
            this.Text = "AdminPanel";
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.panelContent.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.statsLayout.ResumeLayout(false);
            this.panelStatUsers.ResumeLayout(false);
            this.panelStatUsers.PerformLayout();
            this.panelStatAdmins.ResumeLayout(false);
            this.panelStatAdmins.PerformLayout();
            this.panelStatManagers.ResumeLayout(false);
            this.panelStatManagers.PerformLayout();
            this.panelStatCashiers.ResumeLayout(false);
            this.panelStatCashiers.PerformLayout();
            this.bodyLayout.ResumeLayout(false);
            this.leftLayout.ResumeLayout(false);
            this.panelAddUser.ResumeLayout(false);
            this.panelAddUser.PerformLayout();
            this.panelQuickActions.ResumeLayout(false);
            this.panelQuickActions.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelUsers.ResumeLayout(false);
            this.panelUsers.PerformLayout();
            this.flowUserActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAdminPanel;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnStockDisplay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.Label lblSubheader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel statsLayout;
        private System.Windows.Forms.Panel panelStatUsers;
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Label lblTotalUsersTitle;
        private System.Windows.Forms.Panel panelStatAdmins;
        private System.Windows.Forms.Label lblAdmins;
        private System.Windows.Forms.Label lblAdminsTitle;
        private System.Windows.Forms.Panel panelStatManagers;
        private System.Windows.Forms.Label lblManagers;
        private System.Windows.Forms.Label lblManagersTitle;
        private System.Windows.Forms.Panel panelStatCashiers;
        private System.Windows.Forms.Label lblCashiers;
        private System.Windows.Forms.Label lblCashiersTitle;
        private System.Windows.Forms.TableLayoutPanel bodyLayout;
        private System.Windows.Forms.TableLayoutPanel leftLayout;
        private System.Windows.Forms.Panel panelAddUser;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelQuickActions;
        private System.Windows.Forms.Button btnQuickRefresh;
        private System.Windows.Forms.Button btnQuickHome;
        private System.Windows.Forms.Button btnQuickPos;
        private System.Windows.Forms.Button btnQuickStock;
        private System.Windows.Forms.Label lblQuickTitle;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblInfoBody;
        private System.Windows.Forms.Label lblInfoTitle;
        private System.Windows.Forms.Panel panelUsers;
        private System.Windows.Forms.FlowLayoutPanel flowUserActions;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Button btnUpdateRole;
        private System.Windows.Forms.ComboBox cboRoleChange;
        private System.Windows.Forms.Label lblSelection;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblUsersTitle;
    }
}
