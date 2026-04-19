namespace FanKit.Transformer.Operators
{
    public abstract class CanvasOperator2
    {
        public event SingleStartingEventHandler Single_Start = null;
        public event SingleEventHandler Single_Delta = null;
        public event SingleEventHandler Single_Complete = null;

        public event RightEventHandler Pointer_Over = null;

        public event RightEventHandler Right_Start = null;
        public event RightEventHandler Right_Delta = null;
        public event RightEventHandler Right_Complete = null;

        public event WheelEventHandler Wheel_Changed = null;

        TouchState2 State;

        // Single | Right | Wheel
        uint Id = 0;
        double SPX, SPY; // Starting Point
        double PX, PY; // Point

        bool IsDisable; // Disable Single Events
        bool IsToRight; // Raise Right Events

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
            if (this.IsDisable) //TODO: Fix
            {
                return false;
            }

            switch (this.State)
            {
                case TouchState2.None:
                    return true;
                default:
                    return false;
            }
        }

        public bool CapturePointerPressedTouch() => false;

        public void PressedTouch(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            if (this.IsToRight)
            {
                this.State = TouchState2.SingleFingerToRightButton;
                this.Right_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
            }
            else
            {
                this.State = TouchState2.SingleFinger;
                this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
            }
        }
        #endregion

        #region PressedPen
        public bool AllowPressedPen()
        {
            switch (this.State)
            {
                case TouchState2.None:
                    return true;
                default:
                    return false;
            }
        }

        public bool CapturePointerPressedPen() => true;

        public void PressedPen(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState2.Pen;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
        }
        #endregion

        #region PressedMouse
        public bool AllowPressedMouse()
        {
            switch (this.State)
            {
                case TouchState2.None:
                    return true;
                default:
                    return false;
            }
        }

        public bool CapturePointerPressedMouse() => true;

        public void PressedRightButton(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState2.RightButton;
            this.Right_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
        }

        public void PressedMiddleButton(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState2.MiddleButton;
            this.Right_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
        }

        public void PressedLeftButton(uint pointerId, double x, double y)
        {
            this.Id = pointerId;
            this.SPX = this.PX = x;
            this.SPY = this.PY = y;

            this.State = TouchState2.LeftButton;
            this.Single_Start?.Invoke(this.FlipX(this.SPX), this.SPY);
        }
        #endregion

        #region Moved
        public bool AllowMoved(uint pointerId)
        {
            switch (this.State)
            {
                case TouchState2.None:
                    return this.Pointer_Over != null;
                case TouchState2.SingleFinger:
                case TouchState2.SingleFingerToRightButton:
                    return this.Id == pointerId;
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                case TouchState2.MiddleButton:
                case TouchState2.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public void Moved(uint pointerId, double x, double y)
        {
            switch (this.State)
            {
                case TouchState2.None:
                    this.PX = x;
                    this.PY = y;

                    this.Pointer_Over(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.SingleFinger:
                    this.PX = x;
                    this.PY = y;

                    this.Single_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.SingleFingerToRightButton:
                    this.PX = x;
                    this.PY = y;

                    this.Right_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                    this.PX = x;
                    this.PY = y;

                    this.Single_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.MiddleButton:
                    this.PX = x;
                    this.PY = y;

                    this.Right_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.RightButton:
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
                case TouchState2.None:
                    return false;
                case TouchState2.SingleFinger:
                case TouchState2.SingleFingerToRightButton:
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                case TouchState2.MiddleButton:
                case TouchState2.RightButton:
                    return this.Id == pointerId;
                default:
                    return false;
            }
        }

        public bool ReleasePointerReleased()
        {
            switch (this.State)
            {
                case TouchState2.None:
                    return false;
                case TouchState2.SingleFinger:
                case TouchState2.SingleFingerToRightButton:
                    return false;
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                case TouchState2.MiddleButton:
                case TouchState2.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public void Released(uint pointerId, double x, double y)
        {
            switch (this.State)
            {
                case TouchState2.None:
                    break;
                case TouchState2.SingleFinger:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.SingleFingerToRightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Right_Delta?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.MiddleButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.RightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
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
                case TouchState2.None:
                    return false;
                case TouchState2.SingleFinger:
                    return true;
                case TouchState2.SingleFingerToRightButton:
                    return this.Id == pointerId;
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                case TouchState2.MiddleButton:
                case TouchState2.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public bool ReleasePointerCanceled()
        {
            switch (this.State)
            {
                case TouchState2.None:
                    return false;
                case TouchState2.SingleFinger:
                case TouchState2.SingleFingerToRightButton:
                    return false;
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                case TouchState2.MiddleButton:
                case TouchState2.RightButton:
                    return true;
                default:
                    return false;
            }
        }

        public void Canceled(uint pointerId, double x, double y)
        {
            switch (this.State)
            {
                case TouchState2.None:
                    break;
                case TouchState2.SingleFinger:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.SingleFingerToRightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.Pen:
                case TouchState2.LeftButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Single_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.MiddleButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
                    this.Right_Complete?.Invoke(this.FlipX(this.PX), this.PY);
                    break;
                case TouchState2.RightButton:
                    this.PX = x;
                    this.PY = y;

                    this.State = TouchState2.None;
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