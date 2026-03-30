using FanKit.Transformer.Compute;
using FanKit.Transformer.Controllers;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FanKit.Transformer.Transforms
{
    public partial class SizeBounds : MapBounds2
    {
        public float SourceWidth { get; private set; }
        public float SourceHeight { get; private set; }
        SizeMatrix SourceNormalize;

        public Bounds Destination => this.Bounds;

        public Matrix2x2 HomographyMatrix => this.Matrix;

        public float TranslationX => this.Host.TranslateX;
        public float TranslationY => this.Host.TranslateY;
        public Matrix2x2 TransformMatrix => this.Host;

        internal override void Find()
        {
            this.DestNorm = this.Bounds.Normalize();
            this.Matrix = this.SourceNormalize.Map(this.DestNorm);
        }

        #region Bounds.Initialize
        public void Initialize(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingMatrix = this.Matrix = Matrix2x2.Identity;

            this.Host = Matrix2x2.Identity;

            this.StartingBounds = this.Bounds = new Bounds(this.SourceWidth, this.SourceHeight);
        }

        public void Initialize(float sourceWidth, float sourceHeight, Matrix2x2 matrix)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingMatrix = this.Matrix = matrix;

            this.Host = Matrix2x2.Identity;

            this.StartingBounds = this.Bounds = new Bounds(this.SourceWidth, this.SourceHeight, this.Matrix);
        }

        public void Extend(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.Host = Matrix2x2.Identity;

            this.StartingBounds = this.Bounds = new Bounds(this.SourceWidth, this.SourceHeight, this.Matrix);
        }

        public void UpdateSource(float sourceWidth, float sourceHeight)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.Find();

            this.Host = Matrix2x2.Identity;
        }

        public void UpdateDestination(Bounds destination)
        {
            this.UD(destination);
        }

        public void UpdateAll(float sourceWidth, float sourceHeight, Bounds destination)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingBounds = this.Bounds = destination;

            this.Find();

            this.Host = Matrix2x2.Identity;
        }
        #endregion

        #region Bounds.Set
        public void SetTranslation(Vector2 translate)
        {
            this.ST0(translate);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.ST1(indicator, mode, translate);
        }

        public void SetTranslationX(float translateX)
        {
            this.STX0(translateX);
        }
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.STX1(indicator, mode, translateX);
        }

        public void SetTranslationY(float translateY)
        {
            this.STY0(translateY);
        }
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.STY1(indicator, mode, translateY);
        }

        public void SetTransform(Matrix2x2 matrix)
        {
            this.SF0(matrix);
        }
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix2x2 matrix)
        {
            this.SF1(indicator, mode, matrix);
        }

        public void SetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.SW(indicator, mode, value, keepRatio);
        }
        public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        {
            this.SH(indicator, mode, value, keepRatio);
        }
        #endregion

        #region Bounds.Transform
        public void CacheTranslation()
        {
            this.CT();
        }

        public void CacheTransform()
        {
            this.CF();
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.TD0(startingPoint, point);
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.TD1(indicator, mode, startingPoint, point);
        }

        public void Translate(Vector2 translate)
        {
            this.T0(translate);
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.T1(indicator, mode, translate);
        }

        public void Translate(float translateX, float translateY)
        {
            this.TXY0(translateX, translateY);
        }
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.TXY1(indicator, mode, translateX, translateY);
        }

        public void TranslateX(float translateX)
        {
            this.TX0(translateX);
        }
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.TX1(indicator, mode, translateX);
        }

        public void TranslateY(float translateY)
        {
            this.TY0(translateY);
        }
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.TY1(indicator, mode, translateY);
        }

        public void Transform(Matrix2x2 matrix)
        {
            this.Transform2(matrix);
        }
        #endregion

        #region Bounds.Transform2
        public void CacheTransform(CropMode mode)
        {
            this.CF(mode);
        }

        public void TransformSize(Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.TWH0(point, keepRatio, centeredScaling);
        }
        public void TransformSize(IIndicator indicator, BoxMode mode, Vector2 point, bool keepRatio, bool centeredScaling)
        {
            this.TWH1(indicator, mode, point, keepRatio, centeredScaling);
        }
        #endregion
    }
}