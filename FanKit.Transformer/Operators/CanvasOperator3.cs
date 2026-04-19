using System.Diagnostics;

namespace FanKit.Transformer.Operators
{
    public abstract class CanvasOperator3
    {
        public event SingleStartedEventHandler Single_Start = null;
        public event SingleEventHandler Single_Delta = null;
        public event SingleEventHandler Single_Complete = null;

        public event RightEventHandler Pointer_Over = null;

        public event RightEventHandler Right_Start = null;
        public event RightEventHandler Right_Delta = null;
        public event RightEventHandler Right_Complete = null;

        public event DoubleEventHandler Double_Start = null;
        public event DoubleEventHandler Double_Delta = null;
        public event DoubleEventHandler Double_Complete = null;

        public event WheelEventHandler Wheel_Changed = null;

        TouchState3 State;

        // Single | Right | Wheel
        uint Id = 0;
        double SPX, SPY; // Starting Point
        double PX, PY; // Point

        // Double
        uint Id0 = 0;
        uint Id1 = 0;

        double P0X, P0Y; // Point 0
        double P1X, P1Y; // Point 1

        bool IsDisable; // Disable Single Events
        bool IsToRight; // Raise Right Events

        // Double
        readonly Stopwatch Stopwatch = new Stopwatch();

        public long ThresholdTicks { get; set; } = 100 * 10000;

        public TouchMode TouchMode
        {
            get
            {
                if (this.IsDisable)
                    return TouchMode.Disable;
                else if (this.IsToRight)
                    return TouchMode.RightButton;
                else
                    return TouchMode.SingleFinger;
            }
            set
            {
                switch (value)
                {
                    case TouchMode.Disable:
                        this.IsDisable = true;
                        this.IsToRight = false;
                        break;
                    case TouchMode.SingleFinger:
                        this.IsDisable = false;
                        this.IsToRight = false;
                        break;
                    case TouchMode.RightButton:
                        this.IsDisable = false;
                        this.IsToRight = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public bool IsInContact => this.State != default;

        public abstract double FlipX(double x);

        #region PressedTouch
        public bool AllowPressedTouch()
        {
            switch (this.State)
            {
                case TouchState3.None:
                case TouchState3.Indeterminate:
                case TouchState3.DoubleFingersOnly0:
                case TouchState3.DoubleFingersOnly1:
                    return true;
                default:
                    return false;
            }
        }

        public bool CapturePointerPressedTouch() => false;

        public void PressedTouch(uint pointerId, double x, double y)
        {
            switch (this.State)
            {
                case TouchState3.None:
                    this.Id = this.Id0 = pointerId;
                    this.SPX = this.PX = this.P0X = x;
                    this.SPY = this.PY = this.P0Y = y;

                    if (this.IsDisable)
                    {
                        this.State = TouchState3.DoubleFingersOnly0;
                    }
                    else
                    {
                        this.Stopwatch.Start();
                        this.State = TouchState3.Indeterminate;
                    }
                    break;
                case TouchState3.Indeterminate:
                    this.Id1 = pointerId;
                    this.P1X = x;
                    this.P1Y = y;

                    this.Stopwatch.Stop(); // Goto Double
                    this.State = TouchState3.DoubleFingers;
                    this.Double_Start?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    break;
                case TouchState3.DoubleFingersOnly0:
                    this.Id1 = pointerId;
                    this.P1X = x;
                    this.P1Y = y;

                    this.State = TouchState3.DoubleFingers;
                    this.Double_Start?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    break;
                case TouchState3.DoubleFingersOnly1:
                    this.Id0 = pointerId;
                    this.P0X = x;
                    this.P0Y = y;

                    this.State = TouchState3.DoubleFingers;
                    this.Double_Start?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region PressedPen
        public bool AllowPressedPen()
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return true;
                default:
                    return false;
            }
        }

        public bool CapturePointerPressedPen()
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return true;
                default:
                    return false;
            }
        }

        public void PressedPen(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState3.Pen;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, this.PX, this.PY);
        }
        #endregion

