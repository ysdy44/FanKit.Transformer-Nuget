using System;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer
{
    partial class Constants
    {
        // Static
        internal static Vector2 QuadraticPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t, float i)
        {
            float i2 = i * i;
            float t2 = t * t;
            float it2 = 2f * i * t;

            return new Vector2
            {
                X = t2 * p2.X +
                     it2 * p1.X +
                     i2 * p0.X,
                Y = t2 * p2.Y +
                     it2 * p1.Y +
                     i2 * p0.Y,
            };
        }

        internal static List<float> QuadraticExtrema(float p0, float p1, float p2)
        {
            List<float> tValues = new List<float>();

            float a = p0 - 2 * p1 + p2;
            float b = p1 - p0;

            if (Math.Abs(a) > float.Epsilon)
            {
                float t = -b / a;
                if (t >= 0 && t <= 1)
                    tValues.Add(t);
            }

            return tValues;
        }

        #region GaussLegendre_Order10
        const float weight0o10 = 0.06667134f;
        const float weight1o10 = 0.1494513f;
        const float weight2o10 = 0.2190864f;
        const float weight3o10 = 0.2692667f;
        const float weight4o10 = 0.2955242f;
        const float weight5o10 = 0.2955242f;
        const float weight6o10 = 0.2692667f;
        const float weight7o10 = 0.2190864f;
        const float weight8o10 = 0.1494513f;
        const float weight9o10 = 0.06667134f;

        const float a0o10 = -1.973907f;
        const float a1o10 = -1.865063f;
        const float a2o10 = -1.67941f;
        const float a3o10 = -1.433395f;
        const float a4o10 = -1.148874f;
        const float a5o10 = -0.8511257f;
        const float a6o10 = -0.5666046f;
        const float a7o10 = -0.3205905f;
        const float a8o10 = -0.1349366f;
        const float a9o10 = -0.02609348f;

        const float b0o10 = 1.947813f;
        const float b1o10 = 1.730127f;
        const float b2o10 = 1.358819f;
        const float b3o10 = 0.8667908f;
        const float b4o10 = 0.2977487f;
        const float b5o10 = -0.2977486f;
        const float b6o10 = -0.8667908f;
        const float b7o10 = -1.358819f;
        const float b8o10 = -1.730127f;
        const float b9o10 = -1.947813f;

        const float c0o10 = 0.02609348f;
        const float c1o10 = 0.1349366f;
        const float c2o10 = 0.3205904f;
        const float c3o10 = 0.5666046f;
        const float c4o10 = 0.8511257f;
        const float c5o10 = 1.148874f;
        const float c6o10 = 1.433395f;
        const float c7o10 = 1.67941f;
        const float c8o10 = 1.865063f;
        const float c9o10 = 1.973907f;

        internal static float QuadraticLengthOrder10(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float x0o10 = p0.X * a0o10 + p1.X * b0o10 + p2.X * c0o10;
            float x1o10 = p0.X * a1o10 + p1.X * b1o10 + p2.X * c1o10;
            float x2o10 = p0.X * a2o10 + p1.X * b2o10 + p2.X * c2o10;
            float x3o10 = p0.X * a3o10 + p1.X * b3o10 + p2.X * c3o10;
            float x4o10 = p0.X * a4o10 + p1.X * b4o10 + p2.X * c4o10;
            float x5o10 = p0.X * a5o10 + p1.X * b5o10 + p2.X * c5o10;
            float x6o10 = p0.X * a6o10 + p1.X * b6o10 + p2.X * c6o10;
            float x7o10 = p0.X * a7o10 + p1.X * b7o10 + p2.X * c7o10;
            float x8o10 = p0.X * a8o10 + p1.X * b8o10 + p2.X * c8o10;
            float x9o10 = p0.X * a9o10 + p1.X * b9o10 + p2.X * c9o10;

            float y0o10 = p0.Y * a0o10 + p1.Y * b0o10 + p2.Y * c0o10;
            float y1o10 = p0.Y * a1o10 + p1.Y * b1o10 + p2.Y * c1o10;
            float y2o10 = p0.Y * a2o10 + p1.Y * b2o10 + p2.Y * c2o10;
            float y3o10 = p0.Y * a3o10 + p1.Y * b3o10 + p2.Y * c3o10;
            float y4o10 = p0.Y * a4o10 + p1.Y * b4o10 + p2.Y * c4o10;
            float y5o10 = p0.Y * a5o10 + p1.Y * b5o10 + p2.Y * c5o10;
            float y6o10 = p0.Y * a6o10 + p1.Y * b6o10 + p2.Y * c6o10;
            float y7o10 = p0.Y * a7o10 + p1.Y * b7o10 + p2.Y * c7o10;
            float y8o10 = p0.Y * a8o10 + p1.Y * b8o10 + p2.Y * c8o10;
            float y9o10 = p0.Y * a9o10 + p1.Y * b9o10 + p2.Y * c9o10;

            Vector2 derivative0o10 = new Vector2(x0o10, y0o10);
            Vector2 derivative1o10 = new Vector2(x1o10, y1o10);
            Vector2 derivative2o10 = new Vector2(x2o10, y2o10);
            Vector2 derivative3o10 = new Vector2(x3o10, y3o10);
            Vector2 derivative4o10 = new Vector2(x4o10, y4o10);
            Vector2 derivative5o10 = new Vector2(x5o10, y5o10);
            Vector2 derivative6o10 = new Vector2(x6o10, y6o10);
            Vector2 derivative7o10 = new Vector2(x7o10, y7o10);
            Vector2 derivative8o10 = new Vector2(x8o10, y8o10);
            Vector2 derivative9o10 = new Vector2(x9o10, y9o10);

            float speed0o10 = derivative0o10.Length();
            float speed1o10 = derivative1o10.Length();
            float speed2o10 = derivative2o10.Length();
            float speed3o10 = derivative3o10.Length();
            float speed4o10 = derivative4o10.Length();
            float speed5o10 = derivative5o10.Length();
            float speed6o10 = derivative6o10.Length();
            float speed7o10 = derivative7o10.Length();
            float speed8o10 = derivative8o10.Length();
            float speed9o10 = derivative9o10.Length();

            return (weight0o10 * speed0o10
                + weight1o10 * speed1o10
                + weight2o10 * speed2o10
                + weight3o10 * speed3o10
                + weight4o10 * speed4o10
                + weight5o10 * speed5o10
                + weight6o10 * speed6o10
                + weight7o10 * speed7o10
                + weight8o10 * speed8o10
                + weight9o10 * speed9o10) * 0.5f;
        }
        #endregion

        #region GaussLegendre_Order20
        const float weight0o20 = 0.01761401f;
        const float weight1o20 = 0.04060143f;
        const float weight2o20 = 0.06267205f;
        const float weight3o20 = 0.08327674f;
        const float weight4o20 = 0.1019301f;
        const float weight5o20 = 0.1181945f;
        const float weight6o20 = 0.1316886f;
        const float weight7o20 = 0.1420961f;
        const float weight8o20 = 0.149173f;
        const float weight9o20 = 0.1527534f;
        const float weight10o20 = 0.1527534f;
        const float weight11o20 = 0.149173f;
        const float weight12o20 = 0.1420961f;
        const float weight13o20 = 0.1316886f;
        const float weight14o20 = 0.1181945f;
        const float weight15o20 = 0.1019301f;
        const float weight16o20 = 0.08327674f;
        const float weight17o20 = 0.06267205f;
        const float weight18o20 = 0.04060143f;
        const float weight19o20 = 0.01761401f;

        const float a0o20 = -1.993129f;
        const float a1o20 = -1.963972f;
        const float a2o20 = -1.912234f;
        const float a3o20 = -1.839117f;
        const float a4o20 = -1.746332f;
        const float a5o20 = -1.636054f;
        const float a6o20 = -1.510867f;
        const float a7o20 = -1.373706f;
        const float a8o20 = -1.227786f;
        const float a9o20 = -1.076527f;
        const float a10o20 = -0.9234735f;
        const float a11o20 = -0.7722142f;
        const float a12o20 = -0.6262939f;
        const float a13o20 = -0.489133f;
        const float a14o20 = -0.3639463f;
        const float a15o20 = -0.2536681f;
        const float a16o20 = -0.1608829f;
        const float a17o20 = -0.08776557f;
        const float a18o20 = -0.03602815f;
        const float a19o20 = -0.006871462f;

        const float b0o20 = 1.986257f;
        const float b1o20 = 1.927944f;
        const float b2o20 = 1.824469f;
        const float b3o20 = 1.678234f;
        const float b4o20 = 1.492664f;
        const float b5o20 = 1.272107f;
        const float b6o20 = 1.021734f;
        const float b7o20 = 0.7474122f;
        const float b8o20 = 0.4555717f;
        const float b9o20 = 0.153053f;
        const float b10o20 = -0.153053f;
        const float b11o20 = -0.4555717f;
        const float b12o20 = -0.7474122f;
        const float b13o20 = -1.021734f;
        const float b14o20 = -1.272107f;
        const float b15o20 = -1.492664f;
        const float b16o20 = -1.678234f;
        const float b17o20 = -1.824469f;
        const float b18o20 = -1.927944f;
        const float b19o20 = -1.986257f;

        const float c0o20 = 0.006871402f;
        const float c1o20 = 0.03602809f;
        const float c2o20 = 0.08776557f;
        const float c3o20 = 0.160883f;
        const float c4o20 = 0.2536681f;
        const float c5o20 = 0.3639463f;
        const float c6o20 = 0.489133f;
        const float c7o20 = 0.6262939f;
        const float c8o20 = 0.7722142f;
        const float c9o20 = 0.9234735f;
        const float c10o20 = 1.076527f;
        const float c11o20 = 1.227786f;
        const float c12o20 = 1.373706f;
        const float c13o20 = 1.510867f;
        const float c14o20 = 1.636054f;
        const float c15o20 = 1.746332f;
        const float c16o20 = 1.839117f;
        const float c17o20 = 1.912234f;
        const float c18o20 = 1.963972f;
        const float c19o20 = 1.993129f;

        internal static float QuadraticLengthOrder20(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float x0o20 = p0.X * a0o20 + p1.X * b0o20 + p2.X * c0o20;
            float x1o20 = p0.X * a1o20 + p1.X * b1o20 + p2.X * c1o20;
            float x2o20 = p0.X * a2o20 + p1.X * b2o20 + p2.X * c2o20;
            float x3o20 = p0.X * a3o20 + p1.X * b3o20 + p2.X * c3o20;
            float x4o20 = p0.X * a4o20 + p1.X * b4o20 + p2.X * c4o20;
            float x5o20 = p0.X * a5o20 + p1.X * b5o20 + p2.X * c5o20;
            float x6o20 = p0.X * a6o20 + p1.X * b6o20 + p2.X * c6o20;
            float x7o20 = p0.X * a7o20 + p1.X * b7o20 + p2.X * c7o20;
            float x8o20 = p0.X * a8o20 + p1.X * b8o20 + p2.X * c8o20;
            float x9o20 = p0.X * a9o20 + p1.X * b9o20 + p2.X * c9o20;
            float x10o20 = p0.X * a10o20 + p1.X * b10o20 + p2.X * c10o20;
            float x11o20 = p0.X * a11o20 + p1.X * b11o20 + p2.X * c11o20;
            float x12o20 = p0.X * a12o20 + p1.X * b12o20 + p2.X * c12o20;
            float x13o20 = p0.X * a13o20 + p1.X * b13o20 + p2.X * c13o20;
            float x14o20 = p0.X * a14o20 + p1.X * b14o20 + p2.X * c14o20;
            float x15o20 = p0.X * a15o20 + p1.X * b15o20 + p2.X * c15o20;
            float x16o20 = p0.X * a16o20 + p1.X * b16o20 + p2.X * c16o20;
            float x17o20 = p0.X * a17o20 + p1.X * b17o20 + p2.X * c17o20;
            float x18o20 = p0.X * a18o20 + p1.X * b18o20 + p2.X * c18o20;
            float x19o20 = p0.X * a19o20 + p1.X * b19o20 + p2.X * c19o20;

            float y0o20 = p0.Y * a0o20 + p1.Y * b0o20 + p2.Y * c0o20;
            float y1o20 = p0.Y * a1o20 + p1.Y * b1o20 + p2.Y * c1o20;
            float y2o20 = p0.Y * a2o20 + p1.Y * b2o20 + p2.Y * c2o20;
            float y3o20 = p0.Y * a3o20 + p1.Y * b3o20 + p2.Y * c3o20;
            float y4o20 = p0.Y * a4o20 + p1.Y * b4o20 + p2.Y * c4o20;
            float y5o20 = p0.Y * a5o20 + p1.Y * b5o20 + p2.Y * c5o20;
            float y6o20 = p0.Y * a6o20 + p1.Y * b6o20 + p2.Y * c6o20;
            float y7o20 = p0.Y * a7o20 + p1.Y * b7o20 + p2.Y * c7o20;
            float y8o20 = p0.Y * a8o20 + p1.Y * b8o20 + p2.Y * c8o20;
            float y9o20 = p0.Y * a9o20 + p1.Y * b9o20 + p2.Y * c9o20;
            float y10o20 = p0.Y * a10o20 + p1.Y * b10o20 + p2.Y * c10o20;
            float y11o20 = p0.Y * a11o20 + p1.Y * b11o20 + p2.Y * c11o20;
            float y12o20 = p0.Y * a12o20 + p1.Y * b12o20 + p2.Y * c12o20;
            float y13o20 = p0.Y * a13o20 + p1.Y * b13o20 + p2.Y * c13o20;
            float y14o20 = p0.Y * a14o20 + p1.Y * b14o20 + p2.Y * c14o20;
            float y15o20 = p0.Y * a15o20 + p1.Y * b15o20 + p2.Y * c15o20;
            float y16o20 = p0.Y * a16o20 + p1.Y * b16o20 + p2.Y * c16o20;
            float y17o20 = p0.Y * a17o20 + p1.Y * b17o20 + p2.Y * c17o20;
            float y18o20 = p0.Y * a18o20 + p1.Y * b18o20 + p2.Y * c18o20;
            float y19o20 = p0.Y * a19o20 + p1.Y * b19o20 + p2.Y * c19o20;

            Vector2 derivative0o20 = new Vector2(x0o20, y0o20);
            Vector2 derivative1o20 = new Vector2(x1o20, y1o20);
            Vector2 derivative2o20 = new Vector2(x2o20, y2o20);
            Vector2 derivative3o20 = new Vector2(x3o20, y3o20);
            Vector2 derivative4o20 = new Vector2(x4o20, y4o20);
            Vector2 derivative5o20 = new Vector2(x5o20, y5o20);
            Vector2 derivative6o20 = new Vector2(x6o20, y6o20);
            Vector2 derivative7o20 = new Vector2(x7o20, y7o20);
            Vector2 derivative8o20 = new Vector2(x8o20, y8o20);
            Vector2 derivative9o20 = new Vector2(x9o20, y9o20);
            Vector2 derivative10o20 = new Vector2(x10o20, y10o20);
            Vector2 derivative11o20 = new Vector2(x11o20, y11o20);
            Vector2 derivative12o20 = new Vector2(x12o20, y12o20);
            Vector2 derivative13o20 = new Vector2(x13o20, y13o20);
            Vector2 derivative14o20 = new Vector2(x14o20, y14o20);
            Vector2 derivative15o20 = new Vector2(x15o20, y15o20);
            Vector2 derivative16o20 = new Vector2(x16o20, y16o20);
            Vector2 derivative17o20 = new Vector2(x17o20, y17o20);
            Vector2 derivative18o20 = new Vector2(x18o20, y18o20);
            Vector2 derivative19o20 = new Vector2(x19o20, y19o20);

            float speed0o20 = derivative0o20.Length();
            float speed1o20 = derivative1o20.Length();
            float speed2o20 = derivative2o20.Length();
            float speed3o20 = derivative3o20.Length();
            float speed4o20 = derivative4o20.Length();
            float speed5o20 = derivative5o20.Length();
            float speed6o20 = derivative6o20.Length();
            float speed7o20 = derivative7o20.Length();
            float speed8o20 = derivative8o20.Length();
            float speed9o20 = derivative9o20.Length();
            float speed10o20 = derivative10o20.Length();
            float speed11o20 = derivative11o20.Length();
            float speed12o20 = derivative12o20.Length();
            float speed13o20 = derivative13o20.Length();
            float speed14o20 = derivative14o20.Length();
            float speed15o20 = derivative15o20.Length();
            float speed16o20 = derivative16o20.Length();
            float speed17o20 = derivative17o20.Length();
            float speed18o20 = derivative18o20.Length();
            float speed19o20 = derivative19o20.Length();

            return (weight0o20 * speed0o20
                + weight1o20 * speed1o20
                + weight2o20 * speed2o20
                + weight3o20 * speed3o20
                + weight4o20 * speed4o20
                + weight5o20 * speed5o20
                + weight6o20 * speed6o20
                + weight7o20 * speed7o20
                + weight8o20 * speed8o20
                + weight9o20 * speed9o20
                + weight10o20 * speed10o20
                + weight11o20 * speed11o20
                + weight12o20 * speed12o20
                + weight13o20 * speed13o20
                + weight14o20 * speed14o20
                + weight15o20 * speed15o20
                + weight16o20 * speed16o20
                + weight17o20 * speed17o20
                + weight18o20 * speed18o20
                + weight19o20 * speed19o20) * 0.5f;
        }
        #endregion
    }
}