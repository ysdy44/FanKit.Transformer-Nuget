using FanKit.Transformer.Input;
using System;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class CanvasHub12 : UserControl
    {
        public event RangeBaseValueChangedEventHandler ScaleChanged
        {
            remove => this.ScaleSlider.ValueChanged -= value;
            add => this.ScaleSlider.ValueChanged += value;
        }

        public CanvasHub12()
        {
            this.InitializeComponent();

            this.ScaleSlider.Value = 10f;
            this.ScaleSlider.Minimum = 0.32;
            this.ScaleSlider.Maximum = 32;
        }

        public void Update(CanvasTransformer1 canvas)
        {
            // Transform
            this.XTextBlock.Text = $"{(int)canvas.PositionX}";
            this.YTextBlock.Text = $"{(int)canvas.PositionY}";
            this.ScaleTextBlock.Text = $"{(int)(canvas.ScaleFactor * 100f)}%";

            // Matrix
            Matrix3x2 m = canvas.Matrix;
            this.M11TextBlock.Text = $"{m.M11}";
            this.M22TextBlock.Text = $"{m.M22}";
            this.M31TextBlock.Text = $"{m.M31}"; this.M32TextBlock.Text = $"{m.M32}";
        }

        public void Update(CanvasTransformer2 canvas)
        {
            // Transform
            this.XTextBlock.Text = $"{(int)canvas.PositionX}";
            this.YTextBlock.Text = $"{(int)canvas.PositionY}";
            this.ScaleTextBlock.Text = $"{(int)(canvas.ScaleFactor * 100f)}%";

            // Matrix
            Matrix3x2 m = canvas.Matrix;
            this.M11TextBlock.Text = $"{m.M11}";
            this.M22TextBlock.Text = $"{m.M22}";
            this.M31TextBlock.Text = $"{m.M31}"; this.M32TextBlock.Text = $"{m.M32}";
        }
    }
}