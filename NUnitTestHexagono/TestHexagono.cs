using NUnit.Framework;
using Hexagono;
using Shared;
namespace NUnitTestHexagono
{
    public class TestHexagono
    {
        private static void EqualHex(string name, Hex a, Hex b)
        {
            if (!(a.Q == b.Q && a.S == b.S && a.R == b.R))
            {
                Assert.Fail(name);
            }
        }

        private static void EqualOffsetCoord(string name, Coordinate a, Coordinate b)
        {
            if (!(a.Col == b.Col && a.Row == b.Row ))
            {
                Assert.Fail(name);
            }
        }

        private static void EqualDoubledCoord(string name, Coordinate a, Coordinate b)
        {
            if (!(a.Col == b.Col && a.Row == b.Row))
            {
                Assert.Fail(name);
            }
        }

        private static void EqualInt(string name, int a, int b)
        {
            if (!(a == b))
            {
                Assert.Fail(name);
            }
        }

        private static void EqualHexArray(string name, Hex [] a, Hex [] b)
        {
            EqualInt(name, a.Length, b.Length);
            for (var i = 0; i < a.Length; i++)
            {
                EqualHex(name, a[i], b[i]);
            }
        }

        private static void test_hex_arithmetic()
        {
            
            EqualHex("hex_add", new Hex(4, -10, 6), HexagonFunction.HexagonAdd(new Hex(1, -3, 2), new Hex(3, -7, 4)));
            EqualHex("hex_subtract", new Hex(-2, 4, -2), HexagonFunction.HexagonSubtract(new Hex(1, -3, 2), new Hex(3, -7, 4)));
        }

        private static void test_hex_direction()
        {
            EqualHex("hex_direction", new Hex(0, -1, 1), HexagonFunction.HexagonDirection((2)));
        }

        private static void test_hex_neighbor()
        {
            EqualHex("hex_neighbor", new Hex(1, -3, 2), HexagonFunction.HexagonNeighbor (new Hex(1, -2, 1), 2));
        }

        private static void test_hex_diagonal()
        {
            EqualHex("hex_diagonal", new Hex(-1, -1, 2), HexagonFunction.HexagonDiagonalNeighbor(new Hex(1, -2, 1), 3));
        }

        private static void test_hex_distance()
        {
            EqualInt("hex_distance", 7,(int) HexagonFunction.HexagonDistance(new Hex(3, -7, 4), new Hex(0, 0, 0)));
        }

        private static void test_hex_rotate_right()
        {
            EqualHex("hex_rotate_right", HexagonFunction.HexagonRotateRight(new Hex(1, -3, 2)), new Hex(3, -2, -1));
        }

        private static void test_hex_rotate_left()
        {
            EqualHex("hex_rotate_left", HexagonFunction.HexagonRotateLeft(new Hex(1, -3, 2)), new Hex(-2, -1, 3));
        }

        private static void test_hex_round()
        {
            var a = new Hex(0.0m, 0.0m, 0.0m);
            var b = new Hex(1.0m, -1.0m, 0.0m);
            var c = new Hex(0.0m, -1.0m, 1.0m);
            EqualHex ("hex_round 1", new Hex(5, -10, 5), HexagonFunction.HexagonRound(HexagonFunction.HexagonLerp(new Hex(0.0m, 0.0m, 0.0m), new Hex(10.0m, -20.0m, 10.0m), 0.5m)));
            EqualHex("hex_round 2", HexagonFunction.HexagonRound(a), HexagonFunction.HexagonRound(HexagonFunction.HexagonLerp(a, b, 0.499m)));
            EqualHex("hex_round 3", HexagonFunction.HexagonRound(b), HexagonFunction.HexagonRound(HexagonFunction.HexagonLerp(a, b, 0.501m)));
            EqualHex("hex_round 4", HexagonFunction.HexagonRound(a), HexagonFunction.HexagonRound(new Hex(a.Q * 0.4m + b.Q * 0.3m + c.Q * 0.3m, a.R * 0.4m + b.R * 0.3m + c.R * 0.3m, a.S * 0.4m + b.S * 0.3m + c.S * 0.3m)));
            EqualHex("hex_round 5", HexagonFunction.HexagonRound(c), HexagonFunction.HexagonRound(new Hex(a.Q * 0.3m + b.Q * 0.3m + c.Q * 0.4m, a.R * 0.3m + b.R * 0.3m + c.R * 0.4m, a.S * 0.3m + b.S * 0.3m + c.S * 0.4m)));
        }

        private static void test_hex_linedraw()
        {
            EqualHexArray("hex_linedraw", new Hex[] { new Hex(0, 0, 0), new Hex(0, -1, 1), new Hex(0, -2, 2), new Hex(1, -3, 2), new Hex(1, -4, 3), new Hex(1, -5, 4) }, 
                HexagonFunction.HexagonLinedraw(new Hex(0, 0, 0), new Hex(1, -5, 4)).ToArray());
        }

