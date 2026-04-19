using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace FanKit.Transformer.TestApp
{
    public class CanvasOperator2 : Operators.CanvasOperator2<PointerPointProperties>
    {
        public readonly FrameworkElement DestinationControl;

        double Width;

        public bool IsDisableFlipX { get; set; } // Disable Flip X

        public CanvasOperator2(FrameworkElement destinationControl)
        {
            this.DestinationControl = destinationControl;

            this.DestinationControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.Width = e.NewSize.Width;
            };
            this.DestinationControl.PointerPressed += (s, e) =>
            {
                switch (e.Pointer.PointerDeviceType)
                {
                    case PointerDeviceType.Touch:
                        if (this.AllowPressedTouch())
                        {
                            if (this.CapturePointerPressedTouch())
                                this.DestinationControl.CapturePointer(e.Pointer);

                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.PressedTouch(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                        }
                        break;
                    case PointerDeviceType.Pen:
                        if (this.AllowPressedPen())
                        {
                            if (this.CapturePointerPressedPen())
                                this.DestinationControl.CapturePointer(e.Pointer);

                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            this.PressedPen(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                        }
                        break;
                    case PointerDeviceType.Mouse:
                        if (this.AllowPressedMouse())
                        {
                            if (this.CapturePointerPressedMouse())
                                this.DestinationControl.CapturePointer(e.Pointer);

                            PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                            if (pp.Properties.IsRightButtonPressed)
                                this.PressedRightButton(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                            else if (pp.Properties.IsMiddleButtonPressed)
                                this.PressedMiddleButton(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                            else
                                this.PressedLeftButton(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                        }
                        break;
                    default:
                        break;
                }
            };
            this.DestinationControl.PointerMoved += (s, e) =>
            {
                if (this.AllowMoved(e.Pointer.PointerId))
                {
                    PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                    this.Moved(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                }
            };
            this.DestinationControl.PointerReleased += (s, e) =>
            {
                if (this.AllowReleased(e.Pointer.PointerId))
                {
                    if (this.ReleasePointerReleased())
                        this.DestinationControl.ReleasePointerCaptures();

                    PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                    this.Released(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                }
            };

            this.DestinationControl.PointerCaptureLost += (s, e) =>
            {
            };
            this.DestinationControl.PointerCanceled += (s, e) =>
            {
                if (this.AllowCanceled(e.Pointer.PointerId))
                {
                    if (this.ReleasePointerCanceled())
                        this.DestinationControl.ReleasePointerCaptures();

                    PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                    this.Canceled(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties);
                }
            };

            this.DestinationControl.PointerWheelChanged += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.DestinationControl);
                this.WheelChanged(e.Pointer.PointerId, pp.Position.X, pp.Position.Y, pp.Properties.MouseWheelDelta);
            };
        }

        public override double FlipX(double x)
        {
            if (this.IsDisableFlipX)
            {
                return x;
            }

            switch (this.DestinationControl.FlowDirection)
            {
                case FlowDirection.RightToLeft:
                    return this.Width - x;
                default:
                    return x;
            }
        }
    }
}