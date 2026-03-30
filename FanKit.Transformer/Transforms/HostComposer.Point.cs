using FanKit.Transformer.Indicators;
using FanKit.Transformer.Mathematics;
using System.Numerics;

namespace FanKit.Transformer.Transforms
{
    partial class HostComposer
    {
        public Vector2 PointPoint => this.Point.Point;

        #region Points.Set
        public void PointSetTranslation(Vector2 translate)
        {
            this.Point.ST0(translate);
        }
        public void PointSetTranslation(IIndicator indicator, Vector2 translate)
        {
            this.Point.ST1(indicator, translate);
        }

        public void PointSetTranslationX(float translateX)
        {
            this.Point.STX0(translateX);
        }
        public void PointSetTranslationX(IIndicator indicator, float translateX)
        {
            this.Point.STX1(indicator, translateX);
        }

        public void PointSetTranslationY(float translateY)
        {
            this.Point.STY0(translateY);
        }
        public void PointSetTranslationY(IIndicator indicator, float translateY)
        {
            this.Point.STY1(indicator, translateY);
        }
        #endregion

        #region Points.Transform
        public void PointCacheTranslation()
        {
            this.Point.CT();
        }

        public void PointTranslate(Vector2 startingPoint, Vector2 point)
        {
            this.Point.TD0(startingPoint, point);
        }
        public void PointTranslate(IIndicator indicator, Vector2 startingPoint, Vector2 point)
        {
            this.Point.TD1(indicator, startingPoint, point);
        }

        public void PointTranslate(Vector2 translate)
        {
            this.Point.T0(translate);
        }
        public void PointTranslate(IIndicator indicator, Vector2 translate)
        {
            this.Point.T1(indicator, translate);
        }

        public void PointTranslate(float translateX, float translateY)
        {
            this.Point.TXY0(translateX, translateY);
        }
        public void PointTranslate(IIndicator indicator, float translateX, float translateY)
        {
            this.Point.TXY1(indicator, translateX, translateY);
        }

        public void PointTranslateX(float translateX)
        {
            this.Point.TX0(translateX);
        }
        public void PointTranslateX(IIndicator indicator, float translateX)
        {
            this.Point.TX1(indicator, translateX);
        }

        public void PointTranslateY(float translateY)
        {
            this.Point.TY0(translateY);
        }
        public void PointTranslateY(IIndicator indicator, float translateY)
        {
            this.Point.TY1(indicator, translateY);
        }
        #endregion
    }
}