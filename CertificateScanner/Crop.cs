using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using CertificateScanner.Ini;
using System.Windows.Forms;
using CertificateScanner.ExceptionDecor;
using CertificateScanner.Utils;

namespace CertificateScanner
{
    public partial class Crop : Form
    {
        Boolean bHaveMouse;
        Point ptOriginal = new Point();
        Point ptCurrent = new Point();
        Point ptOriginalNormalised = new Point();
        Point ptCurrentNormalised = new Point();
        Point ptLast = new Point();
        Rectangle rectCropArea;
        Rectangle mainRectangle;
        double ratio;

        string filename;
        string iniFileName;
        string iniKey;

        public Crop(string file, string inifilename, string inikey)
        {
            iniFileName = inifilename;
            iniKey = inikey;
            filename = file;
            
            InitializeComponent();
            this.Text += (inikey == "Regionphoto") ? " " + this.Messages("cropTitlePhoto") : ((inikey == "Regionbar") ? " " + this.Messages("cropTitleBar") : " " + this.Messages("cropTitleSign"));
            bHaveMouse = false;
        }
      
        private void SrcPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Make a note that we "have the mouse".
            bHaveMouse = true;

            // Store the "starting point" for this rubber-band rectangle.
            ptOriginal.X = e.X;
            ptOriginal.Y = e.Y;

            ptOriginalNormalised.X = (int)(e.X / ratio);
            ptOriginalNormalised.Y = (int)(e.Y / ratio);

            // Special value lets us know that no previous
            // rectangle needs to be erased.

            ptLast.X = -1;
            ptLast.Y = -1;
            
            rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
        }

        private void SrcPicBox_MouseUp(object sender, MouseEventArgs e)
        {
            ptCurrentNormalised = new Point((int)(e.X / ratio), (int)(e.Y / ratio));

            // Set internal flag to know we no longer "have the mouse".
            bHaveMouse = false;

            // Set flags to know that there is no "previous" line to reverse.
            ptLast.X = -1;
            ptLast.Y = -1;
            ptOriginal.X = -1;
            ptOriginal.Y = -1;

            if (iniKey != "MainRegion")
            {
                SaveData();
            }
            
        }

        private void InitRectCropArea(Point e, Point ptOriginalinit)
        {
            IniInterface oIni = new IniInterface(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iniFileName));
            var phWidht = Convert.ToInt32(oIni.ReadValue("Save", "photowidth"));
            var phHeight = Convert.ToInt32(oIni.ReadValue("Save", "photoheight"));
            var sgnWidht = Convert.ToInt32(oIni.ReadValue("Save", "signwidth"));
            var sgnHeight = Convert.ToInt32(oIni.ReadValue("Save", "signheight"));

            double phCoef = (double)phWidht / phHeight;
            double sgnCoef = (double)sgnWidht / sgnHeight;
            double coef = 1;
            switch (iniKey.ToLower())
            {
                case "regionphoto": coef = phCoef; break;
                case "regionsign": coef = sgnCoef; break;
                default: coef = 1; break;
            }

