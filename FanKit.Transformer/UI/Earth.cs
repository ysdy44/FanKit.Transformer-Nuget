using System.Collections.Generic;
using System.Numerics;
using static FanKit.Transformer.UI.EarthTextureSize;

namespace FanKit.Transformer.UI
{
    public class Earth
    {
        const float PolarEpsilon = 120f;
        const float PolarEpsilonRadius = 6f / PolarEpsilon;
        const float PolarEpsilonRadians = 4f / PolarEpsilon;

        // Textures
        readonly bool[,] QuadIsFarSides = new bool[VCountPlus, UCount];
        readonly Quadrilateral[,] Quads = new Quadrilateral[VCountPlus, UCount];
        readonly Matrix4x4[,] TransformMatrixes = new Matrix4x4[VCountPlus, UCount];

        // Vectors
        readonly Vector3 NorthVector = new Vector3(0f, -1f, 0f);
        readonly Vector3 SouthVector = new Vector3(0f, 1f, 0f);
        readonly Vector3[,] Vectors = new Vector3[VCountPlus, UCount];

        // Vector IsFarSide
        bool NorthVectorIsFarSide;
        bool SouthVectorIsFarSide;
        readonly bool[,] VectorIsFarSides = new bool[VCountPlus, UCount];

        // Vertexes
        Vector2 NorthVertex;
        Vector2 SouthVertex;
        readonly Vector2[,] Vertexes = new Vector2[VCountPlus, UCount];

        readonly Vector2[] NorthPoleVertexes = new Vector2[UCount];
        readonly Vector2[] SouthPoleVertexes = new Vector2[UCount];

        public bool[,] TextureIsFarSides => this.QuadIsFarSides;
        public Quadrilateral[,] TextureOutlines => this.Quads;
        public Matrix4x4[,] TextureTransformMatrixes => this.TransformMatrixes;

        public bool NorthPoleIsFarSide => this.NorthVectorIsFarSide;
        public bool SouthPoleIsFarSide => this.SouthVectorIsFarSide;

        public Vector2[] NorthPolePolygon => this.NorthPoleVertexes;
        public Vector2[] SouthPolePolygon => this.SouthPoleVertexes;

