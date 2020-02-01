using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DemotivatorGenerator.Processing
{
    interface IImageProcessor
    {
        Bitmap Process(Bitmap inputImage); 
    }
}
