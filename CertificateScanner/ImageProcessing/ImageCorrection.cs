using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using CertificateScanner.Ini;
using System.Windows.Forms;
using CertificateScanner.ExceptionDecor;
using CertificateScanner.Utils;

namespace CertificateScanner.ImageProcessing
{
    enum RectangleStatus { Creating, Resizing, Moving };

    public partial class ImageCorrection : Form
    {
        //
        int _resizeArea = 5;
        RectangleStatus _rectangleStatus;
        Point _rectangleLocation;
        Point _mouseClick;
        Rectangle _prevRectangle;
        // current line
        //  1        2         3
        //   .---------------.
        //   |               |
        // 8 |               | 4
        //   |               |
        //   .---------------. 
        //  7        6         5
        int _currentLine;
        //
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

        int defaultlefttreshold = 0;
        int defaultrighttreshold = 255;

        bool correct;

        public bool Corrected
        {
            get
            {
                return correct;
            }

            set { }
        }

        public ImageCorrection(Image image, string inifilename, string inikey)
        {
            iniFileName = inifilename;
            _rectangleStatus = RectangleStatus.Creating;
            iniKey = inikey;
            _image = image;
            InitializeComponent();
            bHaveMouse = false;
        }

        private void SrcPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Make a note that we "have the mouse".
            bHaveMouse = true;

            if (rectCropArea != null)
            {
                if (_rectangleStatus == RectangleStatus.Moving)
                {
                    _rectangleLocation = rectCropArea.Location;
                    _mouseClick = new Point(e.X, e.Y);
                }
                else if (_rectangleStatus == RectangleStatus.Resizing)
                {
                    _mouseClick = new Point(e.X, e.Y);
                    _prevRectangle = new Rectangle(rectCropArea.X, rectCropArea.Y, rectCropArea.Width, rectCropArea.Height);
                }
                else
                {
                    _rectangleStatus = RectangleStatus.Creating;
                    // Store the "starting point" for this rubber-band rectangle.
                    ptOriginal.X = e.X;
                    ptOriginal.Y = e.Y;
                    // Special value lets us know that no previous
                    // rectangle needs to be erased.
                    ptLast.X = -1;
                    ptLast.Y = -1;
                    rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
                }
            }
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
            pictureBoxOut.Image = ImageComputation.ImageConvertions.ApplyRangeLevels(defaultlefttreshold, defaultrighttreshold, _imageOut);

            histogram.DrawHistogram(GetHistogram((Bitmap)_imageOut));
            rangeLevels.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(defaultlefttreshold, defaultrighttreshold);

