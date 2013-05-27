using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace CertificateScanner.ExceptionDecor
{

    /// <summary>
    /// Exception and log decorator
    /// </summary>
    public static class ExceptionExtension
    {

        private static void ErrorLog(ExceptionWrapper.ErrorTypes type, Exception ex, string message)
        {
            if (ex is ExceptionWrapper)
                throw ex;

            throw new ExceptionWrapper()
            {
                ex = ex,
                message = message,
                withMessage = String.IsNullOrWhiteSpace(message) ? false : true,
                type = type
            };
        }

        #region Fatal

        /// <summary>
        /// Crash error without message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        public static void Fatal(this Control Control, Exception ex, string message = "")
        {
            ErrorLog(ExceptionWrapper.ErrorTypes.Fatal, ex, message);
        }


        /// <summary>
        /// Crash error with message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Fatal(this Component Control, Exception ex, string message = "")
        {
            ErrorLog(ExceptionWrapper.ErrorTypes.Fatal, ex, message);
        }

        #endregion

        #region Error

        /// <summary>
        /// Simple Error with message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Error(this Control Control, Exception ex, string message = "")
        {
            ErrorLog(ExceptionWrapper.ErrorTypes.Error, ex, message);
        }
        
        /// <summary>
        /// Simple Error with message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Error(this Component Control, Exception ex, string message = "")
        {
            ErrorLog(ExceptionWrapper.ErrorTypes.Error, ex, message);
        }
        #endregion

        #region Warning

        /// <summary>
        /// Warning(input data error for example) without message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Warn(this Control Control, Exception ex, string message = "")
        {
            ErrorLog(ExceptionWrapper.ErrorTypes.Warning, ex, message);
        }
        
        /// <summary>
        /// Warning(input data error for example) without message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Warn(this Component Control, Exception ex, string message = "")
        {
            ErrorLog(ExceptionWrapper.ErrorTypes.Warning, ex, message);
        }
        #endregion 

        #region Info
        /// <summary>
        /// Information 
        /// </summary>
        /// <param name="comment">description</param>
        /// <param name="data">key/value pair of data</param>
        /// <param name="message">MessageBox text</param>
        public static void Info(this Control Control, string comment, string message = "", ICollection<DictionaryEntry> data = null)
        {
            ExceptionDecorator.Info(comment, message, data);
        }
       
        /// <summary>
        /// Information 
        /// </summary>
        /// <param name="comment">description</param>
        /// <param name="data">key/value pair of data</param>
        /// <param name="message">MessageBox text</param>
        public static void Info(this Component Control, string comment, string message = "", ICollection<DictionaryEntry> data = null)
        {
            ExceptionDecorator.Info(comment, message, data);
        }
        #endregion

        /// <summary>
        /// Not implemented
        /// </summary>
        public static void Debug()
        {
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public static void Trace()
        {
        }
    }
}
