using System.Numerics;

namespace FanKit.Transformer.UI
{
    public struct Plane
    {
        public Vector3 LeftTop;
        public Vector3 RightTop;
        public Vector3 LeftBottom;
        public Vector3 RightBottom;

        public Plane Add(Vector3 adds)
        {
            return new Plane
            {
                LeftTop = this.LeftTop + adds,
                RightTop = this.RightTop + adds,
                LeftBottom = this.LeftBottom + adds,
                RightBottom = this.RightBottom + adds,
            };
        }

        public Vector3 Center()
        {
            return (this.LeftTop
                + this.RightTop
                + this.RightBottom
                + this.LeftBottom) / 4f;
        }

        public Quadrilateral Rasterize(Vector2 position)
        {
            return new Quadrilateral
            {
                LeftTop = Cube.Rasterize(this.LeftTop, position),
                RightTop = Cube.Rasterize(this.RightTop, position),
                RightBottom = Cube.Rasterize(this.RightBottom, position),
                LeftBottom = Cube.Rasterize(this.LeftBottom, position)
            };
        }
    }
}