using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    internal readonly partial struct Handle
    {
        // 6
        readonly float rx, ry; // Right - Right
        readonly float cx, cy; // Right
        readonly float ox, oy; // Left

        // 8
        readonly float ox0, oy0, ox1, oy1; // Orign-Center Distance
        readonly float ax0, ay0, ax1, ay1; // LeftBottom-Center Distance

        // 1
        readonly float b;
        readonly IntersectionMode m;

        #region Triangles
        internal Handle(HandleOrign o, Triangle s, Center6 c)
        {
            switch (o)
            {
                case HandleOrign.Left:
                    rx = s.LeftBottom.X - s.LeftTop.X;
                    ry = s.LeftBottom.Y - s.LeftTop.Y;

                    cx = (s.LeftBottom.X + s.LeftTop.X) / 2f;
                    cy = (s.LeftBottom.Y + s.LeftTop.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = s.RightTop.X + (s.LeftBottom.X - s.LeftTop.X) / 2f;
                        oy = s.RightTop.Y + (s.LeftBottom.Y - s.LeftTop.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.RightTop.X - c.c.X;
                        ay0 = s.RightTop.Y - c.c.Y;

                        ax1 = s.RightTop.X - cx;
                        ay1 = s.RightTop.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = s.RightTop.X + (s.LeftBottom.X - s.LeftTop.X) / 2f;
                            oy = s.RightTop.Y + (s.LeftBottom.Y - s.LeftTop.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.RightTop.X - c.c.X;
                            ay0 = s.RightTop.Y - c.c.Y;

                            ax1 = s.RightTop.X - cx;
                            ay1 = s.RightTop.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                case HandleOrign.Top:
                    rx = s.LeftTop.X - s.RightTop.X;
                    ry = s.LeftTop.Y - s.RightTop.Y;

                    cx = (s.LeftTop.X + s.RightTop.X) / 2f;
                    cy = (s.LeftTop.Y + s.RightTop.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = s.LeftBottom.X + (s.RightTop.X - s.LeftTop.X) / 2f;
                        oy = s.LeftBottom.Y + (s.RightTop.Y - s.LeftTop.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.RightTop.X + s.LeftBottom.X - s.LeftTop.X - c.c.X;
                        ay0 = s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y - c.c.Y;

                        ax1 = s.RightTop.X + s.LeftBottom.X - s.LeftTop.X - cx;
                        ay1 = s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = s.LeftBottom.X + (s.RightTop.X - s.LeftTop.X) / 2f;
                            oy = s.LeftBottom.Y + (s.RightTop.Y - s.LeftTop.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.RightTop.X + s.LeftBottom.X - s.LeftTop.X - c.c.X;
                            ay0 = s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y - c.c.Y;

                            ax1 = s.RightTop.X + s.LeftBottom.X - s.LeftTop.X - cx;
                            ay1 = s.RightTop.Y + s.LeftBottom.Y - s.LeftTop.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                case HandleOrign.Right:
                    rx = s.LeftTop.X - s.LeftBottom.X;
                    ry = s.LeftTop.Y - s.LeftBottom.Y;

                    cx = s.RightTop.X + (s.LeftBottom.X - s.LeftTop.X) / 2f;
                    cy = s.RightTop.Y + (s.LeftBottom.Y - s.LeftTop.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = (s.LeftBottom.X + s.LeftTop.X) / 2f;
                        oy = (s.LeftBottom.Y + s.LeftTop.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.LeftBottom.X - c.c.X;
                        ay0 = s.LeftBottom.Y - c.c.Y;

                        ax1 = s.LeftBottom.X - cx;
                        ay1 = s.LeftBottom.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = (s.LeftBottom.X + s.LeftTop.X) / 2f;
                            oy = (s.LeftBottom.Y + s.LeftTop.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.LeftBottom.X - c.c.X;
                            ay0 = s.LeftBottom.Y - c.c.Y;

                            ax1 = s.LeftBottom.X - cx;
                            ay1 = s.LeftBottom.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                case HandleOrign.Bottom:
                    rx = s.RightTop.X - s.LeftTop.X;
                    ry = s.RightTop.Y - s.LeftTop.Y;

                    cx = s.LeftBottom.X + (s.RightTop.X + -s.LeftTop.X) / 2f;
                    cy = s.LeftBottom.Y + (s.RightTop.Y + -s.LeftTop.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = (s.LeftTop.X + s.RightTop.X) / 2f;
                        oy = (s.LeftTop.Y + s.RightTop.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.LeftTop.X - c.c.X;
                        ay0 = s.LeftTop.Y - c.c.Y;

                        ax1 = s.LeftTop.X - cx;
                        ay1 = s.LeftTop.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = (s.LeftTop.X + s.RightTop.X) / 2f;
                            oy = (s.LeftTop.Y + s.RightTop.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.LeftTop.X - c.c.X;
                            ay0 = s.LeftTop.Y - c.c.Y;

                            ax1 = s.LeftTop.X - cx;
                            ay1 = s.LeftTop.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                default:
                    rx = 0f;
                    ry = 0f;

                    cx = 0f;
                    cy = 0f;

                    ox = 0f;
                    oy = 0f;

                    ox0 = 0f;
                    oy0 = 0f;

                    ox1 = 0f;
                    oy1 = 0f;

                    ax0 = 0f;
                    ay0 = 0f;

                    ax1 = 0f;
                    ay1 = 0f;

                    b = 0f;
                    m = default;
                    break;
            }
        }

        internal Triangle To(HandleOrign o, Triangle s, Center6 c, Vector2 p, bool center)
        {
            switch (o)
            {
                case HandleOrign.Left:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Triangle
                                    {
                                        RightTop = s.RightTop + v0,

                                        LeftBottom = s.LeftBottom - v0,
                                        LeftTop = s.LeftTop - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Triangle
                                    {
                                        RightTop = s.RightTop + v1,

                                        LeftBottom = s.LeftBottom,
                                        LeftTop = s.LeftTop,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Triangle
                                {
                                    RightTop = s.RightTop + v0,

                                    LeftBottom = s.LeftBottom - v0,
                                    LeftTop = s.LeftTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Triangle
                                {
                                    RightTop = s.RightTop + v1,

                                    LeftBottom = s.LeftBottom,
                                    LeftTop = s.LeftTop,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Triangle
                                {
                                    RightTop = s.RightTop + v0,

                                    LeftBottom = s.LeftBottom - v0,
                                    LeftTop = s.LeftTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Triangle
                                {
                                    RightTop = s.RightTop + v1,

                                    LeftBottom = s.LeftBottom,
                                    LeftTop = s.LeftTop,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                case HandleOrign.Top:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Triangle
                                    {
                                        LeftBottom = s.LeftBottom + v0,

                                        LeftTop = s.LeftTop - v0,
                                        RightTop = s.RightTop - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Triangle
                                    {
                                        LeftBottom = s.LeftBottom + v1,

                                        LeftTop = s.LeftTop,
                                        RightTop = s.RightTop,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v0,

                                    LeftTop = s.LeftTop - v0,
                                    RightTop = s.RightTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v1,

                                    LeftTop = s.LeftTop,
                                    RightTop = s.RightTop,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v0,

                                    LeftTop = s.LeftTop - v0,
                                    RightTop = s.RightTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v1,

                                    LeftTop = s.LeftTop,
                                    RightTop = s.RightTop,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                case HandleOrign.Right:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Triangle
                                    {
                                        LeftBottom = s.LeftBottom + v0,
                                        LeftTop = s.LeftTop + v0,

                                        RightTop = s.RightTop - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Triangle
                                    {
                                        LeftBottom = s.LeftBottom + v1,
                                        LeftTop = s.LeftTop + v1,

                                        RightTop = s.RightTop,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v0,
                                    LeftTop = s.LeftTop + v0,

                                    RightTop = s.RightTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v1,
                                    LeftTop = s.LeftTop + v1,

                                    RightTop = s.RightTop,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v0,
                                    LeftTop = s.LeftTop + v0,

                                    RightTop = s.RightTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Triangle
                                {
                                    LeftBottom = s.LeftBottom + v1,
                                    LeftTop = s.LeftTop + v1,

                                    RightTop = s.RightTop,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                case HandleOrign.Bottom:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Triangle
                                    {
                                        LeftTop = s.LeftTop + v0,
                                        RightTop = s.RightTop + v0,

                                        LeftBottom = s.LeftBottom - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Triangle
                                    {
                                        LeftTop = s.LeftTop + v1,
                                        RightTop = s.RightTop + v1,

                                        LeftBottom = s.LeftBottom,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Triangle
                                {
                                    LeftTop = s.LeftTop + v0,
                                    RightTop = s.RightTop + v0,

                                    LeftBottom = s.LeftBottom - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Triangle
                                {
                                    LeftTop = s.LeftTop + v1,
                                    RightTop = s.RightTop + v1,

                                    LeftBottom = s.LeftBottom,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Triangle
                                {
                                    LeftTop = s.LeftTop + v0,
                                    RightTop = s.RightTop + v0,

                                    LeftBottom = s.LeftBottom - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Triangle
                                {
                                    LeftTop = s.LeftTop + v1,
                                    RightTop = s.RightTop + v1,

                                    LeftBottom = s.LeftBottom,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                default:
                    return s;
            }
        }
        #endregion

        #region Quadrilaterals
        internal Handle(HandleOrign o, Quadrilateral s, Center6 c)
        {
            switch (o)
            {
                case HandleOrign.Left:
                    rx = s.RightBottom.X - s.RightTop.X;
                    ry = s.RightBottom.Y - s.RightTop.Y;

                    cx = (s.LeftBottom.X + s.LeftTop.X) / 2f;
                    cy = (s.LeftBottom.Y + s.LeftTop.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = (s.RightTop.X + s.RightBottom.X) / 2f;
                        oy = (s.RightTop.Y + s.RightBottom.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.RightTop.X - c.c.X;
                        ay0 = s.RightTop.Y - c.c.Y;

                        ax1 = s.RightTop.X - cx;
                        ay1 = s.RightTop.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = (s.RightTop.X + s.RightBottom.X) / 2f;
                            oy = (s.RightTop.Y + s.RightBottom.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.RightTop.X - c.c.X;
                            ay0 = s.RightTop.Y - c.c.Y;

                            ax1 = s.RightTop.X - cx;
                            ay1 = s.RightTop.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                case HandleOrign.Top:
                    rx = s.LeftBottom.X - s.RightBottom.X;
                    ry = s.LeftBottom.Y - s.RightBottom.Y;

                    cx = (s.LeftTop.X + s.RightTop.X) / 2f;
                    cy = (s.LeftTop.Y + s.RightTop.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = (s.RightBottom.X + s.LeftBottom.X) / 2f;
                        oy = (s.RightBottom.Y + s.LeftBottom.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.RightBottom.X - c.c.X;
                        ay0 = s.RightBottom.Y - c.c.Y;

                        ax1 = s.RightBottom.X - cx;
                        ay1 = s.RightBottom.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = (s.RightBottom.X + s.LeftBottom.X) / 2f;
                            oy = (s.RightBottom.Y + s.LeftBottom.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.RightBottom.X - c.c.X;
                            ay0 = s.RightBottom.Y - c.c.Y;

                            ax1 = s.RightBottom.X - cx;
                            ay1 = s.RightBottom.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                case HandleOrign.Right:
                    rx = s.LeftTop.X - s.LeftBottom.X;
                    ry = s.LeftTop.Y - s.LeftBottom.Y;

                    cx = (s.RightTop.X + s.RightBottom.X) / 2f;
                    cy = (s.RightTop.Y + s.RightBottom.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = (s.LeftBottom.X + s.LeftTop.X) / 2f;
                        oy = (s.LeftBottom.Y + s.LeftTop.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.LeftBottom.X - c.c.X;
                        ay0 = s.LeftBottom.Y - c.c.Y;

                        ax1 = s.LeftBottom.X - cx;
                        ay1 = s.LeftBottom.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = (s.LeftBottom.X + s.LeftTop.X) / 2f;
                            oy = (s.LeftBottom.Y + s.LeftTop.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.LeftBottom.X - c.c.X;
                            ay0 = s.LeftBottom.Y - c.c.Y;

                            ax1 = s.LeftBottom.X - cx;
                            ay1 = s.LeftBottom.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                case HandleOrign.Bottom:
                    rx = s.RightTop.X - s.LeftTop.X;
                    ry = s.RightTop.Y - s.LeftTop.Y;

                    cx = (s.RightBottom.X + s.LeftBottom.X) / 2f;
                    cy = (s.RightBottom.Y + s.LeftBottom.Y) / 2f;

                    if (System.Math.Abs(rx) > float.Epsilon)
                    {
                        ox = (s.LeftTop.X + s.RightTop.X) / 2f;
                        oy = (s.LeftTop.Y + s.RightTop.Y) / 2f;

                        ox0 = c.c.X - ox;
                        oy0 = c.c.Y - oy;

                        ox1 = cx - ox;
                        oy1 = cy - oy;

                        ax0 = s.LeftTop.X - c.c.X;
                        ay0 = s.LeftTop.Y - c.c.Y;

                        ax1 = s.LeftTop.X - cx;
                        ay1 = s.LeftTop.Y - cy;

                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            b = ry / rx;
                            m = IntersectionMode.N3;
                        }
                        else
                        {
                            b = 0f;
                            m = IntersectionMode.N2;
                        }
                    }
                    else
                    {
                        if (System.Math.Abs(ry) > float.Epsilon)
                        {
                            ox = (s.LeftTop.X + s.RightTop.X) / 2f;
                            oy = (s.LeftTop.Y + s.RightTop.Y) / 2f;

                            ox0 = c.c.X - ox;
                            oy0 = c.c.Y - oy;

                            ox1 = cx - ox;
                            oy1 = cy - oy;

                            ax0 = s.LeftTop.X - c.c.X;
                            ay0 = s.LeftTop.Y - c.c.Y;

                            ax1 = s.LeftTop.X - cx;
                            ay1 = s.LeftTop.Y - cy;

                            b = 0f;
                            m = IntersectionMode.N1;
                        }
                        else
                        {
                            ox = 0f;
                            oy = 0f;

                            ox0 = 0f;
                            oy0 = 0f;

                            ox1 = 0f;
                            oy1 = 0f;

                            ax0 = 0f;
                            ay0 = 0f;

                            ax1 = 0f;
                            ay1 = 0f;

                            b = 0f;
                            m = IntersectionMode.N0;
                        }
                    }
                    break;
                default:
                    rx = 0f;
                    ry = 0f;

                    cx = 0f;
                    cy = 0f;

                    ox = 0f;
                    oy = 0f;

                    ox0 = 0f;
                    oy0 = 0f;

                    ox1 = 0f;
                    oy1 = 0f;

                    ax0 = 0f;
                    ay0 = 0f;

                    ax1 = 0f;
                    ay1 = 0f;

                    b = 0f;
                    m = default;
                    break;
            }
        }

        internal Quadrilateral To(HandleOrign o, Quadrilateral s, Center6 c, Vector2 p, bool center)
        {
            switch (o)
            {
                case HandleOrign.Left:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Quadrilateral
                                    {
                                        RightTop = s.RightTop + v0,
                                        RightBottom = s.RightBottom + v0,

                                        LeftBottom = s.LeftBottom - v0,
                                        LeftTop = s.LeftTop - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Quadrilateral
                                    {
                                        RightTop = s.RightTop + v1,
                                        RightBottom = s.RightBottom + v1,

                                        LeftBottom = s.LeftBottom,
                                        LeftTop = s.LeftTop,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    RightTop = s.RightTop + v0,
                                    RightBottom = s.RightBottom + v0,

                                    LeftBottom = s.LeftBottom - v0,
                                    LeftTop = s.LeftTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    RightTop = s.RightTop + v1,
                                    RightBottom = s.RightBottom + v1,

                                    LeftBottom = s.LeftBottom,
                                    LeftTop = s.LeftTop,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    RightTop = s.RightTop + v0,
                                    RightBottom = s.RightBottom + v0,

                                    LeftBottom = s.LeftBottom - v0,
                                    LeftTop = s.LeftTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    RightTop = s.RightTop + v1,
                                    RightBottom = s.RightBottom + v1,

                                    LeftBottom = s.LeftBottom,
                                    LeftTop = s.LeftTop,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                case HandleOrign.Top:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Quadrilateral
                                    {
                                        RightBottom = s.RightBottom + v0,
                                        LeftBottom = s.LeftBottom + v0,

                                        LeftTop = s.LeftTop - v0,
                                        RightTop = s.RightTop - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Quadrilateral
                                    {
                                        RightBottom = s.RightBottom + v1,
                                        LeftBottom = s.LeftBottom + v1,

                                        LeftTop = s.LeftTop,
                                        RightTop = s.RightTop,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    RightBottom = s.RightBottom + v0,
                                    LeftBottom = s.LeftBottom + v0,

                                    LeftTop = s.LeftTop - v0,
                                    RightTop = s.RightTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    RightBottom = s.RightBottom + v1,
                                    LeftBottom = s.LeftBottom + v1,

                                    LeftTop = s.LeftTop,
                                    RightTop = s.RightTop,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    RightBottom = s.RightBottom + v0,
                                    LeftBottom = s.LeftBottom + v0,

                                    LeftTop = s.LeftTop - v0,
                                    RightTop = s.RightTop - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    RightBottom = s.RightBottom + v1,
                                    LeftBottom = s.LeftBottom + v1,

                                    LeftTop = s.LeftTop,
                                    RightTop = s.RightTop,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                case HandleOrign.Right:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Quadrilateral
                                    {
                                        LeftBottom = s.LeftBottom + v0,
                                        LeftTop = s.LeftTop + v0,

                                        RightTop = s.RightTop - v0,
                                        RightBottom = s.RightBottom - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Quadrilateral
                                    {
                                        LeftBottom = s.LeftBottom + v1,
                                        LeftTop = s.LeftTop + v1,

                                        RightTop = s.RightTop,
                                        RightBottom = s.RightBottom,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    LeftBottom = s.LeftBottom + v0,
                                    LeftTop = s.LeftTop + v0,

                                    RightTop = s.RightTop - v0,
                                    RightBottom = s.RightBottom - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    LeftBottom = s.LeftBottom + v1,
                                    LeftTop = s.LeftTop + v1,

                                    RightTop = s.RightTop,
                                    RightBottom = s.RightBottom,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    LeftBottom = s.LeftBottom + v0,
                                    LeftTop = s.LeftTop + v0,

                                    RightTop = s.RightTop - v0,
                                    RightBottom = s.RightBottom - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    LeftBottom = s.LeftBottom + v1,
                                    LeftTop = s.LeftTop + v1,

                                    RightTop = s.RightTop,
                                    RightBottom = s.RightBottom,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                case HandleOrign.Bottom:
                    switch (m)
                    {
                        case IntersectionMode.None:
                            return s;
                        case IntersectionMode.N3:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float k0 = py0 / px0;
                                float c0 = b - k0;

                                if (System.Math.Abs(c0) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z0 = ax0 * b - ay0;

                                    Vector2 v0 = new Vector2(z0 / c0 + ox0,
                                         k0 * z0 / c0 + oy0);

                                    return new Quadrilateral
                                    {
                                        LeftTop = s.LeftTop + v0,
                                        RightTop = s.RightTop + v0,

                                        RightBottom = s.RightBottom - v0,
                                        LeftBottom = s.LeftBottom - v0,
                                    };
                                }
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float k1 = py1 / px1;
                                float c1 = b - k1;

                                if (System.Math.Abs(c1) < float.Epsilon)
                                {
                                    goto case IntersectionMode.None;
                                }
                                else
                                {
                                    float z1 = ax1 * b - ay1;

                                    Vector2 v1 = new Vector2(z1 / c1 + ox1,
                                         k1 * z1 / c1 + oy1);

                                    return new Quadrilateral
                                    {
                                        LeftTop = s.LeftTop + v1,
                                        RightTop = s.RightTop + v1,

                                        RightBottom = s.RightBottom,
                                        LeftBottom = s.LeftBottom,
                                    };
                                }
                            }
                        case IntersectionMode.N2:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = py0 / px0;

                                Vector2 v0 = new Vector2(ay0 / z0 + ox0,
                                     z0 * ay0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    LeftTop = s.LeftTop + v0,
                                    RightTop = s.RightTop + v0,

                                    RightBottom = s.RightBottom - v0,
                                    LeftBottom = s.LeftBottom - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = py1 / px1;

                                Vector2 v1 = new Vector2(ay1 / z1 + ox1,
                                     z1 * ay1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    LeftTop = s.LeftTop + v1,
                                    RightTop = s.RightTop + v1,

                                    RightBottom = s.RightBottom,
                                    LeftBottom = s.LeftBottom,
                                };
                            }
                        case IntersectionMode.N1:
                            if (center) // 0
                            {
                                float px0 = p.X - c.c.X;
                                float py0 = p.Y - c.c.Y;

                                float z0 = px0 / py0;

                                Vector2 v0 = new Vector2(z0 * ax0 / z0 + ox0,
                                      ax0 / z0 + oy0);

                                return new Quadrilateral
                                {
                                    LeftTop = s.LeftTop + v0,
                                    RightTop = s.RightTop + v0,

                                    RightBottom = s.RightBottom - v0,
                                    LeftBottom = s.LeftBottom - v0,
                                };
                            }
                            else
                            {
                                float px1 = p.X - cx;
                                float py1 = p.Y - cy;

                                float z1 = px1 / py1;

                                Vector2 v1 = new Vector2(z1 * ax1 / z1 + ox1,
                                      ax1 / z1 + oy1);

                                return new Quadrilateral
                                {
                                    LeftTop = s.LeftTop + v1,
                                    RightTop = s.RightTop + v1,

                                    RightBottom = s.RightBottom,
                                    LeftBottom = s.LeftBottom,
                                };
                            }
                        case IntersectionMode.N0:
                            return s;
                        default:
                            return s;
                    }
                default:
                    return s;
            }
        }
        #endregion
    }
}