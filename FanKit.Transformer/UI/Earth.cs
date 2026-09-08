using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public partial class Graticule
    {
        internal const float PolarEpsilon = 0.36787945f; // 1f / (float)System.Math.E;
        const float PolarEpsilonRadians = Mathematics.Math.PITwice * PolarEpsilon;

        internal readonly GraticuleUV UV;

        // Textures
        internal readonly EarthTextureIsFarSide[,] QuadIsFarSides;
        internal readonly Quadrilateral[,] Quads;

        // Vectors
        readonly Vector3 NorthVector = new Vector3(0f, -1f, 0f);
        readonly Vector3 SouthVector = new Vector3(0f, 1f, 0f);
        readonly Vector3[,] Vectors;

        // Vector IsFarSide
        bool NorthVectorIsFarSide;
        bool SouthVectorIsFarSide;
        internal readonly bool[,] VectorIsFarSides;

        // Vector Rotated
        Vector3 NorthVectorRotated = new Vector3(0f, -1f, 0f);
        Vector3 SouthVectorRotated = new Vector3(0f, 1f, 0f);
        readonly Vector3[,] VectorsRotated;

        // Vertexes
        Vector2 NorthVertex;
        Vector2 SouthVertex;
        internal readonly Vector2[,] Vertexes;

        readonly Vector2[] NorthPoleVertexes;
        readonly Vector2[] SouthPoleVertexes;

        public EarthTextureIsFarSide[,] TextureIsFarSides => this.QuadIsFarSides;
        public Quadrilateral[,] TextureOutlines => this.Quads;

        public bool NorthPoleIsFarSide => this.NorthVectorIsFarSide;
        public bool SouthPoleIsFarSide => this.SouthVectorIsFarSide;

        public Vector2[] NorthPolePolygon => this.NorthPoleVertexes;
        public Vector2[] SouthPolePolygon => this.SouthPoleVertexes;

        public Graticule(GraticuleUV uv)
        {
            this.UV = uv;

            // Textures
            this.QuadIsFarSides = new EarthTextureIsFarSide[uv.VCountPlus, uv.UCount];
            this.Quads = new Quadrilateral[uv.VCountPlus, uv.UCount];

            // Vectors
            //readonly Vector3 NorthVector = new Vector3(0f, -1f, 0f);
            //readonly Vector3 SouthVector = new Vector3(0f, 1f, 0f);
            this.Vectors = new Vector3[uv.VCountPlus, uv.UCount];

            // Vector IsFarSide
            //bool NorthVectorIsFarSide;
            //bool SouthVectorIsFarSide;
            this.VectorIsFarSides = new bool[uv.VCountPlus, uv.UCount];

            // Vector Rotated
            //Vector3 NorthVectorRotated = new Vector3(0f, -1f, 0f);
            //Vector3 SouthVectorRotated = new Vector3(0f, 1f, 0f);
            this.VectorsRotated = new Vector3[uv.VCountPlus, uv.UCount];

            // Vertexes
            //Vector2 NorthVertex;
            //Vector2 SouthVertex;
            this.Vertexes = new Vector2[uv.VCountPlus, uv.UCount];

            this.NorthPoleVertexes = new Vector2[uv.UCount];
            this.SouthPoleVertexes = new Vector2[uv.UCount];

            float radians = PolarEpsilonRadians / uv.VCountTwiceF;
            Rotation2x2 vRadians0 = new Rotation2x2(Mathematics.Math.PIOver2 + radians);
            Rotation2x2 vRadians1 = new Rotation2x2(Mathematics.Math.PI + Mathematics.Math.PIOver2 - radians);

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                float uScale = ui / uv.UCountHalfF;

                Rotation2x2 uRadians = new Rotation2x2(Mathematics.Math.PI + Mathematics.Math.PI * uScale);
                float uSin = uRadians.S;
                float uCos = uRadians.C;

                for (int vi = 1; vi < uv.VCount; vi++)
                {
                    float vScale = vi / uv.VCountTwiceF;

                    Rotation2x2 vRadians = new Rotation2x2(Mathematics.Math.PIOver2 + Mathematics.Math.PITwice * vScale);
                    float vSin = vRadians.S;
                    float vCos = vRadians.C;

                    this.Vectors[vi, ui] = new Vector3
                    {
                        Z = vCos * uCos,
                        X = vCos * uSin,
                        Y = -vSin,
                    };
                }

                {
                    const int vi = 0;

                    Rotation2x2 vRadians = vRadians0;
                    float vSin = vRadians.S;
                    float vCos = vRadians.C;

                    this.Vectors[vi, ui] = new Vector3
                    {
                        Z = vCos * uCos,
                        X = vCos * uSin,
                        Y = -vSin,
                    };
                }

                {
                    int vi = uv.VCount;

                    Rotation2x2 vRadians = vRadians1;
                    float vSin = vRadians.S;
                    float vCos = vRadians.C;

                    this.Vectors[vi, ui] = new Vector3
                    {
                        Z = vCos * uCos,
                        X = vCos * uSin,
                        Y = -vSin,
                    };
                }
            }
        }
    }

    partial class Earth {
        public IEnumerable<EarthTextureIndex> DrawTextures()
        {
            for (int vi = 0; vi < this.UV.VCountPlus; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.ThreeCorners)
                    {
                        yield return new EarthTextureIndex
                        {
                            U = ui,
                            V = vi,
                        };
                    }
                }
            }

            for (int vi = 0; vi < this.UV.VCountPlus; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.TwoCorners)
                    {
                        yield return new EarthTextureIndex
                        {
                            U = ui,
                            V = vi,
                        };
                    }
                }
            }

            for (int vi = 0; vi < this.UV.VCountPlus; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.OneCorner)
                    {
                        yield return new EarthTextureIndex
                        {
                            U = ui,
                            V = vi,
                        };
                    }
                }
            }

            for (int vi = 0; vi < this.UV.VCountPlus; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.ZeroCorner)
                    {
                        yield return new EarthTextureIndex
                        {
                            U = ui,
                            V = vi,
                        };
                    }
                }
            }
        }
    }

    partial class Graticule {
        public IEnumerable<GraticuleLine> DrawLines()
        {
            for (int vi = 1; vi < this.UV.VCount + 1; vi++)
            {
                int vi1 = vi - 1;
                int vi2 = vi;

                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;

                    bool f3 = this.VectorIsFarSides[vi2, ui2];
                    if (!f3)
                    {
                        int ui1 = ui;

                        bool f4 = this.VectorIsFarSides[vi2, ui1];

                        bool f2 = this.VectorIsFarSides[vi1, ui2];
                        if (!f2)
                        {
                            Vector2 p2 = this.Vertexes[vi1, ui2];
                            Vector2 p3 = this.Vertexes[vi2, ui2];
                            yield return new GraticuleLine
                            {
                                Point0 = p2,
                                Point1 = p3,
                            };

                            if (!f4)
                            {
                                Vector2 p4 = this.Vertexes[vi2, ui1];
                                yield return new GraticuleLine
                                {
                                    Point0 = p4,
                                    Point1 = p3,
                                };
                            }
                        }
                        else if (!f4)
                        {
                            Vector2 p3 = this.Vertexes[vi2, ui2];
                            Vector2 p4 = this.Vertexes[vi2, ui1];
                            yield return new GraticuleLine
                            {
                                Point0 = p4,
                                Point1 = p3,
                            };
                        }
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                const int vi2 = 0;

                int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;

                bool f3 = this.VectorIsFarSides[vi2, ui2];
                if (!f3)
                {
                    int ui1 = ui;

                    bool f4 = this.VectorIsFarSides[vi2, ui1];

                    bool f2 = this.NorthVectorIsFarSide;
                    if (!f2)
                    {
                        Vector2 p2 = this.NorthVertex;
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        yield return new GraticuleLine
                        {
                            Point0 = p2,
                            Point1 = p3,
                        };

                        if (!f4)
                        {
                            Vector2 p4 = this.Vertexes[vi2, ui1];
                            yield return new GraticuleLine
                            {
                                Point0 = p4,
                                Point1 = p3,
                            };
                        }
                    }
                    else if (!f4)
                    {
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        Vector2 p4 = this.Vertexes[vi2, ui1];
                        yield return new GraticuleLine
                        {
                            Point0 = p4,
                            Point1 = p3,
                        };
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                int vi1 = this.UV.VCountMinus;

                int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;

                bool f2 = this.VectorIsFarSides[vi1, ui2];
                if (!f2)
                {
                    int ui1 = ui;

                    bool f3 = this.SouthVectorIsFarSide;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    if (!f1)
                    {
                        Vector2 p1 = this.Vertexes[vi1, ui1];
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        yield return new GraticuleLine
                        {
                            Point0 = p1,
                            Point1 = p2,
                        };

                        if (!f3)
                        {
                            Vector2 p3 = this.SouthVertex;
                            yield return new GraticuleLine
                            {
                                Point0 = p3,
                                Point1 = p2,
                            };
                        }
                    }
                    else if (!f3)
                    {
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        Vector2 p3 = this.SouthVertex;
                        yield return new GraticuleLine
                        {
                            Point0 = p3,
                            Point1 = p2,
                        };
                    }
                }
            }
        }

        public IEnumerable<Vector2> DrawVertexes()
        {
            for (int vi = 1; vi < this.UV.VCount; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    bool f = this.VectorIsFarSides[vi, ui];
                    if (f) continue;

                    Vector2 p = this.Vertexes[vi, ui];

                    yield return p;
                }
            }

            if (!this.NorthVectorIsFarSide)
            {
                //e.DrawingSession.FillCircle(this.NorthVertex, PolarEpsilonRadius * this.Radius, Colors.Red);
                yield return this.NorthVertex;
            }

            if (!this.SouthVectorIsFarSide)
            {
                //e.DrawingSession.FillCircle(this.SouthVertex, PolarEpsilonRadius * this.Radius, Colors.GreenYellow);
                yield return this.SouthVertex;
            }
        }
        public void Update(SphereLayout layout, SphereRotation rotation)
        {
            this.Update1(layout, rotation);
            this.Update4();
        }
        public void Update(SphereLayout layout)
        {
            this.Update2(layout);
            this.Update4();
        }

        internal void Update1(SphereLayout layout, SphereRotation rotation)
        {
            for (int vi = 0; vi < this.UV.VCountPlus; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    Vector3 e = this.Vectors[vi, ui];
                    Vector3 t = rotation.RotateUnitVector(e);

                    this.VectorIsFarSides[vi, ui] = t.Z < 0f;
                    this.VectorsRotated[vi, ui] = t;

                    if (vi == 0)
                        this.Vertexes[vi, ui] = this.NorthPoleVertexes[ui] = layout.GetPoint(t);
                    else if (vi == this.UV.VCount)
                        this.Vertexes[vi, ui] = this.SouthPoleVertexes[ui] = layout.GetPoint(t);
                    else
                        this.Vertexes[vi, ui] = layout.GetPoint(t);
                }
            }

            {
                Vector3 e = this.NorthVector;
                Vector3 t = rotation.RotateUnitVector(e);

                this.NorthVectorIsFarSide = t.Z < 0f;
                this.NorthVectorRotated = t;
                this.NorthVertex = layout.GetPoint(t);
            }

            {
                Vector3 e = this.SouthVector;
                Vector3 t = rotation.RotateUnitVector(e);

                this.SouthVectorIsFarSide = t.Z < 0f;
                this.SouthVectorRotated = t;
                this.SouthVertex = layout.GetPoint(t);
            }
        }

        internal void Update2(SphereLayout layout)
        {
            for (int vi = 0; vi < this.UV.VCountPlus; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    Vector3 t = this.VectorsRotated[vi, ui];

                    if (vi == 0)
                        this.Vertexes[vi, ui] = this.NorthPoleVertexes[ui] = layout.GetPoint(t);
                    else if (vi == this.UV.VCount)
                        this.Vertexes[vi, ui] = this.SouthPoleVertexes[ui] = layout.GetPoint(t);
                    else
                        this.Vertexes[vi, ui] = layout.GetPoint(t);
                }
            }

            {
                Vector3 t = this.NorthVectorRotated;
                this.NorthVertex = layout.GetPoint(t);
            }

            {
                Vector3 t = this.SouthVectorRotated;
                this.SouthVertex = layout.GetPoint(t);
            }
        }

        private void Update4()
        {
            for (int vi = 2; vi < this.UV.VCount; vi++)
            {
                int vi1 = vi - 1;
                int vi2 = vi;

                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    bool f2 = this.VectorIsFarSides[vi1, ui2];
                    bool f3 = this.VectorIsFarSides[vi2, ui2];
                    bool f4 = this.VectorIsFarSides[vi2, ui1];

                    EarthTextureIsFarSide f = f1 ?
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.FourCorners : EarthTextureIsFarSide.ThreeCorners : f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners :
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                            f3 ? f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner : f4 ? EarthTextureIsFarSide.OneCorner : EarthTextureIsFarSide.ZeroCorner;
                    this.QuadIsFarSides[vi1, ui] = f;

                    if (f != EarthTextureIsFarSide.FourCorners)
                    {
                        Vector2 p1 = this.Vertexes[vi1, ui1];
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        Vector2 p4 = this.Vertexes[vi2, ui1];

                        Quadrilateral quad = new Quadrilateral
                        {
                            LeftTop = p1,
                            RightTop = p2,
                            RightBottom = p3,
                            LeftBottom = p4,
                        };

                        this.Quads[vi1, ui] = quad;
                    }
                    else
                    {
                        this.Quads[vi1, ui] = Quadrilateral.Identity;
                    }
                }
            }

            {
                const int vi1 = 0;
                const int vi2 = 1;

                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    bool f2 = this.VectorIsFarSides[vi1, ui2];
                    bool f3 = this.VectorIsFarSides[vi2, ui2];
                    bool f4 = this.VectorIsFarSides[vi2, ui1];

                    EarthTextureIsFarSide f = f1 ?
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.FourCorners : EarthTextureIsFarSide.ThreeCorners : f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners :
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                            f3 ? f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner : f4 ? EarthTextureIsFarSide.OneCorner : EarthTextureIsFarSide.ZeroCorner;
                    this.QuadIsFarSides[vi1, ui] = f;

                    if (f != EarthTextureIsFarSide.FourCorners)
                    {
                        Vector2 p1 = this.Vertexes[vi1, ui1];
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        Vector2 p4 = this.Vertexes[vi2, ui1];

                        Quadrilateral quad = new Quadrilateral
                        {
                            LeftTop = p1,
                            RightTop = p2,
                            RightBottom = p3,
                            LeftBottom = p4,
                        };

                        this.Quads[vi1, ui] = quad;
                    }
                    else
                    {
                        this.Quads[vi1, ui] = Quadrilateral.Identity;
                    }
                }
            }

            {
                int vi1 = this.UV.VCountMinus;
                int vi2 = this.UV.VCount;

                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    bool f2 = this.VectorIsFarSides[vi1, ui2];
                    bool f3 = this.VectorIsFarSides[vi2, ui2];
                    bool f4 = this.VectorIsFarSides[vi2, ui1];

                    EarthTextureIsFarSide f = f1 ?
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.FourCorners : EarthTextureIsFarSide.ThreeCorners : f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners :
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                            f3 ? f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner : f4 ? EarthTextureIsFarSide.OneCorner : EarthTextureIsFarSide.ZeroCorner;
                    this.QuadIsFarSides[vi1, ui] = f;

                    if (f != EarthTextureIsFarSide.FourCorners)
                    {
                        Vector2 p1 = this.Vertexes[vi1, ui1];
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        Vector2 p4 = this.Vertexes[vi2, ui1];

                        Quadrilateral quad = new Quadrilateral
                        {
                            LeftTop = p1,
                            RightTop = p2,
                            RightBottom = p3,
                            LeftBottom = p4,
                        };

                        this.Quads[vi1, ui] = quad;
                    }
                    else
                    {
                        this.Quads[vi1, ui] = Quadrilateral.Identity;
                    }
                }
            }
        }

        public Vector2? GetAmount(SphereLayout layout, Vector2 point)
        {
            float ds = Vector2.DistanceSquared(point, layout.Center);
            if (ds > layout.Radius * layout.Radius)
                return null;

            const int vi0 = 0;
            int vi1 = this.UV.VCount - 1;

            #region ZeroCorner
            for (int vi = 1; vi < vi1; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.ZeroCorner)
                    {
                        Quadrilateral quad = this.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            float w = 1f / this.UV.UCountF;
                            float h = 1f / this.UV.VCount;

                            Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, h);

                            float y = vi * h;
                            float x = ui * w;

                            Vector2 offset = Mathematics.Math.Transform(point, m);

                            return new Vector2
                            {
                                X = x + offset.X,
                                Y = y + offset.Y,
                            };
                        }
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.ZeroCorner)
                {
                    Quadrilateral quad = this.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y0 = r;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y0 + offset.Y,
                        };
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.ZeroCorner)
                {
                    Quadrilateral quad = this.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y1 = vi1 * h;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y1 + offset.Y,
                        };
                    }
                }
            }
            #endregion

            #region OneCorner
            for (int vi = 1; vi < vi1; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.OneCorner)
                    {
                        Quadrilateral quad = this.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            float w = 1f / this.UV.UCountF;
                            float h = 1f / this.UV.VCount;

                            Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, h);

                            float y = vi * h;
                            float x = ui * w;

                            Vector2 offset = Mathematics.Math.Transform(point, m);

                            return new Vector2
                            {
                                X = x + offset.X,
                                Y = y + offset.Y,
                            };
                        }
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.OneCorner)
                {
                    Quadrilateral quad = this.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y0 = r;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y0 + offset.Y,
                        };
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.OneCorner)
                {
                    Quadrilateral quad = this.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y1 = vi1 * h;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y1 + offset.Y,
                        };
                    }
                }
            }
            #endregion

            #region TwoCorners
            for (int vi = 1; vi < vi1; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.TwoCorners)
                    {
                        Quadrilateral quad = this.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            float w = 1f / this.UV.UCountF;
                            float h = 1f / this.UV.VCount;

                            Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, h);

                            float y = vi * h;
                            float x = ui * w;

                            Vector2 offset = Mathematics.Math.Transform(point, m);

                            return new Vector2
                            {
                                X = x + offset.X,
                                Y = y + offset.Y,
                            };
                        }
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.TwoCorners)
                {
                    Quadrilateral quad = this.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y0 = r;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y0 + offset.Y,
                        };
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.TwoCorners)
                {
                    Quadrilateral quad = this.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y1 = vi1 * h;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y1 + offset.Y,
                        };
                    }
                }
            }
            #endregion

            #region ThreeCorners
            for (int vi = 1; vi < vi1; vi++)
            {
                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    if (this.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.ThreeCorners)
                    {
                        Quadrilateral quad = this.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            float w = 1f / this.UV.UCountF;
                            float h = 1f / this.UV.VCount;

                            Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, h);

                            float y = vi * h;
                            float x = ui * w;

                            Vector2 offset = Mathematics.Math.Transform(point, m);

                            return new Vector2
                            {
                                X = x + offset.X,
                                Y = y + offset.Y,
                            };
                        }
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.ThreeCorners)
                {
                    Quadrilateral quad = this.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y0 = r;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y0 + offset.Y,
                        };
                    }
                }
            }

            for (int ui = 0; ui < this.UV.UCount; ui++)
            {
                if (this.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.ThreeCorners)
                {
                    Quadrilateral quad = this.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        float w = 1f / this.UV.UCountF;
                        float h = 1f / this.UV.VCount;

                        float r = h * PolarEpsilon;
                        float hr = h - r;

                        Matrix4x4 m = new Mathematics.InvertiblePerspSizeMatrix3x3(quad, w, hr);

                        float y1 = vi1 * h;
                        float x = ui * w;

                        Vector2 offset = Mathematics.Math.Transform(point, m);

                        return new Vector2
                        {
                            X = x + offset.X,
                            Y = y1 + offset.Y,
                        };
                    }
                }
            }
            #endregion

            return null;
        }

        public static Vector3 GetUnitVector(float uAmount, float vAmount)
        {
            Rotation2x2 uRadians = new Rotation2x2(Mathematics.Math.PI + Mathematics.Math.PITwice * uAmount);
            float uSin = uRadians.S;
            float uCos = uRadians.C;

            Rotation2x2 vRadians = new Rotation2x2(Mathematics.Math.PIOver2 + Mathematics.Math.PI * vAmount);
            float vSin = vRadians.S;
            float vCos = vRadians.C;

            return new Vector3
            {
                Z = vCos * uCos,
                X = vCos * uSin,
                Y = -vSin,
            };
        }
    }

    public partial class Earth : Graticule
    {
        readonly Matrix4x4[,] TransformMatrixes;

        public Matrix4x4[,] TextureTransformMatrixes => this.TransformMatrixes;

        public Earth(GraticuleUV uv) : base(uv)
        {
            this.TransformMatrixes = new Matrix4x4[uv.VCountPlus, uv.UCount];
        }

        public EarthTextureSize ToTextureSize(float bitmapWidth, float bitmapHeight)
        {
            return new EarthTextureSize(this.UV, bitmapWidth, bitmapHeight);
        }

        public IEnumerable<EarthCreateTexture> CreateTextures(EarthTextureSize textureSize)
        {
            return textureSize.CreateTextures(this.UV);
        }

        public void Update(EarthTextureSize textureSize, SphereLayout layout, SphereRotation rotation)
        {
            this.Update1(layout, rotation);
            this.Update3(textureSize);
        }

        public void Update(EarthTextureSize textureSize, SphereLayout layout)
        {
            this.Update2(layout);
            this.Update3(textureSize);
        }

        private void Update3(EarthTextureSize textureSize)
        {
            for (int vi = 2; vi < this.UV.VCount; vi++)
            {
                int vi1 = vi - 1;
                int vi2 = vi;

                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    bool f2 = this.VectorIsFarSides[vi1, ui2];
                    bool f3 = this.VectorIsFarSides[vi2, ui2];
                    bool f4 = this.VectorIsFarSides[vi2, ui1];

                    EarthTextureIsFarSide f = f1 ?
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.FourCorners : EarthTextureIsFarSide.ThreeCorners : f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners :
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                            f3 ? f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner : f4 ? EarthTextureIsFarSide.OneCorner : EarthTextureIsFarSide.ZeroCorner;
                    this.QuadIsFarSides[vi1, ui] = f;

                    if (f != EarthTextureIsFarSide.FourCorners)
                    {
                        Vector2 p1 = this.Vertexes[vi1, ui1];
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        Vector2 p4 = this.Vertexes[vi2, ui1];

                        Quadrilateral quad = new Quadrilateral
                        {
                            LeftTop = p1,
                            RightTop = p2,
                            RightBottom = p3,
                            LeftBottom = p4,
                        };

                        Matrix4x4 transformMatrix = textureSize.SourceNormalize.ToPerspMatrix(quad);

                        this.Quads[vi1, ui] = quad;
                        this.TransformMatrixes[vi1, ui] = transformMatrix;
                    }
                    else
                    {
                        this.Quads[vi1, ui] = Quadrilateral.Identity;
                        this.TransformMatrixes[vi1, ui] = Matrix4x4.Identity;
                    }
                }
            }

            {
                const int vi1 = 0;
                const int vi2 = 1;

                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    bool f2 = this.VectorIsFarSides[vi1, ui2];
                    bool f3 = this.VectorIsFarSides[vi2, ui2];
                    bool f4 = this.VectorIsFarSides[vi2, ui1];

                    EarthTextureIsFarSide f = f1 ?
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.FourCorners : EarthTextureIsFarSide.ThreeCorners : f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners :
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                            f3 ? f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner : f4 ? EarthTextureIsFarSide.OneCorner : EarthTextureIsFarSide.ZeroCorner;
                    this.QuadIsFarSides[vi1, ui] = f;

                    if (f != EarthTextureIsFarSide.FourCorners)
                    {
                        Vector2 p1 = this.Vertexes[vi1, ui1];
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        Vector2 p4 = this.Vertexes[vi2, ui1];

                        Quadrilateral quad = new Quadrilateral
                        {
                            LeftTop = p1,
                            RightTop = p2,
                            RightBottom = p3,
                            LeftBottom = p4,
                        };

                        Matrix4x4 transformMatrix = textureSize.SourceNormalizePolarEpsilon.ToPerspMatrix(quad);

                        this.Quads[vi1, ui] = quad;
                        this.TransformMatrixes[vi1, ui] = transformMatrix;
                    }
                    else
                    {
                        this.Quads[vi1, ui] = Quadrilateral.Identity;
                        this.TransformMatrixes[vi1, ui] = Matrix4x4.Identity;
                    }
                }
            }

            {
                int vi1 = this.UV.VCountMinus;
                int vi2 = this.UV.VCount;

                for (int ui = 0; ui < this.UV.UCount; ui++)
                {
                    int ui2 = ui == this.UV.UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    bool f2 = this.VectorIsFarSides[vi1, ui2];
                    bool f3 = this.VectorIsFarSides[vi2, ui2];
                    bool f4 = this.VectorIsFarSides[vi2, ui1];

                    EarthTextureIsFarSide f = f1 ?
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.FourCorners : EarthTextureIsFarSide.ThreeCorners : f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners :
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                        f2 ?
                            f3 ? f4 ? EarthTextureIsFarSide.ThreeCorners : EarthTextureIsFarSide.TwoCorners : f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner :
                            f3 ? f4 ? EarthTextureIsFarSide.TwoCorners : EarthTextureIsFarSide.OneCorner : f4 ? EarthTextureIsFarSide.OneCorner : EarthTextureIsFarSide.ZeroCorner;
                    this.QuadIsFarSides[vi1, ui] = f;

                    if (f != EarthTextureIsFarSide.FourCorners)
                    {
                        Vector2 p1 = this.Vertexes[vi1, ui1];
                        Vector2 p2 = this.Vertexes[vi1, ui2];
                        Vector2 p3 = this.Vertexes[vi2, ui2];
                        Vector2 p4 = this.Vertexes[vi2, ui1];

                        Quadrilateral quad = new Quadrilateral
                        {
                            LeftTop = p1,
                            RightTop = p2,
                            RightBottom = p3,
                            LeftBottom = p4,
                        };

                        Matrix4x4 transformMatrix = textureSize.SourceNormalizePolarEpsilon.ToPerspMatrix(quad);

                        this.Quads[vi1, ui] = quad;
                        this.TransformMatrixes[vi1, ui] = transformMatrix;
                    }
                    else
                    {
                        this.Quads[vi1, ui] = Quadrilateral.Identity;
                        this.TransformMatrixes[vi1, ui] = Matrix4x4.Identity;
                    }
                }
            }
        }
    }
}