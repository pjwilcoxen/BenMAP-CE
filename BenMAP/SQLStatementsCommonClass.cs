﻿// ***********************************************************************
// Assembly         : BenMAP
// Author           : boberkirsch
// Created          : 04-09-2014
//
// Last Modified By : boberkirsch
// Last Modified On : 04-09-2014
// ***********************************************************************
// <copyright file="SQLStatementsCommonClass.cs" company="RTI International">
//     RTI International. All rights reserved.
// </copyright>
// <summary>
//  This class is for all and any sql commands that relate to commands doing inserts, updates, deletes, 
//  or any other type of quering against the firebird BenMap database.
//  </summary>
// ***********************************************************************
using System;
using System.Data;
using ESIL.DBUtility;
using FirebirdSql.Data.FirebirdClient;
/// <summary>
/// The BenMAP namespace.
/// </summary>
namespace BenMAP
{
	/// <summary>
	/// Class SQL Statements Common Class.
	/// 
	/// </summary>
	class SQLStatementsCommonClass
	{

		/// <summary>
		/// Inserts the metadata into the Metadata Table.
		/// </summary>
		/// <param name="metadataObj">The metadata object.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
		/// <exception cref="System.Exception"></exception>
		public static bool insertMetadata(MetadataClassObj metadataObj)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();

			int rtv = 0;
			bool bPassed = false;
			string commandText = string.Empty;
			try
			{           //The 'F' is for the locked column in metadatainformation - it is created or imported not predefined.  
									//I think this could be removed because there is no predefined metadata
				commandText = string.Format("INSERT INTO METADATAINFORMATION " +
												"(SETUPID, DATASETID, DATASETTYPEID, FILENAME, " +
												"EXTENSION, DATAREFERENCE, FILEDATE, IMPORTDATE, DESCRIPTION, " +
												"PROJECTION, GEONAME, DATUMNAME, DATUMTYPE, SPHEROIDNAME, " +
												"MERIDIANNAME, UNITNAME, PROJ4STRING, NUMBEROFFEATURES, METADATAENTRYID, LOCKED) " +
												"VALUES('{0}', '{1}', '{2}', '{3}', '{4}','{5}', '{6}', '{7}', '{8}', '{9}', " +
												"'{10}', '{11}', '{12}', '{13}', '{14}','{15}', '{16}', '{17}', {18}, 'F')",
												metadataObj.SetupId, metadataObj.DatasetId, metadataObj.DatasetTypeId, metadataObj.FileName,
												metadataObj.Extension, metadataObj.DataReference, metadataObj.FileDate, metadataObj.ImportDate,
												metadataObj.Description, metadataObj.Projection, metadataObj.GeoName, metadataObj.DatumName,
												metadataObj.DatumType, metadataObj.SpheroidName, metadataObj.MeridianName, metadataObj.UnitName,
												metadataObj.Proj4String, metadataObj.NumberOfFeatures, metadataObj.MetadataEntryId);
				rtv = fb.ExecuteNonQuery(CommonClass.Connection, CommandType.Text, commandText);

				bPassed = true;
			}
			catch (Exception ex)
			{
				throw (new Exception(ex.Message));
			}

			return bPassed;
		}

		public static bool updateMonitorsTable(int metadataId, int monitordatasetId, int pollutantId)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();
			bool bPassed = false;
			int rtv = 0;
			string commandText = string.Empty;
			try
			{
				commandText = string.Format("UPDATE MONITORS SET METADATAID = {0} WHERE MONITORDATASETID ={1} AND POLLUTANTID={2}",
																		 metadataId, monitordatasetId, pollutantId);
				rtv = fb.ExecuteNonQuery(CommonClass.Connection, CommandType.Text, commandText);
				bPassed = true;
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}

