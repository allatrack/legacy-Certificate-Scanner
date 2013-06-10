using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CertificateScanner.Components
{
    enum RectangleStatus { Creating, Resizing, Moving };
    public delegate void SelectedAreaChangedEventHandler(object sender, EventArgs e);
    public delegate void ImageChangedHandler(object sender, EventArgs e);
    class SA_PictureBox : System.Windows.Forms.PictureBox
    {
        // current line
        //  1        2         3
        //   .---------------.
        //   |               |
        // 8 |               | 4
        //   |               |
        //   .---------------. 
        //  7        6         5
        int _currentLine;
        int _resizeArea = 5;

        RectangleStatus _rectangleStatus;
        Rectangle _prevRectangle, rectCropArea;
        Point _mouseClickLocation;
        Boolean isMousePressed;
        double _coef, _ratio;
        int _imageX, _imageY, _imageW, _imageH, _pbxW, _pbxH;
        int oldX, oldY, oldPictureBoxWidth, oldPictureBoxHeight;
        Bitmap _image;
        public double Coef { get { return _coef; } set { _coef = value; } }
        public Bitmap SelectedImage { get; set; }
        public Rectangle SelectedRectangle { get; set; }
        //public Point ImageLoc
        //{
        //    get
        //    { 
        //        if(this.SizeMode==PictureBoxSizeMode.
        //    }
        //}

        
        public SA_PictureBox() :
            base()
        {
            isMousePressed = false;
            _coef = 1;
            this.MouseDown += new MouseEventHandler(PBX_MouseDown);
            this.MouseMove += new MouseEventHandler(PBX_MouseMove);
            this.MouseUp += new MouseEventHandler(PBX_MouseUp);
            this.Paint += new PaintEventHandler(PBX_Paint);
            this.Resize += SA_PictureBox_Resize;
        
        }
        public Image Image
        {
            get { return base.Image; }
            set { base.Image = value; CalculateImagePosition(); OnImageChanged(EventArgs.Empty); }
        }

        private void SA_PictureBox_Resize(object sender, EventArgs e)
        {
            if (_image != null)
            {
                var oldX = _imageX;
                var oldY = _imageY;
                var oldW = _pbxW - _imageX * 2;
                var oldH = _pbxH - _imageY * 2;
                
                CalculateImagePosition();

                var W = _pbxW - _imageX * 2;
                var H = _pbxH - _imageY * 2;

                var cfX = (double)W / oldW;
                var cfY = (double)H / oldH;


                var recX = (int)(_imageX + (rectCropArea.X - oldX) * cfX);
                var recY = (int)((rectCropArea.Y - oldY) * cfY + _imageY);

                var recW = (int)(cfX*rectCropArea.Width);
                var recH = (int)(cfY*rectCropArea.Height);

                rectCropArea = new Rectangle(recX, recY, recW, recH);
            }
        }


        private void PBX_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePressed = true;
            if (rectCropArea != null)
            {
                _prevRectangle = rectCropArea;
                _mouseClickLocation = e.Location;
                if (_rectangleStatus != RectangleStatus.Moving && _rectangleStatus != RectangleStatus.Resizing)
                {
                    _rectangleStatus = RectangleStatus.Creating;
                    rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
                }
            }
        }
        private void PBX_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePressed = false;
            if (this.Image != null)
            {
                Rectangle realRectCropArea = new Rectangle((int)((rectCropArea.X - _imageX) / _ratio),
                                                           (int)((rectCropArea.Y - _imageY) / _ratio),
                                                           (int)(rectCropArea.Width / _ratio),
                                                           (int)(rectCropArea.Height / _ratio));
                if (realRectCropArea.X + realRectCropArea.Width > _image.Width)
                    realRectCropArea.Width = _image.Width - realRectCropArea.X;
                if (realRectCropArea.Y + realRectCropArea.Height > _image.Height)
                    realRectCropArea.Height = _image.Height - realRectCropArea.Y;
                realRectCropArea = NormaliseRect(realRectCropArea.Width, realRectCropArea.Height, realRectCropArea, false);
                SelectedRectangle = realRectCropArea;
                if (_image != null)
                {
                    var sourceimg = (Bitmap)_image.Clone();
                    SelectedImage = new Bitmap(sourceimg).Clone(realRectCropArea, PixelFormat.Format24bppRgb);
                }
                OnSelectedAreaChanged(EventArgs.Empty);

                oldPictureBoxWidth = this.Width;
                oldPictureBoxHeight = this.Height;
            }
        }
        private void PBX_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMousePressed)
            {
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
                    InitRectCropArea(new Point(e.X, e.Y));
                }
                this.Refresh();
            }
            else
            {
                SetPosition(e.Location);
            }
        }
        private void PBX_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(110, Color.DarkGray)), rectCropArea);
            e.Graphics.DrawRectangle(Pens.Red, rectCropArea);
        }

        private Rectangle NormaliseRect(int outputWidth, int outputHeight, Rectangle inputRect, bool Maximize)
        {
            Rectangle result = inputRect;
            int h2;
            if (_coef == 1)
            {
                result.Width = outputWidth;
                result.Height = outputHeight;
            }
            else
            {
                h2 = (int)(outputWidth / _coef);
                if (outputHeight < h2)
                {
                    if (Maximize)
                    {
                        result.Width = outputWidth;
                        result.Height = (int)(outputWidth / _coef);
                    }
                    else
                    {
                        result.Height = outputHeight;
                        result.Width = (int)(outputHeight * _coef);
                    }
                }
                else
                {
                    if (Maximize)
                    {
                        result.Height = outputHeight;
                        result.Width = (int)(outputHeight * _coef);
                    }
                    else
                    {
                        result.Width = outputWidth;
                        result.Height = (int)(outputWidth / _coef);
                    }
                }
            }
            return result;
        }
        private void InitRectCropArea(Point e)
        {
            int w;
            int h1;
            Point ptOriginalinit = new Point(_mouseClickLocation.X, _mouseClickLocation.Y);
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
        private void MoveRect(Point mpoint)
        {
            var x = _mouseClickLocation.X - mpoint.X;
            var y = _mouseClickLocation.Y - mpoint.Y;
            var rX = _prevRectangle.Location.X - x;
            var rY = _prevRectangle.Location.Y - y;
            var w = rectCropArea.Width + rX;
            var h = rectCropArea.Height + rY;
            if (w > this.Width || h > this.Height || rX < 0 || rY < 0)
            {
                return;
            }
            else
            {
                rectCropArea.Location = new Point(rX, rY);
            }
        }
        private void ResizeRect(Point mpoint)
        {
            int w;
            int h;
            switch (_currentLine)
            {
                case 1:
                    w = Math.Abs(_prevRectangle.Width + _mouseClickLocation.X - mpoint.X);
                    h = Math.Abs(_prevRectangle.Height + _mouseClickLocation.Y - mpoint.Y);
                    rectCropArea.X = mpoint.X;
                    rectCropArea.Y = mpoint.Y;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 2:
                    h = _prevRectangle.Height + _mouseClickLocation.Y - mpoint.Y;
                    rectCropArea.Y = mpoint.Y;
                    rectCropArea = NormaliseRect(_prevRectangle.Width, h, rectCropArea, true);
                    break;
                case 3:
                    w = _prevRectangle.Width + mpoint.X - _mouseClickLocation.X;
                    h = _prevRectangle.Height + _mouseClickLocation.Y - mpoint.Y;
                    rectCropArea.Y = mpoint.Y;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 4:
                    w = _prevRectangle.Width - _mouseClickLocation.X + mpoint.X;
                    rectCropArea = NormaliseRect(w, _prevRectangle.Height, rectCropArea, true);
                    break;
                case 5:
                    w = _prevRectangle.Width - _mouseClickLocation.X + mpoint.X;
                    h = _prevRectangle.Height - _mouseClickLocation.Y + mpoint.Y;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 6:
                    h = _prevRectangle.Height - _mouseClickLocation.Y + mpoint.Y;
                    rectCropArea = NormaliseRect(_prevRectangle.Width, h, rectCropArea, true);
                    break;
                case 7:
                    w = _prevRectangle.Width + _mouseClickLocation.X - mpoint.X;
                    h = _prevRectangle.Height - _mouseClickLocation.Y + mpoint.Y;
                    rectCropArea.X = mpoint.X;
                    rectCropArea = NormaliseRect(w, h, rectCropArea, true);
                    break;
                case 8:
                    w = _prevRectangle.Width + _mouseClickLocation.X - mpoint.X;
                    rectCropArea.X = mpoint.X;
                    rectCropArea = NormaliseRect(w, _prevRectangle.Height, rectCropArea, true);
                    break;
            }
        }
        private void SetPosition(Point e)
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
        private void CalculateImagePosition()
        {
            _image = (Bitmap)this.Image;
            if (_image != null)
            {
                var ratioX = (double)this.Width / _image.Width;
                var ratioY = (double)this.Height / _image.Height;
                _ratio = Math.Min(ratioX, ratioY);

                var scaledWidth = _image.Width * _ratio;
                var scaledHeight = _image.Height * _ratio;
                _imageX = (int)((this.Width - scaledWidth) / 2);
                _imageY = (int)((this.Height - scaledHeight) / 2);
                _imageW = _image.Width;
                _imageH = _image.Height;
                _pbxW = this.Width;
                _pbxH = this.Height;
            }
        }


        public event SelectedAreaChangedEventHandler SelectedAreaChanged;
        public event ImageChangedHandler ImageChanged;

        protected virtual void OnSelectedAreaChanged(EventArgs e)
        {
            if (SelectedAreaChanged != null)
                SelectedAreaChanged(this, e);
        }
        protected virtual void OnImageChanged(EventArgs e)
        {
            if (ImageChanged != null)
                ImageChanged(this, e);
        }

        public void InitCropArea(Point e, Point ptOriginalinit)
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
            rectCropArea.Width = w;
            rectCropArea.Height = h1;
        }
    }
}