        #region PressedMouse
        public bool AllowPressedMouse()
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return true;
                default:
                    return false;
            }
        }

        public bool CapturePointerPressedMouse()
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return true;
                default:
                    return false;
            }
        }

        public void PressedRightButton(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState3.RightButton;
            this.Right_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
        }

        public void PressedMiddleButton(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState3.MiddleButton;
            this.Right_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
        }

        public void PressedLeftButton(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState3.LeftButton;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, this.PX, this.PY);
        }
        #endregion

        #region Moved
        public bool AllowMoved(uint pointerId)
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return this.Pointer_Over != null;
                case TouchState3.Indeterminate:
                case TouchState3.SingleFinger:
                case TouchState3.SingleFingerToRightButton:
                    return this.Id == pointerId;
                case TouchState3.DoubleFingers:
                    return this.Id0 == pointerId || this.Id1 == pointerId;
                case TouchState3.DoubleFingersOnly0:
                    return this.Id0 == pointerId;
                case TouchState3.DoubleFingersOnly1:
                    return this.Id1 == pointerId;
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                case TouchState3.MiddleButton:
                case TouchState3.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public void Moved(uint pointerId, double x, double y)
        {
            switch (this.State)
            {
                case TouchState3.None:
                    this.PX = x;
                    this.PY = y;

                    this.Pointer_Over(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.Indeterminate:
                    this.PX = x;
                    this.PY = y;

                    if (this.Stopwatch.ElapsedTicks > this.ThresholdTicks)
                    {
                        if (this.IsToRight)
                        {
                            this.Stopwatch.Stop(); // Goto Right
                            this.State = TouchState3.SingleFingerToRightButton;
                            this.Right_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
                        }
                        else
                        {
                            this.Stopwatch.Stop(); // Goto Single
                            this.State = TouchState3.SingleFinger;
                            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, this.PX, this.PY);
                        }
                    }
                    break;
                case TouchState3.SingleFinger:
                    this.PX = x;
                    this.PY = y;

                    this.Single_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.SingleFingerToRightButton:
                    this.PX = x;
                    this.PY = y;

                    this.Right_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.DoubleFingers:
                    if (this.Id0 == pointerId)
                    {
                        this.P0X = x;
                        this.P0Y = y;

                        this.Double_Delta?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    }
                    else if (this.Id1 == pointerId)
                    {
                        this.P1X = x;
                        this.P1Y = y;

                        this.Double_Delta?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    }
                    break;
                case TouchState3.DoubleFingersOnly0:
                    this.P0X = x;
                    this.P0Y = y;
                    break;
                case TouchState3.DoubleFingersOnly1:
                    this.P1X = x;
                    this.P1Y = y;
                    break;
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                    this.PX = x;
                    this.PY = y;

                    this.Single_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.MiddleButton:
                    this.PX = x;
                    this.PY = y;

                    this.Right_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.RightButton:
                    this.PX = x;
                    this.PY = y;

                    this.Right_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Released
        public bool AllowReleased(uint pointerId)
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return false;
                case TouchState3.Indeterminate:
                    return true;
                case TouchState3.SingleFinger:
                case TouchState3.SingleFingerToRightButton:
                    return this.Id == pointerId;
                case TouchState3.DoubleFingers:
                    return this.Id0 == pointerId || this.Id1 == pointerId;
                case TouchState3.DoubleFingersOnly0:
                    return this.Id0 == pointerId;
                case TouchState3.DoubleFingersOnly1:
                    return this.Id1 == pointerId;
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                case TouchState3.MiddleButton:
                case TouchState3.RightButton:
                    return this.Id == pointerId;
                default:
                    return false;
            }
        }

        public bool ReleasePointerReleased()
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return false;
                case TouchState3.Indeterminate:
                case TouchState3.SingleFinger:
                case TouchState3.SingleFingerToRightButton:
                case TouchState3.DoubleFingers:
                case TouchState3.DoubleFingersOnly0:
                case TouchState3.DoubleFingersOnly1:
                    return false;
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                case TouchState3.MiddleButton:
                case TouchState3.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public void Released(uint pointerId, double x, double y)
        {
            switch (this.State)
            {
                case TouchState3.None:
                    break;
                case TouchState3.Indeterminate:
                    this.Id0 = 0;
                    this.Id1 = 0;

                    this.Stopwatch.Stop();
                    this.State = TouchState3.None;
                    break;
                case TouchState3.SingleFinger:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.SingleFingerToRightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Right_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.DoubleFingers:
                    if (this.Id0 == pointerId)
                    {
                        this.Id0 = 0;
                        this.P0X = x;
                        this.P0Y = y;

                        this.State = this.Id1 == 0 ? TouchState3.None : TouchState3.DoubleFingersOnly1;
                        this.Double_Complete?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    }
                    else if (this.Id1 == pointerId)
                    {
                        this.Id1 = 0;
                        this.P1X = x;
                        this.P1Y = y;

                        this.State = this.Id0 == 0 ? TouchState3.None : TouchState3.DoubleFingersOnly0;
                        this.Double_Complete?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    }
                    break;
                case TouchState3.DoubleFingersOnly0:
                    this.Id0 = 0;
                    this.P0X = x;
                    this.P0Y = y;

                    this.State = TouchState3.None;
                    this.Double_Complete?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    break;
                case TouchState3.DoubleFingersOnly1:
                    this.Id1 = 0;
                    this.P1X = x;
                    this.P1Y = y;

                    this.State = TouchState3.None;
                    this.Double_Complete?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    break;
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.MiddleButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.RightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Canceled
        public bool AllowCanceled(uint pointerId)
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return false;
                case TouchState3.Indeterminate:
                case TouchState3.SingleFinger:
                    return true;
                case TouchState3.SingleFingerToRightButton:
                    return this.Id == pointerId;
                case TouchState3.DoubleFingers:
                case TouchState3.DoubleFingersOnly0:
                case TouchState3.DoubleFingersOnly1:
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                case TouchState3.MiddleButton:
                case TouchState3.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public bool ReleasePointerCanceled()
        {
            switch (this.State)
            {
                case TouchState3.None:
                    return false;
                case TouchState3.Indeterminate:
                case TouchState3.SingleFinger:
                case TouchState3.SingleFingerToRightButton:
                case TouchState3.DoubleFingers:
                case TouchState3.DoubleFingersOnly0:
                case TouchState3.DoubleFingersOnly1:
                    return false;
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                case TouchState3.MiddleButton:
                case TouchState3.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public void Canceled(uint pointerId, double x, double y)
        {
            switch (this.State)
            {
                case TouchState3.None:
                    break;
                case TouchState3.Indeterminate:
                    this.Id0 = 0;
                    this.Id1 = 0;

                    this.Stopwatch.Stop();
                    this.State = TouchState3.None;
                    break;
                case TouchState3.SingleFinger:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.SingleFingerToRightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.DoubleFingers:
                    if (this.Id0 == pointerId)
                    {
                        goto case TouchState3.DoubleFingersOnly0;
                    }
                    else if (this.Id1 == pointerId)
                    {
                        goto case TouchState3.DoubleFingersOnly1;
                    }
                    else
                    {
                        this.Id0 = 0;
                        this.Id1 = 0;

                        this.State = TouchState3.None;
                        this.Double_Complete?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    }
                    break;
                case TouchState3.DoubleFingersOnly0:
                    this.Id0 = 0;
                    this.P0X = x;
                    this.P0Y = y;

                    this.State = TouchState3.None;
                    this.Double_Complete?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    break;
                case TouchState3.DoubleFingersOnly1:
                    this.Id1 = 0;
                    this.P1X = x;
                    this.P1Y = y;

                    this.State = TouchState3.None;
                    this.Double_Complete?.Invoke(this.FlipX(this.P0X), this.P0Y, this.P1X, this.P1Y);
                    break;
                case TouchState3.Pen:
                case TouchState3.LeftButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.MiddleButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState3.RightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState3.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region WheelChanged
        public void WheelChanged(uint pointerId, double x, double y, int wheelDelta)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.Wheel_Changed?.Invoke(this.FlipX(this.SPX), this.SPY, wheelDelta);
        }
        #endregion
    }
}