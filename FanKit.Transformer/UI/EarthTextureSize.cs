using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct EarthTextureSize
    {
        public const float DemoBitmapWidth = 2048f;
        public const float DemoBitmapHeight = 1024f;

        readonly float TextureWidthF;
        readonly float TextureHeightF;
        readonly float HeightPolarEpsilonF;
        readonly float TextureHeightPolarEpsilonF;

        readonly int TextureWidth;
        readonly int TextureHeight;
        readonly int TextureHeightPolarEpsilon;

        internal readonly SizeMatrix SourceNormalize;
        internal readonly SizeMatrix SourceNormalizePolarEpsilon;

        public EarthTextureSize(EarthUV uv, float bitmapWidth, float bitmapHeight)
        {
            this.TextureWidthF = bitmapWidth / uv.UCountF;
            this.TextureHeightF = bitmapHeight / uv.VCount;
            this.HeightPolarEpsilonF = -this.TextureHeightF * Earth.PolarEpsilon;
            this.TextureHeightPolarEpsilonF = this.TextureHeightF + this.HeightPolarEpsilonF;

            this.TextureWidth = (int)this.TextureWidthF;
            this.TextureHeightPolarEpsilon = (int)this.TextureHeightPolarEpsilonF;
            this.TextureHeight = (int)this.TextureHeightF;

            this.SourceNormalize = new SizeMatrix(this.TextureWidthF, this.TextureHeightF);
            this.SourceNormalizePolarEpsilon = new SizeMatrix(this.TextureWidthF, this.TextureHeightPolarEpsilonF);
        }

        public IEnumerable<EarthCreateTexture> CreateTextures(EarthUV uv)
        {
            for (int vi = 1; vi < uv.VCount; vi++)
            {
                float y = -vi * this.TextureHeightF;

                for (int ui = 0; ui < uv.UCount; ui++)
                {
                    float x = -ui * this.TextureWidthF;

                    yield return new EarthCreateTexture
                    {
                        Index = new EarthTextureIndex
                        {
                            U = ui,
                            V = vi,
                        },

                        ImageX = x,
                        ImageY = y,

                        TextureWidth = this.TextureWidth + 1,
                        TextureHeight = this.TextureHeight + 1,
                    };
                }
            }

            const int vi0 = 0;
            float y0 = this.HeightPolarEpsilonF;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                float x = -ui * this.TextureWidthF;

                yield return new EarthCreateTexture
                {
                    Index = new EarthTextureIndex
                    {
                        U = ui,
                        V = vi0,
                    },

                    ImageX = x,
                    ImageY = y0,

                    TextureWidth = this.TextureWidth + 1,
                    TextureHeight = this.TextureHeightPolarEpsilon + 1,
                };
            }

            int vi1 = uv.VCount;
            float y1 = -vi1 * this.TextureHeightF;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                float x = -ui * this.TextureWidthF;

                yield return new EarthCreateTexture
                {
                    Index = new EarthTextureIndex
                    {
                        U = ui,
                        V = vi1,
                    },

                    ImageX = x,
                    ImageY = y1,

                    TextureWidth = this.TextureWidth + 1,
                    TextureHeight = this.TextureHeightPolarEpsilon + 1,
                };
            }
        }

        public Vector2? GetAmount(EarthUV uv, Earth earth, Vector2 point, float bitmapWidth, float bitmapHeight)
        {
            #region ZeroCorner
            for (int vi = 1; vi < uv.VCount; vi++)
            {
                for (int ui = 0; ui < uv.UCount; ui++)
                {
                    if (earth.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.ZeroCorner)
                    {
                        Quadrilateral quad = earth.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            Matrix4x4 matrix = earth.TransformMatrixes[vi, ui];

                            float y = vi * this.TextureHeightF;
                            float x = ui * this.TextureWidthF;

                            if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                            {
                                Vector2 offset = Math.Transform(point, m);

                                return new Vector2
                                {
                                    X = (x + offset.X) / bitmapWidth,
                                    Y = (y + offset.Y) / bitmapHeight,
                                };
                            }
                            else
                            {
                                return new Vector2
                                {
                                    X = x / bitmapWidth,
                                    Y = y / bitmapHeight,
                                };
                            }
                        }
                    }
                }
            }

            const int vi0 = 0;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.ZeroCorner)
                {
                    Quadrilateral quad = earth.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi0, ui];

                        float y0 = -this.HeightPolarEpsilonF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y0 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y0 / bitmapHeight,
                            };
                        }
                    }
                }
            }

            int vi1 = uv.VCount;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.ZeroCorner)
                {
                    Quadrilateral quad = earth.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi1, ui];

                        float y1 = vi1 * this.TextureHeightF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y1 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y1 / bitmapHeight,
                            };
                        }
                    }
                }
            }
            #endregion

            #region OneCorner
            for (int vi = 1; vi < uv.VCount; vi++)
            {
                for (int ui = 0; ui < uv.UCount; ui++)
                {
                    if (earth.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.OneCorner)
                    {
                        Quadrilateral quad = earth.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            Matrix4x4 matrix = earth.TransformMatrixes[vi, ui];

                            float y = vi * this.TextureHeightF;
                            float x = ui * this.TextureWidthF;

                            if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                            {
                                Vector2 offset = Math.Transform(point, m);

                                return new Vector2
                                {
                                    X = (x + offset.X) / bitmapWidth,
                                    Y = (y + offset.Y) / bitmapHeight,
                                };
                            }
                            else
                            {
                                return new Vector2
                                {
                                    X = x / bitmapWidth,
                                    Y = y / bitmapHeight,
                                };
                            }
                        }
                    }
                }
            }

            //const int vi0 = 0;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.OneCorner)
                {
                    Quadrilateral quad = earth.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi0, ui];

                        float y0 = -this.HeightPolarEpsilonF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y0 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y0 / bitmapHeight,
                            };
                        }
                    }
                }
            }

            //int vi1 = uv.VCount;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.OneCorner)
                {
                    Quadrilateral quad = earth.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi1, ui];

                        float y1 = vi1 * this.TextureHeightF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y1 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y1 / bitmapHeight,
                            };
                        }
                    }
                }
            }
            #endregion

            #region TwoCorners
            for (int vi = 1; vi < uv.VCount; vi++)
            {
                for (int ui = 0; ui < uv.UCount; ui++)
                {
                    if (earth.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.TwoCorners)
                    {
                        Quadrilateral quad = earth.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            Matrix4x4 matrix = earth.TransformMatrixes[vi, ui];

                            float y = vi * this.TextureHeightF;
                            float x = ui * this.TextureWidthF;

                            if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                            {
                                Vector2 offset = Math.Transform(point, m);

                                return new Vector2
                                {
                                    X = (x + offset.X) / bitmapWidth,
                                    Y = (y + offset.Y) / bitmapHeight,
                                };
                            }
                            else
                            {
                                return new Vector2
                                {
                                    X = x / bitmapWidth,
                                    Y = y / bitmapHeight,
                                };
                            }
                        }
                    }
                }
            }

            //const int vi0 = 0;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.TwoCorners)
                {
                    Quadrilateral quad = earth.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi0, ui];

                        float y0 = -this.HeightPolarEpsilonF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y0 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y0 / bitmapHeight,
                            };
                        }
                    }
                }
            }

            //int vi1 = uv.VCount;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.TwoCorners)
                {
                    Quadrilateral quad = earth.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi1, ui];

                        float y1 = vi1 * this.TextureHeightF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y1 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y1 / bitmapHeight,
                            };
                        }
                    }
                }
            }
            #endregion

            #region ThreeCorners
            for (int vi = 1; vi < uv.VCount; vi++)
            {
                for (int ui = 0; ui < uv.UCount; ui++)
                {
                    if (earth.QuadIsFarSides[vi, ui] == EarthTextureIsFarSide.ThreeCorners)
                    {
                        Quadrilateral quad = earth.Quads[vi, ui];

                        if (quad.ContainsPoint(point))
                        {
                            Matrix4x4 matrix = earth.TransformMatrixes[vi, ui];

                            float y = vi * this.TextureHeightF;
                            float x = ui * this.TextureWidthF;

                            if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                            {
                                Vector2 offset = Math.Transform(point, m);

                                return new Vector2
                                {
                                    X = (x + offset.X) / bitmapWidth,
                                    Y = (y + offset.Y) / bitmapHeight,
                                };
                            }
                            else
                            {
                                return new Vector2
                                {
                                    X = x / bitmapWidth,
                                    Y = y / bitmapHeight,
                                };
                            }
                        }
                    }
                }
            }

            //const int vi0 = 0;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi0, ui] == EarthTextureIsFarSide.ThreeCorners)
                {
                    Quadrilateral quad = earth.Quads[vi0, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi0, ui];

                        float y0 = -this.HeightPolarEpsilonF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y0 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y0 / bitmapHeight,
                            };
                        }
                    }
                }
            }

            //int vi1 = uv.VCount;

            for (int ui = 0; ui < uv.UCount; ui++)
            {
                if (earth.QuadIsFarSides[vi1, ui] == EarthTextureIsFarSide.ThreeCorners)
                {
                    Quadrilateral quad = earth.Quads[vi1, ui];

                    if (quad.ContainsPoint(point))
                    {
                        Matrix4x4 matrix = earth.TransformMatrixes[vi1, ui];

                        float y1 = vi1 * this.TextureHeightF;
                        float x = ui * this.TextureWidthF;

                        if (Matrix4x4.Invert(matrix, out Matrix4x4 m))
                        {
                            Vector2 offset = Math.Transform(point, m);

                            return new Vector2
                            {
                                X = (x + offset.X) / bitmapWidth,
                                Y = (y1 + offset.Y) / bitmapHeight,
                            };
                        }
                        else
                        {
                            return new Vector2
                            {
                                X = x / bitmapWidth,
                                Y = y1 / bitmapHeight,
                            };
                        }
                    }
                }
            }
            #endregion

            return null;
        }

        public static readonly Vector2[][] DemoBitmapPolygons = new Vector2[][]
        {
            new Vector2[]
            {
                new Vector2(2048,156), new Vector2(1911,228), new Vector2(1904,191), new Vector2(1951,159), new Vector2(1839,174), new Vector2(1798,199), new Vector2(1841,208),
                new Vector2(1839,253), new Vector2(1856,266), new Vector2(1828,276), new Vector2(1820,313), new Vector2(1760,336), new Vector2(1802,304), new Vector2(1819,269),
                new Vector2(1831,221), new Vector2(1792,270), new Vector2(1748,288), new Vector2(1766,309), new Vector2(1741,319), new Vector2(1734,288), new Vector2(1704,288),
                new Vector2(1706,299), new Vector2(1724,301), new Vector2(1705,315), new Vector2(1721,324), new Vector2(1723,346), new Vector2(1680,381), new Vector2(1653,395),
                new Vector2(1642,385), new Vector2(1622,402), new Vector2(1648,443), new Vector2(1623,466), new Vector2(1601,439), new Vector2(1594,436), new Vector2(1592,458),
                new Vector2(1610,482), new Vector2(1618,505), new Vector2(1601,493), new Vector2(1585,464), new Vector2(1581,430), new Vector2(1551,387), new Vector2(1522,390),
                new Vector2(1470,468), new Vector2(1435,393), new Vector2(1400,366), new Vector2(1299,347), new Vector2(1318,373), new Vector2(1342,368), new Vector2(1368,385),
                new Vector2(1326,422), new Vector2(1272,437), new Vector2(1229,352), new Vector2(1218,354), new Vector2(1275,453), new Vector2(1318,440), new Vector2(1292,497),
                new Vector2(1251,533), new Vector2(1258,596), new Vector2(1222,627), new Vector2(1224,652), new Vector2(1181,703), new Vector2(1127,711), new Vector2(1091,614),
                new Vector2(1103,573), new Vector2(1079,521), new Vector2(1080,488), new Vector2(1045,474), new Vector2(973,486), new Vector2(926,444), new Vector2(929,384),
                new Vector2(995,312), new Vector2(1079,306), new Vector2(1189,335), new Vector2(1223,330), new Vector2(1234,306), new Vector2(1183,306), new Vector2(1173,289),
                new Vector2(1215,273), new Vector2(1258,281), new Vector2(1242,252), new Vector2(1199,247), new Vector2(1155,283), new Vector2(1162,295), new Vector2(1149,305),
                new Vector2(1099,252), new Vector2(1090,259), new Vector2(1124,285), new Vector2(1115,302), new Vector2(1078,261), new Vector2(1046,264), new Vector2(1007,302),
                new Vector2(970,296), new Vector2(975,267), new Vector2(1015,265), new Vector2(1018,248), new Vector2(1000,240), new Vector2(1059,204), new Vector2(1076,187),
                new Vector2(1103,204), new Vector2(1145,196), new Vector2(1165,140), new Vector2(1124,164), new Vector2(1135,171), new Vector2(1110,187), new Vector2(1078,175),
                new Vector2(1050,177), new Vector2(1045,157), new Vector2(1120,116), new Vector2(1211,107), new Vector2(1303,122), new Vector2(1486,84), new Vector2(1655,76),
                new Vector2(1745,94), new Vector2(2048,120), new Vector2(2048,156),
            },
            new Vector2[]
            {
                new Vector2(0,116), new Vector2(62,134), new Vector2(39,148), new Vector2(0,142), new Vector2(0,116),
            },
            new Vector2[]
            {
                new Vector2(953,51), new Vector2(893,116), new Vector2(797,140), new Vector2(783,177), new Vector2(736,163), new Vector2(705,88), new Vector2(609,76),
                new Vector2(581,81), new Vector2(669,132), new Vector2(661,158), new Vector2(688,188), new Vector2(727,228), new Vector2(612,277), new Vector2(595,311),
                new Vector2(562,337), new Vector2(568,370), new Vector2(542,341), new Vector2(494,343), new Vector2(472,363), new Vector2(475,403), new Vector2(505,403),
                new Vector2(513,390), new Vector2(533,390), new Vector2(518,422), new Vector2(555,421), new Vector2(548,448), new Vector2(578,458), new Vector2(611,443),
                new Vector2(671,456), new Vector2(822,542), new Vector2(801,594), new Vector2(786,639), new Vector2(747,658), new Vector2(670,743), new Vector2(644,788),
                new Vector2(651,827), new Vector2(598,772), new Vector2(628,617), new Vector2(592,596), new Vector2(564,538), new Vector2(582,469), new Vector2(539,457),
                new Vector2(483,417), new Vector2(470,424), new Vector2(421,393), new Vector2(384,336), new Vector2(373,338), new Vector2(405,386), new Vector2(397,387),
                new Vector2(320,288), new Vector2(319,243), new Vector2(244,179), new Vector2(169,165), new Vector2(75,211), new Vector2(108,171), new Vector2(71,139),
                new Vector2(141,101), new Vector2(250,116), new Vector2(399,62), new Vector2(574,39), new Vector2(850,35), new Vector2(953,51),
            },
            new Vector2[]
            {
                new Vector2(2048,1024), new Vector2(2048,957), new Vector2(1961,948), new Vector2(2001,917), new Vector2(1774,880), new Vector2(1487,894),
                new Vector2(1326,875), new Vector2(974,904), new Vector2(813,957), new Vector2(682,942), new Vector2(704,863), new Vector2(557,926),
                new Vector2(321,924), new Vector2(111,961), new Vector2(0,952), new Vector2(0,1024), new Vector2(2048,1024),
            },
            new Vector2[]
            {
                new Vector2(1836,572), new Vector2(1900,669), new Vector2(1876,730), new Vector2(1830,731), new Vector2(1783,694), new Vector2(1682,711),
                new Vector2(1668,643), new Vector2(1774,577), new Vector2(1801,584), new Vector2(1801,604), new Vector2(1828,614), new Vector2(1836,572),
            },
            new Vector2[]
            {
                new Vector2(1691,470), new Vector2(1684,533), new Vector2(1649,529), new Vector2(1641,504), new Vector2(1691,470),
            },
            new Vector2[]
            {
                new Vector2(1566,481), new Vector2(1586,490), new Vector2(1632,537), new Vector2(1614,549), new Vector2(1566,481),
            },
            new Vector2[]
            {
                new Vector2(1792,528), new Vector2(1808,523), new Vector2(1864,549), new Vector2(1877,568), new Vector2(1846,557), new Vector2(1825,566), new Vector2(1792,528),
            },
            new Vector2[]
            {
                new Vector2(1303,577), new Vector2(1313,606), new Vector2(1293,657), new Vector2(1267,658), new Vector2(1303,577),
            },
        };
    }
}