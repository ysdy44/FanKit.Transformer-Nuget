using System;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class AnimatePad : Canvas
    {
        public event EventHandler<Vector2> Moved;
        public event RangeBaseValueChangedEventHandler ValueChanged
        {
            remove => this.Slider.ValueChanged -= value;
            add => this.Slider.ValueChanged += value;
        }

        double X;
        double Y;

        public double Value
        {
            get => this.Slider.Value;
            set => this.Slider.Value = value;
        }

        public AnimatePad()
        {
            this.InitializeComponent();
            this.Thumb.DragStarted += delegate
            {
                this.X = GetLeft(this.Canvas);
                this.Y = GetTop(this.Canvas);
            };
            this.Thumb.DragDelta += (s, e) =>
            {
                this.X += e.HorizontalChange;
                this.Y += e.VerticalChange;

                SetLeft(this.Canvas, X);
                SetTop(this.Canvas, Y);

                this.Moved?.Invoke(this, new Vector2
                {
                    X = (float)X,
                    Y = (float)Y,
                });
            };
            this.Thumb.DragCompleted += delegate
            {
            };
        }

        public void Reset(double cx, double cy)
        {
            this.X = cx - 100;
            this.Y = cy - 118;

            SetLeft(this.Canvas, this.X);
            SetTop(this.Canvas, this.Y);

            this.Slider.Value = 0;
            this.Moved?.Invoke(this, new Vector2
            {
                X = (float)this.X,
                Y = (float)this.Y,
            });
        }
    }
}