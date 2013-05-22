using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace CertificateScanner.Utils
{
    public static class ConstantAndMessages
    {
        /// <summary>
        /// Application constant. Getting from settings.ini
        /// </summary>
        public static string Constant(this Control Control, string Key)
        {
            return Settings.Instance.Constant(Key);
        }

        /// <summary>
        /// Application messages and errors. Getting from settings.ini
        /// </summary>
        public static string Messages(this Control Control, string Key)
        {
            return Settings.Instance.Messages(Key);
        }

        /// <summary>
        /// Application constant. Getting from settings.ini
        /// </summary>
        public static string Constant(this Component Control, string Key)
        {
            return Settings.Instance.Constant(Key);
        }

        /// <summary>
        /// Application messages and errors. Getting from settings.ini
        /// </summary>
        public static string Messages(this Component Control, string Key)
        {
            return Settings.Instance.Messages(Key);
        }
    }
}
