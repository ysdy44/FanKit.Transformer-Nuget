using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public static partial class Math
    {
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