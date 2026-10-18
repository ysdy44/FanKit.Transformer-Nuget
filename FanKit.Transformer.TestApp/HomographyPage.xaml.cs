using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Transforms;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FanKit.Transformer.TestApp
{
    public abstract partial class HomographyPage : Page
    {
        public readonly static Color LeftStrokeColor = Colors.OrangeRed;
        public readonly static Color LeftFillColor = Color.FromArgb(51, Colors.OrangeRed.R, Colors.OrangeRed.G, Colors.OrangeRed.B);

        public readonly static Color RightStrokeColor = Colors.DeepSkyBlue;
        public readonly static Color RightFillColor = Color.FromArgb(51, Colors.DeepSkyBlue.R, Colors.DeepSkyBlue.G, Colors.DeepSkyBlue.B);

        Vector2 Position;
        CanvasBitmap Bitmap;

        public HomographyPage()
        {
            this.InitializeComponent();
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
            };

            this.CanvasControl.CreateResources += (s, args) =>
            {
                args.TrackAsyncAction(CreateResourcesAsync(s).AsAsyncAction());
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                this.Draw(s, e.DrawingSession, this.Bitmap);

                e.DrawingSession.FillCircle(this.Position, 6f, LeftStrokeColor);
                e.DrawingSession.FillCircle(this.TransformPoint(this.Position), 6f, RightStrokeColor);
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

                this.CanvasControl.Invalidate();
            };
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                float x0 = width / 4;
                float x1 = width - x0;
                float centerY = height / 2;
                this.InitializeDestination(x0, x1, centerY);
                this.InitializeMatrix();
            };
        }

        private async Task CreateResourcesAsync(ICanvasResourceCreator resourceCreator)
        {
            CanvasBitmap bitmap = (await CanvasBitmap.LoadAsync(resourceCreator, "Images/avatar.jpg"));
            float width = (float)bitmap.Size.Width;
            float height = (float)bitmap.Size.Height;

            this.Bitmap = bitmap;
            this.InitializeSource(width, height);
            this.InitializeMatrix();
        }

        public abstract void InitializeDestination(float x0, float x1, float centerY);
        public abstract void InitializeSource(float width, float height);
        public abstract void InitializeMatrix();
        public abstract void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasBitmap bitmap);
        public abstract Vector2 TransformPoint(Vector2 point);
    }

    public sealed class DirectAffinePage : HomographyPage
    {
        Rectangle LeftDest;
        RectMatrix LeftNorm;
        Matrix3x2 LeftMatrix;

        Triangle RightDest;
        Matrix3x2 RightNorm;
        Matrix3x2 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[3];

        SizeMatrix SourceNormalize;
        Matrix3x2 Matrix;

        public override void InitializeDestination(float x0, float x1, float centerY)
        {
            this.LeftDest = new Rectangle
            {
                X = x0 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
            this.LeftNorm = new RectMatrix(this.LeftDest);

            this.RightDest = new Triangle
            {
                LeftTop = new Vector2(x1 - 100, centerY - 120),
                RightTop = new Vector2(x1 + 100, centerY - 80),
                LeftBottom = new Vector2(x1 - 100, centerY + 80),
            };
            this.RightPoints[0] = this.RightDest.LeftTop;
            this.RightPoints[1] = this.RightDest.RightTop;
            this.RightPoints[2] = this.RightDest.LeftBottom;
        }

        public override void InitializeSource(float width, float height)
        {
            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            this.LeftMatrix = this.SourceNormalize.Map(this.LeftDest).ToMatrix3x2();

            this.RightNorm = this.RightDest.Normalize();
            this.RightMatrix = this.SourceNormalize.Affine(this.RightNorm);

            this.Matrix = this.LeftNorm.Affine(this.RightNorm);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasBitmap bitmap)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = bitmap,
            });

            drawingSession.FillRectangle(this.LeftDest.X, this.LeftDest.Y, this.LeftDest.Width, this.LeftDest.Height, LeftFillColor);
            drawingSession.DrawRectangle(this.LeftDest.X, this.LeftDest.Y, this.LeftDest.Width, this.LeftDest.Height, LeftStrokeColor, 3f);

            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.RightPoints);
            drawingSession.FillGeometry(geometry, RightFillColor);
            drawingSession.DrawGeometry(geometry, RightStrokeColor, 3f);
        }

        public override Vector2 TransformPoint(Vector2 point)
        {
            return Vector2.Transform(point, this.Matrix);
        }
    }

    public sealed class BidirectionalAffinePage : HomographyPage
    {
        Triangle LeftDest;
        TriangleMatrix LeftNorm;
        Matrix3x2 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[3];

        Triangle RightDest;
        Matrix3x2 RightNorm;
        Matrix3x2 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[3];

        SizeMatrix SourceNormalize;
        Matrix3x2 Matrix;

        public override void InitializeDestination(float x0, float x1, float centerY)
        {
            this.LeftDest = new Triangle
            {
                LeftTop = new Vector2(x0 - 100, centerY - 120),
                RightTop = new Vector2(x0 + 100, centerY - 80),
                LeftBottom = new Vector2(x0 - 100, centerY + 80),
            };
            this.LeftPoints[0] = this.LeftDest.LeftTop;
            this.LeftPoints[1] = this.LeftDest.RightTop;
            this.LeftPoints[2] = this.LeftDest.LeftBottom;

            this.RightDest = new Triangle
            {
                LeftTop = new Vector2(x1 - 100, centerY - 100),
                RightTop = new Vector2(x1 + 100, centerY - 120),
                LeftBottom = new Vector2(x1 - 100, centerY + 100),
            };
            this.RightPoints[0] = this.RightDest.LeftTop;
            this.RightPoints[1] = this.RightDest.RightTop;
            this.RightPoints[2] = this.RightDest.LeftBottom;
        }

        public override void InitializeSource(float width, float height)
        {
            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            this.LeftNorm = new TriangleMatrix(this.LeftDest);
            this.LeftMatrix = this.SourceNormalize.Affine(this.LeftDest.Normalize());

            this.RightNorm = this.RightDest.Normalize();
            this.RightMatrix = this.SourceNormalize.Affine(this.RightNorm);

            this.Matrix = this.LeftNorm.BidiAffine(this.RightNorm);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasBitmap bitmap)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = bitmap,
            });

            CanvasGeometry geometry0 = CanvasGeometry.CreatePolygon(resourceCreator, this.LeftPoints);
            drawingSession.FillGeometry(geometry0, LeftFillColor);
            drawingSession.DrawGeometry(geometry0, LeftStrokeColor, 3f);

            CanvasGeometry geometry1 = CanvasGeometry.CreatePolygon(resourceCreator, this.RightPoints);
            drawingSession.FillGeometry(geometry1, RightFillColor);
            drawingSession.DrawGeometry(geometry1, RightStrokeColor, 3f);
        }

        public override Vector2 TransformPoint(Vector2 point)
        {
            return Vector2.Transform(point, this.Matrix);
        }
    }

    public sealed class InverseAffinePage : HomographyPage
    {
        Triangle LeftDest;
        TriangleMatrix LeftNorm;
        Matrix3x2 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[3];

        Rectangle RightDest;
        Matrix3x2 RightMatrix;

        SizeMatrix SourceNormalize;
        Matrix3x2 Matrix;

        public override void InitializeDestination(float x0, float x1, float centerY)
        {
            this.LeftDest = new Triangle
            {
                LeftTop = new Vector2(x0 - 100, centerY - 120),
                RightTop = new Vector2(x0 + 100, centerY - 80),
                LeftBottom = new Vector2(x0 - 100, centerY + 80),
            };
            this.LeftPoints[0] = this.LeftDest.LeftTop;
            this.LeftPoints[1] = this.LeftDest.RightTop;
            this.LeftPoints[2] = this.LeftDest.LeftBottom;

            this.RightDest = new Rectangle
            {
                X = x1 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
        }

        public override void InitializeSource(float width, float height)
        {
            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            this.LeftNorm = new TriangleMatrix(this.LeftDest);
            this.LeftMatrix = this.SourceNormalize.Affine(this.LeftDest.Normalize());

            this.RightMatrix = this.SourceNormalize.Map(this.RightDest).ToMatrix3x2();

            this.Matrix = this.LeftNorm.InvAffine(this.RightDest);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasBitmap bitmap)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = bitmap,
            });

            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.LeftPoints);
            drawingSession.FillGeometry(geometry, LeftFillColor);
            drawingSession.DrawGeometry(geometry, LeftStrokeColor, 3f);

            drawingSession.FillRectangle(this.RightDest.X, this.RightDest.Y, this.RightDest.Width, this.RightDest.Height, RightFillColor);
            drawingSession.DrawRectangle(this.RightDest.X, this.RightDest.Y, this.RightDest.Width, this.RightDest.Height, RightStrokeColor, 3f);
        }

        public override Vector2 TransformPoint(Vector2 point)
        {
            return Vector2.Transform(point, this.Matrix);
        }
    }

    public sealed class DirectPerspectivePage : HomographyPage
    {
        Rectangle LeftDest;
        RectMatrix LeftNorm;
        Matrix3x2 LeftMatrix;

        Quadrilateral RightDest;
        Matrix4x4 RightNorm;
        Matrix4x4 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[4];

        SizeMatrix SourceNormalize;
        Matrix4x4 Matrix;

        public override void InitializeDestination(float x0, float x1, float centerY)
        {
            this.LeftDest = new Rectangle
            {
                X = x0 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
            this.LeftNorm = new RectMatrix(this.LeftDest);

            this.RightDest = new Quadrilateral
            {
                LeftTop = new Vector2(x1 - 100, centerY - 80),
                RightTop = new Vector2(x1 + 100, centerY - 120),
                LeftBottom = new Vector2(x1 - 100, centerY + 80),
                RightBottom = new Vector2(x1 + 100, centerY + 120),
            };
            this.RightPoints[0] = this.RightDest.LeftTop;
            this.RightPoints[1] = this.RightDest.RightTop;
            this.RightPoints[2] = this.RightDest.RightBottom;
            this.RightPoints[3] = this.RightDest.LeftBottom;
        }

        public override void InitializeSource(float width, float height)
        {
            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            this.LeftMatrix = this.SourceNormalize.Map(this.LeftDest).ToMatrix3x2();

            this.RightNorm = this.RightDest.Normalize();
            this.RightMatrix = this.SourceNormalize.Persp(this.RightNorm);

            this.Matrix = this.LeftNorm.Persp(this.RightNorm);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasBitmap bitmap)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = bitmap,
            });
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = bitmap,
            });

            drawingSession.FillRectangle(this.LeftDest.X, this.LeftDest.Y, this.LeftDest.Width, this.LeftDest.Height, LeftFillColor);
            drawingSession.DrawRectangle(this.LeftDest.X, this.LeftDest.Y, this.LeftDest.Width, this.LeftDest.Height, LeftStrokeColor, 3f);

            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.RightPoints);
            drawingSession.FillGeometry(geometry, RightFillColor);
            drawingSession.DrawGeometry(geometry, RightStrokeColor, 3f);
        }

        public override Vector2 TransformPoint(Vector2 point)
        {
            return Mathematics.Math.Transform(point, this.Matrix);
        }
    }

    public sealed class BidirectionalPerspectivePage : HomographyPage
    {
        Quadrilateral LeftDest;
        Matrix4x4 LeftNorm;
        Matrix4x4 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[4];

        Quadrilateral RightDest;
        Matrix4x4 RightNorm;
        Matrix4x4 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[4];

        SizeMatrix SourceNormalize;
        Matrix4x4 Matrix;

        public override void InitializeDestination(float x0, float x1, float centerY)
        {
            this.LeftDest = new Quadrilateral
            {
                LeftTop = new Vector2(x0 - 100, centerY - 80),
                RightTop = new Vector2(x0 + 100, centerY - 120),
                LeftBottom = new Vector2(x0 - 100, centerY + 80),
                RightBottom = new Vector2(x0 + 100, centerY + 120),
            };
            this.LeftPoints[0] = this.LeftDest.LeftTop;
            this.LeftPoints[1] = this.LeftDest.RightTop;
            this.LeftPoints[2] = this.LeftDest.RightBottom;
            this.LeftPoints[3] = this.LeftDest.LeftBottom;

            this.RightDest = new Quadrilateral
            {
                LeftTop = new Vector2(x1 - 100, centerY - 120),
                RightTop = new Vector2(x1 + 100, centerY - 80),
                LeftBottom = new Vector2(x1 - 100, centerY + 120),
                RightBottom = new Vector2(x1 + 100, centerY + 80),
            };
            this.RightPoints[0] = this.RightDest.LeftTop;
            this.RightPoints[1] = this.RightDest.RightTop;
            this.RightPoints[2] = this.RightDest.RightBottom;
            this.RightPoints[3] = this.RightDest.LeftBottom;
        }

        public override void InitializeSource(float width, float height)
        {
            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            this.LeftNorm = (this.LeftDest).Normalize();
            this.LeftMatrix = this.SourceNormalize.Persp(this.LeftNorm);

            this.RightNorm = this.RightDest.Normalize();
            this.RightMatrix = this.SourceNormalize.Persp(this.RightNorm);

            this.Matrix = new QuadrilateralMatrix(this.LeftDest).InvPersp(this.RightNorm);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasBitmap bitmap)
        {
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = bitmap,
            });
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = bitmap,
            });

            CanvasGeometry geometry0 = CanvasGeometry.CreatePolygon(resourceCreator, this.LeftPoints);
            drawingSession.FillGeometry(geometry0, LeftFillColor);
            drawingSession.DrawGeometry(geometry0, LeftStrokeColor, 3f);

            CanvasGeometry geometry1 = CanvasGeometry.CreatePolygon(resourceCreator, this.RightPoints);
            drawingSession.FillGeometry(geometry1, RightFillColor);
            drawingSession.DrawGeometry(geometry1, RightStrokeColor, 3f);
        }

        public override Vector2 TransformPoint(Vector2 point)
        {
            return Mathematics.Math.Transform(point, this.Matrix);
        }
    }

    public sealed class InversePerspectivePage : HomographyPage
    {
        Quadrilateral LeftDest;
        Matrix4x4 LeftNorm;
        Matrix4x4 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[4];

        Rectangle RightDest;
        Matrix3x2 RightMatrix;

        SizeMatrix SourceNormalize;
        Matrix4x4 Matrix;

        public override void InitializeDestination(float x0, float x1, float centerY)
        {
            this.LeftDest = new Quadrilateral
            {
                LeftTop = new Vector2(x0 - 100, centerY - 80),
                RightTop = new Vector2(x0 + 100, centerY - 120),
                LeftBottom = new Vector2(x0 - 100, centerY + 80),
                RightBottom = new Vector2(x0 + 100, centerY + 120),
            };
            this.LeftPoints[0] = this.LeftDest.LeftTop;
            this.LeftPoints[1] = this.LeftDest.RightTop;
            this.LeftPoints[2] = this.LeftDest.RightBottom;
            this.LeftPoints[3] = this.LeftDest.LeftBottom;

            this.RightDest = new Rectangle
            {
                X = x1 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
        }

        public override void InitializeSource(float width, float height)
        {
            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            this.LeftNorm = this.LeftDest.Normalize();
            this.LeftMatrix = this.SourceNormalize.Persp(this.LeftNorm);

            this.RightMatrix = this.SourceNormalize.Map(this.RightDest).ToMatrix3x2();

            this.Matrix = new QuadrilateralMatrix(this.LeftDest).InvPersp(this.RightDest);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasBitmap bitmap)
        {
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = bitmap,
            });

            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.LeftPoints);
            drawingSession.FillGeometry(geometry, LeftFillColor);
            drawingSession.DrawGeometry(geometry, LeftStrokeColor, 3f);

            drawingSession.FillRectangle(this.RightDest.X, this.RightDest.Y, this.RightDest.Width, this.RightDest.Height, RightFillColor);
            drawingSession.DrawRectangle(this.RightDest.X, this.RightDest.Y, this.RightDest.Width, this.RightDest.Height, RightStrokeColor, 3f);
        }

        public override Vector2 TransformPoint(Vector2 point)
        {
            return Mathematics.Math.Transform(point, this.Matrix);
        }
    }
}