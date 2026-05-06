using System.Numerics;

namespace FanKit.Transformer.Cache
{
    public readonly partial struct Line0
    {
        // Line
        public readonly Vector2 Point0;
        public readonly Vector2 Point1;

        #region Constructors
        public Line0(Vector2 point0, Vector2 point1)
        {
            // Line
            this.Point0 = point0;
            this.Point1 = point1;
        }

        public Line0(Vector2 point0, Vector2 point1, ICanvasMatrix canvasMatrix)
        {
            // Line
            this.Point0 = canvasMatrix.Transform(point0);
            this.Point1 = canvasMatrix.Transform(point1);
        }
        #endregion Constructors
    }
}