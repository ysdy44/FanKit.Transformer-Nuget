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
    public sealed partial class ExtendPage : Page
    {
        // Source
        Bounds BoundsSource;
        Triangle TriangleSource;
        Quadrilateral QuadrilateralSource;

        readonly Vector2[] BoundsLeftPoints = new Vector2[4];
        readonly Vector2[] TriangleLeftPoints = new Vector2[3];
        readonly Vector2[] QuadrilateralLeftPoints = new Vector2[4];

        readonly static Color LeftStrokeColor = Colors.OrangeRed;
        readonly static Color LeftFillColor = Color.FromArgb(51, Colors.OrangeRed.R, Colors.OrangeRed.G, Colors.OrangeRed.B);

        // Destination
        Bounds BoundsDest;
        Triangle TriangleDest;
        Quadrilateral QuadrilateralDest;

        readonly Vector2[] BoundsRightPoints = new Vector2[4];
        readonly Vector2[] TriangleRightPoints = new Vector2[3];
        readonly Vector2[] QuadrilateralRightPoints = new Vector2[4];

        readonly static Color RightStrokeColor = Colors.DeepSkyBlue;
        readonly static Color RightFillColor = Color.FromArgb(51, Colors.DeepSkyBlue.R, Colors.DeepSkyBlue.G, Colors.DeepSkyBlue.B);

        int ShapeType = 0;
        bool IsExtend = true;
        Vector2 Position;

        QuadrilateralPointKind BoundsKind = QuadrilateralPointKind.LeftTop;
        TrianglePointKind TriangleKind = TrianglePointKind.LeftTop;
        QuadrilateralPointKind QuadrilateralKind = QuadrilateralPointKind.LeftTop;

        public ExtendPage()
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
                        this.BoundsDest = this.BoundsSource.MovePoint(this.BoundsKind, this.Position);

                        this.BoundsRightPoints[0] = new Vector2(this.BoundsDest.Left, this.BoundsDest.Top);
                        this.BoundsRightPoints[1] = new Vector2(this.BoundsDest.Right, this.BoundsDest.Top);
                        this.BoundsRightPoints[2] = new Vector2(this.BoundsDest.Right, this.BoundsDest.Bottom);
                        this.BoundsRightPoints[3] = new Vector2(this.BoundsDest.Left, this.BoundsDest.Bottom);
                        break;
                    case 1:
                        this.TriangleDest = this.TriangleSource.MovePoint(this.TriangleKind, this.Position, this.IsExtend);

                        this.TriangleRightPoints[0] = this.TriangleDest.LeftTop;
                        this.TriangleRightPoints[1] = this.TriangleDest.RightTop;
                        this.TriangleRightPoints[2] = this.TriangleDest.LeftBottom;
                        break;
                    case 2:
                        this.QuadrilateralDest = this.QuadrilateralSource.MovePoint(this.QuadrilateralKind, this.Position, this.IsExtend);

                        this.QuadrilateralRightPoints[0] = this.QuadrilateralDest.LeftTop;
                        this.QuadrilateralRightPoints[1] = this.QuadrilateralDest.RightTop;
                        this.QuadrilateralRightPoints[2] = this.QuadrilateralDest.RightBottom;
                        this.QuadrilateralRightPoints[3] = this.QuadrilateralDest.LeftBottom;
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

                // 0
                this.BoundsSource = new Bounds
                {
                    Left = centerX - 100,
                    Right = centerX + 100,
                    Top = centerY - 100,
                    Bottom = centerY + 100,
                };
                this.BoundsLeftPoints[0] = new Vector2(this.BoundsSource.Left, this.BoundsSource.Top);
                this.BoundsLeftPoints[1] = new Vector2(this.BoundsSource.Right, this.BoundsSource.Top);
                this.BoundsLeftPoints[2] = new Vector2(this.BoundsSource.Right, this.BoundsSource.Bottom);
                this.BoundsLeftPoints[3] = new Vector2(this.BoundsSource.Left, this.BoundsSource.Bottom);

                this.BoundsDest = this.BoundsSource;
                this.BoundsRightPoints[0] = new Vector2(this.BoundsDest.Left, this.BoundsDest.Top);
                this.BoundsRightPoints[1] = new Vector2(this.BoundsDest.Right, this.BoundsDest.Top);
                this.BoundsRightPoints[2] = new Vector2(this.BoundsDest.Right, this.BoundsDest.Bottom);
                this.BoundsRightPoints[3] = new Vector2(this.BoundsDest.Left, this.BoundsDest.Bottom);

                // 1
                this.TriangleSource = new Triangle
                {
                    LeftTop = new Vector2(centerX - 100, centerY - 100),
                    RightTop = new Vector2(centerX + 134, centerY - 66),
                    LeftBottom = new Vector2(centerX - 66, centerY + 134),
                };
                this.TriangleLeftPoints[0] = this.TriangleSource.LeftTop;
                this.TriangleLeftPoints[1] = this.TriangleSource.RightTop;
                this.TriangleLeftPoints[2] = this.TriangleSource.LeftBottom;

                this.TriangleDest = this.TriangleSource;
                this.TriangleRightPoints[0] = this.TriangleDest.LeftTop;
                this.TriangleRightPoints[1] = this.TriangleDest.RightTop;
                this.TriangleRightPoints[2] = this.TriangleDest.LeftBottom;

                // 2
                this.QuadrilateralSource = new Quadrilateral
                {
                    LeftTop = new Vector2(centerX - 100 - 17, centerY - 100 - 17),
                    RightTop = new Vector2(centerX + 134 - 17, centerY - 66 - 17),
                    RightBottom = new Vector2(centerX + 168 - 17, centerY + 168 - 17),
                    LeftBottom = new Vector2(centerX - 66 - 17, centerY + 134 - 17),
                };
                this.QuadrilateralLeftPoints[0] = this.QuadrilateralSource.LeftTop;
                this.QuadrilateralLeftPoints[1] = this.QuadrilateralSource.RightTop;
                this.QuadrilateralLeftPoints[2] = this.QuadrilateralSource.RightBottom;
                this.QuadrilateralLeftPoints[3] = this.QuadrilateralSource.LeftBottom;

                this.QuadrilateralDest = this.QuadrilateralSource;
                this.QuadrilateralRightPoints[0] = this.QuadrilateralDest.LeftTop;
                this.QuadrilateralRightPoints[1] = this.QuadrilateralDest.RightTop;
                this.QuadrilateralRightPoints[2] = this.QuadrilateralDest.RightBottom;
                this.QuadrilateralRightPoints[3] = this.QuadrilateralDest.LeftBottom;
            };

            this.ShapeComboBox.SelectionChanged += (s, e) =>
            {
                this.ShapeType = this.ShapeComboBox.SelectedIndex;

                this.CanvasControl.Invalidate();
            };
            this.ExtendButton.Toggled += delegate
            {
                this.IsExtend = this.ExtendButton.IsOn;

                this.CanvasControl.Invalidate();
            };
            this.BoundsComboBox.SelectionChanged += (s, e) =>
            {
                switch (this.BoundsComboBox.SelectedIndex)
                {
                    case 0: this.BoundsKind = QuadrilateralPointKind.LeftTop; break;
                    case 1: this.BoundsKind = QuadrilateralPointKind.RightTop; break;
                    case 2: this.BoundsKind = QuadrilateralPointKind.RightBottom; break;
                    case 3: this.BoundsKind = QuadrilateralPointKind.LeftBottom; break;
                    default: break;
                }

                this.CanvasControl.Invalidate();
            };
            this.TriangleComboBox.SelectionChanged += (s, e) =>
            {
                switch (this.TriangleComboBox.SelectedIndex)
                {
                    case 0: this.TriangleKind = TrianglePointKind.LeftTop; break;
                    case 1: this.TriangleKind = TrianglePointKind.RightTop; break;
                    case 2: this.TriangleKind = TrianglePointKind.LeftBottom; break;
                    default: break;
                }

                this.CanvasControl.Invalidate();
            };
            this.QuadrilateralComboBox.SelectionChanged += (s, e) =>
            {
                switch (this.QuadrilateralComboBox.SelectedIndex)
                {
                    case 0: this.QuadrilateralKind = QuadrilateralPointKind.LeftTop; break;
                    case 1: this.QuadrilateralKind = QuadrilateralPointKind.RightTop; break;
                    case 2: this.QuadrilateralKind = QuadrilateralPointKind.RightBottom; break;
                    case 3: this.QuadrilateralKind = QuadrilateralPointKind.LeftBottom; break;
                    default: break;
                }

                this.CanvasControl.Invalidate();
            };
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            CanvasGeometry geometry0;

            switch (this.ShapeType)
            {
                case 0:
                    geometry0 = CanvasGeometry.CreatePolygon(resourceCreator, this.BoundsLeftPoints);
                    break;
                case 1:
                    geometry0 = CanvasGeometry.CreatePolygon(resourceCreator, this.TriangleLeftPoints);
                    break;
                case 2:
                    geometry0 = CanvasGeometry.CreatePolygon(resourceCreator, this.QuadrilateralLeftPoints);
                    break;
                default:
                    geometry0 = null;
                    return;
            }

            drawingSession.FillGeometry(geometry0, LeftFillColor);
            drawingSession.DrawGeometry(geometry0, LeftStrokeColor, 3f);

            CanvasGeometry geometry1;

            switch (this.ShapeType)
            {
                case 0:
                    geometry1 = CanvasGeometry.CreatePolygon(resourceCreator, this.BoundsRightPoints);
                    break;
                case 1:
                    geometry1 = CanvasGeometry.CreatePolygon(resourceCreator, this.TriangleRightPoints);
                    break;
                case 2:
                    geometry1 = CanvasGeometry.CreatePolygon(resourceCreator, this.QuadrilateralRightPoints);
                    break;
                default:
                    geometry1 = null;
                    return;
            }

            drawingSession.FillGeometry(geometry1, RightFillColor);
            drawingSession.DrawGeometry(geometry1, RightStrokeColor, 3f);

            drawingSession.FillCircle(this.Position, 6f, LeftStrokeColor);
        }
    }
}