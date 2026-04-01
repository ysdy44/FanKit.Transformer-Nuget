using System.Numerics;

namespace FanKit.Transformer.Cache
{
    public readonly partial struct Line0
    {
        // Line
        public readonly Vector2 Point0;
        public readonly Vector2 Point1;

        #region Constructors
        public Line0(Vector2 point0, Vector2 point1, float handleLength = 32f)
        {
            // Line
            this.Point0 = point0;
            this.Point1 = point1;
        }

        public Line0(Vector2 point0, Vector2 point1, ICanvasMatrix matrix, float handleLength = 32f)
        {
            // Line
            this.Point0 = matrix.Transform(point0);
            this.Point1 = matrix.Transform(point1);
        }
        #endregion Constructors
    }
}