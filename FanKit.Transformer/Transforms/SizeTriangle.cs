using FanKit.Transformer.Compute;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class SizeTriangle : AffineTriangle2
    {
        public float SourceWidth { get; private set; }
        public float SourceHeight { get; private set; }
        SizeMatrix SourceNormalize;

        public Triangle Destination => this.Triangle;

        public Matrix3x2 HomographyMatrix => this.Matrix;

        public float TranslationX => this.Host.M31;
        public float TranslationY => this.Host.M32;
        public Matrix3x2 TransformMatrix => this.Host;

        internal override void Find()
        {
            this.DestNorm = this.Triangle.Normalize();
            this.Matrix = this.SourceNormalize.Affine(this.DestNorm);
        }

        #region Triangles.Initialize
        public void Initialize(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingMatrix = this.Matrix = Matrix3x2.Identity;

            this.Host = Matrix3x2.Identity;

            this.StartingTriangle = this.Triangle = new Triangle(this.SourceWidth, this.SourceHeight);
        }

        public void Initialize(float sourceWidth, float sourceHeight, Matrix3x2 homographyMatrix)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingMatrix = this.Matrix = homographyMatrix;

            this.Host = Matrix3x2.Identity;

            this.StartingTriangle = this.Triangle = new Triangle(this.SourceWidth, this.SourceHeight, this.Matrix);
        }

        public void Extend(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.Host = Matrix3x2.Identity;

            this.StartingTriangle = this.Triangle = new Triangle(this.SourceWidth, this.SourceHeight, this.Matrix);
        }

        public void UpdateSource(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        public void UpdateDestination(Triangle destination) => this.UD(destination);

        public void UpdateAll(float sourceWidth, float sourceHeight, Triangle destination)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingTriangle = this.Triangle = destination;

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Triangles.Set
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

        #region Triangles.Transform
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

        #region Triangles.Transform2
        public void CacheRotation(Vector2 point) => this.CR(point);

        public void CacheTransform(TransformMode mode) => this.CF1(mode);

        public void Rotate(Vector2 point, float stepFrequency = float.NaN) => this.R0(point, stepFrequency);
        public void Rotate(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 point, float stepFrequency = float.NaN) => this.R1(indicator, anchorMode, point, stepFrequency);

        public void TransformSize(Vector2 point, bool keepRatio, bool centeredScaling) => this.TWH0(point, keepRatio, centeredScaling);
        public void TransformSize(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 point, bool keepRatio, bool centeredScaling) => this.TWH1(indicator, anchorMode, point, keepRatio, centeredScaling);

        public void TransformSkew(Vector2 point, bool keepRatio, bool centeredScaling) => this.TS0(point, keepRatio, centeredScaling);
        public void TransformSkew(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 point, bool keepRatio, bool centeredScaling) => this.TS1(indicator, anchorMode, point, keepRatio, centeredScaling);
        #endregion
    }
}