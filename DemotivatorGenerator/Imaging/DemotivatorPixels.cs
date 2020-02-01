using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DemotivatorGenerator.Imaging
{
    public class DemotivatorPixels
    {
        public readonly Pixel[,] Pixels;
        public readonly int Height;
        public readonly int Width;

        public DemotivatorPixels(int height, int width)
        {
            this.Height = height;
            this.Width = width;
            this.Pixels = new Pixel[height, width];
        }

        public static explicit operator DemotivatorPixels(Bitmap bmp)
        {
            return ToDemotivatorPixels(bmp);
        }

        public static explicit operator Bitmap(DemotivatorPixels matrix)
        {
            return ToBitmap(matrix);
        }

        private static unsafe DemotivatorPixels ToDemotivatorPixels(Bitmap bitmap)
        {
            var lowerSide = bitmap.Height > bitmap.Width ? bitmap.Width : bitmap.Height;
            var lowerAdditionHeight = lowerSide / 4;
            var upperAdditionHeight = lowerSide / 20;
            var height = bitmap.Height + lowerAdditionHeight + upperAdditionHeight;

            var addtionWidth = lowerSide / 16;
            var width = bitmap.Width + addtionWidth * 2;

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var matrix = new DemotivatorPixels(height, width);
            Console.WriteLine(bitmap.PixelFormat);
            var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var ptr = bmpData.Scan0;

            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    if (j < upperAdditionHeight || i < addtionWidth || i >= width - addtionWidth || j >= height - lowerAdditionHeight)
                    {
                        if (j >= upperAdditionHeight - lowerSide/200 && j < height - lowerAdditionHeight + lowerSide / 200 && i >= addtionWidth - lowerSide / 200 && i < width - addtionWidth + lowerSide / 200)
                            matrix.Pixels[j, i] = new Pixel(255, 255, 255, 255);
                        else
                            matrix.Pixels[j, i] = new Pixel(0, 0, 0, 255);
                        continue;
                    }
                    var x = *(Pixel*)ptr;
                    ptr = ptr + 4;
                    matrix.Pixels[j, i] = x;
                }
            }

            bitmap.UnlockBits(bmpData);

            return matrix;
        }

        private static unsafe Bitmap ToBitmap(DemotivatorPixels matrix)
        {
            var bmp = new Bitmap(matrix.Width, matrix.Height);
            var rect = new Rectangle(0, 0, matrix.Width, matrix.Height);
            var bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var ptr = bmpData.Scan0;

            for (var j = 0; j < rect.Height; j++)
            {
                for (var i = 0; i < rect.Width; i++)
                {
                    var pixel = matrix.Pixels[j, i];
                    *(Pixel*)ptr = pixel;
                    ptr = ptr + 4;
                }
            }

            bmp.UnlockBits(bmpData);

            return bmp;
        }
    } 
}