			return bPassed;
		}

		public static bool updateMetadata(MetadataClassObj metadataObj)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();
			bool bPassed = false;
			int rtv = -1;
			//int metaId = -1;
			string commandText = string.Empty;
			try
			{
				//commandText = string.Format("SELECT METADATAID FROM METADATAINFORMATION WHERE DATASETID = {0} AND SETUPID = {1} AND FILENAME = '{2}'",
				//                            metadataObj.DatasetId, metadataObj.SetupId, metadataObj.FileName);
				//metaId = Convert.ToInt32(fb.ExecuteScalar(CommonClass.Connection,CommandType.Text, commandText));

				commandText = string.Format("UPDATE METADATAINFORMATION set DATAREFERENCE = '{0}', DESCRIPTION = '{1}' " +
																		"WHERE DATASETID = {2} AND SETUPID = {3} AND METADATAENTRYID = {4}",
																		metadataObj.DataReference, metadataObj.Description, metadataObj.DatasetId,
																		metadataObj.SetupId, metadataObj.MetadataEntryId);

				//commandText = string.Format("UPDATE METADATAINFORMATION set DATAREFERENCE = '{0}', DESCRIPTION = '{1}' " +
				//               "WHERE DATASETID = {2} AND SETUPID = {3} AND METADATAID = {4}",
				//               metadataObj.DataReference, metadataObj.Description, metadataObj.DatasetId,
				//               metadataObj.SetupId, metadataObj.MetadataId);

				rtv = fb.ExecuteNonQuery(CommonClass.Connection, CommandType.Text, commandText);
				if (rtv == 0)
				{
					throw (new Exception("Metadata did not get updated"));
				}
				bPassed = true;

			}
			catch (Exception ex)
			{
				throw (new Exception(ex.Message));
			}

			return bPassed;
		}

		/// <summary>
		/// Gets the dataset identifier.
		/// Selects Dataset Id from Datasets table where Dataset Name is the name passed in.
		/// "SELECT DATASETTYPEID FROM DATASETTYPES WHERE DATASETTYPENAME = '{0}'"
		/// </summary>
		/// <param name="datasetname">The datasetname.</param>
		/// <returns>System.Int32.</returns>
		/// <exception cref="System.Exception"></exception>
		public static int getDatasetID(string datasetname)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();
			int rtvID;
			string commandText = string.Empty;
			try
			{
				commandText = string.Format("SELECT DATASETTYPEID FROM DATASETTYPES WHERE DATASETTYPENAME = '{0}'", datasetname);
				rtvID = Convert.ToInt32(fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, commandText));
			}
			catch (Exception ex)
			{
				throw (new Exception(ex.Message));
			}

			return rtvID;
		}


		//this version of getMetadata is used
		//by incidence and inflation datasets
		// 2015 09 22 BENMAP-322 - fix problems with metadata not displaying when two different dataset types ahve the same datasetID
		// - in other words, to retrieve metadata id from metadatainformation table, the datasettypeid is always needed
		public static MetadataClassObj getMetadata(int datasetID, int setupId, int datasetTypeID)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();
			//FbDataReader fbDataReader = null;
			MetadataClassObj _metadataObj = new MetadataClassObj();
			DataSet ds = null;

			string commandText = string.Format("SELECT METADATAID, METADATAENTRYID, SETUPID, DATASETID, DATASETTYPEID, FILENAME, " +
								"EXTENSION, DATAREFERENCE, FILEDATE, IMPORTDATE, DESCRIPTION, " +
								"PROJECTION, GEONAME, DATUMNAME, DATUMTYPE, SPHEROIDNAME, " +
								"MERIDIANNAME, UNITNAME, PROJ4STRING, NUMBEROFFEATURES " +
								"FROM METADATAINFORMATION " +
								"WHERE DATASETID = '{0}' AND SETUPID = '{1}' AND DATASETTYPEID = {2}", datasetID, setupId, datasetTypeID);

			//fbDataReader = fb.ExecuteReader(CommonClass.Connection, CommandType.Text, commandText);
			ds = fb.ExecuteDataset(CommonClass.Connection, CommandType.Text, commandText);
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				_metadataObj.MetadataEntryId = Convert.ToInt32(dr["METADATAENTRYID"]);//Convert.ToInt32(dr["METADATAID"]);
				_metadataObj.SetupId = Convert.ToInt32(dr["SETUPID"]);
				_metadataObj.DatasetId = Convert.ToInt32(dr["DATASETID"]);
				_metadataObj.DatasetTypeId = Convert.ToInt32(dr["DATASETTYPEID"]);
				_metadataObj.FileName = dr["FILENAME"].ToString();
				_metadataObj.Extension = dr["EXTENSION"].ToString();
				_metadataObj.DataReference = dr["DATAREFERENCE"].ToString();
				_metadataObj.FileDate = dr["FILEDATE"].ToString();
				_metadataObj.ImportDate = dr["IMPORTDATE"].ToString();
				_metadataObj.Description = dr["DESCRIPTION"].ToString();
				_metadataObj.Projection = dr["PROJECTION"].ToString();
				_metadataObj.GeoName = dr["GEONAME"].ToString();
				_metadataObj.DatumName = dr["DATUMNAME"].ToString();
				_metadataObj.DatumType = dr["DATUMTYPE"].ToString();
				_metadataObj.SpheroidName = dr["SPHEROIDNAME"].ToString();
				_metadataObj.MeridianName = dr["MERIDIANNAME"].ToString();
				_metadataObj.UnitName = dr["UNITNAME"].ToString();
				_metadataObj.Proj4String = dr["PROJ4STRING"].ToString();
				_metadataObj.NumberOfFeatures = dr["NUMBEROFFEATURES"].ToString();

			}
			return _metadataObj;
		}

		//this version of getMetadata is called by
		//incomegrowth, grid definitions, health impact, monitor, population, valuation function,
		//and variable datasets
		public static MetadataClassObj getMetadata(int datasetID, int setupId, int datasetTypeId, int metadataentryid)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();
			MetadataClassObj _metadataObj = new MetadataClassObj();
			DataSet ds = null;

			string commandText = string.Format("SELECT METADATAID, METADATAENTRYID, SETUPID, DATASETID, DATASETTYPEID, FILENAME, " +
								"EXTENSION, DATAREFERENCE, FILEDATE, IMPORTDATE, DESCRIPTION, " +
								"PROJECTION, GEONAME, DATUMNAME, DATUMTYPE, SPHEROIDNAME, " +
								"MERIDIANNAME, UNITNAME, PROJ4STRING, NUMBEROFFEATURES " +
								"FROM METADATAINFORMATION " +
								"WHERE DATASETID = '{0}' AND SETUPID = '{1}' AND DATASETTYPEID = '{2}' AND METADATAENTRYID = '{3}'",
								datasetID, setupId, datasetTypeId, metadataentryid);
			//datasetID, setupId, datasetTypeId, metadataId);

			ds = fb.ExecuteDataset(CommonClass.Connection, CommandType.Text, commandText);
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				_metadataObj.MetadataEntryId = Convert.ToInt32(dr["METADATAENTRYID"]);//Convert.ToInt32(dr["METADATAID"]);
				_metadataObj.SetupId = Convert.ToInt32(dr["SETUPID"]);
				_metadataObj.DatasetId = Convert.ToInt32(dr["DATASETID"]);
				_metadataObj.DatasetTypeId = Convert.ToInt32(dr["DATASETTYPEID"]);
				_metadataObj.FileName = dr["FILENAME"].ToString();
				_metadataObj.Extension = dr["EXTENSION"].ToString();
				_metadataObj.DataReference = dr["DATAREFERENCE"].ToString();
				_metadataObj.FileDate = dr["FILEDATE"].ToString();
				_metadataObj.ImportDate = dr["IMPORTDATE"].ToString();
				_metadataObj.Description = dr["DESCRIPTION"].ToString();
				_metadataObj.Projection = dr["PROJECTION"].ToString();
				_metadataObj.GeoName = dr["GEONAME"].ToString();
				_metadataObj.DatumName = dr["DATUMNAME"].ToString();
				_metadataObj.DatumType = dr["DATUMTYPE"].ToString();
				_metadataObj.SpheroidName = dr["SPHEROIDNAME"].ToString();
				_metadataObj.MeridianName = dr["MERIDIANNAME"].ToString();
				_metadataObj.UnitName = dr["UNITNAME"].ToString();
				_metadataObj.Proj4String = dr["PROJ4STRING"].ToString();
				_metadataObj.NumberOfFeatures = dr["NUMBEROFFEATURES"].ToString();

			}
			return _metadataObj;

		}

		public static int getMetadataEntryID(int setupId, int datasetId, int datasettypeid)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();
			int rtv;
			string commandText = string.Empty;
			try
			{
				commandText = string.Format("SELECT METADATAENTRYID FROM METADATAINFORMATION WHERE SETUPID = {0} AND DATASETID = {1} AND DATASETTYPEID = {2}", setupId, datasetId, datasettypeid);
				rtv = Convert.ToInt32(fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, commandText));
			}
			catch (Exception ex)
			{
				throw (new Exception(ex.Message));
			}

			return rtv;
		}

		public static int selectMaxID(string nameId, string fromTableName)
		{
			FireBirdHelperBase fb = new ESILFireBirdHelper();
			int rtvID = -1;
			string commandText = string.Empty;
			try
			{
				commandText = string.Format("select max({0}) from {1}", nameId, fromTableName);
				object rtv = fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, commandText);
				if (string.IsNullOrEmpty(rtv.ToString()))
				{
					rtvID = 1;
				}
				else
				{
					rtvID = Convert.ToInt32(rtv.ToString()) + 1;
				}

			}
			catch (Exception ex)
			{

				throw (new Exception(ex.Message));
			}

			return rtvID;
		}


	}
}