        private static void test_layout()
        {

            var h = new Hex(3, 4, -7);
            var flat = new Layout(HexagonFunction.LayoutFlat, new Point(10, 15), new Point(250, 259));
            
            EqualHex("LayoutFlat", h, HexagonFunction.HexagonRound(HexagonFunction.PixelToHexagon(flat, HexagonFunction.HexagonToPixel(flat, h))));
            
            var pointy = new Layout(HexagonFunction.LayoutPointy, new Point(10, 15), new Point(35, 71));
            
            EqualHex("LayoutPointy", h, HexagonFunction.HexagonRound(HexagonFunction.PixelToHexagon(pointy, HexagonFunction.HexagonToPixel(pointy, h))));

        }

        private static void test_offset_roundtrip()
        {
            var a = new Hex(3, 4, -7);
            var b = new Coordinate(1, -3);
            EqualHex("conversion_roundtrip even-q", a, HexagonFunction.QOffsetToCube(HexagonFunction.EVEN, HexagonFunction.QOffsetFromCube(HexagonFunction.EVEN, a)));
            EqualOffsetCoord("conversion_roundtrip even-q", b, HexagonFunction.QOffsetFromCube(HexagonFunction.EVEN, HexagonFunction.QOffsetToCube(HexagonFunction.EVEN, b)));
            EqualHex("conversion_roundtrip odd-q", a, HexagonFunction.QOffsetToCube(HexagonFunction.ODD, HexagonFunction.QOffsetFromCube(HexagonFunction.ODD, a)));
            EqualOffsetCoord("conversion_roundtrip odd-q", b, HexagonFunction.QOffsetFromCube(HexagonFunction.ODD, HexagonFunction.QOffsetToCube(HexagonFunction.ODD, b)));
            EqualHex("conversion_roundtrip even-r", a, HexagonFunction.ROffsetToCube(HexagonFunction.EVEN, HexagonFunction.ROffsetFromCube(HexagonFunction.EVEN, a)));
            EqualOffsetCoord("conversion_roundtrip even-r", b, HexagonFunction.ROffsetFromCube(HexagonFunction.EVEN, HexagonFunction.ROffsetToCube(HexagonFunction.EVEN, b)));
            EqualHex("conversion_roundtrip odd-r", a, HexagonFunction.ROffsetToCube(HexagonFunction.ODD, HexagonFunction.ROffsetFromCube(HexagonFunction.ODD, a)));
            EqualOffsetCoord("conversion_roundtrip odd-r", b, HexagonFunction.ROffsetFromCube(HexagonFunction.ODD, HexagonFunction.ROffsetToCube(HexagonFunction.ODD, b)));
        }

        private static void test_offset_from_cube()
        {
            EqualOffsetCoord("offset_from_cube even-q", new Coordinate(1, 3), HexagonFunction.QOffsetFromCube(HexagonFunction.EVEN, new Hex(1, 2, -3)));
            EqualOffsetCoord("offset_from_cube odd-q", new Coordinate(1, 2), HexagonFunction.QOffsetFromCube(HexagonFunction.ODD, new Hex(1, 2, -3)));
        }

        private static void test_offset_to_cube()
        {
            EqualHex("offset_to_cube even-", new Hex(1, 2, -3), HexagonFunction.QOffsetToCube(HexagonFunction.EVEN, new Coordinate(1, 3)));
            EqualHex("offset_to_cube odd-q", new Hex(1, 2, -3), HexagonFunction.QOffsetToCube(HexagonFunction.ODD, new Coordinate(1, 2)));
        }

        private static void test_doubled_roundtrip()
        {
            var a = new Hex(3, 4, -7);
            var b = new Coordinate(1, -3);
            EqualHex("conversion_roundtrip doubled-q", a, HexagonFunction.QDoubledToCube(HexagonFunction.QDoubledFromCube(a)));
            EqualDoubledCoord("conversion_roundtrip doubled-q", b, HexagonFunction.QDoubledFromCube(HexagonFunction.QDoubledToCube(b)));
            EqualHex("conversion_roundtrip doubled-r", a, HexagonFunction.RDoubledToCube(HexagonFunction.RDoubledFromCube(a)));
            EqualDoubledCoord("conversion_roundtrip doubled-r", b, HexagonFunction.RDoubledFromCube(HexagonFunction.RDoubledToCube(b)));
        }

        private static void test_doubled_from_cube()
        {
            EqualDoubledCoord("doubled_from_cube doubled-q", new Coordinate(1, 5), HexagonFunction.QDoubledFromCube(new Hex(1, 2, -3)));
            EqualDoubledCoord("doubled_from_cube doubled-r", new Coordinate(4, 2), HexagonFunction.RDoubledFromCube(new Hex(1, 2, -3)));
        }

        private static void test_doubled_to_cube()
        {
            EqualHex("doubled_to_cube doubled-q", new Hex(1, 2, -3), HexagonFunction.QDoubledToCube(new Coordinate(1, 5)));
            EqualHex("doubled_to_cube doubled-r", new Hex(1, 2, -3), HexagonFunction.RDoubledToCube(new Coordinate(4, 2)));
        }

        public static void test_all()
        {
            test_hex_arithmetic();
            test_hex_direction();
            test_hex_neighbor();
            test_hex_diagonal();
            test_hex_distance();
            test_hex_rotate_right();
            test_hex_rotate_left();
            test_hex_round();
            test_hex_linedraw();
            test_layout();
            test_offset_roundtrip();
            test_offset_from_cube();
            test_offset_to_cube();
            test_doubled_roundtrip();
            test_doubled_from_cube();
            test_doubled_to_cube();
        }
    }
}
