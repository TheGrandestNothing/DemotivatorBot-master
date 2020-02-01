using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace DemotivatorGenerator.Processing
{
    public class ResizeProcessor : IImageProcessor
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public ResizeProcessor(int height) : this(300, height)
        {

        }
        public ResizeProcessor(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void SetSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
             
        public Bitmap Process(Bitmap inputImage)
        {
            var destWidth = Width;
            var destHeight = Height;
            var destImg = new Bitmap(destWidth, destHeight);
            destImg.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);
            using (var graphics = Graphics.FromImage(destImg))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(inputImage, new Rectangle(0, 0, destWidth, destHeight), 0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImg;
        }
    }
}
