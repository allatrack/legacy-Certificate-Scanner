using System;
using System.Collections.Generic;
using System.Collections;

namespace CertificateScanner.ExceptionDecor
{
    public class ExceptionWrapper : Exception
    {
        public enum ErrorTypes { Fatal, Error, Warning, Info, Debug, Trace }
        public Exception ex;
        public bool withMessage;
        public string message;
        public string comment;
        public ICollection<DictionaryEntry> data;
        public ErrorTypes type;
    }
}
