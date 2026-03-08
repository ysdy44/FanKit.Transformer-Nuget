using FanKit.Transformer.Input;
using System;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class CanvasHub3 : UserControl
    {
        public event RangeBaseValueChangedEventHandler ScaleChanged
        {
            remove => this.ScaleSlider.ValueChanged -= value;
            add => this.ScaleSlider.ValueChanged += value;
        }
        public event RangeBaseValueChangedEventHandler RotateChanged
        {
            remove => this.RotateSlider.ValueChanged -= value;
            add => this.RotateSlider.ValueChanged += value;
        }

        public CanvasHub3()
        {
            this.InitializeComponent();

            this.RotateSlider.Value = 0f;
            this.RotateSlider.Minimum = -90;
            this.RotateSlider.Maximum = 90;

            this.ScaleSlider.Value = 10f;
            this.ScaleSlider.Minimum = 0.32;
            this.ScaleSlider.Maximum = 32;
        }

        public void Update(CanvasTransformer3 canvas)
        {
            // Transform
            this.XTextBlock.Text = $"{(int)canvas.PositionX}";
            this.YTextBlock.Text = $"{(int)canvas.PositionY}";
            this.ScaleTextBlock.Text = $"{(int)(canvas.ScaleFactor * 100f)}%";
            this.RotateTextBlock.Text = $"{(int)(canvas.Rotation * 180f / System.Math.PI)}°";

            // Matrix
            Matrix3x2 m = canvas.Matrix;
            this.M11TextBlock.Text = $"{m.M11}"; this.M12TextBlock.Text = $"{m.M12}";
            this.M21TextBlock.Text = $"{m.M21}"; this.M22TextBlock.Text = $"{m.M22}";
            this.M31TextBlock.Text = $"{m.M31}"; this.M32TextBlock.Text = $"{m.M32}";
        }
    }
}