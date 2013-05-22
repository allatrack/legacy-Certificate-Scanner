﻿using System.Collections.Generic;
using CertificateScanner.ADFScanner;
using System.Drawing;
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using CertificateScanner.Ini;
using CertificateScanner.Utils;
using FreeImageAPI;
using CertificateScanner.ExceptionDecor;
using AForge.Imaging.Filters;

namespace CertificateScanner
{
    public partial class Main : Form
    {
        ADFScan _scanner;
        private const string iniFileName = "config.ini";
        ScanColor _color;
        int _dpi;
        string _path;
        Rectangle _signRect;
        Rectangle _photoRect;
        Rectangle _barRect;
        int _photoMaxWeight = 18000; //in bytes
        int _signMaxWeight = 8000; //in bytes
        int _compressDPI = 300;
        string _deviceuuid;
        string _photoPrefix;
        string _signPrefix;
        string _photoJP2Prefix;
        string _signJP2Prefix;
        string _photoSufix;
        string _signSufix;
        string _photoJP2Sufix;
        string _signJP2Sufix;

        public Main()
        {
            InitializeComponent();

        }

        private void _scanner_ScanComplete(object sender, EventArgs e)
        {
            if (!checkBoxAuto.Checked)
            {
                using (Crop fCrop = new Crop(Path.GetTempPath() + "tmp.jpg", iniFileName, "Regionphoto"))
                    fCrop.ShowDialog();
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
                {
                    IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);
                    _signRect = RectAndINI.ReadRectFromIni(oIni, "Regionsign");
                    _photoRect = RectAndINI.ReadRectFromIni(oIni, "Regionphoto");
                    _barRect = RectAndINI.ReadRectFromIni(oIni, "Regionbar");
                }

                checkBoxAuto.Checked = true;
            }

            buttonSave.Enabled = buttonPhotoRect.Enabled = buttonSignatureRect.Enabled = buttonBarRect.Enabled = true;

            using (var fs = new FileStream(Path.GetTempPath() + "tmp.jpg", FileMode.Open)) //File not block
            {
                var bmp = new Bitmap(fs);
                var sourceimg = (Bitmap)bmp.Clone();

                //Photo
                pictureBoxPhoto.Image = new Bitmap(sourceimg).Clone(_photoRect, PixelFormat.Format24bppRgb);
                this.Info("Successfuly get Photo");

                //Signature
                pictureBoxSignature.Image = ImageComputation.ImageConvertions.MakeGrayscale3(new Bitmap(sourceimg).Clone(_signRect, PixelFormat.Format24bppRgb));
                this.Info("Successfuly get Signature");

                //QRCode
                string numb = "";
                if (checkBoxBarNumber.Checked)
                {
                    numb = ScanQRCode(_barRect, sourceimg);
                    textBoxNumber.Text = (!String.IsNullOrWhiteSpace(numb)) ? numb : textBoxNumber.Text;
                    this.Info("Successfuly get QRCode");
                }
                if (String.IsNullOrWhiteSpace(numb))
                    textBoxNumber.ReadOnly = checkBoxBarNumber.Checked = false;

            }
        }

        private void _scanner_Scanning(object sender, WiaImageEventArgs e)
        {
            if (File.Exists(Path.GetTempPath() + "tmp.jpg"))
            {
                //file exists, delete it
                File.Delete(Path.GetTempPath() + "tmp.jpg");
            }

            //stream safed save
            MemoryStream mss = new MemoryStream();

            FileStream fs = new FileStream(Path.GetTempPath() + "tmp.jpg", FileMode.Create, FileAccess.ReadWrite);

            ImageCodecInfo jpegCodec = CertificateScanner.ImageComputation.ImageConvertions.GetEncoderInfo(@"image/jpeg");
            Encoder encoder = Encoder.Quality;
            EncoderParameters encoderParams = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(encoder, 100L);
            encoderParams.Param[0] = encoderParameter;
            e.ScannedImage.Save(mss, jpegCodec, encoderParams);//FILES ARE RETURNED AS BITMAPS
            byte[] matriz = mss.ToArray();
            fs.Write(matriz, 0, matriz.Length);

            mss.Close();
            fs.Close();

            this.Info(String.Format("Scaned full paper successfuly into \"{0}tmp.jpg\"", Path.GetTempPath()));
        }        

