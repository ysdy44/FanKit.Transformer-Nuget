using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Cache
{
    public readonly partial struct Box0
    {
        #region Public instance methods
        public Vector2[] To3Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.LeftBottom,
        };
        public Vector2[] To4Points() => new Vector2[]
        {
            this.LeftTop,
            this.RightTop,
            this.RightBottom,
            this.LeftBottom,
        };
        public Bounds ToBounds() => new Bounds
        {
            Left = System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), System.Math.Min(this.RightBottom.X, this.LeftBottom.X)),
            Top = System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), System.Math.Min(this.RightBottom.Y, this.LeftBottom.Y)),
            Right = System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), System.Math.Max(this.RightBottom.X, this.LeftBottom.X)),
            Bottom = System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), System.Math.Max(this.RightBottom.Y, this.LeftBottom.Y)),
        };
        public Triangle ToTriangle() => new Triangle
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
        };
        public Quadrilateral ToQuadrilateral() => new Quadrilateral
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            LeftBottom = this.LeftBottom,
            RightBottom = this.RightBottom,
        };

        public bool ContainsPoint(Vector2 point)
        {
            float x = point.X;
            float y = point.Y;

            Vector2 a = this.RightBottom;
            Vector2 b = this.LeftBottom;
            Vector2 c = this.RightTop;
            Vector2 d = this.LeftTop;

            // Contains
            switch (Comparer<float>.Default.Compare((d.X - b.X) * (y - b.Y), (d.Y - b.Y) * (x - b.X)))
            {
                case 1:
                    return (c.X - d.X) * (y - d.Y) > (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) > (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) > (b.Y - a.Y) * (x - a.X);
                case -1:
                    return (c.X - d.X) * (y - d.Y) < (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) < (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) < (b.Y - a.Y) * (x - a.X);
                default:
                    return false;
            }
        }

        public BoxContainsNodeMode0 ContainsNode(Vector2 point, float minSelectedLengthSquared = 144f, float minSideLengthSquared = 144f)
        {
            float x = point.X;
            float y = point.Y;

            // Corners
            Vector2 a = this.RightBottom;

            float ax = x - a.X;
            float ay = y - a.Y;

            float a2 = ax * ax + ay * ay;
            if (a2 < minSelectedLengthSquared)
                return BoxContainsNodeMode0.RightBottom;

            Vector2 b = this.LeftBottom;

            float bx = x - b.X;
            float by = y - b.Y;

            float b2 = bx * bx + by * by;
            if (b2 < minSelectedLengthSquared)
                return BoxContainsNodeMode0.LeftBottom;

            Vector2 c = this.RightTop;

            float cx = x - c.X;
            float cy = y - c.Y;

            float c2 = cx * cx + cy * cy;
            if (c2 < minSelectedLengthSquared)
                return BoxContainsNodeMode0.RightTop;

            Vector2 d = this.LeftTop;

            float dx = x - d.X;
            float dy = y - d.Y;

            float d2 = dx * dx + dy * dy;
            if (d2 < minSelectedLengthSquared)
                return BoxContainsNodeMode0.LeftTop;

            // Contains
            switch (Comparer<float>.Default.Compare((d.X - b.X) * (y - b.Y), (d.Y - b.Y) * (x - b.X)))
            {
                case 1:
                    if ((c.X - d.X) * (y - d.Y) > (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) > (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) > (b.Y - a.Y) * (x - a.X))
                        return BoxContainsNodeMode0.Contains;
                    else
                        return BoxContainsNodeMode0.None;
                case -1:
                    if ((c.X - d.X) * (y - d.Y) < (c.Y - d.Y) * (x - d.X) &&
                        (a.X - c.X) * (y - c.Y) < (a.Y - c.Y) * (x - c.X) &&
                        (b.X - a.X) * (y - a.Y) < (b.Y - a.Y) * (x - a.X))
                        return BoxContainsNodeMode0.Contains;
                    else
                        return BoxContainsNodeMode0.None;
                default:
                    return BoxContainsNodeMode0.None;
            }
        }
        #endregion Public instance methods
    }
}