using System.Numerics;
using System.Runtime.CompilerServices;

namespace FanKit.Transformer.Mathematics
{
    // 2x2
    public readonly struct RectSource
    {
        public readonly Rectangle Rect;

        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float W;

        #region Constructors
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectSource(float x, float y, float width, float height)
        {
            Rect = new Rectangle(x, y, width, height);

            X = 1f / Rect.Width;
            Y = 1f / Rect.Height;
            Z = -Rect.X / Rect.Width;
            W = -Rect.Y / Rect.Height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectSource(Rectangle rect)
        {
            Rect = rect;

            X = 1f / Rect.Width;
            Y = 1f / Rect.Height;
            Z = -Rect.X / Rect.Width;
            W = -Rect.Y / Rect.Height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectSource(Bounds bounds)
        {
            Rect = new Rectangle(bounds);

            X = 1f / Rect.Width;
            Y = 1f / Rect.Height;
            Z = -Rect.X / Rect.Width;
            W = -Rect.Y / Rect.Height;
        }
        #endregion Constructors

        #region Public instance methods
        #endregion Public instance methods

        #region Public Static Methods
        #endregion Public Static Methods

        #region Public Static Operators
        #endregion Public Static Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PerspRectMatrix3x3 ToPerspMatrix(Quadrilateral quad) => new PerspRectMatrix3x3(this, quad);

        // -------------------- 2x2_2x2 -------------------- // 

        public Matrix2x2 Map(float destX, float destY, float destWidth, float destHeight) => new Matrix2x2
        {
            // First row
            ScaleX = X * destWidth,
            ScaleY = Y * destHeight,

            // Second row
            TranslateX = destX + destWidth * Z,
            TranslateY = destY + destHeight * W,
        };

        public Matrix2x2 Map(Matrix2x2 dest) => new Matrix2x2
        {
            // First row
            ScaleX = X * dest.ScaleX,
            ScaleY = Y * dest.ScaleY,

            // Second row
            TranslateX = dest.TranslateX + dest.ScaleX * Z,
            TranslateY = dest.TranslateY + dest.ScaleY * W,
        };

        // -------------------- 2x2_3x2 -------------------- // 

        public Matrix3x2 Affine(Matrix3x2 destNorm) => new Matrix3x2
        {
            // First row
            M11 = X * destNorm.M11,
            M12 = X * destNorm.M12,

            // Second row
            M21 = Y * destNorm.M21,
            M22 = Y * destNorm.M22,

            // Third row
            M31 = Z * destNorm.M11 + W * destNorm.M21 + destNorm.M31,
            M32 = Z * destNorm.M12 + W * destNorm.M22 + destNorm.M32
        };
    }
}