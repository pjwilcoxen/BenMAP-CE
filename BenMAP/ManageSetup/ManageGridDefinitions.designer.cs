namespace BenMAP
{
	partial class ManageGridDefinetions
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


		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageGridDefinetions));
			this.grpManageGrid = new System.Windows.Forms.GroupBox();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.grpAvailableGrid = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkShowAll = new System.Windows.Forms.CheckBox();
			this.cboProjections = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cboDefaultGridType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtGridType = new System.Windows.Forms.TextBox();
			this.lblGridType = new System.Windows.Forms.Label();
			this.lstAvailableGrid = new System.Windows.Forms.ListBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnViewMetadata = new System.Windows.Forms.Button();
			this.grpManageGrid.SuspendLayout();
			this.grpAvailableGrid.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpManageGrid
			// 
			this.grpManageGrid.Controls.Add(this.btnEdit);
			this.grpManageGrid.Controls.Add(this.btnAdd);
			this.grpManageGrid.Controls.Add(this.btnDelete);
			this.grpManageGrid.Controls.Add(this.grpAvailableGrid);
			this.grpManageGrid.Location = new System.Drawing.Point(6, 3);
			this.grpManageGrid.Name = "grpManageGrid";
			this.grpManageGrid.Size = new System.Drawing.Size(628, 412);
			this.grpManageGrid.TabIndex = 0;
			this.grpManageGrid.TabStop = false;
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(517, 371);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(77, 27);
			this.btnEdit.TabIndex = 3;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(430, 371);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(81, 27);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(348, 371);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(76, 27);
			this.btnDelete.TabIndex = 1;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// grpAvailableGrid
			// 
			this.grpAvailableGrid.Controls.Add(this.groupBox2);
			this.grpAvailableGrid.Controls.Add(this.cboDefaultGridType);
			this.grpAvailableGrid.Controls.Add(this.label1);
			this.grpAvailableGrid.Controls.Add(this.txtGridType);
			this.grpAvailableGrid.Controls.Add(this.lblGridType);
			this.grpAvailableGrid.Controls.Add(this.lstAvailableGrid);
			this.grpAvailableGrid.Location = new System.Drawing.Point(6, 8);
			this.grpAvailableGrid.Name = "grpAvailableGrid";
			this.grpAvailableGrid.Size = new System.Drawing.Size(616, 357);
			this.grpAvailableGrid.TabIndex = 0;
			this.grpAvailableGrid.TabStop = false;
			this.grpAvailableGrid.Text = "Available Grid Definitions";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chkShowAll);
			this.groupBox2.Controls.Add(this.cboProjections);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(11, 182);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(599, 167);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "GIS Projection";
			// 
			// chkShowAll
			// 
			this.chkShowAll.AutoSize = true;
			this.chkShowAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkShowAll.Location = new System.Drawing.Point(504, 105);
			this.chkShowAll.Name = "chkShowAll";
			this.chkShowAll.Size = new System.Drawing.Size(73, 18);
			this.chkShowAll.TabIndex = 2;
			this.chkShowAll.Text = "Show All";
			this.chkShowAll.UseVisualStyleBackColor = true;
			this.chkShowAll.CheckedChanged += new System.EventHandler(this.chkShowAll_CheckedChanged);
			// 
			// cboProjections
			// 
			this.cboProjections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboProjections.FormattingEnabled = true;
			this.cboProjections.Location = new System.Drawing.Point(19, 129);
			this.cboProjections.Name = "cboProjections";
			this.cboProjections.Size = new System.Drawing.Size(558, 22);
			this.cboProjections.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(551, 49);
			this.label3.TabIndex = 1;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(577, 35);
			this.label2.TabIndex = 0;
			this.label2.Text = "This projection will be used when performing area- or distance-based calculations" +
