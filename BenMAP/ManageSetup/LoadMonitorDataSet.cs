﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//LoadMonitorDataSet files should have been named LoadSelectedDataset.  Hind site is 20/20
//this is being used for any of the datasets that did not have a load dialog.  It was
//implamented to make most all manageing datasets simaler in the way they worked and
//to make the validation process as simaler as posible.
namespace BenMAP
{
	public partial class LoadSelectedDataSet : Form
	{
		private string _title = string.Empty;
		private string _datasetnamelabel = string.Empty;
		private string _datasetName = string.Empty;
		private string _dataset = string.Empty;
		private string _strPath;
		private string _isForceValidate = string.Empty;
		private string _iniPath = string.Empty;
		private DataTable _monitorDataset;
		private MetadataClassObj _metadataObj = null;

		private string _tabnameref = string.Empty;

		internal MetadataClassObj MetadataObj
		{
			get { return _metadataObj; }
		}
		public DataTable MonitorDataSet
		{
			get { return _monitorDataset; }
		}
		public string DatasetName
		{
			get { return _datasetName; }
		}
		public string DatasetNameLabel
		{
			get { return _datasetnamelabel; }
		}
		public string Title
		{
			get { return _title; }
		}
		public string StrPath
		{
			get { return _strPath; }
		}

		public LoadSelectedDataSet()
		{
			InitializeComponent();
			_iniPath = CommonClass.ResultFilePath + @"\BenMAP.ini";
			_isForceValidate = CommonClass.IniReadValue("appSettings", "IsForceValidate", _iniPath);
			if (_isForceValidate == "T")
			{
				btnOK.Enabled = false;
			}
			else
			{
				btnOK.Enabled = true;
			}

		}
		public LoadSelectedDataSet(string title, string datasetNamelabel, string datasetName, string dataset) : this()
		{
			_title = title;
			_datasetnamelabel = datasetNamelabel;
			_datasetName = datasetName;
			_dataset = dataset;
			this.Text = title;
			this.lblDataSetName.Text = datasetNamelabel;
			this.txtDataSetName.Text = datasetName;
			if (dataset.Equals("Baseline") || dataset.Equals("Control"))
			{
				//baseline and control data are not stored in the database,
				//it looks as if it is stored in the project file.  see the audit trail
				//under air quality data
				btnViewMetadata.Visible = false;
			}
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_strPath))
			{
				_monitorDataset = CommonClass.ExcelToDataTable(_strPath, _tabnameref);

				var rowsToDelete = new List<DataRow>();
				foreach (DataRow dr in _monitorDataset.Rows)            //BenMAP 441/442/444--Address error created when empty lines are passed from Excel--required cells show blank values
				{
					if (String.IsNullOrEmpty(dr[0].ToString()))
					{
						rowsToDelete.Add(dr);
					}
					dr[0].ToString().Trim();
					dr[1].ToString().Trim();
				}

				rowsToDelete.ForEach(x => _monitorDataset.Rows.Remove(x));
				this.DialogResult = DialogResult.OK;
			}
			else
			{
				MessageBox.Show("Please select a file");
				btnBrowse.Focus();
				return;
			}
		}
		private void GetMetadata()
		{
			_metadataObj = new MetadataClassObj();
			Metadata metadata = new Metadata(_strPath);
			_metadataObj = metadata.GetMetadata();
		}

		private void btnValidate_Click(object sender, EventArgs e)
		{
			_monitorDataset = CommonClass.ExcelToDataTable(_strPath, ref _tabnameref, null);

			ValidateDatabaseImport vdi = new ValidateDatabaseImport(_monitorDataset, _dataset, _strPath);//  (_monitorDataset, "Monitor", _strPath);

			DialogResult dlgR = vdi.ShowDialog();
			if (dlgR.Equals(DialogResult.OK))
			{
				//if(vdi.PassedValidation && _isForceValidate == "T")
				//    this.DialogResult = DialogResult.OK;
				btnOK.Enabled = true;

			}
			else
			{
				btnOK.Enabled = false;
			}
		}

		private void txtDatabase_TextChanged(object sender, EventArgs e)
		{
			btnValidate.Enabled = !string.IsNullOrEmpty(txtDatabase.Text);
			btnViewMetadata.Enabled = !string.IsNullOrEmpty(txtDatabase.Text);
			_strPath = txtDatabase.Text;
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog() { RestoreDirectory = true };
				openFileDialog.InitialDirectory = CommonClass.ResultFilePath;
				openFileDialog.Filter = "Supported File Types (*.csv, *.xls, *.xlsx, *.aqgx)|*.csv; *.xls; *.xlsx; *.aqgx|CSV files|*.csv|XLS files|*.xls|XLSX files|*.xlsx|AQGX files|*.aqgx";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;
				if (openFileDialog.ShowDialog() != DialogResult.OK)
				{ return; }
				txtDatabase.Text = openFileDialog.FileName;
				openFileDialog.RestoreDirectory = true;
				GetMetadata();
				_tabnameref = string.Empty;
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
			}
		}

		private void btnViewMetadata_Click(object sender, EventArgs e)
		{
			ViewEditMetadata viewEMdata = null;
			//_metadataObj.SetupName = txtDataSetName.Text;
			if (_metadataObj != null)
			{
				viewEMdata = new ViewEditMetadata(_strPath, _metadataObj);
			}
			else
			{
				viewEMdata = new ViewEditMetadata(_strPath);
			}
			DialogResult dr = viewEMdata.ShowDialog();
			if (dr.Equals(DialogResult.OK))
			{
				_metadataObj = viewEMdata.MetadataObj;
			}
		}
	}
}
