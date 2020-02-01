using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace DemotivatorGenerator.Imaging
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Pixel : IEquatable<Pixel>
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Pixel(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public bool Equals([AllowNull] Pixel other)
        {
            return this.R.Equals(other.R) && this.G.Equals(other.G) && this.B.Equals(other.B) && this.A.Equals(other.A);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Pixel other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hash = (int)(this.R.GetHashCode());
            hash = (hash * 397) ^ this.G.GetHashCode();
            hash = (hash * 397) ^ this.B.GetHashCode();
            hash = (hash * 397) ^ this.A.GetHashCode();
            return hash;
        }
    }
}
