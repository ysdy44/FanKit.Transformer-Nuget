using FanKit.Transformer.Mathematics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class CombineBoundsPage : Page
    {
        // Source
        Rect Source;
        readonly static Color LeftStrokeColor = Colors.OrangeRed;
        readonly static Color LeftFillColor = Color.FromArgb(51, Colors.OrangeRed.R, Colors.OrangeRed.G, Colors.OrangeRed.B);

        // Destination
        Bounds Dest1;
        Bounds Dest2;
        readonly static Color RightStrokeColor = Colors.DeepSkyBlue;
        //readonly static Color RightFillColor = Color.FromArgb(51, Colors.DeepSkyBlue.R, Colors.DeepSkyBlue.G, Colors.DeepSkyBlue.B);

        Vector2 Position;

        public CombineBoundsPage()
        {
            this.InitializeComponent();
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
            };

            this.CanvasControl.Draw += (s, e) =>
            {
                this.Draw(e.DrawingSession);
            };
            this.CanvasControl.PointerMoved += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.CanvasControl);

                double x;
                switch (this.CanvasControl.FlowDirection)
                {
                    case FlowDirection.RightToLeft:
                        x = this.CanvasControl.ActualWidth - pp.Position.X;
                        break;
                    default:
                        x = pp.Position.X;
                        break;
                }

                this.Position = new Vector2
                {
                    X = (float)x,
                    Y = (float)pp.Position.Y,
                };

                switch (this.ComboBox.SelectedIndex)
                {
                    case 0:
                        this.Dest2 = Bounds.Union(this.Dest1, this.Position);
                        break;
                    case 1:
                        this.Dest2 = Bounds.Intersect(this.Dest1, this.Position);
                        break;
                    default:
                        break;
                }

                this.CanvasControl.Invalidate();
            };
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                float centerX = width / 2;
                float centerY = height / 2;

                this.Source = new Rect
                {
                    X = centerX - 100,
                    Y = centerY - 100,
                    Width = 200,
                    Height = 200,
                };

                this.Dest1 = this.Dest2 = new Bounds
                {
                    Left = centerX - 100,
                    Top = centerY - 100,
                    Right = centerX + 100,
                    Bottom = centerY + 100,
                };
            };
        }

        private void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.FillRectangle(this.Source, LeftFillColor);
            drawingSession.DrawRectangle(this.Source, LeftStrokeColor, 3f);

            drawingSession.DrawLine(this.Dest2.Left, this.Dest2.Top, this.Dest2.Right, this.Dest2.Top, RightStrokeColor, 3f);
            drawingSession.DrawLine(this.Dest2.Right, this.Dest2.Top, this.Dest2.Right, this.Dest2.Bottom, RightStrokeColor, 3f);
            drawingSession.DrawLine(this.Dest2.Right, this.Dest2.Bottom, this.Dest2.Left, this.Dest2.Bottom, RightStrokeColor, 3f);
            drawingSession.DrawLine(this.Dest2.Left, this.Dest2.Bottom, this.Dest2.Left, this.Dest2.Top, RightStrokeColor, 3f);

            drawingSession.FillCircle(this.Position, 6f, LeftStrokeColor);
        }
    }
}