        private void buttonScan_Click(object sender, EventArgs e)
        {
            try
            {
                _scanner = new ADFScan();
                _scanner.Scanning += _scanner_Scanning;
                textBoxNumber.Text = "";
                _scanner.ScanComplete += _scanner_ScanComplete;
                _scanner.BeginScan(_color, _dpi, _deviceuuid, AppDomain.CurrentDomain.BaseDirectory + iniFileName);
            }
            catch(Exception ex)
            {
                //Log.Log.WriteLog("Main form->buttonScan_click.||||" + ex.Source + "|||" + ex.Message);
                if ((ex is System.Runtime.InteropServices.ExternalException) && (ex.Message == "A generic error occurred in GDI+."))
                    this.Error(ex, this.Messages("resultScanCntSave"));
                else
                    this.Error(ex, this.Messages("generalScanError"));
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            string _oldPath = textBoxPath.Text;
            FolderBrowserDialog fld = new FolderBrowserDialog() {SelectedPath = textBoxPath.Text};
            if (fld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxPath.Text = _path = fld.SelectedPath;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
                {
                    //Connect to Ini File "Config.ini" in current directory
                    IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);
                    oIni.WriteValue("Save", "path", textBoxPath.Text);
                    this.Info(String.Format("Change path to save files from \"{0}\", to \"{1}\"", _oldPath, _path));
                }
            }
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);
                string val = oIni.ReadValue("Scan", "color", "Color");
                checkBoxAuto.Checked = (oIni.ReadValue("Scan", "auto", "true") == "true") ? true : false;
                checkBoxBarNumber.Checked = (oIni.ReadValue("Scan", "barNumber", "true") == "true") ? true : false;

                textBoxNumber.ReadOnly =
                    checkBoxBarNumber.Checked;

                _color = val == "Color" ? ScanColor.Color : (
                         val == "BlackWhite" ? ScanColor.BlackWhite :
                         ScanColor.Gray);
                if (!int.TryParse(oIni.ReadValue("Scan", "dpi", "300"), out _dpi))  _dpi = 300;
                
                _path = oIni.ReadValue("Save", "path", AppDomain.CurrentDomain.BaseDirectory);
                textBoxPath.Text = _path;
                _photoPrefix = oIni.ReadValue("Save", "photoprefix", "Photo#");
                _signPrefix = oIni.ReadValue("Save", "signprefix", "Sign#");
                _photoJP2Prefix = oIni.ReadValue("Save", "photojp2prefix", "Photo2#");
                _signJP2Prefix = oIni.ReadValue("Save", "signjp2prefix", "Sign2#");
                _photoSufix = oIni.ReadValue("Save", "photosufix", "<F>");
                _signSufix = oIni.ReadValue("Save", "signsufix", "<P>");
                _photoJP2Sufix = oIni.ReadValue("Save", "photojp2sufix", "<F2>");
                _signJP2Sufix = oIni.ReadValue("Save", "signjp2sufix", "<P2>");

                if (!int.TryParse(oIni.ReadValue("Save", "photomaxweight", "18000"), out _photoMaxWeight)) _photoMaxWeight = 18000;
                if (!int.TryParse(oIni.ReadValue("Save", "signmaxweight", "8000"), out _signMaxWeight)) _signMaxWeight = 8000;
                if (!int.TryParse(oIni.ReadValue("Save", "compressdpi", "300"), out _compressDPI)) _compressDPI = 300;

                _deviceuuid = oIni.ReadValue("Scan", "deviceuuid", AppDomain.CurrentDomain.BaseDirectory);
                
