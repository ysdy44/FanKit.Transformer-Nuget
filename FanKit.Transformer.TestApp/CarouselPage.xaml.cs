using FanKit.Transformer.Mathematics;
using FanKit.Transformer.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class CarouselPage : Page
    {
        Vector2 Center;

        CanvasBitmap Bitmap;
        SizeMatrix SourceNormalize;

        readonly Carousel Carousel = new Carousel(256f, 256f);
        CarouselItem1 Item;

        public CarouselPage()
        {
            this.InitializeComponent();
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
                this.Item = new CarouselItem1(this.Carousel, this.Center.X, this.Center.Y, 0f);

                this.CanvasControl.Invalidate();
            };
            this.CanvasControl.PointerMoved += (s, e) =>
            {
                PointerPoint pp = e.GetCurrentPoint(this.CanvasControl);

                double x;
                switch (this.CanvasControl.FlowDirection)
                {
                    case FlowDirection.RightToLeft:
                        x = this.CanvasControl.ActualWidth - pp.Position.X;
                        break;
                    default:
                        x = pp.Position.X;
                        break;
                }

                this.Item = new CarouselItem1(this.Carousel, this.Center.X, this.Center.Y, (this.Center.X - (float)x) / 256f);

                this.CanvasControl.Invalidate();
            };
        }

        private async Task CreateResourcesAsync(ICanvasResourceCreator sender)
        {
            this.Bitmap = await CanvasBitmap.LoadAsync(sender, "Images/avatar.jpg");

            float width = (float)this.Bitmap.Size.Width;
            float height = (float)this.Bitmap.Size.Height;
            this.SourceNormalize = new SizeMatrix(width, height);

            this.CanvasControl.Invalidate();
        }

        private void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.Bitmap == null)
                return;

            drawingSession.DrawImage(new Transform3DEffect
            {
                TransformMatrix = this.SourceNormalize.ToPerspMatrix(this.Item.Box),
                Source = this.Bitmap,
            });
        }
    }
}