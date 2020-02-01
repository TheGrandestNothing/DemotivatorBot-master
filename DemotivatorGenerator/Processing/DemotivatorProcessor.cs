using DemotivatorGenerator.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace DemotivatorGenerator.Processing
{
    public class DemotivatorProcessor : IImageProcessor
    {
        public string UpperText { get; set; }
        public string LowerText { get; set; }

        public DemotivatorProcessor(string upperText = "ЭТО ДИМАТИВАТОР", string lowerText = "нет")
        {
            this.UpperText = upperText;
            this.LowerText = lowerText;
        }

        public Bitmap Process(Bitmap inputImage)
        {
            var procImg = (Bitmap)((DemotivatorPixels)inputImage);
            DrawText(procImg, inputImage.Width / 15, inputImage.Width / 25, inputImage.Height > inputImage.Width ? inputImage.Width : inputImage.Height);

            return procImg;
        }

        private void DrawText(Bitmap procImg, int strSize, int smallStrSize, int lowerSide)
        {
            using (var graphics = Graphics.FromImage(procImg))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var str = UpperText;
                var smallStr = LowerText;
                var lowerAdditionHeight = lowerSide / 4;
                var upperAdditionHeight = lowerSide / 20;
                var addtionWidth = lowerSide / 16;
                var sizeInPixels = (int)((double)strSize / 3 * 4);
                var smallSizeInPixels = (int)((double)smallStrSize / 3 * 4);
                var font = new Font("Times New Roman", strSize);
                var smallFont = new Font("Times New Roman", smallStrSize);
                var fontStringSize = graphics.MeasureString(str, font);
                var smallFontStringSize = graphics.MeasureString(smallStr, smallFont);
                graphics.DrawString(str, font, Brushes.White, new RectangleF((procImg.Width / 2) - (fontStringSize.Width/2), procImg.Height - lowerAdditionHeight, fontStringSize.Width, fontStringSize.Height + 5));
                graphics.DrawString(smallStr, smallFont, Brushes.White, new RectangleF((procImg.Width / 2) - (smallFontStringSize.Width / 2), procImg.Height - lowerAdditionHeight + fontStringSize.Height + 5, smallFontStringSize.Width, smallFontStringSize.Height + 5));
            }
        }
    }
}
