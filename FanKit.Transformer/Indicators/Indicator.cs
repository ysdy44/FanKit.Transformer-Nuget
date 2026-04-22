using System.Numerics;

namespace FanKit.Transformer.Indicators
{
    public partial class Indicator : IIndicator
    {
        // Delegate
        public event System.EventHandler<IndicatorSizeType> SizeTypeChanged;

        public event System.EventHandler<float> XChanged;
        public event System.EventHandler<float> YChanged;

        public event System.EventHandler<float> WidthChanged;
        public event System.EventHandler<float> HeightChanged;

        public event System.EventHandler<float> RotationChanged;
        public event System.EventHandler<float> SkewChanged;

        // 1. Old Data
        // XY
        Vector2 c; // Center

        // W
        float hx; // Horizontal Vector X
        float hy; // Horizontal Vector Y
        float hls; // Horizontal Vector Lenght Squared
        float hl; // Horizontal Vector Lenght

        // H
        float vx; // Vertical Vector X
        float vy; // Vertical Vector Y
        float vls; // Vertical Vector Lenght Squared
        float vl; // Vertical Vector Lenght

        // RS
        float hr; // Horizontal Vector Rotation (Atan2)
        float vr; // Vertical Vector Rotation (Atan2)

        // R
        float r; // Rotation

        // S
        float v; // Rotation Skew
        float s; // Skew

        // 2. New Date
        IndicatorSizeType ST = IndicatorSizeType.Point;

        float x = float.NaN;
        float y = float.NaN;

        float width = float.NaN;
        float height = float.NaN;

        float rotation = float.NaN;
        float skew = float.NaN;

        IndicatorSkew K;

        // 3. Property
        public IndicatorSizeType SizeType => ST;

        public float X => c.X;
        public float Y => c.Y;

        public float Width => hl;
        public float Height => vl;

        public float Rotation => r;
        public float Skew => s;

        public Matrix3x2 CreateRotation(float rotationAngleInDegrees)
        {
            return Matrix3x2.CreateRotation((rotationAngleInDegrees - r) * Constants.DegreesToRadians, c);
        }

        public void ClearAll()
        {
            // ST
            ST0();
            // XY
            EXY();
            // WH
            EW();
            EH();
            // RS
            ER();
            ES();
        }

        public void ChangeAll(Vector2 point)
        {
            // ST
            ST1();

            // XY
            c = point;
            if (x != c.X)
            {
                x = c.X;
                this.XChanged?.Invoke(this, X);
            }

            if (y != c.Y)
            {
                y = c.Y;
                this.YChanged?.Invoke(this, Y);
            }

            // WH
            EW();
            EH();
            // RS
            ER();
            ES();
        }

        public void ChangeXY(Vector2 point0, Vector2 point1, ColumnLineMode mode)
        {
            XY(point0, point1, mode);
        }
        public void ChangeXY(Vector2 point0, Vector2 point1, RowLineMode mode)
        {
            XY(point0, point1, mode);
        }

        public void ChangeAll(Vector2 point0, Vector2 point1, ColumnLineMode mode)
        {
            ST3();
            XY(point0, point1, mode);

            if (point1.Y == point0.Y)
            {
                if (point1.X == point0.X)
                {
                    EW();
                    EH();
                    ER();
                    ES();
                }
                else
                {
                    EW();

                    // H
                    vx = point1.X - point0.X;
                    vy = 0f;
                    vls = vx;
                    vl = vx;

                    if (height != vx)
                    {
                        height = vx;
                        this.HeightChanged?.Invoke(this, Height);
                    }

                    ER();
                    ES();
                }
            }
            else
            {
                if (point1.X == point0.X)
                {
                    EW();

                    // H
                    vx = 0f;
                    vy = point1.Y - point0.Y;
                    vls = vy;
                    vl = vy;

                    if (height != vy)
                    {
                        height = vy;
                        this.HeightChanged?.Invoke(this, Height);
                    }

                    // RS
                    vr = Constants.PIOver2;
                    hr = 0f;

                    // R
                    r = 90.0f;

                    if (rotation != 90.0f)
                    {
                        rotation = 90.0f;
                        RotationChanged?.Invoke(this, Rotation);
                    }

                    ES();
                }
                else
                {
                    EW();

                    // H
                    vx = point1.X - point0.X;
                    vy = point1.Y - point0.Y;
                    vls = vx * vx + vy * vy;
                    vl = (float)System.Math.Sqrt(vls);

                    if (height != vl)
                    {
                        height = vl;
                        this.HeightChanged?.Invoke(this, Height);
                    }

                    // RS
                    vr = (float)System.Math.Atan2(vy, vx);
                    hr = 0f;

                    VR();
                    ES();
                }
            }
        }

        public void ChangeAll(Vector2 point0, Vector2 point1, RowLineMode mode)
        {
            ST2();
            XY(point0, point1, mode);

            if (point1.Y == point0.Y)
            {
                if (point1.X == point0.X)
                {
                    EW();
                    EH();
                    ER();
                    ES();
                }
                else
                {
                    // W
                    hx = point1.X - point0.X;
                    hy = 0f;
                    hls = hx;
                    hl = hx;

                    if (width != hx)
                    {
                        width = hx;
                        this.WidthChanged?.Invoke(this, Width);
                    }

                    EH();
                    ER();
                    ES();
                }
            }
            else
            {
                if (point1.X == point0.X)
                {
                    // W
                    hx = 0f;
                    hy = point1.Y - point0.Y;
                    hls = hy;
                    hl = hy;

                    if (width != hy)
                    {
                        width = hy;
                        this.WidthChanged?.Invoke(this, Width);
                    }

                    EH();

                    // RS
                    hr = Constants.PIOver2;
                    vr = 0f;

                    // R
                    r = 90.0f;

                    if (rotation != 90.0f)
                    {
                        rotation = 90.0f;
                        RotationChanged?.Invoke(this, Rotation);
                    }

                    ES();
                }
                else
                {
                    // W
                    hx = point1.X - point0.X;
                    hy = point1.Y - point0.Y;
                    hls = hx * hx + hy * hy;
                    hl = (float)System.Math.Sqrt(hls);

                    if (width != hl)
                    {
                        width = hl;
                        this.WidthChanged?.Invoke(this, Width);
                    }

                    EH();

                    hr = (float)System.Math.Atan2(hy, hx);
                    vr = 0f;
                    HR();
                    ES();
                }
            }
        }

