using FanKit.Transformer.Controllers;
using FanKit.Transformer.Mathematics;
using System.Numerics;
using FanKit.Transformer.Indicators;

namespace FanKit.Transformer.Transforms
{
    public partial class QuadrilateralRect : PerspRect
    {
        // Step 0. Initialize
        //public int Count;
        public float DestinationWidth;
        public float DestinationHeight;

        // Step 1. Transformer
        public Quadrilateral StartingQuadrilateral;
        public Quadrilateral Quadrilateral;

        // Step 3. Matrix
        public Matrix4x4 StartingMatrix;
        public Matrix4x4 Matrix;
        //public Matrix4x4 InverseMatrix;

        // Step 4. Host
        //InvertibleMatrix3x2 HostSource;
        //Matrix3x2 HostDestination;
        Matrix3x2 Host;

        // Step 6. Controller
        FreeTransformController Controller;

        public QuadrilateralPointKind PointKind => this.Controller.PointKind;

        protected void Find()
        {
            this.FindHomography(this.Quadrilateral, this.DestinationWidth, this.DestinationHeight);
            this.Matrix = m;
        }

        #region Quadrilaterals.Initialize
        public void Initialize(float destWidth, float destHeight)
        {
            this.DestinationWidth = destWidth;
            this.DestinationHeight = destHeight;
            this.Quadrilateral = new Quadrilateral(0f, 0f, this.DestinationWidth, this.DestinationHeight);

            this.Matrix = Matrix4x4.Identity;
        }

        public void UpdateSource(Quadrilateral source)
        {
            this.Quadrilateral = source;

            this.Find();
        }

        public void UpdateDestination(float destWidth, float destHeight)
        {
            this.DestinationWidth = destWidth;
            this.DestinationHeight = destHeight;

            this.Find();
        }

        public void UpdateAll(float destWidth, float destHeight, Quadrilateral source)
        {
            this.DestinationWidth = destWidth;
            this.DestinationHeight = destHeight;

            this.Quadrilateral = source;

            this.Find();
        }
        #endregion

        #region Quadrilaterals.FreeTransform
        public void CacheFreeTransform(FreeTransformMode mode)
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            this.Host = Matrix3x2.Identity;

            this.Controller = new FreeTransformController(this.Quadrilateral, mode, 8f);
        }

        public void MovePoint(Vector2 point)
        {
            this.Quadrilateral = this.StartingQuadrilateral.MovePoint(this.Controller.PointKind, point);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }

        public void MovePointOfConvexQuadrilateral(Vector2 point)
        {
            this.Quadrilateral = this.Controller.MovePointOfConvexQuadrilateral(this.StartingQuadrilateral, point);

            this.Find();

            this.Host = Matrix3x2.Identity;
        }
        #endregion

        #region Quadrilaterals.Transform
        public void CacheTranslation()
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void CacheTransform()
        {
            this.StartingQuadrilateral = this.Quadrilateral;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingQuadrilateral.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
        }
        //public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        //{
        //    this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
        //    this.T();
        //    indicator.ChangeXY(this.Quadrilateral, mode);
        //}

        public void Translate(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
        }
        //public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate)
        //{
        //    this.Host = Matrix3x2.CreateTranslation(translate);
        //    this.T();
        //    indicator.ChangeXY(this.Quadrilateral, mode);
        //}

        public void Translate(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
        }
        //public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        //{
        //    this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
        //    this.T();
        //    indicator.ChangeXY(this.Quadrilateral, mode);
        //}

        public void TranslateX(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
        }
        //public void TranslateX(IIndicator indicator, BoxMode mode, float translateX)
        //{
        //    this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
        //    this.TX();
        //    indicator.ChangeXY(this.Quadrilateral, mode);
        //}

        public void TranslateY(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
        }
        //public void TranslateY(IIndicator indicator, BoxMode mode, float translateY)
        //{
        //    this.Host = Matrix3x2.CreateTranslation(0f, translateY);
        //    this.TY();
        //    indicator.ChangeXY(this.Quadrilateral, mode);
        //}

        //public void Transform(Matrix3x2 matrix)
        //{
        //    this.Host = matrix;

        //    this.Quadrilateral = this.StartingQuadrilateral * this.Host;
        //    this.Matrix = Math.Transform(this.StartingMatrix, this.Host);
        //    //this.Invert()
        //}

        private void T()
        {
            this.Quadrilateral = Quadrilateral.Translate(this.StartingQuadrilateral, this.Host.M31, this.Host.M32);
            this.Find();
            //this.Invert()
        }
        private void TX()
        {
            this.Quadrilateral = Quadrilateral.TranslateX(this.StartingQuadrilateral, this.Host.M31);
            this.Find();
            //this.Invert()
        }
        private void TY()
        {
            this.Quadrilateral = Quadrilateral.TranslateY(this.StartingQuadrilateral, this.Host.M32);
            this.Find();
            //this.Invert()
        }
        #endregion
    }
}