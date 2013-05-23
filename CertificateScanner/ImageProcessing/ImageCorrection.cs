using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using CertificateScanner.Ini;
using System.Windows.Forms;
using CertificateScanner.ExceptionDecor;
using CertificateScanner.Utils;
using AForge.Imaging.Filters;

namespace CertificateScanner.ImageProcessing
{
    public partial class ImageCorrection : Form
    {
        Boolean bHaveMouse;
        Point ptOriginal = new Point();
        Point ptCurrent = new Point();
        Point ptLast = new Point();
        Rectangle rectCropArea;
        double ratio;
        double coef = 1;
        int imageX;
        int imageY;

        string iniFileName;
        string iniKey;
        Image _image;
        Image _imageOut;

        public ImageCorrection(Image image, string inifilename, string inikey)
        {
            iniFileName = inifilename;
            iniKey = inikey;

            _image = image;
            
            InitializeComponent();
            bHaveMouse = false;
        }
      
        private void SrcPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Make a note that we "have the mouse".
            bHaveMouse = true;

            // Store the "starting point" for this rubber-band rectangle.
            ptOriginal.X = e.X;
            ptOriginal.Y = e.Y;

            // Special value lets us know that no previous
            // rectangle needs to be erased.

            ptLast.X = -1;
            ptLast.Y = -1;
            
            rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
        }

        private void SrcPicBox_MouseUp(object sender, MouseEventArgs e)
        {
            // Set internal flag to know we no longer "have the mouse".
            bHaveMouse = false;

            // Set flags to know that there is no "previous" line to reverse.
            ptLast.X = -1;
            ptLast.Y = -1;
            ptOriginal.X = -1;
            ptOriginal.Y = -1;

            Rectangle realRectCropArea = new Rectangle((int)((rectCropArea.X - imageX) / ratio), 
                                                       (int)((rectCropArea.Y - imageY) / ratio), 
                                                       (int)(rectCropArea.Width / ratio), 
                                                       (int)(rectCropArea.Height / ratio));

            if (realRectCropArea.X + realRectCropArea.Width > _image.Width)
                realRectCropArea.Width = _image.Width - realRectCropArea.X;

            if (realRectCropArea.Y + realRectCropArea.Height > _image.Height)
                realRectCropArea.Height = _image.Height - realRectCropArea.Y;

            realRectCropArea = NormaliseRect(realRectCropArea.Width, realRectCropArea.Height, realRectCropArea, false);

            _imageOut = ((Bitmap)_image).Clone(realRectCropArea, PixelFormat.Format24bppRgb);
            pictureBoxOut.Image = _imageOut;

            histogram.DrawHistogram(GetHistogram((Bitmap)pictureBoxOut.Image));
            rangeLevels.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 255);
        }

        private static long[] GetHistogram(Bitmap picture)
        {
            long[] myHistogram = new long[256];

            for (int i = 0; i < picture.Size.Width; i++)
                for (int j = 0; j < picture.Size.Height; j++)
                {
                    Color c = picture.GetPixel(i, j);

                    long Temp = 0;
                    Temp += c.R;
                    Temp += c.G;
                    Temp += c.B;

                    Temp = (int)Temp / 3;
                    myHistogram[Temp]++;
                }

            return myHistogram;
        }

        private void InitRectCropArea(Point e, Point ptOriginalinit)
        {
            int w;
            int h1;
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

            rectCropArea = NormaliseRect(w, h1, rectCropArea, true);
        }

        private Rectangle NormaliseRect(int outputWidth, int outputHeight, Rectangle inputRect, bool Maximize)
        {
            Rectangle result = inputRect;
            int h2;
            if (coef == 1)
            {
                result.Width = outputWidth;
                result.Height = outputHeight;
            }
            else
            {
                h2 = (int)(outputWidth / coef);
                if (outputHeight < h2)
                {
                    if (Maximize)
                    {
                        result.Width = outputWidth;
                        result.Height = (int)(outputWidth / coef);
                    }
                    else
                    {
                        result.Height = outputHeight;
                        result.Width = (int)(outputHeight * coef);
                    }
                }
                else
                {
                    if (Maximize)
                    {
                        result.Height = outputHeight;
                        result.Width = (int)(outputHeight * coef);
                    }
                    else
                    {
                        result.Width = outputWidth;
                        result.Height = (int)(outputWidth / coef);
                    }
                }
            }
            return result;
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
            _imageOut = SrcPicBox.Image = pictureBoxOut.Image = _image;

            histogram.DrawHistogram(GetHistogram((Bitmap)pictureBoxOut.Image));

            var ratioX = (double)SrcPicBox.Width / _image.Width;
            var ratioY = (double)SrcPicBox.Height / _image.Height;
            ratio = Math.Min(ratioX, ratioY);

            // Compute the offset of the image to center it in the picture box
            var scaledWidth  = _image.Width * ratio;
            var scaledHeight = _image.Height * ratio;
            imageX = (int)((SrcPicBox.Width - scaledWidth) / 2);
            imageY = (int)((SrcPicBox.Height - scaledHeight) / 2);

            IniInterface oIni = new IniInterface(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iniFileName));
            var phWidht = Convert.ToInt32(oIni.ReadValue("Save", "photowidth"));
            var phHeight = Convert.ToInt32(oIni.ReadValue("Save", "photoheight"));
            var sgnWidht = Convert.ToInt32(oIni.ReadValue("Save", "signwidth"));
            var sgnHeight = Convert.ToInt32(oIni.ReadValue("Save", "signheight"));

            double phCoef = (double)phWidht / phHeight;
            double sgnCoef = (double)sgnWidht / sgnHeight;
            switch (iniKey.ToLower())
            {
                case "photo": coef = phCoef; break;
                case "sign": coef = sgnCoef; break;
                default: coef = 1; break;
            }
        }

        private void SrcPicBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(110, Color.DarkGray)), rectCropArea);
            e.Graphics.DrawRectangle(Pens.Red, rectCropArea);
        }

        private void rangeLevels_EditValueChanged(object sender, EventArgs e)
        {
            // create filter
            LevelsLinear filter = new LevelsLinear() { /* set ranges*/
                InRed = new AForge.IntRange(rangeLevels.Value.Minimum, rangeLevels.Value.Maximum), 
                InGreen = new AForge.IntRange(rangeLevels.Value.Minimum, rangeLevels.Value.Maximum), 
                InBlue = new AForge.IntRange(rangeLevels.Value.Minimum, rangeLevels.Value.Maximum) };
            // apply the filter
            using (Bitmap filteredImage = (Bitmap)_imageOut.Clone())
            {
                filter.ApplyInPlace(filteredImage);
                pictureBoxOut.Image = (Bitmap)filteredImage.Clone();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Path.Combine(Path.GetTempPath(), "tmpcorrection.jpg")))
                    File.Delete(Path.Combine(Path.GetTempPath(), "tmpcorrection.jpg"));
                pictureBoxOut.Image.Save(Path.Combine(Path.GetTempPath(), "tmpcorrection.jpg"));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                this.Error(
                    new Exception(String.Format(this.Messages("saveCorrectImageError") + ".Path: {0}", Path.Combine(Path.GetTempPath(), "tmpcorrection.jpg")), ex),
                    this.Messages("saveCorrectImageError"));
            }
        }

    }
}
