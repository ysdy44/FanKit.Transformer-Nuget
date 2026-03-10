using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
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
using FanKit.Transformer.Transforms;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls.Primitives;
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes13;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode1;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoSizeBounds"/>
    /// </summary>
    public sealed partial class CanvasCrop1Page : CanvasSolo1Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool CenteredScaling => this.IsCtrl || this.ToolBox1.CenteredScaling;
        bool KeepRatio => this.IsShift || this.ToolBox1.KeepRatio;

        //bool HasStepFrequency => this.IsShift;

        //@Const
        const float X = 78;
        const float Y = 123;
        const float W = 256;
        const float H = 256;

        //const float StepFrequency = (float)System.Math.PI / 12f;
        //const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        // Cropper
        BoxContainsNodeMode Mode;
        readonly DemoSizeBounds Cropper = new DemoSizeBounds
        (
            sourceWidth: W,
            sourceHeight: H,
            destination: new Bounds
            {
                Left = X,
                Top = Y,
                Right = X + W,
                Bottom = Y + H,
            }
        );

        CanvasBitmap Bitmap;
        readonly Indicator Indicator = new Indicator();

        readonly ToolBox1 ToolBox1 = new ToolBox1
        {
            Title = "Transform"
        };

        public CanvasCrop1Page()
        {
            this.Children.Add(this.ToolBox1);

            this.Indicator.SizeTypeChanged += (s, e) =>
            {
                IndicatorSizeType type = e;
                this.ParameterPanel.UpdateSizeType(type);
            };
            this.Indicator.XChanged += (s, e) => this.ParameterPanel.UpdateX(e);
            this.Indicator.YChanged += (s, e) => this.ParameterPanel.UpdateY(e);
            this.Indicator.WidthChanged += (s, e) => this.ParameterPanel.UpdateWidth(e);
            this.Indicator.HeightChanged += (s, e) => this.ParameterPanel.UpdateHeight(e);
            this.Indicator.RotationChanged += (s, e) => this.ParameterPanel.UpdateRotation(e);
            this.Indicator.SkewChanged += (s, e) => this.ParameterPanel.UpdateSkew(e);

            this.ParameterPanel.ModeChanged += (s, e) => this.Indicator.ChangeXY(this.Cropper.Destination, e);

            this.ParameterPanel.Apply += (s, e) =>
            {
                this.Apply(e, this.ParameterPanel.Value);
            };
        }

        public override void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(resourceCreator).AsAsyncAction());
        }
        private async Task CreateResourcesAsync(ICanvasResourceCreator sender)
        {
            if (this.Bitmap != null)
                return;

            this.Bitmap = await CanvasBitmap.LoadAsync(sender, "Images/avatar.jpg");

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;
            this.Cropper.UpdateSource(width, height);

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitCanvas
                //| InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                drawingSession.DrawImage(new Transform2DEffect
                {
                    TransformMatrix = this.Cropper.ActualMatrix,
                    Source = this.Bitmap,
                });
            }

            drawingSession.DrawBox(this.Cropper.ActualBox);
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap != null)
            {
                drawingSession.DrawImage(new Transform2DEffect
                {
                    TransformMatrix = this.Cropper.HomographyMatrix.ToMatrix3x2(),
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
                this.Indicator.ChangeAll(this.Cropper.Destination, this.ParameterPanel.Mode);

                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Cropper.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.UpdateCanvas))
            {
                this.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateIndicator))
            {
                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Cropper.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.CanvasControl))
            {
                this.Invalidate();
            }
        }

        #region Crop1
        private void CacheSingle0()
        {
            const float d = 12f;
            const float ds = d * d;

            this.Mode = this.Cropper.ActualBox.ContainsNode(this.StartingPoint, ds);

            switch (this.Mode)
            {
                case BoxContainsNodeMode.None: break;
                // Multiple Translation 2
                case BoxContainsNodeMode.Contains: this.Cropper.CacheTranslation(); break;

                // Multiple Transform 2
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom: this.Cropper.CacheRotation(this.StartingPosition); break;

                case BoxContainsNodeMode.HandleLeft: this.Cropper.CacheTransform(CropMode.SkewLeft); break;
                case BoxContainsNodeMode.HandleTop: this.Cropper.CacheTransform(CropMode.SkewTop); break;
                case BoxContainsNodeMode.HandleRight: this.Cropper.CacheTransform(CropMode.SkewRight); break;
                case BoxContainsNodeMode.HandleBottom: this.Cropper.CacheTransform(CropMode.SkewBottom); break;
                 */

                case BoxContainsNodeMode.CenterLeft: this.Cropper.CacheTransform(CropMode.ScaleLeft); break;
                case BoxContainsNodeMode.CenterTop: this.Cropper.CacheTransform(CropMode.ScaleTop); break;
                case BoxContainsNodeMode.CenterRight: this.Cropper.CacheTransform(CropMode.ScaleRight); break;
                case BoxContainsNodeMode.CenterBottom: this.Cropper.CacheTransform(CropMode.ScaleBottom); break;

                case BoxContainsNodeMode.LeftTop: this.Cropper.CacheTransform(CropMode.ScaleLeftTop); break;
                case BoxContainsNodeMode.RightTop: this.Cropper.CacheTransform(CropMode.ScaleRightTop); break;
                case BoxContainsNodeMode.LeftBottom: this.Cropper.CacheTransform(CropMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode.RightBottom: this.Cropper.CacheTransform(CropMode.ScaleRightBottom); break;
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
                    this.Cropper.Translate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom:
                    if (this.HasStepFrequency)
                        this.Cropper.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Position, StepFrequency);
                    else
                        this.Cropper.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Position);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.HandleLeft:
                case BoxContainsNodeMode.HandleTop:
                case BoxContainsNodeMode.HandleRight:
                case BoxContainsNodeMode.HandleBottom:
                    this.Cropper.TransformSkew(this.Indicator, this.ParameterPanel.Mode, this.Position, this.KeepRatio, this.CenteredScaling);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                    */
                default:
                    this.Cropper.TransformSize(this.Indicator, this.ParameterPanel.Mode, this.Position, this.KeepRatio, this.CenteredScaling);

                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }
        #endregion

        #region Panel
        private void Apply(IndicatorKind kind, float value)
        {
            BoxMode mode = this.ParameterPanel.Mode;

            switch (kind)
            {
                case IndicatorKind.X:
                    float translateX = value - this.Indicator.X;

                    this.Cropper.SetTranslationX(this.Indicator, mode, translateX);
                    break;
                case IndicatorKind.Y:
                    float translateY = value - this.Indicator.Y;

                    this.Cropper.SetTranslationY(this.Indicator, mode, translateY);
                    break;
                case IndicatorKind.Width:
                    this.Cropper.SetWidth(this.Indicator, mode, value, this.KeepRatio);
                    break;
                case IndicatorKind.Height:
                    this.Cropper.SetHeight(this.Indicator, mode, value, this.KeepRatio);
                    break;
                //case IndicatorKind.Rotation:
                //    this.Cropper.SetRotation(this.Indicator, mode, value);
                //    break;
                //case IndicatorKind.Skew:
                //    this.Cropper.SetSkew(this.Indicator, mode, value);
                //    break;
                default:
                    break;
            }

            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }
        #endregion
    }
}