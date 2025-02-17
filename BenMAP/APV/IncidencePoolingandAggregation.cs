using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Collections;
using BenMAP.APVX;
using System.IO;

namespace BenMAP
{
	public partial class IncidencePoolingandAggregation : FormBase
	{

		private int _operationStatus;
		public int OperationStatus
		{
			get { return _operationStatus; }
			set
			{
				_operationStatus = value;
			}
		}
		public Dictionary<string, int> _dicPoolingWindowOperation = new Dictionary<string, int>();
		public IncidencePoolingandAggregation()
		{
			InitializeComponent();
			this.tabControlSelected.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControlSelected.DrawItem += new DrawItemEventHandler(tabControlSelected_DrawItem);

			this.tabControlSelected.AllowDrop = true;
		}
		private List<CRSelectFunctionCalculateValue> lstSelectCRSelectFunctionCalculateValue = new List<CRSelectFunctionCalculateValue>();
		private Dictionary<string, List<CRSelectFunctionCalculateValue>> dicTabCR = new Dictionary<string, List<CRSelectFunctionCalculateValue>>();
		private bool isSelectTile = false;
		public List<IncidencePoolingAndAggregation> lstIncidencePoolingAndAggregationOld;
		private Tools.IncidenceBusinessCardRenderer incidenceBusinessCardRenderer = new Tools.IncidenceBusinessCardRenderer();
		private List<int> lstExists = new List<int>();
		private void IncidencePoolingandAggregation_Load(object sender, EventArgs e)
		{

			//CommonClass.SetupOLVEmptyListOverlay(this.olvTile.EmptyListMsgOverlay as BrightIdeasSoftware.TextOverlay);//YY: olvTile is not in use any more.
			CommonClass.SetupOLVEmptyListOverlay(this.treeListView.EmptyListMsgOverlay as BrightIdeasSoftware.TextOverlay);
			try
			{
				_operationStatus = 0;
				if ((CommonClass.LstDelCRFunction == null || CommonClass.LstDelCRFunction.Count == 0) && (CommonClass.LstUpdateCRFunction == null || CommonClass.LstUpdateCRFunction.Count == 0))
				{
					btnShowChanges.Enabled = false;
				}
				else
				{
					btnShowChanges.Enabled = true;
					this.toolTip1.SetToolTip(btnShowChanges, "Please resolve conflicts between the pooling and configuration files.");
				}
				if (CommonClass.BaseControlCRSelectFunctionCalculateValue != null && CommonClass.BaseControlCRSelectFunctionCalculateValue.lstCRSelectFunctionCalculateValue != null)
				{
					if (CommonClass.ValuationMethodPoolingAndAggregation != null)
					{
						txtOpenExistingCFGR.Text = CommonClass.ValuationMethodPoolingAndAggregation.CFGRPath;
					}
					splitContainerTile.Panel2.Hide(); //YY: Panel2 should always be hidden as we do not use olvTile anymore.
					splitContainerTile.SplitterDistance = splitContainerTile.Width;

					//YY: No need to prepare columns for olvTile
					//int iColumns = 0;
					//foreach (OLVColumn olvc in treeListView.Columns)
					//{

					//    BrightIdeasSoftware.OLVColumn olvcTileColumn = new OLVColumn();

					//    olvcTileColumn.Text = olvc.Text;
					//    olvcTileColumn.AspectName = olvc.AspectName;
					//    if (iColumns <= 5)
					//    {
					//        olvcTileColumn.IsTileViewColumn = true;
					//    }
					//    else
					//        olvcTileColumn.IsVisible = false;
					//    olvTile.AllColumns.Add(olvcTileColumn);

					//    iColumns++;
					//}
					/*                    tabControlSelected.TabPages.Clear();
															tabControlSelected.TabPages.Add("PoolingWindow0", "PoolingWindow0");
															tabControlSelected.TabPages[0].Controls.Add(this.treeListView);
					*/
					this.olvAvailable.SetObjects(CommonClass.BaseControlCRSelectFunctionCalculateValue.lstCRSelectFunctionCalculateValue);
					TypedObjectListView<CRSelectFunctionCalculateValue> tlist = new TypedObjectListView<CRSelectFunctionCalculateValue>(this.olvAvailable);
					tlist.GenerateAspectGetters();
					//this.olvAvailable.TileSize = new Size(120, 90);
					//this.olvTile.TileSize = new Size(120, 110);
					this.olvAvailable.ItemRenderer = incidenceBusinessCardRenderer;
					//this.olvTile.ItemRenderer = new Tools.IncidenceBusinessCardRenderer();
					olvAvailable.OwnerDraw = true;
					//this.olvTile.OwnerDraw = true;
					this.olvAvailable.DropSink = new IncidenceDropSink(true, this);
					this.treeListView.DropSink = new IncidenceDropSink(true, this);
					if (CommonClass.lstIncidencePoolingAndAggregation != null && CommonClass.lstIncidencePoolingAndAggregation.Count > 0)
					{
						lstIncidencePoolingAndAggregationOld = new List<IncidencePoolingAndAggregation>();
						foreach (IncidencePoolingAndAggregation ip in CommonClass.lstIncidencePoolingAndAggregation)
						{
							IncidencePoolingAndAggregation ipold = new IncidencePoolingAndAggregation();
							ipold.ConfigurationResultsFilePath = ip.ConfigurationResultsFilePath;
							if (ip.lstAllSelectCRFuntion != null)
							{
								ipold.lstAllSelectCRFuntion = new List<AllSelectCRFunction>();
								foreach (AllSelectCRFunction ascr in ip.lstAllSelectCRFuntion)
								{
									ipold.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
									{
										Author = ascr.Author,
										AgeRange = ascr.AgeRange, //YY: new
										CRID = ascr.CRID,
										CRIndex = ascr.CRIndex,
										CRSelectFunctionCalculateValue = ascr.CRSelectFunctionCalculateValue,
										ChildCount = ascr.ChildCount,//YY: new
										CountStudies = ascr.CountStudies, //YY: new
										DataSet = ascr.DataSet,
										EndAge = ascr.EndAge,
										EndPoint = ascr.EndPoint,
										EndPointGroup = ascr.EndPointGroup,
										EndPointGroupID = ascr.EndPointGroupID,
										EndPointID = ascr.EndPointID,
										Ethnicity = ascr.Ethnicity,
										Function = ascr.Function,
										Gender = ascr.Gender,
										ID = ascr.ID,
										Location = ascr.Location,
										GeographicArea = ascr.GeographicArea,
										Metric = ascr.Metric,
										MetricStatistic = ascr.MetricStatistic,
										Name = ascr.Name,
										Nickname = ascr.Nickname, //YY: new
										NodeType = ascr.NodeType,
										OtherPollutants = ascr.OtherPollutants,
										PID = ascr.PID,
										Pollutant = ascr.Pollutant,
										PoolingMethod = ascr.PoolingMethod,
										Qualifier = ascr.Qualifier,
										Race = ascr.Race,
										SeasonalMetric = ascr.SeasonalMetric,
										StartAge = ascr.StartAge,
										Version = ascr.Version,
										Weight = ascr.Weight,
										Year = ascr.Year
									});
								}

							}
							if (ip.lstColumns != null)
							{
								ipold.lstColumns = new List<string>();
								foreach (string s in ip.lstColumns)
								{
									ipold.lstColumns.Add(s);
								}
							}
							ipold.VariableDataset = ip.VariableDataset;
							if (ip.Weights != null)
							{
								ipold.Weights = new List<double>();
								ipold.Weights.AddRange(ip.Weights);
							}
							if (ip.PoolLevel >= 0) //make -9 the default value?
							{
								ipold.PoolLevel = ip.PoolLevel;
							}
							//else
							//{
							//    ipold.PoolLevel = 3; //YY: ??? how to handle old apvx files?
							//}
							ipold.PoolingName = ip.PoolingName;
							lstIncidencePoolingAndAggregationOld.Add(ipold);

						}
						dicTabCR = new Dictionary<string, List<CRSelectFunctionCalculateValue>>();

						tabControlSelected.TabPages.Clear();
						tabControlSelected.TabPages.Add(CommonClass.lstIncidencePoolingAndAggregation.First().PoolingName, CommonClass.lstIncidencePoolingAndAggregation.First().PoolingName);
						tabControlSelected.TabPages[0].Controls.Add(this.treeListView);
						cbPoolLevel.SelectedItem = CommonClass.lstIncidencePoolingAndAggregation.First().PoolLevel.ToString();//YY: pop cbPoolLevel


						foreach (IncidencePoolingAndAggregation ip in CommonClass.lstIncidencePoolingAndAggregation)
						{
							//int widthWeight = 0;
							if (!istabControlSelectedContainText(ip.PoolingName))
							{
								tabControlSelected.TabPages.Add(ip.PoolingName);

							}
							if (ip.lstAllSelectCRFuntion != null && ip.lstAllSelectCRFuntion.Count > 0)
								dicTabCR.Add(ip.PoolingName, ip.lstAllSelectCRFuntion.Where(p => p.CRSelectFunctionCalculateValue != null && p.CRID < 9999).Select(a => a.CRSelectFunctionCalculateValue).ToList());
							else
								dicTabCR.Add(ip.PoolingName, null);

							//if (ip.lstAllSelectCRFuntion.Select(p => p.PoolingMethod).Contains("User Defined Weights"))
							//{
							//    widthWeight = 60;
							//}
							//OLVColumn weightColumn = treeListView.AllColumns[2];
							//if (weightColumn.Width != widthWeight)
							//{
							//    weightColumn.Width = widthWeight;
							//    treeListView.RebuildColumns();
							//}

						}
						if (CommonClass.ValuationMethodPoolingAndAggregation != null && CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase != null
								&& CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Count > 0)
						{
							foreach (ValuationMethodPoolingAndAggregationBase vb in CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase)
							{
								try
								{
									IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == vb.IncidencePoolingAndAggregation.PoolingName).First();
									//YY: use ipCopy as vb.IncidencePoolingAndAggregation can't share the same reference address as ip
									IncidencePoolingAndAggregation ipCopy = CommonClassExtension.DeepClone(ip);




									//vb.IncidencePoolingAndAggregation = ip;
									vb.IncidencePoolingAndAggregation = ipCopy;
								}
								catch
								{
								}
							}
						}

					}
					else
					{
						CommonClass.lstIncidencePoolingAndAggregation = new List<IncidencePoolingAndAggregation>();
						tabControlSelected.TabPages.Clear();
						foreach (CRSelectFunctionCalculateValue o in CommonClass.BaseControlCRSelectFunctionCalculateValue.lstCRSelectFunctionCalculateValue)
						{
							if (!tabControlSelected.TabPages.ContainsKey(o.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
							{
								tabControlSelected.TabPages.Add(o.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup, o.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup);
								CommonClass.lstIncidencePoolingAndAggregation.Add(new IncidencePoolingAndAggregation()
								{
									PoolingName = o.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup
								});
							}
						}
						tabControlSelected.TabPages[0].Controls.Add(this.treeListView);
						cbPoolLevel.SelectedItem = "3"; //YY: Set default value.

					}

					//YY: add an unselectable tab to the end with "+" as the name
					tabControlSelected.TabPages.Add("+");

					tabControlSelected.SelectedIndex = 0;
					tabControlSelected_SelectedIndexChanged(sender, e);
					tbPoolingName.Text = tabControlSelected.TabPages[0].Text;

				}

				this.treeListView.CheckBoxes = false;
				List<CRSelectFunctionCalculateValue> lstAvailable = (List<CRSelectFunctionCalculateValue>)this.olvAvailable.Objects;
				Dictionary<string, int> DicFilterDataSet = new Dictionary<string, int>();
				DicFilterDataSet.Add("", -1);
				var query = from a in lstAvailable select new { a.CRSelectFunction.BenMAPHealthImpactFunction.DataSetName, a.CRSelectFunction.BenMAPHealthImpactFunction.DataSetID };
				if (query != null && query.Count() > 0)
				{
					List<KeyValuePair<string, int>> lstFilterDataSet = DicFilterDataSet.ToList();
					lstFilterDataSet.AddRange(query.Distinct().ToDictionary(p => p.DataSetName, p => p.DataSetID));
					DicFilterDataSet = lstFilterDataSet.ToDictionary(p => p.Key, p => p.Value);
				}

				//prepare "Filter Dataset" dropdown
				BindingSource bs = new BindingSource();

				bs.DataSource = DicFilterDataSet;
				this.cbDataSet.DataSource = bs;
				cbDataSet.DisplayMember = "Key";
				cbDataSet.ValueMember = "Value";
				int maxDatasetWidth = 166;
				int DatasetWidth = 166;
				foreach (var q in DicFilterDataSet)
				{
					using (Graphics g = this.CreateGraphics())
					{
						SizeF string_size = g.MeasureString(q.Key.ToString(), this.Font);
						DatasetWidth = Convert.ToInt16(string_size.Width) + 50;
					}
					maxDatasetWidth = Math.Max(maxDatasetWidth, DatasetWidth);
				}
				cbDataSet.DropDownWidth = maxDatasetWidth;
				//prepare "Filter Endpoint Group" dropdown
				Dictionary<string, int> DicFilterGroup = new Dictionary<string, int>();
				DicFilterGroup.Add("", -1);
				var queryGroup = from a in lstAvailable select new { a.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup, a.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID };
				if (queryGroup != null && queryGroup.Count() > 0)
				{
					List<KeyValuePair<string, int>> lstGroup = DicFilterGroup.ToList();
					lstGroup.AddRange(queryGroup.Distinct().ToDictionary(p => p.EndPointGroup, p => p.EndPointGroupID));
					DicFilterGroup = lstGroup.ToDictionary(p => p.Key, p => p.Value);
				}
				BindingSource bsqueryGroup = new BindingSource();

				bsqueryGroup.DataSource = DicFilterGroup;
				cbEndPointGroup.DataSource = bsqueryGroup;
				cbEndPointGroup.DisplayMember = "Key";
				cbEndPointGroup.ValueMember = "Value";
				int maxEndpointGroupWidth = 154;
				int EndpointGroupWidth = 154;
				foreach (var q in DicFilterGroup)
				{
					using (Graphics g = this.CreateGraphics())
					{
						SizeF string_size = g.MeasureString(q.Key.ToString(), this.Font);
						EndpointGroupWidth = Convert.ToInt16(string_size.Width) + 50;
					}
					maxEndpointGroupWidth = Math.Max(maxEndpointGroupWidth, EndpointGroupWidth);
				}
				cbEndPointGroup.DropDownWidth = maxEndpointGroupWidth;

				//Setup treeview structure
				this.treeListView.CanExpandGetter = delegate (object x)
				{
					try
					{
						AllSelectCRFunction dir = (AllSelectCRFunction)x;
						IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
						if (ip.lstAllSelectCRFuntion != null &&  ip.lstAllSelectCRFuntion.Where(p => p.PID == dir.ID).Count() > 0)
							return true;
						else
							return false;
					}
					catch
					{
						return false;
					}
				};
				this.treeListView.ChildrenGetter = delegate (object x)
				{
					AllSelectCRFunction dir = (AllSelectCRFunction)x;
					try
					{
						return getChildFromAllSelectCRFunction(dir);


					}
					catch (UnauthorizedAccessException ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return new List<AllSelectCRFunction>();
					}
				};

				if (CommonClass.lstIncidencePoolingAndAggregation.First() != null && CommonClass.lstIncidencePoolingAndAggregation.First().lstAllSelectCRFuntion != null)
				{

					if (CommonClass.lstIncidencePoolingAndAggregation.First().lstColumns != null && CommonClass.lstIncidencePoolingAndAggregation.First().lstColumns.Count > 0)
					{
						updateTreeColumns(ref CommonClass.lstIncidencePoolingAndAggregation.First().lstColumns);
					}
					else
					{
						updateTreeColumns(ref CommonClass.lstIncidencePoolingAndAggregation.First().lstColumns);

					}
					initTreeView(CommonClass.lstIncidencePoolingAndAggregation.First());
				}


			}
			catch (Exception ex)
			{

			}


		}
		private List<AllSelectCRFunction> getChildFromAllSelectCRFunction(AllSelectCRFunction allSelectValuationMethod)
		{
			try
			{
				List<AllSelectCRFunction> lstAll = new List<AllSelectCRFunction>();
				IncidencePoolingAndAggregation incidencePoolingAndAggregation = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();

				var query = from a in incidencePoolingAndAggregation.lstAllSelectCRFuntion where a.PID == allSelectValuationMethod.ID select a;
				lstAll = query.ToList();

				return lstAll;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public static void getAllChildFromAllSelectCRFunction(AllSelectCRFunction allSelectValuationMethod, List<AllSelectCRFunction> lstAll, ref List<AllSelectCRFunction> lstAllChild)
		{
			try
			{

				if (lstAllChild == null) lstAllChild = new List<AllSelectCRFunction>();
				List<AllSelectCRFunction> lst = lstAll.Where(p => p.PID == allSelectValuationMethod.ID).ToList();
				if (lst.Count() > 0) lstAllChild.AddRange(lst);
				foreach (AllSelectCRFunction ac in lst)
				{
					if (ac.PoolingMethod != "" || ac.NodeType != 100) //YY:
					{
						getAllChildFromAllSelectCRFunction(ac, lstAll, ref lstAllChild);

					}
				}


			}
			catch (Exception ex)
			{

			}
		}

		private void initTreeView(IncidencePoolingAndAggregation incidencePoolingAndAggregation)
		{


			List<AllSelectCRFunction> lstRoot = new List<AllSelectCRFunction>();
			if (incidencePoolingAndAggregation.lstAllSelectCRFuntion == null || incidencePoolingAndAggregation.lstAllSelectCRFuntion.Count == 0)
			{
				incidencePoolingAndAggregation.lstAllSelectCRFuntion = new List<AllSelectCRFunction>();
				treeListView.Objects = null;
			}
			else
			{
				//order all the HIF by their endpoint group so that the subsequent for loop only adds each endpoint group once
				List<AllSelectCRFunction> sortedIP = incidencePoolingAndAggregation.lstAllSelectCRFuntion
						.OrderBy(x => x.EndPointGroup)
						.ToList();
				lstRoot.Add(sortedIP.First());

				for (int i = 1; i < sortedIP.Count(); i++)
				{
					if (sortedIP[i].EndPointGroup != sortedIP[i - 1].EndPointGroup)
						lstRoot.Add(sortedIP[i]);
				}
				treeListView.Roots = lstRoot;
				//Change tree line colour from blue(default) to grey.
				((TreeListView.TreeRenderer)treeListView.TreeColumnRenderer).LinePen = Pens.Gray;

				//ImageGetter delegate simply returns the index of the image that should be drawn against the cell.
				this.treeColumnName.ImageGetter = delegate (object x)
{
if (((AllSelectCRFunction)x).NodeType == 100)
			//return 1; // 1-- old image for end node (study) in imageList1
			return 2; // 2-- new image for end node (study) in imageList1
		else
return 0; // 0-- image for folder node (group) in imageList1
	};
				treeListView.ExpandAll();
				treeListView.RebuildAll(true);
			}
			treeListView.RebuildAll(true);
			treeListView.ExpandAll();

			//add image to pooling columns
			int poolLevel = incidencePoolingAndAggregation.PoolLevel;
			foreach (OLVColumn olvColumn in treeListView.Columns)
			{
				if (olvColumn.DisplayIndex < poolLevel + 4 && olvColumn.DisplayIndex >= 4) //YY: 4 columns included new added column
				{
					if (olvColumn.DisplayIndex == 4)
					{
						olvColumn.HeaderImageKey = "headerP1";
					}
					else if (olvColumn.DisplayIndex == 5)
					{
						olvColumn.HeaderImageKey = "headerP2";
					}
					else if (olvColumn.DisplayIndex == 6)
					{
						olvColumn.HeaderImageKey = "headerP3";
					}
						;
				}
				else if (olvColumn.HeaderImageKey != "headerE")
				{
					olvColumn.HeaderImageKey = "";
				}
			}
		}
		private void btnAdvanced_Click(object sender, EventArgs e)
		{
			try
			{
				if (CommonClass.IncidencePoolingAndAggregationAdvance == null) CommonClass.IncidencePoolingAndAggregationAdvance = new IncidencePoolingAndAggregationAdvance();
				APVConfigurationAdvancedSettings frm = new APVConfigurationAdvancedSettings();
				frm.AdvanceOptionType(1);
				frm.IncidencePoolingAndAggregationAdvance = CommonClass.IncidencePoolingAndAggregationAdvance; DialogResult rtn = frm.ShowDialog();
				if (rtn != DialogResult.OK) { return; }

				CommonClass.IncidencePoolingAndAggregationAdvance = frm.IncidencePoolingAndAggregationAdvance;

			}
			catch (Exception ex)
			{
				Logger.LogError(ex);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CommonClass.lstIncidencePoolingAndAggregation = lstIncidencePoolingAndAggregationOld;
			this.DialogResult = DialogResult.Cancel;
		}

		private string AsyncLoadCFGR(string strFile)
		{

			try
			{
				if (CommonClass.BaseControlCRSelectFunctionCalculateValue == null || CommonClass.BaseControlCRSelectFunctionCalculateValue.lstCRSelectFunctionCalculateValue.Count == 0 ||
				 CommonClass.BaseControlCRSelectFunctionCalculateValue.lstCRSelectFunctionCalculateValue.First().CRCalculateValues == null ||
					CommonClass.BaseControlCRSelectFunctionCalculateValue.lstCRSelectFunctionCalculateValue.First().CRCalculateValues.Count == 0)
				{
					string tip = "Creating Incidence Pooling And Aggregation. Please wait.";
					WaitShow(tip);
					string err = "";
					CommonClass.BaseControlCRSelectFunctionCalculateValue = Configuration.ConfigurationCommonClass.LoadCFGRFile(strFile, ref err); if (CommonClass.BaseControlCRSelectFunctionCalculateValue == null)
					{
						MessageBox.Show(err);
						return "";
					}
					CommonClass.ValuationMethodPoolingAndAggregation.BaseControlCRSelectFunctionCalculateValue = CommonClass.BaseControlCRSelectFunctionCalculateValue;
					foreach (ValuationMethodPoolingAndAggregationBase vb in CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase)
					{
						foreach (AllSelectCRFunction alsc in vb.IncidencePoolingAndAggregation.lstAllSelectCRFuntion.Where(p => p.NodeType == 100).ToList()) //YY: original p.PoolingMethod == ""
						{
							if (alsc.NodeType == 100)
							{
								alsc.CRSelectFunctionCalculateValue = CommonClass.BaseControlCRSelectFunctionCalculateValue.lstCRSelectFunctionCalculateValue.Where(p => p.CRSelectFunction.CRID == alsc.CRID).First();
							}
						}
					}


				}
				WaitClose();
				return "";
			}
			catch (Exception ex)
			{
				WaitClose();
				Logger.LogError(ex);
				return "";

			}
		}
		public delegate string AsyncCFGRDelegate(string strFile);

		private void btnNext_Click(object sender, EventArgs e)
		{
			//show pooling preview window (unless supressed by user)
			PoolingPreview pp = new PoolingPreview();
			string iniPath = CommonClass.ResultFilePath + @"\BenMAP.ini";
			string isShow = "T";
			if (System.IO.File.Exists(iniPath))
			{
				isShow = CommonClass.IniReadValue("appSettings", "IsShowPoolPreview", iniPath);
			}
			if (isShow == "T")
			{
				DialogResult rtn;
				rtn = pp.ShowDialog();
			}


			try
			{
				string msg = string.Empty;
				string tip = string.Empty;
				DialogResult rtn;
				MessageForm messageBox;
				if ((CommonClass.LstDelCRFunction != null && CommonClass.LstDelCRFunction.Count > 0) || (CommonClass.LstUpdateCRFunction != null && CommonClass.LstUpdateCRFunction.Count > 0))
				{
					messageBox = new MessageForm(1);
					messageBox.Message = "This APV pools studies not found in the CFGRX. BenMAP may not produce correctly pooled results."; messageBox.Text = "Continue or back?";
					messageBox.BTNOneText = "Continue";
					messageBox.BTNThirdText = "Back to check";
					messageBox.SetFirstButton();
					messageBox.SetLabel(697, 40);
					messageBox.Size = new System.Drawing.Size(710, 139);
					messageBox.FirstButtonLocation = new Point(490, messageBox.FirstButtonLocation.Y);
					messageBox.LabelLocation = new Point(7, 20);
					rtn = messageBox.ShowDialog(); if (rtn == DialogResult.Cancel)
					{ return; }

				}
				if (treeListView.Objects == null)
				{
					MessageBox.Show("No pooling method is available.");
					return;
				}
				else if (tabControlSelected.SelectedIndex > -1)
				{
					IncidencePoolingAndAggregation incidencePoolingAndAggregation = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
					if (incidencePoolingAndAggregation.lstColumns == null || incidencePoolingAndAggregation.lstColumns.Count() == 0)
					{
						updateTreeColumns(ref incidencePoolingAndAggregation.lstColumns);
					}

				}
				foreach (IncidencePoolingAndAggregation ip in CommonClass.lstIncidencePoolingAndAggregation)
				{
					if (ip.lstAllSelectCRFuntion == null || ip.lstAllSelectCRFuntion.Count == 0)
					{
						MessageBox.Show("Please set up all pooling windows first.");

						return;

					}

				}

				//start passing incidence pooling settings to valuation window

				//Save current setting in lstIncidencePoolingAndAggregationOld
				lstIncidencePoolingAndAggregationOld = new List<IncidencePoolingAndAggregation>();
				foreach (IncidencePoolingAndAggregation ip in CommonClass.lstIncidencePoolingAndAggregation)
				{
					IncidencePoolingAndAggregation ipold = new IncidencePoolingAndAggregation();
					ipold.ConfigurationResultsFilePath = ip.ConfigurationResultsFilePath;
					if (ip.lstAllSelectCRFuntion != null)
					{
						ipold.lstAllSelectCRFuntion = new List<AllSelectCRFunction>();
						foreach (AllSelectCRFunction ascr in ip.lstAllSelectCRFuntion)
						{
							ipold.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
							{
								Author = ascr.Author,
								AgeRange = ascr.AgeRange, //YY: new
								CRID = ascr.CRID,
								CRIndex = ascr.CRIndex,
								CRSelectFunctionCalculateValue = ascr.CRSelectFunctionCalculateValue,
								ChildCount = ascr.ChildCount, //YY: new
								CountStudies = ascr.CountStudies, //YY: new
								DataSet = ascr.DataSet,
								EndAge = ascr.EndAge,
								EndPoint = ascr.EndPoint,
								EndPointGroup = ascr.EndPointGroup,
								EndPointGroupID = ascr.EndPointGroupID,
								EndPointID = ascr.EndPointID,
								Ethnicity = ascr.Ethnicity,
								Function = ascr.Function,
								Gender = ascr.Gender,
								ID = ascr.ID,
								Location = ascr.Location,
								GeographicArea = ascr.GeographicArea,
								Metric = ascr.Metric,
								MetricStatistic = ascr.MetricStatistic,
								Name = ascr.Name,
								Nickname = ascr.Nickname, //YY: new
								NodeType = ascr.NodeType,
								OtherPollutants = ascr.OtherPollutants,
								PID = ascr.PID,
								Pollutant = ascr.Pollutant,
								PoolingMethod = ascr.PoolingMethod,
								Qualifier = ascr.Qualifier,
								Race = ascr.Race,
								SeasonalMetric = ascr.SeasonalMetric,
								StartAge = ascr.StartAge,
								Version = ascr.Version,
								Weight = ascr.Weight,
								Year = ascr.Year,


							});
						}

					}
					if (ip.lstColumns != null)
					{
						ipold.lstColumns = new List<string>();
						foreach (string s in ip.lstColumns)
						{
							ipold.lstColumns.Add(s);
						}
					}
					ipold.VariableDataset = ip.VariableDataset;
					if (ip.Weights != null)
					{
						ipold.Weights = new List<double>();
						ipold.Weights.AddRange(ip.Weights);
					}
					ipold.PoolingName = ip.PoolingName;
					ipold.PoolLevel = ip.PoolLevel; //YY: added 20191121

					lstIncidencePoolingAndAggregationOld.Add(ipold);

				}

				//make sure each pooling in lstIncidencePoolingAndAggregation has lstColumns. 
				//This step may not be necessary as it's already checked at the begining of this function.
				foreach (IncidencePoolingAndAggregation ipTmp in CommonClass.lstIncidencePoolingAndAggregation)
				{
					if (ipTmp.lstColumns == null || ipTmp.lstColumns.Count() == 0)
					{
						updateTreeColumns(ref ipTmp.lstColumns);
					}
					/* This is obsolete now that user defined weights are set in this screen
					if (ipTmp.lstAllSelectCRFuntion.Select(p => p.PoolingMethod).Contains("User Defined Weights"))
					{
							SelectSubjectiveWeight frmAPV = new SelectSubjectiveWeight(ipTmp);
							DialogResult rtnAPV = frmAPV.ShowDialog();
							if (rtnAPV != DialogResult.OK) { return; }
							ipTmp.Weights = frmAPV.dicAllWeight.Values.ToList();
					}
					*/
				}
				if (txtOpenExistingCFGR.Text != "")
				{
					AsyncLoadCFGR(txtOpenExistingCFGR.Text);
				}
				else
				{



				}

				msg = "The HIF studies or pooling methods may have been changed. If the HIF studies are not consistent with the pooling setup, incorrect results may be generated.";
				tip = "Load the previous valuation settings? ";
				if (CommonClass.ValuationMethodPoolingAndAggregation == null) { CommonClass.ValuationMethodPoolingAndAggregation = new ValuationMethodPoolingAndAggregation(); }
				else if (_operationStatus != 0)
				{

					messageBox = new MessageForm(-1);
					messageBox.Message = msg;
					messageBox.Size = new System.Drawing.Size(700, 139);
					messageBox.LabelLocation = new Point(7, 11);
					messageBox.Text = "Load the previous valuation settings?";
					messageBox.BTNOneText = "Open previous valuation settings";
					messageBox.BTNSecondText = "Reset valuation settings";
					messageBox.BTNThirdText = "Cancel";
					messageBox.SetFirstButton();

					rtn = messageBox.ShowDialog();
					if (rtn == DialogResult.No)
					{
						//reset CommonClass.ValuationMethodPoolingAndAggregation for valuation window, 
						//get .lstValuationMethodPoolingAndAggregationBase.IncidencePoolingAndAggregation from .lstIncidencePoolingAndAggregation
						if (CommonClass.ValuationMethodPoolingAndAggregation != null && _dicPoolingWindowOperation != null)
						{
							foreach (KeyValuePair<string, int> k in _dicPoolingWindowOperation)
							{
								if (CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Where(p => p.IncidencePoolingAndAggregation.PoolingName == k.Key).Count() > 0)
								{
									CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Where(p => p.IncidencePoolingAndAggregation.PoolingName == k.Key).First().IncidencePoolingAndAggregation = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == k.Key).First();
									CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Where(p => p.IncidencePoolingAndAggregation.PoolingName == k.Key).First().LstAllSelectValuationMethod = null;
									CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Where(p => p.IncidencePoolingAndAggregation.PoolingName == k.Key).First().lstAllSelectQALYMethod = null;
									CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Where(p => p.IncidencePoolingAndAggregation.PoolingName == k.Key).First().lstAllSelectQALYMethodAndValue = null;
									CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Where(p => p.IncidencePoolingAndAggregation.PoolingName == k.Key).First().LstAllSelectValuationMethodAndValue = null;


								}
							}
						}


					}
					else if (rtn == DialogResult.Cancel)
					{ return; }
				}

				SelectValuationMethods frm = new SelectValuationMethods();
				rtn = frm.ShowDialog();
				if (rtn != DialogResult.OK) { return; }
				else
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
				return;

				List<CRSelectFunctionCalculateValue> lstSelected = treeListView.Objects as List<CRSelectFunctionCalculateValue>;
				/* This is obsolete now that user defined weights are set in this screen
				foreach (IncidencePoolingAndAggregation ip in CommonClass.lstIncidencePoolingAndAggregation)
				{
				if (ip.lstAllSelectCRFuntion.Select(p => p.PoolingMethod).Contains("User Defined Weights"))
				{
						SelectSubjectiveWeight frmAPV = new SelectSubjectiveWeight(ip);
						DialogResult rtnAPV = frmAPV.ShowDialog();
						if (rtnAPV != DialogResult.OK) { return; }
						CommonClass.lstIncidencePoolingAndAggregation.First().Weights = frmAPV.dicAllWeight.Values.ToList();
				}
				}
				*/
				CommonClass.lstIncidencePoolingAndAggregation.First().ConfigurationResultsFilePath = ""; CommonClass.ValuationMethodPoolingAndAggregation = null;
				SelectValuationMethods frm2 = new SelectValuationMethods();
				DialogResult rtn2 = frm2.ShowDialog();
				if (rtn2 != DialogResult.OK) { return; }
				else
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
			}
			catch (Exception ex)
			{

			}
		}
		TipFormGIF waitMess = new TipFormGIF(); bool sFlog = true;
		private void ShowWaitMess()
		{
			try
			{
				if (!waitMess.IsDisposed)
				{
					waitMess.ShowDialog();
				}

			}
			catch (System.Threading.ThreadAbortException Err)
			{
				MessageBox.Show(Err.Message);
			}
		}

		public void WaitShow(string msg)
		{
			try
			{
				if (sFlog == true)
				{
					sFlog = false;
					waitMess.Msg = msg;
					System.Threading.Thread upgradeThread = null;
					upgradeThread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowWaitMess));
					upgradeThread.Start();
				}
			}
			catch (System.Threading.ThreadAbortException Err)
			{
				MessageBox.Show(Err.Message);
			}
		}
		private delegate void CloseFormDelegate();