        // Empty
        private void EXY()
        {
            c = default;

            if (x != 0f)
            {
                x = 0f;
                this.XChanged?.Invoke(this, X);
            }

            if (y != 0f)
            {
                y = 0f;
                this.YChanged?.Invoke(this, Y);
            }
        }

        private void EW()
        {
            hx = 0f;
            hy = 0f;
            hls = 0f;
            hl = 0f;

            if (width != 0f)
            {
                width = 0f;
                this.WidthChanged?.Invoke(this, Width);
            }
        }

        private void EH()
        {
            vx = 0f;
            vy = 0f;
            vls = 0f;
            vl = 0f;

            if (height != 0f)
            {
                height = 0f;
                this.HeightChanged?.Invoke(this, Height);
            }
        }

        private void ER()
        {
            // RS
            hr = 0f;
            vr = 0f;

            // R
            r = 0f;

            if (rotation != 0f)
            {
                rotation = 0f;
                RotationChanged?.Invoke(this, Rotation);
            }
        }

        private void ES()
        {
            v = 0f;
            s = 0f;

            if (skew != 0f)
            {
                skew = 0f;
                SkewChanged?.Invoke(this, Skew);
            }
        }

        private void RS()
        {
            // RS
            hr = (float)System.Math.Atan2(hy, hx);
            vr = (float)System.Math.Atan2(vy, vx);

            HR();
            VS();
        }

        // XY
        private void XY(Vector2 point0, Vector2 point1, ColumnLineMode mode)
        {
            switch (mode)
            {
                case ColumnLineMode.Top:
                    c = point0;
                    break;
                case ColumnLineMode.Center:
                    c = new Vector2((point0.X + point1.X) / 2f,
                        (point0.Y + point1.Y) / 2f);
                    break;
                case ColumnLineMode.Bottom:
                    c = point1;
                    break;
                default:
                    c = default;
                    break;
            }

            if (x != c.X)
            {
                x = c.X;
                this.XChanged?.Invoke(this, X);
            }

            if (y != c.Y)
            {
                y = c.Y;
                this.YChanged?.Invoke(this, Y);
            }
        }
        private void XY(Vector2 point0, Vector2 point1, RowLineMode mode)
        {
            switch (mode)
            {
                case RowLineMode.Left:
                    c = point0;
                    break;
                case RowLineMode.Center:
                    c = new Vector2((point0.X + point1.X) / 2f,
                        (point0.Y + point1.Y) / 2f);
                    break;
                case RowLineMode.Right:
                    c = point1;
                    break;
                default:
                    c = default;
                    break;
            }

            if (x != c.X)
            {
                x = c.X;
                this.XChanged?.Invoke(this, X);
            }

            if (y != c.Y)
            {
                y = c.Y;
                this.YChanged?.Invoke(this, Y);
            }
        }

        // R
        private void HR()
        {
            if (float.IsNaN(hr))
            {
                r = 0f;

                if (rotation != 0f)
                {
                    rotation = 0f;
                    RotationChanged?.Invoke(this, Rotation);
                }
            }
            else
            {
                r = hr % Constants.PI * Constants.RadiansToDegrees;

                if (rotation != r)
                {
                    rotation = r;
                    RotationChanged?.Invoke(this, Rotation);
                }
            }
        }

        private void VR()
        {
            if (float.IsNaN(vr))
            {
                r = 0f;

                if (rotation != 0f)
                {
                    rotation = 0f;
                    RotationChanged?.Invoke(this, Rotation);
                }
            }
            else
            {
                r = vr % Constants.PI * Constants.RadiansToDegrees;

                if (rotation != r)
                {
                    rotation = r;
                    RotationChanged?.Invoke(this, Rotation);
                }
            }
        }

        // S
        private void VS()
        {
            if (float.IsNaN(vr))
            {
                ES();
            }
            else if (vy > float.Epsilon)
            {
                v = hr - vr + Constants.PIOver2;
                s = v % Constants.PI * Constants.RadiansToDegrees;

                if (skew != s)
                {
                    skew = s;
                    SkewChanged?.Invoke(this, Skew);
                }
            }
            else
            {
                v = hr - vr + Constants.PIOver2;
                s = v % Constants.PI * Constants.RadiansToDegrees;

                if (skew != s)
                {
                    skew = s;
                    SkewChanged?.Invoke(this, Skew);
                }
            }
        }

        #region Bounds
        public void ChangeX(Bounds bounds, BoxMode mode)
        {
            XY(bounds, mode);
        }
        public void ChangeY(Bounds bounds, BoxMode mode)
        {
            XY(bounds, mode);
        }
        public void ChangeXY(Bounds bounds, BoxMode mode)
        {
            XY(bounds, mode);
        }

        public void ChangeXYWH(Bounds bounds, BoxMode mode)
        {
            XY(bounds, mode);
            WH(bounds);
        }

        //public void ChangeXYWHRS(Bounds bounds, BoxMode mode)
        //{
        //XY(bounds, mode);
        //WH(bounds);
        //RS();
        //}
        public void ChangeAll(Bounds bounds, BoxMode mode)
        {
            XY(bounds, mode);
            WH(bounds);
            //RS();
        }

