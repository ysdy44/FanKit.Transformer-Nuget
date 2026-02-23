using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    internal readonly partial struct Center6
    {
        internal readonly float x4, y4; // Center + Center + Center + Center
        internal readonly float x2, y2; // Center + Center
        internal readonly Vector2 c; // Center

        #region Triangles
        internal Center6(Triangle t)
        {
            x2 = t.RightTop.X + t.LeftBottom.X;
            y2 = t.RightTop.Y + t.LeftBottom.Y;

            x4 = x2 + x2;
            y4 = y2 + y2;

            c = new Vector2(x2 / 2f,
                y2 / 2f);
        }
        #endregion

        #region Quadrilaterals
        internal Center6(Quadrilateral t)
        {
            x4 = t.LeftTop.X + t.RightTop.X + t.RightBottom.X + t.LeftBottom.X;
            y4 = t.LeftTop.Y + t.RightTop.Y + t.RightBottom.Y + t.LeftBottom.Y;

            x2 = x4 / 2f;
            y2 = y4 / 2f;

            c = new Vector2(x4 / 4f,
                y4 / 4f);
        }
        #endregion
    }
}