		public void WaitClose()
		{
			if (waitMess.InvokeRequired)
				waitMess.Invoke(new CloseFormDelegate(DoCloseJob));
			else
				DoCloseJob();
		}

		private void DoCloseJob()
		{
			try
			{
				if (!waitMess.IsDisposed)
				{
					if (waitMess.Created)
					{
						sFlog = true;
						waitMess.Close();
					}
				}
			}
			catch (System.Threading.ThreadAbortException Err)
			{
				MessageBox.Show(Err.Message);
			}
		}

		private void textBoxFilterSimple_TextChanged(object sender, EventArgs e)
		{
			this.olvcDataSet.ValuesChosenForFiltering.Clear();
			this.olvcEndPointGroup.ValuesChosenForFiltering.Clear();
			this.TimedFilter(this.olvAvailable, textBoxFilterSimple.Text);
		}
		void TimedFilter(ObjectListView olv, string txt)
		{
			this.TimedFilter(olv, txt, 0);
		}

		void TimedFilter(ObjectListView olv, string txt, int matchKind)
		{
			TextMatchFilter filter = null;
			if (!String.IsNullOrEmpty(txt))
			{
				switch (matchKind)
				{
					case 0:
					default:
						filter = TextMatchFilter.Contains(olv, txt);
						break;
					case 1:
						filter = TextMatchFilter.Prefix(olv, txt);
						break;
					case 2:
						filter = TextMatchFilter.Regex(olv, txt);
						break;
				}
			}
			if (filter == null)
				olv.DefaultRenderer = null;
			else
			{
				olv.DefaultRenderer = new HighlightTextRenderer(filter);

			}


			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			olv.ModelFilter = filter;
			stopWatch.Stop();

			IList objects = olv.Objects as IList;
		}

