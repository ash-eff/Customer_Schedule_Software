using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace software_2_c969
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.ThreadException += Application_ThreadException;
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormLogin());
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
			string errorMessage = "An unexpected error has occurred:\n\n" + e.Exception.Message;
			MessageBox.Show(errorMessage, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
	}
}
