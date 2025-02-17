namespace BenMAP.APVX
{
	partial class Aggregation
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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.cboValuationAggregation = new System.Windows.Forms.ComboBox();
			this.cboIncidenceAggregation = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cboQALYAggregation = new System.Windows.Forms.ComboBox();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnCancel);
			this.groupBox2.Controls.Add(this.btnOK);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox2.Location = new System.Drawing.Point(0, 126);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(363, 56);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(195, 19);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 31);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(276, 18);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 31);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// cboValuationAggregation
			// 
			this.cboValuationAggregation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboValuationAggregation.FormattingEnabled = true;
			this.cboValuationAggregation.Location = new System.Drawing.Point(158, 78);
			this.cboValuationAggregation.Name = "cboValuationAggregation";
			this.cboValuationAggregation.Size = new System.Drawing.Size(183, 22);
			this.cboValuationAggregation.TabIndex = 3;
			// 
			// cboIncidenceAggregation
			// 
			this.cboIncidenceAggregation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIncidenceAggregation.FormattingEnabled = true;
			this.cboIncidenceAggregation.Location = new System.Drawing.Point(158, 28);
			this.cboIncidenceAggregation.Name = "cboIncidenceAggregation";
			this.cboIncidenceAggregation.Size = new System.Drawing.Size(183, 22);
			this.cboIncidenceAggregation.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(22, 81);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(129, 14);
			this.label2.TabIndex = 1;
			this.label2.Text = "Valuation Aggregation:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(130, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "Incidence Aggregation:";
			// 
			// cboQALYAggregation
			// 
			this.cboQALYAggregation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboQALYAggregation.FormattingEnabled = true;
			this.cboQALYAggregation.Location = new System.Drawing.Point(158, 116);
			this.cboQALYAggregation.Name = "cboQALYAggregation";
			this.cboQALYAggregation.Size = new System.Drawing.Size(183, 22);
			this.cboQALYAggregation.TabIndex = 4;
			this.cboQALYAggregation.Visible = false;
			// 
			// Aggregation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(363, 182);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.cboIncidenceAggregation);
			this.Controls.Add(this.cboValuationAggregation);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboQALYAggregation);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Aggregation";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Aggregation";
			this.Load += new System.EventHandler(this.Aggregation_Load);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.ComboBox cboValuationAggregation;
		public System.Windows.Forms.ComboBox cboIncidenceAggregation;
		public System.Windows.Forms.ComboBox cboQALYAggregation;
	}
}