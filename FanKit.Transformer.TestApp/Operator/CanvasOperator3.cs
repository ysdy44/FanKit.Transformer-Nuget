using FanKit.Transformer.Operators;
using System.Diagnostics;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace FanKit.Transformer.TestApp
{
    public class CanvasOperator3 : ICanvasOperator3<PointerPointProperties>
    {
        public event OperatorSingleStartedEventHandler<PointerPointProperties> Single_Start = null;
        public event OperatorSingleEventHandler<PointerPointProperties> Single_Delta = null;
        public event OperatorSingleEventHandler<PointerPointProperties> Single_Complete = null;

        public event OperatorRightEventHandler Pointer_Over = null;

        public event OperatorRightEventHandler Right_Start = null;
        public event OperatorRightEventHandler Right_Delta = null;
        public event OperatorRightEventHandler Right_Complete = null;

        public event OperatorDoubleEventHandler Double_Start = null;
        public event OperatorDoubleEventHandler Double_Delta = null;
        public event OperatorDoubleEventHandler Double_Complete = null;

        public event OperatorWheelEventHandler Wheel_Changed = null;

        TouchState3 State;

        // Single | Right | Wheel
        uint Id = 0;
        Point SP; // Starting Point
        Point P; // Point

        // Double
        uint Id0 = 0;
        uint Id1 = 0;

        Point P0; // Point 0
        Point P1; // Point 1

        bool IsDisable; // Disable Single Events
        bool IsToRight; // Raise Right Events

        // Double
        readonly Stopwatch Stopwatch = new Stopwatch();

        public readonly FrameworkElement DestinationControl;

        public bool IsDisableFlipX { get; set; } // Disable Flip X

        /// <summary>
        /// Gets the total threshold time measured by the current instance, in timer ticks.
        /// </summary>
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

        /// <summary>
        /// <inheritdoc cref="Windows.UI.Xaml.Input.Pointer.IsInContact"/>
        /// </summary>
        public bool IsInContact => this.State != default;

        public CanvasOperator3(FrameworkElement destinationControl)
        {
            this.DestinationControl = destinationControl;

            this.DestinationControl.PointerPressed += (s, e) =>
            {
                switch (e.Pointer.PointerDeviceType)
                {
                    case PointerDeviceType.Touch:
                        switch (this.State)
                        {
                            case TouchState3.None:
                                {
                                    PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                    this.Id = this.Id0 = e.Pointer.PointerId;
                                    this.SP = this.P = this.P0 = pp.Position;

                                    if (this.IsDisable)
                                    {
                                        this.State = TouchState3.DoubleFingersOnly0;
                                    }
                                    else
                                    {
                                        this.Stopwatch.Start();
                                        this.State = TouchState3.Indeterminate;
                                    }
                                }
                                break;
                            case TouchState3.Indeterminate:
                                {
                                    PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                    this.Id1 = e.Pointer.PointerId;
                                    this.P1 = pp.Position;

                                    this.Stopwatch.Stop(); // Goto Double
                                    this.State = TouchState3.DoubleFingers;
                                    this.Double_Start?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                                }
                                break;
                            case TouchState3.DoubleFingersOnly0:
                                {
                                    PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                    this.Id1 = e.Pointer.PointerId;
                                    this.P1 = pp.Position;

                                    this.State = TouchState3.DoubleFingers;
                                    this.Double_Start?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                                }
                                break;
                            case TouchState3.DoubleFingersOnly1:
                                {
                                    PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                    this.Id0 = e.Pointer.PointerId;
                                    this.P0 = pp.Position;

                                    this.State = TouchState3.DoubleFingers;
                                    this.Double_Start?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case PointerDeviceType.Pen:
                        switch (this.State)
                        {
                            case TouchState3.None:
                                this.DestinationControl.CapturePointer(e.Pointer);
                                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                this.Id = pp.PointerId;
                                this.SP = this.P = pp.Position;

                                this.State = TouchState3.Pen;
                                this.Single_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y, this.P.X, this.P.Y, pp.Properties);
                                break;
                            default:
                                break;
                        }
                        break;
                    case PointerDeviceType.Mouse:
                        switch (this.State)
                        {
                            case TouchState3.None:
                                this.DestinationControl.CapturePointer(e.Pointer);
                                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                                this.Id = pp.PointerId;
                                this.SP = this.P = pp.Position;

                                if (pp.Properties.IsRightButtonPressed)
                                {
                                    this.State = TouchState3.RightButton;
                                    this.Right_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y);
                                }
                                else if (pp.Properties.IsMiddleButtonPressed)
                                {
                                    this.State = TouchState3.MiddleButton;
                                    this.Right_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y);
                                }
                                else
                                {
                                    this.State = TouchState3.LeftButton;
                                    this.Single_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y, this.P.X, this.P.Y, pp.Properties);
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
                    case TouchState3.None:
                        if (this.Pointer_Over != null)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;
                            this.Pointer_Over(this.P.X, this.P.Y);
                        }
                        break;
                    case TouchState3.Indeterminate:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            if (this.Stopwatch.ElapsedTicks > this.ThresholdTicks)
                            {
                                if (this.IsToRight)
                                {
                                    this.Stopwatch.Stop(); // Goto Right
                                    this.State = TouchState3.SingleFingerToRightButton;
                                    this.Right_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y);
                                }
                                else
                                {
                                    this.Stopwatch.Stop(); // Goto Single
                                    this.State = TouchState3.SingleFinger;
                                    this.Single_Start?.Invoke(this.FlipX(this.SP.X), this.SP.Y, this.P.X, this.P.Y, pp.Properties);
                                }
                            }
                        }
                        break;
                    case TouchState3.SingleFinger:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Single_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState3.SingleFingerToRightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Right_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState3.DoubleFingers:
                        if (this.Id0 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P0 = pp.Position;

                            this.Double_Delta?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        else if (this.Id1 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P1 = pp.Position;

                            this.Double_Delta?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        break;
                    case TouchState3.DoubleFingersOnly0:
                        if (this.Id0 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P0 = pp.Position;
                        }
                        break;
                    case TouchState3.DoubleFingersOnly1:
                        if (this.Id1 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P1 = pp.Position;
                        }
                        break;
                    case TouchState3.Pen:
                    case TouchState3.LeftButton:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Single_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState3.MiddleButton:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.Right_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState3.RightButton:
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
                    case TouchState3.None:
                        break;
                    case TouchState3.Indeterminate:
                        this.Id0 = 0;
                        this.Id1 = 0;

                        this.Stopwatch.Stop();
                        this.State = TouchState3.None;
                        break;
                    case TouchState3.SingleFinger:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState3.SingleFingerToRightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Right_Delta?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState3.DoubleFingers:
                        if (this.Id0 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id0 = 0;
                            this.P0 = pp.Position;

                            this.State = this.Id1 == 0 ? TouchState3.None : TouchState3.DoubleFingersOnly1;
                            this.Double_Complete?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        else if (this.Id1 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id1 = 0;
                            this.P1 = pp.Position;

                            this.State = this.Id0 == 0 ? TouchState3.None : TouchState3.DoubleFingersOnly0;
                            this.Double_Complete?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        break;
                    case TouchState3.DoubleFingersOnly0:
                        if (this.Id0 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id0 = 0;
                            this.P0 = pp.Position;

                            this.State = TouchState3.None;
                            this.Double_Complete?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        break;
                    case TouchState3.DoubleFingersOnly1:
                        if (this.Id1 == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id1 = 0;
                            this.P1 = pp.Position;

                            this.State = TouchState3.None;
                            this.Double_Complete?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        break;
                    case TouchState3.Pen:
                    case TouchState3.LeftButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState3.MiddleButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState3.RightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
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
                    case TouchState3.None:
                        break;
                    case TouchState3.Indeterminate:
                        this.Id0 = 0;
                        this.Id1 = 0;

                        this.Stopwatch.Stop();
                        this.State = TouchState3.None;
                        break;
                    case TouchState3.SingleFinger:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState3.SingleFingerToRightButton:
                        if (this.Id == e.Pointer.PointerId)
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState3.DoubleFingers:
                        if (this.Id0 == e.Pointer.PointerId)
                        {
                            goto case TouchState3.DoubleFingersOnly0;
                        }
                        else if (this.Id1 == e.Pointer.PointerId)
                        {
                            goto case TouchState3.DoubleFingersOnly1;
                        }
                        else
                        {
                            this.Id0 = 0;
                            this.Id1 = 0;

                            this.State = TouchState3.None;
                            this.Double_Complete?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        break;
                    case TouchState3.DoubleFingersOnly0:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id0 = 0;
                            this.P0 = pp.Position;

                            this.State = TouchState3.None;
                            this.Double_Complete?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        break;
                    case TouchState3.DoubleFingersOnly1:
                        {
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.Id1 = 0;
                            this.P1 = pp.Position;

                            this.State = TouchState3.None;
                            this.Double_Complete?.Invoke(this.FlipX(this.P0.X), this.P0.Y, this.P1.X, this.P1.Y);
                        }
                        break;
                    case TouchState3.Pen:
                    case TouchState3.LeftButton:
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Single_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y, pp.Properties);
                        }
                        break;
                    case TouchState3.MiddleButton:
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
                            this.Right_Complete?.Invoke(this.FlipX(this.P.X), this.P.Y);
                        }
                        break;
                    case TouchState3.RightButton:
                        {
                            this.DestinationControl.ReleasePointerCaptures();
                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.P = pp.Position;

                            this.State = TouchState3.None;
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