using FanKit.Transformer;
using FanKit.Transformer.Cache;
using FanKit.Transformer.TestApp;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformer.TestApp
{
    public sealed partial class BoxPage : Page
    {
        // Box
        static readonly Bounds B = new Bounds(50, 50, 150, 150);
        static readonly Quadrilateral Q = new Quadrilateral(B);

        readonly Box0 B0 = new Box0(Q);
        readonly Box1 B1 = new Box1(Q);
        readonly Box2 B2 = new Box2(Q);
        readonly Box3 B3 = new Box3(Q);

        // Line
        static readonly Vector2 P1 = new Vector2(50, 50);
        static readonly Vector2 P2 = new Vector2(150, 50);

        readonly Line0 L0 = new Line0(P1, P2);
        readonly Line1 L1 = new Line1(P1, P2);
        readonly Line2 L2 = new Line2(P1, P2);
        readonly Line3 L3 = new Line3(P1, P2);

        public BoxPage()
        {
            this.InitializeComponent();

            // Box
            this.CanvasControlB0.Draw += (s, e) => e.DrawingSession.DrawBox(this.B0);
            this.CanvasControlB1.Draw += (s, e) => e.DrawingSession.DrawBox(this.B1);
            this.CanvasControlB2.Draw += (s, e) => e.DrawingSession.DrawBox(this.B2);
            this.CanvasControlB3.Draw += (s, e) => e.DrawingSession.DrawBox(this.B3);

            // Line
            this.CanvasControlL0.Draw += (s, e) => e.DrawingSession.DrawLine(this.L0);
            this.CanvasControlL1.Draw += (s, e) => e.DrawingSession.DrawLine(this.L1);
            this.CanvasControlL2.Draw += (s, e) => e.DrawingSession.DrawLine(this.L2);
            this.CanvasControlL3.Draw += (s, e) => e.DrawingSession.DrawLine(this.L3);
        }
    }
}