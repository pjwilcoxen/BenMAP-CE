using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.ExceptionServices;
using System.Threading;


namespace BenMAP
{
	static class Program
	{
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static extern bool AllocConsole();

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static extern bool AttachConsole(int pid);

		[STAThread]
		static int Main(string[] arg)
		{
			string strArg = "";
			foreach (string s in arg)
			{
				strArg = strArg + " " + s;
			}
			strArg = strArg.Trim();
			CommonClass.InputParams = new string[] { strArg };

			// Check for a recognized command line argument and set
			// the appropriate mode flag
			if(strArg != "")
			{
				bool badArg = false;

				string lowerArg = strArg.ToLower();
				if (lowerArg.Contains(".ctlx"))
					CommonClass.BatchMode = true;
				else if (lowerArg.Contains(".projx"))
					CommonClass.ProjectMode = true;
				else if (lowerArg.Contains(".smat"))
					CommonClass.SMATMode = true;
				else
					badArg = true;

				if( (CommonClass.BatchMode || badArg) && !AttachConsole(-1) )
					AllocConsole();

				if (badArg)
				{
					Console.WriteLine("Incorrect command line argument:"+strArg);
					Console.WriteLine("Expected a ctlx, projx or smat file.");
					Environment.Exit(0);
				}
			}

			if ( CommonClass.BatchMode )
			{
				if (!File.Exists(strArg))
				{
					Console.WriteLine("Control file not found: "+strArg);
					Environment.Exit(0);
				}
			}
			else
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				AppDomain currentDomain = AppDomain.CurrentDomain;
				// Add handler for UI thread exceptions
				Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionHandler);
				// Force all WinForms errors to go through handler
				Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
				// This handler is for catching non-UI thread exceptions
				currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

				// This handler is for catching all exceptions, handled or not
				//currentDomain.FirstChanceException += FirstChanceExceptionHandler;
			}

			Application.Run(new Main());

			if(CommonClass.BatchMode)
				Console.WriteLine("Batch run complete");

			return 0;
		}

		//static void FirstChanceExceptionHandler(object source, FirstChanceExceptionEventArgs args)
		//{

		//    Exception ex = args.Exception;

		//    HandleException(ex);
		//}


		static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs args)
		{
			Exception ex = (Exception)args.Exception;

			HandleException(ex);
		}

		static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;

			HandleException(ex);
		}

		static void HandleException(Exception ex)
		{

			Logger.LogError(ex);

			//check for Jira Connector needed by error reporting form
			//if no jira connector, then terminate application
			if (String.IsNullOrEmpty(CommonClass.JiraConnectorFilePath) || String.IsNullOrEmpty(CommonClass.JiraConnectorFilePathTXT))
			{
				MessageBox.Show("The application encountered the following fatal error and will now terminate." + Environment.NewLine + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK);
				Environment.Exit(0);
			}


			//show error reporting form unless error is in error reporting form
			if (ex.StackTrace.IndexOf("ErrorReporting", StringComparison.OrdinalIgnoreCase) < 0)
			{
				try
				{
					DialogResult dialogResult = MessageBox.Show("The application encountered the following fatal error:" + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "Would you like to report the error to the BenMAP-CE development team before the application terminates?", "Error", MessageBoxButtons.YesNo);

					if (dialogResult == DialogResult.Yes)
					{
						ErrorReporting frm = new ErrorReporting();
						string err = ex.StackTrace + Environment.NewLine + Environment.NewLine + "Please enter any additional information about the error that might prove useful:";
						frm.ErrorMessage = err;

						frm.ShowDialog();
					}

					Environment.Exit(0);

				}
				catch (Exception ex2)
				{
					//quit if error occurs opening or after opening error reporting form
					Logger.LogError(ex2);
					Environment.Exit(0);
				}
			}
			else //this error occurred in Error Reporting so alert user then quit
			{
				MessageBox.Show("The application encountered the following fatal error and will now terminate." + Environment.NewLine + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK);
				Environment.Exit(0);
			}

		}




	}
}
