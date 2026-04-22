using System.Numerics;
using System.Runtime.InteropServices;

namespace FanKit.Transformer.Controllers
{
    [StructLayout(LayoutKind.Explicit, Size = 13 * 4)]
    public readonly struct LineController
    {
        const int b1 = 4;
        const int b2 = 4;
        const int b3 = 2 * 4;

        const int b4 = 4;
        const int b5 = 4;
        const int b6 = 4;

        const int b7 = 4;
        const int b8 = 4;
        const int b9 = 4;

        const int b10 = 4;

        // Center
        [FieldOffset(0)] readonly float cx;
        [FieldOffset(b1)] readonly float cy;
        [FieldOffset(b1 + b2)] internal readonly Vector2 c;

        // Foot / Distance
        [FieldOffset(b1 + b2 + b3)] public readonly float abX; // dx
        [FieldOffset(b1 + b2 + b3 + b4)] public readonly float abY; // dy
        [FieldOffset(b1 + b2 + b3 + b4 + b5)] public readonly float xy; // ls

        // Rotate
        // Starting Angle of Transformer
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6)] internal readonly Angle r0; // Span 1: A

        // Angle of Point
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8 + b9)] internal readonly Angle r1; // Span 2: A

        // Size
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6)] readonly float d; // Span 1: B Distance
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6 + b7)] readonly float s; // Span 1: B Scale
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8)] readonly float i; // Span 1: B Inverse Scale

        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8 + b9)] readonly float x; // Span 2: B
        [FieldOffset(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8 + b9 + b10)] readonly float y; // Span 2: B

        // Rotate
        public LineController(Vector2 point0, Vector2 point1, Vector2 point)
        {
            // Center
            cx = point0.X + point1.X;
            cy = point0.Y + point1.Y;
            c = new Vector2(cx / 2f, cy / 2f);

            // Foot
            abX = default;
            abY = default;
            xy = default;

            #region Span 1: Rotate
            r0 = new Angle(point1, point0);

            r1 = new Angle(point, c);
            #endregion

            #region Span 2: Size
            d = default;
            s = default;
            i = default;

            x = default;
            y = default;
            #endregion
        }

        // ElongatePoint
        public LineController(Vector2 point0, Vector2 point1)
        {
            // Center
            cx = default;
            cy = default;
            c = default;

            // Foot
            abX = point0.X - point1.X;
            abY = point0.Y - point1.Y;
            xy = abX * abX + abY * abY;

            #region Span 1: Rotate
            r0 = default;

            r1 = default;
            #endregion

            #region Span 2: Size
            d = default;
            s = default;
            i = default;

            x = default;
            y = default;
            #endregion
        }

        // Size
        public LineController(float value, Vector2 point0, Vector2 point1, int mode)
        {
            switch (mode)
            {
                case 0:
                    // Center
                    cx = point0.X;
                    cy = point0.Y;
                    c = point0;

                    // Distance
                    abX = point1.X - point0.X;
                    abY = point1.Y - point0.Y;
                    xy = abX * abX + abY * abY;

                    #region Span 1: Rotate
                    r0 = default;
                    r1 = default;
                    #endregion

                    #region Span 2: Size
                    d = (float)System.Math.Sqrt(xy);
                    s = value / d;
                    i = 1f - s;

                    x = s * abX;
                    y = s * abY;
                    #endregion
                    break;
                case 1:
                    // Center
                    cx = point0.X + point1.X;
                    cy = point0.Y + point1.Y;
                    c = new Vector2(cx / 2f, cy / 2f);

                    // Distance
                    abX = point0.X - point1.X;
                    abY = point0.Y - point1.Y;
                    xy = abX * abX + abY * abY;

                    #region Span 1: Rotate
                    r0 = default;
                    r1 = default;
                    #endregion

                    #region Span 2: Size
                    d = (float)System.Math.Sqrt(xy);
                    s = value / d;
                    i = 1f - s;

                    x = s * abX / 2f;
                    y = s * abY / 2f;
                    #endregion
                    break;
                case 2:
                    // Center
                    cx = point1.X;
                    cy = point1.Y;
                    c = point1;

                    // Distance
                    abX = point1.X - point0.X;
                    abY = point1.Y - point0.Y;
                    xy = abX * abX + abY * abY;

                    #region Span 1: Rotate
                    r0 = default;
                    r1 = default;
                    #endregion

                    #region Span 2: Size
                    d = (float)System.Math.Sqrt(xy);
                    s = value / d;
                    i = 1 - s;

                    x = s * abX;
                    y = s * abY;
                    #endregion
                    break;
                default:
                    cx = 0f;
                    cy = 0f;
                    c = default;

                    abX = 0f;
                    abY = 0f;
                    xy = 0f;

                    #region Span 1: Rotate
                    r0 = default;
                    r1 = default;
                    #endregion

                    #region Span 2: Size
                    d = 0f;
                    s = 0f;
                    i = 0f;

                    x = 0f;
                    y = 0f;
                    #endregion
                    break;
            }
        }

        // Rotate
        public Matrix3x2 Rotate(ControllerRadians rotationAngle)
        {
            return Matrix3x2.CreateRotation(rotationAngle.Radians, c);
        }

        public ControllerRadians ToRadians(Vector2 point, float stepFrequency = float.NaN)
        {
            return new ControllerRadians(this, point, stepFrequency);
        }

        // Size
        public Vector2 GetPoint1()
        {
            return new Vector2(c.X + x, c.Y + y);
        }

        public Vector2 GetPoint0()
        {
            return new Vector2(c.X - x, c.Y - y);
        }

        public Matrix3x2 Multiply()
        {
            Matrix3x2 m;

            // First row
            m.M11 = s;
            m.M12 = 0f;

            // Second row
            m.M21 = 0f;
            m.M22 = s;

            // Third row
            m.M31 = c.X * i;
            m.M32 = c.Y * i;

            return m;
        }
    }
}