namespace FanKit.Transformer.Operators
{
    public abstract class CanvasOperator1<T>
    {
        public event SingleStartingEventHandler<T> Single_Start = null;
        public event SingleEventHandler<T> Single_Delta = null;
        public event SingleEventHandler<T> Single_Complete = null;

        public event RightEventHandler Pointer_Over = null;

        public event WheelEventHandler Wheel_Changed = null;

        TouchState1 State;

        uint Id = 0;
        double SPX, SPY; // Starting Point
        double PX, PY; // Point

        public bool IsDisableTouch { get; set; } // Disable Single Events

        public bool IsInContact => this.State != default;

        public abstract double FlipX(double x);

        #region PressedTouch
        public bool AllowPressedTouch()
        {
            if (this.IsDisableTouch)
                return false;

            switch (this.State)
            {
                case TouchState1.None:
                    return true;
                default:
                    return false;
            }
        }

        public bool CapturePointerPressedTouch() => true;

        public void PressedTouch(uint pointerId, double x, double y, T properties)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState1.SingleFinger;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, properties);
        }
        #endregion

        #region PressedPen
        public bool AllowPressedPen() => true;

        public bool CapturePointerPressedPen() => true;

        public void PressedPen(uint pointerId, double x, double y, T properties)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState1.Pen;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, properties);
        }
        #endregion

        #region PressedMouse
        public bool AllowPressedMouse() => true;

        public bool CapturePointerPressedMouse() => true;

        public void PressedRightButton(uint pointerId, double x, double y, T properties)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState1.RightButton;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, properties);
        }

        public void PressedMiddleButton(uint pointerId, double x, double y, T properties)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState1.MiddleButton;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, properties);
        }

        public void PressedLeftButton(uint pointerId, double x, double y, T properties)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState1.LeftButton;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY, properties);
        }
        #endregion

        #region Moved
        public bool AllowMoved(uint pointerId)
        {
            switch (this.State)
            {
                case TouchState1.None:
                    return this.Pointer_Over != null;
                case TouchState1.SingleFinger:
                    return this.Id == pointerId;
                case TouchState1.Pen:
                case TouchState1.LeftButton:
                case TouchState1.MiddleButton:
                case TouchState1.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public void Moved(uint pointerId, double x, double y, T properties)
        {
            switch (this.State)
            {
                case TouchState1.None:
                    this.PX = x;
                    this.PY = y;

                    this.Pointer_Over(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState1.SingleFinger:
                    this.PX = x;
                    this.PY = y;

                    this.Single_Delta?.Invoke(this.FlipX(this.PX), this.PY, properties);
                    break;
                case TouchState1.Pen:
                case TouchState1.LeftButton:
                case TouchState1.MiddleButton:
                case TouchState1.RightButton:
                    this.PX = x;
                    this.PY = y;

                    this.Single_Delta?.Invoke(this.FlipX(this.PX), this.PY, properties);
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
                case TouchState1.None:
                    return false;
                case TouchState1.SingleFinger:
                    return this.Id == pointerId;
                case TouchState1.Pen:
                case TouchState1.LeftButton:
                case TouchState1.MiddleButton:
                case TouchState1.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public bool ReleasePointerReleased() => true;

        public void Released(uint pointerId, double x, double y, T properties)
        {
            switch (this.State)
            {
                case TouchState1.None:
                    break;
                case TouchState1.SingleFinger:
                    this.Id = 0;
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState1.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY, properties);
                    break;
                case TouchState1.Pen:
                case TouchState1.LeftButton:
                case TouchState1.MiddleButton:
                case TouchState1.RightButton:
                    this.Id = 0;
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState1.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY, properties);
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
                case TouchState1.None:
                    return false;
                case TouchState1.SingleFinger:
                case TouchState1.Pen:
                case TouchState1.LeftButton:
                case TouchState1.MiddleButton:
                case TouchState1.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public bool ReleasePointerCanceled() => true;

        public void Canceled(uint pointerId, double x, double y, T properties)
        {
            this.Id = 0;
            this.PX = x;
            this.PY = y;

            this.State = TouchState1.None;
            this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY, properties);
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