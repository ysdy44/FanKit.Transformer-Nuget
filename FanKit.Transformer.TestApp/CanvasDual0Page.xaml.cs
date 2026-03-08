using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Input;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="SettingsDialog0"/>
    /// <see cref="CanvasOperator2"/>
    ///
    /// <see cref="Vector2"/>
    /// <see cref="CanvasTransformer0"/>
    /// </summary>
    public abstract partial class CanvasDual0Page : Page
    {
        //@Const
        const float CW = 512f;
        const float CH = 512f;

        public Vector2 StartingPoint;
        public Vector2 Point;

        public Vector2 StartingPosition;
        public Vector2 Position;

        Viewport ThumbView;
        Fit1 ThumbFit;
        ViewportThumbnail ViewThumb;
        readonly CanvasOperator1 ThumbCanvasOperator;

        //Pivot3 StartingPivot;
        //Pivot3 Pivot;
        readonly CanvasOperator2 CanvasOperatorSource;
        readonly CanvasOperator2 CanvasOperatorDestination;

        Viewport View;
        Vector2 Fit;
        Bounds TransformedBounds = new Bounds(CW, CH);
        readonly Bounds Bounds = new Bounds(CW, CH);
        public readonly CanvasTransformer0 Canvas = new CanvasTransformer0();

        public bool IsInContact => this.CanvasOperatorSource.IsInContact;

        public CanvasDual0Page()
        {
            this.InitializeComponent();
            this.CanvasOperatorSource = new CanvasOperator2(this.CanvasControlSource);
            this.CanvasOperatorDestination = new CanvasOperator2(this.CanvasControlDestination);
            this.ThumbCanvasOperator = new CanvasOperator1(this.ThumbCanvasControl);
            this.SettingsDialog.TouchMode = this.CanvasOperatorSource.TouchMode;
            //this.SettingsDialog.TouchMode = this.CanvasOperatorDestination.TouchMode;
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControlSource.RemoveFromVisualTree();
                this.CanvasControlDestination.RemoveFromVisualTree();
                this.ThumbCanvasControl.RemoveFromVisualTree();
                this.CanvasControlSource = null;
                this.CanvasControlDestination = null;
                this.ThumbCanvasControl = null;
            };

            this.CanvasControlSource.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.CanvasControlDestination.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.CanvasControlSource.Draw += (s, e) =>
            {
                e.DrawingSession.DrawCanvas(this.Bounds);

                e.DrawingSession.Transform = this.Canvas.Matrix;
                e.DrawingSession.FillCanvas(CW, CH);
                e.DrawingSession.Transform = Matrix3x2.Identity;

                e.DrawingSession.DrawCanvas(this.TransformedBounds);

                e.DrawingSession.DrawLine(0f, this.Canvas.OriginY, this.View.ViewportWidth, this.Canvas.OriginY, Windows.UI.Colors.OrangeRed);
                e.DrawingSession.DrawLine(this.Canvas.OriginX, 0f, this.Canvas.OriginX, this.View.ViewportHeight, Windows.UI.Colors.LawnGreen);

                this.DrawSource(s, e.DrawingSession);
            };
            this.CanvasControlDestination.Draw += (s, e) =>
            {
                e.DrawingSession.DrawCanvas(this.Bounds);

                e.DrawingSession.Transform = this.Canvas.Matrix;
                e.DrawingSession.FillCanvas(CW, CH);
                e.DrawingSession.Transform = Matrix3x2.Identity;

                e.DrawingSession.DrawCanvas(this.TransformedBounds);

                e.DrawingSession.DrawLine(0f, this.Canvas.OriginY, this.View.ViewportWidth, this.Canvas.OriginY, Windows.UI.Colors.OrangeRed);
                e.DrawingSession.DrawLine(this.Canvas.OriginX, 0f, this.Canvas.OriginX, this.View.ViewportHeight, Windows.UI.Colors.LawnGreen);

                this.DrawDestination(s, e.DrawingSession);
            };
            this.CanvasControlSource.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.View = new Viewport
                {
                    ExtentWidth = CW,
                    ExtentHeight = CH,

                    ViewportWidth = (float)e.NewSize.Width,
                    ViewportHeight = (float)e.NewSize.Height,
                };
                this.Fit = this.View.ToFitTranslation();

                if (this.CanvasControlSource.ReadyToDraw)
                {
                    this.UpdateCanvasControl2();
                }
            };
            this.CanvasControlDestination.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.View = new Viewport
                {
                    ExtentWidth = CW,
                    ExtentHeight = CH,

                    ViewportWidth = (float)e.NewSize.Width,
                    ViewportHeight = (float)e.NewSize.Height,
                };
                this.Fit = this.View.ToFitTranslation();

                if (this.CanvasControlDestination.ReadyToDraw)
                {
                    this.UpdateCanvasControl2();
                }
            };

            this.CanvasOperatorSource.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);
                this.StartingPosition = this.Position = this.Canvas.InverseTransform(this.Point);

                this.CacheSingle();
            };
            this.CanvasOperatorDestination.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);
                this.StartingPosition = this.Position = this.Canvas.InverseTransform(this.Point);

                this.CacheSingle();
            };
            this.CanvasOperatorSource.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);
                this.Position = this.Canvas.InverseTransform(this.Point);

                this.Single();
            };
            this.CanvasOperatorDestination.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);
                this.Position = this.Canvas.InverseTransform(this.Point);

                this.Single();
            };
            this.CanvasOperatorSource.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);
                this.Position = this.Canvas.InverseTransform(this.Point);

                this.DisposeSingle();
            };
            this.CanvasOperatorDestination.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);
                this.Position = this.Canvas.InverseTransform(this.Point);

                this.DisposeSingle();
            };
            this.CanvasOperatorSource.Pointer_Over += (x, y) =>
            {
                this.Point = new Vector2((float)x, (float)y);
                this.Position = this.Canvas.InverseTransform(this.Point);

                this.Over();
            };
            this.CanvasOperatorDestination.Pointer_Over += (x, y) =>
            {
                this.Point = new Vector2((float)x, (float)y);
                this.Position = this.Canvas.InverseTransform(this.Point);

                this.Over();
            };

            this.CanvasOperatorSource.Right_Start += (x, y) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)x, (float)y);

                {
                    this.Canvas.CacheMove(CanvasMoveUnits.Normal);
                }
            };
            this.CanvasOperatorDestination.Right_Start += (x, y) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)x, (float)y);

                {
                    this.Canvas.CacheMove(CanvasMoveUnits.Normal);
                }
            };
            this.CanvasOperatorSource.Right_Delta += (x, y) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                {
                    this.Canvas.Move(this.StartingPoint, this.Point, CanvasMoveUnits.Normal);
                }

                this.UpdateCanvasControl2();
            };
            this.CanvasOperatorDestination.Right_Delta += (x, y) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                {
                    this.Canvas.Move(this.StartingPoint, this.Point, CanvasMoveUnits.Normal);
                }

                this.UpdateCanvasControl2();
            };
            this.CanvasOperatorSource.Right_Complete += (x, y) => { };
            this.CanvasOperatorDestination.Right_Complete += (x, y) => { };

            this.ThumbCanvasControl.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.ThumbCanvasControl.Draw += (s, e) =>
            {
                e.DrawingSession.FillCanvas(this.ThumbFit.Bounds);
                e.DrawingSession.DrawCanvas(this.ThumbFit.Bounds);

                e.DrawingSession.Transform = this.ThumbFit.Coord.Matrix;

                this.DrawThumb(s, e.DrawingSession);

                e.DrawingSession.Transform = this.ViewThumb.Matrix;
                e.DrawingSession.FillViewport(this.View.ViewportWidth, this.View.ViewportHeight);

                e.DrawingSession.Transform = Matrix3x2.Identity;
                e.DrawingSession.DrawViewport(this.ViewThumb.Quadrilateral);
            };
            this.ThumbCanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.ThumbView = new Viewport
                {
                    ExtentWidth = CW,
                    ExtentHeight = CH,

                    ViewportWidth = (float)e.NewSize.Width,
                    ViewportHeight = (float)e.NewSize.Height,
                };
                this.ThumbFit = this.ThumbView.ToFit1();

                if (this.CanvasControlSource.ReadyToDraw)
                //if (this.CanvasControlDestination.ReadyToDraw)
                {
                    this.UpdateCanvasControl1();
                }
            };

            this.ThumbCanvasOperator.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);
                this.StartingPosition = this.Position = this.StartingPoint * this.ThumbFit.Coord.InverseScaleFactor;

                this.Canvas.CacheMove(CanvasMoveUnits.Thumbnail);

                this.UpdateCanvasControl2();
            };
            this.ThumbCanvasOperator.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);
                this.Position = this.Point * this.ThumbFit.Coord.InverseScaleFactor;

                this.Canvas.Move(this.StartingPosition, this.Position, CanvasMoveUnits.Thumbnail);

                this.UpdateCanvasControl2();
            };
            this.ThumbCanvasOperator.Single_Complete += (x, y, p) => { };

            this.SettingsDialog.TouchModeChanged += delegate
            {
                this.CanvasOperatorSource.TouchMode = this.SettingsDialog.TouchMode;
                //this.CanvasOperatorDestination.TouchMode = this.SettingsDialog.TouchMode;
            };
            this.BottomBar.EventsUnchecked += delegate
            {
                this.SettingsDialog.Visibility = Visibility.Collapsed;
            };
            this.BottomBar.EventsChecked += delegate
            {
                this.SettingsDialog.Visibility = Visibility.Visible;
            };

            this.BottomBar.HideToggled += delegate
            {
                this.ToolBox3.Visibility =
                this.ThumbBorder.Visibility =
                this.ParameterPanel.Visibility =
                this.CanvasHub.Visibility =
                this.BottomBar.IsHidden ? Visibility.Collapsed : Visibility.Visible;
            };
        }

        public abstract void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args);
        public abstract void DrawSource(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
        public abstract void DrawDestination(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
        public abstract void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);

        public abstract void CacheSingle();
        public abstract void Single();
        public abstract void DisposeSingle();
        public abstract void Over();

        public abstract void UpdateCanvasControl1();
        public abstract void UpdateCanvasControl2();

        public void InitCanvas()
        {
            this.Canvas.Fit(this.Fit);
            this.CanvasHub.Update(this.Canvas);
            this.TransformedBounds = this.Canvas.TransformSize(CW, CH);
            this.ViewThumb = new ViewportThumbnail(this.View, this.Canvas, this.ThumbFit.Coord);
        }

        public void UpdateCanvas()
        {
            this.CanvasHub.Update(this.Canvas);
            this.TransformedBounds = this.Canvas.TransformSize(CW, CH);
            this.ViewThumb = new ViewportThumbnail(this.View, this.Canvas, this.ThumbFit.Coord);
        }

        public void Invalidate()
        {
            this.CanvasControlSource.Invalidate();
            this.CanvasControlDestination.Invalidate();
            this.ThumbCanvasControl.Invalidate(); // Invalidate
        }
    }
}