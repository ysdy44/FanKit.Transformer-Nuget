using FanKit.Transformer.Input;
using System;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class CanvasHub0 : UserControl
    {
        public CanvasHub0()
        {
            this.InitializeComponent();
        }

        public void Update(CanvasTransformer0 canvas)
        {
            // Transform
            this.XTextBlock.Text = $"{(int)canvas.PositionX}";
            this.YTextBlock.Text = $"{(int)canvas.PositionY}";

            // Matrix
            Matrix3x2 m = canvas.Matrix;
            this.M31TextBlock.Text = $"{m.M31}"; this.M32TextBlock.Text = $"{m.M32}";
        }
    }
}