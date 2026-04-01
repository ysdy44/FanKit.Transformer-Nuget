using FanKit.Transformer.Compute;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class RectTriangle : AffineTriangle2
    {
        public Bounds SourceBounds { get; private set; }
        public Rectangle SourceRect { get; private set; }
        RectMatrix SourceNormalize;

        TransformedBounds TransformedBounds;

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
        public void Initialize(Bounds source)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.StartingMatrix = this.Matrix = Matrix3x2.Identity;

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = default;
            this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);
        }

        public void Initialize(Bounds source, Matrix3x2 matrix)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.StartingMatrix = this.Matrix = matrix;

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        public void Extend(Bounds source)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.Host = Matrix3x2.Identity;

            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        public void UpdateSource(Bounds source)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        public void UpdateDestination(Triangle destination) => this.UD(destination);

        public void UpdateAll(Bounds source, Triangle destination)
        {
            this.SourceBounds = source;
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            this.StartingTriangle = this.Triangle = destination;

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Triangles.Set
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

        #region Triangles.Transform
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

        #region Triangles.Transform2
        public void CacheRotation(Vector2 point) => this.CR(point);

        public void CacheTransform(TransformMode mode) => this.CF(mode);

        public void Rotate(Vector2 point, float stepFrequency = float.NaN) => this.R0(point, stepFrequency);
        public void Rotate(IIndicator indicator, BoxMode mode, Vector2 point, float stepFrequency = float.NaN) => this.R1(indicator, mode, point, stepFrequency);

        public void TransformSize(Vector2 point, bool keepRatio, bool centeredScaling) => this.TWH0(point, keepRatio, centeredScaling);
        public void TransformSize(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling) => this.TWH1(indicator, mode, point, keepRatio, centeredScaling);

        public void TransformSkew(Vector2 point, bool keepRatio, bool centeredScaling) => this.TS0(point, keepRatio, centeredScaling);
        public void TransformSkew(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling) => this.TS1(indicator, mode, point, keepRatio, centeredScaling);
        #endregion
    }
}