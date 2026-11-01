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
    public sealed partial class ContainsPage : Page
    {
        Bounds Bounds;
        Triangle Triangle;
        Quadrilateral Quadrilateral;

        readonly Vector2[] BoundsPoints = new Vector2[4];
        readonly Vector2[] TrianglePoints = new Vector2[3];
        readonly Vector2[] QuadrilateralPoints = new Vector2[4];

        readonly static Color LeftStrokeColor = Colors.OrangeRed;
        readonly static Color LeftFillColor = Color.FromArgb(51, Colors.OrangeRed.R, Colors.OrangeRed.G, Colors.OrangeRed.B);

        readonly static Color RightStrokeColor = Colors.DeepSkyBlue;
        readonly static Color RightFillColor = Color.FromArgb(51, Colors.DeepSkyBlue.R, Colors.DeepSkyBlue.G, Colors.DeepSkyBlue.B);

        int ShapeType = 0;
        bool IsContaind;
        Vector2 Position;

        public ContainsPage()
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
                this.Draw(s, e.DrawingSession);
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

                switch (this.ShapeType)
                {
                    case 0:
                        this.IsContaind = this.Bounds.ContainsPoint(this.Position);
                        break;
                    case 1:
                        this.IsContaind = this.Triangle.ContainsPoint(this.Position);
                        break;
                    case 2:
                        this.IsContaind = this.Quadrilateral.ContainsPoint(this.Position);
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

                // 0:
                this.Bounds = new Bounds
                {
                    Left = centerX - 100,
                    Right = centerX + 100,
                    Top = centerY - 100,
                    Bottom = centerY + 100,
                };
                this.BoundsPoints[0] = new Vector2(this.Bounds.Left, this.Bounds.Top);
                this.BoundsPoints[1] = new Vector2(this.Bounds.Right, this.Bounds.Top);
                this.BoundsPoints[2] = new Vector2(this.Bounds.Right, this.Bounds.Bottom);
                this.BoundsPoints[3] = new Vector2(this.Bounds.Left, this.Bounds.Bottom);

                // 1:
                this.Triangle = new Triangle
                {
                    LeftTop = new Vector2(centerX - 100, centerY - 100),
                    RightTop = new Vector2(centerX + 134, centerY - 66),
                    LeftBottom = new Vector2(centerX - 66, centerY + 134),
                };
                this.TrianglePoints[0] = this.Triangle.LeftTop;
                this.TrianglePoints[1] = this.Triangle.RightTop;
                this.TrianglePoints[2] = this.Triangle.LeftBottom;

                // 2:
                this.Quadrilateral = new Quadrilateral
                {
                    LeftTop = new Vector2(centerX - 100 - 17, centerY - 100 - 17),
                    RightTop = new Vector2(centerX + 134 - 17, centerY - 66 - 17),
                    RightBottom = new Vector2(centerX + 168 - 17, centerY + 168 - 17),
                    LeftBottom = new Vector2(centerX - 66 - 17, centerY + 134 - 17),
                };
                this.QuadrilateralPoints[0] = this.Quadrilateral.LeftTop;
                this.QuadrilateralPoints[1] = this.Quadrilateral.RightTop;
                this.QuadrilateralPoints[2] = this.Quadrilateral.RightBottom;
                this.QuadrilateralPoints[3] = this.Quadrilateral.LeftBottom;
            };

            this.ShapeTypeComboBox.SelectionChanged += (s, e) =>
            {
                this.ShapeType = this.ShapeTypeComboBox.SelectedIndex;

                this.CanvasControl.Invalidate();
            };
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            CanvasGeometry geometry;
            switch (this.ShapeType)
            {
                case 0:
                    geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.BoundsPoints);
                    break;
                case 1:
                    geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.TrianglePoints);
                    break;
                case 2:
                    geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.QuadrilateralPoints);
                    break;
                default:
                    return;
            }

            if (this.IsContaind)
            {
                drawingSession.FillGeometry(geometry, LeftFillColor);
                drawingSession.DrawGeometry(geometry, LeftStrokeColor, 3f);
            }
            else
            {
                drawingSession.FillGeometry(geometry, RightFillColor);
                drawingSession.DrawGeometry(geometry, RightStrokeColor, 3f);
            }

            drawingSession.FillCircle(this.Position, 6f, LeftStrokeColor);
        }
    }
}