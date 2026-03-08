using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Input;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
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
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes13;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode0;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoQuadrilateralRect"/>
    /// </summary>
    public sealed partial class CanvasPerspectiveCrop3Page : CanvasDual3Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool KeepConvex => this.IsShift || this.ToolBox3.KeepConvex;

        //@Const
        const float X = 128;
        const float Y = 128;
        const float W = 256;
        const float H = 256;

        // FreeTransformer
        BoxContainsNodeMode Mode;
        readonly DemoQuadrilateralRect FreeTransformer = new DemoQuadrilateralRect
        (
            destWidth: W,
            destHeight: H,
            source: new Quadrilateral
            {
                LeftTop = new Vector2(X, Y),
                RightTop = new Vector2(X + W, Y),
                LeftBottom = new Vector2(X, Y + H),
                RightBottom = new Vector2(X + W, Y + H),
            }
        );

        CanvasBitmap Bitmap;

        public CanvasPerspectiveCrop3Page()
        {
            this.InitializeComponent();
            this.ParameterPanel.Apply += (s, e) =>
            {
                QuadrilateralChannelKind kind = e;
                float value = this.ParameterPanel.Value;

                this.FreeTransformer.UpdateSource(this.FreeTransformer.Quadrilateral.MoveChannel(kind, value));

                this.Invalidate(InvalidateModes.None
                    //| InvalidateModes.UpdateLayers
                    | InvalidateModes.UpdateIndicator
                    | InvalidateModes.CanvasControl);
            };
        }

        public override void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(this.CreateResourcesAsync(resourceCreator).AsAsyncAction());
        }

        private async Task CreateResourcesAsync(ICanvasResourceCreator sender)
        {
            if (this.Bitmap != null)
                return;

            this.Bitmap = await CanvasBitmap.LoadAsync(sender, "Images/avatar.jpg");

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;
            this.FreeTransformer.UpdateDestination(width, height);

            this.ParameterPanel.UpdateAll(this.FreeTransformer.Quadrilateral);

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitCanvas
                //| InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator);
        }
        public override void DrawSource(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                drawingSession.DrawImage(new Transform2DEffect
                {
                    TransformMatrix = this.Canvas.Matrix,
                    Source = this.Bitmap,
                });
            }

            drawingSession.DrawBox(this.FreeTransformer.ActualSourceBox);
        }
        public override void DrawDestination(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                if (this.IsInContact)
                {
                    drawingSession.DrawImage(new OpacityEffect
                    {
                        Opacity = 0.5f,
                        Source = new Transform3DEffect
                        {
                            TransformMatrix = this.FreeTransformer.ActualDestinationMatrix,
                            Source = this.Bitmap,
                        }
                    });
                }

                Vector2[] points = this.FreeTransformer.ActualDestBox.To4Points();

                using (CanvasGeometry geometry = CanvasGeometry.CreatePolygon(resourceCreator, points))
                using (CanvasActiveLayer layer = drawingSession.CreateLayer(1f, geometry))
                {
                    drawingSession.DrawImage(new Transform3DEffect
                    {
                        TransformMatrix = this.FreeTransformer.ActualDestinationMatrix,
                        Source = this.Bitmap,
                    });
                }
            }

            drawingSession.DrawBounds(this.FreeTransformer.ActualDestBox);
        }
        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                Vector2[] points = this.FreeTransformer.ActualDestBox.To4Points();

                using (CanvasGeometry geometry = CanvasGeometry.CreatePolygon(resourceCreator, points))
                using (CanvasActiveLayer layer = drawingSession.CreateLayer(1f, geometry))
                {
                    drawingSession.DrawImage(new Transform3DEffect
                    {
                        TransformMatrix = this.FreeTransformer.ActualDestinationMatrix,
                        Source = this.Bitmap,
                    });
                }
            }
        }

        public override void CacheSingle()
        {
            this.CacheSingle0();
        }
        public override void Single()
        {
            this.Single0();
        }
        public override void DisposeSingle()
        {
        }
        public override void Over()
        {
        }

        public override void UpdateCanvasControl1()
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateCanvas
                | InvalidateModes.UpdateCanvas
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }

        public override void UpdateCanvasControl2()
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateCanvas
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.InitCanvas))
            {
                this.InitCanvas();
            }

            if (modes.HasFlag(InvalidateModes.InitIndicator))
            {
                //this.Indicator.ChangeAll(this.FreeTransformer.Quadrilateral, this.ParameterPanel.Mode);

                this.FreeTransformer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.UpdateCanvas))
            {
                this.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateIndicator))
            {
                //this.Indicator.ChangeAll(this.FreeTransformer.Quadrilateral, this.ParameterPanel.Mode);

                this.FreeTransformer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.CanvasControl))
            {
                this.Invalidate();
            }
        }

        #region FreeTransform
        private void CacheSingle0()
        {
            const float d = 12f;
            const float ds = d * d;

            {
                this.Mode = this.FreeTransformer.ActualSourceBox.ContainsNode(this.StartingPoint, ds);
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode.None: break;

                case BoxContainsNodeMode.Contains: this.FreeTransformer.CacheTranslation(); break;

                case BoxContainsNodeMode.LeftTop: this.FreeTransformer.CacheFreeTransform(FreeTransformMode.MoveLeftTop); break;
                case BoxContainsNodeMode.RightTop: this.FreeTransformer.CacheFreeTransform(FreeTransformMode.MoveRightTop); break;
                case BoxContainsNodeMode.LeftBottom: this.FreeTransformer.CacheFreeTransform(FreeTransformMode.MoveLeftBottom); break;
                case BoxContainsNodeMode.RightBottom: this.FreeTransformer.CacheFreeTransform(FreeTransformMode.MoveRightBottom); break;

                default: break;
            }
        }

        private void Single0()
        {
            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    break;
                case BoxContainsNodeMode.Contains:
                    this.FreeTransformer.Translate(this.StartingPosition, this.Position);

                    this.ParameterPanel.UpdateAll(this.FreeTransformer.Quadrilateral);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    if (this.KeepConvex)
                        this.FreeTransformer.MovePointOfConvexQuadrilateral(this.Position);
                    else
                        this.FreeTransformer.MovePoint(this.Position);

                    this.ParameterPanel.Update(this.FreeTransformer.Quadrilateral, this.FreeTransformer.PointKind);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }
        #endregion
    }
}