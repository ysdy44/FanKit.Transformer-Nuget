using System.Numerics;

namespace FanKit.Transformer.UI
{
    public struct Cube
    {
        public Vector3 BackLeftTop;
        public Vector3 BackRightTop;
        public Vector3 BackLeftBottom;
        public Vector3 BackRightBottom;

        public Vector3 FrontLeftTop;
        public Vector3 FrontRightTop;
        public Vector3 FrontLeftBottom;
        public Vector3 FrontRightBottom;

        public Cube Add(Vector3 adds)
        {
            return new Cube
            {
                BackLeftTop = this.BackLeftTop + adds,
                BackRightTop = this.BackRightTop + adds,
                BackLeftBottom = this.BackLeftBottom + adds,
                BackRightBottom = this.BackRightBottom + adds,

                FrontLeftTop = this.FrontLeftTop + adds,
                FrontRightTop = this.FrontRightTop + adds,
                FrontLeftBottom = this.FrontLeftBottom + adds,
                FrontRightBottom = this.FrontRightBottom + adds,
            };
        }

        public Vector3 Center()
        {
            return (this.BackLeftTop
                + this.BackRightTop
                + this.BackRightBottom
                + this.BackLeftBottom
                + this.FrontLeftTop
                + this.FrontRightTop
                + this.FrontRightBottom
                + this.FrontLeftBottom) / 8f;
        }

        public Vector3 CenterOfBack()
        {
            return (this.BackLeftTop +
                this.BackRightTop +
                this.BackRightBottom +
                this.BackLeftBottom) / 4f;
        }

        public Vector3 CenterOfFront()
        {
            return (this.FrontLeftTop +
                this.FrontRightTop +
                this.FrontRightBottom +
                this.FrontLeftBottom) / 4f;
        }

        public Vector3 CenterOfLeft()
        {
            return (this.FrontLeftTop +
                this.BackLeftTop +
                this.BackLeftBottom +
                this.FrontLeftBottom) / 4f;
        }

        public Vector3 CenterOfRight()
        {
            return (this.FrontRightTop +
                this.BackRightTop +
                this.BackRightBottom +
                this.FrontRightBottom) / 4f;
        }

        public Vector3 CenterOfTop()
        {
            return (this.FrontLeftTop +
                this.BackLeftTop +
                this.BackRightTop +
                this.FrontRightTop) / 4f;
        }

        public Vector3 CenterOfBottom()
        {
            return (this.FrontRightBottom +
                this.BackLeftBottom +
                this.BackRightBottom +
                this.FrontRightBottom) / 4f;
        }

        public Quadrilateral RasterizeBack(Vector2 position)
        {
            return new Quadrilateral
            {
                LeftTop = Rasterize(this.BackRightTop, position),
                RightTop = Rasterize(this.BackLeftTop, position),
                RightBottom = Rasterize(this.BackLeftBottom, position),
                LeftBottom = Rasterize(this.BackRightBottom, position)
            };
        }

        public Quadrilateral RasterizeFront(Vector2 position)
        {
            return new Quadrilateral
            {
                LeftTop = Rasterize(this.FrontLeftTop, position),
                RightTop = Rasterize(this.FrontRightTop, position),
                RightBottom = Rasterize(this.FrontRightBottom, position),
                LeftBottom = Rasterize(this.FrontLeftBottom, position)
            };
        }

        public Quadrilateral RasterizeLeft(Vector2 position)
        {
            return new Quadrilateral
            {
                LeftTop = Rasterize(this.BackLeftTop, position),
                RightTop = Rasterize(this.FrontLeftTop, position),
                RightBottom = Rasterize(this.FrontLeftBottom, position),
                LeftBottom = Rasterize(this.BackLeftBottom, position)
            };
        }

        public Quadrilateral RasterizeRight(Vector2 position)
        {
            return new Quadrilateral
            {
                LeftTop = Rasterize(this.FrontRightTop, position),
                RightTop = Rasterize(this.BackRightTop, position),
                RightBottom = Rasterize(this.BackRightBottom, position),
                LeftBottom = Rasterize(this.FrontRightBottom, position),
            };
        }

        public Quadrilateral RasterizeTop(Vector2 position)
        {
            return new Quadrilateral
            {
                LeftTop = Rasterize(this.FrontLeftTop, position),
                RightTop = Rasterize(this.BackLeftTop, position),
                RightBottom = Rasterize(this.BackRightTop, position),
                LeftBottom = Rasterize(this.FrontRightTop, position)
            };
        }

        public Quadrilateral RasterizeBottom(Vector2 position)
        {
            return new Quadrilateral
            {
                LeftTop = Rasterize(this.BackLeftBottom, position),
                RightTop = Rasterize(this.FrontLeftBottom, position),
                RightBottom = Rasterize(this.FrontRightBottom, position),
                LeftBottom = Rasterize(this.BackRightBottom, position),
            };
        }

        public static Vector2 Rasterize(Vector3 point, Vector2 position)
        {
            const float yscale = 1f / 1000;

            float d = -point.Z;
            float z = 1 / (d * yscale + 1);

            float x = point.X * z + position.X;
            float y = point.Y * z + position.Y;

            return new Vector2(x, y);
        }
    }
}