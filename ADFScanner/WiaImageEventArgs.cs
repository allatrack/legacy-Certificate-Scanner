using System;
using System.Collections.Generic;
using System.Drawing;

namespace CertificateScanner.ADFScanner
{
    public class WiaImageEventArgs : EventArgs
    {
        public WiaImageEventArgs(Image img)
        {
            ScannedImage = img;
        }
        public Image ScannedImage { get; private set; }
    }
}