            correct = true;
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
                if (_rectangleStatus == RectangleStatus.Moving)
                {
                    MoveRect(e.Location);
                }
                else if (_rectangleStatus == RectangleStatus.Resizing)
                {
                    ResizeRect(e.Location);
                }
                else
                {
                    ptLast = ptCurrent;
                    InitRectCropArea(new Point(e.X, e.Y), ptOriginal);
                }
                SrcPicBox.Refresh();
            }
            else
            {
                if (rectCropArea != null)
                {
                    if
                      (
                       (e.X >= (rectCropArea.X + _resizeArea)) &&
                       (e.X <= (rectCropArea.X + rectCropArea.Width - _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y + _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + rectCropArea.Height - _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Moving;
                        this.Cursor = Cursors.Hand;
                        _currentLine = 0;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X + _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y - _resizeArea)) &&
                       (e.X <= (rectCropArea.X + rectCropArea.Width - _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeNS;
                        _currentLine = 2;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X - _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y + _resizeArea)) &&
                       (e.X <= (rectCropArea.X + _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + rectCropArea.Height - _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeWE;
                        _currentLine = 8;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X + _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y + rectCropArea.Height - _resizeArea)) &&
                       (e.X <= (rectCropArea.X + rectCropArea.Width - _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + rectCropArea.Height + _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeNS;
                        _currentLine = 6;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X + rectCropArea.Width - _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y + _resizeArea)) &&
                       (e.X <= (rectCropArea.X + rectCropArea.Width + _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + rectCropArea.Height - _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeWE;
                        _currentLine = 4;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X - _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y - _resizeArea)) &&
                       (e.X <= (rectCropArea.X + _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeNWSE;
                        _currentLine = 1;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X + rectCropArea.Width - _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y - _resizeArea)) &&
                       (e.X <= (rectCropArea.X + rectCropArea.Width + _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeNESW;
                        _currentLine = 3;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X + rectCropArea.Width - _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y + rectCropArea.Height - _resizeArea)) &&
                       (e.X <= (rectCropArea.X + rectCropArea.Width + _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + rectCropArea.Height + _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeNWSE;
                        _currentLine = 5;
                    }
                    else if
                      (
                       (e.X >= (rectCropArea.X - _resizeArea)) &&
                       (e.Y >= (rectCropArea.Y + rectCropArea.Height - _resizeArea)) &&
                       (e.X <= (rectCropArea.X + _resizeArea)) &&
                       (e.Y <= (rectCropArea.Y + rectCropArea.Height + _resizeArea))
                      )
                    {
                        _rectangleStatus = RectangleStatus.Resizing;
                        this.Cursor = Cursors.SizeNESW;
                        _currentLine = 7;
                    }
                    else
                    {
                        _rectangleStatus = RectangleStatus.Creating;
                        this.Cursor = Cursors.Default;
                        _currentLine = 0;
                    }
                }
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            _imageOut = SrcPicBox.Image = _image;

            var ratioX = (double)SrcPicBox.Width / _image.Width;
            var ratioY = (double)SrcPicBox.Height / _image.Height;
            ratio = Math.Min(ratioX, ratioY);

            // Compute the offset of the image to center it in the picture box
            var scaledWidth = _image.Width * ratio;
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
            var initresholdleftkey = "signdefaultlefttreshold";
            var initresholdrightkey = "signdefaultrighttreshold";
            switch (iniKey.ToLower())
            {
                case "photo": 
                    {
                        coef = phCoef; 
                        initresholdleftkey = "photodefaultlefttreshold";
                        initresholdrightkey = "photodefaultrighttreshold";
                    }
                    break;
                case "sign": 
                    {
                        coef = sgnCoef; 
                        initresholdleftkey = "signdefaultlefttreshold";
                        initresholdrightkey = "signdefaultrighttreshold";
                    }
                    break;
                default: 
                    {
                        coef = 1; 
                        initresholdleftkey = "signdefaultlefttreshold";
                        initresholdrightkey = "signdefaultrighttreshold";
                    }
                    break;
            }

            if (!int.TryParse(oIni.ReadValue("Save", initresholdleftkey, "0"), out defaultlefttreshold)) defaultlefttreshold = 125;
            if (!int.TryParse(oIni.ReadValue("Save", initresholdrightkey, "255"), out defaultrighttreshold)) defaultrighttreshold = 255;
            
            pictureBoxOut.Image = ImageComputation.ImageConvertions.ApplyRangeLevels(defaultlefttreshold, defaultrighttreshold, _imageOut);
            histogram.DrawHistogram(GetHistogram((Bitmap)_imageOut));
            rangeLevels.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(defaultlefttreshold, defaultrighttreshold);
        }

        private void SrcPicBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(110, Color.DarkGray)), rectCropArea);
            e.Graphics.DrawRectangle(Pens.Red, rectCropArea);
        }

        private void rangeLevels_EditValueChanged(object sender, EventArgs e)
        {
            pictureBoxOut.Image = ImageComputation.ImageConvertions.ApplyRangeLevels(rangeLevels.Value.Minimum, rangeLevels.Value.Maximum, _imageOut);
            
            //correct = true;
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
                this.Info(iniKey + " image successfully corrected");
                this.Close();
            }
            catch (Exception ex)
            {
                this.Error(
                    new Exception(String.Format(this.Messages("saveCorrectImageError") + ".Path: {0}", Path.Combine(Path.GetTempPath(), "tmpcorrection.jpg")), ex),
                    this.Messages("saveCorrectImageError"));
            }
        }

        private void MoveRect(Point mpoint)
        {
            var x = _mouseClick.X - mpoint.X;
            var y = _mouseClick.Y - mpoint.Y;
            var rX = _rectangleLocation.X - x;
            var rY = _rectangleLocation.Y - y;
            var w = rectCropArea.Width + rX;
            var h = rectCropArea.Height + rY;
            if (w > SrcPicBox.Width || h > SrcPicBox.Height || rX < 0 || rY < 0)
            {
                return;
            }
            else
            {
                rectCropArea.Location = new Point(rX, rY);
            }
            //if(end.X

        }
        private void ResizeRect(Point mpoint)
        {
            int w;
            int h;
            switch (_currentLine)
            {
                case 1:
                    w = Math.Abs(_prevRectangle.Width + _mouseClick.X - mpoint.X);
                    h = Math.Abs(_prevRectangle.Height + _mouseClick.Y - mpoint.Y);
                    rectCropArea.X = mpoint.X;
                    rectCropArea.Y = mpoint.Y;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 2:
                    h = _prevRectangle.Height + _mouseClick.Y - mpoint.Y;
                    rectCropArea.Y = mpoint.Y;
                    rectCropArea = NormaliseRect(0, h, rectCropArea, true);
                    break;
                case 3:
                    w = _prevRectangle.Width + mpoint.X - _mouseClick.X;
                    h = _prevRectangle.Height + _mouseClick.Y - mpoint.Y;
                    rectCropArea.Y = mpoint.Y;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 4:
                    w = _prevRectangle.Width - _mouseClick.X + mpoint.X;
                    rectCropArea = NormaliseRect(w, 0, rectCropArea, true);
                    break;
                case 5:
                    w = _prevRectangle.Width - _mouseClick.X + mpoint.X;
                    h = _prevRectangle.Height - _mouseClick.Y + mpoint.Y;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 6:
                    h = _prevRectangle.Height - _mouseClick.Y + mpoint.Y;
                    rectCropArea = NormaliseRect(0, h, rectCropArea, true);
                    break;
                case 7:
                    w = _prevRectangle.Width + _mouseClick.X - mpoint.X;
                    h = _prevRectangle.Height - _mouseClick.Y + mpoint.Y;
                    rectCropArea.X = mpoint.X;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 8:
                    w = _prevRectangle.Width + _mouseClick.X - mpoint.X;
                    rectCropArea.X = mpoint.X;
                    rectCropArea = NormaliseRect(w, 0, rectCropArea, true);
                    break;
            }
        }
    }
}