        public Earth()
        {
            for (int ui = 0; ui < UCount; ui++)
            {
                float uScale = ui / UCountHalfF;

                Rotation2x2 uRadians = new Rotation2x2(Mathematics.Math.PI + Mathematics.Math.PI * uScale);
                float uSin = uRadians.S;
                float uCos = uRadians.C;

                for (int vi = 1; vi < VCount; vi++)
                {
                    float vScale = ((float)(vi - 1) + 0.5f) / VCountMinusTwiceF;

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

                    Rotation2x2 vRadians = new Rotation2x2(Mathematics.Math.PIOver2 + PolarEpsilonRadians);
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
                    const int vi = VCount;

                    Rotation2x2 vRadians = new Rotation2x2(Mathematics.Math.PIOver2 + Mathematics.Math.PI - PolarEpsilonRadians);
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

        public IEnumerable<EarthIndex> DrawTextures()
        {
            for (int vi = 0; vi < VCountPlus; vi++)
            {
                for (int ui = 0; ui < UCount; ui++)
                {
                    if (!this.QuadIsFarSides[vi, ui])
                    {
                        yield return new EarthIndex
                        {
                            U = ui,
                            V = vi,
                        };
                    }
                }
            }
        }

        public IEnumerable<EarthDrawLine> DrawLines()
        {
            for (int vi = 2; vi < VCount + 1; vi++)
            {
                int vi1 = vi - 1;
                int vi2 = vi;

                for (int ui = 0; ui < UCount; ui++)
                {
                    int ui2 = ui == UCountMinus ? 0 : ui + 1;

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
                            yield return new EarthDrawLine
                            {
                                Point0 = p2,
                                Point1 = p3,
                            };

                            if (!f4)
                            {
                                Vector2 p4 = this.Vertexes[vi2, ui1];
                                yield return new EarthDrawLine
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
                            yield return new EarthDrawLine
                            {
                                Point0 = p4,
                                Point1 = p3,
                            };
                        }
                    }
                }
            }

            for (int ui = 0; ui < UCount; ui++)
            {
                const int vi = 1;
                const int vi2 = vi;

                int ui2 = ui == UCountMinus ? 0 : ui + 1;

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
                        yield return new EarthDrawLine
                        {
                            Point0 = p2,
                            Point1 = p3,
                        };

                        if (!f4)
                        {
                            Vector2 p4 = this.Vertexes[vi2, ui1];
                            yield return new EarthDrawLine
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
                        yield return new EarthDrawLine
                        {
                            Point0 = p4,
                            Point1 = p3,
                        };
                    }
                }
            }

            for (int ui = 0; ui < UCount; ui++)
            {
                const int vi = VCount;
                const int vi1 = vi - 1;

                int ui2 = ui == UCountMinus ? 0 : ui + 1;

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
                        yield return new EarthDrawLine
                        {
                            Point0 = p1,
                            Point1 = p2,
                        };

                        if (!f3)
                        {
                            Vector2 p3 = this.SouthVertex;
                            yield return new EarthDrawLine
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
                        yield return new EarthDrawLine
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
            for (int vi = 1; vi < VCount; vi++)
            {
                for (int ui = 0; ui < UCount; ui++)
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

        public void Update(EarthLayout layout, EarthTextureSize textureSize, EarthRotation rotation)
        {
            for (int vi = 0; vi < VCountPlus; vi++)
            {
                for (int ui = 0; ui < UCount; ui++)
                {
                    Vector3 e = this.Vectors[vi, ui];
                    Vector3 t = rotation.RotateUnitVector(e);

                    if (vi == 0)
                        this.Vertexes[vi, ui] = this.NorthPoleVertexes[ui] = layout.GetPoint(t);
                    else if (vi == VCount)
                        this.Vertexes[vi, ui] = this.SouthPoleVertexes[ui] = layout.GetPoint(t);
                    else
                        this.Vertexes[vi, ui] = layout.GetPoint(t);

                    this.VectorIsFarSides[vi, ui] = t.Z < 0f;
                }
            }

            {
                Vector3 e = this.NorthVector;
                Vector3 t = rotation.RotateUnitVector(e);

                this.NorthVertex = layout.GetPoint(t);

                this.NorthVectorIsFarSide = t.Z < 0f;
            }

            {
                Vector3 e = this.SouthVector;
                Vector3 t = rotation.RotateUnitVector(e);

                this.SouthVertex = layout.GetPoint(t);

                this.SouthVectorIsFarSide = t.Z < 0f;
            }

            for (int vi = 2; vi < VCount; vi++)
            {
                int vi1 = vi - 1;
                int vi2 = vi;

                for (int ui = 0; ui < UCount; ui++)
                {
                    int ui2 = ui == UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    if (!f1)
                    {
                        bool f2 = this.VectorIsFarSides[vi1, ui2];
                        if (!f2)
                        {
                            bool f3 = this.VectorIsFarSides[vi2, ui2];
                            if (!f3)
                            {
                                bool f4 = this.VectorIsFarSides[vi2, ui1];
                                if (!f4)
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

                                    this.QuadIsFarSides[vi - 1, ui] = false;
                                    this.Quads[vi - 1, ui] = quad;
                                    this.TransformMatrixes[vi - 1, ui] = transformMatrix;
                                    continue;
                                }
                            }
                        }
                    }

                    this.QuadIsFarSides[vi - 1, ui] = true;
                    this.Quads[vi - 1, ui] = Quadrilateral.Identity;
                    this.TransformMatrixes[vi - 1, ui] = Matrix4x4.Identity;
                }
            }

            {
                const int vi = 1;
                const int vi1 = vi - 1;
                const int vi2 = vi;

                for (int ui = 0; ui < UCount; ui++)
                {
                    int ui2 = ui == UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    if (!f1)
                    {
                        bool f2 = this.VectorIsFarSides[vi1, ui2];
                        if (!f2)
                        {
                            bool f3 = this.VectorIsFarSides[vi2, ui2];
                            if (!f3)
                            {
                                bool f4 = this.VectorIsFarSides[vi2, ui1];
                                if (!f4)
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

                                    Matrix4x4 transformMatrix = textureSize.SourceNormalizeHalf.ToPerspMatrix(quad);

                                    this.QuadIsFarSides[vi - 1, ui] = false;
                                    this.Quads[vi - 1, ui] = quad;
                                    this.TransformMatrixes[vi - 1, ui] = transformMatrix;
                                    continue;
                                }
                            }
                        }
                    }

                    this.QuadIsFarSides[vi - 1, ui] = true;
                    this.Quads[vi - 1, ui] = Quadrilateral.Identity;
                    this.TransformMatrixes[vi - 1, ui] = Matrix4x4.Identity;
                }
            }

            {
                const int vi = VCount;
                const int vi1 = vi - 1;
                const int vi2 = vi;

                for (int ui = 0; ui < UCount; ui++)
                {
                    int ui2 = ui == UCountMinus ? 0 : ui + 1;
                    int ui1 = ui;

                    bool f1 = this.VectorIsFarSides[vi1, ui1];
                    if (!f1)
                    {
                        bool f2 = this.VectorIsFarSides[vi1, ui2];
                        if (!f2)
                        {
                            bool f3 = this.VectorIsFarSides[vi2, ui2];
                            if (!f3)
                            {
                                bool f4 = this.VectorIsFarSides[vi2, ui1];
                                if (!f4)
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

                                    Matrix4x4 transformMatrix = textureSize.SourceNormalizeHalf.ToPerspMatrix(quad);

                                    this.QuadIsFarSides[vi - 1, ui] = false;
                                    this.Quads[vi - 1, ui] = quad;
                                    this.TransformMatrixes[vi - 1, ui] = transformMatrix;
                                    continue;
                                }
                            }
                        }
                    }

                    this.QuadIsFarSides[vi - 1, ui] = true;
                    this.Quads[vi - 1, ui] = Quadrilateral.Identity;
                    this.TransformMatrixes[vi - 1, ui] = Matrix4x4.Identity;
                }
            }
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
}