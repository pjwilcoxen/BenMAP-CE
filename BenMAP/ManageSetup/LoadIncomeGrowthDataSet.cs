using System;
using System.Data;
using System.Windows.Forms;
using ESIL.DBUtility;

namespace BenMAP
{
	public partial class LoadIncomeGrowthDataSet : FormBase
	{
		private DataTable _incomeGrowthData;
		public DataTable IncomeGrowthData
		{
			get { return _incomeGrowthData; }
		}
		private string _strPath;
		private string _isForceValidate = string.Empty;
		private string _iniPath = string.Empty;
		private const int INCOMEGROWTHDATASETTYPEID = 4;
		private MetadataClassObj _metadataObj = null;
		private string _tabnameref = string.Empty;

		public LoadIncomeGrowthDataSet()
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

		private void btnOK_Click(object sender, EventArgs e)
		{
			LoadDatabase();
		}

		private void LoadDatabase()
		{
			try
			{
				if (txtDatabase.Text == string.Empty)
				{
					MessageBox.Show("Please select a datafile.");
					return;
				}
				ESIL.DBUtility.FireBirdHelperBase fb = new ESIL.DBUtility.ESILFireBirdHelper();
				string commandText = string.Format("select IncomeGrowthadjdatasetid from IncomeGrowthadjdatasets where setupid={0} and IncomeGrowthadjdatasetname='{1}'", CommonClass.ManageSetup.SetupID, txtDataSetName.Text);
				object obj = fb.ExecuteScalar(CommonClass.Connection, new CommandType(), commandText);
				if (obj != null)
				{
					MessageBox.Show("This income growth dataset name is already in use. Please enter a different name.");
					return;
				}
				DataTable dt = new DataTable();
				string strfilename = string.Empty;
				string strtablename = string.Empty;
				commandText = string.Empty;

				//dt = CommonClass.ExcelToDataTable(txtDatabase.Text);
				dt = CommonClass.ExcelToDataTable(txtDatabase.Text, _tabnameref);
				int iYear = -1;
				int iMean = -1;
				int iEndpointGroup = -1;
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					switch (dt.Columns[i].ColumnName.ToLower().Replace(" ", ""))
					{
						case "year":
							iYear = i;
							break;
						case "mean":
							iMean = i;
							break;
						case "endpointgroup":
							iEndpointGroup = i;
							break;
					}
				}
				string warningtip = "";
				if (iYear < 0) warningtip = "'Year', ";
				if (iMean < 0) warningtip += "'Mean', ";
				if (iEndpointGroup < 0) warningtip += "'Endpoint Group', ";
				if (warningtip != "")
				{
					warningtip = warningtip.Substring(0, warningtip.Length - 2);
					warningtip = "Please check the column header of " + warningtip + ". It is incorrect or does not exist.\r\n";
					warningtip += "\r\nFile failed to load, please validate the file for a more detail explanation of errors.";
					MessageBox.Show(warningtip, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				commandText = "SELECT max(INCOMEGROWTHADJDATASETID) from INCOMEGROWTHADJDATASETS";
				int incomegrowthadjdatasetID = Convert.ToInt32(fb.ExecuteScalar(CommonClass.Connection, new CommandType(), commandText)) + 1;
				//The 'F' is for the locked column in incomegrowthandadjatests - this is being imported and is not predefined.
				commandText = string.Format("insert into INCOMEGROWTHADJDATASETS VALUES({0},{1},'{2}', 'F' )", incomegrowthadjdatasetID, CommonClass.ManageSetup.SetupID, txtDataSetName.Text);
				fb.ExecuteNonQuery(CommonClass.Connection, new CommandType(), commandText);
				int currentDataSetID = incomegrowthadjdatasetID;

				if (dt == null) { return; }
				int rtn = 0;
				foreach (DataRow row in dt.Rows)
				{
					if (row == null)
					{ continue; }
					commandText = string.Format("insert into INCOMEGROWTHADJFACTORS(INCOMEGROWTHADJDATASETID,YYEAR,MEAN,ENDPOINTGROUPS) values({0},{1},{2},'{3}')", currentDataSetID, int.Parse(row[iYear].ToString()), row[iMean], row[iEndpointGroup]);
					rtn = fb.ExecuteNonQuery(CommonClass.Connection, new CommandType(), commandText);
				}
				if (rtn != 0)
				{
					IncomeGrowthDataSetName = txtDataSetName.Text;

				}

				insertMetadata(currentDataSetID);
			}

			catch (Exception ex)
			{
				Logger.LogError(ex);
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void insertMetadata(int dataSetID)
		{

			_metadataObj.DatasetId = dataSetID;
			// 2015 09 23 BENMAP-350 - hard coded dataset type id, as it was defaulting to zero. This prevented proper loading of metadata
			//_metadataObj.DatasetTypeId = SQLStatementsCommonClass.getDatasetID("Incomegrowth");
			_metadataObj.DatasetTypeId = INCOMEGROWTHDATASETTYPEID;
			if (!SQLStatementsCommonClass.insertMetadata(_metadataObj))
			{
				MessageBox.Show("Failed to save Metadata.");
			}
		}
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.InitialDirectory = CommonClass.ResultFilePath;
				openFileDialog.Filter = "Supported File Types (*.csv, *.xls, *.xlsx)|*.csv; *.xls; *.xlsx|CSV files|*.csv|XLS files|*.xls|XLSX files|*.xlsx";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;
				if (openFileDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				_tabnameref = string.Empty; //forget tab name if users select a different file after validation
				txtDatabase.Text = openFileDialog.FileName;
				GetMetadata();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex);
			}
		}

		private void GetMetadata()
		{
			_metadataObj = new MetadataClassObj();
			Metadata metadata = new Metadata(_strPath);
			_metadataObj = metadata.GetMetadata();
		}

		private string _incomeGrowthDataSetName;

		public string IncomeGrowthDataSetName
		{
			get { return _incomeGrowthDataSetName; }
			set { _incomeGrowthDataSetName = value; }
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void LoadIncomeGrowthDataSet_Load(object sender, EventArgs e)
		{
			try
			{
				FireBirdHelperBase fb = new ESILFireBirdHelper();
				if (_incomeGrowthDataSetName == null)
				{
					int number = 0;
					int incomeGrowthDatasetID = 0;
					do
					{
						string comText = "select incomeGrowthADJDatasetID from incomeGrowthADJDataSets where incomeGrowthADJDatasetName=" + "'IncomeGrowthDataSet" + Convert.ToString(number) + "'";
						incomeGrowthDatasetID = Convert.ToInt16(fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, comText));
						number++;
					} while (incomeGrowthDatasetID > 0);
					txtDataSetName.Text = "IncomeGrowthDataSet" + Convert.ToString(number - 1);
				}
			}
			catch (Exception)
			{ }
		}

		private void btnValidate_Click(object sender, EventArgs e)
		{
			//_incomeGrowthData = CommonClass.ExcelToDataTable(_strPath);
			_incomeGrowthData = CommonClass.ExcelToDataTable(_strPath, ref _tabnameref, null);
			ValidateDatabaseImport vdi = new ValidateDatabaseImport(_incomeGrowthData, "Incomegrowth", _strPath);

			DialogResult dlgR = vdi.ShowDialog();
			// 2015 09 24 - BENMAP349 attempt to fix problem with autoload on validation
			if (dlgR.Equals(DialogResult.OK))
			{
				if (vdi.PassedValidation && _isForceValidate == "T")
				{
					// LoadDatabase();
					// 2015 09 28 BENMAP-351 - fix OK button not enabled on successful validation
					btnOK.Enabled = true;   // if OK, then enable loading
				}
				else
				{
					btnOK.Enabled = false;
				}
			}
		}

		private void txtDatabase_TextChanged(object sender, EventArgs e)
		{
			btnValidate.Enabled = !string.IsNullOrEmpty(txtDatabase.Text);
			btnViewMetadata.Enabled = !string.IsNullOrEmpty(txtDatabase.Text);
			_strPath = txtDatabase.Text;
		}

		private void btnViewMetadata_Click(object sender, EventArgs e)
		{
			ViewEditMetadata viewEMdata = null;
			if (_metadataObj != null)
			{
				viewEMdata = new ViewEditMetadata(_strPath, _metadataObj);
			}
			else
			{
				viewEMdata = new ViewEditMetadata(_strPath);
			}
			viewEMdata.ShowDialog();
			_metadataObj = viewEMdata.MetadataObj;
		}
	}
}