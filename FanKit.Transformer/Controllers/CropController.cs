using System.Numerics;

namespace FanKit.Transformer.Controllers
{
    public readonly struct CropController
    {
        readonly CropMode flags;

        // Center
        readonly float ccx; // Corner & Side
        readonly float ccy; // Corner & Side

        readonly float cx; // Corner
        readonly float cy; // Corner
        readonly float cs; // Corner

        // Ratio
        readonly float dcx; // Corner
        readonly float dcy; // Corner

        readonly float dx; // Corner & Side
        readonly float dy; // Corner & Side
        readonly float ds; // Corner

        public CropController(Bounds bounds, CropMode mode)
        {
            flags = mode;

            switch (mode)
            {
                #region Side
                case CropMode.ScaleLeft:
                    // Center
                    ccx = bounds.Right + bounds.Left;
                    ccy = default;

                    cx = default;
                    cy = default;
                    cs = default;

                    dcx = default;
                    dcy = default;

                    // Ratio
                    dy = bounds.Right - bounds.Left;
                    dx = bounds.Bottom - bounds.Top;
                    ds = default;
                    break;
                case CropMode.ScaleTop:
                    // Center
                    ccx = default;
                    ccy = bounds.Bottom + bounds.Top;

                    cx = default;
                    cy = default;
                    cs = default;

                    dcx = default;
                    dcy = default;

                    // Ratio
                    dy = bounds.Bottom - bounds.Top;
                    dx = bounds.Right - bounds.Left;
                    ds = default;
                    break;
                case CropMode.ScaleRight:
                    // Center
                    ccx = bounds.Left + bounds.Right;
                    ccy = default;

                    cx = default;
                    cy = default;
                    cs = default;

                    dcx = default;
                    dcy = default;

                    // Ratio
                    dy = bounds.Left - bounds.Right;
                    dx = bounds.Top - bounds.Bottom;
                    ds = default;
                    break;
                case CropMode.ScaleBottom:
                    // Center
                    ccx = default;
                    ccy = bounds.Top + bounds.Bottom;

                    cx = default;
                    cy = default;
                    cs = default;

                    dcx = default;
                    dcy = default;

                    // Ratio
                    dy = bounds.Top - bounds.Bottom;
                    dx = bounds.Left - bounds.Right;
                    ds = default;
                    break;
                #endregion

                #region Corner
                case CropMode.ScaleLeftTop:
                    // Center & Ratio
                    ccx = (bounds.Right + bounds.Left) / 2f;
                    ccy = (bounds.Bottom + bounds.Top) / 2f;
                    cx = (bounds.Right - bounds.Left) / 2f;
                    cy = (bounds.Bottom - bounds.Top) / 2f;
                    cs = cx * cx + cy * cy;

                    // Center
                    dcx = bounds.Right;
                    dcy = bounds.Bottom;
                    dx = bounds.Right - bounds.Left;
                    dy = bounds.Bottom - bounds.Top;
                    ds = dx * dx + dy * dy;
                    break;
                case CropMode.ScaleRightTop:
                    // Center & Ratio
                    ccx = (bounds.Left + bounds.Right) / 2f;
                    ccy = (bounds.Bottom + bounds.Top) / 2f;
                    cx = (bounds.Left - bounds.Right) / 2f;
                    cy = (bounds.Bottom - bounds.Top) / 2f;
                    cs = cx * cx + cy * cy;

                    // Center
                    dcx = bounds.Left;
                    dcy = bounds.Bottom;
                    dx = bounds.Left - bounds.Right;
                    dy = bounds.Bottom - bounds.Top;
                    ds = dx * dx + dy * dy;
                    break;
                case CropMode.ScaleLeftBottom:
                    // Center & Ratio
                    ccx = (bounds.Right + bounds.Left) / 2f;
                    ccy = (bounds.Top + bounds.Bottom) / 2f;
                    cx = (bounds.Right - bounds.Left) / 2f;
                    cy = (bounds.Top - bounds.Bottom) / 2f;
                    cs = cx * cx + cy * cy;

                    // Center
                    dcx = bounds.Right;
                    dcy = bounds.Top;
                    dx = bounds.Right - bounds.Left;
                    dy = bounds.Top - bounds.Bottom;
                    ds = dx * dx + dy * dy;
                    break;
                case CropMode.ScaleRightBottom:
                    // Center & Ratio
                    ccx = (bounds.Left + bounds.Right) / 2f;
                    ccy = (bounds.Top + bounds.Bottom) / 2f;
                    cx = (bounds.Left - bounds.Right) / 2f;
                    cy = (bounds.Top - bounds.Bottom) / 2f;
                    cs = cx * cx + cy * cy;

                    // Center
                    dcx = bounds.Left;
                    dcy = bounds.Top;
                    dx = bounds.Left - bounds.Right;
                    dy = bounds.Top - bounds.Bottom;
                    ds = dx * dx + dy * dy;
                    break;
                #endregion

                default:
                    ccx = default;
                    ccy = default;

                    cx = default;
                    cy = default;
                    cs = default;

                    dcx = default;
                    dcy = default;

                    dy = default;
                    dx = default;
                    ds = default;
                    break;
            }
        }

        #region Corner
        private Bounds C(Bounds bounds, float x, float y)
        {
            switch (flags)
            {
                case CropMode.ScaleLeftTop:
                    return new Bounds
                    {
                        Right = bounds.Right,
                        Bottom = bounds.Bottom,

                        Left = x,
                        Top = y
                    };
                case CropMode.ScaleRightTop:
                    return new Bounds
                    {
                        Left = bounds.Left,
                        Bottom = bounds.Bottom,

                        Right = x,
                        Top = y
                    };
                case CropMode.ScaleLeftBottom:
                    return new Bounds
                    {
                        Right = bounds.Right,
                        Top = bounds.Top,

                        Left = x,
                        Bottom = y
                    };
                case CropMode.ScaleRightBottom:
                    return new Bounds
                    {
                        Left = bounds.Left,
                        Top = bounds.Top,

                        Right = x,
                        Bottom = y
                    };
                default:
                    return bounds;
            }
        }

        private Bounds S(Bounds bounds, float x, float y)
        {
            switch (flags)
            {
                case CropMode.ScaleLeftTop:
                    return new Bounds
                    {
                        // Center
                        Right = bounds.Right + bounds.Left - x,
                        Bottom = bounds.Bottom + bounds.Top - y,

                        Left = x,
                        Top = y
                    };
                case CropMode.ScaleRightTop:
                    return new Bounds
                    {
                        // Center
                        Left = bounds.Left + bounds.Right - x,
                        Bottom = bounds.Bottom + bounds.Top - y,

                        Right = x,
                        Top = y
                    };
                case CropMode.ScaleLeftBottom:
                    return new Bounds
                    {
                        // Center
                        Right = bounds.Right + bounds.Left - x,
                        Top = bounds.Top + bounds.Bottom - y,

                        Left = x,
                        Bottom = y
                    };
                case CropMode.ScaleRightBottom:
                    return new Bounds
                    {
                        // Center
                        Left = bounds.Left + bounds.Right - x,
                        Top = bounds.Top + bounds.Bottom - y,

                        Right = x,
                        Bottom = y
                    };
                default:
                    return bounds;
            }
        }
        #endregion

        public Bounds Crop(Bounds bounds, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            switch (flags)
            {
                #region Side
                case CropMode.ScaleLeft:
                    if (keepRatio)
                    {
                        float n = centeredScaling ? point.X + point.X - ccx : point.X - bounds.Right;
                        float half = dx * (n / dy * 0.5f + 0.5f);

                        return new Bounds
                        {
                            Top = bounds.Top + half,
                            Bottom = bounds.Bottom - half,

                            Right = centeredScaling ? ccx - point.X : bounds.Right,
                            Left = point.X
                        };
                    }
                    else
                    {
                        return new Bounds
                        {
                            Top = bounds.Top,
                            Bottom = bounds.Bottom,

                            Right = centeredScaling ? ccx - point.X : bounds.Right,
                            Left = point.X,
                        };
                    }
                case CropMode.ScaleTop:
                    if (keepRatio)
                    {
                        float n = centeredScaling ? point.Y + point.Y - ccy : point.Y - bounds.Bottom;
                        float half = dx * (n / dy * 0.5f + 0.5f);

                        return new Bounds
                        {
                            Left = bounds.Left + half,
                            Right = bounds.Right - half,

                            Bottom = centeredScaling ? ccy - point.Y : bounds.Bottom,
                            Top = point.Y
                        };
                    }
                    else
                    {
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Right = bounds.Right,

                            Bottom = centeredScaling ? ccy - point.Y : bounds.Bottom,
                            Top = point.Y,
                        };
                    }
                case CropMode.ScaleRight:
                    if (keepRatio)
                    {
                        float n = centeredScaling ? point.X + point.X - ccx : point.X - bounds.Left;
                        float half = dx * (n / dy * 0.5f + 0.5f);

                        return new Bounds
                        {
                            Bottom = bounds.Bottom + half,
                            Top = bounds.Top - half,

                            Left = centeredScaling ? ccx - point.X : bounds.Left,
                            Right = point.X
                        };
                    }
                    else
                    {
                        return new Bounds
                        {
                            Bottom = bounds.Bottom,
                            Top = bounds.Top,

                            Left = centeredScaling ? ccx - point.X : bounds.Left,
                            Right = point.X,
                        };
                    }
                case CropMode.ScaleBottom:
                    if (keepRatio)
                    {
                        float n = centeredScaling ? point.Y + point.Y - ccy : point.Y - bounds.Top;
                        float half = dx * (n / dy * 0.5f + 0.5f);

                        return new Bounds
                        {
                            Right = bounds.Right + half,
                            Left = bounds.Left - half,

                            Top = centeredScaling ? ccy - point.Y : bounds.Top,
                            Bottom = point.Y
                        };
                    }
                    else
                    {
                        return new Bounds
                        {
                            Right = bounds.Right,
                            Left = bounds.Left,

                            Top = centeredScaling ? ccy - point.Y : bounds.Top,
                            Bottom = point.Y,
                        };
                    }
                #endregion

                #region Corner
                case CropMode.ScaleLeftTop:
                case CropMode.ScaleRightTop:
                case CropMode.ScaleLeftBottom:
                case CropMode.ScaleRightBottom:
                    if (keepRatio)
                    {
                        if (centeredScaling)
                        {
                            // FootPoint
                            float a = ccx - point.X;
                            float b = ccy - point.Y;
                            float p = a * cx + b * cy;

                            //Judge = x * b - y * a;
                            float v = p / cs;
                            float fx = ccx - cx * v;
                            float fy = ccy - cy * v;

                            return S(bounds, fx, fy);
                        }
                        else
                        {
                            // FootPoint
                            float a = dcx - point.X;
                            float b = dcy - point.Y;
                            float p = a * dx + b * dy;

                            //Judge = x * b - y * a;
                            float v = p / ds;
                            float fx = dcx - dx * v;
                            float fy = dcy - dy * v;

                            return C(bounds, fx, fy);
                        }
                    }
                    else
                    {
                        if (centeredScaling)
                        {
                            return S(bounds, point.X, point.Y);
                        }
                        else
                        {
                            return C(bounds, point.X, point.Y);
                        }
                    }
                #endregion

                default:
                    return bounds;
            }
        }
    }
}