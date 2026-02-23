using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    public readonly struct PinchMatrix3x2
    {
        public readonly bool IsEmpty;

        readonly float dx;
        readonly float dy;
        internal readonly float r; // Radians

        readonly float ls; // Length Squared
        readonly float d; // Distance (Scale of Normalize)
        internal readonly float a; // Angle

        // First row
        // Second row
        readonly Rotation2x2 r2;
        internal readonly float c; // Cos / Ratio
        internal readonly float s; // Sin

        // Third row
        internal readonly float x;
        internal readonly float y;

        #region Constructors
        // ElongatePoint
        public PinchMatrix3x2(LineMatrix src, Vector2 sp0, Vector2 p1)
        {
            if (p1.X == sp0.X && p1.Y == sp0.Y)
            {
                IsEmpty = true;

                dx = 0f;
                dy = 0f;
                r = 0f;

                ls = 0f;
                d = 0f;
                a = -src.r;

                r2 = default;
                c = 1f;
                s = 0f;

                x = 0f;
                y = 0f;
            }
            else
            {
                IsEmpty = false;

                dx = p1.X - sp0.X;
                dy = p1.Y - sp0.Y;
                ls = dy * src.dy + dx * src.dx;

                r = 0f;
                d = 0f;
                a = -src.r;

                r2 = default;
                c = ls / src.ls;
                s = 0f;

                x = sp0.X - sp0.X * c;
                y = sp0.Y - sp0.Y * c;
            }
        }

        // MovePoint
        public PinchMatrix3x2(LineMatrix src, Vector2 sp0, Vector2 p0, Vector2 p1)
        {
            if (p1.X == p0.X && p1.Y == p0.Y)
            {
                IsEmpty = true;

                dx = 0f;
                dy = 0f;
                r = 0f;

                ls = 0f;
                d = 0f;
                a = -src.r;

                r2 = default;
                c = 1f;
                s = 0f;

                x = 0f;
                y = 0f;
            }
            else
            {
                IsEmpty = false;

                dx = p1.X - p0.X;
                dy = p1.Y - p0.Y;
                ls = dx * dx + dy * dy;

                r = (float)System.Math.Atan2(dy, dx);
                d = (float)System.Math.Sqrt(ls);
                a = r - src.r;

                r2 = new Rotation2x2(a);
                c = src.i * r2.C * d;
                s = src.i * r2.S * d;

                x = p0.X - sp0.X * c + sp0.Y * s;
                y = p0.Y - sp0.X * s - sp0.Y * c;
            }
        }
        #endregion Constructors

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public operator methods
        #endregion Public operator methods

        #region Public Static Operators
        public static implicit operator Matrix3x2(PinchMatrix3x2 matrix)
        {
            return new Matrix3x2
            {
                // First row
                M11 = matrix.c,
                M12 = matrix.s,

                // Second row
                M21 = -matrix.s,
                M22 = matrix.c,

                // Third row
                M31 = matrix.x,
                M32 = matrix.y
            };
        }
        #endregion Public Static Operators
    }
}