using Shared;
namespace Hexagono
{
    public struct Layout
    {
        public Layout(Orientation orientation, Point size, Point origin)
        {
            this.Orientation = orientation;
            this.Size = size;
            this.Origin = origin;
        }
        public Orientation Orientation { get; }
        public Point Size { get; }
        public Point Origin { get; }
    }
}
