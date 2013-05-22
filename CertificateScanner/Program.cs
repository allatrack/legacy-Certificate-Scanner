using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace CertificateScanner
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Subscribe to thread (unhandled) exception events
            

            Application.ThreadException +=
                new ThreadExceptionEventHandler(
                    CertificateScanner.ExceptionDecor.ThreadExceptionHandler.Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        public static string AppPath { get { return Application.StartupPath; } }
    }
}