            int w;
            int h1;
            int h2;
            // Draw new lines.
            // e.X - rectCropArea.X;
            // normal
            if (e.X > ptOriginalinit.X && e.Y > ptOriginalinit.Y)
            {
                rectCropArea.X = ptOriginalinit.X;
                rectCropArea.Y = ptOriginalinit.Y;
                w = e.X - ptOriginalinit.X;
                h1 = e.Y - ptOriginalinit.Y;
            }
            else if (e.X < ptOriginalinit.X && e.Y > ptOriginalinit.Y)
            {
                rectCropArea.X = e.X;
                rectCropArea.Y = ptOriginalinit.Y;
                w = ptOriginalinit.X - e.X;
                h1 = e.Y - ptOriginalinit.Y;
            }
            else if (e.X > ptOriginalinit.X && e.Y < ptOriginalinit.Y)
            {
                rectCropArea.X = ptOriginalinit.X;
                rectCropArea.Y = e.Y;
                w = e.X - ptOriginalinit.X;
                h1 = ptOriginalinit.Y - e.Y;
            }
            else
            {
                rectCropArea.X = e.X;
                rectCropArea.Y = e.Y;
                w = ptOriginalinit.X - e.X;
                h1 = ptOriginalinit.Y - e.Y;
            }
            if (coef == 1)
            {
                rectCropArea.Width = w;
                rectCropArea.Height = h1;
            }
            else
            {
                h2 = (int)(w / coef);
                if (h1 < h2)
                {
                    rectCropArea.Width = w;
                    rectCropArea.Height = (int)(w / coef);
                }
                else
                {
                    rectCropArea.Height = h1;
                    rectCropArea.Width = (int)(h1 * coef);
                }
            }
        }

        private void SrcPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            //Point ptCurrentNormalised = new Point((int)(e.X / ratio), (int)(e.Y / ratio));

            // If we "have the mouse", then we draw our lines.
            if (bHaveMouse)
            {
                // Update last point.
                ptLast = ptCurrent;
                InitRectCropArea(new Point(e.X, e.Y), ptOriginal);
                
                SrcPicBox.Refresh();
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            int CropWidth = 320;
            int CropHeight = 320;
            int CropX = 0;
            int CropY = 0;

            mainRectangle = new Rectangle(0, 0, int.MaxValue, int.MaxValue);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);

                mainRectangle = RectAndINI.ReadRectFromIni(oIni, "MainRegion", true);

                var CropRect = RectAndINI.ReadRectFromIni(oIni, iniKey, false);
                //CropX = CropRect.X - mainRectangle.X;
                //CropY = CropRect.Y - mainRectangle.Y;
                CropX = CropRect.X;
                CropY = CropRect.Y;
                CropWidth = CropRect.Width;
                CropHeight = CropRect.Height;
            }

            LoadImage(mainRectangle, true);

            if (iniKey != "")
            {
                radioButtonPhoto.Visible = radioButtonSignature.Visible = radioButtonBar.Visible = true;
                radioButtonPhoto.Checked = (iniKey == "Regionphoto") ? true : false; //Regionbar
                radioButtonSignature.Checked = (iniKey == "Regionsign") ? true : false;
                radioButtonBar.Checked = (iniKey == "Regionbar") ? true : false;
            }
            else
                iniKey = "Regionphoto";

           // InitRectCropArea(
           //  new Point(
           //    (radioButtonMainRect.Checked) ?
           //  (int)((CropWidth + CropX) * ratio) :
           //                    (int)((CropWidth + CropX - mainRectangle.X) * ratio),
           //(radioButtonMainRect.Checked) ?
           //(int)((CropHeight + CropY) * ratio) :
           //(int)((CropHeight + CropY - mainRectangle.Y) * ratio)),
           //new Point(
           //  (radioButtonMainRect.Checked) ?
           //                    (int)(CropX * ratio) :
           //                  (int)((CropX - mainRectangle.X) * ratio),
           //                (radioButtonMainRect.Checked) ?
           //              (int)(CropY * ratio) :
           //            (int)((CropY - mainRectangle.Y) * ratio)));
            InitRectCropArea(
               new Point((radioButtonMainRect.Checked) ? (int)((CropWidth + CropX) * ratio) : (int)((CropWidth + CropX) * ratio) - (int)(mainRectangle.X * ratio),
                   (radioButtonMainRect.Checked) ? (int)((CropHeight + CropY) * ratio) : (int)((CropHeight + CropY) * ratio) - (int)(mainRectangle.Y * ratio)),
               new Point((radioButtonMainRect.Checked) ? (int)(CropX * ratio) : (int)(CropX * ratio) - (int)(mainRectangle.X * ratio),
                   (radioButtonMainRect.Checked) ? (int)(CropY * ratio) : (int)(CropY * ratio) - (int)(mainRectangle.Y * ratio)));

            SrcPicBox.Refresh();
        }

        private void LoadImage(Rectangle mainRectangle, bool useRectangle)
        {
            using (var fs = new FileStream(filename, FileMode.Open)) //File not block
            {
                var bmp = new Bitmap(fs);
                var srcImage = (Bitmap)bmp.Clone();
                if (useRectangle)
                {
                    if (mainRectangle.Height > bmp.Height)
                        mainRectangle.Height = bmp.Height;
                    if (mainRectangle.Width > bmp.Width)
                        mainRectangle.Width = bmp.Width;
                    if (mainRectangle.X < 0)
                        mainRectangle.X = 0;
                    if (mainRectangle.Y < 0)
                        mainRectangle.Y = 0;
                    srcImage = (Bitmap)bmp.Clone(mainRectangle, PixelFormat.Format24bppRgb);
                }
                var scaledImage = ImageComputation.ImageConvertions.ScaleImage(srcImage, 490, 520, out ratio);
                SrcPicBox.Image = scaledImage;
            }
        }

        private void SrcPicBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(110, Color.DarkGray)), rectCropArea);
            e.Graphics.DrawRectangle(Pens.Red, rectCropArea);
        }

        private void BtnCrop_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void radioButtonClick(object sender, EventArgs e)
        {
            if (radioButtonPhoto.Checked)
                iniKey = "Regionphoto";
            else
                iniKey = "Regionphoto"; //by default
            if (radioButtonSignature.Checked)
                iniKey = "Regionsign";
            if (radioButtonBar.Checked)
                iniKey = "Regionbar";
            if (radioButtonMainRect.Checked)
                iniKey = "MainRegion";

            if (radioButtonMainRect.Checked)
            {
                LoadImage(mainRectangle, false);
            }
            else
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
                {
                    IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);

                    mainRectangle = RectAndINI.ReadRectFromIni(oIni, "MainRegion", true);
                }
                LoadImage(mainRectangle, true);
            }

            int CropWidth = 320;
            int CropHeight = 320;
            int CropX = 0;
            int CropY = 0;

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);

                var CropRect = RectAndINI.ReadRectFromIni(oIni, iniKey, false);
                CropX = CropRect.X;
                CropY = CropRect.Y;
                CropWidth = CropRect.Width;
                CropHeight = CropRect.Height;
            }

            InitRectCropArea(
                new Point((radioButtonMainRect.Checked) ? (int)((CropWidth + CropX) * ratio) : (int)((CropWidth + CropX) * ratio) - (int)(mainRectangle.X * ratio),
                    (radioButtonMainRect.Checked) ? (int)((CropHeight + CropY) * ratio) : (int)((CropHeight + CropY) * ratio) - (int)(mainRectangle.Y * ratio)),
                new Point((radioButtonMainRect.Checked) ? (int)(CropX * ratio) : (int)(CropX * ratio) - (int)(mainRectangle.X * ratio),
                    (radioButtonMainRect.Checked) ? (int)(CropY * ratio) : (int)(CropY * ratio) - (int)(mainRectangle.Y * ratio)));

            SrcPicBox.Refresh();
        }

        void SaveData()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                //Connect to Ini File "Config.ini" in current directory
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);

                Rectangle area = mainRectangle;
                if (iniKey == "MainRegion")
                    area = new Rectangle(0, 0, SrcPicBox.Image.Width, SrcPicBox.Image.Height);
                Rectangle res = RectAndINI.WriteRectToIni(oIni, iniKey, rectCropArea, ratio, SrcPicBox.Image.Width, SrcPicBox.Image.Height, area);

                this.Info(
                    String.Format("Rect: X=\"{0}\", Y=\"{1}\", Width=\"{2}\", Height=\"{3}\".",
                                  res.X, res.Y, res.Width, res.Height),
                    this.Messages("rectSaved"));
            }
            else
                this.Warn(new FileNotFoundException(
                                String.Format("Can`t save rect to ini file. RectCropArea: X=\"{0}\", Y=\"{1}\", Width=\"{2}\", Height=\"{3}\". Ratio={4}. SrcPicBox.Image.Width={5}. SrcPicBox.Image.Height={6}",
                                    rectCropArea.X, rectCropArea.Y, rectCropArea.Width, rectCropArea.Height, ratio, SrcPicBox.Image.Width, SrcPicBox.Image.Height), AppDomain.CurrentDomain.BaseDirectory + iniFileName),
                            this.Messages("iniWriteError"));
        }
    }
}
