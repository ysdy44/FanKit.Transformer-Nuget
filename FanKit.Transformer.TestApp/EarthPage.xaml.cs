using FanKit.Transformer.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class EarthPage : Page
    {
        Vector2 StartingPoint;
        Vector2 Point;

        Vector3 StartingRadians;
        Vector3 Radians;

        EarthLayout EarthLayout;
        EarthTextureSize EarthTextureSize;
        EarthRotation EarthRotation = new EarthRotation(Vector3.Zero);

        readonly Earth Earth = new Earth();
        readonly CanvasBitmap[,] Textures = new CanvasBitmap[EarthTextureSize.V, EarthTextureSize.U];

        readonly CanvasOperator1 CanvasOperator;

        readonly Color VertexColor = Color.FromArgb(63, 81, 177, 255);
        readonly Color DemoSeaColor = Color.FromArgb(255, 18, 54, 106);
        readonly Color DemoLandColor = Color.FromArgb(255, 90, 98, 46);
        readonly Color AtmosphereColor3 = Color.FromArgb(255, 6, 137, 249);
        readonly Color AtmosphereColor2 = Color.FromArgb(255, 42, 226, 249);
        readonly Color AtmosphereColor = Color.FromArgb(255, 2, 5, 20);
        readonly CanvasGradientStop[] AtmosphereGradientStops = new CanvasGradientStop[]
        {
            new CanvasGradientStop
            {
                Position = 0.0f,
                Color = Color.FromArgb(15, 81, 177, 255),
            },
            new CanvasGradientStop
            {
                Position = 0.5f,
                Color = Color.FromArgb(31, 81, 177, 255),
            },
            new CanvasGradientStop
            {
                Position = 0.8f,
                Color = Color.FromArgb(63, 81, 177, 255),
            },
            new CanvasGradientStop
            {
                Position = 0.94f,
                Color = Color.FromArgb(127, 81, 177, 255),
            },
            new CanvasGradientStop
            {
                Position = 1f,
                Color = Color.FromArgb(127, 255, 255, 255),
            },
        };

        public EarthPage()
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
                this.CreateResources(s, args);
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                e.DrawingSession.FillCircle(this.EarthLayout.Center, this.EarthLayout.Radius + 3f, this.AtmosphereColor3);
                e.DrawingSession.FillCircle(this.EarthLayout.Center, this.EarthLayout.Radius + 1f, this.AtmosphereColor2);
                e.DrawingSession.FillCircle(this.EarthLayout.Center, this.EarthLayout.Radius, this.AtmosphereColor);

                foreach (var item in this.Earth.DrawTextures())
                {
                    int vi = item.V;
                    int ui = item.U;

                    e.DrawingSession.DrawImage(new Transform3DEffect
                    {
                        TransformMatrix = this.Earth.TransformMatrixes[vi, ui],
                        Source = this.Textures[vi, ui]
                    });
                }

                using (CanvasRadialGradientBrush brush = new CanvasRadialGradientBrush(s, this.AtmosphereGradientStops)
                {
                    Center = this.EarthLayout.Center,
                    RadiusX = this.EarthLayout.Radius,
                    RadiusY = this.EarthLayout.Radius,
                })
                {
                    e.DrawingSession.FillCircle(this.EarthLayout.Center, this.EarthLayout.Radius, brush);
                }

                foreach (var item in this.Earth.DrawLines())
                {
                    Vector2 point0 = item.Point0;
                    Vector2 point1 = item.Point1;

                    e.DrawingSession.DrawLine(point0, point1, this.VertexColor);
                }

                foreach (var item in this.Earth.DrawPoints())
                {
                    e.DrawingSession.FillCircle(item, 2f, this.VertexColor);
                }
            };
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                float viewportWidth = (float)e.NewSize.Width;
                float viewportHeight = (float)e.NewSize.Height;

                this.EarthLayout = new EarthLayout
                {
                    Radius = 0.45f * System.Math.Min(viewportWidth, viewportHeight),
                    Center = new Vector2
                    {
                        X = 0.5f * viewportWidth,
                        Y = 0.5f * viewportHeight,
                    }
                };

                this.Earth.Update(this.EarthLayout, this.EarthTextureSize, this.EarthRotation);
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
                this.Radians = this.EarthLayout.ScrollTo(this.StartingRadians, horizontalOffset, verticalOffset);

                this.EarthRotation = new EarthRotation(this.Radians);

                this.Earth.Update(this.EarthLayout, this.EarthTextureSize, this.EarthRotation);
                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Single_Complete += (x, y, p) => { };

            this.CanvasOperator.Wheel_Changed += (x, y, d) =>
            {
                this.EarthLayout.Radius = d > 0 ? this.EarthLayout.Radius * 1.04f : this.EarthLayout.Radius / 1.04f;

                this.Earth.Update(this.EarthLayout, this.EarthTextureSize, this.EarthRotation);
                this.CanvasControl.Invalidate();
            };

            this.ResetButton.Click += delegate
            {
                this.Radians = Vector3.Zero;
                this.EarthRotation = new EarthRotation(Vector3.Zero);

                this.Earth.Update(this.EarthLayout, this.EarthTextureSize, this.EarthRotation);
                this.CanvasControl.Invalidate();
            };
        }

        public Vector3 GetVector(float uAmount, float vAmount) => Earth.GetVector(uAmount, vAmount);
        public Vector3 RotateVector(Vector3 vector) => this.EarthRotation.RotateVector(vector);

        public Vector2 GetVertex(Vector3 vector) => this.EarthLayout.GetVertex(vector);
        public Vector2 GetVertexEx(Vector3 vector) => this.EarthLayout.GetVertex(vector, this.EarthLayout.Radius * 1.1f);

        //private void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        //{
        //    args.TrackAsyncAction(CreateResourcesAsync(resourceCreator).AsAsyncAction());
        //}
        //private async Task CreateResourcesAsync(ICanvasResourceCreator resourceCreator)
        //{
        //    using (CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, "Images/ad189db39db704e.jpg"))
        //    {
        //        this.CreateTextures(resourceCreator, bitmap);
        //        this.Earth.Update(this.EarthLayout, this.EarthTextureSize, this.EarthRotation);
        //    }
        //}
        private void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        {
            using (var renderTarget = new CanvasRenderTarget(resourceCreator, EarthTextureSize.DemoBitmapWidth, EarthTextureSize.DemoBitmapHeight, 96f))
            {
                using (var drawingSession = renderTarget.CreateDrawingSession())
                {
                    drawingSession.Clear(this.DemoSeaColor);

                    foreach (var item in EarthTextureSize.DemoBitmapPolygons)
                    {
                        using (var geometry = CanvasGeometry.CreatePolygon(resourceCreator, item))
                        {
                            drawingSession.FillGeometry(geometry, 0f, 0f, this.DemoLandColor);
                        }
                    }
                }

                this.CreateTextures(resourceCreator, renderTarget);
                this.Earth.Update(this.EarthLayout, this.EarthTextureSize, this.EarthRotation);
            }
        }

        private void CreateTextures(ICanvasResourceCreator resourceCreator, CanvasBitmap bitmap)
        {
            float bitmapWidth = (float)bitmap.Size.Width;
            float bitmapHeight = (float)bitmap.Size.Height;
            this.EarthTextureSize = new EarthTextureSize(bitmapWidth, bitmapHeight);

            foreach (var item in this.EarthTextureSize.CreateTextures())
            {
                int textureWidth = item.TextureWidth;
                int textureHeight = item.TextureHeight;
                CanvasRenderTarget texture = new CanvasRenderTarget(resourceCreator, textureWidth, textureHeight, 96f);

                float x = item.ImageX;
                float y = item.ImageY;
                using (CanvasDrawingSession drawingSession = texture.CreateDrawingSession())
                {
                    drawingSession.Clear(Colors.Black);
                    drawingSession.DrawImage(bitmap, x, y);
                }

                int vi = item.Index.V;
                int ui = item.Index.U;
                this.Textures[vi, ui] = texture;
            }
        }
    }
}