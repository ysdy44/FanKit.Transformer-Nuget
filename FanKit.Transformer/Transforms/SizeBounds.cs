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

        public void Initialize(float sourceWidth, float sourceHeight, Matrix2x2 homographyMatrix)
        {
            this.SourceWidth = sourceWidth;
            this.SourceHeight = sourceHeight;
            this.SourceNormalize = new SizeMatrix(this.SourceWidth, this.SourceHeight);

            this.StartingMatrix = this.Matrix = homographyMatrix;

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

        public void UpdateDestination(Bounds destination) => this.UD(destination);

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
        public void SetTranslation(Vector2 translate) => this.ST0(translate);
        public void SetTranslation(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 translate) => this.ST1(indicator, anchorMode, translate);

        public void SetTranslationX(float translateX) => this.STX0(translateX);
        public void SetTranslationX(IIndicator indicator, PanelAnchorMode anchorMode, float translateX) => this.STX1(indicator, anchorMode, translateX);

        public void SetTranslationY(float translateY) => this.STY0(translateY);
        public void SetTranslationY(IIndicator indicator, PanelAnchorMode anchorMode, float translateY) => this.STY1(indicator, anchorMode, translateY);

        public void SetTransform(Matrix2x2 matrix) => this.SF0(matrix);
        public void SetTransform(IIndicator indicator, PanelAnchorMode anchorMode, Matrix2x2 matrix) => this.SF1(indicator, anchorMode, matrix);

        public void SetWidth(IIndicator indicator, PanelAnchorMode anchorMode, float value, bool keepRatio) => this.SW(indicator, anchorMode, value, keepRatio);
        public void SetHeight(IIndicator indicator, PanelAnchorMode anchorMode, float value, bool keepRatio) => this.SH(indicator, anchorMode, value, keepRatio);
        #endregion

        #region Bounds.Transform
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

        public void Transform(Matrix2x2 matrix) => this.F0(matrix);
        public void Transform(IIndicator indicator, PanelAnchorMode anchorMode, Matrix2x2 matrix) => this.F1(indicator, anchorMode, matrix);
        #endregion

        #region Bounds.Transform2
        public void CacheTransform(CropMode mode) => this.CF1(mode);

        public void TransformSize(Vector2 point, bool keepRatio, bool centeredScaling) => this.TWH0(point, keepRatio, centeredScaling);
        public void TransformSize(IIndicator indicator, PanelAnchorMode anchorMode, Vector2 point, bool keepRatio, bool centeredScaling) => this.TWH1(indicator, anchorMode, point, keepRatio, centeredScaling);
        #endregion
    }
}