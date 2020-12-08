using System;

namespace Hexagono
{
    public struct Hex
    {
        public Hex(decimal q, decimal r, decimal s)
        {
            if (Math.Round(q + r + s) != 0) throw new Exception("q + r + s must be 0");

            R = r;
            S = s;
            Q = q;

        }

        public decimal Q { get; }
        public decimal R { get; }
        public decimal S { get; }

        public override string ToString() => $"({Q}, {R} , {R})";
        public decimal Length()
        {
            return (Math.Abs(this.Q) + Math.Abs(this.R) + Math.Abs(this.S)) / 2;
        }
    }
}
