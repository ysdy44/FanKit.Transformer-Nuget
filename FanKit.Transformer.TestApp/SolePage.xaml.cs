using FanKit.Transformer.Cache;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Demos;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Sample;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    /// <summary>
    /// <see cref="CanvasOperator1"/>
    /// </summary>
    public abstract partial class SolePage : Page
    {
        public Vector2 StartingPoint;
        public Vector2 Point;

        readonly Matrix3x2 ThumbMatrix = Matrix3x2.CreateScale(0.2f);
        readonly CanvasOperator1 CanvasOperator;

        public bool IsInContact => this.CanvasOperator.IsInContact;
        public UIElementCollection Children => this.Grid.Children;
        public UIElement Child
        {
            get => this.Border.Child;
            set => this.Border.Child = value;
        }

        public SolePage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.ThumbCanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
                this.ThumbCanvasControl = null;
            };

            this.CanvasControl.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                this.Draw(s, e.DrawingSession);
            };

            this.CanvasOperator.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

                this.CacheSingle();
            };
            this.CanvasOperator.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                this.Single();
            };
            this.CanvasOperator.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                this.DisposeSingle();
            };

            this.ThumbCanvasControl.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.ThumbCanvasControl.Draw += (s, e) =>
            {
                e.DrawingSession.Transform = this.ThumbMatrix;

                this.DrawThumb(s, e.DrawingSession);
            };

            this.BottomBar.HideToggled += delegate
            {
                this.Grid.Visibility =
                this.ThumbBorder.Visibility =
                this.ParameterPanel.Visibility =
                this.Border.Visibility =
                this.BottomBar.IsHidden ? Visibility.Collapsed : Visibility.Visible;
            };
        }

        public abstract void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args);
        public abstract void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);
        public abstract void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession);

        public abstract void CacheSingle();
        public abstract void Single();
        public abstract void DisposeSingle();
        public abstract void Over();

        public abstract void UpdateCanvasControl1();
        public abstract void UpdateCanvasControl2();

        public void Invalidate()
        {
            this.CanvasControl.Invalidate();
            this.ThumbCanvasControl.Invalidate(); // Invalidate
        }
    }
}