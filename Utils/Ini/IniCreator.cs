using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CertificateScanner.Utils
{
    public class IniCreator
    {
        private string _fileName;

        public IniCreator(string fileName)
        {
            _fileName = fileName;
        }

        public bool CreateIni(string content)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(_fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(_fileName))
                    sw.WriteLine(content);
                return true;
            }
            catch(Exception ex)
            {
                ExceptionDecor.ExceptionDecorator.Fatal(new Exception(String.Format(Settings.Instance.Constant("iniCreateError") + ". {1}", _fileName, ex.Message), ex), 
                    String.Format(Settings.Instance.Constant("iniCreateError"), _fileName));
                return false;
            }
        }
    }
}
