using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using DotSpatial.Data;
using ESIL.DBUtility;
using FirebirdSql.Data.FirebirdClient;

namespace BenMAP.Crosswalks
{
	internal class DAL : IDisposable
	{
		private readonly FbConnection _connection;
		private readonly FireBirdHelperBase _fbh;
		private readonly bool _closeUnderlyingConnection;

		public DAL(FbConnection connection)
		{
			if (connection == null) throw new ArgumentNullException("connection");
			_connection = connection;
			_fbh = new ESILFireBirdHelper();
			if (_connection.State != ConnectionState.Open)
			{
				_connection.Open();
				_closeUnderlyingConnection = true;
			}
		}

		private int ExecuteNonQuery(string commandText, FbTransaction transaction = null)
		{
			var result = transaction != null ?
					_fbh.ExecuteNonQuery(transaction, CommandType.Text, commandText) :
					_fbh.ExecuteNonQuery(_connection, CommandType.Text, commandText);

			return result;
		}

		private object ExecuteScalar(string commandText, FbTransaction transaction = null)
		{
			var result = transaction != null ?
					_fbh.ExecuteScalar(transaction, CommandType.Text, commandText) :
					_fbh.ExecuteScalar(_connection, CommandType.Text, commandText);

			return result;
		}

		#region Public methods

		public void DeleteAllCrosswalks(int setupId)
		{
			using (var tran = _connection.BeginTransaction())
			{
				ExecuteNonQuery(string.Format("DELETE from GRIDDEFINITIONPERCENTAGES where SOURCEGRIDDEFINITIONID in (SELECT GRIDDEFINITIONID from GRIDDEFINITIONS where setupID={0} and Locked!='T')", setupId), tran);
				ExecuteNonQuery(string.Format("DELETE from GRIDDEFINITIONPERCENTAGES where TARGETGRIDDEFINITIONID in (SELECT GRIDDEFINITIONID from GRIDDEFINITIONS where setupID={0} and Locked!='T')", setupId), tran);
				ExecuteNonQuery("DELETE from GRIDDEFINITIONPERCENTAGEENTRIES where PERCENTAGEID not in (SELECT PERCENTAGEID from GRIDDEFINITIONPERCENTAGES)", tran);

				tran.Commit();
			}
		}

		public void DeleteCrosswalk(int grid1, int grid2)
		{
			/* dpa 1/28/2017 this function is used to clear the contents of the crosswalk tables just for the two selected entries.
			 * The two tables of interest are: GRIDDEFINITIONPERCENTAGES and GRIDDEFINITIONPERCENTAGEENTRIES.
			 */

			using (var tran = _connection.BeginTransaction())
			{
				DeleteGridPercentage(grid1, grid2, tran);
				DeleteGridPercentage(grid2, grid1, tran);

				tran.Commit();
			}
		}

		private void DeleteGridPercentage(int sourceId, int targetId, FbTransaction tran)
		{
			// Find the correct percentageid entry for the forward direction crosswalk
			var commandText = string.Format("SELECT PERCENTAGEID from GRIDDEFINITIONPERCENTAGES where SOURCEGRIDDEFINITIONID={0} and TARGETGRIDDEFINITIONID={1}", sourceId, targetId);
			var res = ExecuteScalar(commandText, tran);

			if (res == DBNull.Value) return; // Nothing to remove, exit

			var iResult = Convert.ToInt32(res);

			// Remove this percentageid entry
			commandText = string.Format("DELETE from GRIDDEFINITIONPERCENTAGEENTRIES where PERCENTAGEID={0}", iResult);
			ExecuteNonQuery(commandText, tran);

			// Remove all data entries for this percentageid
			commandText = string.Format("DELETE from GRIDDEFINITIONPERCENTAGES where PERCENTAGEID={0}", iResult);
			ExecuteNonQuery(commandText, tran);
		}

		public DataTable GetSetups()
		{
			var commandText = "select SetupID,SetupName from Setups order by SetupID";
			var ds = _fbh.ExecuteDataset(_connection, commandText);
			return ds.Tables[0];
		}

		public DataTable GetGridDefinitions(int setupId)
		{
			var commandText = string.Format("select GridDefinitionName, GridDefinitionID, Locked from GridDefinitions where setupID={0}", setupId);
			var ds = _fbh.ExecuteDataset(_connection, commandText);
			return ds.Tables[0];
		}

		public bool CrosswalkExists(int grid1, int grid2)
		{
			bool forwardExists = false, backwardExists = false;

			var commandText = string.Format("select Count(*) from GridDefinitionPercentages where SOURCEGRIDDEFINITIONID={0} AND TARGETGRIDDEFINITIONID={1}", grid1, grid2);
			var iResult = Convert.ToInt32(ExecuteScalar(commandText));
			if (iResult > 0)
			{
				forwardExists = true;
			}

			commandText = string.Format("select Count(*) from GridDefinitionPercentages where SOURCEGRIDDEFINITIONID={0} AND TARGETGRIDDEFINITIONID={1}", grid2, grid1);
			iResult = Convert.ToInt32(ExecuteScalar(commandText));
			if (iResult > 0)
			{
				backwardExists = true;
			}

			return forwardExists && backwardExists;
		}

