using FanKit.Transformer.Controllers;
using FanKit.Transformer.Mathematics;
using FanKit.Transformer.Indicators;
using System.Numerics;

namespace FanKit.Transformer.Compute
{
    public abstract partial class PerspQuadSize
    {
        internal Quadrilateral StartingQuadrilateral;
        internal Quadrilateral Quadrilateral;

        internal Matrix3x2 Host;

        internal FreeTransformController Controller;

        internal abstract void Find();

        #region Quadrilaterals.Initialize
        internal void US(Quadrilateral source)
        {
            this.Quadrilateral = source;

            this.Find();
        }
        #endregion

        #region Quadrilaterals.FreeTransform
        internal void CFF(FreeTransformMode mode)
        {
            this.StartingQuadrilateral = this.Quadrilateral;

            this.Host = Matrix3x2.Identity;

            this.Controller = new FreeTransformController(this.Quadrilateral, mode, 8f);
        }

        internal void M0(Vector2 point)
        {
            this.Quadrilateral = this.StartingQuadrilateral.MovePoint(this.Controller.PointKind, point);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        internal void M1(Vector2 point)
        {
            this.Quadrilateral = this.Controller.MovePointOfConvexQuadrilateral(this.StartingQuadrilateral, point);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Quadrilaterals.Transform
        internal void CT()
        {
            this.StartingQuadrilateral = this.Quadrilateral;

            this.Host = Matrix3x2.Identity;
        }

        internal void CF()
        {
            this.StartingQuadrilateral = this.Quadrilateral;

            this.Host = Matrix3x2.Identity;
        }

        internal void STXY0(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }

        internal void T0(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
        }

        internal void TXY0(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
        }

        internal void TX0(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
        }

        internal void TY0(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
        }

        private void T()
        {
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);
            this.Find();
        }
        private void TX()
        {
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);
            this.Find();
        }
        private void TY()
        {
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);
            this.Find();
        }
        #endregion
    }
}