        //public Bounds CreateWidth(Bounds bounds, BoxMode mode, float value, bool keepRatio) => Resize(bounds, mode, value, keepRatio, false);
        public Bounds CreateWidth(Bounds bounds, BoxMode mode, float value, bool keepRatio)
        {
            if (keepRatio)
            {
                float x = bounds.Right - bounds.Left;
                float y = bounds.Bottom - bounds.Top;

                float c = (bounds.Bottom + bounds.Top) / 2f;
                float s = value * y / x;

                switch (mode)
                {
                    case BoxMode.LeftTop:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = bounds.Top,
                            Right = bounds.Left + value,
                            Bottom = c + s,
                        };
                    case BoxMode.LeftBottom:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = c - s,
                            Right = bounds.Left + value,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.RightTop:
                        return new Bounds
                        {
                            Left = bounds.Left - value,
                            Top = bounds.Top,
                            Right = bounds.Right,
                            Bottom = c + s,
                        };
                    case BoxMode.RightBottom:
                        return new Bounds
                        {
                            Left = bounds.Left - value,
                            Top = c - s,
                            Right = bounds.Right,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.CenterTop:
                        return new Bounds
                        {
                            Left = (bounds.Left + bounds.Right - value) / 2f,
                            Top = bounds.Top,
                            Right = (bounds.Left + bounds.Right + value) / 2f,
                            Bottom = c + s,
                        };
                    case BoxMode.CenterLeft:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = c - s / 2f,
                            Right = bounds.Left + value,
                            Bottom = c + s / 2f,
                        };
                    case BoxMode.CenterBottom:
                        return new Bounds
                        {
                            Left = (bounds.Left + bounds.Right - value) / 2f,
                            Top = c - s,
                            Right = (bounds.Left + bounds.Right + value) / 2f,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.CenterRight:
                        return new Bounds
                        {
                            Left = bounds.Right - value,
                            Top = c - s / 2f,
                            Right = bounds.Right,
                            Bottom = c + s / 2f,
                        };
                    case BoxMode.Center:
                        return new Bounds
                        {
                            Left = (bounds.Left + bounds.Right - value) / 2f,
                            Top = c - s / 2f,
                            Right = (bounds.Left + bounds.Right + value) / 2f,
                            Bottom = c + s / 2f,
                        };
                    default:
                        return bounds;
                }
            }
            else
            {
                switch (mode)
                {
                    case BoxMode.LeftTop:
                    case BoxMode.CenterLeft:
                    case BoxMode.LeftBottom:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = bounds.Top,
                            Right = bounds.Left + value,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.CenterTop:
                    case BoxMode.Center:
                    case BoxMode.CenterBottom:
                        return new Bounds
                        {
                            Left = (bounds.Left + bounds.Right - value) / 2f,
                            Top = bounds.Top,
                            Right = (bounds.Left + bounds.Right + value) / 2f,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.RightTop:
                    case BoxMode.CenterRight:
                    case BoxMode.RightBottom:
                        return new Bounds
                        {
                            Left = bounds.Right - value,
                            Top = bounds.Top,
                            Right = bounds.Right,
                            Bottom = bounds.Bottom,
                        };
                    default:
                        return bounds;
                }
            }
        }
        //public Bounds CreateHeight(Bounds bounds, BoxMode mode, float value, bool keepRatio) => Resize(bounds, mode, value, keepRatio, false);
        public Bounds CreateHeight(Bounds bounds, BoxMode mode, float value, bool keepRatio)
        {
            if (keepRatio)
            {
                float x = bounds.Right - bounds.Left;
                float y = bounds.Bottom - bounds.Top;

                float c = (bounds.Right + bounds.Left) / 2f;
                float s = value * x / y;

                switch (mode)
                {
                    case BoxMode.LeftTop:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = bounds.Top,
                            Right = c + s,
                            Bottom = bounds.Top + value,
                        };
                    case BoxMode.RightTop:
                        return new Bounds
                        {
                            Left = c - s,
                            Top = bounds.Top,
                            Right = bounds.Right,
                            Bottom = bounds.Top + value,
                        };
                    case BoxMode.LeftBottom:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = bounds.Top - value,
                            Right = c + s,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.RightBottom:
                        return new Bounds
                        {
                            Left = c - s,
                            Top = bounds.Top - value,
                            Right = bounds.Right,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.CenterLeft:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = (bounds.Top + bounds.Bottom - value) / 2f,
                            Right = c + s,
                            Bottom = (bounds.Top + bounds.Bottom + value) / 2f,
                        };
                    case BoxMode.CenterTop:
                        return new Bounds
                        {
                            Left = c - s / 2f,
                            Top = bounds.Top,
                            Right = c + s / 2f,
                            Bottom = bounds.Top + value,
                        };
                    case BoxMode.CenterRight:
                        return new Bounds
                        {
                            Left = c - s,
                            Top = (bounds.Top + bounds.Bottom - value) / 2f,
                            Right = bounds.Right,
                            Bottom = (bounds.Top + bounds.Bottom + value) / 2f,
                        };
                    case BoxMode.CenterBottom:
                        return new Bounds
                        {
                            Left = c - s / 2f,
                            Top = bounds.Bottom - value,
                            Right = c + s / 2f,
                            Bottom = bounds.Bottom,
                        };
                    case BoxMode.Center:
                        return new Bounds
                        {
                            Left = c - s / 2f,
                            Top = (bounds.Top + bounds.Bottom - value) / 2f,
                            Right = c + s / 2f,
                            Bottom = (bounds.Top + bounds.Bottom + value) / 2f,
                        };
                    default:
                        return bounds;
                }
            }
            else
            {
                switch (mode)
                {
                    case BoxMode.LeftTop:
                    case BoxMode.CenterTop:
                    case BoxMode.RightTop:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = bounds.Top,
                            Right = bounds.Right,
                            Bottom = bounds.Top + value,
                        };
                    case BoxMode.CenterLeft:
                    case BoxMode.Center:
                    case BoxMode.CenterRight:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = (bounds.Top + bounds.Bottom - value) / 2f,
                            Right = bounds.Right,
                            Bottom = (bounds.Top + bounds.Bottom + value) / 2f,
                        };
                    case BoxMode.LeftBottom:
                    case BoxMode.CenterBottom:
                    case BoxMode.RightBottom:
                        return new Bounds
                        {
                            Left = bounds.Left,
                            Top = bounds.Bottom - value,
                            Right = bounds.Right,
                            Bottom = bounds.Bottom,
                        };
                    default:
                        return bounds;
                }
            }
        }

        //public Bounds CreateSkew(Bounds bounds, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum) => Reskew(bounds, mode, skewAngleInDegrees, minimum, maximum);

        private void XY(Bounds t, BoxMode m)
        {
            switch (m)
            {
                case BoxMode.LeftTop:
                    c = new Vector2(t.Left, t.Top);
                    break;
                case BoxMode.RightTop:
                    c = new Vector2(t.Right, t.Top);
                    break;
                case BoxMode.LeftBottom:
                    c = new Vector2(t.Left, t.Bottom);
                    break;
                case BoxMode.RightBottom:
                    c = new Vector2(t.Right, t.Bottom);
                    break;

                case BoxMode.CenterLeft:
                    c = new Vector2(t.Left,
                    (t.Top + t.Bottom) / 2f);
                    break;
                case BoxMode.CenterTop:
                    c = new Vector2((t.Left + t.Right) / 2f,
                    t.Top);
                    break;
                case BoxMode.CenterRight:
                    c = new Vector2(t.Right,
                    (t.Top + t.Bottom) / 2f);
                    break;
                case BoxMode.CenterBottom:
                    c = new Vector2((t.Left + t.Right) / 2f,
                    t.Bottom);
                    break;

                case BoxMode.Center:
                    c = new Vector2((t.Left + t.Right) / 2f,
                    (t.Top + t.Bottom) / 2f);
                    break;

                default:
                    c = default;
                    break;
            }

            if (x != c.X)
            {
                x = c.X;
                this.XChanged?.Invoke(this, X);
            }

            if (y != c.Y)
            {
                y = c.Y;
                this.YChanged?.Invoke(this, Y);
            }
        }

        private void WH(Bounds t)
        {
            hx = default;
            hy = default;
            hls = default;
            hl = t.Right - t.Left;

            vx = default;
            vy = default;
            vls = default;
            vl = t.Bottom - t.Top;

            STC4(); // Crop

            if (width != hl)
            {
                width = hl;
                this.WidthChanged?.Invoke(this, Width);
            }

            if (height != vl)
            {
                height = vl;
                this.HeightChanged?.Invoke(this, Height);
            }
        }
        #endregion

        #region Triangles
        public void ChangeX(Triangle triangle, BoxMode mode)
        {
            XY(triangle, mode);
        }
        public void ChangeY(Triangle triangle, BoxMode mode)
        {
            XY(triangle, mode);
        }
        public void ChangeXY(Triangle triangle, BoxMode mode)
        {
            XY(triangle, mode);
        }

        public void ChangeXYWH(Triangle triangle, BoxMode mode)
        {
            XY(triangle, mode);
            WH(triangle);
        }

        public void ChangeXYWHRS(Triangle triangle, BoxMode mode)
        {
            XY(triangle, mode);
            WH(triangle);
            RS();
        }
        public void ChangeAll(Triangle triangle, BoxMode mode)
        {
            XY(triangle, mode);
            WH(triangle);
            RS();
        }

        public Triangle CreateWidth(Triangle triangle, BoxMode mode, float value, bool keepRatio) => Resize(triangle, mode, value, keepRatio, true);
        public Triangle CreateHeight(Triangle triangle, BoxMode mode, float value, bool keepRatio) => Resize(triangle, mode, value, keepRatio, false);

        public Triangle CreateSkew(Triangle triangle, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum) => Reskew(triangle, mode, skewAngleInDegrees, minimum, maximum);

        private void XY(Triangle t, BoxMode m)
        {
            switch (m)
            {
                case BoxMode.LeftTop:
                    c = t.LeftTop;
                    break;
                case BoxMode.RightTop:
                    c = t.RightTop;
                    break;
                case BoxMode.LeftBottom:
                    c = t.LeftBottom;
                    break;
                case BoxMode.RightBottom:
                    c = new Vector2(t.RightTop.X + t.LeftBottom.X - t.LeftTop.X,
                        t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y);
                    break;

                case BoxMode.CenterLeft:
                    c = new Vector2((t.LeftTop.X + t.LeftBottom.X) / 2f,
                        (t.LeftTop.Y + t.LeftBottom.Y) / 2f);
                    break;
                case BoxMode.CenterTop:
                    c = new Vector2((t.LeftTop.X + t.RightTop.X) / 2f,
                        (t.LeftTop.Y + t.RightTop.Y) / 2f);
                    break;
                case BoxMode.CenterRight:
                    c = new Vector2(t.RightTop.X + (t.LeftBottom.X - t.LeftTop.X) / 2f,
                        t.RightTop.Y + (t.LeftBottom.Y - t.LeftTop.Y) / 2f);
                    break;
                case BoxMode.CenterBottom:
                    c = new Vector2(t.LeftBottom.X + (t.RightTop.X - t.LeftTop.X) / 2f,
                        t.LeftBottom.Y + (t.RightTop.Y - t.LeftTop.Y) / 2f);
                    break;

                case BoxMode.Center:
                    c = new Vector2((t.RightTop.X + t.LeftBottom.X) / 2f,
                        (t.RightTop.Y + t.LeftBottom.Y) / 2f);
                    break;

                default:
                    c = default;
                    break;
            }

            if (x != c.X)
            {
                x = c.X;
                this.XChanged?.Invoke(this, X);
            }

            if (y != c.Y)
            {
                y = c.Y;
                this.YChanged?.Invoke(this, Y);
            }
        }

        private void WH(Triangle t)
        {
            hx = t.RightTop.X - t.LeftTop.X;
            hy = t.RightTop.Y - t.LeftTop.Y;
            hls = hx * hx + hy * hy;
            hl = (float)System.Math.Sqrt(hls);

            vx = t.LeftBottom.X - t.LeftTop.X;
            vy = t.LeftBottom.Y - t.LeftTop.Y;
            vls = vx * vx + vy * vy;
            vl = (float)System.Math.Sqrt(vls);

            STC5(); // Transform

            if (width != hl)
            {
                width = hl;
                this.WidthChanged?.Invoke(this, Width);
            }

            if (height != vl)
            {
                height = vl;
                this.HeightChanged?.Invoke(this, Height);
            }
        }

        private Triangle Resize(Triangle t, BoxMode m, float v, bool k, bool w)
        {
            if (k)
            {
                float s = w ? v / hl : v / vl;

                switch (m)
                {
                    case BoxMode.LeftTop:
                        {
                            float x = t.LeftTop.X - t.LeftTop.X * s;
                            float y = t.LeftTop.Y - t.LeftTop.Y * s;

                            return new Triangle
                            {
                                LeftTop = t.LeftTop,
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                            };
                        }
                    case BoxMode.RightTop:
                        {
                            float x = t.RightTop.X - t.RightTop.X * s;
                            float y = t.RightTop.Y - t.RightTop.Y * s;

                            return new Triangle
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = t.RightTop,
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                            };
                        }
                    case BoxMode.LeftBottom:
                        {
                            float x = t.LeftBottom.X - t.LeftBottom.X * s;
                            float y = t.LeftBottom.Y - t.LeftBottom.Y * s;

                            return new Triangle
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = t.LeftBottom,
                            };
                        }
                    case BoxMode.RightBottom:
                        {
                            float rbx = t.RightTop.X + t.LeftBottom.X - t.LeftTop.X;
                            float rby = t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y;

                            float x = rbx - rbx * s;
                            float y = rby - rby * s;

                            return new Triangle
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                            };
                        }
                    case BoxMode.CenterLeft:
                        {
                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = t.LeftBottom.X + t.LeftTop.X;
                            float cy = t.LeftBottom.Y + t.LeftTop.Y;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Triangle
                            {
                                LeftBottom = new Vector2(t.LeftBottom.X * ra + t.LeftTop.X * rs, t.LeftBottom.Y * ra + t.LeftTop.Y * rs),
                                LeftTop = new Vector2(t.LeftTop.X * ra + t.LeftBottom.X * rs, t.LeftTop.Y * ra + t.LeftBottom.Y * rs),

                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                            };
                        }
                    case BoxMode.CenterTop:
                        {
                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = t.LeftTop.X + t.RightTop.X;
                            float cy = t.LeftTop.Y + t.RightTop.Y;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Triangle
                            {
                                LeftTop = new Vector2(t.LeftTop.X * ra + t.RightTop.X * rs, t.LeftTop.Y * ra + t.RightTop.Y * rs),
                                RightTop = new Vector2(t.RightTop.X * ra + t.LeftTop.X * rs, t.RightTop.Y * ra + t.LeftTop.Y * rs),

                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                            };
                        }
                    case BoxMode.CenterRight:
                        {
                            float rbx = t.RightTop.X + t.LeftBottom.X - t.LeftTop.X;
                            float rby = t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y;

                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = t.RightTop.X + rbx;
                            float cy = t.RightTop.Y + rby;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Triangle
                            {
                                RightTop = new Vector2(t.RightTop.X * ra + rbx * rs, t.RightTop.Y * ra + rby * rs),

                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                            };
                        }
                    case BoxMode.CenterBottom:
                        {
                            float rbx = t.RightTop.X + t.LeftBottom.X - t.LeftTop.X;
                            float rby = t.RightTop.Y + t.LeftBottom.Y - t.LeftTop.Y;

                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = rbx + t.LeftBottom.X;
                            float cy = rby + t.LeftBottom.Y;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Triangle
                            {
                                LeftBottom = new Vector2(t.LeftBottom.X * ra + rbx * rs, t.LeftBottom.Y * ra + rby * rs),

                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                            };
                        }
                    case BoxMode.Center:
                    default:
                        {
                            const float f = 1f / 4f;

                            float fs = f - s / 4f;
                            float x = (t.RightTop.X + t.LeftBottom.X) * 2f * fs;
                            float y = (t.RightTop.Y + t.LeftBottom.Y) * 2f * fs;

                            return new Triangle
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                            };
                        }
                }
            }
            else if (w)
            {
                float scale = v / hl - 1f;
                switch (m)
                {
                    case BoxMode.LeftTop:
                    case BoxMode.CenterLeft:
                    case BoxMode.LeftBottom:
                        {
                            float x = hx * scale;
                            float y = hy * scale;

                            return new Triangle
                            {
                                LeftTop = t.LeftTop,
                                LeftBottom = t.LeftBottom,

                                RightTop = new Vector2(t.RightTop.X + x, t.RightTop.Y + y),
                            };
                        }

                    case BoxMode.RightTop:
                    case BoxMode.CenterRight:
                    case BoxMode.RightBottom:
                        {
                            float x = hx * scale;
                            float y = hy * scale;

                            return new Triangle
                            {
                                RightTop = t.RightTop,

                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                LeftBottom = new Vector2(t.LeftBottom.X - x, t.LeftBottom.Y - y),
                            };
                        }

                    default:
                        {
                            float x = hx * scale / 2f;
                            float y = hy * scale / 2f;

                            return new Triangle
                            {
                                RightTop = new Vector2(t.RightTop.X + x, t.RightTop.Y + y),

                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                LeftBottom = new Vector2(t.LeftBottom.X - x, t.LeftBottom.Y - y),
                            };
                        }
                }
            }
            else
            {
                float scale = v / vl - 1f;
                switch (m)
                {
                    case BoxMode.LeftTop:
                    case BoxMode.CenterTop:
                    case BoxMode.RightTop:
                        {
                            float x = vx * scale;
                            float y = vy * scale;

                            return new Triangle
                            {
                                LeftTop = t.LeftTop,
                                RightTop = t.RightTop,

                                LeftBottom = new Vector2(t.LeftBottom.X + x, t.LeftBottom.Y + y),
                            };
                        }

                    case BoxMode.LeftBottom:
                    case BoxMode.CenterBottom:
                    case BoxMode.RightBottom:
                        {
                            float x = vx * scale;
                            float y = vy * scale;

                            return new Triangle
                            {
                                LeftBottom = t.LeftBottom,

                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                RightTop = new Vector2(t.RightTop.X - x, t.RightTop.Y - y),
                            };
                        }

                    default:
                        {
                            float x = vx * scale / 2f;
                            float y = vy * scale / 2f;

                            return new Triangle
                            {
                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                RightTop = new Vector2(t.RightTop.X - x, t.RightTop.Y - y),

                                LeftBottom = new Vector2(t.LeftBottom.X + x, t.LeftBottom.Y + y),
                            };
                        }
                }
            }
        }

        private Triangle Reskew(Triangle t, BoxMode m, float a, float min, float max)
        {
            this.K = new IndicatorSkew(hr, t, m, a, min, max);

            float hx2 = hx / 2;
            float hy2 = hy / 2;

            switch (m)
            {
                case BoxMode.LeftTop:
                case BoxMode.CenterTop:
                case BoxMode.RightTop:
                    return new Triangle
                    {
                        LeftTop = t.LeftTop,
                        RightTop = t.RightTop,
                        LeftBottom = new Vector2(this.K.fx0 - hx2, this.K.fy0 - hy2),
                    };
                case BoxMode.LeftBottom:
                case BoxMode.CenterBottom:
                case BoxMode.RightBottom:
                    return new Triangle
                    {
                        LeftTop = new Vector2(this.K.fx1 - hx2, this.K.fy1 - hy2),
                        RightTop = new Vector2(this.K.fx1 + hx2, this.K.fy1 + hy2),
                        LeftBottom = t.LeftBottom,
                    };
                case BoxMode.CenterLeft:
                case BoxMode.CenterRight:
                case BoxMode.Center:
                    return new Triangle
                    {
                        LeftTop = new Vector2(this.K.fx1 - hx2, this.K.fy1 - hy2),
                        RightTop = new Vector2(this.K.fx1 + hx2, this.K.fy1 + hy2),
                        LeftBottom = new Vector2(this.K.fx0 - hx2, this.K.fy0 - hy2),
                    };
                default:
                    return t;
            }
        }
        #endregion

        #region Quadrilaterals
        public void ChangeX(Quadrilateral quad, BoxMode mode)
        {
            XY(quad, mode);
        }
        public void ChangeY(Quadrilateral quad, BoxMode mode)
        {
            XY(quad, mode);
        }
        public void ChangeXY(Quadrilateral quad, BoxMode mode)
        {
            XY(quad, mode);
        }

        public void ChangeXYWH(Quadrilateral quad, BoxMode mode)
        {
            XY(quad, mode);
            WH(quad);
        }

        public void ChangeXYWHRS(Quadrilateral quad, BoxMode mode)
        {
            XY(quad, mode);
            WH(quad);
            RS();
        }
        public void ChangeAll(Quadrilateral quad, BoxMode mode)
        {
            XY(quad, mode);
            WH(quad);
            RS();
        }

        public Quadrilateral CreateWidth(Quadrilateral quad, BoxMode mode, float value, bool keepRatio) => Resize(quad, mode, value, keepRatio, true);
        public Quadrilateral CreateHeight(Quadrilateral quad, BoxMode mode, float value, bool keepRatio) => Resize(quad, mode, value, keepRatio, false);

        public Quadrilateral CreateSkew(Quadrilateral quad, BoxMode mode, float skewAngleInDegrees, float minimum, float maximum) => Reskew(quad, mode, skewAngleInDegrees, minimum, maximum);

        private void XY(Quadrilateral t, BoxMode m)
        {
            switch (m)
            {
                case BoxMode.LeftTop:
                    c = t.LeftTop;
                    break;
                case BoxMode.RightTop:
                    c = t.RightTop;
                    break;
                case BoxMode.LeftBottom:
                    c = t.LeftBottom;
                    break;
                case BoxMode.RightBottom:
                    c = t.RightBottom;
                    break;

                case BoxMode.CenterLeft:
                    c = new Vector2((t.LeftTop.X + t.LeftBottom.X) / 2f,
                        (t.LeftTop.Y + t.LeftBottom.Y) / 2f);
                    break;
                case BoxMode.CenterTop:
                    c = new Vector2((t.LeftTop.X + t.RightTop.X) / 2f,
                        (t.LeftTop.Y + t.RightTop.Y) / 2f);
                    break;
                case BoxMode.CenterRight:
                    c = new Vector2((t.RightTop.X + t.RightBottom.X) / 2f,
                        (t.RightTop.Y + t.RightBottom.Y) / 2f);
                    break;
                case BoxMode.CenterBottom:
                    c = new Vector2((t.RightBottom.X + t.LeftBottom.X) / 2f,
                        (t.RightBottom.Y + t.LeftBottom.Y) / 2f);
                    break;

                case BoxMode.Center:
                    c = new Vector2((t.LeftTop.X + t.RightTop.X + t.RightBottom.X + t.LeftBottom.X) / 4f,
                        (t.LeftTop.Y + t.RightTop.Y + t.RightBottom.Y + t.LeftBottom.Y) / 4f);
                    break;

                default:
                    c = default;
                    break;
            }

            if (x != c.X)
            {
                x = c.X;
                this.XChanged?.Invoke(this, X);
            }

            if (y != c.Y)
            {
                y = c.Y;
                this.YChanged?.Invoke(this, Y);
            }
        }

        private void WH(Quadrilateral t)
        {
            hx = (t.RightTop.X + t.RightBottom.X - t.LeftTop.X - t.LeftBottom.X) / 2f;
            hy = (t.RightTop.Y + t.RightBottom.Y - t.LeftTop.Y - t.LeftBottom.Y) / 2f;
            hls = hx * hx + hy * hy;
            hl = (float)System.Math.Sqrt(hls);

            vx = (t.LeftBottom.X + t.RightBottom.X - t.LeftTop.X - t.RightTop.X) / 2f;
            vy = (t.LeftBottom.Y + t.RightBottom.Y - t.LeftTop.Y - t.RightTop.Y) / 2f;
            vls = vx * vx + vy * vy;
            vl = (float)System.Math.Sqrt(vls);

            STC5(); // Transform

            if (width != hl)
            {
                width = hl;
                this.WidthChanged?.Invoke(this, Width);
            }

            if (height != vl)
            {
                height = vl;
                this.HeightChanged?.Invoke(this, Height);
            }
        }

        private Quadrilateral Resize(Quadrilateral t, BoxMode m, float v, bool k, bool w)
        {
            if (k)
            {
                float s = w ? v / hl : v / vl;

                switch (m)
                {
                    case BoxMode.LeftTop:
                        {
                            float x = t.LeftTop.X - t.LeftTop.X * s;
                            float y = t.LeftTop.Y - t.LeftTop.Y * s;

                            return new Quadrilateral
                            {
                                LeftTop = t.LeftTop,
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                                RightBottom = new Vector2(t.RightBottom.X * s + x, t.RightBottom.Y * s + y),
                            };
                        }
                    case BoxMode.RightTop:
                        {
                            float x = t.RightTop.X - t.RightTop.X * s;
                            float y = t.RightTop.Y - t.RightTop.Y * s;

                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = t.RightTop,
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                                RightBottom = new Vector2(t.RightBottom.X * s + x, t.RightBottom.Y * s + y),
                            };
                        }
                    case BoxMode.LeftBottom:
                        {
                            float x = t.LeftBottom.X - t.LeftBottom.X * s;
                            float y = t.LeftBottom.Y - t.LeftBottom.Y * s;

                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = t.LeftBottom,
                                RightBottom = new Vector2(t.RightBottom.X * s + x, t.RightBottom.Y * s + y),
                            };
                        }
                    case BoxMode.RightBottom:
                        {
                            float x = t.RightBottom.X - t.RightBottom.X * s;
                            float y = t.RightBottom.Y - t.RightBottom.Y * s;

                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                                RightBottom = t.RightBottom,
                            };
                        }
                    case BoxMode.CenterLeft:
                        {
                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = t.LeftBottom.X + t.LeftTop.X;
                            float cy = t.LeftBottom.Y + t.LeftTop.Y;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Quadrilateral
                            {
                                LeftBottom = new Vector2(t.LeftBottom.X * ra + t.LeftTop.X * rs, t.LeftBottom.Y * ra + t.LeftTop.Y * rs),
                                LeftTop = new Vector2(t.LeftTop.X * ra + t.LeftBottom.X * rs, t.LeftTop.Y * ra + t.LeftBottom.Y * rs),

                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                RightBottom = new Vector2(t.RightBottom.X * s + x, t.RightBottom.Y * s + y),
                            };
                        }
                    case BoxMode.CenterTop:
                        {
                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = t.LeftTop.X + t.RightTop.X;
                            float cy = t.LeftTop.Y + t.RightTop.Y;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(t.LeftTop.X * ra + t.RightTop.X * rs, t.LeftTop.Y * ra + t.RightTop.Y * rs),
                                RightTop = new Vector2(t.RightTop.X * ra + t.LeftTop.X * rs, t.RightTop.Y * ra + t.LeftTop.Y * rs),

                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                                RightBottom = new Vector2(t.RightBottom.X * s + x, t.RightBottom.Y * s + y),
                            };
                        }
                    case BoxMode.CenterRight:
                        {
                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = t.RightTop.X + t.RightBottom.X;
                            float cy = t.RightTop.Y + t.RightBottom.Y;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Quadrilateral
                            {
                                RightTop = new Vector2(t.RightTop.X * ra + t.RightBottom.X * rs, t.RightTop.Y * ra + t.RightBottom.Y * rs),
                                RightBottom = new Vector2(t.RightBottom.X * ra + t.RightTop.X * rs, t.RightBottom.Y * ra + t.RightTop.Y * rs),

                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                            };
                        }
                    case BoxMode.CenterBottom:
                        {
                            float ra = (1f + s) / 2f;
                            float rs = (1f - s) / 2f;

                            float cx = t.RightBottom.X + t.LeftBottom.X;
                            float cy = t.RightBottom.Y + t.LeftBottom.Y;

                            float rx = cx - cx * s;
                            float ry = cy - cy * s;

                            float x = rx / 2f;
                            float y = ry / 2f;

                            return new Quadrilateral
                            {
                                RightBottom = new Vector2(t.RightBottom.X * ra + t.LeftBottom.X * rs, t.RightBottom.Y * ra + t.LeftBottom.Y * rs),
                                LeftBottom = new Vector2(t.LeftBottom.X * ra + t.RightBottom.X * rs, t.LeftBottom.Y * ra + t.RightBottom.Y * rs),

                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                            };
                        }
                    case BoxMode.Center:
                    default:
                        {
                            const float f = 1f / 4f;

                            float fs = f - s / 4f;
                            float x = (t.LeftTop.X + t.RightTop.X + t.RightBottom.X + t.LeftBottom.X) * fs;
                            float y = (t.LeftTop.Y + t.RightTop.Y + t.RightBottom.Y + t.LeftBottom.Y) * fs;

                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(t.LeftTop.X * s + x, t.LeftTop.Y * s + y),
                                RightTop = new Vector2(t.RightTop.X * s + x, t.RightTop.Y * s + y),
                                LeftBottom = new Vector2(t.LeftBottom.X * s + x, t.LeftBottom.Y * s + y),
                                RightBottom = new Vector2(t.RightBottom.X * s + x, t.RightBottom.Y * s + y),
                            };
                        }
                }
            }
            else if (w)
            {
                float scale = v / hl - 1f;
                switch (m)
                {
                    case BoxMode.LeftTop:
                    case BoxMode.CenterLeft:
                    case BoxMode.LeftBottom:
                        {
                            float x = hx * scale;
                            float y = hy * scale;

                            return new Quadrilateral
                            {
                                LeftTop = t.LeftTop,
                                LeftBottom = t.LeftBottom,

                                RightTop = new Vector2(t.RightTop.X + x, t.RightTop.Y + y),
                                RightBottom = new Vector2(t.RightBottom.X + x, t.RightBottom.Y + y),
                            };
                        }

                    case BoxMode.RightTop:
                    case BoxMode.CenterRight:
                    case BoxMode.RightBottom:
                        {
                            float x = hx * scale;
                            float y = hy * scale;

                            return new Quadrilateral
                            {
                                RightTop = t.RightTop,
                                RightBottom = t.RightBottom,

                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                LeftBottom = new Vector2(t.LeftBottom.X - x, t.LeftBottom.Y - y),
                            };
                        }

                    default:
                        {
                            float x = hx * scale / 2f;
                            float y = hy * scale / 2f;

                            return new Quadrilateral
                            {
                                RightTop = new Vector2(t.RightTop.X + x, t.RightTop.Y + y),
                                RightBottom = new Vector2(t.RightBottom.X + x, t.RightBottom.Y + y),

                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                LeftBottom = new Vector2(t.LeftBottom.X - x, t.LeftBottom.Y - y),
                            };
                        }
                }
            }
            else
            {
                float scale = v / vl - 1f;
                switch (m)
                {
                    case BoxMode.LeftTop:
                    case BoxMode.CenterTop:
                    case BoxMode.RightTop:
                        {
                            float x = vx * scale;
                            float y = vy * scale;

                            return new Quadrilateral
                            {
                                LeftTop = t.LeftTop,
                                RightTop = t.RightTop,

                                LeftBottom = new Vector2(t.LeftBottom.X + x, t.LeftBottom.Y + y),
                                RightBottom = new Vector2(t.RightBottom.X + x, t.RightBottom.Y + y),
                            };
                        }

                    case BoxMode.LeftBottom:
                    case BoxMode.CenterBottom:
                    case BoxMode.RightBottom:
                        {
                            float x = vx * scale;
                            float y = vy * scale;

                            return new Quadrilateral
                            {
                                LeftBottom = t.LeftBottom,
                                RightBottom = t.RightBottom,

                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                RightTop = new Vector2(t.RightTop.X - x, t.RightTop.Y - y),
                            };
                        }

                    default:
                        {
                            float x = vx * scale / 2f;
                            float y = vy * scale / 2f;

                            return new Quadrilateral
                            {
                                LeftTop = new Vector2(t.LeftTop.X - x, t.LeftTop.Y - y),
                                RightTop = new Vector2(t.RightTop.X - x, t.RightTop.Y - y),

                                LeftBottom = new Vector2(t.LeftBottom.X + x, t.LeftBottom.Y + y),
                                RightBottom = new Vector2(t.RightBottom.X + x, t.RightBottom.Y + y),
                            };
                        }
                }
            }
        }

        private Quadrilateral Reskew(Quadrilateral t, BoxMode m, float a, float min, float max)
        {
            this.K = new IndicatorSkew(hr, t, m, a, min, max);

            float hx2 = hx / 2;
            float hy2 = hy / 2;

            switch (m)
            {
                case BoxMode.LeftTop:
                case BoxMode.CenterTop:
                case BoxMode.RightTop:
                    return new Quadrilateral
                    {
                        LeftTop = t.LeftTop,
                        RightTop = t.RightTop,
                        RightBottom = new Vector2(this.K.fx0 + hx2, this.K.fy0 + hy2),
                        LeftBottom = new Vector2(this.K.fx0 - hx2, this.K.fy0 - hy2),
                    };
                case BoxMode.LeftBottom:
                case BoxMode.CenterBottom:
                case BoxMode.RightBottom:
                    return new Quadrilateral
                    {
                        LeftTop = new Vector2(this.K.fx1 - hx2, this.K.fy1 - hy2),
                        RightTop = new Vector2(this.K.fx1 + hx2, this.K.fy1 + hy2),
                        RightBottom = t.RightBottom,
                        LeftBottom = t.LeftBottom,
                    };
                case BoxMode.CenterLeft:
                case BoxMode.CenterRight:
                case BoxMode.Center:
                    return new Quadrilateral
                    {
                        LeftTop = new Vector2(this.K.fx1 - hx2, this.K.fy1 - hy2),
                        RightTop = new Vector2(this.K.fx1 + hx2, this.K.fy1 + hy2),
                        RightBottom = new Vector2(this.K.fx0 + hx2, this.K.fy0 + hy2),
                        LeftBottom = new Vector2(this.K.fx0 - hx2, this.K.fy0 - hy2),
                    };
                default:
                    return t;
            }
        }
        #endregion
    }
}