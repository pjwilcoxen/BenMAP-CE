﻿namespace BenMAP
{
	partial class ErrorReporting
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorReporting));
			this.label1 = new System.Windows.Forms.Label();
			this.txtOS = new System.Windows.Forms.TextBox();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtCountry = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblSeverity = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cboComponent = new System.Windows.Forms.ComboBox();
			this.lblAuditTrail = new System.Windows.Forms.Label();
			this.chkAuditTrail = new System.Windows.Forms.CheckBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.gbReportType = new System.Windows.Forms.GroupBox();
			this.rbFeature = new System.Windows.Forms.RadioButton();
			this.rbError = new System.Windows.Forms.RadioButton();
			this.gbSeverity = new System.Windows.Forms.GroupBox();
			this.rbBlocking = new System.Windows.Forms.RadioButton();
			this.rbMajor = new System.Windows.Forms.RadioButton();
			this.rbMinor = new System.Windows.Forms.RadioButton();
			this.lblErrorText = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtBenMAPCEVersion = new System.Windows.Forms.TextBox();
			this.gbReportType.SuspendLayout();
			this.gbSeverity.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(828, 32);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// txtOS
			// 
			this.txtOS.BackColor = System.Drawing.SystemColors.Control;
			this.txtOS.Location = new System.Drawing.Point(139, 61);
			this.txtOS.Name = "txtOS";
			this.txtOS.ReadOnly = true;
			this.txtOS.Size = new System.Drawing.Size(302, 22);
			this.txtOS.TabIndex = 3;
			this.txtOS.TabStop = false;
			// 
			// txtEmail
			// 
			this.txtEmail.Location = new System.Drawing.Point(650, 92);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(300, 22);
			this.txtEmail.TabIndex = 9;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(650, 64);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(300, 22);
			this.txtName.TabIndex = 7;
			// 
			// txtCountry
			// 
			this.txtCountry.Location = new System.Drawing.Point(650, 120);
			this.txtCountry.Name = "txtCountry";
			this.txtCountry.Size = new System.Drawing.Size(300, 22);
			this.txtCountry.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(15, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Operating System";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(464, 95);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(180, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Your email address (optional)";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(464, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(180, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Your name (optional)";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(464, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(183, 33);
			this.label5.TabIndex = 10;
			this.label5.Text = "Country where you are located (optional)";
			// 
			// btnSubmit
			// 
			this.btnSubmit.Location = new System.Drawing.Point(794, 648);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(75, 23);
			this.btnSubmit.TabIndex = 23;
			this.btnSubmit.Text = "Submit";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(875, 648);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 24;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(356, 652);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(432, 13);
			this.label6.TabIndex = 22;
			this.label6.Text = "To submit this report to us, click the Submit button.  Thank you for your time.";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(69, 173);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(256, 14);
			this.label7.TabIndex = 12;
			this.label7.Text = "What type of report would you like to submit?";
			// 
			// lblSeverity
			// 
			this.lblSeverity.AutoSize = true;
			this.lblSeverity.Location = new System.Drawing.Point(489, 173);
			this.lblSeverity.Name = "lblSeverity";
			this.lblSeverity.Size = new System.Drawing.Size(231, 14);
			this.lblSeverity.TabIndex = 14;
			this.lblSeverity.Text = "How severe is the error you experienced?";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(15, 304);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(296, 14);
			this.label9.TabIndex = 16;
			this.label9.Text = "What component of BenMAP-CE does the error affect?";
			// 
			// cboComponent
			// 
			this.cboComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboComponent.FormattingEnabled = true;
			this.cboComponent.Location = new System.Drawing.Point(15, 335);
			this.cboComponent.Name = "cboComponent";
			this.cboComponent.Size = new System.Drawing.Size(322, 22);
			this.cboComponent.TabIndex = 18;
			// 
			// lblAuditTrail
			// 
			this.lblAuditTrail.AutoSize = true;
			this.lblAuditTrail.Location = new System.Drawing.Point(430, 304);
			this.lblAuditTrail.Name = "lblAuditTrail";
			this.lblAuditTrail.Size = new System.Drawing.Size(421, 14);
			this.lblAuditTrail.TabIndex = 17;
			this.lblAuditTrail.Text = "The audit trail report may help us to debug or understand your report better.";
			// 
			// chkAuditTrail
			// 
			this.chkAuditTrail.AutoSize = true;
			this.chkAuditTrail.Location = new System.Drawing.Point(430, 335);
			this.chkAuditTrail.Name = "chkAuditTrail";
			this.chkAuditTrail.Size = new System.Drawing.Size(286, 18);
			this.chkAuditTrail.TabIndex = 19;
			this.chkAuditTrail.Text = "Include BenMAP-CE generated audit trail report.";
			this.chkAuditTrail.UseVisualStyleBackColor = true;
			// 
			// lblDescription
			// 
			this.lblDescription.Location = new System.Drawing.Point(15, 389);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(746, 14);
			this.lblDescription.TabIndex = 20;
			this.lblDescription.Text = "Please describe what you were doing when you encountered the error.  Can you tell" +
    " us how to reproduce the error? (5000 character limit)";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(15, 422);
			this.txtDescription.MaxLength = 5000;
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(935, 209);
			this.txtDescription.TabIndex = 21;
			// 
			// gbReportType
			// 
			this.gbReportType.Controls.Add(this.rbFeature);
			this.gbReportType.Controls.Add(this.rbError);
			this.gbReportType.Location = new System.Drawing.Point(101, 188);
			this.gbReportType.Name = "gbReportType";
			this.gbReportType.Size = new System.Drawing.Size(159, 75);
			this.gbReportType.TabIndex = 13;
			this.gbReportType.TabStop = false;
			// 
			// rbFeature
			// 
			this.rbFeature.AutoSize = true;
			this.rbFeature.Location = new System.Drawing.Point(16, 43);
			this.rbFeature.Name = "rbFeature";
			this.rbFeature.Size = new System.Drawing.Size(129, 18);
			this.rbFeature.TabIndex = 1;
			this.rbFeature.TabStop = true;
			this.rbFeature.Text = "Requested Feature";
			this.rbFeature.UseVisualStyleBackColor = true;
			// 
			// rbError
			// 
			this.rbError.AutoSize = true;
			this.rbError.Location = new System.Drawing.Point(16, 18);
			this.rbError.Name = "rbError";
			this.rbError.Size = new System.Drawing.Size(100, 18);
			this.rbError.TabIndex = 0;
			this.rbError.TabStop = true;
			this.rbError.Text = "Software Error";
			this.rbError.UseVisualStyleBackColor = true;
			this.rbError.CheckedChanged += new System.EventHandler(this.rbError_CheckedChanged);
			// 
			// gbSeverity
			// 
			this.gbSeverity.Controls.Add(this.rbBlocking);
			this.gbSeverity.Controls.Add(this.rbMajor);
			this.gbSeverity.Controls.Add(this.rbMinor);
			this.gbSeverity.Location = new System.Drawing.Point(506, 188);
			this.gbSeverity.Name = "gbSeverity";
			this.gbSeverity.Size = new System.Drawing.Size(436, 100);
			this.gbSeverity.TabIndex = 15;
			this.gbSeverity.TabStop = false;
			// 
			// rbBlocking
			// 
			this.rbBlocking.AutoSize = true;
			this.rbBlocking.Location = new System.Drawing.Point(10, 66);
			this.rbBlocking.Name = "rbBlocking";
			this.rbBlocking.Size = new System.Drawing.Size(329, 18);
			this.rbBlocking.TabIndex = 2;
			this.rbBlocking.TabStop = true;
			this.rbBlocking.Text = "Blocking - this issue prevents me from using BenMAP-CE";
			this.rbBlocking.UseVisualStyleBackColor = true;
			// 
			// rbMajor
			// 
			this.rbMajor.AutoSize = true;
			this.rbMajor.Location = new System.Drawing.Point(10, 42);
			this.rbMajor.Name = "rbMajor";
			this.rbMajor.Size = new System.Drawing.Size(389, 18);
			this.rbMajor.TabIndex = 1;
			this.rbMajor.TabStop = true;
			this.rbMajor.Text = "Major - this issue significantly hinders my ability to use BenMAP-CE";
			this.rbMajor.UseVisualStyleBackColor = true;
			// 
			// rbMinor
			// 
			this.rbMinor.AutoSize = true;
			this.rbMinor.Location = new System.Drawing.Point(10, 17);
			this.rbMinor.Name = "rbMinor";
			this.rbMinor.Size = new System.Drawing.Size(418, 18);
			this.rbMinor.TabIndex = 0;
			this.rbMinor.TabStop = true;
			this.rbMinor.Text = "Minor - this issue has little or no impact on my ability to use BenMAP-CE";
			this.rbMinor.UseVisualStyleBackColor = true;
			// 
			// lblErrorText
			// 
			this.lblErrorText.ForeColor = System.Drawing.Color.Red;
			this.lblErrorText.Location = new System.Drawing.Point(14, 41);
			this.lblErrorText.Name = "lblErrorText";
			this.lblErrorText.Size = new System.Drawing.Size(950, 13);
			this.lblErrorText.TabIndex = 1;
			this.lblErrorText.Text = "Error Text";
			this.lblErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(15, 98);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(123, 13);
			this.label8.TabIndex = 4;
			this.label8.Text = "BenMAP-CE Version";
			// 
			// txtBenMAPCEVersion
			// 
			this.txtBenMAPCEVersion.BackColor = System.Drawing.SystemColors.Control;
			this.txtBenMAPCEVersion.Location = new System.Drawing.Point(139, 95);
			this.txtBenMAPCEVersion.Name = "txtBenMAPCEVersion";
			this.txtBenMAPCEVersion.ReadOnly = true;
			this.txtBenMAPCEVersion.Size = new System.Drawing.Size(302, 22);
			this.txtBenMAPCEVersion.TabIndex = 5;
			this.txtBenMAPCEVersion.TabStop = false;
			// 
			// ErrorReporting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(971, 689);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.txtBenMAPCEVersion);
			this.Controls.Add(this.lblErrorText);
			this.Controls.Add(this.gbSeverity);
			this.Controls.Add(this.gbReportType);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.chkAuditTrail);
			this.Controls.Add(this.lblAuditTrail);
			this.Controls.Add(this.cboComponent);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.lblSeverity);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSubmit);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtCountry);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.txtOS);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "ErrorReporting";
			this.Text = "Provide Feedback";
			this.Shown += new System.EventHandler(this.ErrorReporting_Shown);
			this.gbReportType.ResumeLayout(false);
			this.gbReportType.PerformLayout();
			this.gbSeverity.ResumeLayout(false);
			this.gbSeverity.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOS;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtCountry;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnSubmit;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblSeverity;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cboComponent;
		private System.Windows.Forms.Label lblAuditTrail;
		private System.Windows.Forms.CheckBox chkAuditTrail;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.GroupBox gbReportType;
		private System.Windows.Forms.RadioButton rbFeature;
		private System.Windows.Forms.RadioButton rbError;
		private System.Windows.Forms.GroupBox gbSeverity;
		private System.Windows.Forms.RadioButton rbBlocking;
		private System.Windows.Forms.RadioButton rbMajor;
		private System.Windows.Forms.RadioButton rbMinor;
		private System.Windows.Forms.Label lblErrorText;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtBenMAPCEVersion;
	}
}