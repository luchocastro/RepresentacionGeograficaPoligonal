namespace Hexagono
{
        public struct Orientation
        {
            public Orientation(decimal f0, decimal f1, decimal f2, decimal f3, decimal b0, decimal b1, decimal b2, decimal b3, decimal startAngle)
            {
                this.F0 = f0;
                this.F1 = f1;
                this.F2 = f2;
                this.F3 = f3;
                this.B0 = b0;
                this.B1 = b1;
                this.B2 = b2;
                this.B3 = b3;
                this.StartAngle = startAngle;
            }
                public decimal F0 { get;}
            public decimal F1 { get; }
            public decimal F2 { get; }
            public decimal F3 { get; }
            public decimal B0 { get; }
            public decimal B1 { get; }
            public decimal B2 { get; }
            public decimal B3 { get; }
            public decimal StartAngle { get; }

        }
    }

