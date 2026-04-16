using FanKit.Transformer.Mathematics;
using FanKit.Transformer.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class ScrollerPage : Page
    {
        Vector2 StartingPoint;
        Vector2 Point;

        ScrollerBounds Bounds;
        Linear LeftLinear;
        Linear RightLinear;

        Scroller Quad;
        Linear FloatLinear;

        int PageIndex;
        CanvasRenderTarget[] Bitmaps;

        readonly ScrollerAnimation Animation = new ScrollerAnimation();
        readonly CanvasOperator1 CanvasOperator;

        readonly Color BackColor1 = Color.FromArgb(255, 41, 41, 41);
        readonly Color BackColor2 = Windows.UI.Colors.White;

        readonly ScrollerColors Colors = new ScrollerColors(8, 38, 58, 8);
        /*
        readonly ScrollerColors Colors = new ScrollerColors(
            Color.FromArgb(255, 255, 243, 227),
            Color.FromArgb(255, 204, 195, 182),
            Color.FromArgb(255, 171, 163, 152),
            Color.FromArgb(255, 204, 195, 182));
         */

        bool IsFlipX;
        CanvasBitmap Bitmap1 => this.PageIndex % 2 == 0 ? this.Bitmaps[2] : this.Bitmaps[0];
        CanvasBitmap Bitmap2 => this.PageIndex % 2 == 0 ? this.Bitmaps[3] : this.Bitmaps[1];
        CanvasBitmap Bitmap3 => this.PageIndex % 2 == 0 ? this.Bitmaps[0] : this.Bitmaps[2];
        CanvasBitmap Bitmap4 => this.PageIndex % 2 == 0 ? this.Bitmaps[1] : this.Bitmaps[3];

        public ScrollerPage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl)
            {
                IsDisableFlipX = true
            };
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                this.Bounds = new ScrollerBounds(width / 6f, height / 6f, width / 3f * 2f, height / 3f * 2f);
                this.LeftLinear = this.Bounds.ToLeftLinear(100f);
                this.RightLinear = this.Bounds.ToRightLinear(100f);

                //this.Quad = this.Bounds.Scroll(this.StartingPoint.Y, this.Point);
                //this.FloatLinear = this.Quad.ToFloatLinear(100f);

                this.CanvasControl.Invalidate();
            };
            this.CanvasControl.CreateResources += (s, args) =>
            {
                this.IsFlipX = this.FlowDirection == Windows.UI.Xaml.FlowDirection.RightToLeft;
                this.CreateResourcesAsync(s);
            };
            this.CanvasControl.Update += (s, e) =>
            {
                if (this.Animation.Update(e.Timing.ElapsedTime))
                {
                    this.Quad = this.Bounds.Scroll(this.Animation.StartingY, this.Animation.Value);
                    this.FloatLinear = this.Quad.ToFloatLinear(100f);
                }
                else
                {
                    switch (this.Animation.Direction)
                    {
                        case ScrollerDirection.None:
                            this.Quad = default;
                            this.FloatLinear = default;

                            this.CanvasControl.Paused = true;
                            this.CanvasControl.Invalidate();
                            break;
                        case ScrollerDirection.PageUp:
                            this.PageIndex--;
                            goto case ScrollerDirection.None;
                        case ScrollerDirection.PageDown:
                            this.PageIndex++;
                            goto case ScrollerDirection.None;
                        default:
                            break;
                    }
                }
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                this.Draw(s, e.DrawingSession);
            };

            this.CanvasOperator.Single_Start += (startingX, startingY, p) =>
            {
                this.CacheSingle(startingX, startingY);
            };
            this.CanvasOperator.Single_Delta += (x, y, p) =>
            {
                this.Single(x, y);
            };
            this.CanvasOperator.Single_Complete += (x, y, p) =>
            {
                this.DisposeSingle(x, y);
            };
        }

        private void CreateResourcesAsync(ICanvasResourceCreator resourceCreator)
        {
            this.Bitmaps = new CanvasRenderTarget[]
            {
                new CanvasRenderTarget(resourceCreator, 720f, 1080f, 96f),
                new CanvasRenderTarget(resourceCreator, 720f, 1080f, 96f),
                new CanvasRenderTarget(resourceCreator, 720f, 1080f, 96f),
                new CanvasRenderTarget(resourceCreator, 720f, 1080f, 96f),
            };

            using (CanvasTextFormat textFormat = new CanvasTextFormat
            {
                HorizontalAlignment = CanvasHorizontalAlignment.Center,
                VerticalAlignment = CanvasVerticalAlignment.Center,
                FontWeight = Windows.UI.Text.FontWeights.Medium,
                FontSize = 240f,
            })
            {
                for (int i = 0; i < this.Bitmaps.Length; i++)
                {
                    CanvasRenderTarget item = this.Bitmaps[i];
                    using (CanvasDrawingSession drawingSession = item.CreateDrawingSession())
                    using (CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, $"{i}", textFormat, 720f, 1080f))
                    {
                        drawingSession.DrawRectangle(72f, 72f, 576f, 936f, Windows.UI.Colors.Black, 4f);
                        drawingSession.DrawTextLayout(textLayout, 0f, 0f, Windows.UI.Colors.Black);
                    }
                }
            }
        }

        private void Draw(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            drawingSession.FillRectangle(this.Bounds.Left - 20f, this.Bounds.Top - 20f, this.Bounds.Width + 40f, this.Bounds.Height + 40f, this.BackColor1);
            drawingSession.FillRectangle(this.Bounds.Left, this.Bounds.Top, this.Bounds.Width, this.Bounds.Height, this.BackColor2);

            switch (this.Quad.State)
            {
                case ScrollerState.LeftOutside:
                    // Left Page 3
                    if (this.Bitmap3 != null)
                    {
                        drawingSession.DrawImage(new Transform2DEffect
                        {
                            TransformMatrix = this.Bounds.GetLeftTransformMatrix((float)this.Bitmap3.Size.Width, (float)this.Bitmap3.Size.Height, this.IsFlipX),
                            Source = this.Bitmap3
                        });
                    }
                    break;
                default:
                    // Left Page 1
                    if (this.Bitmap1 != null)
                    {
                        drawingSession.DrawImage(new Transform2DEffect
                        {
                            TransformMatrix = this.Bounds.GetLeftTransformMatrix((float)this.Bitmap1.Size.Width, (float)this.Bitmap1.Size.Height, this.IsFlipX),
                            Source = this.Bitmap1
                        });
                    }
                    break;
            }

            switch (this.Quad.State)
            {
                case ScrollerState.LeftOutside:
                    // Right Page 4
                    if (this.Bitmap4 != null)
                    {
                        drawingSession.DrawImage(new Transform2DEffect
                        {
                            TransformMatrix = this.Bounds.GetRightTransformMatrix((float)this.Bitmap4.Size.Width, (float)this.Bitmap4.Size.Height, this.IsFlipX),
                            Source = this.Bitmap4
                        });
                    }
                    break;
                case ScrollerState.RightOutside:
                    // Right Page 2
                    if (this.Bitmap2 != null)
                    {
                        drawingSession.DrawImage(new Transform2DEffect
                        {
                            TransformMatrix = this.Bounds.GetRightTransformMatrix((float)this.Bitmap2.Size.Width, (float)this.Bitmap2.Size.Height, this.IsFlipX),
                            Source = this.Bitmap2
                        });
                    }
                    break;
                default:
                    if (this.Bitmap2 != null)
                    {
                        using (CanvasGeometry polygon = CanvasGeometry.CreatePolygon(resourceCreator, this.Quad.ToLeftPoints()))
                        using (drawingSession.CreateLayer(1.0f, polygon))
                        {
                            // Right Page 2
                            drawingSession.DrawImage(new Transform2DEffect
                            {
                                TransformMatrix = this.Bounds.GetRightTransformMatrix((float)this.Bitmap2.Size.Width, (float)this.Bitmap2.Size.Height, this.IsFlipX),
                                Source = this.Bitmap2
                            });
                        }
                    }

                    if (this.Bitmap4 != null)
                    {
                        using (CanvasGeometry polygon = CanvasGeometry.CreatePolygon(resourceCreator, this.Quad.ToRightPoints()))
                        using (drawingSession.CreateLayer(1.0f, polygon))
                        {
                            // Right Page 4
                            drawingSession.DrawImage(new Transform2DEffect
                            {
                                TransformMatrix = this.Bounds.GetRightTransformMatrix((float)this.Bitmap4.Size.Width, (float)this.Bitmap4.Size.Height, this.IsFlipX),
                                Source = this.Bitmap4
                            });
                        }
                    }
                    break;
            }

            using (CanvasLinearGradientBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.Colors.LeftGradientStops)
            {
                StartPoint = this.LeftLinear.L0,
                EndPoint = this.LeftLinear.L1,
            })
            {
                // Left Page
                drawingSession.FillRectangle(this.Bounds.Left, this.Bounds.Top, this.Bounds.WidthHalf, this.Bounds.Height, brush);
            }

            using (CanvasLinearGradientBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.Colors.RightGradientStops)
            {
                StartPoint = this.RightLinear.L0,
                EndPoint = this.RightLinear.L1,
            })
            {
                // Right Page
                drawingSession.FillRectangle(this.Bounds.CenterX, this.Bounds.Top, this.Bounds.WidthHalf, this.Bounds.Height, brush);
            }

            switch (this.Quad.State)
            {
                case ScrollerState.LeftOutside:
                case ScrollerState.RightOutside:
                    break;
                default:
                    using (CanvasGeometry polygon = CanvasGeometry.CreatePolygon(resourceCreator, this.Quad.ToFloatPoints()))
                    {
                        using (CanvasCommandList list = new CanvasCommandList(resourceCreator))
                        {
                            using (CanvasDrawingSession d = list.CreateDrawingSession())
                            {
                                d.FillGeometry(polygon, Windows.UI.Colors.Black);
                            }

                            // Shade Left Page 3
                            float v = this.Quad.ToOpacity(this.Bounds.Width);
                            drawingSession.DrawImage(new OpacityEffect
                            {
                                Opacity = 0.75f * v,
                                Source = new GaussianBlurEffect
                                {
                                    BlurAmount = 32f,
                                    Source = list,
                                }
                            });

                            drawingSession.FillGeometry(polygon, this.BackColor2);
                        }

                        // Float Right Page 3
                        if (this.Bitmap3 != null)
                        {
                            using (drawingSession.CreateLayer(1.0f, polygon))
                            {
                                drawingSession.DrawImage(new Transform2DEffect
                                {
                                    TransformMatrix = this.Bounds.GetFloatTransformMatrix(this.Quad, (float)this.Bitmap3.Size.Width, (float)this.Bitmap3.Size.Height, this.IsFlipX),
                                    Source = this.Bitmap3,
                                });
                            }
                        }

                        using (CanvasLinearGradientBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.Colors.LeftGradientStops)
                        {
                            StartPoint = this.FloatLinear.L0,
                            EndPoint = this.FloatLinear.L1,
                        })
                        {
                            // Float Left Page 3
                            drawingSession.FillGeometry(polygon, brush);
                        }
                    }
                    break;
            }
        }

        private void CacheSingle(double startingX, double startingY)
        {
            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);

            switch (this.Bounds.GetDirection(this.StartingPoint.X))
            {
                case ScrollerDirection.None:
                    this.Quad = this.Bounds.Scroll(this.StartingPoint.X);
                    this.FloatLinear = this.Quad.ToFloatLinear(100f);

                    this.CanvasControl.Invalidate();
                    break;
                case ScrollerDirection.PageUp:
                    this.PageIndex--;
                    goto case ScrollerDirection.None;
                case ScrollerDirection.PageDown:
                    this.PageIndex++;
                    goto case ScrollerDirection.None;
                default:
                    break;
            }
        }

        private void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            this.Quad = this.Bounds.Scroll(this.StartingPoint.Y, this.Point);
            this.FloatLinear = this.Quad.ToFloatLinear(100f);

            this.CanvasControl.Invalidate();
        }

        private void DisposeSingle(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            this.Quad = this.Bounds.Scroll(this.StartingPoint.Y, this.Point);
            this.FloatLinear = this.Quad.ToFloatLinear(100f);

            switch (this.Quad.State)
            {
                case ScrollerState.LeftOutside:
                case ScrollerState.RightOutside:
                    this.CanvasControl.Invalidate();
                    break;
                default:
                    this.Animation.Reset(this.Point, this.Bounds, this.StartingPoint.Y);
                    this.CanvasControl.Paused = false;
                    break;
            }
        }
    }

    public class ScrollerColors
    {
        public readonly Color Color1;
        public readonly Color Color2;
        public readonly Color Color3;
        public readonly Color Color4;
        public readonly CanvasGradientStop[] LeftGradientStops;
        public readonly CanvasGradientStop[] RightGradientStops;

        public ScrollerColors(byte gray1, byte gray2, byte gray3, byte gray4) : this(
            Color.FromArgb(gray1, 0, 0, 0),
            Color.FromArgb(gray2, 0, 0, 0),
            Color.FromArgb(gray3, 0, 0, 0),
            Color.FromArgb(gray4, 0, 0, 0))
        {
        }

        public ScrollerColors(Color color1, Color color2, Color color3, Color color4)
        {
            this.Color1 = color1;
            this.Color2 = color2;
            this.Color3 = color3;
            this.Color4 = color4;

            this.LeftGradientStops = new CanvasGradientStop[]
            {
                new CanvasGradientStop { Position = 0f, Color = this.Color1, },
                new CanvasGradientStop { Position = 0.5f, Color = this.Color2, },
                new CanvasGradientStop { Position = 0.75f, Color = this.Color3, },
                new CanvasGradientStop { Position = 1f, Color =   color4, },
            };
            this.RightGradientStops = new CanvasGradientStop[]
            {
                new CanvasGradientStop { Position = 0f, Color = this.Color3, },
                new CanvasGradientStop { Position = 0.25f, Color = this.Color2, },
                new CanvasGradientStop { Position = 0.5f, Color = this.Color1, },
            };
        }
    }
}