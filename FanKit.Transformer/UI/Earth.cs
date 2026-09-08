using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public class Earth : Graticule
    {
        readonly Matrix4x4[,] TransformMatrixes;

        public Matrix4x4[,] TextureTransformMatrixes => this.TransformMatrixes;

        public Earth(GraticuleUV uv) : base(uv)
        {
            this.TransformMatrixes = new Matrix4x4[uv.VCountPlus, uv.UCount];
        }

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