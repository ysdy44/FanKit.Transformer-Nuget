using System.Numerics;

namespace FanKit.Transformer.UI
{
    public struct EarthLayout
    {
        // -180West ~ 180East
        public const float West = -180f; // 西经
        public const float East = 180f; // 东经
        public const float Longitude = 360f; // 经度

        // -90South ~ 90North
        public const float South = -90f; // 南纬
        public const float North = 90f; // 北纬
        public const float Latitude = 180f; // 纬度

        public float Radius;

        public Vector2 Center;

        public Vector2 GetPoint(Vector3 unitVector)
        {
            float x = unitVector.X * this.Radius + this.Center.X;
            float y = unitVector.Y * this.Radius + this.Center.Y;

            return new Vector2(x, y);
        }

        public Vector2 GetPoint(Vector3 unitVector, float radius)
        {
            float x = unitVector.X * radius + this.Center.X;
            float y = unitVector.Y * radius + this.Center.Y;

            return new Vector2(x, y);
        }

        public Vector2 Scroll(float horizontalOffset, float verticalOffset)
        {
            return new Vector2
            {
                X = -Mathematics.Math.PIOver2 * verticalOffset / this.Radius,
                Y = Mathematics.Math.PIOver2 * horizontalOffset / this.Radius,
            };
        }

        public Vector3 ScrollTo(Vector3 startingRadians, float horizontalOffset, float verticalOffset)
        {
            return new Vector3
            {
                X = startingRadians.X - Mathematics.Math.PIOver2 * verticalOffset / this.Radius,
                Y = startingRadians.Y + Mathematics.Math.PIOver2 * horizontalOffset / this.Radius,
                Z = 0f
            };
        }
    }
}