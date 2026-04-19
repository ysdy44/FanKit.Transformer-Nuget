using FanKit.Transformer.Operators;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace FanKit.Transformer.TestApp
{
    public class CanvasOperator2 : ICanvasOperator2<PointerPointProperties>
    {
        public event SingleStartingEventHandler<PointerPointProperties> Single_Start = null;
        public event SingleEventHandler<PointerPointProperties> Single_Delta = null;
        public event SingleEventHandler<PointerPointProperties> Single_Complete = null;

        public event RightEventHandler Pointer_Over = null;

        public event RightEventHandler Right_Start = null;
        public event RightEventHandler Right_Delta = null;
        public event RightEventHandler Right_Complete = null;

        public event WheelEventHandler Wheel_Changed = null;

        TouchState2 State;

        // Single | Right | Wheel
        uint Id = 0;
        Point SP; // Starting Point
        Point P; // Point

        bool IsDisable; // Disable Single Events
        bool IsToRight; // Raise Right Events

        public readonly FrameworkElement DestinationControl;

        public bool IsDisableFlipX { get; set; } // Disable Flip X

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

        /// <summary>
        /// <inheritdoc cref="Windows.UI.Xaml.Input.Pointer.IsInContact"/>
        /// </summary>
        public bool IsInContact => this.State != default;

        public CanvasOperator2(FrameworkElement destinationControl)
        {
            this.DestinationControl = destinationControl;

            this.DestinationControl.PointerPressed += (s, e) =>
            {
                switch (e.Pointer.PointerDeviceType)
                {
                    case PointerDeviceType.Touch:
                        switch (this.State)
                        {
                            case TouchState2.None:
                                if (this.IsDisable)
                                    break;

                                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                this.Id = e.Pointer.PointerId;
                                this.SP = this.P = pp.Position;

                                if (this.IsToRight)
                                {
                                    this.State = TouchState2.SingleFingerToRightButton;
                                    this.Right_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y);
                                }
                                else
                                {
                                    this.State = TouchState2.SingleFinger;
                                    this.Single_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y, pp.Properties);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case PointerDeviceType.Pen:
                        switch (this.State)
                        {
                            case TouchState2.None:
                                this.DestinationControl.CapturePointer(e.Pointer);
                                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                this.Id = pp.PointerId;
                                this.SP = this.P = pp.Position;

                                this.State = TouchState2.Pen;
                                this.Single_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y, pp.Properties);
                                break;
                            default:
                                break;
                        }
                        break;
                    case PointerDeviceType.Mouse:
                        switch (this.State)
                        {
                            case TouchState2.None:
                                this.DestinationControl.CapturePointer(e.Pointer);
                                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                this.Id = pp.PointerId;
                                this.SP = this.P = pp.Position;

                                if (pp.Properties.IsRightButtonPressed)
                                {
                                    this.State = TouchState2.RightButton;
                                    this.Right_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y);
                                }
                                else if (pp.Properties.IsMiddleButtonPressed)
                                {
                                    this.State = TouchState2.MiddleButton;
                                    this.Right_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y);
                                }
                                else
                                {
                                    this.State = TouchState2.LeftButton;
                                    this.Single_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y, pp.Properties);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            };
            this.DestinationControl.PointerMoved += (s, e) =>
            {
                switch (this.State)
                {
                    case TouchState2.None:
                        if (this.Pointer_Over != null)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;
                            this.Pointer_Over(this.P.X, this.P.Y);
                        }
                        break;
                    case TouchState2.SingleFinger:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Single_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState2.SingleFingerToRightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Right_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState2.Pen:
                    case TouchState2.LeftButton:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Single_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState2.MiddleButton:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Right_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState2.RightButton:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Right_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    default:
                        break;
                }
            };
            this.DestinationControl.PointerReleased += (s, e) =>
            {
                switch (this.State)
                {
                    case TouchState2.None:
                        break;
                    case TouchState2.SingleFinger:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState2.SingleFingerToRightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Right_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState2.Pen:
                    case TouchState2.LeftButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState2.MiddleButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState2.RightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    default:
                        break;
                }
            };

            this.DestinationControl.PointerCaptureLost += (s, e) =>
            {
            };
            this.DestinationControl.PointerCanceled += (s, e) =>
            {
                switch (this.State)
                {
                    case TouchState2.None:
                        break;
                    case TouchState2.SingleFinger:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState2.SingleFingerToRightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState2.Pen:
                    case TouchState2.LeftButton:
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState2.MiddleButton:
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState2.RightButton:
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState2.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    default:
                        break;
                }
            };

            this.DestinationControl.PointerWheelChanged += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                this.Id = e.Pointer.PointerId;
                this.SP = this.P = pp.Position;

                this.Wheel_Changed?.Invoke(this.FlipX(this.SP.X), this.SP.Y, pp.Properties.MouseWheelDelta);
            };
        }

        private double FlipX(double x)
        {
            if (this.IsDisableFlipX)
            {
                return x;
            }

            switch (this.DestinationControl.FlowDirection)
            {
                case FlowDirection.RightToLeft:
                    return this.DestinationControl.ActualWidth - x;
                default:
                    return x;
            }
        }
    }
}