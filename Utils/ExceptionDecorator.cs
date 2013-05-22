using System;
using System.Collections;
using System.Collections.Generic;
using NLog;
using System.Windows.Forms;
using CertificateScanner.Utils;

namespace CertificateScanner.ExceptionDecor
{

    /// <summary>
    /// Exception and log decorator
    /// </summary>
    internal class ExceptionDecorator
    {
        #region closed constructor


        #endregion

        #region Private Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private static void ErrorLog(string type, Exception ex, string message = "")
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            string baseString = String.Format("User:\"{0}\"; SystemMessage:\"{1}\"; Source:\"{2}\"; StackTrace:\"{3}\"; Method:\"{4}\"; ", Settings.Instance.Constant("baseUser"), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite);

            if (!String.IsNullOrWhiteSpace(message))
                baseString += String.Format("UserMessage:\"{0}\"; ", message);
            
            bool debugDataLevel = false;
            bool debugDataLevelChanged = false;

            try
            {
                debugDataLevel = (Settings.Instance.Constant("debugDataLevel") != "false") ? true : false;
                debugDataLevelChanged = true;
            }
            catch(Exception ex1)
            {
                ExceptionDecorator.Warn(ex);

                if (!debugDataLevelChanged)
                    debugDataLevel = false;
            }

            if (debugDataLevel)
            {
                baseString += "Data :";
                foreach (DictionaryEntry de in ex.Data)
                    baseString += String.Format("({0};{1}) ", de.Key, de.Value);
                baseString += "; ";
                if ((ex is ArgumentException) || (ex is ArgumentNullException))
                    baseString += String.Format("ParamName : \"{0}\";", (ex as ArgumentException).ParamName);
            }

            if (Settings.Instance.Constant("EncryptLog") != "false")
            {
                Crypto cryptString = new Crypto();
                baseString = cryptString.encryptStringToString_AES(baseString);
            }

            switch (type)
            {
                case "Fatal":
                    logger.Fatal(baseString);
                    break;
                case "Error":
                    logger.Error(baseString);
                    break;
                case "Warn":
                    logger.Warn(baseString);
                    break;
                default:
                    logger.Error(baseString);
                    break;
            }

            if (!String.IsNullOrWhiteSpace(message))
                MessageBox.Show(message);
        }

        private static void InfoLog(string comment, ICollection<DictionaryEntry> data = null, string message = "")
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            string baseString = String.Format("User:\"{0}\"; CommentMessage:\"{1}\"; ", Settings.Instance.Constant("baseUser"), comment);

            if (!String.IsNullOrWhiteSpace(message))
                baseString += String.Format("UserMessage:\"{0}\"; ", message);
            
            bool debugDataLevel = false;
            bool debugDataLevelChanged = false;

            try
            {
                debugDataLevel = (Settings.Instance.Constant("debugDataLevel") != "false") ? true : false;
                debugDataLevelChanged = true;
            }
            finally
            {
                if (!debugDataLevelChanged)
                    debugDataLevel = false;
            }

            if ((debugDataLevel) && (data != null))
            {
                baseString += "Data :";
                foreach (DictionaryEntry de in data)
                    baseString += String.Format("({0};{1}) ", de.Key, de.Value);
                baseString += ";";
            }

            if (Settings.Instance.Constant("EncryptLog") != "false")
            {
                Crypto cryptString = new Crypto();
                baseString = cryptString.encryptStringToString_AES(baseString);
            }

            logger.Info(baseString);

            if (!String.IsNullOrWhiteSpace(message))
                MessageBox.Show(message);
        }

        #endregion

        #region Fatal

        /// <summary>
        /// Crash error without message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        public static void Fatal(Exception  ex)
        {
            ErrorLog("Fatal", ex);
        }

        /// <summary>
        /// Crash error with message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Fatal(Exception  ex, string message)
        {
            ErrorLog("Fatal", ex, message);
        }

        #endregion

        #region Error

        /// <summary>
        /// Simple Error without message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        public static void Error(Exception ex)
        {
            ErrorLog("Error", ex);
        }

        /// <summary>
        /// Simple Error with message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Error(Exception ex, string message)
        {
            ErrorLog("Error", ex, message);
        }
        #endregion

        #region Warning

        /// <summary>
        /// Warning(input data error for example) without message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        public static void Warn(Exception  ex)
        {
            ErrorLog("Warn", ex);
        }

        /// <summary>
        /// Warning(input data error for example) without message
        /// </summary>
        /// <param name="ex">Exception variable</param>
        /// <param name="message">MessageBox text</param>
        public static void Warn(Exception ex, string message)
        {
            ErrorLog("Warn", ex, message);
        }
        #endregion 

        #region Info

        /// <summary>
        /// Information 
        /// </summary>
        /// <param name="comment">description</param>
        /// <param name="data">key/value pair of data</param>
        public static void Info(string comment, ICollection<DictionaryEntry> data = null)
        {
            InfoLog(comment, data);
        }

        /// <summary>
        /// Information 
        /// </summary>
        /// <param name="comment">description</param>
        /// <param name="data">key/value pair of data</param>
        /// <param name="message">MessageBox text</param>
        public static void Info(string comment, string message, ICollection<DictionaryEntry> data = null)
        {
            InfoLog(comment, data, message);
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
