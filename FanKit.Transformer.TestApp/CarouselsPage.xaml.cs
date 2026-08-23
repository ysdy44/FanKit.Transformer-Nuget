using FanKit.Transformer.Mathematics;
using FanKit.Transformer.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public class CarouselItems
    {
        public CanvasBitmap Bitmap;
        public SizeMatrix SourceNormalize;
        public CarouselItem Carousel;
    }

    public sealed partial class CarouselsPage : Page
    {
        float RotationAngle = 45f;
        float ItemMargin = 60f;
        float ItemSpacing = 110f;
        bool ShowGrid;

        // Singe
        Vector2 StartingPoint;
        Vector2 Point;

        float StartingX;
        float X;

        Vector2 Center;

        Carousel Carousel = new Carousel(256f, 256f, 45f);

        // Canvas
        readonly CanvasOperator1 CanvasOperator;
        readonly CarouselAnimation Animation = new CarouselAnimation();

        readonly List<CarouselItems> Items = new List<CarouselItems>();

        public CarouselsPage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl)
            {
                IsDisableFlipX = true
            };
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
            };

            this.CanvasControl.CreateResources += (s, args) =>
            {
                args.TrackAsyncAction(CreateResourcesAsync(s).AsAsyncAction());
            };
            this.CanvasControl.Update += (s, e) =>
            {
                if (this.Animation.Update(e.Timing.ElapsedTime))
                {
                    this.X = this.Animation.Value;

                    this.Update();
                }
                else
                    this.CanvasControl.Paused = true;
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                this.Draw(e.DrawingSession);
            };
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                this.Center = new Vector2(width / 2f, height / 2f);

                this.Update();
            };
            this.CanvasControl.KeyDown += (s, e) =>
            {
                if (this.Key(e.Key))
                    e.Handled = true;
            };

            this.CanvasOperator.Single_Start += (startingX, startingY, p) => this.CacheSingle(startingX, startingY);
            this.CanvasOperator.Single_Delta += (x, y, p) => this.Single(x, y);
            this.CanvasOperator.Single_Complete += (x, y, p) => this.DisposeSingle(x, y);

            this.CanvasOperator.Wheel_Changed += (x, y, d) => this.Wheel(x, y, d);

            // Rotation Angle
            this.RotationAngleTextBlock.Text = $"{this.RotationAngle}°";
            this.RotationAngleSlider.Minimum = 10;
            this.RotationAngleSlider.Maximum = 80;
            this.RotationAngleSlider.Value = this.RotationAngle;
            this.RotationAngleSlider.ValueChanged += (s, e) =>
            {
                this.RotationAngle = (float)e.NewValue;
                this.RotationAngleTextBlock.Text = $"{this.RotationAngle}°";

                this.Carousel = new Carousel(256f, 256f, this.RotationAngle);

                this.Update();

                this.CanvasControl.Invalidate();
            };

            // Margin
            this.MarginTextBlock.Text = $"{this.ItemMargin}";
            this.MarginSlider.Minimum = 20;
            this.MarginSlider.Maximum = 200;
            this.MarginSlider.Value = this.ItemMargin;
            this.MarginSlider.ValueChanged += (s, e) =>
            {
                this.ItemMargin = (float)e.NewValue;
                this.MarginTextBlock.Text = $"{this.ItemMargin}";

                this.Update();

                this.CanvasControl.Invalidate();
            };

            // Spacing
            this.SpacingTextBlock.Text = $"{this.ItemSpacing}";
            this.SpacingSlider.Minimum = 20;
            this.SpacingSlider.Maximum = 200;
            this.SpacingSlider.Value = this.ItemSpacing;
            this.SpacingSlider.ValueChanged += (s, e) =>
            {
                this.ItemSpacing = (float)e.NewValue;
                this.SpacingTextBlock.Text = $"{this.ItemSpacing}";

                this.Update();

                this.CanvasControl.Invalidate();
            };

            this.ShowGridButton.Toggled += delegate
            {
                this.ShowGrid = this.ShowGridButton.IsOn;

                this.CanvasControl.Invalidate();
            };
        }

        private async Task CreateResourcesAsync(ICanvasResourceCreator resourceCreator)
        {
            CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, "Images/avatar.jpg");

            float width = (float)bitmap.Size.Width;
            float height = (float)bitmap.Size.Height;
            SizeMatrix sourceNormalize = new SizeMatrix(width, height);

            for (int i = 0; i < 10; i++)
            {
                this.Items.Add(new CarouselItems
                {
                    Bitmap = bitmap,
                    SourceNormalize = sourceNormalize,
                    Carousel = this.Carousel.ToItem(sourceNormalize, this.Center, i, this.X, this.ItemMargin, this.ItemSpacing),
                });
            }

            this.CanvasControl.Invalidate();
        }

        private void Draw(CanvasDrawingSession drawingSession)
        {
            if (!this.ShowGrid)
            {
                for (int i = this.Items.Count - 1; i >= 0; i--)
                {
                    CarouselItems item = this.Items[i];
                    if (item.Bitmap == null)
                        continue;

                    switch (item.Carousel.State)
                    {
                        case CarouselState.DockRight:
                            if (item.Carousel.ActualX > this.Center.X * 2f)
                                break;

                            drawingSession.DrawImage(new Transform3DEffect
                            {
                                TransformMatrix = item.Carousel.TextureTransformMatrix,
                                Source = item.Bitmap,
                            });
                            break;
                        default:
                            break;
                    }
                }

                for (int i = 0; i < this.Items.Count; i++)
                {
                    CarouselItems item = this.Items[i];
                    if (item.Bitmap == null)
                        continue;

                    switch (item.Carousel.State)
                    {
                        case CarouselState.Float:
                        case CarouselState.DockLeft:
                            if (item.Carousel.ActualX < 0f)
                                break;

                            drawingSession.DrawImage(new Transform3DEffect
                            {
                                TransformMatrix = item.Carousel.TextureTransformMatrix,
                                Source = item.Bitmap,
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                drawingSession.DrawLine(this.Center.X - this.ItemMargin - this.ItemSpacing / 2f, 0f, this.Center.X - this.ItemMargin - this.ItemSpacing / 2f, this.Center.Y + this.Center.Y, Windows.UI.Colors.Red);
                drawingSession.DrawLine(this.Center.X + this.ItemMargin + this.ItemSpacing / 2f, 0f, this.Center.X + this.ItemMargin + this.ItemSpacing / 2f, this.Center.Y + this.Center.Y, Windows.UI.Colors.Red);

                foreach (CarouselItems item in this.Items)
                {
                    drawingSession.DrawBounds(item.Carousel.TextureOutline);
                }

                foreach (CarouselItems item in this.Items)
                {
                    float x = item.Carousel.ActualX;
                    switch (item.Carousel.State)
                    {
                        case CarouselState.Float:
                            drawingSession.DrawLine(x, 0f, x, this.Center.Y + this.Center.Y, Windows.UI.Colors.Red);
                            break;
                        case CarouselState.DockLeft:
                            drawingSession.DrawLine(x, 0f, x, this.Center.Y + this.Center.Y, Windows.UI.Colors.Gray);
                            break;
                        case CarouselState.DockRight:
                            drawingSession.DrawLine(x, 0f, x, this.Center.Y + this.Center.Y, Windows.UI.Colors.Gray);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void CacheSingle(double startingX, double startingY)
        {
            this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);
            this.StartingX = this.X;
        }

        private void Single(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);
            this.X = this.StartingX + this.Point.X - this.StartingPoint.X;

            this.Update();

            this.CanvasControl.Invalidate();
        }

        private void DisposeSingle(double x, double y)
        {
            this.Point = new Vector2((float)x, (float)y);

            if (this.CanvasControl.Paused)
            {
                if (System.Math.Abs(this.StartingPoint.X - this.Point.X) < 4f)
                    this.Animate(this.ContainsIndex(this.Point));
                else
                    this.Animate(this.NearestIndex(this.Center.X));
            }
        }

        private void Wheel(double x, double y, double d)
        {
            if (this.CanvasControl.Paused)
            {
                if (d > 0)
                    this.Animate(this.NearestIndex(this.Center.X) - 1);
                else
                    this.Animate(this.NearestIndex(this.Center.X) + 1);
            }
        }

        private bool Key(VirtualKey key)
        {
            if (this.CanvasControl.Paused)
            {
                switch (key)
                {
                    case VirtualKey.Left:
                    case VirtualKey.GamepadDPadLeft:
                    case VirtualKey.GamepadLeftThumbstickLeft:

                    case VirtualKey.Up:
                    case VirtualKey.GamepadDPadUp:
                    case VirtualKey.GamepadLeftThumbstickUp:
                        this.Animate(this.NearestIndex(this.Center.X) - 1);
                        return true;

                    case VirtualKey.Right:
                    case VirtualKey.GamepadDPadRight:
                    case VirtualKey.GamepadLeftThumbstickRight:

                    case VirtualKey.Down:
                    case VirtualKey.GamepadDPadDown:
                    case VirtualKey.GamepadLeftThumbstickDown:
                        this.Animate(this.NearestIndex(this.Center.X) + 1);
                        return true;

                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void Update()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].Carousel = this.Carousel.ToItem(this.Items[i].SourceNormalize, this.Center, i, this.X, this.ItemMargin, this.ItemSpacing);
            }
        }

        private void Animate(int index)
        {
            if (index < 0)
                return;

            if (index >= this.Items.Count)
                return;

            CarouselItems item = this.Items[index];
            this.Animation.Reset(this.X, item.Carousel);

            this.CanvasControl.Paused = false;
            this.CanvasControl.Invalidate();
        }

        private int NearestIndex(float positonX)
        {
            int index = -1;
            float distance = float.MaxValue;

            for (int i = 0; i < this.Items.Count; i++)
            {
                CarouselItems item = this.Items[i];

                float d = System.Math.Abs(positonX - item.Carousel.ActualX);
                if (distance > d)
                {
                    distance = d;
                    index = i;
                }
            }

            return index;
        }

        private int ContainsIndex(Vector2 point)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                CarouselItems item = this.Items[i];

                if (item.Carousel.TextureOutline.ContainsPoint(point))
                    return i;
            }

            return -1;
        }
    }
}