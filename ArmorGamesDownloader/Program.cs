using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArmorGamesDownloader
{
	public struct Wnd
	{
		public static String Name = "ArmorGamesDownloader " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
	}
	static class Program
	{
		
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		/*public class SingleInstanceApplication : WindowsFormsApplicationBase
		{
			private SingleInstanceApplication()
			{
				base.IsSingleInstance = true;
			}

			public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
			{
				SingleInstanceApplication app = new SingleInstanceApplication();
				app.MainForm = f;
				app.StartupNextInstance += startupHandler;
				app.Run(Environment.GetCommandLineArgs());
			}
		}*/
		[STAThread]
		static void Main(String[] args)
		{
            if (UnsafeNativeMethods.IsFirstRun)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new frmMain(args));
			}
			else
			{
				//windowName текст заголовка окна, типа Form1
				//Wnd.Name = "ArmorGamesDownloader " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                UnsafeNativeMethods.ShowWindow(Wnd.Name); // разворачивает окно и выводит на первый план
                UnsafeNativeMethods.SendArgs(Wnd.Name, args); //отправляет параметры командной строки, если нужно
				return;
			}
		}
	}
}