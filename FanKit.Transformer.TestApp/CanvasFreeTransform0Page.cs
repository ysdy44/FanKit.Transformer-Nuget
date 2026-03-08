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
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes13;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode0;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoSizeQuadrilateral"/>
    /// </summary>
    public partial class CanvasFreeTransform0Page : CanvasSole0Page
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
        readonly DemoSizeQuadrilateral FreeTransformer = new DemoSizeQuadrilateral
        (
            sourceWidth: W,
            sourceHeight: H,
            destination: new Quadrilateral
            {
                LeftTop = new Vector2(X, Y + 20),
                RightTop = new Vector2(X + W, Y - 20),
                LeftBottom = new Vector2(X, Y + H - 20),
                RightBottom = new Vector2(X + W, Y + H + 20),
            }
        );

        CanvasBitmap Bitmap;

        readonly ToolBox3 ToolBox3 = new ToolBox3
        {
            Title = "Free Transform"
        };

        public CanvasFreeTransform0Page()
        {
            this.Children.Add(this.ToolBox3);
            this.ParameterPanel.Apply += (s, e) =>
            {
                QuadrilateralChannelKind kind = e;
                float value = this.ParameterPanel.Value;

                this.FreeTransformer.UpdateDestination(this.FreeTransformer.Quadrilateral.MoveChannel(kind, value));

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
            this.FreeTransformer.UpdateSource(width, height);

            this.ParameterPanel.UpdateAll(this.FreeTransformer.Quadrilateral);

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitCanvas
                //| InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                drawingSession.DrawImage(new Transform3DEffect
                {
                    TransformMatrix = this.FreeTransformer.ActualMatrix,
                    Source = this.Bitmap,
                });
            }

            drawingSession.DrawBox(this.FreeTransformer.ActualBox);
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                drawingSession.DrawImage(new Transform3DEffect
                {
                    TransformMatrix = this.FreeTransformer.Matrix,
                    Source = this.Bitmap,
                });
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
                //| InvalidateModes.UpdateLayers
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
                //this.Indicator.ChangeAll(this.Transformer.Quadrilateral, this.ParameterPanel.Mode);

                this.FreeTransformer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.UpdateCanvas))
            {
                this.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateIndicator))
            {
                //this.Indicator.ChangeAll(this.Transformer.Quadrilateral, this.ParameterPanel.Mode);

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

            this.Mode = this.FreeTransformer.ActualBox.ContainsNode(this.StartingPoint, ds);

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