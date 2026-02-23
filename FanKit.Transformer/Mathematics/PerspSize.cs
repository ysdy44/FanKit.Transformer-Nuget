using System.Numerics;

namespace FanKit.Transformer.Mathematics
{
    public class PerspSize : PerspMatrix
    {
        // Normalize
        float m11;
        float m12;
        float m14;

        float m21;
        float m22;
        float m24;

        float m41;
        float m42;

        // Result
        public Matrix4x4 m;

        public void FindHomography(Quadrilateral src, float width, float height)
        {
            Normalize(src);

            m11 = x[0];
            m12 = x[3];
            m14 = x[6];

            m21 = x[1];
            m22 = x[4];
            m24 = x[7];

            m41 = x[2];
            m42 = x[5];

            m = new Matrix4x4
            {
                // First row
                M11 = m11 * width,
                M12 = m12 * height,
                M13 = 0f,
                M14 = m14,

                // Second row
                M21 = m21 * width,
                M22 = m22 * height,
                M23 = 0f,
                M24 = m24,

                // Third row
                M31 = 0f,
                M32 = 0f,
                M33 = 1f,
                M34 = 0f,

                // Fourth row
                M41 = m41 * width,
                M42 = m42 * height,
                M43 = 0f,
                M44 = 1f,
            };
        }
    }
}