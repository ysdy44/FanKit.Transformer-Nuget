using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class SizeQuadrilateral : Compute.PerspQuadrilateral
    {
        public float SourceWidth { get; private set; }
        public float SourceHeight { get; private set; }
        SizeMatrix SourceNormalize;

        FreeTransformedSize TransformedBounds;

        PerspSizeMatrix3x3 DestNorm;
        public Quadrilateral Destination => this.Quadrilateral;

        public Matrix4x4 HomographyMatrix => this.Matrix;

        public QuadrilateralPointKind PointKind => this.Controller.PointKind;

        internal override void Find()
        {
            this.DestNorm = this.SourceNormalize.ToPerspMatrix(this.Quadrilateral);
            this.Matrix = this.DestNorm;
        }

        #region Quadrilaterals.Initialize
        public void Initialize(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingMatrix = this.Matrix = Matrix4x4.Identity;

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = default;
            this.StartingQuadrilateral = this.Quadrilateral = new Quadrilateral(this.SourceWidth, this.SourceHeight);
        }

        public void Initialize(float sourceWidth, float sourceHeight, Matrix4x4 matrix)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingMatrix = this.Matrix = matrix;

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new FreeTransformedSize(this.SourceWidth, this.SourceHeight, this.Matrix);
            this.StartingQuadrilateral = this.Quadrilateral = this.TransformedBounds.ToQuadrilateral();
        }

        public void Extend(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new FreeTransformedSize(this.SourceWidth, this.SourceHeight, this.Matrix);
            this.StartingQuadrilateral = this.Quadrilateral = this.TransformedBounds.ToQuadrilateral();
        }

        public void UpdateSource(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        public void UpdateDestination(Quadrilateral destination)
        {
            this.StartingQuadrilateral = this.Quadrilateral = destination;

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        public void UpdateAll(float sourceWidth, float sourceHeight, Quadrilateral destination)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

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
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate) => this.ST1(indicator, mode, translate);

        public void SetTranslationX(float translateX) => this.STX0(translateX);
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX) => this.STX1(indicator, mode, translateX);

        public void SetTranslationY(float translateY) => this.STY0(translateY);
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY) => this.STY1(indicator, mode, translateY);

        public void SetTransform(Matrix3x2 matrix) => this.SF0(matrix);
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix) => this.SF1(indicator, mode, matrix);

        public void SetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio) => this.SW(indicator, mode, value, keepRatio);
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio) => this.SH(indicator, mode, value, keepRatio);

        public void SetRotation(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees) => this.SR(indicator, mode, rotationAngleInDegrees);
        public void SetSkew(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f) => this.SS(indicator, mode, skewAngleInDegrees, minimum, maximum);
        #endregion

        #region Quadrilaterals.Transform
        public void CacheTranslation() => this.CT();

        public void CacheTransform() => this.CF();

        public void Translate(Vector2 startingPoint, Vector2 point) => this.TD0(startingPoint, point);
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point) => this.TD1(indicator, mode, startingPoint, point);

        public void Translate(Vector2 translate) => this.T0(translate);
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate) => this.T1(indicator, mode, translate);

        public void Translate(float translateX, float translateY) => this.TXY0(translateX, translateY);
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY) => this.TXY1(indicator, mode, translateX, translateY);

        public void TranslateX(float translateX) => this.TX0(translateX);
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX) => this.TX1(indicator, mode, translateX);

        public void TranslateY(float translateY) => this.TY0(translateY);
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY) => this.TY1(indicator, mode, translateY);

        public void Transform(Matrix3x2 matrix) => this.F(matrix);
        #endregion
    }
}