                _signRect = RectAndINI.ReadRectFromIni(oIni, "Regionsign");
                _photoRect = RectAndINI.ReadRectFromIni(oIni, "Regionphoto");
                _barRect = RectAndINI.ReadRectFromIni(oIni, "Regionbar");
            }
        }

        private void buttonClearSkannerUUID_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                //Connect to Ini File "Config.ini" in current directory
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);
                oIni.WriteValue("Scan", "deviceuuid", "");
            }
            string _olddeviceuuid = _deviceuuid;
            _deviceuuid = "";
            this.Info(String.Format("Droped old scanner with uuid=\"{0}\"", _olddeviceuuid), this.Messages("scannerUUIDDroped"));
        }

        private void buttonSignatureRect_Click(object sender, EventArgs e)
        {
            using (Crop fCrop = new Crop(Path.GetTempPath() + "tmp.jpg", iniFileName, "Regionsign"))
                fCrop.ShowDialog();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);

                _signRect = RectAndINI.ReadRectFromIni(oIni, "Regionsign");
            }

            using (var fs = new FileStream(Path.GetTempPath() + "tmp.jpg", FileMode.Open)) //File not block
            {
                var bmp = new Bitmap(fs);
                var sourceimg = (Bitmap)bmp.Clone();

                //Signature
                pictureBoxSignature.Image = ImageComputation.ImageConvertions.MakeGrayscale3(new Bitmap(sourceimg).Clone(_signRect, PixelFormat.Format24bppRgb));
            }
        }

        private void buttonPhotoRect_Click(object sender, EventArgs e)
        {
            using (Crop fCrop = new Crop(Path.GetTempPath() + "tmp.jpg", iniFileName, "Regionphoto"))
                fCrop.ShowDialog();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);

                _photoRect = RectAndINI.ReadRectFromIni(oIni, "Regionphoto");
            }

            using (var fs = new FileStream(Path.GetTempPath() + "tmp.jpg", FileMode.Open)) //File not block
            {
                var bmp = new Bitmap(fs);
                var sourceimg = (Bitmap)bmp.Clone();

                //Photo
                pictureBoxPhoto.Image = new Bitmap(sourceimg).Clone(_photoRect, PixelFormat.Format24bppRgb);
            }
        }

        private void buttonBarRect_Click(object sender, EventArgs e)
        {
            using (Crop fCrop = new Crop(Path.GetTempPath() + "tmp.jpg", iniFileName, "Regionbar"))
                fCrop.ShowDialog();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);

                _barRect = RectAndINI.ReadRectFromIni(oIni, "Regionbar");
            }

            //BarCode
            using (var fs = new FileStream(Path.GetTempPath() + "tmp.jpg", FileMode.Open)) //File not block
            {
                var bmp = new Bitmap(fs);
                var sourceimg = (Bitmap)bmp.Clone();
                string numb = ScanQRCode(_barRect, sourceimg);
                textBoxNumber.Text = (!String.IsNullOrWhiteSpace(numb)) ? numb : textBoxNumber.Text;
                if (String.IsNullOrWhiteSpace(numb))
                    textBoxNumber.ReadOnly = checkBoxBarNumber.Checked = false;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.Delete(Path.GetTempPath() + "tmp.jpg");

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + iniFileName))
            {
                //Connect to Ini File "Config.ini" in current directory
                IniInterface oIni = new IniInterface(AppDomain.CurrentDomain.BaseDirectory + iniFileName);
                oIni.WriteValue("Scan", "auto", checkBoxAuto.Checked ? "true" : "false");
                oIni.WriteValue("Scan", "barNumber", checkBoxBarNumber.Checked ? "true" : "false");
                this.Info("Save to ini file \"barNumber\" and \"auto\" flags");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxNumber.Text))
                this.Warn(new ArgumentNullException("textBoxNumber.Text"), this.Messages("sertificateNumberIsEmpty"));
            else
            {
                string log = "";
                string successlog = "";

                try
                {
                    log = successlog = "Saved in files:\n"; //

                    string signjpgpath = String.Format(@"{0}\{1}{2}{3}.jpg", _path, _signPrefix, textBoxNumber.Text, _signSufix);
                    string photojpgpath = String.Format(@"{0}\{1}{2}{3}.jpg", _path, _photoPrefix, textBoxNumber.Text, _photoSufix);
                    string signjp2path = String.Format(@"{0}\{1}{2}{3}.jp2", _path, _signJP2Prefix, textBoxNumber.Text, _signJP2Sufix);
                    string photojp2path = String.Format(@"{0}\{1}{2}{3}.jp2", _path, _photoJP2Prefix, textBoxNumber.Text, _photoJP2Sufix);
                    
                    successlog = String.Format("Signature in jpg -- \"{0}\";\n", signjpgpath) +
                                 String.Format("Photo in jpg -- \"{0}\";\n", photojpgpath) +
                                 String.Format("Signature in jp2 -- \"{0}\";\n", signjp2path) +
                                 String.Format("Photo in jp2 -- \"{0}\";\n", photojp2path);

                    log = CertificateScanner.WaiteWindow.WaitWindow.Show(SaveImagesWorkerMethod, this.Messages("saveProgress"), 
                        new object[] { log, signjpgpath, photojpgpath, signjp2path, photojp2path }).ToString();
                    //SaveImages(log, signjpgpath, photojpgpath, signjp2path, photojp2path);

                    this.Info(log, this.Messages("resultSaved"));
                }
                catch (Exception ex)
                {
                    this.Error(new Exception(String.Format("successfully saved: {0}\n Must be:{1}", log, successlog), ex), 
                               this.Messages("resultCntSave"));
                }
            }
        }

        private void SaveImagesWorkerMethod(object sender, CertificateScanner.WaiteWindow.WaitWindowEventArgs e)
        {
            var log = (String)e.Arguments[0];

            pictureBoxSignature.Image.Save((String)e.Arguments[1], ImageFormat.Jpeg);
            log += String.Format("Signature in jpg -- \"{0}\";\n", (String)e.Arguments[1]);

            pictureBoxPhoto.Image.Save((String)e.Arguments[2], ImageFormat.Jpeg);
            log += String.Format("Photo in jpg -- \"{0}\";\n", (String)e.Arguments[2]);

            SaveInJP2((String)e.Arguments[1], (String)e.Arguments[3], _signMaxWeight);
            log += String.Format("Signature in jp2 -- \"{0}\";\n", (String)e.Arguments[3]);

            SaveInJP2((String)e.Arguments[2], (String)e.Arguments[4], _photoMaxWeight);
            log += String.Format("Photo in jp2 -- \"{0}\";\n", (String)e.Arguments[4]);
            e.Result = log;
        }

        private void SaveInJP2(string input, string output, int maxWeight)
        {
            FIBITMAP dib = new FIBITMAP();
            bool dpiChanged = false;
            // Load bitmap           
            if (!LoadJpegToDib(input, out dib))
                return;

            // Convert Bitmap to JPEG2000 and save it on the hard disk
            FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_NONE);
            var weight = new FileInfo(output).Length;
            if (_dpi > _compressDPI)
            {
                using (var fs = new FileStream(input, FileMode.Open)) //File not block
                {
                    var bmp = new Bitmap(fs);
                    double ratio;
                    ImageComputation.ImageConvertions.ScaleImage(bmp, (bmp.Width * _compressDPI) / _dpi, (bmp.Height * _compressDPI) / _dpi, out ratio).Save(
                        Path.GetTempPath() + "tmpdpi.jpg", ImageFormat.Jpeg);
                }
                if (!LoadJpegToDib(Path.GetTempPath() + "tmpdpi.jpg", out dib))
                    return;
                dpiChanged = true;
            }

            int stepcompression = 0;
            while ((weight > maxWeight) && (stepcompression < 14))
            {
                switch (stepcompression)
                {
                    case 0:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_NONE);
                        break;
                    case 1:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PIZ);
                        break;
                    case 2:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PXR24);
                        break;
                    case 3:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PXR24 | FREE_IMAGE_SAVE_FLAGS.EXR_PIZ);
                        break;
                    case 4:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_B44);
                        break;
                    case 5:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_B44 | FREE_IMAGE_SAVE_FLAGS.EXR_PIZ);
                        break;
                    case 6:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_B44 | FREE_IMAGE_SAVE_FLAGS.EXR_PXR24 | FREE_IMAGE_SAVE_FLAGS.EXR_PIZ);
                        break;
                    case 7:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_LC);
                        break;
                    case 8:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PIZ | FREE_IMAGE_SAVE_FLAGS.EXR_LC);
                        break;
                    case 9:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PXR24 | FREE_IMAGE_SAVE_FLAGS.EXR_LC);
                        break;
                    case 10:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PIZ | FREE_IMAGE_SAVE_FLAGS.EXR_PXR24 | FREE_IMAGE_SAVE_FLAGS.EXR_LC);
                        break;
                    case 11:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_B44 | FREE_IMAGE_SAVE_FLAGS.EXR_LC);
                        break;
                    case 12:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PIZ | FREE_IMAGE_SAVE_FLAGS.EXR_B44 | FREE_IMAGE_SAVE_FLAGS.EXR_LC);
                        break;
                    default:
                        FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JP2, dib, output, FREE_IMAGE_SAVE_FLAGS.EXR_PIZ | FREE_IMAGE_SAVE_FLAGS.EXR_B44 | FREE_IMAGE_SAVE_FLAGS.EXR_PXR24 | FREE_IMAGE_SAVE_FLAGS.EXR_LC);
                        break;
                }

                weight = new FileInfo(output).Length;
                stepcompression++;
            }

            // Unload source bitmap
            FreeImage.UnloadEx(ref dib);
            if (dpiChanged)
                File.Delete(Path.GetTempPath() + "tmpdpi.jpg");
        }

        private bool LoadJpegToDib(string input, out FIBITMAP dib)
        {
            dib = FreeImage.LoadEx(input);
            // Check success
            if (dib.IsNull)
            {
                this.Error(new FileNotFoundException("", input), this.Messages("jp2CdntLoadSource"));
                return false;
            }
            return true;
        }

        private void checkBoxBarNumber_CheckedChanged(object sender, EventArgs e)
        {
            if ((buttonSave.Enabled) && (checkBoxBarNumber.Checked))
            {
                using (var fs = new FileStream(Path.GetTempPath() + "tmp.jpg", FileMode.Open)) //File not block
                {
                    var bmp = new Bitmap(fs);
                    var sourceimg = (Bitmap)bmp.Clone();
                    string numb = ScanQRCode(_barRect, sourceimg);
                    textBoxNumber.Text = (!String.IsNullOrWhiteSpace(numb)) ? numb : textBoxNumber.Text;
                    if (String.IsNullOrWhiteSpace(numb))
                        textBoxNumber.ReadOnly = checkBoxBarNumber.Checked = false;
                }
            }
            textBoxNumber.ReadOnly = checkBoxBarNumber.Checked;
        }

        private string ScanQRCode(Rectangle _barRect, Bitmap sourceimg)
        {
            try
            {
                //var sR = sourceimg.Clone(_barRect, PixelFormat.Max);
                /*if (_dpi > _compressDPI)
                {
                    double ratio;
                    Image qrimage = ImageComputation.ImageConvertions.ScaleImage(sourceimg, (sourceimg.Width * _compressDPI) / _dpi, (sourceimg.Height * _compressDPI) / _dpi, out ratio);
                    var result = CertificateScanner.WaiteWindow.WaitWindow.Show(QRWorkerMethod, this.Messages("qrProgress"), new object[] { new Bitmap(qrimage).Clone(_barRect, System.Drawing.Imaging.PixelFormat.Format24bppRgb) });
                    return result.ToString();
                }*/
                var result = CertificateScanner.WaiteWindow.WaitWindow.Show(QRWorkerMethod, this.Messages("qrProgress"), new object[] { sourceimg.Clone(_barRect, System.Drawing.Imaging.PixelFormat.Format24bppRgb) });
                return result.ToString();
            }
            catch(Exception ex)
            {
                this.Warn(ex, this.Messages("qrFail"));
            }
            return "";
        }

        private void QRWorkerMethod(object sender, CertificateScanner.WaiteWindow.WaitWindowEventArgs e)
        {
            try
            {
                var sourceimg = (Bitmap)e.Arguments[0];

                AForge.Imaging.Filters.ContrastStretch filter = new AForge.Imaging.Filters.ContrastStretch();
                sourceimg = filter.Apply(sourceimg);
                sourceimg = filter.Apply(sourceimg);
                sourceimg = filter.Apply(sourceimg);

                AForge.Imaging.Filters.Median f1 = new AForge.Imaging.Filters.Median();
                sourceimg = f1.Apply(sourceimg);
                sourceimg = f1.Apply(sourceimg);
                sourceimg = f1.Apply(sourceimg);

                //for (int j = 0; j < sourceimg.Height; j++)
                //    for (int i = 0; i < sourceimg.Width; i++)
                //    {
                //        if (sourceimg.GetPixel(i , j).ToArgb() < -3355443)
                //        {
                //            sourceimg.SetPixel(i, j, Color.Black);
                //        }
                //        if (sourceimg.GetPixel(i , j).ToArgb() > -3355443)
                //        {
                //            sourceimg.SetPixel(i, j, Color.White);
                //        }
                //    }

                AForge.Imaging.Filters.Blur filter2 = new AForge.Imaging.Filters.Blur();
                sourceimg = filter2.Apply(sourceimg);
                sourceimg = filter2.Apply(sourceimg);

                //sourceimg.Save("d:\\tst.jpg", ImageFormat.Jpeg);
                //System.Diagnostics.Process.Start("d:\\tst.jpg");
               
                com.google.zxing.LuminanceSource source = new RGBLuminanceSource(sourceimg, sourceimg.Width, sourceimg.Height);
                com.google.zxing.Binarizer binarizer = new com.google.zxing.common.HybridBinarizer(source);

                com.google.zxing.qrcode.QRCodeReader reader = new com.google.zxing.qrcode.QRCodeReader();

                var result = reader.decode(new com.google.zxing.BinaryBitmap(binarizer));
                e.Result = result.Text;

            }
            catch (Exception ex)
            {
                e.Result = "12323123213123";
                this.Warn(ex, this.Messages("qrFail"));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fs = new FileStream("d:\\tmp.jpg", FileMode.Open)) //File not block
                {
                    var sourceimg = new Bitmap(fs);
                    var result = CertificateScanner.WaiteWindow.WaitWindow.Show(QRWorkerMethod, this.Messages("qrProgress"), new object[] { sourceimg.Clone(_barRect, System.Drawing.Imaging.PixelFormat.Format24bppRgb) });
                    MessageBox.Show(result.ToString());
                }
            }
            catch (Exception ex)
            {
                this.Warn(ex, this.Messages("qrFail"));
            }
        }

        
    }
}