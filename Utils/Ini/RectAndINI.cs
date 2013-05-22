using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace CertificateScanner.Ini
{
    public static class RectAndINI
    {
        public static Rectangle ReadRectFromIni(IniInterface oIni, string iniPath, bool withTolerance = true)
        {
            int x = 0, y = 0, width = 100, height = 100, tolerance = 0;

            if (withTolerance)
                tolerance = (int.TryParse(oIni.ReadValue(iniPath, "tolerance", "0"), out tolerance)) ? tolerance : 0;

            return new Rectangle((int.TryParse(oIni.ReadValue(iniPath, "x", "0"), out x)) ? x + tolerance : 0 + tolerance,
                (int.TryParse(oIni.ReadValue(iniPath, "y", "0"), out y)) ? y + tolerance : 0 + tolerance,
                (int.TryParse(oIni.ReadValue(iniPath, "width", "0"), out width)) ? width - tolerance : 100 - tolerance,
                (int.TryParse(oIni.ReadValue(iniPath, "height", "0"), out height)) ? height - tolerance : 100 - tolerance);
        }

        public static Rectangle WriteRectToIni(IniInterface oIni, string iniPath, Rectangle rectCropArea, 
            double ratio, int imageWidth, int imageHeight, Rectangle mainRectangle)
        {
            Rectangle rect = new Rectangle(
                (int)((((rectCropArea.X > 0) ? rectCropArea.X : 0) / ratio) + mainRectangle.X),
                (int)((((rectCropArea.Y > 0) ? rectCropArea.Y : 0) / ratio) + mainRectangle.Y),
                (int)(((rectCropArea.Width < mainRectangle.Width - rectCropArea.X) ? rectCropArea.Width : mainRectangle.Width * ratio - rectCropArea.X) / ratio),
                (int)(((rectCropArea.Height < mainRectangle.Height - rectCropArea.Y) ? rectCropArea.Height : mainRectangle.Height * ratio - rectCropArea.Y) / ratio));

            oIni.WriteValue(iniPath, "x", (rect.X).ToString());
            oIni.WriteValue(iniPath, "y", (rect.Y).ToString());
            oIni.WriteValue(iniPath, "width", (rect.Width).ToString());
            oIni.WriteValue(iniPath, "height", (rect.Height).ToString());

            return rect;
        }
    }
}
