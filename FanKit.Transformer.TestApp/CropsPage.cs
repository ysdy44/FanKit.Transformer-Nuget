using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FanKit.Transformer.Input;
using FanKit.Transformer.Transforms;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Primitives;
using Layer = FanKit.Transformer.Demos.DemoSizeBoundsLayer;
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes12;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode1;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoHostBounds"/>
    /// </summary>
    public sealed partial class CropsPage : SoloPage
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool CenteredScaling => this.IsCtrl || this.ToolBox2.CenteredScaling;
        bool KeepRatio => this.IsShift || this.ToolBox2.KeepRatio;

        //bool HasStepFrequency => this.IsShift;

        //@Const
        const float X = -72;
        const float Y = 123;
        const float W = 256;
        const float H = 256;

        //const float StepFrequency = (float)System.Math.PI / 12f;
        //const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        // Multi
        Bounds RectChoose;
        Quadrilateral ActualRectChoose;
        bool HasRectChoose;

        // CropperMulti
        BoxContainsNodeMode Mode;
        readonly DemoHostBounds Cropper = new DemoHostBounds();

        CanvasBitmap Bitmap;
        readonly Indicator Indicator = new Indicator();
        readonly List<Layer> Layers = new List<Layer>
        {
            new Layer
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
            )
            {
                IsSelected = true
            },
            new Layer
            (
                sourceWidth: W,
                sourceHeight: H,
                destination: new Bounds
                {
                    Left = 300 + X,
                    Top = Y,
                    Right = 300 + X + W,
                    Bottom = Y + H,
                }
            )
            {
                IsSelected = true
            },
        };

        readonly TopBar0 TopBar = new TopBar0();
        readonly ToolBox2 ToolBox2 = new ToolBox2
        {
            Title = "Transform"
        };

        public CropsPage()
        {
            this.Child = this.TopBar;
            this.Children.Add(this.ToolBox2);

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

            this.ParameterPanel.ModeChanged += (s, e) => this.Indicator.ChangeXY(this.Cropper.Bounds, e);

            this.ParameterPanel.Apply += (s, e) =>
            {
                this.Apply(e, this.ParameterPanel.Value);
            };

            this.ToolBox2.RemoveClick += delegate { this.Remove(); };
            this.ToolBox2.DeselectAllClick += delegate { this.DeselectAll(); };
            this.ToolBox2.SelectAllClick += delegate { this.SelectAll(); };
            this.ToolBox2.SelectNextClick += delegate { this.SelectNext(); };
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
            foreach (Layer item in this.Layers)
            {
                item.UpdateSource(width, height);
            }

            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.InitCanvas
                | InvalidateModes.InitLayers
                | InvalidateModes.InitIndicator);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            foreach (Layer item in this.Layers)
            {
                if (this.Bitmap != null)
                {
                    drawingSession.DrawImage(new Transform2DEffect
                    {
                        TransformMatrix = item.ActualMatrix,
                        Source = this.Bitmap,
                    });
                }
            }

            this.Draw0(drawingSession);
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            foreach (Layer item in this.Layers)
            {
                if (this.Bitmap != null)
                {
                    drawingSession.DrawImage(new Transform2DEffect
                    {
                        TransformMatrix = item.Matrix.ToMatrix3x2(),
                        Source = this.Bitmap,
                    });
                }
            }
        }

        public override void CacheSingle()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType0.Transform:
                    this.CacheSingle0();
                    break;
                case ToolType0.CreateNew:
                    this.CacheSingle1();
                    break;
                default:
                    break;
            }
        }

        public override void Single()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType0.Transform:
                    this.Single0();
                    break;
                case ToolType0.CreateNew:
                    this.Single1();
                    break;
                default:
                    break;
            }
        }

        public override void DisposeSingle()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType0.Transform:
                    this.DisposeSingle0();
                    break;
                case ToolType0.CreateNew:
                    this.DisposeSingle1();
                    break;
                default:
                    break;
            }
        }

        public override void Over()
        {
        }

        public override void UpdateCanvasControl1()
        {
            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.UpdateCanvas
                //| InvalidateModes.UpdateLayers
                | InvalidateModes.CanvasControl);
        }

        public override void UpdateCanvasControl2()
        {
            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.UpdateCanvas
                | InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }

        #region Transform
        private void CacheTranslation()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.CacheTranslation();
            }
        }
        private void CacheTransform()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.CacheTransform();
            }
        }
        private void Translate()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.Translate(this.Cropper.HostTranslateX, this.Cropper.HostTranslateY);
            }
        }
        private void Transform()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.Transform(this.Cropper.HostMatrix);
            }
        }
        private void RectChooses()
        {
            foreach (Layer item in this.Layers)
            {
                item.RectChoose(this.RectChoose);
            }
        }
        #endregion

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.InitLayers))
            {
                for (int i = 0; i < this.Layers.Count; i++)
                {
                    Layer item = this.Layers[i];

                    item.UpdateCanvas();
                }
            }

            if (modes.HasFlag(InvalidateModes.InitIndicator))
            {
                this.Cropper.BeginExtend();
                foreach (Layer item in this.Layers)
                {
                    if (item.IsSelected)
                    {
                        switch (this.Cropper.Count)
                        {
                            case 0:
                                this.Cropper.Reset(item);
                                break;
                            default:
                                this.Cropper.Extend(item.Bounds);
                                break;
                        }
                    }
                }
                this.Cropper.EndExtend();

                switch (this.Cropper.Count)
                {
                    case 0: this.Indicator.ClearAll(); break;
                    default: this.Indicator.ChangeAll(this.Cropper.Bounds, this.ParameterPanel.Mode); break;
                }

                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Cropper.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateLayers))
            {
                for (int i = 0; i < this.Layers.Count; i++)
                {
                    Layer item = this.Layers[i];

                    item.UpdateCanvas();
                }
            }

            if (modes.HasFlag(InvalidateModes.UpdateIndicator))
            {
                this.ParameterPanel.UpdateAll(this.Indicator);

                this.Cropper.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.CanvasControl))
            {
                this.Invalidate();
            }
        }

        #region CreateNew
        private void CacheSingle1()
        {
            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;

            Bounds bounds = new Bounds(this.StartingPoint, this.Point, this.KeepRatio, this.CenteredScaling);
            Layer createNew = new Layer(width, height, bounds)
            {
                IsSelected = true,
            };

            createNew.UpdateCanvas();

            foreach (Layer item in this.Layers)
            {
                item.IsSelected = false;
            }
            this.Layers.Add(createNew);

            this.Invalidate(InvalidateModes.None
            //| InvalidateModes.UpdateLayers
            | InvalidateModes.InitIndicator
            | InvalidateModes.CanvasControl);
        }

        private void Single1()
        {
            Bounds bounds = new Bounds(this.StartingPoint, this.Point, this.KeepRatio, this.CenteredScaling);

            Layer createNew = this.Layers.Last();
            createNew.UpdateDestination(bounds);

            createNew.UpdateCanvas();

            this.Invalidate(InvalidateModes.None
            //| InvalidateModes.UpdateLayers
            | InvalidateModes.InitIndicator
            | InvalidateModes.CanvasControl);
        }

        private void DisposeSingle1()
        {
            this.Invalidate(InvalidateModes.CanvasControl);
        }

        private static bool ContainsPoint(Layer item, Vector2 point)
        {
            return item.ActualBox.ContainsPoint(point);
        }
        #endregion

        #region Crop0
        private void CacheSingle0()
        {
            const float d = 12f;
            const float ds = d * d;

            switch (this.Cropper.Count)
            {
                case 0:
                    this.Mode = BoxContainsNodeMode.None;
                    break;
                default:
                    this.Mode = this.Cropper.ActualBox.ContainsNode(this.StartingPoint, ds);
                    break;
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    this.Cropper.Reset();
                    this.Invalidate(InvalidateModes.UpdateIndicator);

                    foreach (Layer item in this.Layers)
                    {
                        if (this.Mode == BoxContainsNodeMode.None)
                        {
                            item.IsSelected = ContainsPoint(item, this.StartingPoint);
                            if (item.IsSelected)
                            {
                                // None -> Contains
                                item.CacheTranslation(); // Single Translation 1
                                this.Mode = BoxContainsNodeMode.Contains;

                                this.Cropper.Reset(item.Bounds);  // Single Translation 2
                            }
                        }
                        else
                        {
                            if (item.IsSelected)
                            {
                                item.IsSelected = false;
                            }
                        }
                    }

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.Contains:
                    // Multiple Translation 1
                    this.CacheTranslation();
                    break;
                default:
                    // Multiple Transform 1
                    this.CacheTransform();
                    break;
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    this.RectChoose = new Bounds(this.StartingPoint);
                    this.ActualRectChoose = new Quadrilateral(this.StartingPoint);

                    this.HasRectChoose = true;
                    this.Invalidate(InvalidateModes.CanvasControl);
                    break;
                // Multiple Translation 2
                case BoxContainsNodeMode.Contains: this.Cropper.CacheTranslation(); break;

                // Multiple Transform 2
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom: this.Cropper.CacheRotation(this.StartingPoint); break;

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
                    this.RectChoose = new Bounds(this.StartingPoint, this.Point);
                    this.ActualRectChoose = new Quadrilateral(this.RectChoose);

                    this.Invalidate(InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.Contains:
                    this.Cropper.Translate(this.Indicator, this.ParameterPanel.Mode, this.StartingPoint, this.Point);

                    this.Translate();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom:
                    if (this.HasStepFrequency)
                        this.Cropper.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Point, StepFrequency);
                    else
                        this.Cropper.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Point);

                    this.Transform();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.HandleLeft:
                case BoxContainsNodeMode.HandleTop:
                case BoxContainsNodeMode.HandleRight:
                case BoxContainsNodeMode.HandleBottom:
                    this.Cropper.TransformSkew(this.Indicator, this.ParameterPanel.Mode, this.Point, this.KeepRatio, this.CenteredScaling);

                    this.Transform();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                default:
                    this.Cropper.TransformSize(this.Indicator, this.ParameterPanel.Mode, this.Point, this.KeepRatio, this.CenteredScaling);

                    this.Transform();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateIndicator
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }

        private void DisposeSingle0()
        {
            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    this.RectChoose = new Bounds(this.StartingPoint, this.Point);
                    this.ActualRectChoose = new Quadrilateral(this.RectChoose);

                    this.RectChooses();
                    this.HasRectChoose = false;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitIndicator
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region SizeType
        private void Remove()
        {
            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                Layer item = this.Layers[i];
                if (item.IsSelected)
                {
                    this.Layers.RemoveAt(i);
                }
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitIndicator
                | InvalidateModes.CanvasControl);
        }

        private void DeselectAll()
        {
            foreach (Layer item in this.Layers)
            {
                item.IsSelected = false;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitIndicator
                | InvalidateModes.CanvasControl);
        }

        private void SelectAll()
        {
            foreach (Layer item in this.Layers)
            {
                item.IsSelected = true;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitIndicator
                | InvalidateModes.CanvasControl);
        }

        private void SelectNext()
        {
            int index = 0;
            for (int i = 1; i < this.Layers.Count; i++)
            {
                if (this.Layers[i - 1].IsSelected)
                {
                    index = i;
                    break;
                }
            }

            for (int i = 0; i < this.Layers.Count; i++)
            {
                Layer item = this.Layers[i];

                item.IsSelected = i == index;
            }
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitIndicator
                | InvalidateModes.CanvasControl);
        }

        private void Draw0(CanvasDrawingSession drawingSession)
        {
            if (this.HasRectChoose)
            {
                drawingSession.FillRectChoose(new Rectangle(this.RectChoose));

                drawingSession.DrawRectChoose(this.ActualRectChoose);
            }
            else
            {
                switch (this.Cropper.Count)
                {
                    case 0:
                        break;
                    default:
                        drawingSession.DrawBox(this.Cropper.ActualBox);
                        break;
                }
            }
        }

        private void Apply(IndicatorKind kind, float value)
        {
            BoxMode mode = this.ParameterPanel.Mode;

            switch (this.Cropper.CropperSizeType(kind))
            {
                case CropperSizeType.None:
                    break;
                case CropperSizeType.X:
                    float translateX = value - this.Indicator.X;

                    this.Cropper.SetTranslationX(this.Indicator, mode, translateX);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.UpdateDestination(this.Cropper.Bounds);
                        }
                    }
                    break;
                case CropperSizeType.Y:
                    float translateY = value - this.Indicator.Y;

                    this.Cropper.SetTranslationY(this.Indicator, mode, translateY);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.UpdateDestination(this.Cropper.Bounds);
                        }
                    }
                    break;
                case CropperSizeType.Width:
                    this.Cropper.SetWidth(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.UpdateDestination(this.Cropper.Bounds);
                        }
                    }
                    break;
                case CropperSizeType.Height:
                    this.Cropper.SetHeight(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.UpdateDestination(this.Cropper.Bounds);
                        }
                    }
                    break;
                case CropperSizeType.MultiX:
                    float translateXs = value - this.Indicator.X;

                    this.Cropper.SetTranslationX(this.Indicator, mode, translateXs);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTranslationX(translateXs);
                        }
                    }
                    break;
                case CropperSizeType.MultiY:
                    float translateYs = value - this.Indicator.Y;

                    this.Cropper.SetTranslationY(this.Indicator, mode, translateYs);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTranslationY(translateYs);
                        }
                    }
                    break;
                case CropperSizeType.MultiWidth:
                    this.Cropper.SetWidth(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTransform(this.Cropper.HostMatrix);
                        }
                    }
                    break;
                case CropperSizeType.MultiHeight:
                    this.Cropper.SetHeight(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTransform(this.Cropper.HostMatrix);
                        }
                    }
                    break;
                default:
                    break;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateIndicator
                | InvalidateModes.CanvasControl);
        }
        #endregion
    }
}