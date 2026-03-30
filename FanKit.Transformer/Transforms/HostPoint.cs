using FanKit.Transformer.Compute;
using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    public partial class HostPoint
    {
        public Vector2 Point
        {
            get => this.Core.Point;
            set => this.Core.Point = value;
        }

        public float TranslationX => this.Host.Matrix.M31;
        public float TranslationY => this.Host.Matrix.M32;
        public Matrix3x2 TransformMatrix => this.Host.Matrix;

        readonly M3x2 Host;
        readonly ComposerPoint Core;

        public HostPoint()
        {
            this.Host = new M3x2();
            this.Core = new ComposerPoint(this.Host);
        }

        #region Points.Set
        public void SetTranslation(Vector2 translate)
        {
            this.Core.ST0(translate);
        }
        public void SetTranslation(IIndicator indicator, Vector2 translate)
        {
            this.Core.ST1(indicator, translate);
        }

        public void SetTranslationX(float translateX)
        {
            this.Core.STX0(translateX);
        }
        public void SetTranslationX(IIndicator indicator, float translateX)
        {
            this.Core.STX1(indicator, translateX);
        }

        public void SetTranslationY(float translateY)
        {
            this.Core.STY0(translateY);
        }
        public void SetTranslationY(IIndicator indicator, float translateY)
        {
            this.Core.STY1(indicator, translateY);
        }
        #endregion

        #region Points.Transform
        public void CacheTranslation()
        {
            this.Core.CT();
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.Core.TD0(startingPoint, point);
        }
        public void Translate(IIndicator indicator, Vector2 startingPoint, Vector2 point)
        {
            this.Core.TD1(indicator, startingPoint, point);
        }

        public void Translate(Vector2 translate)
        {
            this.Core.T0(translate);
        }
        public void Translate(IIndicator indicator, Vector2 translate)
        {
            this.Core.T1(indicator, translate);
        }

        public void Translate(float translateX, float translateY)
        {
            this.Core.TXY0(translateX, translateY);
        }
        public void Translate(IIndicator indicator, float translateX, float translateY)
        {
            this.Core.TXY1(indicator, translateX, translateY);
        }

        public void TranslateX(float translateX)
        {
            this.Core.TX0(translateX);
        }
        public void TranslateX(IIndicator indicator, float translateX)
        {
            this.Core.TX1(indicator, translateX);
        }

        public void TranslateY(float translateY)
        {
            this.Core.TY0(translateY);
        }
        public void TranslateY(IIndicator indicator, float translateY)
        {
            this.Core.TY1(indicator, translateY);
        }
        #endregion
    }
}