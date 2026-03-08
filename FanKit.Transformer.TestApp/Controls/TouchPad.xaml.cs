using FanKit.Transformer.Operators;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class TouchPad : Canvas
    {
        public event OperatorDoubleEventHandler Double_Start = null;
        public event OperatorDoubleEventHandler Double_Delta = null;
        public event OperatorDoubleEventHandler Double_Complete = null;

        public TouchPad()
        {
            this.InitializeComponent();
            this.ThumbCenter.DragStarted += delegate
            {
            };
            this.ThumbCenter.DragDelta += (s, e) =>
            {
                this.Line.X1 += e.HorizontalChange; this.Line.Y1 += e.VerticalChange;
                this.Line.X2 += e.HorizontalChange; this.Line.Y2 += e.VerticalChange;

                SetLeft(this.ThumbCenter, this.Line.X1 / 2 + this.Line.X2 / 2 - 12); SetTop(this.ThumbCenter, this.Line.Y1 / 2 + this.Line.Y2 / 2 - 12);
                SetLeft(this.Thumb1, this.Line.X1 - 18); SetTop(this.Thumb1, this.Line.Y1 - 18);
                SetLeft(this.Thumb2, this.Line.X2 - 18); SetTop(this.Thumb2, this.Line.Y2 - 18);
            };
            this.ThumbCenter.DragCompleted += delegate
            {
            };

            this.Thumb1.DragStarted += delegate
            {
                Double_Start?.Invoke(Line.X1, Line.Y1, Line.X2, Line.Y2);
            };
            this.Thumb1.DragDelta += (s, e) =>
            {
                this.Line.X1 += e.HorizontalChange; this.Line.Y1 += e.VerticalChange;
                this.Line.X2 -= e.HorizontalChange; this.Line.Y2 -= e.VerticalChange;

                SetLeft(this.Thumb1, this.Line.X1 - 18); SetTop(this.Thumb1, this.Line.Y1 - 18);
                SetLeft(this.Thumb2, this.Line.X2 - 18); SetTop(this.Thumb2, this.Line.Y2 - 18);
            };
            this.Thumb1.DragCompleted += delegate
            {
            };

            this.Thumb2.DragStarted += delegate
            {
                Double_Start?.Invoke(Line.X1, Line.Y1, Line.X2, Line.Y2);
            };
            this.Thumb2.DragDelta += (s, e) =>
            {
                this.Line.X1 -= e.HorizontalChange; this.Line.Y1 -= e.VerticalChange;
                this.Line.X2 += e.HorizontalChange; this.Line.Y2 += e.VerticalChange;

                SetLeft(this.Thumb1, this.Line.X1 - 18); SetTop(this.Thumb1, this.Line.Y1 - 18);
                SetLeft(this.Thumb2, this.Line.X2 - 18); SetTop(this.Thumb2, this.Line.Y2 - 18);

                Double_Delta?.Invoke(this.Line.X1, this.Line.Y1, this.Line.X2, this.Line.Y2);
            };
            this.Thumb2.DragCompleted += delegate
            {
                Double_Complete?.Invoke(this.Line.X1, this.Line.Y1, this.Line.X2, this.Line.Y2);
            };
        }

        public void Reset(double cx, double cy)
        {
            Line.X1 = cx - 100;
            Line.Y1 = cy - 100;

            Line.X2 = cx + 100;
            Line.Y2 = cy + 100;

            SetLeft(this.ThumbCenter, cx - 12); SetTop(this.ThumbCenter, cy - 12);
            SetLeft(this.Thumb1, Line.X1 - 25); SetTop(this.Thumb1, Line.Y1 - 25);
            SetLeft(this.Thumb2, Line.X2 - 25); SetTop(this.Thumb2, Line.Y2 - 25);
        }

        public void Reset(double x1, double y1, double x2, double y2)
        {
            Line.X1 = x1;
            Line.Y1 = y1;

            Line.X2 = x2;
            Line.Y2 = y2;

            SetLeft(this.ThumbCenter, this.Line.X1 / 2 + this.Line.X2 / 2 - 12); SetTop(this.ThumbCenter, this.Line.Y1 / 2 + this.Line.Y2 / 2 - 12);
            SetLeft(this.Thumb1, this.Line.X1 - 18); SetTop(this.Thumb1, this.Line.Y1 - 18);
            SetLeft(this.Thumb2, this.Line.X2 - 18); SetTop(this.Thumb2, this.Line.Y2 - 18);
        }
    }
}