		private void cbGroups_CheckedChanged(object sender, EventArgs e)
		{
			ShowGroupsChecked(this.olvAvailable, (CheckBox)sender);
		}
		void ShowGroupsChecked(ObjectListView olv, CheckBox cb)
		{
			if (cb.Checked && olv.View == View.List)
			{
				cb.Checked = false;
				MessageBox.Show("ListView's cannot show groups when in List view.", "Object List View Demo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				olv.ShowGroups = cb.Checked;
				olv.BuildList();
			}

		}

		private void cbDataSet_SelectedIndexChanged(object sender, EventArgs e)
		{
			ObjectListView olv = olvAvailable;
			if (olv == null || olv.IsDisposed)
				return;
			OLVColumn column = olv.GetColumn("olvcDataSet");

			ArrayList chosenValues = new ArrayList();
			KeyValuePair<string, int> kvp = (KeyValuePair<string, int>)cbDataSet.SelectedItem;
			if (!string.IsNullOrEmpty(kvp.Key))
			{
				chosenValues.Add(kvp.Key);
				olvcDataSet.ValuesChosenForFiltering = chosenValues;
				if (olvcDataSet.IsVisible == false)
				{
					olvcDataSet.IsVisible = true;
					olv.RebuildColumns();
					olv.UpdateColumnFiltering();
					olvcDataSet.IsVisible = false;
					olv.RebuildColumns();
				}
				else
					olv.UpdateColumnFiltering();
			}
			else
			{
				olvcDataSet.ValuesChosenForFiltering.Clear();
				if (olvcDataSet.IsVisible == false)
				{
					olvcDataSet.IsVisible = true;
					olv.RebuildColumns();
					olv.UpdateColumnFiltering();
					olvcDataSet.IsVisible = false;
					olv.RebuildColumns();
				}
				else
					olv.UpdateColumnFiltering();
			}

		}

		private void cbEndPointGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			ObjectListView olv = olvAvailable;
			if (olv == null || olv.IsDisposed)
				return;
			OLVColumn column = olv.GetColumn("olvcEndPointGroup");

			ArrayList chosenValues = new ArrayList();
			KeyValuePair<string, int> kvp = (KeyValuePair<string, int>)cbEndPointGroup.SelectedItem;
			if (!string.IsNullOrEmpty(kvp.Key))
			{
				chosenValues.Add(kvp.Key);
				olvcEndPointGroup.ValuesChosenForFiltering = chosenValues;
				olv.UpdateColumnFiltering();

			}
			else
			{
				olvcEndPointGroup.ValuesChosenForFiltering.Clear();
				olv.UpdateColumnFiltering();

			}

		}
		private void treeListView_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			if (e.Column.Text == "Pooling Method")
			{

				((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(cbPoolingMethod_SelectedIndexChanged);

				((TreeListView)sender).RefreshItem(e.ListViewItem);
				((ComboBox)e.Control).Dispose();
				e.Cancel = true;
			}
			else if (e.Column.Text == "Weight")
			{
				((TextBox)e.Control).TextChanged -= new EventHandler(txt_TextChanged);
				((TreeListView)sender).RefreshItem(e.ListViewItem);
				((TextBox)e.Control).Dispose();
				e.Cancel = true;
			}
		}
		private void treeListView_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			base.OnClick(e);
			if (e.Column == null) return;
			AllSelectCRFunction asvm = (AllSelectCRFunction)e.RowObject;

			if (e.Column.Text == "Pooling Method" && asvm.NodeType != 100 && (asvm.ChildCount > 1 || asvm.NodeType == 0)) //YY: Add asvm.ChildCount >1 to not allow editing for one-child groups.
			{

				ComboBox cb = new ComboBox();
				cb.Bounds = e.CellBounds;
				cb.Font = ((TreeListView)sender).Font;
				cb.DropDownStyle = ComboBoxStyle.DropDownList;
				if (!CommonClass.CRRunInPointMode)
				{
					cb.Items.Add("None");
					cb.Items.Add("Sum Dependent");
					cb.Items.Add("Sum Independent");
					cb.Items.Add("Subtraction Dependent");
					cb.Items.Add("Subtraction Independent");
					cb.Items.Add("User Defined Weights");
					cb.Items.Add("Random Or Fixed Effects");
					cb.Items.Add("Fixed Effects");
				}
				else
				{
					cb.Items.Add("None");
					cb.Items.Add("Sum Dependent");
					cb.Items.Add("Subtraction Dependent");
					cb.Items.Add("User Defined Weights");


				}

				if (e.Value != null)
				{
					cb.SelectedText = e.Value.ToString();
					cb.Text = e.Value.ToString();
				}

				cb.SelectedIndexChanged += new EventHandler(cbPoolingMethod_SelectedIndexChanged);
				cb.Tag = e.RowObject;
				e.Control = cb;
			}
			else if (e.Column.Text == "Weight")
			{
				if (asvm.PoolingMethod == "None" || (asvm.PoolingMethod == "" && asvm.NodeType != 100))
				{
					e.Cancel = true;
					return;
				}
				List<AllSelectCRFunction> lstParent = new List<AllSelectCRFunction>();
				getParentNotNone(asvm, lstParent);
				if (lstParent.Where(p => p.PoolingMethod == "User Defined Weights").Count() > 0)
				{
					TextBox txt = new TextBox();
					txt.Bounds = e.CellBounds;
					txt.Font = ((ObjectListView)sender).Font;
					txt.TextChanged += new EventHandler(txt_TextChanged);
					txt.Tag = e.RowObject;
					e.Control = txt;
					if (e.Value != null)//&& asvm.PoolingMethod != "None")
					{
						txt.Text = e.Value.ToString();
					}
				}
				else
				{
					e.Cancel = true;
				}
			}
			else if (e.Column.Text == "Nickname")
			{
				if (asvm.NodeType == 100)
				{
					e.Cancel = true;
				}
				else
				{
					//YY: allow users to edit text in this field.
					TextBox txt = new TextBox();
					txt.Bounds = e.CellBounds;
					txt.Font = ((ObjectListView)sender).Font;
					txt.Tag = e.RowObject;
					e.Control = txt;
					if (e.Value != null)//&& asvm.PoolingMethod != "None")
					{
						txt.Text = e.Value.ToString();
					}
				}

			}
			else
			{
				e.Cancel = true;
			}
		}


		private void getAllParent(AllSelectCRFunction allSelectCRFunction, List<AllSelectCRFunction> lstReturn)
		{
			IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
			var query = ip.lstAllSelectCRFuntion.Where(p => p.ID == allSelectCRFunction.PID);
			if (query != null && query.Count() > 0)
			{
				lstReturn.Add(query.First());
				getAllParent(query.First(), lstReturn);
			}

		}

		private void getParent(AllSelectCRFunction allSelectCRFunction, List<AllSelectCRFunction> lstReturn)
		{
			IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
			var query = ip.lstAllSelectCRFuntion.Where(p => p.ID == allSelectCRFunction.PID);
			if (query != null && query.Count() > 0)
			{
				lstReturn.Add(query.First());
				getParent(query.First(), lstReturn); //YY: Reenabled Nov 2019
			}

		}

		private void getParentNotNone(AllSelectCRFunction allSelectCRFunction, List<AllSelectCRFunction> lstReturn)
		{
			IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
			if (ip.lstAllSelectCRFuntion == null)
			{
				return;
			}
			var query = ip.lstAllSelectCRFuntion.Where(p => p.ID == allSelectCRFunction.PID);
			if (query != null && query.Count() > 0)
			{
				lstReturn.Add(query.First());
				if (query.First().PoolingMethod == "None" || query.First().PoolingMethod == "")
				{
					//getParent(query.First(), lstReturn);
					getParentNotNone(query.First(), lstReturn); //YY:
				}
			}

		}
		private void getAllChildMethodNotNone(AllSelectCRFunction allSelectCRFunction, List<AllSelectCRFunction> lstAll, ref List<AllSelectCRFunction> lstReturn)
		{
			//-------------YY: This is the orignal code. 
			//List<AllSelectCRFunction> lstOne = lstAll.Where(p => p.PID == allSelectCRFunction.ID).ToList();
			//lstReturn.AddRange(lstOne.Where(p => p.PoolingMethod != "None" || p.NodeType == 100).ToList());
			//foreach (AllSelectCRFunction asvm in lstOne.Where(p => p.PoolingMethod == "None").ToList())
			//{
			//    getAllChildMethodNotNone(asvm, lstAll, ref lstReturn);

			//}
			//-------------End of original code -----------

			//YY: new code. get all children (either pooled group or indivisual studies.)
			List<AllSelectCRFunction> lstDirectChildren = lstAll.Where(p => p.PID == allSelectCRFunction.ID).ToList();
			foreach (AllSelectCRFunction asvm in lstDirectChildren)
			{
				if (asvm.PoolingMethod != "None" && asvm.ChildCount != 1)
				{
					lstReturn.Add(asvm);
				}
				else //direct child is the a final node, look further
				{
					getAllChildMethodNotNone(asvm, lstAll, ref lstReturn);
				}
			}

		}

		void txt_TextChanged(object sender, EventArgs e)
		{
			try
			{
				TextBox txt = (TextBox)sender;
				List<double> list = new List<double>();
				AllSelectCRFunction txttag = (AllSelectCRFunction)txt.Tag;
				if (Convert.ToDouble(txt.Text) >= 0 && Convert.ToDouble(txt.Text) < 1)
					txttag.Weight = Math.Round(Convert.ToDouble(txt.Text), 2);

			}
			catch (Exception ex)
			{
				Logger.LogError(ex);
			}

		}

		void cb_MouseLeave(object sender, EventArgs e)
		{
			treeListView.FinishCellEdit();
		}


		void cbPoolingMethod_SelectedIndexChanged(object sender, EventArgs e)
		{


			ComboBox cb = (ComboBox)sender;
			if (((AllSelectCRFunction)cb.Tag).PoolingMethod == cb.Text) return;
			IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
			if (cb.Text.Contains("Subtraction"))
			{
				AllSelectCRFunction ascr = (AllSelectCRFunction)treeListView.SelectedObjects[0];
				if (ascr.AgeRange.Contains(";"))
				{
					MessageBox.Show("The functions within this group have discrete age ranges."
							+ "\n"
							+ "Using \"" + cb.Text + "\" may generate unreliable results."
							, "Questionable Method Selected"
							, MessageBoxButtons.OK
							, MessageBoxIcon.Warning
							);
				}
				else
				{
					List<AllSelectCRFunction> lstAll = ip.lstAllSelectCRFuntion;
					List<AllSelectCRFunction> lstChildMethodNotNone = new List<AllSelectCRFunction>();
					getAllChildMethodNotNone(ascr, lstAll, ref lstChildMethodNotNone);
					lstChildMethodNotNone = lstChildMethodNotNone.OrderBy(p => p.ID).ToList();
					int startAge = Convert.ToInt32(lstChildMethodNotNone.First().StartAge);
					int endAge = Convert.ToInt32(lstChildMethodNotNone.First().EndAge);
					int i = 1;
					foreach (AllSelectCRFunction cr in lstChildMethodNotNone)
					{
						if (i == 1)
						{

						}
						else
						{
							if (Convert.ToInt32(cr.StartAge) < startAge || Convert.ToInt32(cr.EndAge) > endAge)
							{
								MessageBox.Show("The functions within this groups have age ranges may not work for subtraction."
												+ "\n"
												+ "Using \"" + cb.Text + "\" may generate unreliable results."
												, "Questionable Method Selected"
												, MessageBoxButtons.OK
												, MessageBoxIcon.Warning
												);
								break;
							}
						}
						i++;
					}



				}
			}

			_operationStatus = 2;
			((AllSelectCRFunction)cb.Tag).PoolingMethod = cb.Text;

			ip.lstAllSelectCRFuntion.Where(p => p.ID == ((AllSelectCRFunction)cb.Tag).ID).First().PoolingMethod = cb.Text;
			if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
			{
				_dicPoolingWindowOperation[ip.PoolingName] = 3;
			}
			else
			{
				_dicPoolingWindowOperation.Add(ip.PoolingName, 3);
			}
			int iTop = Convert.ToInt32(treeListView.TopItemIndex.ToString());

			double d = 0;
			int widthWeight = 0;
			foreach (AllSelectCRFunction allSelectCRFunction in ip.lstAllSelectCRFuntion)
			{
				if (allSelectCRFunction.PoolingMethod == "User Defined Weights")
				{
					widthWeight = 60;
					List<AllSelectCRFunction> lst = new List<AllSelectCRFunction>();
					getAllChildMethodNotNone(allSelectCRFunction, ip.lstAllSelectCRFuntion, ref lst);
					d = 0;
					if (lst.Count > 0 && (lst.Sum(p => p.Weight) < 0.99 || lst.Sum(p => p.Weight) > 1.01)) // lst.Min(p => p.Weight) == 0)
					{
						d = Math.Round(Convert.ToDouble(1.000 / Convert.ToDouble(lst.Count)), 2);
						for (int i = 0; i < lst.Count; i++)
						{
							lst[i].Weight = d;
						}
					}
				}
				else if (allSelectCRFunction.PoolingMethod == "None" || (allSelectCRFunction.PoolingMethod == "" && allSelectCRFunction.NodeType != 100))
				{
					allSelectCRFunction.Weight = 0;
				}
				else //Reset to 0
				{
					List<AllSelectCRFunction> lst = new List<AllSelectCRFunction>();
					getAllChildMethodNotNone(allSelectCRFunction, ip.lstAllSelectCRFuntion, ref lst);
					d = 0;
					if (lst.Count > 0)
					{
						for (int i = 0; i < lst.Count; i++)
						{
							lst[i].Weight = d;
						}
					}
				}

			}
			OLVColumn weightColumn = treeListView.AllColumns[2];
			if (weightColumn.Width != widthWeight)
			{
				weightColumn.Width = widthWeight;
				treeListView.RebuildColumns();
			}

			treeListView.TopItemIndex = iTop;

		}
		public void btAddCRFunctions_Click(object sender, EventArgs e)
		{
			try
			{
				IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
				if (!dicTabCR.ContainsKey(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text))
				{
					dicTabCR.Add(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text, new List<CRSelectFunctionCalculateValue>());
				}
				List<CRSelectFunctionCalculateValue> lstAvailable = new List<CRSelectFunctionCalculateValue>();
				List<string> lstAvalilableEndPointGroup = new List<string>();
				foreach (CRSelectFunctionCalculateValue cr in olvAvailable.SelectedObjects)
				{
					lstAvailable.Add(cr);
					if (!lstAvalilableEndPointGroup.Contains(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
					{
						lstAvalilableEndPointGroup.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup);
					}
				}
				if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] == null) dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();
				if (ip.lstAllSelectCRFuntion != null && ip.lstAllSelectCRFuntion.Count > 0)
				{
					dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 100 && p.CRSelectFunctionCalculateValue != null && p.CRSelectFunctionCalculateValue.CRSelectFunction != null).Select(p => p.CRSelectFunctionCalculateValue).ToList();
				}
				else
				{
					dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();

				}

				if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
				{
				}

				dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].AddRange(lstAvailable);
				List<BrightIdeasSoftware.OLVColumn> lstOLVColumns = new List<OLVColumn>();
				foreach (BrightIdeasSoftware.OLVColumn olvc2 in this.treeListView.Columns)
				{
					lstOLVColumns.Add(olvc2);
				}
				lstOLVColumns = lstOLVColumns.OrderBy(p => p.DisplayIndex).ToList();
				lstOLVColumns = lstOLVColumns.Where(p => p.DisplayIndex != 0 && p.DisplayIndex != 1 && p.DisplayIndex != 2 && p.DisplayIndex != 3).OrderBy(p => p.DisplayIndex).ToList();

				List<AllSelectCRFunction> lstAllSelectCRFunction = new List<AllSelectCRFunction>(); Dictionary<string, List<CRSelectFunctionCalculateValue>> dicEndPointGroupCR = new Dictionary<string, List<CRSelectFunctionCalculateValue>>();
				foreach (CRSelectFunctionCalculateValue cr in dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text])
				{
					if (dicEndPointGroupCR.ContainsKey(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
						dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
					else
					{
						dicEndPointGroupCR.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup, new List<CRSelectFunctionCalculateValue>());
						dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
					}
				}
				lstAllSelectCRFunction = ip.lstAllSelectCRFuntion;
				if (lstAllSelectCRFunction == null) lstAllSelectCRFunction = new List<AllSelectCRFunction>();
				List<AllSelectCRFunction> lstRemoveAllSelectCRFuntion = ip.lstAllSelectCRFuntion.Where(p => lstAvalilableEndPointGroup.Contains(p.EndPointGroup)).ToList();
				foreach (AllSelectCRFunction ascr in lstRemoveAllSelectCRFuntion)
				{
					lstAllSelectCRFunction.Remove(ascr);
				}
				int poolLevel = ip.PoolLevel;
				foreach (KeyValuePair<string, List<CRSelectFunctionCalculateValue>> k in dicEndPointGroupCR)
				{
					if (!lstAvalilableEndPointGroup.Contains(k.Key)) continue;
					List<AllSelectCRFunction> lstTemp = getLstAllSelectCRFunction(k.Value, lstOLVColumns.Select(p => p.Text).ToList(), k.Key, -1, poolLevel);
					if (lstAllSelectCRFunction.Count() > 0)
					{
						for (int iTemp = 0; iTemp < lstTemp.Count; iTemp++)
						{
							lstTemp[iTemp].ID = lstTemp[iTemp].ID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
							if (lstTemp[iTemp].PID != -1)
								lstTemp[iTemp].PID = lstTemp[iTemp].PID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
						}
					}
					if (lstTemp != null && lstTemp.Count > 0) lstAllSelectCRFunction.AddRange(lstTemp);
				}
				_operationStatus = 1; if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
				{
					_dicPoolingWindowOperation[ip.PoolingName] = 1;
				}
				else
				{
					_dicPoolingWindowOperation.Add(ip.PoolingName, 1);
				}
				ip.lstAllSelectCRFuntion = lstAllSelectCRFunction;
				if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
					incidenceBusinessCardRenderer.lstExists = dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Select(p => p.CRSelectFunction.CRID).ToList();
				else
					incidenceBusinessCardRenderer.lstExists = new List<int>();
				olvAvailable.Refresh();
				initTreeView(ip);
			}
			catch
			{ }
			return;
			try
			{
				List<CRSelectFunctionCalculateValue> lstAvailable = new List<CRSelectFunctionCalculateValue>();
				foreach (CRSelectFunctionCalculateValue cr in olvAvailable.SelectedObjects)
				{
					lstAvailable.Add(cr);

				}
				if (lstAvailable.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID).Distinct().Count() > 1)
				{
					MessageBox.Show("Pooling requires that functions have the same endpoint group.");
					return;
				}
				IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
				if (ip.lstAllSelectCRFuntion != null && ip.lstAllSelectCRFuntion.Count > 0)
				{
					if (lstAvailable.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID != ip.lstAllSelectCRFuntion.Where(a => a.NodeType == 4).First().CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID)
					{
						MessageBox.Show("Pooling requires that functions have the same endpoint group.");
						return;

					}
					lstAvailable = new List<CRSelectFunctionCalculateValue>();
					var queryCRID = ip.lstAllSelectCRFuntion.Where(a => a.NodeType == 4).Select(p => p.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.ID);
					foreach (CRSelectFunctionCalculateValue cr in olvAvailable.SelectedObjects)
					{
						lstAvailable.Add(cr);

					}

					var query = lstAvailable.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint).ToList().Distinct().ToList();
					int i = ip.lstAllSelectCRFuntion.Max(p => p.ID) + 1;
					int iEndPoint = 0, iAuthor = 0, iQualifier = 0;
					string strTemp = "";
					for (int iquery = 0; iquery < query.Count(); iquery++)
					{
						strTemp = query[iquery];
						var queryEndPoint = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 1 && p.Name == strTemp).ToList();
						if (queryEndPoint.Count() == 0)
						{
							ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
							{
								NodeType = 1,
								ID = i,
								Name = query[iquery],
								PID = 0,
								PoolingMethod = "None",


							});
							iEndPoint = i;
							i++;
						}
						else
						{
							iEndPoint = queryEndPoint.First().ID;
						}
						var author = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery]).Select(a => a.CRSelectFunction.BenMAPHealthImpactFunction.Author).Distinct().ToList();
						for (int iauthor = 0; iauthor < author.Count; iauthor++)
						{
							var queryAuthor = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 2 && p.Name == author[iauthor] && p.PID == iEndPoint);
							if (queryAuthor.Count() == 0)
							{

								ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
								{
									NodeType = 2,
									ID = i,
									Name = author[iauthor],
									PID = iEndPoint,
									PoolingMethod = "None",


								});
								iAuthor = i;
								i++;
							}
							else
							{
								iAuthor = queryAuthor.First().ID;

							}
							var Qualifier = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor] && p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery]).Select(a => a.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier).Distinct().ToList();
							for (int iQ = 0; iQ < Qualifier.Count; iQ++)
							{
								var queryQualifier = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 3 && p.Name == Qualifier[iQ] && p.PID == iAuthor);
								if (queryQualifier.Count() == 0)
								{
									ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
									{
										NodeType = 3,
										ID = i,
										Name = Qualifier[iQ],
										PID = iAuthor,
										PoolingMethod = "None",


									});
									iQualifier = i;
									i++;

								}
								else
								{
									iQualifier = queryQualifier.First().ID;
								}
								var funtion = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor] && p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery] && p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier == Qualifier[iQ]).ToList();
								for (int ifunction = 0; ifunction < funtion.Count; ifunction++)
								{
									ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
									{
										NodeType = 4,
										ID = i,
										Name = funtion[ifunction].CRSelectFunction.BenMAPHealthImpactFunction.strLocations,
										PID = iQualifier,
										CRSelectFunctionCalculateValue = funtion[ifunction]


									});
									i++;
								}
							}

						}


					}




				}
				else if (ip.lstAllSelectCRFuntion == null || ip.lstAllSelectCRFuntion.Count == 0)
				{
					ip.lstAllSelectCRFuntion = new List<AllSelectCRFunction>();
					ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
					{
						NodeType = 0,
						EndPointGroupID = lstAvailable.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID,
						ID = 0,
						Name = lstAvailable.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup,
						PID = -1,
						PoolingMethod = "None",


					});
					var query = lstAvailable.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint).ToList().Distinct().ToList();
					int i = 1;
					int iEndPoint = 0, iAuthor = 0, iQualifier = 0;
					for (int iquery = 0; iquery < query.Count(); iquery++)
					{
						ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
						{
							NodeType = 1,
							ID = i,
							Name = query[iquery],
							PID = 0,
							PoolingMethod = "None",


						});
						iEndPoint = i;
						i++;
						var author = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery]).Select(a => a.CRSelectFunction.BenMAPHealthImpactFunction.Author).Distinct().ToList();
						for (int iauthor = 0; iauthor < author.Count; iauthor++)
						{
							ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
							{
								NodeType = 2,
								ID = i,
								Name = author[iauthor],
								PID = iEndPoint,
								PoolingMethod = "None",


							});
							iAuthor = i;
							i++;
							var Qualifier = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery] && p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor]).Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier).Distinct().ToList();
							for (int iqualifier = 0; iqualifier < Qualifier.Count; iqualifier++)
							{
								ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
								{
									NodeType = 3,
									ID = i,
									Name = Qualifier[iqualifier],
									PID = iAuthor,
									PoolingMethod = "None",


								});
								iQualifier = i;
								i++;
								var function = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery] && p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor] && p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier == Qualifier[iqualifier]).ToList();
								for (int ifunction = 0; ifunction < function.Count(); ifunction++)
								{
									ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
									{
										NodeType = 4,
										ID = i,
										Name = function[ifunction].CRSelectFunction.BenMAPHealthImpactFunction.strLocations,
										PID = iQualifier,
										CRSelectFunctionCalculateValue = function[ifunction]


									});

									i++;

								}
							}

						}


					}


				}
				initTreeView(ip);
			}
			catch (Exception ex)
			{

			}

		}

		public void updateDrop(AllSelectCRFunction source, AllSelectCRFunction targe)
		{
			IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();

			if (source.NodeType == 4 && targe.NodeType == 3)
			{
				var cr3 = ip.lstAllSelectCRFuntion.Where(p => p.ID == source.PID).First();

				source.PID = targe.ID;
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr3.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr3);

				}
				var cr2 = ip.lstAllSelectCRFuntion.Where(p => p.ID == cr3.PID).First();
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr2.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr2);

				}
				var cr1 = ip.lstAllSelectCRFuntion.Where(p => p.ID == cr2.PID).First();
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr1.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr1);

				}
				var cr0 = ip.lstAllSelectCRFuntion.Where(p => p.ID == cr1.PID).First();
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr0.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr0);

				}

			}
			else if (source.NodeType == 4 && targe.NodeType == 4)
			{

				var cr3 = ip.lstAllSelectCRFuntion.Where(p => p.ID == source.PID).First();

				source.PID = targe.PID;
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr3.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr3);

				}
				var cr2 = ip.lstAllSelectCRFuntion.Where(p => p.ID == cr3.PID).First();
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr2.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr2);

				}
				var cr1 = ip.lstAllSelectCRFuntion.Where(p => p.ID == cr2.PID).First();
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr1.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr1);

				}
				var cr0 = ip.lstAllSelectCRFuntion.Where(p => p.ID == cr1.PID).First();
				if (ip.lstAllSelectCRFuntion.Where(p => p.PID == cr0.ID).Count() == 0)
				{
					ip.lstAllSelectCRFuntion.Remove(cr0);

				}

			}



			treeListView.RebuildAll(true);
			treeListView.ExpandAll();

		}
		public void btDelSelectMethod_Click(object sender, EventArgs e)
		{
			IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
			_operationStatus = 3; if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
			{
				_dicPoolingWindowOperation[ip.PoolingName] = 3;
			}
			else
			{
				_dicPoolingWindowOperation.Add(ip.PoolingName, 3);
			}

			foreach (AllSelectCRFunction cr in treeListView.SelectedObjects)
			{
				if (cr.NodeType == 100)
				{
					ip.lstAllSelectCRFuntion.Remove(cr);
					dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Remove(cr.CRSelectFunctionCalculateValue);

				}

			}
			List<AllSelectCRFunction> lstRemove = new List<AllSelectCRFunction>();
			foreach (AllSelectCRFunction allSelectCRFunction in ip.lstAllSelectCRFuntion)
			{
				List<AllSelectCRFunction> lstTmp = new List<AllSelectCRFunction>();
				APVX.APVCommonClass.getAllChildCR(allSelectCRFunction, ip.lstAllSelectCRFuntion, ref lstTmp);
				if (lstTmp.Where(p => p.NodeType == 100).Count() == 0)
					lstRemove.Add(allSelectCRFunction);
				if (lstTmp.Where(p => p.NodeType == 100).Count() == 1)
				{
					lstRemove.Add(allSelectCRFunction);
					lstTmp.First().PID = allSelectCRFunction.PID;
					var query = ip.lstAllSelectCRFuntion.Where(p => p.ID == allSelectCRFunction.PID).ToList();
					while (query.Count > 0)
					{
						APVX.APVCommonClass.getAllChildCR(query.First(), ip.lstAllSelectCRFuntion, ref lstTmp);
						if (lstTmp.Where(p => p.NodeType == 100).Count() == 1)
						{
							lstRemove.Add(query.First());
							lstTmp.First().PID = query.First().PID;
						}
						else
							break;
					}
				}

			}
			lstRemove = lstRemove.Where(p => p.NodeType != 100).ToList();
			foreach (AllSelectCRFunction allSelectCRFunction in lstRemove)
			{
				ip.lstAllSelectCRFuntion.Remove(allSelectCRFunction);
			}












			initTreeView(ip);
			if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] == null) dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();
			if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
				incidenceBusinessCardRenderer.lstExists = dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Select(p => p.CRSelectFunction.CRID).ToList();
			else
				incidenceBusinessCardRenderer.lstExists = new List<int>();
			olvAvailable.Refresh();

		}
		private void updateTreeColumns(ref List<string> lstString)
		{
			if (lstString == null)
			{
				lstString = new List<string>();
				for (int i = 4; i < treeListView.Columns.Count; i++) //YY: changed 3 to 4 for new added column
				{
					OLVColumn olvc = treeListView.Columns[i] as OLVColumn;
					lstString.Add(olvc.Text);
				}
			}
			int j = 0;
			foreach (OLVColumn olvc in treeListView.Columns)
			{

				olvc.DisplayIndex = j;
				j++;
			}
			for (int i = 0; i < lstString.Count(); i++)
			{
				foreach (OLVColumn olvc in treeListView.Columns)
				{
					if (olvc.Text == lstString[i])
						olvc.DisplayIndex = i + 4; //YY: changed 3 to 4 for new added column
				}
			}
		}
		private void btTileSet_Click(object sender, EventArgs e)
		{
			try
			{
				APVX.TileSet tileSet = new APVX.TileSet();
				tileSet.olv = this.olvAvailable;
				DialogResult dr = tileSet.ShowDialog();
				if (dr == System.Windows.Forms.DialogResult.Cancel)
					return;
				int count = 0;
				for (int i = 0; i < this.olvAvailable.AllColumns.Count; i++)
				{
					if ((this.olvAvailable.AllColumns[i] as BrightIdeasSoftware.OLVColumn).IsTileViewColumn)
					{
						count++;
						(this.olvAvailable.AllColumns[i] as BrightIdeasSoftware.OLVColumn).IsVisible = true;
					}
					else
					{
						(this.olvAvailable.AllColumns[i] as BrightIdeasSoftware.OLVColumn).IsVisible = false;
					}
				}
				this.olvAvailable.TileSize = new Size(120, Convert.ToInt32(count * 15.2 + 10));
				this.olvAvailable.RebuildColumns();
				this.olvAvailable.Refresh();
			}
			catch
			{
			}
		}

		private bool _hasDelPoolingWindows = false;

		private void cbPoolingWindow_SelectedIndexChanged(object sender, EventArgs e)
		{

		}



		private void btnSTileSet_Click(object sender, EventArgs e)
		{
			APVX.TileSet tileSet = new APVX.TileSet();
			tileSet.olv = this.treeListView;
			DialogResult dr = tileSet.ShowDialog();
			if (dr == System.Windows.Forms.DialogResult.Cancel)
				return;
			int count = 0;
			for (int i = 0; i < this.treeListView.Columns.Count; i++)
			{
				if ((this.treeListView.Columns[i] as BrightIdeasSoftware.OLVColumn).IsTileViewColumn)
					count++;
			}
			this.treeListView.TileSize = new Size(300, Convert.ToInt32(count * 14.3 + 47));
			this.treeListView.Refresh();
		}

		private void tabControlSelected_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (tabControlSelected.SelectedIndex == -1) return;
				if (tabControlSelected.SelectedIndex == tabControlSelected.TabPages.Count) //YY: add a new tab.
				{
					cbPoolLevel.SelectedItem = "3";
					return;
				}
				IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();

				if (tabControlSelected.SelectedIndex > -1)
				{
					cbPoolLevel.SelectedItem = ip.PoolLevel.ToString(); //YY: update cbPoolLevel
					tbPoolingName.Text = tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text;
					updateTreeColumns(ref ip.lstColumns);
					initTreeView(ip);
					treeListView.Dock = DockStyle.Fill;
					treeListView.TabIndex = tabControlSelected.SelectedIndex;
					int widthWeight = 0;
					if (ip.lstAllSelectCRFuntion.Select(p => p.PoolingMethod).Contains("User Defined Weights"))
					{
						widthWeight = 60;
					}
					OLVColumn weightColumn = treeListView.AllColumns[2];
					if (weightColumn.Width != widthWeight)
					{
						weightColumn.Width = widthWeight;
						//treeListView.RebuildColumns();
					}

					treeListView.Refresh();
					tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Controls.Add(treeListView);


				}
				treeListView.Visible = true;

				if (!dicTabCR.ContainsKey(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text))
				{
					dicTabCR.Add(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text, new List<CRSelectFunctionCalculateValue>());


					if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] == null) dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();
					if (ip.lstAllSelectCRFuntion != null && ip.lstAllSelectCRFuntion.Count > 0)
					{
						dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 100 && p.CRSelectFunctionCalculateValue != null && p.CRSelectFunctionCalculateValue.CRSelectFunction != null).Select(p => p.CRSelectFunctionCalculateValue).ToList();
					}
					else
					{
						dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();

					}
				}
				if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
					incidenceBusinessCardRenderer.lstExists = dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Select(p => p.CRSelectFunction.CRID).ToList();
				else
					incidenceBusinessCardRenderer.lstExists = new List<int>();
				olvAvailable.Refresh();

			}
			catch
			{
				treeListView.Objects = null;

			}

		}

		private void tbPoolingName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btChangeName_Click(sender, e);
			}

		}

		private bool istabControlSelectedContainText(string text)
		{
			bool btp = false;
			foreach (TabPage tp in tabControlSelected.TabPages)
			{
				if (tp.Text == text) btp = true;
			}
			return btp;

		}
		private void btChangeName_Click(object sender, EventArgs e)
		{
			try
			{
				IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
				if (ip.PoolingName == tbPoolingName.Text)
					return;
				if (tbPoolingName.Text == "")
				{
					tbPoolingName.Text = ip.PoolingName;
					return;
				}
				if (!istabControlSelectedContainText(tbPoolingName.Text) && tbPoolingName.Text != "")
				{
					if (dicTabCR.ContainsKey(ip.PoolingName))
					{
						dicTabCR.Keys.ToList()[dicTabCR.Keys.ToList().IndexOf(ip.PoolingName)] = tbPoolingName.Text;
					}
					if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
					{
						_dicPoolingWindowOperation.Add(tbPoolingName.Text, _dicPoolingWindowOperation[ip.PoolingName]);
						_dicPoolingWindowOperation.Remove(ip.PoolingName);
					}

					ip.PoolingName = tbPoolingName.Text;
					_operationStatus = 2;

					tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text = tbPoolingName.Text;


				}
				else
				{
					tbPoolingName.Text = ip.PoolingName;
					MessageBox.Show("This pooling window name is already defined. Please use a different name.");
				}
			}
			catch
			{ }
		}

		private void treeListView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
		{
			try
			{
				if (e.NewDisplayIndex == 0 || e.NewDisplayIndex == 1 || e.NewDisplayIndex == 2 || e.NewDisplayIndex == 3
						|| e.OldDisplayIndex == 0 || e.OldDisplayIndex == 1 || e.OldDisplayIndex == 2 || e.OldDisplayIndex == 3) //YY: new column added
				{
					e.Cancel = true;
					return;
				}
				_operationStatus = 1;

				OLVColumn olvc = treeListView.Columns[e.OldDisplayIndex] as OLVColumn;
				List<BrightIdeasSoftware.OLVColumn> lstOLVColumns = new List<OLVColumn>();
				foreach (BrightIdeasSoftware.OLVColumn olvc2 in this.treeListView.Columns)
				{
					lstOLVColumns.Add(olvc2);
				}
				olvc = lstOLVColumns.Where(p => p.DisplayIndex == e.OldDisplayIndex).First();
				lstOLVColumns = lstOLVColumns.OrderBy(p => p.DisplayIndex).ToList();
				lstOLVColumns.Remove(olvc);
				lstOLVColumns.Insert(e.NewDisplayIndex, olvc);
				lstOLVColumns = lstOLVColumns.Where(p => p.DisplayIndex != 0 && p.DisplayIndex != 1 && p.DisplayIndex != 2 && p.DisplayIndex != 3).ToList(); //YY: new column added

				IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
				ip.lstColumns = lstOLVColumns.Select(p => p.Text).ToList();
				if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
				{
					_dicPoolingWindowOperation[ip.PoolingName] = 1;
				}
				else
				{
					_dicPoolingWindowOperation.Add(ip.PoolingName, 1);
				}
				List<AllSelectCRFunction> lstAllSelectCRFunction = new List<AllSelectCRFunction>(); Dictionary<string, List<CRSelectFunctionCalculateValue>> dicEndPointGroupCR = new Dictionary<string, List<CRSelectFunctionCalculateValue>>();
				foreach (CRSelectFunctionCalculateValue cr in dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text])
				{
					try
					{
						if (dicEndPointGroupCR.ContainsKey(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
							dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
						else
						{
							dicEndPointGroupCR.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup, new List<CRSelectFunctionCalculateValue>());
							dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
						}
					}
					catch
					{ }
				}
				int poolLevel = ip.PoolLevel;
				foreach (KeyValuePair<string, List<CRSelectFunctionCalculateValue>> k in dicEndPointGroupCR)
				{
					List<AllSelectCRFunction> lstTemp = getLstAllSelectCRFunction(k.Value, lstOLVColumns.Select(p => p.Text).ToList(), k.Key, -1, poolLevel);
					if (lstAllSelectCRFunction.Count() > 0)
					{
						for (int iTemp = 0; iTemp < lstTemp.Count; iTemp++)
						{
							lstTemp[iTemp].ID = lstTemp[iTemp].ID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
							if (lstTemp[iTemp].PID != -1)
								lstTemp[iTemp].PID = lstTemp[iTemp].PID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
						}
					}
					if (lstTemp != null && lstTemp.Count > 0) lstAllSelectCRFunction.AddRange(lstTemp);
				}
				ip.lstAllSelectCRFuntion = lstAllSelectCRFunction;
				initTreeView(ip);
				//use new displayindex to add icon

				foreach (OLVColumn olvColumn in treeListView.Columns)
				{
					int newDisplayIndex = olvColumn.DisplayIndex;
					if (newDisplayIndex == e.OldDisplayIndex)
					{
						newDisplayIndex = e.NewDisplayIndex;
					}
					else if (e.OldDisplayIndex > e.NewDisplayIndex)
					{
						if (newDisplayIndex < e.OldDisplayIndex && newDisplayIndex >= e.NewDisplayIndex)
						{
							newDisplayIndex = newDisplayIndex + 1;
						}
					}
					else if (e.OldDisplayIndex < e.NewDisplayIndex)
					{
						if (newDisplayIndex > e.OldDisplayIndex && newDisplayIndex <= e.NewDisplayIndex)
						{
							newDisplayIndex = newDisplayIndex - 1;
						}
					}

					if (newDisplayIndex < poolLevel + 4 && newDisplayIndex >= 4) //YY: 4 columns included new added column
					{
						if (newDisplayIndex == 4)
						{
							olvColumn.HeaderImageKey = "headerP1";
						}
						else if (newDisplayIndex == 5)
						{
							olvColumn.HeaderImageKey = "headerP2";
						}
						else if (newDisplayIndex == 6)
						{
							olvColumn.HeaderImageKey = "headerP3";
						}
					;

					}
					else if (olvColumn.HeaderImageKey != "headerE")
					{
						olvColumn.HeaderImageKey = "";
					}
				}
			}
			catch (Exception ex)
			{
			}

		}
		private string getNameFromOLVColumn(string column, CRSelectFunctionCalculateValue cr)
		{
			string sReturn = "";
			switch (column)
			{
				case "endpoint":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint;
					break;
				case "author":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.Author;
					break;
				case "qualifier":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier;
					break;
				case "location":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.strLocations;
					break;
				case "startage":
					sReturn = cr.CRSelectFunction.StartAge.ToString();
					break;
				case "endage":
					sReturn = cr.CRSelectFunction.EndAge.ToString();
					break;
				case "year":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.Year.ToString();
					break;
				case "otherpollutants":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.OtherPollutants;
					break;
				case "race":
					sReturn = cr.CRSelectFunction.Race;
					break;
				case "ethnicity":
					sReturn = cr.CRSelectFunction.Ethnicity;
					break;
				case "gender":
					sReturn = cr.CRSelectFunction.Gender;
					break;
				case "function":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.Function;
					break;
				case "pollutant":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.Pollutant.PollutantName;
					break;
				case "metric":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.Metric.MetricName;
					break;
				case "seasonalmetric":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric.SeasonalMetricName;
					break;
				case "metricstatistic":
					sReturn = Enum.GetName(typeof(MetricStatic), cr.CRSelectFunction.BenMAPHealthImpactFunction.MetricStatistic);
					break;
				case "dataset":
					sReturn = cr.CRSelectFunction.BenMAPHealthImpactFunction.DataSetName;
					break;

			}
			return sReturn;
		}
		public static void getSecond(ref List<CRSelectFunctionCalculateValue> lstSecond, List<string> lstOLVColumns, int i, List<string> lstParent)
		{
			for (int k = 0; k < i; k++)
			{
				switch (lstOLVColumns[k].Replace(" ", "").ToLower())
				{
					case "endpoint":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == lstParent[i - k - 1]).ToList();
						break;

					case "author":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Author == lstParent[i - k - 1]).ToList();
						break;
					case "qualifier":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier == lstParent[i - k - 1]).ToList();
						break;
					case "location":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.strLocations == lstParent[i - k - 1]).ToList();
						break;
					case "startage":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.StartAge.ToString() == lstParent[i - k - 1]).ToList();
						break;
					case "endage":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.EndAge.ToString() == lstParent[i - k - 1]).ToList();
						break;
					case "year":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Year.ToString() == lstParent[i - k - 1]).ToList();
						break;
					case "otherpollutants":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.OtherPollutants == lstParent[i - k - 1]).ToList();
						break;
					case "race":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.Race == lstParent[i - k - 1]).ToList();
						break;
					case "ethnicity":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.Ethnicity == lstParent[i - k - 1]).ToList();
						break;
					case "gender":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.Gender == lstParent[i - k - 1]).ToList();
						break;
					case "function":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Function == lstParent[i - k - 1]).ToList();
						break;
					case "pollutant":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Pollutant.PollutantName == lstParent[i - k - 1]).ToList();
						break;
					case "metric":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Metric.MetricName == lstParent[i - k - 1]).ToList();
						break;
					case "seasonalmetric":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric == null ? "" == lstParent[i - k - 1] : p.CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric.SeasonalMetricName == lstParent[i - k - 1]).ToList();
						break;
					case "metricstatistic":
						lstSecond = lstSecond.Where(p => Enum.GetName(typeof(MetricStatic), p.CRSelectFunction.BenMAPHealthImpactFunction.MetricStatistic) == lstParent[i - k - 1]).ToList();
						break;
					case "dataset":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.DataSetName == lstParent[i - k - 1]).ToList();
						break;
					case "studylocation":
						lstSecond = lstSecond.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.strLocations == lstParent[i - k - 1]).ToList();
						break;
					case "version":
						var query = lstSecond.GroupBy(p => p.CRSelectFunction.CRID);
						List<CRSelectFunctionCalculateValue> lstVersion = new List<CRSelectFunctionCalculateValue>();
						foreach (IGrouping<int, CRSelectFunctionCalculateValue> ig in query)
						{
							int iCount = Convert.ToInt32(lstParent[i - k - 1]);
							if (ig.Count() >= iCount)
							{
								lstVersion.Add(ig.ElementAt(iCount - 1));

							}
						}
						lstSecond = lstVersion;
						break;
				}
			}
		}
		public static List<string> getLstStringFromColumnName(string columName, List<CRSelectFunctionCalculateValue> lstCR)
		{
			List<string> lstString = new List<string>();
			switch (columName)
			{
				case "endpoint":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint).Distinct().ToList();
					break;
				case "author":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Author).Distinct().ToList();
					break;
				case "qualifier":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier).Distinct().ToList();
					break;
				case "location":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.strLocations).Distinct().ToList();
					break;
				case "startage":
					lstString = lstCR.Select(p => p.CRSelectFunction.StartAge.ToString()).Distinct().ToList();
					break;
				case "endage":
					lstString = lstCR.Select(p => p.CRSelectFunction.EndAge.ToString()).Distinct().ToList();
					break;
				case "year":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Year.ToString()).Distinct().ToList();
					break;
				case "otherpollutants":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.OtherPollutants).Distinct().ToList();
					break;
				case "race":
					lstString = lstCR.Select(p => p.CRSelectFunction.Race).Distinct().ToList();
					break;
				case "ethnicity":
					lstString = lstCR.Select(p => p.CRSelectFunction.Ethnicity).Distinct().ToList();
					break;
				case "gender":
					lstString = lstCR.Select(p => p.CRSelectFunction.Gender).Distinct().ToList();
					break;
				case "function":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Function).Distinct().ToList();
					break;
				case "pollutant":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Pollutant.PollutantName).Distinct().ToList();
					break;
				case "metric":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Metric.MetricName).Distinct().ToList();
					break;
				case "seasonalmetric":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric == null ? "" : p.CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric.SeasonalMetricName).Distinct().ToList();
					break;
				case "metricstatistic":
					lstString = lstCR.Select(p => Enum.GetName(typeof(MetricStatic), p.CRSelectFunction.BenMAPHealthImpactFunction.MetricStatistic)).Distinct().ToList();
					break;
				case "dataset":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.DataSetName).Distinct().ToList();
					break;
				case "studylocation":
					lstString = lstCR.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.strLocations).Distinct().ToList();
					break;
				case "version":
					int iversion = lstCR.GroupBy(p => p.CRSelectFunction.CRID).Max(p => p.Count());
					lstString = new List<string>();
					for (int i = 1; i <= iversion; i++)
					{
						lstString.Add(i.ToString());
					}
					break;
			}

			return lstString;
		}
		public static List<AllSelectCRFunction> getLstAllSelectCRFunction(List<CRSelectFunctionCalculateValue> lstCR, List<string> lstOLVColumns, string EndPointGroup, int iMaxNodeType, int poolLevel)
		{
			try
			{
				List<AllSelectCRFunction> lstReturn = new List<AllSelectCRFunction>();
				if (lstCR == null) return null;
				//When there is only one study.
				if (lstCR.Count == 1)
				{
					lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.GeographicAreaName = lstCR.First().CRSelectFunction.GeographicAreaName;
					lstReturn.Add(new AllSelectCRFunction()
					{
						CRID = lstCR.First().CRSelectFunction.CRID,
						CRSelectFunctionCalculateValue = lstCR.First(),
						EndPointGroupID = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID,
						EndPointID = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointID,
						ID = 0,
						Name = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup,
						Nickname = "", //YY: new
						NodeType = 100,
						PID = -1,
						PoolingMethod = "",
						Version = "1",

						EndPointGroup = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup,
						EndPoint = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPoint,
						Author = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Author,
						Qualifier = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Qualifier,
						Location = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.strLocations,
						GeographicArea = lstCR.First().CRSelectFunction.GeographicAreaName,
						StartAge = lstCR.First().CRSelectFunction.StartAge.ToString(),
						EndAge = lstCR.First().CRSelectFunction.EndAge.ToString(),
						Year = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Year.ToString(),
						OtherPollutants = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.OtherPollutants,
						Race = lstCR.First().CRSelectFunction.Race,
						Ethnicity = lstCR.First().CRSelectFunction.Ethnicity,
						Gender = lstCR.First().CRSelectFunction.Gender,
						Function = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Function,
						Pollutant = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Pollutant == null ? "" : lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Pollutant.PollutantName.ToString(),
						Metric = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Metric == null ? "" : lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.Metric.MetricName,
						SeasonalMetric = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric == null ? "" : lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric.SeasonalMetricName,
						MetricStatistic = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.MetricStatistic == null ? "" : Enum.GetName(typeof(MetricStatic), lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.MetricStatistic),
						DataSet = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.DataSetName,


					});
					return lstReturn;
				}
				else //when there are multiple studies
				{
					//Add Endpint Group as first row (first group)
					lstReturn.Add(new AllSelectCRFunction()
					{

						EndPointGroupID = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID,

						ID = 0,
						Name = EndPointGroup,
						Nickname = EndPointGroup, //YY: new
						EndPointGroup = EndPointGroup,
						NodeType = 0,
						PID = -1,
						PoolingMethod = "None",
						Version = "",

					});

					List<string> lstColumns = new List<string>();

					//Loop through all group fields
					for (int i = 0; i < poolLevel; i++) // YY: lstOLVColumns.Count is changed to poolLevel value from dropdown as we want to limit users only pool at most 3 levels
					{

						List<string> lstString = new List<string>();
						int iParent = 0;

						lstString = getLstStringFromColumnName(lstOLVColumns[i].Replace(" ", "").ToLower(), lstCR);
						if (lstString.Count() > 0)
						{
							if (i == 0) // Add group rows for values from first column
							{
								iParent = 0;


								for (int j = 0; j < lstString.Count(); j++)
								{
									lstReturn.Add(new AllSelectCRFunction()
									{
										EndPointGroupID = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID,
										EndPointGroup = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup,
										ID = j + 1,
										Name = lstString[j],
										Nickname = lstString[j], //YY: new
										NodeType = i + 1,
										PID = 0,
										PoolingMethod = "None",
										Version = "",

									});
									if (lstOLVColumns[i].Replace(" ", "").ToLower() == "version")
									{
										lstReturn.Last().Version = lstReturn.Last().Name;
									}
								}
							}
							else // For the second columns, 
							{
								List<AllSelectCRFunction> query = lstReturn.Where(p => p.NodeType == i).ToList();
								if (query.Count() == 0) { i = lstOLVColumns.Count - 1; }
								else
								{
									for (int j = 0; j < query.Count(); j++)
									{
										List<string> lstParent = new List<string>();
										lstParent.Add(query[j].Name);
										var parent = lstReturn.Where(p => p.ID == query[j].PID).First();
										lstParent.Add(parent.Name);
										for (int k = i; k > 1; k--)
										{
											parent = lstReturn.Where(p => p.ID == parent.PID).First();
											lstParent.Add(parent.Name);

										}
										List<CRSelectFunctionCalculateValue> lstSecond = lstCR;
										getSecond(ref lstSecond, lstOLVColumns, i, lstParent);
										if (lstSecond.Count() > 0) //YY: Changed >1 to >0 to keep folder even when there is only one item.
										{
											lstString = getLstStringFromColumnName(lstOLVColumns[i].Replace(" ", "").ToLower(), lstSecond);
											if (lstString.Count > 0)
											{
												for (int k = 0; k < lstString.Count(); k++)
												{
													lstReturn.Add(new AllSelectCRFunction()
													{
														EndPointGroupID = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID,
														EndPointGroup = lstCR.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup,
														ID = lstReturn.Count(),
														Name = lstString[k],
														Nickname = lstString[k], //YY: new
														NodeType = i + 1,
														PID = query[j].ID,
														PoolingMethod = "None", //YY: if only 1 child, pooling method will be removed in later step.
														Version = "",

													});
													if (lstOLVColumns[i].Replace(" ", "").ToLower() == "version")
													{
														lstReturn.Last().Version = lstReturn.Last().Name;
													}
												}
											}
											else
											{

											}
										}
									}
								}
							}


						}
						else
						{
							//if the column doesn't exist in getLstStringFromColumnName, stop moving to the next column.
							i = lstOLVColumns.Count - 1;
						}
					}
					var queryTree = lstReturn.Where(p => lstReturn.Where(a => a.PID == p.ID).Count() == 0).ToList();
					foreach (AllSelectCRFunction acf in queryTree)
					{
						List<string> lstParent = new List<string>();
						lstParent.Add(acf.Name);
						if (lstReturn.Where(p => p.ID == acf.PID).Count() > 0)
						{
							var parent = lstReturn.Where(p => p.ID == acf.PID).First();
							lstParent.Add(parent.Name);
							for (int k = acf.NodeType; k > 1; k--)
							{
								parent = lstReturn.Where(p => p.ID == parent.PID).First();
								lstParent.Add(parent.Name);

							}
						}
						List<CRSelectFunctionCalculateValue> lstSecond = lstCR;
						getSecond(ref lstSecond, lstOLVColumns, acf.NodeType, lstParent);
						int iVersion = 1;
						int iVersionNodeType = 0;
						for (int io = 0; io < lstOLVColumns.Count(); io++)
						{
							if (lstOLVColumns[io].ToLower() == "version")
							{
								iVersionNodeType = io + 1;

							}
						}
						foreach (CRSelectFunctionCalculateValue crc in lstSecond)
						{
							iVersion = lstReturn.Where(p => p.CRID == crc.CRSelectFunction.CRID).Count() + 1;

							string strAuthor = crc.CRSelectFunction.BenMAPHealthImpactFunction.Author;
							if (strAuthor != null && strAuthor != "" && strAuthor.Contains(" "))
							{
								strAuthor = strAuthor.Substring(0, strAuthor.IndexOf(" "));
							}
							lstReturn.Add(new AllSelectCRFunction()
							{
								CRID = crc.CRSelectFunction.CRID,
								CRSelectFunctionCalculateValue = crc,
								EndPointGroupID = crc.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID,
								EndPointID = crc.CRSelectFunction.BenMAPHealthImpactFunction.EndPointID,
								ID = lstReturn.Count(),
								Name = strAuthor,
								Nickname = "", //YY: new
								NodeType = 100,
								PID = acf.ID,
								PoolingMethod = "",
								Version = iVersion.ToString(),
								EndPointGroup = crc.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup,
								EndPoint = crc.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint,
								Author = crc.CRSelectFunction.BenMAPHealthImpactFunction.Author,
								Qualifier = crc.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier,
								Location = crc.CRSelectFunction.BenMAPHealthImpactFunction.strLocations,
								GeographicArea = crc.CRSelectFunction.GeographicAreaName,
								StartAge = crc.CRSelectFunction.StartAge.ToString(),
								EndAge = crc.CRSelectFunction.EndAge.ToString(),
								Year = crc.CRSelectFunction.BenMAPHealthImpactFunction.Year.ToString(),
								OtherPollutants = crc.CRSelectFunction.BenMAPHealthImpactFunction.OtherPollutants,
								Race = crc.CRSelectFunction.Race,
								Ethnicity = crc.CRSelectFunction.Ethnicity,
								Gender = crc.CRSelectFunction.Gender,
								Function = crc.CRSelectFunction.BenMAPHealthImpactFunction.Function,
								Pollutant = crc.CRSelectFunction.BenMAPHealthImpactFunction.Pollutant.PollutantName,
								Metric = crc.CRSelectFunction.BenMAPHealthImpactFunction.Metric.MetricName,
								SeasonalMetric = crc.CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric == null ? "" : crc.CRSelectFunction.BenMAPHealthImpactFunction.SeasonalMetric.SeasonalMetricName,
								MetricStatistic = Enum.GetName(typeof(MetricStatic), crc.CRSelectFunction.BenMAPHealthImpactFunction.MetricStatistic),
								DataSet = crc.CRSelectFunction.BenMAPHealthImpactFunction.DataSetName,
								AgeRange = crc.CRSelectFunction.StartAge.ToString() + "-" + crc.CRSelectFunction.EndAge.ToString(),
							});
						}

					}
					if (lstReturn.Count > 0)
					{
						int iMaxLstReturnNodeType = iMaxNodeType != -1 ? iMaxNodeType : lstReturn.Where(p => p.NodeType != 100).Max(p => p.NodeType);
						if (lstOLVColumns.GetRange(0, iMaxNodeType != -1 ? iMaxNodeType : lstReturn.Where(p => p.NodeType != 100).Max(p => p.NodeType)).Contains("Version"))
						{
							var lstTemp = lstReturn.Where(p => p.NodeType == lstOLVColumns.IndexOf("Version") + 1).ToList();
							for (int ilstTemp = 0; ilstTemp < lstTemp.Count; ilstTemp++)
							{
								List<AllSelectCRFunction> lstTempReturn = new List<AllSelectCRFunction>();
								APVX.APVCommonClass.getAllChildCR(lstTemp[ilstTemp], lstReturn, ref lstTempReturn);
								foreach (AllSelectCRFunction alsc in lstTempReturn)
								{
									if (alsc.Version == "")
										alsc.Version = lstTemp[ilstTemp].Name;
								}
							}

						}
						for (int iMax = iMaxLstReturnNodeType; iMax > 0; iMax--)
						{
							var lstTemp = lstReturn.Where(p => p.NodeType == iMax).ToList();
							foreach (AllSelectCRFunction alcr in lstTemp)
							{
								var lstTempSec = lstReturn.Where(p => p.PID == alcr.ID).ToList();
								var lstTempSec2 = lstReturn.Where(p => p.PID == alcr.PID).ToList();
								if (lstTempSec2.Count == 1 || (lstTempSec.Count == 1 && lstTempSec[0].NodeType == 100))
								{

									//YY: skip this step as we want to keep all parents
									//foreach (AllSelectCRFunction alcrSec in lstTempSec)
									//{
									//    alcrSec.PID = alcr.PID;

									//}
									//lstReturn.Remove(alcr);
								}
								//YY: assign ChildCount
								alcr.ChildCount = lstTempSec.Count;
								if (alcr.ChildCount <= 1)
								{
									alcr.PoolingMethod = "";
								}
							}
						}
						lstReturn.Where(p => p.NodeType != 100).Last().NodeType = iMaxLstReturnNodeType;



					}



				}
				foreach (AllSelectCRFunction acr in lstReturn)
				{
					////agerange for functions
					//if((acr.AgeRange is null || acr.AgeRange == "") && acr.NodeType==100)
					//{
					//    acr.AgeRange = acr.StartAge + "-" + acr.EndAge;
					//}

					//populate values for pooled groups. 
					if (acr.PoolingMethod != "" || acr.NodeType != 100) //YY: added Nodetype
					{
						List<AllSelectCRFunction> lst = new List<AllSelectCRFunction>();
						getAllChildFromAllSelectCRFunction(acr, lstReturn, ref lst);
						if (lst.Count > 0)
						{
							if (acr.CRSelectFunctionCalculateValue == null) acr.CRSelectFunctionCalculateValue = new CRSelectFunctionCalculateValue()
							{
								CRSelectFunction = new CRSelectFunction()
								{

									StartAge = Convert.ToInt32(lst.Min(p => p.StartAge)),
									EndAge = Convert.ToInt32(lst.Max(p => p.EndAge)),
								}
							};
							if (acr.CRSelectFunctionCalculateValue.CRSelectFunction == null)
							{
								acr.CRSelectFunctionCalculateValue.CRSelectFunction = new CRSelectFunction();
								if (acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction == null)
								{
									acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction = new BenMAPHealthImpactFunction();
								}
							}
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.StartAge = Convert.ToInt32(lst.Min(p => p.StartAge));
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.EndAge = Convert.ToInt32(lst.Max(p => p.EndAge));
							acr.StartAge = lst.Min(p => p.StartAge);
							acr.EndAge = lst.Max(p => p.EndAge);


							List<string> lstTemp = lst.Select(p => p.Pollutant).Distinct().ToList();
							acr.Pollutant = "";
							foreach (string s in lstTemp)
							{
								acr.Pollutant += s + " ";
							}


							acr.Author = "";
							acr.EndPoint = "";
							acr.GeographicArea = "";
							acr.AgeRange = "";
							List<string> lstAuthor = new List<string>();
							List<string> lstEndPoint = new List<string>();
							List<string> lstGeoArea = new List<string>();
							List<Tuple<int, int>> lstAgeRange = new List<Tuple<int, int>>(); //YY: new added

							foreach (AllSelectCRFunction alcr in lst)
							{
								if (alcr.CRSelectFunctionCalculateValue == null || alcr.CRSelectFunctionCalculateValue.CRSelectFunction == null || alcr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction == null) continue;
								if (alcr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Author != "" && !lstAuthor.Contains(alcr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Author))
								{
									lstAuthor.Add(alcr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Author);
								}
								if (alcr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint != "" && !lstEndPoint.Contains(alcr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint))
								{
									lstEndPoint.Add(alcr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint);
								}
								if (alcr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName != "" && !lstGeoArea.Contains(alcr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName))
								{
									lstGeoArea.Add(alcr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName);
								}
								Tuple<int, int> range = new Tuple<int, int>(alcr.CRSelectFunctionCalculateValue.CRSelectFunction.StartAge, alcr.CRSelectFunctionCalculateValue.CRSelectFunction.EndAge);
								if (!lstAgeRange.Contains(range))
								{
									lstAgeRange.Add(range);
								}
							}
							if (acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction == null)
							{
								acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction = new BenMAPHealthImpactFunction();

							}

							//Pooled fields
							acr.Race = "Pooled";
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.Race = "";
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Race = "Pooled";
							acr.DataSet = "Pooled";
							//acr.CRSelectFunctionCalculateValue.CRSelectFunction.dataaset
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.DataSetName = "Pooled";
							acr.Ethnicity = "Pooled";
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.Ethnicity = "";
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Ethnicity = "Pooled";
							acr.Gender = "Pooled";
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.Gender = "";
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Gender = "Pooled";
							acr.Location = "Pooled";
							//acr.CRSelectFunctionCalculateValue.CRSelectFunction.Location = "Pooled";
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.strLocations = ""; //treeListView shares the same column with Result for location.

							//YY: count of studies
							acr.CountStudies = lst.Count(n => n.NodeType == 100);
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.CountStudies = acr.CountStudies; //which one do we need? 
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.CountStudies = acr.CountStudies; //which one do we need? 

							//YY: calculate age ranges (same as APVX.APVCommonClass.getGroupAgeRange)
							string strAgeRange = "";
							if (lstAgeRange.Count() == 1)
							{
								strAgeRange = acr.StartAge + "-" + acr.EndAge;
							}
							else
							{
								lstAgeRange.Sort();
								int i = 0;
								int startAge = 0;
								int endAge = 0;
								foreach (Tuple<int, int> range in lstAgeRange)
								{
									if (i == 0)
									{
										startAge = range.Item1;
										endAge = range.Item2;
									}
									else
									{
										if (range.Item1 <= endAge + 1)
										{
											if (endAge < range.Item2) endAge = range.Item2;
										}
										else
										{
											strAgeRange = strAgeRange + startAge.ToString() + "-" + endAge.ToString() + ";";
											startAge = range.Item1;
											endAge = range.Item2;
										}
									}
									i++;
								}
								strAgeRange = strAgeRange + startAge.ToString() + "-" + endAge.ToString();
							}
							acr.AgeRange = strAgeRange;
							acr.CRSelectFunctionCalculateValue.CRSelectFunction.AgeRange = strAgeRange;


							foreach (string s in lstAuthor)
							{

								acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Author += String.IsNullOrEmpty(acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.Author) ? s : "; " + s; //YY: use semicolumn instead of space
								acr.Author += acr.Author == "" ? s : "; " + s; //YY: use semicolumn instead of space
							}
							foreach (string s in lstEndPoint)
							{

								acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint += String.IsNullOrEmpty(acr.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint) ? s : " " + s;
								acr.EndPoint += acr.EndPoint == "" ? s : " " + s;
							}
							foreach (string s in lstGeoArea)
							{
								// Make sure Everywhere and Elsewhere are at the end of the list
								if (s != Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_ELSEWHERE && s != Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_EVERYWHERE)
								{
									acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName = (String.IsNullOrEmpty(acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName) ? s : acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName + "; " + s);
									acr.GeographicArea = (acr.GeographicArea == "" ? s : acr.GeographicArea + "; " + s);
								}
							}
							if (lstGeoArea.Contains(Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_ELSEWHERE))
							{
								acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName = (String.IsNullOrEmpty(acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName)
										? Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_ELSEWHERE
										: acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName + "; " + Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_ELSEWHERE);
								acr.GeographicArea = (acr.GeographicArea == ""
										? Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_ELSEWHERE
										: acr.GeographicArea + "; " + Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_ELSEWHERE);
							}
							if (lstGeoArea.Contains(Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_EVERYWHERE))
							{
								acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName = (String.IsNullOrEmpty(acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName)
										? Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_EVERYWHERE
										: acr.CRSelectFunctionCalculateValue.CRSelectFunction.GeographicAreaName + "; " + Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_EVERYWHERE);
								acr.GeographicArea = (acr.GeographicArea == ""
										? Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_EVERYWHERE
										: acr.GeographicArea + "; " + Configuration.ConfigurationCommonClass.GEOGRAPHIC_AREA_EVERYWHERE);
							}
						}

					}
				}
				return lstReturn;
			}
			catch
			{
				return null;
			}
		}

		private void btOLVTileSet_Click(object sender, EventArgs e)
		{
			//btOLVTileSet button is hidden in Nov 2019. This code and the button can be removed once we are 100% sure we don't want this function any more.
			return;
			try
			{
				APVX.TileSet tileSet = new APVX.TileSet();
				tileSet.olv = this.olvTile;
				DialogResult dr = tileSet.ShowDialog();
				if (dr == System.Windows.Forms.DialogResult.Cancel)
					return;
				int count = 0;
				for (int i = 0; i < this.olvTile.AllColumns.Count; i++)
				{
					if ((this.olvTile.AllColumns[i] as BrightIdeasSoftware.OLVColumn).IsTileViewColumn)
					{
						count++;
						(this.olvTile.AllColumns[i] as BrightIdeasSoftware.OLVColumn).IsVisible = true;
					}
					else
					{
						(this.olvTile.AllColumns[i] as BrightIdeasSoftware.OLVColumn).IsVisible = false;
					}
				}
				this.olvTile.TileSize = new Size(120, Convert.ToInt32(count * 15.2 + 10));
				this.olvTile.RebuildColumns();
				this.olvTile.Refresh();
			}
			catch
			{
			}
		}

		private void SetTileAllSelectCRFunction(AllSelectCRFunction allSelectCRFunction)
		{
			// olvTile is hidden since Nov 2019. Code can be removed if not needed any more.
			return;
			try
			{
				IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
				List<AllSelectCRFunction> lstAllSelectCRFunction = new List<AllSelectCRFunction>();
				if (ip.lstAllSelectCRFuntion.Count == 1)
				{

					lstAllSelectCRFunction.Add(allSelectCRFunction);
					olvTile.SetObjects(lstAllSelectCRFunction);

				}
				else if (allSelectCRFunction.NodeType == 100)
				{

					if (allSelectCRFunction.PID == -1)
					{
						lstAllSelectCRFunction.Add(allSelectCRFunction);
						olvTile.SetObjects(lstAllSelectCRFunction);
					}
					else
					{
						AllSelectCRFunction parent = ip.lstAllSelectCRFuntion.Where(p => p.ID == allSelectCRFunction.PID).First();
						lstAllSelectCRFunction = ip.lstAllSelectCRFuntion.Where(p => p.PID == parent.ID).ToList();
						olvTile.SetObjects(lstAllSelectCRFunction);
					}


				}
				else
				{
					APVX.APVCommonClass.getAllChildCR(allSelectCRFunction, ip.lstAllSelectCRFuntion, ref lstAllSelectCRFunction);
					olvTile.SetObjects(lstAllSelectCRFunction.Where(p => p.NodeType == 100).ToList());
				}
				olvTile.RebuildColumns();
			}
			catch
			{
			}
		}

		//// Comment out as olvTile is not in use since Nov 2019.
		//private void treeListView_DoubleClick(object sender, EventArgs e)
		//{
		//    if (treeListView.SelectedObjects.Count == 0)
		//        return;
		//    AllSelectCRFunction allSelectCRFunction = treeListView.SelectedObjects[0] as AllSelectCRFunction;

		//    SetTileAllSelectCRFunction(allSelectCRFunction);
		//}

		private void btBrowseCR_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.InitialDirectory = Application.StartupPath + @"E:\";
				openFileDialog.Filter = "CFGR files (*.cfgrx)|*.cfgrx";
				openFileDialog.FilterIndex = 3;
				openFileDialog.RestoreDirectory = true;
				if (openFileDialog.ShowDialog() != DialogResult.OK) { return; }
				if (openFileDialog.FileName != CommonClass.ValuationMethodPoolingAndAggregation.CFGRPath)
				{
					MessageBox.Show("This is a different .cfgrx file. Choose again.");

				}
				else
				{
					txtOpenExistingCFGR.Text = openFileDialog.FileName;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("This is a different .cfgrx file. Choose again.");
			}
		}

		private void tabControlSelected_DrawItem(object sender, DrawItemEventArgs e)
		{
			Font fntTab;
			Brush bshBack;
			Brush bshFore;
			if (e.Index == this.tabControlSelected.SelectedIndex)
			{
				fntTab = new Font(e.Font, FontStyle.Bold);
				bshBack = new SolidBrush(Color.White);
				bshFore = Brushes.Black;
			}
			else
			{
				fntTab = e.Font;
				bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, SystemColors.Control, SystemColors.Control, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
				bshFore = new SolidBrush(Color.Black);
			}
			string tabName = this.tabControlSelected.TabPages[e.Index].Text;
			StringFormat sftTab = new StringFormat();
			e.Graphics.FillRectangle(bshBack, e.Bounds);
			Rectangle recTab = e.Bounds;
			recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height - 4);
			//YY: render a "x" mark at the end of the Tab caption
			if (e.Index == tabControlSelected.TabPages.Count - 1) //last tab
			{
				//e.Graphics.DrawString("+", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
			}
			else
			{
				e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
			}

			e.Graphics.DrawString(tabName, fntTab, bshFore, recTab, sftTab);
			e.DrawFocusRectangle();
		}

		private void btnShowChanges_Click(object sender, EventArgs e)
		{
			try
			{
				ChangedCRFunctions ccrf = new ChangedCRFunctions();
				ccrf.Show();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex);
			}
		}

		private void treeListView_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyValue == 46)
				{
					btDelSelectMethod_Click(sender, e);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
			}
		}

		private void tbPoolingName_Leave(object sender, EventArgs e)
		{
			btChangeName_Click(sender, e);
		}
		private void tabControlSelected_MouseDown(object sender, MouseEventArgs e)
		{
			base.OnMouseDown(e);



			//If Clicked last tab, add a tab; if clicked Close bottun, close tab 
			Rectangle r = tabControlSelected.GetTabRect(tabControlSelected.SelectedIndex);
			Rectangle closeNewButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
			if (tabControlSelected.GetTabRect(tabControlSelected.TabCount - 1).Contains(e.Location)) //click on last tab. add new (code moved from btAddPoolingWindow_click and adjusted)
			{
				int i = tabControlSelected.TabCount;
				while (istabControlSelectedContainText("PoolingWindow" + i))
				{
					i++;
				}
				tabControlSelected.TabPages.Insert(tabControlSelected.TabCount - 1, "PoolingWindow" + i, "PoolingWindow" + i);
				IncidencePoolingAndAggregation ip = new IncidencePoolingAndAggregation() { PoolingName = "PoolingWindow" + i };
				CommonClass.lstIncidencePoolingAndAggregation.Add(ip);
				if (CommonClass.ValuationMethodPoolingAndAggregation != null && CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase != null)
				{
					try
					{
						CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Add(new ValuationMethodPoolingAndAggregationBase() { IncidencePoolingAndAggregation = CommonClass.lstIncidencePoolingAndAggregation.Last(), LstAllSelectValuationMethod = new List<AllSelectValuationMethod>() });

					}
					catch
					{
					}
				}
				_operationStatus = 1;
				if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
				{
					_dicPoolingWindowOperation[ip.PoolingName] = 1;
				}
				else
				{
					_dicPoolingWindowOperation.Add(ip.PoolingName, 1);
				}
				tabControlSelected.SelectedIndex = tabControlSelected.TabCount - 2;
			}
			else if (closeNewButton.Contains(e.Location)) // click on non-last tab, and on x button, close (code moved from btDelPoolingWindow_Click)
			{
				if (tabControlSelected.TabCount == 2)
				{
					MessageBox.Show("You can not delete the last pooling window.");
					return;
				}

				if (CommonClass.ValuationMethodPoolingAndAggregation != null && CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase != null)
				{
					try
					{
						CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Remove(CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase.Where(p => p.IncidencePoolingAndAggregation.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First());

					}
					catch
					{
					}

				}
				if (dicTabCR.ContainsKey(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text))
				{
					dicTabCR.Remove(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text);
				}
				CommonClass.lstIncidencePoolingAndAggregation.Remove(CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First());

				_operationStatus = 3;

				tabControlSelected.TabPages.RemoveAt(tabControlSelected.SelectedIndex);
				_hasDelPoolingWindows = true;
				if (tabControlSelected.TabPages.Count > 0)
				{
					tabControlSelected.SelectedIndex = 0;

				}
				else
					tabControlSelected.SelectedIndex = -1;
				return;
			}
			else //else, drag drop
			{
				Point pt = new Point(e.X, e.Y);
				TabPage tp = GetTabPageByTab(pt);
				if (tp != null)
				{
					DoDragDrop(tp, DragDropEffects.All);
				}
			}




		}

		private void tabControlSelected_DragOver(object sender, DragEventArgs e)
		{
			Point pt = new Point(e.X, e.Y);
			pt = tabControlSelected.PointToClient(pt);

			TabPage hover_tab = GetTabPageByTab(pt);

			if (tabControlSelected.TabPages.IndexOf(hover_tab) == tabControlSelected.TabCount - 1)
			{
				return;
			}


			if (hover_tab != null)
			{
				if (e.Data.GetDataPresent(typeof(TabPage)))
				{
					e.Effect = DragDropEffects.Move;
					TabPage drag_tab = (TabPage)e.Data.GetData(typeof(TabPage));

					int item_drag_index = FindIndex(drag_tab);
					int drop_location_index = FindIndex(hover_tab);

					if (item_drag_index != drop_location_index)
					{
						ArrayList pages = new ArrayList();
						List<IncidencePoolingAndAggregation> lstTemp = new List<IncidencePoolingAndAggregation>();
						for (int i = 0; i < tabControlSelected.TabPages.Count; i++)
						{
							if (i != item_drag_index)
							{
								pages.Add(tabControlSelected.TabPages[i]);
								if (i < tabControlSelected.TabPages.Count - 1) //YY: exclude last tab (+)
								{
									lstTemp.Add(CommonClass.lstIncidencePoolingAndAggregation[i]);
								}
								//                                lstTemp.Add(CommonClass.lstIncidencePoolingAndAggregation[i]);
							}
						}

						pages.Insert(drop_location_index, drag_tab);
						if (drop_location_index != tabControlSelected.TabPages.Count - 1)
						{
							lstTemp.Insert(drop_location_index, CommonClass.lstIncidencePoolingAndAggregation[item_drag_index]);
						}
						//lstTemp.Insert(drop_location_index, CommonClass.lstIncidencePoolingAndAggregation[item_drag_index]);
						CommonClass.lstIncidencePoolingAndAggregation = lstTemp;
						if (CommonClass.ValuationMethodPoolingAndAggregation != null && CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase != null)
						{
							List<ValuationMethodPoolingAndAggregationBase> lstTempVB = new List<ValuationMethodPoolingAndAggregationBase>();
							for (int i = 0; i < tabControlSelected.TabPages.Count - 1; i++) //YY: exclude last tab (+)
							{
								if (i != item_drag_index)
								{
									lstTempVB.Add(CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase[i]);
								}
							}


							lstTempVB.Insert(drop_location_index, CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase[item_drag_index]);
							CommonClass.ValuationMethodPoolingAndAggregation.lstValuationMethodPoolingAndAggregationBase = lstTempVB;
						}
						tabControlSelected.TabPages.Clear();

						tabControlSelected.TabPages.AddRange((TabPage[])pages.ToArray(typeof(TabPage)));

						tabControlSelected.SelectedTab = drag_tab;
						tabControlSelected_SelectedIndexChanged(sender, e);

					}
				}
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private TabPage GetTabPageByTab(Point pt)
		{
			TabPage tp = null;

			for (int i = 0; i < tabControlSelected.TabPages.Count; i++)
			{
				if (tabControlSelected.GetTabRect(i).Contains(pt))
				{
					tp = tabControlSelected.TabPages[i];
					break;
				}
			}

			return tp;
		}

		private int FindIndex(TabPage page)
		{
			for (int i = 0; i < tabControlSelected.TabPages.Count; i++)
			{
				if (tabControlSelected.TabPages[i] == page)
					return i;
			}

			return -1;
		}

		private void treeListView_FormatRow(object sender, FormatRowEventArgs e)
		{
			//// YY: no need to do anything as the view will always be in Detailed View
		}

		private void treeListView_FormatCell(object sender, FormatCellEventArgs e)
		{
			if (tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text == "+") return;
			IncidencePoolingAndAggregation ipTest = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
			if (ipTest.lstAllSelectCRFuntion == null) return;

			if (e.Column.Text == "Pooling Method")
			{
				if ((string)e.CellValue != "")
				{
					AllSelectCRFunction asvm = (AllSelectCRFunction)e.Model;
					//TextDecoration decoration = new TextDecoration("None",ContentAlignment.MiddleLeft);
					CellBorderDecoration cbd = new CellBorderDecoration();
					//decoration.TextColor = Color.Gray;
					//Always allow to pool Endpoint Group.
					if (asvm != null && asvm.NodeType == 0)
					{
						cbd.BorderPen = new Pen(Color.LightGray);
						e.SubItem.ForeColor = Color.Black;
					}
					else if (asvm.ChildCount > 1) //|| asvm.NodeType ==0
					{
						cbd.BorderPen = new Pen(Color.LightGray);
						e.SubItem.ForeColor = Color.Black;

					}
					else // items with asvm.ChildCount == 1 always have (string)e.CellValue != "". 
							 //Therefore this else will never be hit unless to choose to show pooling method for all groups.
					{
						cbd.BorderPen = new Pen(Color.LightGray);
						e.SubItem.ForeColor = Color.Gray;
					}

					cbd.FillBrush = null;
					cbd.BoundsPadding = new Size(0, -1);

					//YY: add margin/padding to the pooling method border. 
					//YY: Issues: (1) Cannot only add padding to left. (2) Cannot add padding to Text. Is CellPadding a valid property?
					//int padding = ((3 - asvm.NodeType) <0 ? 0: (3 - asvm.NodeType)) * 1;
					//int padding = (asvm.NodeType > 3 ? 3 : asvm.NodeType) * -1;
					//cbd.BoundsPadding = new Size(padding, -1);

					cbd.CornerRounding = 0.0f;
					e.SubItem.Decorations.Add(cbd);

					Image imgDD = global::BenMAP.Properties.Resources.dropdown_hint;

					e.SubItem.Decorations.Add(new ImageDecoration(imgDD, ContentAlignment.MiddleRight));
				}

				//YY: Change color of direct children of selected item. 
				//AllSelectCRFunction asvm2 = (AllSelectCRFunction)e.Model;
				//if (treeListView.SelectedObjects.Count > 0)
				//{
				//    AllSelectCRFunction cr = (AllSelectCRFunction)treeListView.SelectedObjects[0];
				//    if (asvm2.PID == cr.ID)
				//    {
				//        e.SubItem.BackColor = Color.LightGreen;
				//    }
				//}

			}
			else if (e.Column.Text == "Weight")
			{
				AllSelectCRFunction avsm = (AllSelectCRFunction)e.Item.RowObject;
				if ((avsm.PoolingMethod != "None" && avsm.PoolingMethod != "") || (avsm.NodeType == 100))
				{
					List<AllSelectCRFunction> lstParent = new List<AllSelectCRFunction>();
					//List<AllSelectCRFunction> lstChildren = new List<AllSelectCRFunction>();
					getParentNotNone(avsm, lstParent);
					//lstChildren = getChildFromAllSelectCRFunction(avsm);
					if (lstParent.Where(p => p.PoolingMethod == "User Defined Weights").Count() > 0)//&& lstChildren.Count() == 0
					{
						CellBorderDecoration cbd = new CellBorderDecoration();
						cbd.BorderPen = new Pen(Color.LightGray);
						cbd.FillBrush = null;
						cbd.BoundsPadding = new Size(0, -1);
						cbd.CornerRounding = 0.0f;
						e.SubItem.Decorations.Add(cbd);
					}
				}

			}
			else if (e.Column.Text == "Studies, By Endpoint")
			{
				//YY: Change color of direct children of selected item. 
				//AllSelectCRFunction asvm2 = (AllSelectCRFunction)e.Model;
				//if (treeListView.SelectedObjects.Count > 0)
				//{
				//    AllSelectCRFunction cr = (AllSelectCRFunction)treeListView.SelectedObjects[0];
				//    if (asvm2.PID == cr.ID)
				//    {
				//        e.SubItem.BackColor = Color.LightGreen;
				//    }
				//}
			}
			else if (e.Column.Text == "Nickname")
			{
				AllSelectCRFunction asvm = (AllSelectCRFunction)e.Model;
				if (asvm.NodeType != 100)
				{
					CellBorderDecoration cbd = new CellBorderDecoration();
					cbd.BorderPen = new Pen(Color.LightGray);
					cbd.FillBrush = null;
					cbd.BoundsPadding = new Size(0, -1);
					cbd.CornerRounding = 0.0f;
					e.SubItem.Decorations.Add(cbd);
				}
			}
			else
			{
				//YY: change background of columns used for grouping
				int poolLevel = Convert.ToInt32(cbPoolLevel.SelectedItem);
				if (e.Column.DisplayIndex < poolLevel + 4 && e.ColumnIndex >= 4) //YY: 4 columns included new added column
				{
					e.SubItem.BackColor = Color.LightBlue;
				}
			}

			//YY: highlight all endnode children of selected group
			if (e.Column.Text == "Studies, By Endpoint" || e.Column.Text == "Pooling Method" || e.Column.Text == "Weight" || e.Column.Text == "Nickname")
			{
				AllSelectCRFunction asvm = (AllSelectCRFunction)e.Model;
				if (treeListView.SelectedObjects.Count > 0)
				{
					AllSelectCRFunction cr = (AllSelectCRFunction)treeListView.SelectedObjects[0];
					IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
					List<AllSelectCRFunction> lstAll = ip.lstAllSelectCRFuntion;
					List<AllSelectCRFunction> lstGreen = new List<AllSelectCRFunction>();
					getAllChildMethodNotNone(cr, lstAll, ref lstGreen);
					if (lstGreen.Contains(asvm))
					{
						e.SubItem.BackColor = Color.LightGreen;
						//if (e.Column.Text == "Weight")
						//{
						//    CellBorderDecoration cbd = new CellBorderDecoration();
						//    cbd.BorderPen = new Pen(Color.LightGray);
						//    cbd.FillBrush = null;
						//    cbd.BoundsPadding = new Size(0, -1);
						//    cbd.CornerRounding = 0.0f;
						//    e.SubItem.Decorations.Add(cbd);
						//}
					}
				}
			}
			//getAllChildMethodNotNone(cr, lstAllCrChildren, ref lstGreen)




		}

		private void treeListView_Freezing(object sender, FreezeEventArgs e)
		{

		}

		private void treeListView_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void olvAvailable_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void groupBox4_Enter(object sender, EventArgs e)
		{

		}

		private void groupBox2_Enter(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{

		}

		private void button4_Click(object sender, EventArgs e)
		{

		}

		private void btRemoveStudy_Click(object sender, EventArgs e)
		{
			removeSelectedOrAllStudies(0);
		}
		private void removeSelectedOrAllStudies(int removeType)
		{
			try
			{
				IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
				_operationStatus = 3;
				if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
				{
					_dicPoolingWindowOperation[ip.PoolingName] = 3;
				}
				else
				{
					_dicPoolingWindowOperation.Add(ip.PoolingName, 3);
				}

				if (removeType == 0)
				{
					foreach (AllSelectCRFunction cr in treeListView.SelectedObjects)
					{
						if (cr.NodeType == 100)
						{
							ip.lstAllSelectCRFuntion.Remove(cr);
							dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Remove(cr.CRSelectFunctionCalculateValue);

						}

					}
					foreach (AllSelectCRFunction cr in ip.lstAllSelectCRFuntion)
					{
						if (cr.PoolingMethod == "User Defined Weights")
						{
							cr.PoolingMethod = "None";
						}
						else if (cr.Weight != 0)
						{
							cr.Weight = 0;
						}
					}
				}
				else if (removeType == 1)
				{
					ip.lstAllSelectCRFuntion.Clear();
					dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Clear();
				}

				List<AllSelectCRFunction> lstRemove = new List<AllSelectCRFunction>();
				foreach (AllSelectCRFunction allSelectCRFunction in ip.lstAllSelectCRFuntion)
				{
					List<AllSelectCRFunction> lstTmp = new List<AllSelectCRFunction>();
					APVX.APVCommonClass.getAllChildCR(allSelectCRFunction, ip.lstAllSelectCRFuntion, ref lstTmp);
					if (lstTmp.Where(p => p.NodeType == 100).Count() == 0)
						lstRemove.Add(allSelectCRFunction);
					if (lstTmp.Where(p => p.NodeType == 100).Count() == 1)
					{
						//YY: Comment out as we keep groups even when there is only one study under it.

						//lstRemove.Add(allSelectCRFunction);
						//lstTmp.First().PID = allSelectCRFunction.PID;
						//var query = ip.lstAllSelectCRFuntion.Where(p => p.ID == allSelectCRFunction.PID).ToList();
						//while (query.Count > 0)
						//{
						//    APVX.APVCommonClass.getAllChildCR(query.First(), ip.lstAllSelectCRFuntion, ref lstTmp);
						//    if (lstTmp.Where(p => p.NodeType == 100).Count() == 1)
						//    {
						//        lstRemove.Add(query.First());
						//        lstTmp.First().PID = query.First().PID;
						//    }
						//    else
						//        break;
						//}
					}

				}
				lstRemove = lstRemove.Where(p => p.NodeType != 100).ToList();
				foreach (AllSelectCRFunction allSelectCRFunction in lstRemove)
				{
					ip.lstAllSelectCRFuntion.Remove(allSelectCRFunction);
				}




				initTreeView(ip);
				if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] == null) dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();
				if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
					incidenceBusinessCardRenderer.lstExists = dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Select(p => p.CRSelectFunction.CRID).ToList();
				else
					incidenceBusinessCardRenderer.lstExists = new List<int>();
				olvAvailable.Refresh();

			}
			catch (Exception ex)
			{
				Logger.LogError(ex);
			}
		}

		private void btAddStudy_Click(object sender, EventArgs e)
		{
			addSelectedOrAllStudies(0);
		}
		private void addSelectedOrAllStudies(int addType)
		{
			{
				try
				{
					//selectType = 0 Add selected; selectType = 1 Add all;
					IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
					if (!dicTabCR.ContainsKey(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text))
					{
						dicTabCR.Add(tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text, new List<CRSelectFunctionCalculateValue>());
					}
					List<CRSelectFunctionCalculateValue> lstAvailable = new List<CRSelectFunctionCalculateValue>();
					List<string> lstAvalilableEndPointGroup = new List<string>();
					if (addType == 0)
					{
						foreach (CRSelectFunctionCalculateValue cr in olvAvailable.CheckedObjects)//olvAvailable.SelectedObjects --- highlighted objects
						{
							lstAvailable.Add(cr);
							if (!lstAvalilableEndPointGroup.Contains(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
							{
								lstAvalilableEndPointGroup.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup);
							}
						}
					}
					else if (addType == 1)
					{
						foreach (CRSelectFunctionCalculateValue cr in olvAvailable.FilteredObjects)
						{
							lstAvailable.Add(cr);
							if (!lstAvalilableEndPointGroup.Contains(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
							{
								lstAvalilableEndPointGroup.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup);
							}
						}
					}

					if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] == null) dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();
					if (ip.lstAllSelectCRFuntion != null && ip.lstAllSelectCRFuntion.Count > 0)
					{
						dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 100 && p.CRSelectFunctionCalculateValue != null && p.CRSelectFunctionCalculateValue.CRSelectFunction != null).Select(p => p.CRSelectFunctionCalculateValue).ToList();
					}
					else
					{
						dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] = new List<CRSelectFunctionCalculateValue>();

					}

					if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
					{
					}

					dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].AddRange(lstAvailable);

					List<BrightIdeasSoftware.OLVColumn> lstOLVColumns = new List<OLVColumn>();
					foreach (BrightIdeasSoftware.OLVColumn olvc2 in this.treeListView.Columns)
					{
						lstOLVColumns.Add(olvc2);
					}
					lstOLVColumns = lstOLVColumns.OrderBy(p => p.DisplayIndex).ToList();
					lstOLVColumns = lstOLVColumns.Where(p => p.DisplayIndex != 0 && p.DisplayIndex != 1 && p.DisplayIndex != 2 && p.DisplayIndex != 3).OrderBy(p => p.DisplayIndex).ToList(); //YY: exclude new column

					List<AllSelectCRFunction> lstAllSelectCRFunction = new List<AllSelectCRFunction>();
					Dictionary<string, List<CRSelectFunctionCalculateValue>> dicEndPointGroupCR = new Dictionary<string, List<CRSelectFunctionCalculateValue>>();
					foreach (CRSelectFunctionCalculateValue cr in dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text])
					{
						if (dicEndPointGroupCR.ContainsKey(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
							dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
						else
						{
							dicEndPointGroupCR.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup, new List<CRSelectFunctionCalculateValue>());
							dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
						}
					}
					lstAllSelectCRFunction = ip.lstAllSelectCRFuntion;
					if (lstAllSelectCRFunction == null) lstAllSelectCRFunction = new List<AllSelectCRFunction>();
					//Remove items under this Endpoing Group
					List<AllSelectCRFunction> lstRemoveAllSelectCRFuntion = ip.lstAllSelectCRFuntion.Where(p => lstAvalilableEndPointGroup.Contains(p.EndPointGroup)).ToList();
					foreach (AllSelectCRFunction ascr in lstRemoveAllSelectCRFuntion)
					{
						lstAllSelectCRFunction.Remove(ascr);
					}
					//Add items under this Endpoing Group
					int poolLevel = Convert.ToInt16(cbPoolLevel.SelectedItem); //ip.PoolLevel;
					ip.PoolLevel = poolLevel;
					foreach (KeyValuePair<string, List<CRSelectFunctionCalculateValue>> k in dicEndPointGroupCR)
					{
						if (!lstAvalilableEndPointGroup.Contains(k.Key)) continue;
						List<AllSelectCRFunction> lstTemp = getLstAllSelectCRFunction(k.Value, lstOLVColumns.Select(p => p.Text).ToList(), k.Key, -1, poolLevel);
						if (lstAllSelectCRFunction.Count() > 0)
						{
							for (int iTemp = 0; iTemp < lstTemp.Count; iTemp++)
							{
								lstTemp[iTemp].ID = lstTemp[iTemp].ID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
								if (lstTemp[iTemp].PID != -1)
									lstTemp[iTemp].PID = lstTemp[iTemp].PID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
							}
						}
						if (lstTemp != null && lstTemp.Count > 0) lstAllSelectCRFunction.AddRange(lstTemp);
					}
					_operationStatus = 1; if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
					{
						_dicPoolingWindowOperation[ip.PoolingName] = 1;
					}
					else
					{
						_dicPoolingWindowOperation.Add(ip.PoolingName, 1);
					}
					ip.lstAllSelectCRFuntion = lstAllSelectCRFunction;
					if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
						incidenceBusinessCardRenderer.lstExists = dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Select(p => p.CRSelectFunction.CRID).ToList();
					else
						incidenceBusinessCardRenderer.lstExists = new List<int>();
					olvAvailable.Refresh();
					initTreeView(ip);
					foreach (OLVListItem olvi in olvAvailable.Items)
					{
						olvi.Checked = false;
					}

				}
				catch (Exception ex)
				{
					Logger.LogError(ex);
				}
				return;
				try
				{
					List<CRSelectFunctionCalculateValue> lstAvailable = new List<CRSelectFunctionCalculateValue>();
					foreach (CRSelectFunctionCalculateValue cr in olvAvailable.SelectedObjects)
					{
						lstAvailable.Add(cr);

					}
					if (lstAvailable.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID).Distinct().Count() > 1)
					{
						MessageBox.Show("Pooling requires that functions have the same endpoint group.");
						return;
					}
					IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
					if (ip.lstAllSelectCRFuntion != null && ip.lstAllSelectCRFuntion.Count > 0)
					{
						if (lstAvailable.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID != ip.lstAllSelectCRFuntion.Where(a => a.NodeType == 4).First().CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID)
						{
							MessageBox.Show("Pooling requires that functions have the same endpoint group.");
							return;

						}
						lstAvailable = new List<CRSelectFunctionCalculateValue>();
						var queryCRID = ip.lstAllSelectCRFuntion.Where(a => a.NodeType == 4).Select(p => p.CRSelectFunctionCalculateValue.CRSelectFunction.BenMAPHealthImpactFunction.ID);
						foreach (CRSelectFunctionCalculateValue cr in olvAvailable.SelectedObjects)
						{
							lstAvailable.Add(cr);

						}

						var query = lstAvailable.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint).ToList().Distinct().ToList();
						int i = ip.lstAllSelectCRFuntion.Max(p => p.ID) + 1;
						int iEndPoint = 0, iAuthor = 0, iQualifier = 0;
						string strTemp = "";
						for (int iquery = 0; iquery < query.Count(); iquery++)
						{
							strTemp = query[iquery];
							var queryEndPoint = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 1 && p.Name == strTemp).ToList();
							if (queryEndPoint.Count() == 0)
							{
								ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
								{
									NodeType = 1,
									ID = i,
									Name = query[iquery],
									PID = 0,
									PoolingMethod = "None",


								});
								iEndPoint = i;
								i++;
							}
							else
							{
								iEndPoint = queryEndPoint.First().ID;
							}
							var author = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery]).Select(a => a.CRSelectFunction.BenMAPHealthImpactFunction.Author).Distinct().ToList();
							for (int iauthor = 0; iauthor < author.Count; iauthor++)
							{
								var queryAuthor = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 2 && p.Name == author[iauthor] && p.PID == iEndPoint);
								if (queryAuthor.Count() == 0)
								{

									ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
									{
										NodeType = 2,
										ID = i,
										Name = author[iauthor],
										PID = iEndPoint,
										PoolingMethod = "None",


									});
									iAuthor = i;
									i++;
								}
								else
								{
									iAuthor = queryAuthor.First().ID;

								}
								var Qualifier = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor] && p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery]).Select(a => a.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier).Distinct().ToList();
								for (int iQ = 0; iQ < Qualifier.Count; iQ++)
								{
									var queryQualifier = ip.lstAllSelectCRFuntion.Where(p => p.NodeType == 3 && p.Name == Qualifier[iQ] && p.PID == iAuthor);
									if (queryQualifier.Count() == 0)
									{
										ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
										{
											NodeType = 3,
											ID = i,
											Name = Qualifier[iQ],
											PID = iAuthor,
											PoolingMethod = "None",


										});
										iQualifier = i;
										i++;

									}
									else
									{
										iQualifier = queryQualifier.First().ID;
									}
									var funtion = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor] && p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery] && p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier == Qualifier[iQ]).ToList();
									for (int ifunction = 0; ifunction < funtion.Count; ifunction++)
									{
										ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
										{
											NodeType = 4,
											ID = i,
											Name = funtion[ifunction].CRSelectFunction.BenMAPHealthImpactFunction.strLocations,
											PID = iQualifier,
											CRSelectFunctionCalculateValue = funtion[ifunction]


										});
										i++;
									}
								}

							}


						}




					}
					else if (ip.lstAllSelectCRFuntion == null || ip.lstAllSelectCRFuntion.Count == 0)
					{
						ip.lstAllSelectCRFuntion = new List<AllSelectCRFunction>();
						ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
						{
							NodeType = 0,
							EndPointGroupID = lstAvailable.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroupID,
							ID = 0,
							Name = lstAvailable.First().CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup,
							PID = -1,
							PoolingMethod = "None",


						});
						var query = lstAvailable.Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint).ToList().Distinct().ToList();
						int i = 1;
						int iEndPoint = 0, iAuthor = 0, iQualifier = 0;
						for (int iquery = 0; iquery < query.Count(); iquery++)
						{
							ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
							{
								NodeType = 1,
								ID = i,
								Name = query[iquery],
								PID = 0,
								PoolingMethod = "None",


							});
							iEndPoint = i;
							i++;
							var author = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery]).Select(a => a.CRSelectFunction.BenMAPHealthImpactFunction.Author).Distinct().ToList();
							for (int iauthor = 0; iauthor < author.Count; iauthor++)
							{
								ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
								{
									NodeType = 2,
									ID = i,
									Name = author[iauthor],
									PID = iEndPoint,
									PoolingMethod = "None",


								});
								iAuthor = i;
								i++;
								var Qualifier = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery] && p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor]).Select(p => p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier).Distinct().ToList();
								for (int iqualifier = 0; iqualifier < Qualifier.Count; iqualifier++)
								{
									ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
									{
										NodeType = 3,
										ID = i,
										Name = Qualifier[iqualifier],
										PID = iAuthor,
										PoolingMethod = "None",


									});
									iQualifier = i;
									i++;
									var function = lstAvailable.Where(p => p.CRSelectFunction.BenMAPHealthImpactFunction.EndPoint == query[iquery] && p.CRSelectFunction.BenMAPHealthImpactFunction.Author == author[iauthor] && p.CRSelectFunction.BenMAPHealthImpactFunction.Qualifier == Qualifier[iqualifier]).ToList();
									for (int ifunction = 0; ifunction < function.Count(); ifunction++)
									{
										ip.lstAllSelectCRFuntion.Add(new AllSelectCRFunction()
										{
											NodeType = 4,
											ID = i,
											Name = function[ifunction].CRSelectFunction.BenMAPHealthImpactFunction.strLocations,
											PID = iQualifier,
											CRSelectFunctionCalculateValue = function[ifunction]


										});

										i++;

									}
								}

							}


						}


					}
					initTreeView(ip);
				}
				catch (Exception ex)
				{

				}

			}
		}

		private void btAddAllStudy_Click(object sender, EventArgs e)
		{
			addSelectedOrAllStudies(1);
		}

		private void btRemoveAllStudy_Click(object sender, EventArgs e)
		{
			removeSelectedOrAllStudies(1);
		}

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void groupBox11_Enter(object sender, EventArgs e)
		{

		}

		private void olvAvailable_Click(object sender, EventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				//When shift key is pressed and hold
				foreach (OLVListItem olvi in olvAvailable.SelectedItems)
					olvi.Checked = true;
			}
			else if (Control.ModifierKeys == Keys.Control)
			{
				//When control key is pressed and hold
				foreach (OLVListItem olvi in olvAvailable.SelectedItems)
					olvi.Checked = true;
			}
			else
			{
				foreach (OLVListItem olvi in olvAvailable.SelectedItems)
					olvi.Checked = !olvi.Checked;
			}
		}

		private void btPoolingPreview_Click(object sender, EventArgs e)
		{
			foreach (IncidencePoolingAndAggregation ip in CommonClass.lstIncidencePoolingAndAggregation)
			{
				if (ip.lstAllSelectCRFuntion == null || ip.lstAllSelectCRFuntion.Count == 0)
				{
					MessageBox.Show("Please set up all pooling windows first.");
					return;
				}
			}
			PoolingPreview frm = new PoolingPreview();
			frm.Width = this.Width;
			frm.Height = this.Height;
			//frm.Left = this.Left;
			//frm.Top = this.Top;
			DialogResult rtn = frm.ShowDialog();
		}

		private void TabControlSelected_Selecting(object sender, TabControlCancelEventArgs e)
		{
			//YY: disable selecting last tab
			if (e.TabPageIndex == tabControlSelected.TabCount - 1)
				e.Cancel = true;
		}

		private void cbPoolLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			IncidencePoolingAndAggregation ip = CommonClass.lstIncidencePoolingAndAggregation.Where(p => p.PoolingName == tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text).First();
			if (ip.PoolLevel == Convert.ToInt16(cbPoolLevel.SelectedItem)) return;

			ip.PoolLevel = Convert.ToInt16(cbPoolLevel.SelectedItem);
			//btAddCRFunctions_Click(null, null);
			//olvAvailable.SelectedObjects.Clear();
			//olvAvailable.CheckedObjects.Clear();
			//addSelectedOrAllStudies(0);

			//addSelectedOrAllStudies(int addType)

			List<string> lstAvalilableEndPointGroup = new List<string>();
			List<CRSelectFunctionCalculateValue> lstAvailable = new List<CRSelectFunctionCalculateValue>();

			//get all pooling functions and endpoint groups
			if (dicTabCR.Count == 0) return;
			foreach (CRSelectFunctionCalculateValue cr in dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text])
			{
				if (cr.CRSelectFunction != null && cr.CRSelectFunction.BenMAPHealthImpactFunction != null && cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup != null)
				{
					lstAvailable.Add(cr);
				}
				//YY: compatible with old apvx files
				if (cr.CRSelectFunction.BenMAPHealthImpactFunction != null)
				{
					if (!lstAvalilableEndPointGroup.Contains(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup) && cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup != null)
					{
						lstAvalilableEndPointGroup.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup);
					}
				}

			}

			//get all columns
			List<BrightIdeasSoftware.OLVColumn> lstOLVColumns = new List<OLVColumn>();
			foreach (BrightIdeasSoftware.OLVColumn olvc2 in this.treeListView.Columns)
			{
				lstOLVColumns.Add(olvc2);
			}
			lstOLVColumns = lstOLVColumns.OrderBy(p => p.DisplayIndex).ToList();
			lstOLVColumns = lstOLVColumns.Where(p => p.DisplayIndex != 0 && p.DisplayIndex != 1 && p.DisplayIndex != 2 && p.DisplayIndex != 3).OrderBy(p => p.DisplayIndex).ToList();

			//get level pooling
			int poolLevel = ip.PoolLevel;

			//get EndGroup-Endpoints pairs
			List<AllSelectCRFunction> lstAllSelectCRFunction = new List<AllSelectCRFunction>();
			Dictionary<string, List<CRSelectFunctionCalculateValue>> dicEndPointGroupCR = new Dictionary<string, List<CRSelectFunctionCalculateValue>>();
			foreach (CRSelectFunctionCalculateValue cr in lstAvailable)
			{
				if (dicEndPointGroupCR.ContainsKey(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup))
					dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
				else
				{
					dicEndPointGroupCR.Add(cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup, new List<CRSelectFunctionCalculateValue>());
					dicEndPointGroupCR[cr.CRSelectFunction.BenMAPHealthImpactFunction.EndPointGroup].Add(cr);
				}
			}

			//*****
			lstAllSelectCRFunction = ip.lstAllSelectCRFuntion;
			if (lstAllSelectCRFunction == null) lstAllSelectCRFunction = new List<AllSelectCRFunction>();
			List<AllSelectCRFunction> lstRemoveAllSelectCRFunction = ip.lstAllSelectCRFuntion.Where(p => lstAvalilableEndPointGroup.Contains(p.EndPointGroup)).ToList();
			foreach (AllSelectCRFunction ascr in lstRemoveAllSelectCRFunction)
			{
				lstAllSelectCRFunction.Remove(ascr);
			}
			foreach (KeyValuePair<string, List<CRSelectFunctionCalculateValue>> k in dicEndPointGroupCR)
			{
				if (!lstAvalilableEndPointGroup.Contains(k.Key)) continue;
				List<AllSelectCRFunction> lstTemp = getLstAllSelectCRFunction(k.Value, lstOLVColumns.Select(p => p.Text).ToList(), k.Key, -1, poolLevel);

				if (lstAllSelectCRFunction.Count() > 0)
				{
					for (int iTemp = 0; iTemp < lstTemp.Count; iTemp++)
					{
						lstTemp[iTemp].ID = lstTemp[iTemp].ID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
						if (lstTemp[iTemp].PID != -1)
							lstTemp[iTemp].PID = lstTemp[iTemp].PID + lstAllSelectCRFunction.Max(p => p.ID) + 1;
					}
				}
				if (lstTemp != null && lstTemp.Count > 0) lstAllSelectCRFunction.AddRange(lstTemp);
			}
			_operationStatus = 1;
			if (_dicPoolingWindowOperation.ContainsKey(ip.PoolingName))
			{
				_dicPoolingWindowOperation[ip.PoolingName] = 1;
			}
			else
			{
				_dicPoolingWindowOperation.Add(ip.PoolingName, 1);
			}
			ip.lstAllSelectCRFuntion = lstAllSelectCRFunction;
			if (dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text] != null && dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Count > 0)
				incidenceBusinessCardRenderer.lstExists = dicTabCR[tabControlSelected.TabPages[tabControlSelected.SelectedIndex].Text].Select(p => p.CRSelectFunction.CRID).ToList();
			else
				incidenceBusinessCardRenderer.lstExists = new List<int>();
			olvAvailable.Refresh();
			initTreeView(ip);


		}

		private void Label3_Click(object sender, EventArgs e)
		{

		}


		private void label5_Click_1(object sender, EventArgs e)
		{

		}
	}

	public class IncidenceDropSink : SimpleDropSink
	{
		public IncidencePoolingandAggregation myIncidencePoolingandAggregation;
		public IncidenceDropSink()
		{
			this.CanDropBetween = true;
			this.CanDropOnBackground = true;
			this.CanDropOnItem = false;
		}

		public IncidenceDropSink(bool acceptDropsFromOtherLists, IncidencePoolingandAggregation incidencePoolingandAggregation)
				: this()
		{
			this.AcceptExternal = acceptDropsFromOtherLists;
			myIncidencePoolingandAggregation = incidencePoolingandAggregation;
		}

		protected override void OnModelCanDrop(ModelDropEventArgs args)
		{
			base.OnModelCanDrop(args);

			if (args.Handled)
				return;

			args.Effect = DragDropEffects.Move;

			if (!this.AcceptExternal && args.SourceListView != this.ListView)
			{
				args.Effect = DragDropEffects.None;
				args.DropTargetLocation = DropTargetLocation.None;
				args.InfoMessage = "This list doesn't accept drops from other lists";
			}

			if (args.DropTargetLocation == DropTargetLocation.Background && args.SourceListView == this.ListView)
			{
				args.Effect = DragDropEffects.None;
				args.DropTargetLocation = DropTargetLocation.None;
			}
		}

		protected override void OnModelDropped(ModelDropEventArgs args)
		{

			if (!args.Handled)
				this.RearrangeModels(args);
		}

		public virtual void RearrangeModels(ModelDropEventArgs args)
		{
			switch (args.DropTargetLocation)
			{
				case DropTargetLocation.AboveItem:
					if (this.ListView.Name == "treeListView" && args.SourceListView.Name != "treeListView")
					{
						myIncidencePoolingandAggregation.btAddCRFunctions_Click(null, null);

					}
					else if (this.ListView.Name == "treeListView" && args.SourceListView.Name == "treeListView")
					{

					}
					break;
				case DropTargetLocation.BelowItem:
					if (this.ListView.Name == "treeListView" && args.SourceListView.Name != "treeListView")
					{
						myIncidencePoolingandAggregation.btAddCRFunctions_Click(null, null);
					}
					else if (this.ListView.Name == "treeListView" && args.SourceListView.Name == "treeListView")
					{

					}
					break;
				case DropTargetLocation.Background:
					if (this.ListView.Name == "treeListView" && args.SourceListView.Name != "treeListView")
					{
						myIncidencePoolingandAggregation.btAddCRFunctions_Click(null, null);

					}
					else
					{

					}
					break;
				default:
					return;
			}

			if (args.SourceListView != this.ListView && args.SourceListView.Name == "treeListView")
			{
				args.SourceListView.RemoveObjects(args.SourceModels);
			}
		}
	}

}
