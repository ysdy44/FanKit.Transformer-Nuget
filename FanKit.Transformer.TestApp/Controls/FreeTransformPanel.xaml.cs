using System;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class FreeTransformPanel : UserControl
    {
        public EventHandler<QuadrilateralChannelKind> Apply;

        public float Value;

        public FreeTransformPanel()
        {
            this.InitializeComponent();
            this.LeftTopXButton.Click += delegate
            {
                if (float.TryParse(this.LeftTopXTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.LeftTopX);
            };
            this.LeftTopYButton.Click += delegate
            {
                if (float.TryParse(this.LeftTopYTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.LeftTopY);
            };

            this.RightTopXButton.Click += delegate
            {
                if (float.TryParse(this.RightTopXTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.RightTopX);
            };
            this.RightTopYButton.Click += delegate
            {
                if (float.TryParse(this.RightTopYTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.RightTopY);
            };

            this.LeftBottomXButton.Click += delegate
            {
                if (float.TryParse(this.LeftBottomXTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.LeftBottomX);
            };
            this.LeftBottomYButton.Click += delegate
            {
                if (float.TryParse(this.LeftBottomYTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.LeftBottomY);
            };

            this.RightBottomXButton.Click += delegate
            {
                if (float.TryParse(this.RightBottomXTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.RightBottomX);
            };
            this.RightBottomYButton.Click += delegate
            {
                if (float.TryParse(this.RightBottomYTextBox.Text, out this.Value))
                    this.Apply?.Invoke(this, QuadrilateralChannelKind.RightBottomY);
            };
        }

        public void UpdateAll(Quadrilateral quad)
        {
            this.LeftTopXTextBox.Text = quad.LeftTop.X.ToString("0.00");
            this.LeftTopYTextBox.Text = quad.LeftTop.Y.ToString("0.00");

            this.RightTopXTextBox.Text = quad.RightTop.X.ToString("0.00");
            this.RightTopYTextBox.Text = quad.RightTop.Y.ToString("0.00");

            this.LeftBottomXTextBox.Text = quad.LeftBottom.X.ToString("0.00");
            this.LeftBottomYTextBox.Text = quad.LeftBottom.Y.ToString("0.00");

            this.RightBottomXTextBox.Text = quad.RightBottom.X.ToString("0.00");
            this.RightBottomYTextBox.Text = quad.RightBottom.Y.ToString("0.00");
        }

        public void Update(Quadrilateral quad, QuadrilateralPointKind kind)
        {
            switch (kind)
            {
                case QuadrilateralPointKind.LeftTop:
                    this.LeftTopXTextBox.Text = quad.LeftTop.X.ToString("0.00");
                    this.LeftTopYTextBox.Text = quad.LeftTop.Y.ToString("0.00");
                    break;
                case QuadrilateralPointKind.RightTop:
                    this.RightTopXTextBox.Text = quad.RightTop.X.ToString("0.00");
                    this.RightTopYTextBox.Text = quad.RightTop.Y.ToString("0.00");
                    break;
                case QuadrilateralPointKind.LeftBottom:
                    this.LeftBottomXTextBox.Text = quad.LeftBottom.X.ToString("0.00");
                    this.LeftBottomYTextBox.Text = quad.LeftBottom.Y.ToString("0.00");
                    break;
                case QuadrilateralPointKind.RightBottom:
                    this.RightBottomXTextBox.Text = quad.RightBottom.X.ToString("0.00");
                    this.RightBottomYTextBox.Text = quad.RightBottom.Y.ToString("0.00");
                    break;
                default:
                    break;
            }
        }
    }
}