" on all shapefile grid definitions in this setup.";
			// 
			// cboDefaultGridType
			// 
			this.cboDefaultGridType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDefaultGridType.FormattingEnabled = true;
			this.cboDefaultGridType.Location = new System.Drawing.Point(230, 145);
			this.cboDefaultGridType.Name = "cboDefaultGridType";
			this.cboDefaultGridType.Size = new System.Drawing.Size(164, 22);
			this.cboDefaultGridType.TabIndex = 4;
			this.cboDefaultGridType.SelectedValueChanged += new System.EventHandler(this.cboDefaultGridType_SelectedValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(227, 113);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 14);
			this.label1.TabIndex = 3;
			this.label1.Text = "Default Grid Type:";
			// 
			// txtGridType
			// 
			this.txtGridType.Location = new System.Drawing.Point(230, 76);
			this.txtGridType.Name = "txtGridType";
			this.txtGridType.ReadOnly = true;
			this.txtGridType.Size = new System.Drawing.Size(164, 22);
			this.txtGridType.TabIndex = 2;
			this.txtGridType.Text = "Shapefile";
			// 
			// lblGridType
			// 
			this.lblGridType.AutoSize = true;
			this.lblGridType.Location = new System.Drawing.Point(227, 40);
			this.lblGridType.Name = "lblGridType";
			this.lblGridType.Size = new System.Drawing.Size(60, 14);
			this.lblGridType.TabIndex = 1;
			this.lblGridType.Text = "Grid Type:";
			// 
			// lstAvailableGrid
			// 
			this.lstAvailableGrid.FormattingEnabled = true;
			this.lstAvailableGrid.ItemHeight = 14;
			this.lstAvailableGrid.Location = new System.Drawing.Point(6, 23);
			this.lstAvailableGrid.Name = "lstAvailableGrid";
			this.lstAvailableGrid.Size = new System.Drawing.Size(215, 144);
			this.lstAvailableGrid.TabIndex = 0;
			this.lstAvailableGrid.SelectedValueChanged += new System.EventHandler(this.lstAvailableGrid_SelectedValueChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(455, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(81, 27);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Visible = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(542, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(77, 27);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.flowLayoutPanel1);
			this.groupBox1.Location = new System.Drawing.Point(6, 416);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(628, 56);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.btnOK);
			this.flowLayoutPanel1.Controls.Add(this.btnCancel);
			this.flowLayoutPanel1.Controls.Add(this.btnViewMetadata);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 18);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(622, 35);
			this.flowLayoutPanel1.TabIndex = 0;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// btnViewMetadata
			// 
			this.btnViewMetadata.Location = new System.Drawing.Point(337, 3);
			this.btnViewMetadata.Name = "btnViewMetadata";
			this.btnViewMetadata.Size = new System.Drawing.Size(112, 27);
			this.btnViewMetadata.TabIndex = 0;
			this.btnViewMetadata.Text = "View Metadata";
			this.btnViewMetadata.UseVisualStyleBackColor = true;
			this.btnViewMetadata.Click += new System.EventHandler(this.btnViewMetadata_Click);
			// 
			// ManageGridDefinetions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(647, 479);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grpManageGrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(436, 354);
			this.Name = "ManageGridDefinetions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Manage Grid Definitions";
			this.Activated += new System.EventHandler(this.ManageGridDefinetions_Activated);
			this.Load += new System.EventHandler(this.ManageGridDefinetions_Load);
			this.grpManageGrid.ResumeLayout(false);
			this.grpAvailableGrid.ResumeLayout(false);
			this.grpAvailableGrid.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}


		private System.Windows.Forms.GroupBox grpManageGrid;
		private System.Windows.Forms.GroupBox grpAvailableGrid;
		private System.Windows.Forms.TextBox txtGridType;
		private System.Windows.Forms.Label lblGridType;
		private System.Windows.Forms.ListBox lstAvailableGrid;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cboDefaultGridType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnViewMetadata;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkShowAll;
		private System.Windows.Forms.ComboBox cboProjections;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
	}
}