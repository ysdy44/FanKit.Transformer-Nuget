using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Input;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Polylines;
using FanKit.Transformer.Sample;
using FanKit.Transformer.Transforms;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using Layer = FanKit.Transformer.Demos.DemoPolyline3;
using Figure = FanKit.Transformer.Polylines.Figure3;
using Segment = FanKit.Transformer.Polylines.Segment3;
using InvalidateModes = FanKit.Transformer.Sample.InvalidateModes12345;
using LineContainsNodeMode = FanKit.Transformer.Cache.LineContainsNodeMode2;
using BoxContainsNodeMode = FanKit.Transformer.Cache.BoxContainsNodeMode2;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="DemoHostComposer2"/>
    /// </summary>
    public sealed partial class CanvasPolylines2Page : CanvasSolo2Page
    {
        //@Key
        bool IsKeyDown(VirtualKey key) => Window.Current.CoreWindow.GetKeyState(key).HasFlag(CoreVirtualKeyStates.Down);
        bool IsCtrl => this.IsKeyDown(VirtualKey.Control);
        bool IsShift => this.IsKeyDown(VirtualKey.Shift);

        bool NodeCenteredScaling => this.IsCtrl || this.ToolBox7.CenteredScaling;
        bool NodeKeepRatio => this.IsShift || this.ToolBox7.KeepRatio;

        bool CenteredScaling => this.IsCtrl || this.ToolBox2.CenteredScaling;
        bool KeepRatio => this.IsShift || this.ToolBox2.KeepRatio;

        bool HasStepFrequency => this.IsShift;

        //@Const
        const float X1 = 128f;
        const float Y1 = 256f;

        const float X2 = 384f;
        const float Y2 = 256f;

        const float StepFrequency = (float)System.Math.PI / 12f;
        const float StepFrequencyHalf = (float)System.Math.PI / 24f;

        // MultiCanvas
        Bounds RectChoose;
        TransformedBounds ActualRectChoose;
        bool HasRectChoose;

        // TransformerMulti2
        BoxContainsNodeMode Mode;
        readonly DemoHostTriangle2 Transformer = new DemoHostTriangle2();

        // ComposerMulti2
        int LayerIndex = -1;
        int FigureIndex = -1;
        SegmentIndexer Indexer = SegmentIndexer.Empty;
        //NodeController Controller;

        FootPointer FootPoint;
        SegmentInserter Inserter = SegmentInserter.Empty;
        //NodeSmooth Smooth;

        bool PointMode;
        LineContainsNodeMode LineMode;
        BoxContainsNodeMode PanelMode;
        readonly DemoHostComposer2 Composer = new DemoHostComposer2();

        readonly Indicator Indicator = new Indicator();
        readonly List<Layer> Layers = new List<Layer>
        {
            new Layer(new List<Figure>
            {
                new Figure
                {
                    Segments =
                    {
                        new Segment{ Raw = new Vector2(X1 - 144.6047f, Y1 - 138.5997f) },
                        new Segment{ Raw = new Vector2(X1 + 13.37953f, Y1 - 95.3983f) },
                        new Segment{ Raw = new Vector2(X1 - 12.58583f, Y1 + 87.00745f) },
                        new Segment{ Raw = new Vector2(X1 + 144.6047f, Y1 + 138.5997f) },
                    }
                }
            })
            {
                IsSelected = true,
                StrokeWidth = 4f,
            },
            new Layer(new List<Figure>
            {
                new Figure
                {
                    Segments =
                    {
                        new Segment{ Raw = new Vector2(X2 - 144.6047f, Y2 - 138.5997f) },
                        new Segment{ Raw = new Vector2(X2 + 13.37953f, Y2 - 95.3983f) },
                        new Segment{ Raw = new Vector2(X2 - 12.58583f, Y2 + 87.00745f) },
                        new Segment{ Raw = new Vector2(X2 + 144.6047f, Y2 + 138.5997f) },
                    }
                }
            })
            {
                IsSelected = true,
                StrokeWidth = 4f,
            },
        };

        readonly TopBar2 TopBar = new TopBar2();
        readonly ToolBox2 ToolBox2 = new ToolBox2
        {
            Title = "Transform"
        };
        readonly ToolBox4 ToolBox4 = new ToolBox4
        {
            Visibility = Visibility.Collapsed,
            Title = "Create New"
        };
        readonly ToolBox7 ToolBox7 = new ToolBox7
        {
            Visibility = Visibility.Collapsed,
            Title = "Transform Node"
        };

        public CanvasPolylines2Page()
        {
            this.Child = this.TopBar;
            this.Children.Add(this.ToolBox2);
            this.Children.Add(this.ToolBox4);
            this.Children.Add(this.ToolBox7);

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

            this.ParameterPanel.ModeChanged += (s, e) => this.Indicator.ChangeXY(this.Composer.PanelDestination, e);
            this.ParameterPanel.RowModeChanged += (s, e) => this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, e);
            this.ParameterPanel.ColumnModeChanged += (s, e) => this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, e);

            this.ParameterPanel.Apply += (s, e) =>
            {
                switch (this.TopBar.ToolType)
                {
                    case ToolType2.Transform: this.Apply0(e, this.ParameterPanel.Value); break;
                    default: this.Apply1(e, this.ParameterPanel.Value); break;
                }
            };

            this.TopBar.ToolTypeChanged += delegate
            {
                switch (this.TopBar.ToolType)
                {
                    case ToolType2.Transform:
                        this.ToolBox2.Visibility = Visibility.Visible;
                        this.ToolBox4.Visibility = Visibility.Collapsed;
                        this.ToolBox7.Visibility = Visibility.Collapsed;
                        break;
                    case ToolType2.NodeCreateNew:
                        this.ToolBox2.Visibility = Visibility.Collapsed;
                        this.ToolBox4.Visibility = Visibility.Visible;
                        this.ToolBox7.Visibility = Visibility.Collapsed;
                        break;
                    case ToolType2.NodeMove:
                        this.ToolBox2.Visibility = Visibility.Collapsed;
                        this.ToolBox4.Visibility = Visibility.Collapsed;
                        this.ToolBox7.Visibility = Visibility.Collapsed;
                        break;
                    case ToolType2.NodeTransform:
                        this.ToolBox2.Visibility = Visibility.Collapsed;
                        this.ToolBox4.Visibility = Visibility.Collapsed;
                        this.ToolBox7.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }

                this.Invalidate(InvalidateModes.None
                    | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                    | InvalidateModes.CanvasControl);
            };

            this.ToolBox2.RemoveClick += delegate { this.Remove(); };
            this.ToolBox2.DeselectAllClick += delegate { this.DeselectAll(); };
            this.ToolBox2.SelectAllClick += delegate { this.SelectAll(); };
            this.ToolBox2.SelectNextClick += delegate { this.SelectNext(); };

            this.ToolBox7.CloseClick += delegate { this.NodeClose(); };
            this.ToolBox7.OpenClick += delegate { this.NodeOpen(); };
            this.ToolBox7.RemoveClick += delegate { this.NodeRemove(); };

            this.ToolBox4.EndClick += delegate { this.End(); };
        }

        public override void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        {
            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitCanvas
                | InvalidateModes.InitLayers
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer);
        }

        public override void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            foreach (Layer item in this.Layers)
            {
                foreach (Figure figure in item.Figures)
                {
                    drawingSession.DrawDashActualPolyline(figure.Segments, figure.IsClosed, item.ActualStrokeWidth);
                }
            }

            switch (this.TopBar.ToolType)
            {
                case ToolType2.Transform: this.Draw0(drawingSession); break;
                case ToolType2.NodeCreateNew: this.Draw1(drawingSession); break;
                case ToolType2.NodeMove: this.Draw1(drawingSession); this.Draw2(drawingSession); break;
                case ToolType2.NodeTransform: this.Draw3(drawingSession); break;
                default: break;
            }
        }

        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            foreach (Layer item in this.Layers)
            {
                foreach (Figure figure in item.Figures)
                {
                    drawingSession.DrawDashRawPolyline(figure.Segments, figure.IsClosed, item.StrokeWidth);
                }
            }
        }

        public override void CacheSingle()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType2.Transform: this.CacheSingle0(); break;
                case ToolType2.NodeCreateNew: this.CacheSingle1(); break;
                case ToolType2.NodeMove: this.CacheSingle2(); break;
                case ToolType2.NodeTransform:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty: this.CacheSingle3(); break;
                        case SizeType.Point: this.CacheSingle4(); break;
                        case SizeType.RowLine:
                        case SizeType.ColumnLine: this.CacheSingle5(); break;
                        case SizeType.Panel: this.CacheSingle6(); break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        public override void Single()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType2.Transform: this.Single0(); break;
                case ToolType2.NodeCreateNew: this.Single1(); break;
                case ToolType2.NodeMove: this.Single2(); break;
                case ToolType2.NodeTransform:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty: this.Single3(); break;
                        case SizeType.Point: this.Single4(); break;
                        case SizeType.RowLine: this.SingleRow5(); break;
                        case SizeType.ColumnLine: this.SingleColumn5(); break;
                        case SizeType.Panel: this.Single6(); break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        public override void DisposeSingle()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType2.Transform: this.DisposeSingle0(); break;
                case ToolType2.NodeCreateNew: this.DisposeSingle1(); break;
                case ToolType2.NodeMove: this.DisposeSingle2(); break;
                case ToolType2.NodeTransform:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty: this.DisposeSingle3(); break;
                        case SizeType.Point: this.DisposeSingle4(); break;
                        case SizeType.RowLine:
                        case SizeType.ColumnLine: this.DisposeSingle5(); break;
                        case SizeType.Panel: this.DisposeSingle6(); break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        public override void Over()
        {
            switch (this.TopBar.ToolType)
            {
                case ToolType2.Transform: break;
                case ToolType2.NodeCreateNew: this.Over1(); break;
                case ToolType2.NodeMove: this.Over2(); break;
                case ToolType2.NodeTransform: break;
                default: break;
            }
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
                | InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateComposer | InvalidateModes.UpdateTransformer
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
                    item.Translate(this.Transformer.TranslationX, this.Transformer.TranslationY);
            }
        }
        private void Transform()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.Transform(this.Transformer.TransformMatrix);
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

        #region TransformSelectedItems
        private void CacheTranslationSelectedItems()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.CacheTranslationSelectedItems();
            }
        }
        private void CacheTransformSelectedItems()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.CacheTransformSelectedItems();
            }
        }
        private void TranslateSelectedItems()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.TranslateSelectedItems(this.Composer.TranslationX, this.Composer.TranslationY);
            }
        }
        private void TransformSelectedItems()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.TransformSelectedItems(this.Composer.TransformMatrix);
            }
        }
        private void RectChooseItems()
        {
            foreach (Layer item in this.Layers)
            {
                item.RectChooseItems(this.RectChoose);
            }
        }
        #endregion

        #region SetTransformSelectedItems
        private void SetTranslationXSelectedItems()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.SetTranslationXSelectedItems(this.Composer.TranslationX);
            }
        }
        private void SetTranslationYSelectedItems()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.SetTranslationYSelectedItems(this.Composer.TranslationY);
            }
        }
        private void SetTransformSelectedItems()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                    item.SetTransformSelectedItems(this.Composer.TransformMatrix);
            }
        }
        #endregion

        private void Invalidate(InvalidateModes modes)
        {
            if (modes.HasFlag(InvalidateModes.InitCanvas))
            {
                this.InitCanvas();
            }

            if (modes.HasFlag(InvalidateModes.InitLayers))
            {
                foreach (Layer item in this.Layers)
                {
                    item.UpdateCanvas(this.Canvas);
                }
            }

            if (modes.HasFlag(InvalidateModes.InitComposer))
            {
                this.Composer.BeginExtend();
                foreach (Layer item in this.Layers)
                {
                    if (item.IsSelected)
                    {
                        foreach (Figure figure in item.Figures)
                        {
                            foreach (Segment segment in figure.Segments)
                            {
                                this.Composer.Extend(segment);
                            }
                        }
                    }
                }
                this.Composer.EndExtendByPoints();

                switch (this.TopBar.ToolType)
                {
                    case ToolType2.NodeCreateNew:
                    case ToolType2.NodeMove:
                    case ToolType2.NodeTransform:
                        switch (this.Composer.SizeType)
                        {
                            case SizeType.Empty: this.Indicator.ClearAll(); break;
                            case SizeType.Point: this.Indicator.ChangeAll(this.Composer.PointPoint); break;
                            case SizeType.RowLine: this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, this.ParameterPanel.RowMode); break;
                            case SizeType.ColumnLine: this.Indicator.ChangeAll(this.Composer.LinePoint0, this.Composer.LinePoint1, this.ParameterPanel.ColumnMode); break;
                            case SizeType.Panel: this.Indicator.ChangeAll(this.Composer.PanelDestination, this.ParameterPanel.Mode); break;
                            default: break;
                        }
                        this.ParameterPanel.UpdateAll(this.Indicator);
                        break;
                    default:
                        break;
                }

                this.Composer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.InitTransformer))
            {
                this.Transformer.BeginExtend();
                foreach (Layer item in this.Layers)
                {
                    if (item.IsSelected)
                    {
                        switch (this.Transformer.Count)
                        {
                            case 0:
                                this.Transformer.Reset(item);
                                break;
                            default:
                                this.Transformer.Extend(item.Destination);
                                break;
                        }
                    }
                }
                this.Transformer.EndExtend();

                switch (this.TopBar.ToolType)
                {
                    case ToolType2.Transform:
                        switch (this.Transformer.Count)
                        {
                            case 0: this.Indicator.ClearAll(); break;
                            default: this.Indicator.ChangeAll(this.Transformer.Destination, this.ParameterPanel.Mode); break;
                        }
                        this.ParameterPanel.UpdateAll(this.Indicator);
                        break;
                    default:
                        break;
                }

                this.Transformer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.UpdateCanvas))
            {
                this.UpdateCanvas();
            }

            if (modes.HasFlag(InvalidateModes.UpdateLayers))
            {
                foreach (Layer item in this.Layers)
                {
                    item.UpdateCanvas(this.Canvas);
                }
            }

            if (modes.HasFlag(InvalidateModes.UpdateComposer))
            {
                switch (this.TopBar.ToolType)
                {
                    case ToolType2.NodeCreateNew:
                    case ToolType2.NodeMove:
                    case ToolType2.NodeTransform:
                        this.ParameterPanel.UpdateAll(this.Indicator);
                        break;
                    default:
                        break;
                }

                this.Composer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.UpdateTransformer))
            {
                switch (this.TopBar.ToolType)
                {
                    case ToolType2.Transform:
                        this.ParameterPanel.UpdateAll(this.Indicator);
                        break;
                    default:
                        break;
                }

                this.Transformer.UpdateCanvas(this.Canvas);
            }

            if (modes.HasFlag(InvalidateModes.CanvasControl))
            {
                this.Invalidate();
            }
        }

        #region LineTransform2
        private void CacheSingle5()
        {
            const float d = 12f;
            const float ds = d * d;

            this.LineMode = this.Composer.ActualLine.ContainsNode(this.StartingPoint, ds);

            if (this.LineMode != LineContainsNodeMode.None)
            {
                this.CacheTransformSelectedItems();
            }

            switch (this.LineMode)
            {
                case LineContainsNodeMode.None: this.CacheSingle3(); break;
                /*
                case LineContainsNodeMode.Handle0: this.Composer.LineCacheElongation0(); break;
                case LineContainsNodeMode.Handle1: this.Composer.LineCacheElongation1(); break;
                 */
                case LineContainsNodeMode.Handle: this.Composer.LineCacheRotation(this.StartingPosition); break;
                case LineContainsNodeMode.Center: this.Composer.LineCacheTranslation(); break;
                case LineContainsNodeMode.Point0: this.Composer.LineCacheMovement(); break;
                case LineContainsNodeMode.Point1: this.Composer.LineCacheMovement(); break;
                default: break;
            }
        }

        private void SingleRow5()
        {
            switch (this.LineMode)
            {
                case LineContainsNodeMode.None:
                    this.Single3();
                    break;
                /*
                case LineContainsNodeMode.Handle0:
                    this.Composer.LineElongatePoint0(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Composer.LineElongatePoint1(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.RowMode, this.Position, StepFrequency);
                    else
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Center:
                    this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Composer.LineMovePoint0(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Composer.LineMovePoint1(this.Indicator, this.ParameterPanel.RowMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }

        private void SingleColumn5()
        {
            switch (this.LineMode)
            {
                case LineContainsNodeMode.None:
                    this.Single3();
                    break;
                /*
                case LineContainsNodeMode.Handle0:
                    this.Composer.LineElongatePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Handle1:
                    this.Composer.LineElongatePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                 */
                case LineContainsNodeMode.Handle:
                    if (this.HasStepFrequency)
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Position, StepFrequency);
                    else
                        this.Composer.LineRotate(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Center:
                    this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point0:
                    this.Composer.LineMovePoint0(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case LineContainsNodeMode.Point1:
                    this.Composer.LineMovePoint1(this.Indicator, this.ParameterPanel.ColumnMode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }

        private void DisposeSingle5()
        {
            switch (this.LineMode)
            {
                case LineContainsNodeMode.None:
                    this.DisposeSingle3();
                    break;
                default:
                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.InitComposer
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }
        #endregion

        #region PanelTransform2
        private void CacheSingle6()
        {
            const float d = 12f;
            const float ds = d * d;

            switch (this.Composer.Count)
            {
                case 0:
                    this.PanelMode = BoxContainsNodeMode.None;
                    break;
                default:
                    this.PanelMode = this.Composer.ActualBox.ContainsNode(this.StartingPoint, ds);
                    break;
            }

            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None:
                    break;
                case BoxContainsNodeMode.Contains:
                    // Multiple Translation 1
                    this.CacheTranslationSelectedItems();
                    break;
                default:
                    // Multiple Transform 1
                    this.CacheTransformSelectedItems();
                    break;
            }

            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None: this.CacheSingle3(); break;
                // Multiple Translation 2
                case BoxContainsNodeMode.Contains: this.Composer.PanelCacheTranslation(); break;

                // Multiple Transform 2
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom: this.Composer.PanelCacheRotation(this.StartingPosition); break;
                 */

                case BoxContainsNodeMode.HandleLeft: this.Composer.PanelCacheTransform(TransformMode.SkewLeft); break;
                case BoxContainsNodeMode.HandleTop: this.Composer.PanelCacheTransform(TransformMode.SkewTop); break;
                case BoxContainsNodeMode.HandleRight: this.Composer.PanelCacheTransform(TransformMode.SkewRight); break;
                case BoxContainsNodeMode.HandleBottom: this.Composer.PanelCacheTransform(TransformMode.SkewBottom); break;

                case BoxContainsNodeMode.CenterLeft: this.Composer.PanelCacheTransform(TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode.CenterTop: this.Composer.PanelCacheTransform(TransformMode.ScaleTop); break;
                case BoxContainsNodeMode.CenterRight: this.Composer.PanelCacheTransform(TransformMode.ScaleRight); break;
                case BoxContainsNodeMode.CenterBottom: this.Composer.PanelCacheTransform(TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode.LeftTop: this.Composer.PanelCacheTransform(TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode.RightTop: this.Composer.PanelCacheTransform(TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode.LeftBottom: this.Composer.PanelCacheTransform(TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode.RightBottom: this.Composer.PanelCacheTransform(TransformMode.ScaleRightBottom); break;
                default: break;
            }
        }

        private void Single6()
        {
            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None:
                    this.Single3();
                    break;
                case BoxContainsNodeMode.Contains:
                    this.Composer.PanelTranslate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);

                    this.TranslateSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom:
                    break;
                 */
                case BoxContainsNodeMode.HandleLeft:
                    break;
                case BoxContainsNodeMode.HandleTop:
                    if (this.HasStepFrequency)
                        this.Composer.PanelRotate(this.Indicator, this.ParameterPanel.Mode, this.Position, StepFrequency);
                    else
                        this.Composer.PanelRotate(this.Indicator, this.ParameterPanel.Mode, this.Position);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.HandleRight:
                case BoxContainsNodeMode.HandleBottom:
                    this.Composer.PanelTransformSkew(this.Indicator, this.ParameterPanel.Mode, this.Position, this.NodeKeepRatio, this.NodeCenteredScaling);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    this.Composer.PanelTransformSize(this.Indicator, this.ParameterPanel.Mode, this.Position, this.NodeKeepRatio, this.NodeCenteredScaling);

                    this.TransformSelectedItems();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }

        private void DisposeSingle6()
        {
            switch (this.PanelMode)
            {
                case BoxContainsNodeMode.None:
                    this.DisposeSingle3();
                    break;
                default:
                    this.Invalidate(InvalidateModes.None
                        //| InvalidateModes.InitComposer
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }
        #endregion

        #region ToolBox7
        private void NodeClose()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                {
                    foreach (Figure figure in item.Figures)
                    {
                        if (figure.IsClosed is false)
                        {
                            figure.Setting.UpdateSelectedMode(item.Figures);

                            if (figure.Setting.SelectedMode != SelectedMode.UnSelected)
                            {
                                figure.IsClosed = true;
                            }
                        }
                    }
                }
            }

            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }

        private void NodeOpen()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                {
                    foreach (Figure figure in item.Figures)
                    {
                        if (figure.IsClosed)
                        {
                            figure.Setting.UpdateSelectedMode(item.Figures);

                            if (figure.Setting.SelectedMode != SelectedMode.UnSelected)
                            {
                                figure.IsClosed = false;
                            }
                        }
                    }
                }
            }

            this.Invalidate(InvalidateModes.None
                //| InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }

        private void NodeRemove()
        {
            for (int k = this.Layers.Count - 1; k >= 0; k--)
            {
                Layer item = this.Layers[k];
                if (item.IsSelected)
                {
                    item.Setting.UpdateRemoveMode(item.Figures);

                    switch (item.Setting.RemoveMode)
                    {
                        case RemoveMode.NoRemove:
                            break;
                        case RemoveMode.RemoveCurve:
                            this.Layers.RemoveAt(k);
                            break;
                        case RemoveMode.RemoveNodes:
                            for (int j = item.Figures.Count - 1; j >= 0; j--)
                            {
                                Figure figure = item.Figures[j];

                                switch (figure.Setting.RemoveMode)
                                {
                                    case RemoveMode.NoRemove:
                                        break;
                                    case RemoveMode.RemoveCurve:
                                        figure.Segments.RemoveAt(j);
                                        break;
                                    case RemoveMode.RemoveNodes:
                                        for (int i = figure.Segments.Count - 1; i >= 0; i--)
                                        {
                                            Segment segment = figure.Segments[i];

                                            if (segment.IsChecked)
                                            {
                                                figure.Segments.RemoveAt(i);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            item.Complete();
                            break;
                        default:
                            break;
                    }
                }
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitLayers
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }
        #endregion

        #region Transform2
        private void CacheSingle0()
        {
            const float d = 12f;
            const float ds = d * d;

            switch (this.Transformer.Count)
            {
                case 0:
                    this.Mode = BoxContainsNodeMode.None;
                    break;
                default:
                    this.Mode = this.Transformer.ActualBox.ContainsNode(this.StartingPoint, ds);
                    break;
            }

            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    this.Transformer.Reset();
                    this.Invalidate(InvalidateModes.UpdateTransformer);

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

                                this.Transformer.Reset(item.Destination);  // Single Translation 2
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
                        | InvalidateModes.InitTransformer
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
                    this.RectChoose = new Bounds(this.StartingPosition);
                    this.ActualRectChoose = new TransformedBounds(this.StartingPoint);

                    this.HasRectChoose = true;
                    this.Invalidate(InvalidateModes.CanvasControl);
                    break;
                // Multiple Translation 2
                case BoxContainsNodeMode.Contains: this.Transformer.CacheTranslation(); break;

                // Multiple Transform 2
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom: break;
                 */

                case BoxContainsNodeMode.HandleLeft: break;
                case BoxContainsNodeMode.HandleTop: this.Transformer.CacheRotation(this.StartingPosition); break;
                case BoxContainsNodeMode.HandleRight: this.Transformer.CacheTransform(TransformMode.SkewRight); break;
                case BoxContainsNodeMode.HandleBottom: this.Transformer.CacheTransform(TransformMode.SkewBottom); break;

                case BoxContainsNodeMode.CenterLeft: this.Transformer.CacheTransform(TransformMode.ScaleLeft); break;
                case BoxContainsNodeMode.CenterTop: this.Transformer.CacheTransform(TransformMode.ScaleTop); break;
                case BoxContainsNodeMode.CenterRight: this.Transformer.CacheTransform(TransformMode.ScaleRight); break;
                case BoxContainsNodeMode.CenterBottom: this.Transformer.CacheTransform(TransformMode.ScaleBottom); break;

                case BoxContainsNodeMode.LeftTop: this.Transformer.CacheTransform(TransformMode.ScaleLeftTop); break;
                case BoxContainsNodeMode.RightTop: this.Transformer.CacheTransform(TransformMode.ScaleRightTop); break;
                case BoxContainsNodeMode.LeftBottom: this.Transformer.CacheTransform(TransformMode.ScaleLeftBottom); break;
                case BoxContainsNodeMode.RightBottom: this.Transformer.CacheTransform(TransformMode.ScaleRightBottom); break;
                default: break;
            }
        }

        private void Single0()
        {
            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    this.RectChoose = new Bounds(this.StartingPosition, this.Position);
                    this.ActualRectChoose = this.RectChoose * this.Canvas.Matrix;

                    this.Invalidate(InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.Contains:
                    this.Transformer.Translate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);

                    this.Translate();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateTransformer
                        | InvalidateModes.CanvasControl);
                    break;
                /*
                case BoxContainsNodeMode.HandleLeftTop:
                case BoxContainsNodeMode.HandleRightTop:
                case BoxContainsNodeMode.HandleLeftBottom:
                case BoxContainsNodeMode.HandleRightBottom:
                    break;
                 */
                case BoxContainsNodeMode.HandleLeft:
                    break;
                case BoxContainsNodeMode.HandleTop:
                    if (this.HasStepFrequency)
                        this.Transformer.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Position, StepFrequency);
                    else
                        this.Transformer.Rotate(this.Indicator, this.ParameterPanel.Mode, this.Position);

                    this.Transform();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateTransformer
                        | InvalidateModes.CanvasControl);
                    break;
                case BoxContainsNodeMode.HandleRight:
                case BoxContainsNodeMode.HandleBottom:
                    this.Transformer.TransformSkew(this.Indicator, this.ParameterPanel.Mode, this.Position, this.KeepRatio, this.CenteredScaling);

                    this.Transform();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateTransformer
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    this.Transformer.TransformSize(this.Indicator, this.ParameterPanel.Mode, this.Position, this.KeepRatio, this.CenteredScaling);

                    this.Transform();

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateLayers
                        | InvalidateModes.UpdateTransformer
                        | InvalidateModes.CanvasControl);
                    break;
            }
        }

        private void DisposeSingle0()
        {
            switch (this.Mode)
            {
                case BoxContainsNodeMode.None:
                    this.RectChoose = new Bounds(this.StartingPosition, this.Position);
                    this.ActualRectChoose = this.RectChoose * this.Canvas.Matrix;

                    this.RectChooses();
                    this.HasRectChoose = false;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitTransformer
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region NodeCreateNew
        private void CacheSingle1()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsEditable)
                {
                    item.DeselectAll();

                    Figure figure = item.Figures.Last();

                    figure.Segments.Add(new Segment
                    {
                        IsChecked = true,

                        Raw = this.StartingPosition,
                        Map = this.StartingPosition,
                        Actual = this.StartingPoint,
                    });

                    this.PenCurve(figure.Segments);

                    item.Complete();
                    item.UpdateCanvas(this.Canvas);

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                        | InvalidateModes.CanvasControl);
                    return;
                }
            }

            foreach (Layer item in this.Layers)
            {
                item.IsSelected = false;
            }

            this.Layers.Add(new Layer(new List<Figure>
            {
                new Figure
                {
                    Segments =
                    {
                        new Segment
                        {
                            IsChecked = false,

                        Raw = this.StartingPosition,
                        Map = this.StartingPosition,
                        Actual = this.StartingPoint,
                        },
                        new Segment
                        {
                            IsChecked = true,

                        Raw = this.StartingPosition,
                        Map = this.StartingPosition,
                        Actual = this.StartingPoint,
                        },
                    }
                }
            })
            {
                IsSelected = true,
                IsEditable = true,
            });

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }

        private void Single1()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsEditable)
                {
                    foreach (Figure figure in item.Figures)
                    {
                        this.PenCurve(figure.Segments);
                    }

                    item.Complete();
                    item.UpdateCanvas(this.Canvas);

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                        | InvalidateModes.CanvasControl);
                    return;
                }
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.CanvasControl);
        }

        private void DisposeSingle1()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsEditable)
                {
                    item.Complete();
                    item.UpdateCanvas(this.Canvas);

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                        | InvalidateModes.CanvasControl);
                    return;
                }
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.CanvasControl);
        }

        private void Over1()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsEditable)
                {
                    foreach (Figure figure in item.Figures)
                    {
                        this.PenCurve(figure.Segments);
                    }

                    item.Complete();
                    item.UpdateCanvas(this.Canvas);

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                        | InvalidateModes.CanvasControl);
                    return;
                }
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.CanvasControl);
        }

        private void PenCurve(List<Segment> items)
        {
            items[items.Count - 1] = new Segment
            {
                IsChecked = true,
                Starting = this.Position,

                Raw = this.Position,
                Map = this.Position,
                Actual = this.Canvas.Transform(this.Position),
            };
        }

        private bool ContainsPoint(Layer item, Vector2 point)
        {
            const float l = 4f;
            const float ls = l * l;

            for (int i = 0; i < item.Figures.Count; i++)
            {
                Figure figure = item.Figures[i];

                this.FootPoint = new Polylines.FootPointer(NodePointUnits.Actual, figure.Segments, figure.IsClosed, point, ls);
                if (this.FootPoint.Contains)
                {
                    return true;
                }
            }

            this.FootPoint = default;
            return false;
        }
        #endregion

        #region NodeMoves
        private void End()
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsEditable)
                {
                    item.IsEditable = false;

                    foreach (Figure figure in item.Figures)
                    {
                        figure.Segments.RemoveAt(figure.Segments.Count - 1);
                    }

                    item.Complete();
                    item.UpdateCanvas(this.Canvas);

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                        | InvalidateModes.CanvasControl);
                    return;
                }
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.CanvasControl);
        }

        private void Draw1(CanvasDrawingSession drawingSession)
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                {
                    drawingSession.DrawBounds(item.ActualBox);
                }
            }

            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                {
                    foreach (Figure figure in item.Figures)
                    {
                        foreach (Segment segment in figure.Segments)
                        {
                            if (segment.IsChecked)
                                drawingSession.DrawNode(segment.Actual);
                            else
                                drawingSession.DrawNode3(segment.Actual);
                        }
                    }
                }
            }
        }
        private void Draw2(CanvasDrawingSession drawingSession)
        {
            if (this.HasRectChoose)
            {
                drawingSession.Transform = this.Canvas.Matrix;
                drawingSession.FillRectChoose(new Rectangle(this.RectChoose));
                drawingSession.Transform = Matrix3x2.Identity;

                drawingSession.DrawRectChoose(this.ActualRectChoose);
            }
            else if (this.FootPoint.Contains)
            {
                Vector2 f = this.Canvas.Transform(this.FootPoint.Foot);

                Vector2 e = this.Canvas.Transform(this.FootPoint.LinePoint0);
                Vector2 n = this.Canvas.Transform(this.FootPoint.LinePoint1);

                drawingSession.DrawPreviousLine(f, e);
                drawingSession.DrawNextLine(f, n);

                if (this.IsInContact)
                {
                    drawingSession.DrawLine(this.Point, f);
                    drawingSession.DrawNode(this.Point);
                }
                else
                {
                    Vector2 p = this.Canvas.Transform(this.FootPoint.Point);

                    drawingSession.DrawLine(p, f);
                    drawingSession.DrawNode(p);
                }

                drawingSession.DrawNode(f);
            }
        }
        private void Draw3(CanvasDrawingSession drawingSession)
        {
            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                {
                    drawingSession.DrawBounds(item.ActualBox);
                }
            }

            foreach (Layer item in this.Layers)
            {
                if (item.IsSelected)
                {
                    foreach (Figure figure in item.Figures)
                    {
                        foreach (Segment segment in figure.Segments)
                        {
                            if (segment.IsChecked)
                                drawingSession.DrawNode(segment.Actual);
                            else
                                drawingSession.DrawNode3(segment.Actual);
                        }
                    }
                }
            }

            if (this.HasRectChoose)
            {
                drawingSession.Transform = this.Canvas.Matrix;
                drawingSession.FillRectChoose(new Rectangle(this.RectChoose));
                drawingSession.Transform = Matrix3x2.Identity;

                drawingSession.DrawRectChoose(this.ActualRectChoose);
            }
            else
            {
                switch (this.Composer.SizeType)
                {
                    case SizeType.Empty:
                        break;
                    case SizeType.Point:
                        drawingSession.DrawNode(this.Composer.ActualPoint);
                        break;
                    case SizeType.RowLine:
                    case SizeType.ColumnLine:
                        drawingSession.DrawLine(this.Composer.ActualLine);
                        break;
                    case SizeType.Panel:
                        drawingSession.DrawBox(this.Composer.ActualBox);
                        break;
                    default:
                        break;
                }
            }
        }

        private void CacheSingle2()
        {
            const float d = 12f;
            const float ds = d * d;

            this.Indexer = SegmentIndexer.Empty;

            switch (this.Indexer.Mode)
            {
                case SegmentIndexerMode.None:
                    for (int k = 0; k < this.Layers.Count; k++)
                    {
                        Layer item = this.Layers[k];
                        if (item.IsSelected)
                        {
                            for (int j = 0; j < item.Figures.Count; j++)
                            {
                                Figure figure = item.Figures[j];
                                this.Indexer = new SegmentIndexer(figure.Segments, this.StartingPoint, ds);

                                if (this.Indexer.Mode != SegmentIndexerMode.None)
                                {
                                    this.LayerIndex = k;
                                    this.FigureIndex = j;

                                    if (this.Indexer.Mode == SegmentIndexerMode.PointWithoutChecked) goto case SegmentIndexerMode.PointWithoutChecked;
                                    if (this.Indexer.Mode == SegmentIndexerMode.PointWithChecked) goto case SegmentIndexerMode.PointWithChecked;
                                }
                            }
                        }
                    }

                    float l = 4f * this.Canvas.InverseScaleFactor;
                    float ls = l * l;

                    for (int k = 0; k < this.Layers.Count; k++)
                    {
                        Layer item = this.Layers[k];
                        if (item.IsSelected)
                        {
                            for (int j = 0; j < item.Figures.Count; j++)
                            {
                                Figure figure = item.Figures[j];

                                this.Inserter = new SegmentInserter(ref this.FootPoint, NodePointUnits.Normal, figure.Segments, figure.IsClosed, this.Position, ls);

                                if (this.Inserter.Contains == default)
                                    continue;

                                foreach (Layer item2 in this.Layers)
                                {
                                    if (item2.IsSelected)
                                    {
                                        item2.DeselectAll();
                                    }
                                }

                                figure.Segments.Insert(this.Inserter.Index, new Segment
                                {
                                    IsChecked = true,

                                    Starting = this.FootPoint.Foot,

                                    Raw = Vector2.Transform(this.FootPoint.Foot, item.HomographyInverseMatrix),
                                    Map = this.FootPoint.Foot,
                                    Actual = this.Canvas.Transform(this.FootPoint.Foot),
                                });

                                this.LayerIndex = k;
                                this.FigureIndex = j;
                                this.Indexer = new SegmentIndexer
                                {
                                    Index = this.Inserter.Index,

                                    Mode = SegmentIndexerMode.PointWithoutChecked,
                                };

                                this.Composer.Reset(this.FootPoint.Foot);
                                this.HasRectChoose = false;
                                this.Invalidate(InvalidateModes.CanvasControl);
                                return;
                            }
                        }
                    }

                    this.CacheSingle3();
                    break;
                case SegmentIndexerMode.PointWithoutChecked:
                    for (int k = 0; k < this.Layers.Count; k++)
                    {
                        Layer item = this.Layers[k];
                        if (item.IsSelected)
                        {
                            if (this.LayerIndex == k)
                                item.Select(this.FigureIndex, this.Indexer.Index);
                            else
                                item.DeselectAll();
                        }
                    }

                    {
                        Layer item = this.Layers[this.LayerIndex];
                        Figure figure = item.Figures[this.FigureIndex];
                        Segment segment = figure.Segments[this.Indexer.Index];

                        this.Composer.Reset(segment.Map);

                        this.HasRectChoose = false;

                        this.Invalidate(InvalidateModes.None
                            | InvalidateModes.InitComposer
                            | InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.PointWithChecked:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        case SizeType.Point:
                            this.Composer.PointCacheTranslation();
                            break;
                        case SizeType.RowLine:
                            this.Composer.LineCacheTranslation();
                            break;
                        case SizeType.ColumnLine:
                            this.Composer.LineCacheTranslation();
                            break;
                        case SizeType.Panel:
                            this.Composer.PanelCacheTranslation();
                            break;
                        default:
                            break;
                    }

                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        default:
                            this.CacheTranslationSelectedItems();
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Single2()
        {
            switch (this.Indexer.Mode)
            {
                case SegmentIndexerMode.None:
                    this.Single3();
                    break;
                case SegmentIndexerMode.PointWithoutChecked:
                    {
                        this.Composer.PointTranslate(this.Indicator, this.StartingPosition, this.Position);

                        Layer item = this.Layers[this.LayerIndex];
                        item.SetTranslation(this.Composer.TranslationX, this.Composer.TranslationY, this.Composer.PointPoint, this.Indexer.Index);

                        this.Invalidate(InvalidateModes.None
                            | InvalidateModes.UpdateLayers
                            | InvalidateModes.UpdateComposer
                            | InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.PointWithChecked:
                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        case SizeType.Point:
                            this.Composer.PointTranslate(this.Indicator, this.StartingPosition, this.Position);
                            break;
                        case SizeType.RowLine:
                            this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.RowMode, this.StartingPosition, this.Position);
                            break;
                        case SizeType.ColumnLine:
                            this.Composer.LineTranslate(this.Indicator, this.ParameterPanel.ColumnMode, this.StartingPosition, this.Position);
                            break;
                        case SizeType.Panel:
                            this.Composer.PanelTranslate(this.Indicator, this.ParameterPanel.Mode, this.StartingPosition, this.Position);
                            break;
                        default:
                            break;
                    }

                    switch (this.Composer.SizeType)
                    {
                        case SizeType.Empty:
                            break;
                        default:
                            foreach (Layer item in this.Layers)
                            {
                                if (item.IsSelected)
                                {
                                    item.TranslateSelectedItems(this.Composer.TranslationX, this.Composer.TranslationY);
                                    item.UpdateCanvas(this.Canvas);
                                }
                            }

                            this.Invalidate(InvalidateModes.None
                                | InvalidateModes.UpdateLayers
                                | InvalidateModes.UpdateComposer
                                | InvalidateModes.CanvasControl);
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void DisposeSingle2()
        {
            switch (this.Composer.SizeType)
            {
                case SizeType.Empty:
                    this.DisposeSingle3();
                    break;
                case SizeType.Point:
                    if (this.HasRectChoose)
                        goto case SizeType.Empty;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case SizeType.RowLine:
                case SizeType.ColumnLine:
                    if (this.HasRectChoose)
                        goto case SizeType.Empty;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                case SizeType.Panel:
                    if (this.HasRectChoose)
                        goto case SizeType.Empty;

                    this.Invalidate(InvalidateModes.None
                        | InvalidateModes.UpdateComposer
                        | InvalidateModes.CanvasControl);
                    break;
                default:
                    break;
            }
        }

        private void Over2()
        {
            const float d = 12f;
            const float ds = d * d;

            this.Indexer = SegmentIndexer.Empty;

            switch (this.Indexer.Mode)
            {
                case SegmentIndexerMode.None:
                    for (int k = 0; k < this.Layers.Count; k++)
                    {
                        Layer item = this.Layers[k];
                        if (item.IsSelected)
                        {
                            for (int j = 0; j < item.Figures.Count; j++)
                            {
                                Figure figure = item.Figures[j];
                                this.Indexer = new SegmentIndexer(figure.Segments, this.Point, ds);

                                if (this.Indexer.Mode != SegmentIndexerMode.None)
                                {
                                    this.LayerIndex = k;
                                    this.FigureIndex = j;

                                    if (this.Indexer.Mode == SegmentIndexerMode.PointWithoutChecked) goto case SegmentIndexerMode.PointWithoutChecked;
                                    if (this.Indexer.Mode == SegmentIndexerMode.PointWithChecked) goto case SegmentIndexerMode.PointWithChecked;
                                }
                            }
                        }
                    }

                    float l = 4f * this.Canvas.InverseScaleFactor;
                    float ls = l * l;

                    if (this.FootPoint.Contains)
                    {
                        for (int k = 0; k < this.Layers.Count; k++)
                        {
                            Layer item = this.Layers[k];
                            if (item.IsSelected)
                            {
                                for (int j = 0; j < item.Figures.Count; j++)
                                {
                                    Figure figure = item.Figures[j];

                                    this.FootPoint = new FootPointer(NodePointUnits.Normal, figure.Segments, figure.IsClosed, this.Position, ls);
                                    if (this.FootPoint.Contains)
                                    {
                                        this.Invalidate(InvalidateModes.CanvasControl);
                                        return;
                                    }
                                }
                            }
                        }

                        this.FootPoint = default;
                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    else
                    {
                        for (int k = 0; k < this.Layers.Count; k++)
                        {
                            Layer item = this.Layers[k];
                            if (item.IsSelected)
                            {
                                for (int j = 0; j < item.Figures.Count; j++)
                                {
                                    Figure figure = item.Figures[j];

                                    this.FootPoint = new FootPointer(NodePointUnits.Normal, figure.Segments, figure.IsClosed, this.Position, ls);
                                    if (this.FootPoint.Contains)
                                    {
                                        this.Invalidate(InvalidateModes.CanvasControl);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case SegmentIndexerMode.PointWithoutChecked:
                    {
                        Layer item = this.Layers[this.LayerIndex];
                        Figure figure = item.Figures[this.FigureIndex];
                        Segment segment = figure.Segments[this.Indexer.Index];

                        this.FootPoint = new FootPointer(segment.Map);

                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    break;
                case SegmentIndexerMode.PointWithChecked:
                    {
                        Layer item = this.Layers[this.LayerIndex];
                        Figure figure = item.Figures[this.FigureIndex];
                        Segment segment = figure.Segments[this.Indexer.Index];

                        this.FootPoint = new FootPointer(segment.Map);

                        this.Invalidate(InvalidateModes.CanvasControl);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region NodeTransform
        private void Apply1(IndicatorKind kind, float value)
        {
            switch (this.Composer.ComposerSizeType(kind))
            {
                case ComposerSizeType.Empty:
                    break;
                case ComposerSizeType.PointX:
                    {
                        float translateX = value - this.Indicator.X;

                        this.Composer.PointSetTranslationX(this.Indicator, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.PointY:
                    {
                        float translateY = value - this.Indicator.Y;

                        this.Composer.PointSetTranslationY(this.Indicator, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineX:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.LineSetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineY:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.LineSetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineWidth:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        this.Composer.LineSetWidth(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.RowLineRotation:
                    {
                        RowLineMode mode = this.ParameterPanel.RowMode;

                        this.Composer.LineSetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineX:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.LineSetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineY:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.LineSetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineHeight:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        this.Composer.LineSetHeight(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.ColumnLineRotation:
                    {
                        ColumnLineMode mode = this.ParameterPanel.ColumnMode;

                        this.Composer.LineSetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelX:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        float translateX = value - this.Indicator.X;

                        this.Composer.PanelSetTranslationX(this.Indicator, mode, translateX);

                        this.SetTranslationXSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelY:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        float translateY = value - this.Indicator.Y;

                        this.Composer.PanelSetTranslationY(this.Indicator, mode, translateY);

                        this.SetTranslationYSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelWidth:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetWidth(this.Indicator, mode, value, this.NodeKeepRatio);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelHeight:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetHeight(this.Indicator, mode, value, this.NodeKeepRatio);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelRotation:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetRotation(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                case ComposerSizeType.PanelSkew:
                    {
                        BoxMode mode = this.ParameterPanel.Mode;

                        this.Composer.PanelSetSkew(this.Indicator, mode, value);

                        this.SetTransformSelectedItems();
                    }
                    break;
                default:
                    break;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateComposer | InvalidateModes.UpdateTransformer
                | InvalidateModes.CanvasControl);
        }

        private void CacheSingle3()
        {
            this.RectChoose = new Bounds(this.StartingPosition);
            this.ActualRectChoose = new TransformedBounds(this.StartingPoint);
            this.HasRectChoose = true;

            this.Invalidate(InvalidateModes.CanvasControl);
        }

        private void Single3()
        {
            this.RectChoose = new Bounds(this.StartingPosition, this.Position);
            this.ActualRectChoose = this.RectChoose * this.Canvas.Matrix;

            this.Invalidate(InvalidateModes.CanvasControl);
        }

        private void DisposeSingle3()
        {
            this.RectChoose = new Bounds(this.StartingPosition, this.Position);
            this.ActualRectChoose = this.RectChoose * this.Canvas.Matrix;

            this.RectChooseItems();

            this.HasRectChoose = false;

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }
        #endregion

        #region Point
        private void CacheSingle4()
        {
            const float d = 12f;
            const float ds = d * d;

            this.PointMode = this.Composer.ActualPoint.ContainsNode(this.StartingPoint, ds);

            if (this.PointMode)
            {
                this.Composer.PointCacheTranslation();

                this.CacheTranslationSelectedItems();
            }
            else
            {
                this.CacheSingle3();
            }
        }

        private void Single4()
        {
            if (this.PointMode)
            {
                this.Composer.PointTranslate(this.Indicator, this.StartingPosition, this.Position);

                this.TranslateSelectedItems();

                this.Invalidate(InvalidateModes.None
                    | InvalidateModes.UpdateLayers
                    | InvalidateModes.UpdateComposer
                    | InvalidateModes.CanvasControl);
            }
            else
            {
                this.Single3();
            }
        }

        private void DisposeSingle4()
        {
            if (this.PointMode)
            {
                this.Invalidate(InvalidateModes.None
                    //| InvalidateModes.InitComposer
                    | InvalidateModes.CanvasControl);
            }
            else
            {
                this.DisposeSingle3();
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
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }

        private void DeselectAll()
        {
            foreach (Layer item in this.Layers)
            {
                item.IsSelected = false;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }

        private void SelectAll()
        {
            foreach (Layer item in this.Layers)
            {
                item.IsSelected = true;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
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
                | InvalidateModes.InitComposer | InvalidateModes.InitTransformer
                | InvalidateModes.CanvasControl);
        }

        private void Draw0(CanvasDrawingSession drawingSession)
        {
            if (this.HasRectChoose)
            {
                drawingSession.Transform = this.Canvas.Matrix;
                drawingSession.FillRectChoose(new Rectangle(this.RectChoose));
                drawingSession.Transform = Matrix3x2.Identity;

                drawingSession.DrawRectChoose(this.ActualRectChoose);
            }
            else
            {
                switch (this.Transformer.Count)
                {
                    case 0:
                        break;
                    default:
                        drawingSession.DrawBox(this.Transformer.ActualBox);
                        break;
                }
            }
        }

        private void Apply0(IndicatorKind kind, float value)
        {
            BoxMode mode = this.ParameterPanel.Mode;

            switch (this.Transformer.TransformerSizeType(kind))
            {
                case TransformerSizeType.None:
                    break;
                case TransformerSizeType.X:
                    float translateX = value - this.Indicator.X;

                    this.Transformer.SetTranslationX(this.Indicator, mode, translateX);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.Reset(this.Transformer.Destination);
                        }
                    }
                    break;
                case TransformerSizeType.Y:
                    float translateY = value - this.Indicator.Y;

                    this.Transformer.SetTranslationY(this.Indicator, mode, translateY);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.Reset(this.Transformer.Destination);
                        }
                    }
                    break;
                case TransformerSizeType.Width:
                    this.Transformer.SetWidth(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.Reset(this.Transformer.Destination);
                        }
                    }
                    break;
                case TransformerSizeType.Height:
                    this.Transformer.SetHeight(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.Reset(this.Transformer.Destination);
                        }
                    }
                    break;
                case TransformerSizeType.Rotation:
                    this.Transformer.SetRotation(this.Indicator, mode, value);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.Reset(this.Transformer.Destination);
                        }
                    }
                    break;
                case TransformerSizeType.Skew:
                    this.Transformer.SetSkew(this.Indicator, mode, value);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.Reset(this.Transformer.Destination);
                        }
                    }
                    break;
                case TransformerSizeType.MultiX:
                    float translateXs = value - this.Indicator.X;

                    this.Transformer.SetTranslationX(this.Indicator, mode, translateXs);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTranslationX(translateXs);
                        }
                    }
                    break;
                case TransformerSizeType.MultiY:
                    float translateYs = value - this.Indicator.Y;

                    this.Transformer.SetTranslationY(this.Indicator, mode, translateYs);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTranslationY(translateYs);
                        }
                    }
                    break;
                case TransformerSizeType.MultiWidth:
                    this.Transformer.SetWidth(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTransform(this.Transformer.TransformMatrix);
                        }
                    }
                    break;
                case TransformerSizeType.MultiHeight:
                    this.Transformer.SetHeight(this.Indicator, mode, value, this.KeepRatio);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTransform(this.Transformer.TransformMatrix);
                        }
                    }
                    break;
                case TransformerSizeType.MultiRotation:
                    this.Transformer.SetRotation(this.Indicator, mode, value);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTransform(this.Transformer.TransformMatrix);
                        }
                    }
                    break;
                case TransformerSizeType.MultiSkew:
                    this.Transformer.SetSkew(this.Indicator, mode, value);

                    foreach (Layer item in this.Layers)
                    {
                        if (item.IsSelected)
                        {
                            item.SetTransform(this.Transformer.TransformMatrix);
                        }
                    }
                    break;
                default:
                    break;
            }

            this.Invalidate(InvalidateModes.None
                | InvalidateModes.UpdateLayers
                | InvalidateModes.UpdateTransformer
                | InvalidateModes.CanvasControl);
        }
        #endregion
    }
}