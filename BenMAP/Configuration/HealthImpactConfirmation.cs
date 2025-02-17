﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BenMAP
{
	public partial class HealthImpactConfirmation : FormBase
	{
		public HealthImpactConfirmation()
		{
			InitializeComponent();
		}
		public HealthImpactConfirmation(List<CRSelectFunction> crSelect)
		{
			InitializeComponent();
			_lstSelectFunctions = crSelect;
		}

		private List<CRSelectFunction> _lstSelectFunctions;

		private void HealthImpactConfirmation_Load(object sender, EventArgs e)
		{

			errorProvider1.SetError(label10, "Warning");
			this.errorProvider1.Icon = new Icon(SystemIcons.Warning, 48, 48);
			ESIL.DBUtility.FireBirdHelperBase fb = new ESIL.DBUtility.ESILFireBirdHelper();
			string commandText;

			var namePollutant = CommonClass.LstPollutant.First().PollutantName;
			var popYear = CommonClass.BenMAPPopulation.Year;

			int countMortality = 0;
			int countMorbidity = 0;
			bool diffYear = false;
			bool coarsePopulation = false;

			List<Tuple<string, int, int>> list = new List<Tuple<string, int, int>>();
			string endGroup, endPoint;
			string incidenceName;

			foreach (CRSelectFunction cr in _lstSelectFunctions)
			{
				endGroup = cr.BenMAPHealthImpactFunction.EndPointGroup;
				endPoint = cr.BenMAPHealthImpactFunction.EndPoint;
				incidenceName = cr.IncidenceDataSetName;

				if (endGroup.Contains("Mortality"))
					countMortality += 1;
				else
					countMorbidity += 1;

				if (!String.IsNullOrEmpty(incidenceName))
				{
					if (!incidenceName.Contains(popYear.ToString()))
						diffYear = true;
				}
				var pair = Tuple.Create(endPoint, cr.StartAge, cr.EndAge);
				list.Add(pair);

				//BenMAP 487: If a user selects a HIF with an incidence dataset, check that dataset against the population choice and notify user if the population data are coarser than the incidence data.
				if (cr.IncidenceDataSetID != -1)
				{
					//retrieve count of population age range IDs
					commandText = string.Format("SELECT COUNT (*) FROM (SELECT DISTINCT AGERANGEID FROM POPULATIONENTRIES WHERE POPULATIONDATASETID = {0})", CommonClass.BenMAPPopulation.DataSetID);
					var popEntries = fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, commandText);
					int popEntriesCount = -1;
					if (popEntries != null)
					{
						popEntriesCount = Convert.ToInt32(popEntries);

					}

					//retrieve grid definition of the HIF Incidence Dataset
					commandText = string.Format("select GRIDDEFINITIONID from INCIDENCEDATASETS where INCIDENCEDATASETID = {0}", cr.IncidenceDataSetID);
					var incidenceGrid = fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, commandText);
					int gridID = 0;
					if (incidenceGrid != null)
					{
						gridID = Convert.ToInt32(incidenceGrid);
					}

					//retrieve start and end ages of incidence dataset
					commandText = string.Format("SELECT COUNT(*) FROM (SELECT DISTINCT STARTAGE, ENDAGE from INCIDENCERATES where INCIDENCEDATASETID = {0} AND GRIDDEFINITIONID = {1} AND ENDPOINTGROUPID = {2} AND ENDPOINTID = {3})",
							cr.IncidenceDataSetID,
							gridID,
							cr.BenMAPHealthImpactFunction.EndPointGroupID,
							cr.BenMAPHealthImpactFunction.EndPointID
							);
					var incidenceEntries = fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, commandText);
					int incidenceEntriesCount = -1;
					if (incidenceEntries != null)
					{
						incidenceEntriesCount = Convert.ToInt32(incidenceEntries);
					}

					//check each population entry
					if (incidenceEntriesCount > popEntriesCount)
						coarsePopulation = true;
				}
			}

			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
			rtbResults.AppendText("• is assessing ");
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Bold);
			rtbResults.AppendText(namePollutant.ToString() + Environment.NewLine);
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
			rtbResults.AppendText("• is for year ");
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Bold);
			rtbResults.AppendText(popYear.ToString() + Environment.NewLine);
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
			rtbResults.AppendText("• is assessing ");
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Bold);
			rtbResults.AppendText(countMorbidity.ToString());
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
			rtbResults.AppendText(" morbidity functions" + Environment.NewLine);
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
			rtbResults.AppendText("• is assessing ");
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Bold);
			rtbResults.AppendText(countMortality.ToString());
			rtbResults.SelectionFont = new Font(rtbResults.Font, FontStyle.Regular);
			rtbResults.AppendText(" mortality functions" + Environment.NewLine);

			bool overlap = false;
			int count = 0;
			List<string> overlapGroup = new List<string>();         //Checking for overlapping age ranges, taking each item in the list of HIF and checking it against all following entries.

			foreach (Tuple<string, int, int> toCompare in list)
			{
				for (int i = count + 1; i < list.Count(); i++)
				{
					if (list[i].Item1 == toCompare.Item1)
						if (toCompare.Item2 == list[i].Item2 && toCompare.Item3 == list[i].Item3) //identical age ranges--handled in pooling step
						{
							continue;
						}
						else
						{
							if (toCompare.Item2 <= list[i].Item3 && list[i].Item2 <= toCompare.Item3)
							{
								overlap = true;
								if (!overlapGroup.Contains(toCompare.Item1))
									overlapGroup.Add(toCompare.Item1);
							}
						}
				}
				count++;
			}

			int aqGridID = CommonClass.LstBaseControlGroup.First().GridType.GridDefinitionID;
			int popGridID = CommonClass.BenMAPPopulation.GridType.GridDefinitionID;

			bool crosswalkNeeded = false;
			if (aqGridID != popGridID)
			{

				commandText = string.Format("select percentageid from griddefinitionpercentages where sourcegriddefinitionid={0} and targetgriddefinitionid='{1}'", aqGridID, popGridID);
				var percentageID = fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, commandText);

				if (percentageID == null)
					crosswalkNeeded = true;
			}

			bool reviewIssues = false;

			if (diffYear)
			{
				this.dgvIssuesFlagged.Rows.Add("Population and Incidence", "High", "Population and Incidence years differ");
				reviewIssues = true;
			}
			if (_lstSelectFunctions.Count >= 10)
			{
				this.dgvIssuesFlagged.Rows.Add("Number of Health Impact Functions", "Low", "May increase calculation time");
				reviewIssues = true;
			}
			if (overlap)
			{
				string endptJoined = string.Join(", ", overlapGroup);
				this.dgvIssuesFlagged.Rows.Add("Age Ranges", "Low", "The age ranges of one or more functions overlap within the following: " + endptJoined);
				reviewIssues = true;
			}
			if (HealthImpactFunctions.dailyAQmissing)
			{
				this.dgvIssuesFlagged.Rows.Add("Air Quality Data", "Low", "One or more selected health impact functions are configured to use daily or seasonal metrics. If air quality surfaces do not include daily data, the function will use the annual metric instead");
				reviewIssues = true;
			}
			if (crosswalkNeeded)
			{
				this.dgvIssuesFlagged.Rows.Add("New Crosswalk Needed", "Low", "Creating a new crosswalk will increase the calculation time");
				reviewIssues = true;
			}
			if(coarsePopulation)
			{
				this.dgvIssuesFlagged.Rows.Add("Population and Incidence", "High", "The program identified a potential issue with your analysis. Your population data are broken into age bins that do not match the age bins for your baseline incidence rates. If the population age bins and incidence age bins don't match, the program will distribute the population evenly within each age bin. This assumption may bias your results high or low. Please reference the user manual.");
				reviewIssues = true;
			}

			if (!reviewIssues)
			{
				errorProvider1.Clear();
				label11.Text = "";

				this.dgvIssuesFlagged.BackgroundColor = SystemColors.Control;
				this.dgvIssuesFlagged.ForeColor = SystemColors.Control;
				this.dgvIssuesFlagged.BorderStyle = BorderStyle.None;
				this.dgvIssuesFlagged.Columns["Issue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				this.dgvIssuesFlagged.Columns["Severity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				this.dgvIssuesFlagged.Columns["Comments"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				this.dgvIssuesFlagged.Columns["Issue"].Visible = false;
				this.dgvIssuesFlagged.Columns["Severity"].Visible = false;
				this.dgvIssuesFlagged.Columns["Comments"].Visible = false;
				this.dgvIssuesFlagged.Size = new System.Drawing.Size(5, 5);
				this.dgvIssuesFlagged.Location = new Point(12, 295);

				//this.tableLayoutPanel1.AutoSize = true;
				this.btnConfirm.Dock = DockStyle.Left;
				this.tableLayoutPanel1.Controls.Add(this.btnConfirm, 3, 4);
				this.btnCancel.Dock = DockStyle.Right;
				this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 4);
				this.Height = 225;
			}
			if (this.dgvIssuesFlagged.Rows.Count != 0)
				this.dgvIssuesFlagged.CurrentCell.Selected = false;
		}

	}
}
