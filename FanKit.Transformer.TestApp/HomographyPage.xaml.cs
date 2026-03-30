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
        // Source
        public readonly static Color LeftStrokeColor = Colors.OrangeRed;
        public readonly static Color LeftFillColor = Color.FromArgb(51, Colors.OrangeRed.R, Colors.OrangeRed.G, Colors.OrangeRed.B);

        // Destination
        public readonly static Color RightStrokeColor = Colors.DeepSkyBlue;
        public readonly static Color RightFillColor = Color.FromArgb(51, Colors.DeepSkyBlue.R, Colors.DeepSkyBlue.G, Colors.DeepSkyBlue.B);

        // Homography
        Vector2 Position;

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
                this.Draw(s, e.DrawingSession);

                e.DrawingSession.FillCircle(this.Position, 6f, LeftStrokeColor);
                e.DrawingSession.FillCircle(this.TransformPoint(this.Position), 6f, RightStrokeColor);
            };
            this.CanvasControl.PointerMoved += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.CanvasControl);
                this.Position = new Vector2
                {
                    X = (float)pp.Position.X,
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
                this.InitializeDestination(width, height);
                this.InitializeMatrix();
            };
        }

        private async Task CreateResourcesAsync(ICanvasResourceCreator sender)
        {
            this.InitializeSource(await CanvasBitmap.LoadAsync(sender, "Images/avatar.jpg"));
            this.InitializeMatrix();
        }

        public abstract void InitializeDestination(float width, float height);
        public abstract void InitializeSource(CanvasBitmap bitmap);
        public abstract void InitializeMatrix();
        public abstract void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
        public abstract Vector2 TransformPoint(Vector2 point);
    }

    public sealed class DirectAffinePage : HomographyPage
    {
        //@Const
        const float W = 256;
        const float H = 256;
        SizeMatrix SourceNormalize = new SizeMatrix(W, H);

        // Source
        Rectangle LeftSourceRect;
        RectMatrix LeftSourceNormalize;
        Matrix3x2 LeftMatrix;

        // Destination
        Triangle RightDest;
        Matrix3x2 RightNorm;
        Matrix3x2 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[3];

        // Homography
        Matrix3x2 Matrix;
        CanvasBitmap Bitmap;

        public override void InitializeDestination(float width, float height)
        {
            float x0 = width / 4;
            float x1 = width - x0;
            float centerY = height / 2;

            // Source
            Rectangle rect = new Rectangle
            {
                X = x0 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
            this.LeftSourceRect = rect;
            this.LeftSourceNormalize = new RectMatrix(rect);

            // Destination
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

        public override void InitializeSource(CanvasBitmap bitmap)
        {
            this.Bitmap = bitmap;

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;

            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            // Source
            this.LeftMatrix = this.SourceNormalize.Map(this.LeftSourceRect).ToMatrix3x2();

            // Destination
            this.RightNorm = this.RightDest.Normalize();
            this.RightMatrix = this.SourceNormalize.Affine(this.RightNorm);

            // Homography
            this.Matrix = this.LeftSourceNormalize.Affine(this.RightNorm);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = this.Bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = this.Bitmap,
            });

            drawingSession.FillRectangle(this.LeftSourceRect.X, this.LeftSourceRect.Y, this.LeftSourceRect.Width, this.LeftSourceRect.Height, LeftFillColor);
            drawingSession.DrawRectangle(this.LeftSourceRect.X, this.LeftSourceRect.Y, this.LeftSourceRect.Width, this.LeftSourceRect.Height, LeftStrokeColor, 3f);

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
        //@Const
        const float W = 256;
        const float H = 256;
        SizeMatrix SourceNormalize = new SizeMatrix(W, H);

        // Source
        Triangle LeftDest;
        InvertibleMatrix3x2 LeftNorm;
        Matrix3x2 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[3];

        // Destination
        Triangle RightDest;
        Matrix3x2 RightNorm;
        Matrix3x2 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[3];

        // Homography
        Matrix3x2 Matrix;
        CanvasBitmap Bitmap;

        public override void InitializeDestination(float width, float height)
        {
            float x0 = width / 4;
            float x1 = width - x0;
            float centerY = height / 2;

            // Source
            this.LeftDest = new Triangle
            {
                LeftTop = new Vector2(x0 - 100, centerY - 120),
                RightTop = new Vector2(x0 + 100, centerY - 80),
                LeftBottom = new Vector2(x0 - 100, centerY + 80),
            };
            this.LeftPoints[0] = this.LeftDest.LeftTop;
            this.LeftPoints[1] = this.LeftDest.RightTop;
            this.LeftPoints[2] = this.LeftDest.LeftBottom;

            // Destination
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

        public override void InitializeSource(CanvasBitmap bitmap)
        {
            this.Bitmap = bitmap;

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;

            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            // Source
            this.LeftNorm = new InvertibleMatrix3x2(this.LeftDest);
            this.LeftMatrix = this.SourceNormalize.Affine(this.LeftDest.Normalize());

            // Destination
            this.RightNorm = this.RightDest.Normalize();
            this.RightMatrix = this.SourceNormalize.Affine(this.RightNorm);

            // Homography
            this.Matrix = this.LeftNorm.BidiAffine(this.RightNorm);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = this.Bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = this.Bitmap,
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
        //@Const
        const float W = 256;
        const float H = 256;
        SizeMatrix SourceNormalize = new SizeMatrix(W, H);

        // Source
        Triangle LeftDest;
        InvertibleMatrix3x2 LeftNorm;
        Matrix3x2 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[3];

        // Destination
        Rectangle RightDest;
        Matrix3x2 RightMatrix;

        // Homography
        Matrix3x2 Matrix;
        CanvasBitmap Bitmap;

        public override void InitializeDestination(float width, float height)
        {
            float x0 = width / 4;
            float x1 = width - x0;
            float centerY = height / 2;

            // Source
            this.LeftDest = new Triangle
            {
                LeftTop = new Vector2(x0 - 100, centerY - 120),
                RightTop = new Vector2(x0 + 100, centerY - 80),
                LeftBottom = new Vector2(x0 - 100, centerY + 80),
            };
            this.LeftPoints[0] = this.LeftDest.LeftTop;
            this.LeftPoints[1] = this.LeftDest.RightTop;
            this.LeftPoints[2] = this.LeftDest.LeftBottom;

            // Destination
            this.RightDest = new Rectangle
            {
                X = x1 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
        }

        public override void InitializeSource(CanvasBitmap bitmap)
        {
            this.Bitmap = bitmap;

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;

            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            // Source
            this.LeftNorm = new InvertibleMatrix3x2(this.LeftDest);
            this.LeftMatrix = this.SourceNormalize.Affine(this.LeftDest.Normalize());

            // Destination
            this.RightMatrix = this.SourceNormalize.Map(this.RightDest).ToMatrix3x2();

            // Homography
            this.Matrix = this.LeftNorm.InvAffine(this.RightDest);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = this.Bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = this.Bitmap,
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
        //@Const
        const float W = 256;
        const float H = 256;
        SizeMatrix SourceNormalize = new SizeMatrix(W, H);

        // Source
        Rectangle LeftSourceRect;
        RectMatrix LeftSourceNormalize;
        Matrix3x2 LeftMatrix;

        // Destination
        Quadrilateral RightDest;
        PerspSizeMatrix3x3 RightNorm;
        Matrix4x4 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[4];

        // Homography
        Matrix4x4 Matrix;
        CanvasBitmap Bitmap;

        public override void InitializeDestination(float width, float height)
        {
            float x0 = width / 4;
            float x1 = width - x0;
            float centerY = height / 2;

            // Source
            Rectangle rect = new Rectangle
            {
                X = x0 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
            this.LeftSourceRect = rect;
            this.LeftSourceNormalize = new RectMatrix(rect);

            // Destination
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

        public override void InitializeSource(CanvasBitmap bitmap)
        {
            this.Bitmap = bitmap;

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;

            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            // Source
            this.LeftMatrix = this.SourceNormalize.Map(this.LeftSourceRect).ToMatrix3x2();

            // Destination
            this.RightNorm = this.SourceNormalize.ToPerspMatrix(this.RightDest);
            this.RightMatrix = this.RightNorm;

            // Homography
            this.Matrix = this.LeftSourceNormalize.ToPerspMatrix(this.RightDest);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = this.Bitmap,
            });
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = this.Bitmap,
            });

            drawingSession.FillRectangle(this.LeftSourceRect.X, this.LeftSourceRect.Y, this.LeftSourceRect.Width, this.LeftSourceRect.Height, LeftFillColor);
            drawingSession.DrawRectangle(this.LeftSourceRect.X, this.LeftSourceRect.Y, this.LeftSourceRect.Width, this.LeftSourceRect.Height, LeftStrokeColor, 3f);

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
        //@Const
        const float W = 256;
        const float H = 256;
        SizeMatrix SourceNormalize = new SizeMatrix(W, H);

        // Source
        Quadrilateral LeftDest;
        PerspSizeMatrix3x3 LeftNorm;
        Matrix4x4 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[4];

        // Destination
        Quadrilateral RightDest;
        PerspSizeMatrix3x3 RightNorm;
        Matrix4x4 RightMatrix;
        readonly Vector2[] RightPoints = new Vector2[4];

        // Homography
        readonly InvertiblePerspQuadrilateral T = new InvertiblePerspQuadrilateral();
        CanvasBitmap Bitmap;

        public override void InitializeDestination(float width, float height)
        {
            float x0 = width / 4;
            float x1 = width - x0;
            float centerY = height / 2;

            // Source
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

            // Destination
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

        public override void InitializeSource(CanvasBitmap bitmap)
        {
            this.Bitmap = bitmap;

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;

            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            // Source
            this.LeftNorm = this.SourceNormalize.ToPerspMatrix(this.LeftDest);
            this.LeftMatrix = this.LeftNorm;

            // Destination
            this.RightNorm = this.SourceNormalize.ToPerspMatrix(this.RightDest);
            this.RightMatrix = this.RightNorm;

            // Homography
            this.T.FindHomography(this.LeftDest, this.RightDest);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = this.Bitmap,
            });
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = this.Bitmap,
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
            return Mathematics.Math.Transform(point, this.T.HomographyMatrix);
        }
    }

    public sealed class InversePerspectivePage : HomographyPage
    {
        //@Const
        const float W = 256;
        const float H = 256;
        SizeMatrix SourceNormalize = new SizeMatrix(W, H);

        // Source
        Quadrilateral LeftDest;
        PerspSizeMatrix3x3 LeftNorm;
        Matrix4x4 LeftMatrix;
        readonly Vector2[] LeftPoints = new Vector2[4];

        // Destination
        Rectangle RightSourceRect;
        Matrix3x2 RightMatrix;

        // Homography
        readonly InvertiblePerspectiveRect T = new InvertiblePerspectiveRect();
        CanvasBitmap Bitmap;

        public override void InitializeDestination(float width, float height)
        {
            float x0 = width / 4;
            float x1 = width - x0;
            float centerY = height / 2;

            // Source
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

            // Destination
            Rectangle rect = new Rectangle
            {
                X = x1 - 100,
                Y = centerY - 100,
                Width = 200,
                Height = 200,
            };
            this.RightSourceRect = rect;
        }

        public override void InitializeSource(CanvasBitmap bitmap)
        {
            this.Bitmap = bitmap;

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;

            this.SourceNormalize = new SizeMatrix(width, height);
        }

        public override void InitializeMatrix()
        {
            // Source
            this.LeftNorm = this.SourceNormalize.ToPerspMatrix(this.LeftDest);
            this.LeftMatrix = this.LeftNorm;

            // Destination
            this.RightMatrix = this.SourceNormalize.Map(this.RightSourceRect).ToMatrix3x2();

            // Homography
            this.T.FindHomography(this.LeftDest, this.RightSourceRect);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.LeftMatrix,
                Source = this.Bitmap,
            });
            drawingSession.DrawImage(new Transform2DEffect
            {
                TransformMatrix = this.RightMatrix,
                Source = this.Bitmap,
            });

            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(resourceCreator, this.LeftPoints);
            drawingSession.FillGeometry(geometry, LeftFillColor);
            drawingSession.DrawGeometry(geometry, LeftStrokeColor, 3f);

            drawingSession.FillRectangle(this.RightSourceRect.X, this.RightSourceRect.Y, this.RightSourceRect.Width, this.RightSourceRect.Height, RightFillColor);
            drawingSession.DrawRectangle(this.RightSourceRect.X, this.RightSourceRect.Y, this.RightSourceRect.Width, this.RightSourceRect.Height, RightStrokeColor, 3f);
        }

        public override Vector2 TransformPoint(Vector2 point)
        {
            return Mathematics.Math.Transform(point, this.T.HomographyMatrix);
        }
    }
}