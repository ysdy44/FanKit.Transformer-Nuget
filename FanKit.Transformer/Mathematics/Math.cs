using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public static partial class Math
    {
        public const float PiTwice = Constants.PiTwice;
        public const float InvPI = Constants.InvPI;
        public const float PI = Constants.PI;
        public const float PIOver2 = Constants.PIOver2;
        public const float PIOver4 = Constants.PIOver4;

        public const float RadiansToDegrees = Constants.RadiansToDegrees;
        public const float DegreesToRadians = Constants.DegreesToRadians;

        public static bool ContainsNode(this Vector2 point0, Vector2 point1, float minLengthSquared = 144f)
        {
            return Vector2.DistanceSquared(point0, point1) < minLengthSquared;
        }

        public static float Dot(Vector2 value1, Vector2 value2)
        {
            float x = value1.X - value2.X;
            float y = value1.Y - value2.Y;
            return x * x + y * y;
        }
    }
}