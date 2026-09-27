using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct Camera
    {
        readonly Matrix4x4 Y;
        readonly Matrix4x4 X;
        readonly Matrix4x4 Z;
        readonly Matrix4x4 S;
        readonly Matrix4x4 T;
        readonly Matrix4x4 M;

        public Camera(Vector3 radians)
        {
            this.Y = Matrix4x4.CreateRotationY(radians.Y);
            this.X = Matrix4x4.CreateRotationX(radians.X);
            this.Z = Matrix4x4.CreateRotationZ(radians.Z);
            this.S = Matrix4x4.Identity;
            this.T = Matrix4x4.Identity;
            this.M = this.Y * this.X * this.Z;
        }

        public Camera(Vector3 radians, float scale, Vector2 position)
        {
            this.Y = Matrix4x4.CreateRotationY(radians.Y);
            this.X = Matrix4x4.CreateRotationX(radians.X);
            this.Z = Matrix4x4.CreateRotationZ(radians.Z);
            this.S = Matrix4x4.CreateScale(scale);
            this.T = Matrix4x4.CreateTranslation(position.X, position.Y, 0);
            this.M = this.Y * this.X * this.Z * this.S * this.T;
        }

        public Vector3 Transform(Vector3 point)
        {
            return Vector3.Transform(point, this.M);
        }

        public Plane Transform(Plane plane)
        {
            return new Plane
            {
                LeftTop = Vector3.Transform(plane.LeftTop, this.M),
                RightTop = Vector3.Transform(plane.RightTop, this.M),
                RightBottom = Vector3.Transform(plane.RightBottom, this.M),
                LeftBottom = Vector3.Transform(plane.LeftBottom, this.M)
            };
        }

        public Cube Transform(Cube cube)
        {
            return new Cube
            {
                BackLeftTop = Vector3.Transform(cube.BackLeftTop, this.M),
                BackRightTop = Vector3.Transform(cube.BackRightTop, this.M),
                BackLeftBottom = Vector3.Transform(cube.BackLeftBottom, this.M),
                BackRightBottom = Vector3.Transform(cube.BackRightBottom, this.M),

                FrontLeftTop = Vector3.Transform(cube.FrontLeftTop, this.M),
                FrontRightTop = Vector3.Transform(cube.FrontRightTop, this.M),
                FrontLeftBottom = Vector3.Transform(cube.FrontLeftBottom, this.M),
                FrontRightBottom = Vector3.Transform(cube.FrontRightBottom, this.M),
            };
        }

        public static Vector2 Scroll(float scale, float horizontalOffset, float verticalOffset)
        {
            return new Vector2
            {
                X = -Mathematics.Math.PIOver2 * verticalOffset / scale,
                Y = Mathematics.Math.PIOver2 * horizontalOffset / scale,
            };
        }

        public static Vector3 ScrollTo(Vector3 startingRadians, float scale, float horizontalOffset, float verticalOffset)
        {
            return new Vector3
            {
                X = startingRadians.X - Mathematics.Math.PIOver2 * verticalOffset / scale,
                Y = startingRadians.Y + Mathematics.Math.PIOver2 * horizontalOffset / scale,
                Z = 0f
            };
        }
    }
}