using System;
using System.Windows.Forms;


namespace blob
{
	/// <summary>
	/// Summary description for blob.
	/// </summary>
	public class BlobException
	{

		public static void BlobUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			string Message = MyHandler(e);
			System.Windows.Forms.MessageBox.Show("BlobUnhandledException caught : "+Environment.NewLine + Message);
		}

		public static string MyHandler(Exception exception)
		{
			string exMessage = "";
			exMessage += (exception.Message + Environment.NewLine) ;
			exMessage += (exception.Source + Environment.NewLine) ;
			exMessage += (exception.StackTrace + Environment.NewLine) ;
			return exMessage;
		}
	}
}
