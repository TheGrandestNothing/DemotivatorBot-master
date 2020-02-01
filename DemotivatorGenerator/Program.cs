using DemotivatorGenerator.Processing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DemotivatorGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Expected path argument! \n Usage DemotivatorGenerator.exe <path-to-pic> <upper-text> <lower-text> <amount>");
                return;
            }

            var path = args[0];
            var amount = args.Length > 3 ? int.Parse(args[3]) : 1;
            var upperText = args.Length > 1 ? args[1] : "ЭТО ДИМАТИВАТОР";
            var lowerText = args.Length > 2 ? args[2] : "нет";
            var bmp = Image.FromFile(path);
            if (bmp.Width < 200 || bmp.Height < 200)
            {
                var lowerSide = bmp.Width < bmp.Height ? bmp.Width : bmp.Height;
                var multiplier = 300 / lowerSide;
                var resizer = new ResizeProcessor(bmp.Width * multiplier, bmp.Height * multiplier);
                bmp = resizer.Process((Bitmap)bmp);
            }
            var processor = new DemotivatorProcessor(upperText, lowerText);
            var resizeProc = new ResizeProcessor(0, 0);
            Console.WriteLine(bmp.PixelFormat);
            var procImg = processor.Process((Bitmap)bmp);
            for (var i = 1; i < amount; i++)
            {
                procImg = processor.Process(procImg);
                resizeProc.SetSize(procImg.Width * 5 / 6, procImg.Height * 5 / 6);
                procImg = resizeProc.Process(procImg);
            }

            procImg.Save("sosat.jpg");
        }
    }
}
