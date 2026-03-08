using FanKit.Transformer.Operators;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace FanKit.Transformer.TestApp
{
    public class CanvasOperator1 : ICanvasOperator1<PointerPointProperties>
    {
        public event OperatorSingleStartingEventHandler<PointerPointProperties> Single_Start = null;
        public event OperatorSingleEventHandler<PointerPointProperties> Single_Delta = null;
        public event OperatorSingleEventHandler<PointerPointProperties> Single_Complete = null;

        public event OperatorRightEventHandler Pointer_Over = null;

        public event OperatorWheelEventHandler Wheel_Changed = null;

        TouchState1 State;

        uint Id = 0;
        Point SP; // Starting Point
        Point P; // Point

        public readonly UIElement DestinationControl;

        public bool IsDisableTouch { get; set; } // Disable Single Events

        /// <summary>
        /// <inheritdoc cref="Windows.UI.Xaml.Input.Pointer.IsInContact"/>
        /// </summary>
        public bool IsInContact => this.State != default;

        public CanvasOperator1(UIElement destinationControl)
        {
            this.DestinationControl = destinationControl;

            this.DestinationControl.PointerPressed += (s, e) =>
            {
                switch (e.Pointer.PointerDeviceType)
                {
                    case PointerDeviceType.Touch:
                        if (this.IsDisableTouch)
                            break;

                        switch (this.State)
                        {
                            case TouchState1.None:
                                this.DestinationControl.CapturePointer(e.Pointer);
                                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                this.Id = pp.PointerId;
                                this.SP = this.P = pp.Position;

                                this.State = TouchState1.SingleFinger;
                                this.Single_Start?.Invoke(this.SP.X, this.SP.Y, pp.Properties);
                                break;
                            default:
                                break;
                        }
                        break;
                    case PointerDeviceType.Pen:
                        {
                            this.DestinationControl.CapturePointer(e.Pointer);
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id = pp.PointerId;
                            this.SP = this.P = pp.Position;

                            this.State = TouchState1.Pen;
                            this.Single_Start?.Invoke(this.SP.X, this.SP.Y, pp.Properties);
                        }
                        break;
                    case PointerDeviceType.Mouse:
                        {
                            this.DestinationControl.CapturePointer(e.Pointer);
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id = pp.PointerId;
                            this.SP = this.P = pp.Position;

                            if (pp.Properties.IsRightButtonPressed)
                            {
                                this.State = TouchState1.RightButton;
                                this.Single_Start?.Invoke(this.SP.X, this.SP.Y, pp.Properties);
                            }
                            else if (pp.Properties.IsMiddleButtonPressed)
                            {
                                this.State = TouchState1.MiddleButton;
                                this.Single_Start?.Invoke(this.SP.X, this.SP.Y, pp.Properties);
                            }
                            else
                            {
                                this.State = TouchState1.LeftButton;
                                this.Single_Start?.Invoke(this.SP.X, this.SP.Y, pp.Properties);
                            }
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
                    case TouchState1.None:
                        if (this.Pointer_Over != null)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;
                            this.Pointer_Over(this.P.X, this.P.Y);
                        }
                        break;
                    case TouchState1.SingleFinger:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Single_Delta?.Invoke(this.P.X, this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState1.Pen:
                    case TouchState1.LeftButton:
                    case TouchState1.MiddleButton:
                    case TouchState1.RightButton:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Single_Delta?.Invoke(this.P.X, this.P.Y, pp.Properties);
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
                    case TouchState1.None:
                        break;
                    case TouchState1.SingleFinger:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            this.Id = 0;

                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState1.None;
                            this.Single_Complete?.Invoke(this.P.X, this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState1.Pen:
                    case TouchState1.LeftButton:
                    case TouchState1.MiddleButton:
                    case TouchState1.RightButton:
                        {
                            this.Id = 0;

                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState1.None;
                            this.Single_Complete?.Invoke(this.P.X, this.P.Y, pp.Properties);
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
                    case TouchState1.None:
                        break;
                    case TouchState1.SingleFinger:
                    case TouchState1.Pen:
                    case TouchState1.LeftButton:
                    case TouchState1.MiddleButton:
                    case TouchState1.RightButton:
                        {
                            this.Id = 0;

                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState1.None;
                            this.Single_Complete?.Invoke(this.P.X, this.P.Y, pp.Properties);
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

                this.Wheel_Changed?.Invoke(this.SP.X, this.SP.Y, pp.Properties.MouseWheelDelta);
            };
        }
    }
}