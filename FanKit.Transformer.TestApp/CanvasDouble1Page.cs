using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas;

namespace FanKit.Transformer.TestApp
{
    public class CanvasDouble1Page : CanvasDual1Page
    {
        public override void CreateResources(ICanvasResourceCreator resourceCreator, CanvasCreateResourcesEventArgs args) => this.InitCanvas();
        public override void DrawSource(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession) { }
        public override void DrawDestination(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession) { }
        public override void DrawThumb(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession) { }

        public override void CacheSingle() { }
        public override void Single() { }
        public override void DisposeSingle() { }
        public override void Over() { }

        public override void UpdateCanvasControl1()
        {
            this.UpdateCanvas();
            this.Invalidate();
        }
        public override void UpdateCanvasControl2()
        {
            this.UpdateCanvas();
            this.Invalidate();
        }
    }
}