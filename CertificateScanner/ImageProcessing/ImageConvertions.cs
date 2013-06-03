using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using AForge.Imaging.Filters;

namespace CertificateScanner.ImageComputation
{
    public static class ImageConvertions
    {
        public static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static Image ApplyRangeLevels(int minimum, int maximum, Image inputImage)
        {
            // create filter
            LevelsLinear filter = new LevelsLinear()
            { /* set ranges*/
                InRed = new AForge.IntRange(minimum, maximum),
                InGreen = new AForge.IntRange(minimum, maximum),
                InBlue = new AForge.IntRange(minimum, maximum)
            };
            // apply the filter
            using (Bitmap filteredImage = (Bitmap)inputImage.Clone())
            {
                filter.ApplyInPlace(filteredImage);
                return (Bitmap)filteredImage.Clone();
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight, out double ratio)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var inputratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * inputratio);
            var newHeight = (int)(image.Height * inputratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            ratio = inputratio;
            return newImage;
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static Stream ImageToStream(Image image, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, format);
            return ms;
        }

    }
}
