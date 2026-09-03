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
        struct Stellite
        {
            public Vector3 Vector;
            public Vector3 RotateVector;
            public bool PointIsFarSide;
            public Vector2 Vertex;
        }

        const float TouchPadWidth = 409.6f;
        const float TouchPadHeight = 204.8f;

        const float DeemoScaleX = TouchPadWidth / EarthTextureSize.DemoBitmapWidth;
        const float DeemoScaleY = TouchPadHeight / EarthTextureSize.DemoBitmapHeight;

        static readonly EarthUV UV = EarthUV.U18V11;

        bool ShowGrid;

        Vector2 StartingPoint;
        Vector2 Point = Vector2.Zero;

        Vector3 StartingRadians;
        Vector3 Radians = Vector3.Zero;

        EarthLayout EarthLayout;
        EarthTextureSize EarthTextureSize;
        EarthRotation EarthRotation = new EarthRotation(Vector3.Zero);
        Stellite? Mouse = null;

        readonly Earth Earth = new Earth(UV);
        readonly CanvasBitmap[,] Textures = new CanvasBitmap[UV.V, UV.U];
        readonly List<Stellite> Stellites = new List<Stellite>();

        readonly CanvasOperator1 CanvasOperator;

        readonly Color VertexColor = Color.FromArgb(63, 81, 177, 255);
        readonly Color DemoSeaColor = Color.FromArgb(255, 13, 35, 64);
        readonly Color DemoLandColor = Color.FromArgb(255, 90, 98, 46);
        readonly Color AtmosphereColor3 = Color.FromArgb(255, 6, 137, 249);
        readonly Color AtmosphereColor2 = Color.FromArgb(255, 42, 226, 249);
        readonly Color AtmosphereColor = Color.FromArgb(255, 2, 5, 20);
        readonly SolidColorBrush DemoLandBrush = new SolidColorBrush(Color.FromArgb(255, 89, 103, 59));
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

            foreach (var item in EarthTextureSize.DemoBitmapPolygons)
            {
                this.Touchpad.Children.Add(this.CreatePolygon(item));
            }

            this.CanvasControl.CreateResources += (s, args) =>
            {
                this.CreateResources(s, args);
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                if (this.Mouse.HasValue)
                {
                    e.DrawingSession.FillCircle(this.Mouse.Value.Vertex, 4f, Colors.OrangeRed);
                    e.DrawingSession.DrawCircle(this.Mouse.Value.Vertex, 4f, Colors.White);
                }
                foreach (Stellite item in this.Stellites)
                {
                    e.DrawingSession.FillCircle(item.Vertex, 4f, Colors.DodgerBlue);
                    e.DrawingSession.DrawCircle(item.Vertex, 4f, Colors.White);
                }

                if (this.ShowGrid)
                {
                    e.DrawingSession.DrawCircle(this.EarthLayout.Center, this.EarthLayout.Radius, Colors.DeepSkyBlue);
                }
                else
                {
                    e.DrawingSession.FillCircle(this.EarthLayout.Center, this.EarthLayout.Radius + 3f, this.AtmosphereColor3);
                    e.DrawingSession.FillCircle(this.EarthLayout.Center, this.EarthLayout.Radius + 1f, this.AtmosphereColor2);
                    e.DrawingSession.FillCircle(this.EarthLayout.Center, this.EarthLayout.Radius, this.AtmosphereColor);

                    foreach (var item in this.Earth.DrawTextures(UV))
                    {
                        int vi = item.V;
                        int ui = item.U;

                        e.DrawingSession.DrawImage(new Transform3DEffect
                        {
                            TransformMatrix = this.Earth.TextureTransformMatrixes[vi, ui],
                            Source = this.Textures[vi, ui]
                        });
                    }

                    if (!this.Earth.NorthPoleIsFarSide)
                    {
                        using (CanvasGeometry geometry = CanvasGeometry.CreatePolygon(s, this.Earth.NorthPolePolygon))
                        {
                            e.DrawingSession.FillGeometry(geometry, 0f, 0f, this.DemoSeaColor);
                        }
                    }

                    if (!this.Earth.SouthPoleIsFarSide)
                    {
                        using (CanvasGeometry geometry = CanvasGeometry.CreatePolygon(s, this.Earth.SouthPolePolygon))
                        {
                            e.DrawingSession.FillGeometry(geometry, 0f, 0f, this.DemoLandColor);
                        }
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
                }

                foreach (var item in this.Earth.DrawLines(UV))
                {
                    Vector2 point0 = item.Point0;
                    Vector2 point1 = item.Point1;

                    e.DrawingSession.DrawLine(point0, point1, this.ShowGrid ? Colors.DeepSkyBlue : this.VertexColor);
                }

                foreach (var item in this.Earth.DrawVertexes(UV))
                {
                    e.DrawingSession.FillCircle(item, 2f, this.ShowGrid ? Colors.DeepSkyBlue : this.VertexColor);
                }

                if (this.Mouse.HasValue && !this.Mouse.Value.PointIsFarSide)
                {
                    e.DrawingSession.FillCircle(this.Mouse.Value.Vertex, 4f, Colors.OrangeRed);
                    e.DrawingSession.DrawCircle(this.Mouse.Value.Vertex, 4f, Colors.White);
                }
                foreach (Stellite item in this.Stellites)
                {
                    if (!item.PointIsFarSide)
                    {
                        e.DrawingSession.FillCircle(item.Vertex, 4f, Colors.DodgerBlue);
                        e.DrawingSession.DrawCircle(item.Vertex, 4f, Colors.White);
                    }
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

                this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout);
                for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
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

                this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout, this.EarthRotation);
                for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
                this.CanvasControl.Invalidate();

                this.XTextBlock.Text = $"{(int)(180f * this.Radians.X / Mathematics.Math.PI)}°";
                this.ZTextBlock.Text = $"{(int)(180f * this.Radians.Z / Mathematics.Math.PI)}°";
                this.YTextBlock.Text = $"{(int)(180f * this.Radians.Y / Mathematics.Math.PI)}°";
            };
            this.CanvasOperator.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                if (System.Math.Abs(this.StartingPoint.X - this.Point.X) < 4d)
                {
                    if (System.Math.Abs(this.StartingPoint.Y - this.Point.Y) < 4d)
                    {
                        Vector2? amount = this.Earth.GetAmount(UV, this.EarthTextureSize, this.EarthLayout, this.Point, EarthTextureSize.DemoBitmapWidth, EarthTextureSize.DemoBitmapHeight);
                        if (amount.HasValue)
                        {
                            var uAmount = amount.Value.X;
                            var vAmount = amount.Value.Y;

                            //this.Mouse = this.GetStellite(uAmount, vAmount);
                            this.Stellites.Add(this.GetStellite(uAmount, vAmount));
                            this.CanvasControl.Invalidate();
                        }
                    }
                }
            };

            this.CanvasOperator.Wheel_Changed += (x, y, d) =>
            {
                this.EarthLayout.Radius = d > 0 ? this.EarthLayout.Radius * 1.04f : this.EarthLayout.Radius / 1.04f;

                this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout);
                for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
                this.CanvasControl.Invalidate();
            };

            this.ResetButton.Click += delegate { this.ResetRotation(); };
            this.ShowGridButton.Toggled += delegate
            {
                this.ShowGrid = this.ShowGridButton.IsOn;

                this.CanvasControl.Invalidate();
            };

            this.Touchpad.PointerExited += delegate
            {
                this.Mouse = null;
            };
            this.Touchpad.PointerMoved += (s, e) =>
            {
                var pp = e.GetCurrentPoint(this.Touchpad);
                float uAmount = (float)(pp.Position.X / TouchPadWidth);
                float vAmount = (float)(pp.Position.Y / TouchPadHeight);

                this.Mouse = this.GetStellite(uAmount, vAmount);
                this.CanvasControl.Invalidate();
            };
            this.Touchpad.PointerPressed += (s, e) =>
            {
                var pp = e.GetCurrentPoint(this.Touchpad);
                float uAmount = (float)(pp.Position.X / TouchPadWidth);
                float vAmount = (float)(pp.Position.Y / TouchPadHeight);

                this.Stellites.Add(this.GetStellite(uAmount, vAmount));
                this.CanvasControl.Invalidate();
            };
        }

        public Vector3 GetUnitVector(float uAmount, float vAmount) => Earth.GetUnitVector(uAmount, vAmount);
        public Vector3 RotateUnitVector(Vector3 unitVector) => this.EarthRotation.RotateUnitVector(unitVector);

        public Vector2 GetPoint(Vector3 unitVector) => this.EarthLayout.GetPoint(unitVector);
        public Vector2 GetPointEx(Vector3 unitVector) => this.EarthLayout.GetPoint(unitVector, this.EarthLayout.Radius * 1.1f);

        private Stellite GetStellite(float uAmount, float vAmount) => GetStellite(GetUnitVector(uAmount, vAmount));
        private Stellite GetStellite(Stellite stellite) => GetStellite(stellite.Vector);
        private Stellite GetStellite(Vector3 v)
        {
            Vector3 t = RotateUnitVector(v);
            Vector2 p = GetPoint(t);

            return new Stellite
            {
                Vector = v,
                RotateVector = t,
                PointIsFarSide = t.Z < 0f,
                Vertex = p
            };
        }

        //private void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args)
        //{
        //    args.TrackAsyncAction(CreateResourcesAsync(resourceCreator).AsAsyncAction());
        //}
        //private async Task CreateResourcesAsync(ICanvasResourceCreator resourceCreator)
        //{
        //    using (CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, "Images/ad189db39db704e.jpg"))
        //    {
        //        this.CreateTextures(resourceCreator, bitmap);
        //        this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout, this.EarthRotation);
        //        for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
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
                this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout, this.EarthRotation);
                for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
            }
        }

        private void CreateTextures(ICanvasResourceCreator resourceCreator, CanvasBitmap bitmap)
        {
            float bitmapWidth = (float)bitmap.Size.Width;
            float bitmapHeight = (float)bitmap.Size.Height;
            this.EarthTextureSize = new EarthTextureSize(UV, bitmapWidth, bitmapHeight);

            foreach (var item in this.EarthTextureSize.CreateTextures(UV))
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

        private Windows.UI.Xaml.Shapes.Polygon CreatePolygon(Vector2[] demoBitmapPolygon)
        {
            var polygon = new Windows.UI.Xaml.Shapes.Polygon
            {
                IsHitTestVisible = false,
                Fill = this.DemoLandBrush,
            };

            foreach (var p in demoBitmapPolygon)
            {
                polygon.Points.Add(new Point
                {
                    X = p.X * DeemoScaleX,
                    Y = p.Y * DeemoScaleY,
                });
            }

            return polygon;
        }

        private void ResetRotation()
        {
            this.Radians = Vector3.Zero;
            this.EarthRotation = new EarthRotation(Vector3.Zero);

            this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout, this.EarthRotation);
            for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
            this.CanvasControl.Invalidate();

            this.XTextBlock.Text = "0";
            this.ZTextBlock.Text = "0";
            this.YTextBlock.Text = "0";
        }
        private void RotateXTo(float value) // -180~180
        {
            this.Radians.X = Mathematics.Math.PI * value / 360f;
            this.EarthRotation = new EarthRotation(this.Radians);

            this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout, this.EarthRotation);
            for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
            this.CanvasControl.Invalidate();

            this.XTextBlock.Text = $"{(int)(180f * this.Radians.X / Mathematics.Math.PI)}°";
        }
        private void RotateZTo(float value) // -180~180
        {
            this.Radians.Z = Mathematics.Math.PI * value / 360f;
            this.EarthRotation = new EarthRotation(this.Radians);

            this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout, this.EarthRotation);
            for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
            this.CanvasControl.Invalidate();

            this.ZTextBlock.Text = $"{(int)(180f * this.Radians.Z / Mathematics.Math.PI)}°";
        }
        private void RotateYTo(float value) // -360~360
        {
            this.Radians.Y = Mathematics.Math.PI * value / 360f;
            this.EarthRotation = new EarthRotation(this.Radians);

            this.Earth.Update(UV, this.EarthTextureSize, this.EarthLayout, this.EarthRotation);
            for (int i = 0; i < this.Stellites.Count; i++) this.Stellites[i] = this.GetStellite(this.Stellites[i]);
            this.CanvasControl.Invalidate();

            this.YTextBlock.Text = $"{(int)(180f * this.Radians.Y / Mathematics.Math.PI)}°";
        }
    }
}