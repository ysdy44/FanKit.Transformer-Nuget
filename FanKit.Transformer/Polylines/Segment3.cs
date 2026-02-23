using System.Numerics;

namespace FanKit.Transformer.Polylines
{
    public struct Segment3
    {
        public bool IsChecked;

        public Vector2 Starting;

        public Vector2 Raw;
        public Vector2 Map;
        public Vector2 Actual;
    }
}