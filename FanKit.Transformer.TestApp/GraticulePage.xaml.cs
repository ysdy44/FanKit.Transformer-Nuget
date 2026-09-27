using FanKit.Transformer.UI;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class GraticulePage : Page
    {
        readonly static GraticuleUV UV = GraticuleUV.U36V20;

        Vector2 StartingPoint;
        Vector2 Point = Vector2.Zero;

        Vector3 StartingRadians;
        Vector3 Radians = Vector3.Zero;

        Vector2 EarthCenter;
        float EarthRadius;
        Camera Camera = new Camera(Vector3.Zero, 1f, Vector2.Zero);

        readonly Graticule Earth = new Graticule(UV);

        readonly CanvasOperator1 CanvasOperator;

        public GraticulePage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
            };

            this.CanvasControl.CreateResources += (s, args) =>
            {
                this.Earth.Update(this.Camera);
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                e.DrawingSession.DrawCircle(this.EarthCenter, this.EarthRadius, Colors.DeepSkyBlue);

                foreach (GraticuleLine item in this.Earth.DrawLines())
                {
                    Vector2 point0 = item.Point0;
                    Vector2 point1 = item.Point1;

                    e.DrawingSession.DrawLine(point0, point1, Colors.DeepSkyBlue);
                }

                foreach (Vector2 item in this.Earth.DrawVertexes())
                {
                    e.DrawingSession.FillCircle(item, 2f, Colors.DeepSkyBlue);
                }
            };
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                float viewportWidth = (float)e.NewSize.Width;
                float viewportHeight = (float)e.NewSize.Height;

                this.EarthRadius = 0.45f * System.Math.Min(viewportWidth, viewportHeight);
                this.EarthCenter = new Vector2
                {
                    X = 0.5f * viewportWidth,
                    Y = 0.5f * viewportHeight,
                };
                this.Camera = new Camera(this.Radians, this.EarthRadius, this.EarthCenter);

                this.Earth.Update(this.Camera);
                this.CanvasControl.Invalidate();
            };

            this.CanvasOperator.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);
                this.StartingRadians = this.Radians;
            };
            this.CanvasOperator.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                float horizontalOffset = this.Point.X - this.StartingPoint.X;
                float verticalOffset = this.Point.Y - this.StartingPoint.Y;
                this.Radians = Camera.ScrollTo(this.StartingRadians, this.EarthRadius, horizontalOffset, verticalOffset);

                this.Camera = new Camera(this.Radians, this.EarthRadius, this.EarthCenter);

                this.Earth.Update(this.Camera);
                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);
            };

            this.CanvasOperator.Wheel_Changed += (x, y, d) =>
            {
                this.EarthRadius = d > 0 ? this.EarthRadius * 1.04f : this.EarthRadius / 1.04f;

                this.Earth.Update(this.Camera);
                this.CanvasControl.Invalidate();
            };
        }
    }
}