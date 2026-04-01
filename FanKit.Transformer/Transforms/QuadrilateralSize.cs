using FanKit.Transformer.Compute;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class QuadrilateralSize : PerspQuadSize
    {
        public float DestinationWidth { get; private set; }
        public float DestinationHeight { get; private set; }

        public Quadrilateral Source => this.Quadrilateral;

        public Matrix4x4 HomographyMatrix => this.Core.m;

        public QuadrilateralPointKind PointKind => this.Controller.PointKind;

        readonly PerspSize Core = new PerspSize();

        internal override void Find()
        {
            this.Core.FindHomography(this.Quadrilateral, this.DestinationWidth, this.DestinationHeight);
        }

        #region Quadrilaterals.Initialize
        public void Initialize(float destinationWidth, float destinationHeight)
        {
            this.DestinationWidth = destinationWidth;
            this.DestinationHeight = destinationHeight;
            this.Quadrilateral = new Quadrilateral(0f, 0f, this.DestinationWidth, this.DestinationHeight);

            this.Core.m = Matrix4x4.Identity;
        }

        public void UpdateSource(Quadrilateral source) => this.US(source);

        public void UpdateDestination(float destinationWidth, float destinationHeight)
        {
            this.DestinationWidth = destinationWidth;
            this.DestinationHeight = destinationHeight;

            this.Find();
        }

        public void UpdateAll(float destinationWidth, float destinationHeight, Quadrilateral source)
        {
            this.DestinationWidth = destinationWidth;
            this.DestinationHeight = destinationHeight;

            this.Quadrilateral = source;

            this.Find();
        }
        #endregion

        #region Quadrilaterals.FreeTransform
        public void CacheFreeTransform(FreeTransformMode mode) => this.CFF(mode);

        public void MovePoint(Vector2 point) => this.M0(point);

        public void MovePointOfConvexQuadrilateral(Vector2 point) => this.M1(point);
        #endregion

        #region Quadrilaterals.Transform
        public void CacheTranslation() => this.CT();

        public void CacheTransform() => this.CF();

        public void Translate(Vector2 startingPoint, Vector2 point) => this.STXY0(startingPoint, point);

        public void Translate(Vector2 translate) => this.T0(translate);

        public void Translate(float translateX, float translateY) => this.TXY0(translateX, translateY);

        public void TranslateX(float translateX) => this.TX0(translateX);

        public void TranslateY(float translateY) => this.TY0(translateY);
        #endregion
    }
}