		public string GetShapeFilenameForGrid(int gridId)
		{
			var commandText = string.Format(@"select ShapeFileName from ShapeFileGridDefinitionDetails where GridDefinitionID={0}
                union SELECT SHAPEFILENAME FROM REGULARGRIDDEFINITIONDETAILS where griddefinitionid = {0}", gridId);
			return ExecuteScalar(commandText).ToString();
		}

		public string GetGridDefinitionName(int grid)
		{
			var commandText = string.Format("select GRIDDEFINITIONNAME from GRIDDEFINITIONS where GRIDDEFINITIONID={0}", grid);
			return ExecuteScalar(commandText).ToString();
		}

		public void InsertCrosswalks(int grid1, int grid2, IFeatureSet fsInput1, IFeatureSet fsInput2, IList<Crosswalk> results, CancellationToken ctsToken, IProgress progress)
		{
			using (var tran = _connection.BeginTransaction())
			{
				// first we need to add entries to the griddefinitionpercentages table                

				var commandText = "select max(PercentageID) from GridDefinitionPercentages";
				var maxId = ExecuteScalar(commandText, tran);
				var percentageId1 = maxId == DBNull.Value ? 1 : Convert.ToInt32(maxId) + 1;
				var percentageId2 = percentageId1 + 1;

				commandText = string.Format("insert into GridDefinitionPercentages(PERCENTAGEID, SOURCEGRIDDEFINITIONID, TARGETGRIDDEFINITIONID) values({0},{1},{2})", percentageId1, grid1, grid2);
				ExecuteNonQuery(commandText, tran);

				commandText = string.Format("insert into GridDefinitionPercentages(PERCENTAGEID,SOURCEGRIDDEFINITIONID, TARGETGRIDDEFINITIONID) values({0},{1},{2})", percentageId2, grid2, grid1);
				ExecuteNonQuery(commandText, tran);

				int i = 0, j = 1;
				var fieldNames = new[] { "COL", "ROW" };
				var step = results.Count / 100;
				foreach (var entry in results)
				{
					//update the progress bar to show progress writing output to database - only 100 progres steps.
					i += 1;
					if (i > step * j)
					{
						j += 1;
						var prog = Convert.ToSingle(100 * i / results.Count);
						progress.OnProgressChanged(string.Format("{0} of {1} written to database.", i, results.Count), prog);
					}

					var forward = entry.ForwardRatio;
					var backward = entry.BackwardRatio;

					const float PRECISION = (float)1e-6;
					if (forward > PRECISION || backward > PRECISION)
					{
						// Get the column and row attributes and forward backward results
						var attributes1 = fsInput1.GetAttributes(entry.FeatureId1, 1, fieldNames);
						var attributes2 = fsInput2.GetAttributes(entry.FeatureId2, 1, fieldNames);

						// Write the entries to the firebird database
						commandText =
								 string.Format(
										 "insert into GridDefinitionPercentageEntries(PERCENTAGEID, SOURCECOLUMN, SOURCEROW, TARGETCOLUMN, TARGETROW, PERCENTAGE,NORMALIZATIONSTATE) values({0},{1},{2},{3},{4},{5},{6})",
										 percentageId1, attributes1.Rows[0]["COL"], attributes1.Rows[0]["ROW"],
										 attributes2.Rows[0]["COL"], attributes2.Rows[0]["ROW"], forward, 0);

						ctsToken.ThrowIfCancellationRequested();
						ExecuteNonQuery(commandText, tran);

						commandText =
									string.Format(
											"insert into GridDefinitionPercentageEntries(PERCENTAGEID, SOURCECOLUMN, SOURCEROW, TARGETCOLUMN, TARGETROW, PERCENTAGE,NORMALIZATIONSTATE) values({0},{1},{2},{3},{4},{5},{6})",
											percentageId2, attributes2.Rows[0]["COL"], attributes2.Rows[0]["ROW"],
											attributes1.Rows[0]["COL"], attributes1.Rows[0]["ROW"], backward, 0);

						ctsToken.ThrowIfCancellationRequested();
						ExecuteNonQuery(commandText, tran);
					}
				}

				ctsToken.ThrowIfCancellationRequested();
				tran.Commit();
			}
		}

		public void Dispose()
		{
			if (_closeUnderlyingConnection)
			{
				_connection.Close();
				_connection.Dispose();

				// Wait a bit and give some time to firebird to release it's resources.
				Thread.Sleep(1000);
			}
		}

		#endregion
	}
}