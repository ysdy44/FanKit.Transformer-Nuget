using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class RectQuadrilateral : Compute.PerspQuadrilateral
    {
        public Bounds SourceBounds { get; private set; }
        public Rectangle SourceRect { get; private set; }
        RectMatrix SourceNormalize;

        FreeTransformedBounds TransformedBounds;

        PerspRectMatrix3x3 DestNorm;
        public Quadrilateral Destination => this.Quadrilateral;

        public Matrix4x4 HomographyMatrix => this.Matrix;

        public QuadrilateralPointKind PointKind => this.Controller.PointKind;

        internal override void Find()
        {
            this.DestNorm = this.SourceNormalize.ToPerspMatrix(this.Quadrilateral);
            this.Matrix = this.DestNorm;
        }

        #region Quadrilaterals.Initialize
        public void Initialize(Bounds source)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.StartingMatrix = this.Matrix = Matrix4x4.Identity;

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = default;
            this.StartingQuadrilateral = this.Quadrilateral = new Quadrilateral(this.SourceBounds);
        }

        public void Initialize(Bounds source, Matrix4x4 homographyMatrix)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.StartingMatrix = this.Matrix = homographyMatrix;

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new FreeTransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingQuadrilateral = this.Quadrilateral = this.TransformedBounds.ToQuadrilateral();
        }

        public void Extend(Bounds source)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new FreeTransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingQuadrilateral = this.Quadrilateral = this.TransformedBounds.ToQuadrilateral();
        }

        public void UpdateSource(Bounds source)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        public void UpdateDestination(Quadrilateral destination) => this.UD(destination);

        public void UpdateAll(Bounds source, Quadrilateral destination)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.StartingQuadrilateral = this.Quadrilateral = destination;

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Quadrilaterals.FreeTransform
        public void CacheFreeTransform(FreeTransformMode mode) => this.CFF(mode);

        public void MovePoint(Vector2 point) => this.M0(point);

        public void MovePointOfConvexQuadrilateral(Vector2 point) => this.M1(point);
        #endregion

        #region Quadrilaterals.Set
        public void SetTranslation(Vector2 translate) => this.ST0(translate);
        public void SetTranslation(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 translate) => this.ST1(indicator, anchorMode, translate);

        public void SetTranslationX(float translateX) => this.STX0(translateX);
        public void SetTranslationX(IIndicator indicator, PanelAnchorMode anchorMode, float translateX) => this.STX1(indicator, anchorMode, translateX);

        public void SetTranslationY(float translateY) => this.STY0(translateY);
        public void SetTranslationY(IIndicator indicator, PanelAnchorMode anchorMode, float translateY) => this.STY1(indicator, anchorMode, translateY);

        public void SetTransform(Matrix3x2 matrix) => this.SF0(matrix);
        public void SetTransform(IIndicator indicator, PanelAnchorMode anchorMode, Matrix3x2 matrix) => this.SF1(indicator, anchorMode, matrix);

        public void SetWidth(IIndicator indicator, PanelAnchorMode anchorMode, float value, bool keepRatio) => this.SW(indicator, anchorMode, value, keepRatio);
        public void SetHeight(IIndicator indicator, PanelAnchorMode anchorMode, float value, bool keepRatio) => this.SH(indicator, anchorMode, value, keepRatio);

        public void SetRotation(IIndicator indicator, PanelAnchorMode anchorMode, float rotationAngleInDegrees) => this.SR(indicator, anchorMode, rotationAngleInDegrees);
        public void SetSkew(IIndicator indicator, PanelAnchorMode anchorMode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f) => this.SS(indicator, anchorMode, skewAngleInDegrees, minimum, maximum);
        #endregion

        #region Quadrilaterals.Transform
        public void CacheTranslation() => this.CT();

        public void CacheTransform() => this.CF();

        public void Translate(Vector2 startingPoint, Vector2 point) => this.TD0(startingPoint, point);
        public void Translate(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 startingPoint, Vector2 point) => this.TD1(indicator, anchorMode, startingPoint, point);

        public void Translate(Vector2 translate) => this.T0(translate);
        public void Translate(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 translate) => this.T1(indicator, anchorMode, translate);

        public void Translate(float translateX, float translateY) => this.TXY0(translateX, translateY);
        public void Translate(IIndicator indicator, PanelAnchorMode anchorMode, float translateX, float translateY) => this.TXY1(indicator, anchorMode, translateX, translateY);

        public void TranslateX(float translateX) => this.TX0(translateX);
        public void TranslateX(IIndicator indicator, PanelAnchorMode anchorMode, float translateX) => this.TX1(indicator, anchorMode, translateX);

        public void TranslateY(float translateY) => this.TY0(translateY);
        public void TranslateY(IIndicator indicator, PanelAnchorMode anchorMode, float translateY) => this.TY1(indicator, anchorMode, translateY);

        public void Transform(Matrix3x2 matrix) => this.F0(matrix);
        public void Transform(IIndicator indicator, PanelAnchorMode anchorMode, Matrix3x2 matrix) => this.F1(indicator, anchorMode, matrix);
        #endregion
    }
}