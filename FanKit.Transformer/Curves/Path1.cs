using FanKit.Transformer.Cache;
using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System;
using FanKit.Transformer.Indicators;

namespace FanKit.Transformer.Curves
{
    public partial class Path1 : IGetFigure
    {
        // Step 0. Initialize
        //public int Count;
        public Bounds SourceBounds;
        private Bounds b;
        public Rectangle SourceRect;
        public RectMatrix SourceNormalize;

        // Step 1. Transformer
        TransformedBounds TransformedBounds;
        public Triangle StartingTriangle;
        public Triangle Triangle;

        // Step 2. Homography Matrix
        //Matrix3x2 DestNorm;

        // Step 3. Matrix
        //public Matrix3x2 StartingMatrix;
        //public Matrix3x2 Matrix;
        //public Matrix3x2 InverseMatrix;

        // Step 4. Host
        //InvertibleMatrix3x2 HostSourceNorm;
        //Matrix3x2 HostDestNorm;
        Matrix3x2 Host;
        public float HostTranslateX => this.Host.M31;
        public float HostTranslateY => this.Host.M32;
        public Matrix3x2 HostMatrix => this.Host;

        // Step 6. Controller
        //TransformController Controller;

        //ControllerRadians Radians;

        /*
        void Invert()
        {
            Matrix3x2.Invert(this.Matrix, out this.InverseMatrix);
        }
         */

        /*
        void Find()
        {
            this.DestNorm = this.Triangle.Normalize();
            this.Matrix = this.SourceNormalize.Affine(this.DestNorm);
            this.Invert();
        }
         */

        /*
        void FindHomography()
        {
            this.HostDestNorm = this.Triangle.Normalize();
            this.Host = this.HostSourceNorm * this.HostDestNorm;
        }
         */

        #region Triangles.Initialize
        private void Extend()
        {
            this.SourceBounds = Bounds.Infinity;

            for (int i = 1; i < this.Data.Count; i++)
            {
                Segment1 previous = this.Data[i - 1];
                Segment1 next = this.Data[i];

                this.b = Segment1.Extend(previous, next);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }

            if (this.IsClosed)
            {
                Segment1 first = this.Data[0];
                Segment1 last = this.Data[this.Data.Count - 1];

                this.b = Segment1.Extend(last, first);
                this.SourceBounds = Bounds.Union(this.SourceBounds, this.b);
            }
        }
        private void EndExtend()
        {
            // Step 0. Initialize
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        //public void Reset()
        //{
        //    // Step 0. Initialize
        //    //this.Count = 0;

        //    // Step 2. Homography Matrix
        //    // Step 3. Matrix
        //    this.Find();

        //    // Step 4. Host
        //    this.Host = Matrix3x2.Identity;
        //}

        public void Reset(Triangle triangle)
        {
            // Step 0. Initialize
            //this.Count = 1;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle = triangle;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            //this.Find();
            //this.RawToMap();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        ///*
        //public void Reset(IEnumerable<IGetTriangle> items)
        //{
        //    this.Count = 0;

        //    foreach (IGetTriangle item in items)
        //    {
        //        if (item.HasTriangle())
        //        {
        //            if (this.Count == 0)
        //                this.StartingTriangle = this.Triangle = item.GetTriangle();

        //            this.Count++;
        //        }
        //    }

        //    switch (this.Count)
        //    {
        //        case 0:
        //            break;
        //        case 1:
        //            break;
        //        default:
        //            // Step 0. Initialize
        //            this.SourceBounds = new Bounds(items);

        //            // Step 1. Transformer
        //            this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);

        //            // Step 2. Homography Matrix
        //            // Step 3. Matrix
        //            this.Find();

        //            // Step 4. Host
        //            this.Host = Matrix3x2.Identity;
        //            break;
        //    }
        //}
        // */
        #endregion

        #region Triangles.SelectedItems
        public bool IsClosed;

        public List<Segment1> Data;

        public PathSetting Setting { get; } = new PathSetting();
        public int Count => this.Data.Count;
        public int GetChecksCount() => this.Data.Count(GetIsChecked);
        private static bool GetIsChecked(Segment1 item) => item.IsChecked;

        public Path1(List<Segment1> items)
        {
            this.Data = items;
            //this.Matrix = Matrix3x2.Identity;
            //this.InverseMatrix = Matrix3x2.Identity;

            this.Extend();

            // Step 0. Initialize
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        public void Complete()
        {
            this.Extend();

            this.EndExtend();
        }

        public void Select(int index)
        {
            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];

                if (item.IsChecked)
                {
                    if (index != i)
                    {
                        this.Data[i] = new Segment1
                        {
                            IsChecked = false,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
                else
                {
                    if (index == i)
                    {
                        this.Data[i] = new Segment1
                        {
                            IsChecked = true,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
            }
        }

        public void DeselectAll()
        {
            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];

                if (item.IsChecked)
                {
                    this.Data[i] = new Segment1
                    {
                        IsChecked = false,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }
        }

        public void RectChooseItems(Bounds bounds)
        {
            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];

                if (bounds.ContainsPoint(item.Point.Point))
                {
                    if (item.IsChecked is false)
                    {
                        this.Data[i] = new Segment1
                        {
                            IsChecked = true,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
                else
                {
                    if (item.IsChecked)
                    {
                        this.Data[i] = new Segment1
                        {
                            IsChecked = false,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Point = item.Point,
                            Actual = item.Actual,
                        };
                    }
                }
            }
        }

        /*
        public void RawToMap()
        {
            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }
         */
        #endregion

        #region Triangles.SelectedItems.Set
        public void SetTranslationSelectedItems(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateRaw();
        }
        public void SetTranslationSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateRaw();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void SetTranslationXSelectedItems(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateRaw();
        }
        public void SetTranslationXSelectedItems(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateRaw();
            indicator.ChangeX(this.Triangle, mode);
        }

        public void SetTranslationYSelectedItems(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateRaw();
        }
        public void SetTranslationYSelectedItems(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateRaw();
            indicator.ChangeY(this.Triangle, mode);
        }

        public void SetTransformSelectedItems(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.TransformMap();
        }
        public void SetTransformSelectedItems(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.TransformMap();
            indicator.ChangeAll(this.Triangle, mode);
        }

        //public void SetWidthSelectedItems(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        //{
        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = indicator.CreateWidth(this.StartingTriangle, mode, value, keepRatio);

        //    // Step 2. Homography Matrix
        //    // Step 3. Matrix
        //    this.Find();

        //    // Step 4. Host
        //    //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
        //    //this.FindHomography();

        //    indicator.ChangeXYWH(this.Triangle, mode);
        //}
        //public void SetHeightSelectedItems(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        //{
        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = indicator.CreateHeight(this.StartingTriangle, mode, value, keepRatio);

        //    // Step 2. Homography Matrix
        //    // Step 3. Matrix
        //    this.Find();

        //    // Step 4. Host
        //    //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
        //    //this.FindHomography();

        //    indicator.ChangeXYWH(this.Triangle, mode);
        //}

        //public void SetRotationSelectedItems(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        //{
        //    // Step 4. Host
        //    this.Host = indicator.CreateRotation(rotationAngleInDegrees);

        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

        //    // Step 3. Matrix
        //    this.StartingMatrix = this.Matrix;
        //    this.Matrix = this.StartingMatrix * this.Host;
        //    //this.Invert();

        //    indicator.ChangeXYWHRS(this.Triangle, mode);
        //}
        //public void SetSkewSelectedItems(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f)
        //{
        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = indicator.CreateSkew(this.StartingTriangle, mode, skewAngleInDegrees, minimum, maximum);

        //    // Step 2. Homography Matrix
        //    // Step 3. Matrix
        //    this.Find();

        //    // Step 4. Host
        //    //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
        //    //this.FindHomography();

        //    indicator.ChangeXYWHRS(this.Triangle, mode);
        //}

        private void TranslateRaw()
        {
            bool has = false;

            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Translate(item.Point, this.HostTranslateX, this.HostTranslateY);
                    this.Data[i] = new Segment1
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }

        private void TransformMap()
        {
            bool has = false;

            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Transform(item.Point, this.HostMatrix);
                    this.Data[i] = new Segment1
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }
        #endregion

        #region Triangles.SelectedItems.Set.Index
        public void SetTranslation(float translateX, float translateY, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            SI(point, index);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, float translateX, float translateY, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            SI(point, index);
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void SetTranslation(Vector2 translate, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            SI(point, index);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            SI(point, index);
            indicator.ChangeXY(this.Triangle, mode);
        }

        private void SI(Vector2 point, int index)
        {
            Segment1 segment = this.Data[index];
            Node node = segment.Point.MovePoint(point);

            this.Data[index] = new Segment1
            {
                IsChecked = true,
                Point = node,
                // C# 9.0 : var a = segment with { ... }

                //IsChecked = segment.IsChecked,
                IsSmooth = segment.IsSmooth,
                Starting = segment.Starting,
                //Point = segment.Point,
                Actual = segment.Actual,
            };

            this.Extend();

            this.EndExtend();
        }
        #endregion

        #region Triangles.SelectedItems.Transform
        public void CacheTranslationSelectedItems()
        {
            this.CacheRaw();

            this.Host = Matrix3x2.Identity;
        }

        public void CacheTransformSelectedItems()
        {
            this.CacheMap();

            this.Host = Matrix3x2.Identity;
        }

        public void TranslateSelectedItems(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.TranslateStarting();
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateSelectedItems(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);

            this.TranslateStarting();
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateSelectedItems(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateStarting();
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateXSelectedItems(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateStarting();
        }
        public void TranslateXSelectedItems(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateYSelectedItems(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateStarting();
        }
        public void TranslateYSelectedItems(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TransformSelectedItems(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.TransformStarting();
        }

        private void CacheRaw()
        {
            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];
                if (item.IsChecked)
                {
                    this.Data[i] = new Segment1
                    {
                        IsChecked = true,
                        Starting = item.Point,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        //Starting = item.Starting,
                        Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }
        }

        private void CacheMap()
        {
            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];
                if (item.IsChecked)
                {
                    this.Data[i] = new Segment1
                    {
                        IsChecked = true,
                        Starting = item.Point,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        //Starting = item.Starting,
                        Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }
        }

        private void TranslateStarting()
        {
            bool has = false;

            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Translate(item.Starting, this.HostTranslateX, this.HostTranslateY);
                    this.Data[i] = new Segment1
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }

        private void TransformStarting()
        {
            bool has = false;

            for (int i = 0; i < this.Data.Count; i++)
            {
                Segment1 item = this.Data[i];
                if (item.IsChecked)
                {
                    has = true;

                    Node p = Node.Transform(item.Starting, this.HostMatrix);
                    this.Data[i] = new Segment1
                    {
                        IsChecked = true,
                        Point = p,
                        // C# 9.0 : var a = item with { ... }

                        //IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        //Point = item.Point,
                        Actual = item.Actual,
                    };
                }
            }

            if (has) this.Extend();

            this.EndExtend();
        }
        #endregion

        #region Triangles.Set
        public void SetTranslation(Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert();
            //this.RawToMap();
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            //this.Invert();
            //this.RawToMap();

            indicator.ChangeXY(this.Triangle, mode);
        }

        public void SetTranslationX(float translateX)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert();
            //this.RawToMap();
        }
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            //this.Invert();
            //this.RawToMap();

            indicator.ChangeX(this.Triangle, mode);
        }

        public void SetTranslationY(float translateY)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert();
            //this.RawToMap();
        }
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            //this.Invert();
            //this.RawToMap();

            indicator.ChangeY(this.Triangle, mode);
        }

        public void SetTransform(Matrix3x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
            //this.RawToMap();
        }
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            // Step 3. Matrix
            //this.StartingMatrix = this.Matrix;
            //this.Matrix = this.StartingMatrix * this.Host;
            //this.Invert();
            //this.RawToMap();

            indicator.ChangeAll(this.Triangle, mode);
        }

        //public void SetWidth(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        //{
        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = indicator.CreateWidth(this.StartingTriangle, mode, value, keepRatio);

        //    // Step 2. Homography Matrix
        //    // Step 3. Matrix
        //    this.Find();

        //    // Step 4. Host
        //    //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
        //    //this.FindHomography();

        //    indicator.ChangeXYWH(this.Triangle, mode);
        //}
        //public void SetHeight(IIndicator indicator, BoxMode mode, float value, bool keepRatio)
        //{
        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = indicator.CreateHeight(this.StartingTriangle, mode, value, keepRatio);

        //    // Step 2. Homography Matrix
        //    // Step 3. Matrix
        //    this.Find();

        //    // Step 4. Host
        //    //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
        //    //this.FindHomography();

        //    indicator.ChangeXYWH(this.Triangle, mode);
        //}

        //public void SetRotation(IIndicator indicator, BoxMode mode, float rotationAngleInDegrees)
        //{
        //    // Step 4. Host
        //    this.Host = indicator.CreateRotation(rotationAngleInDegrees);

        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

        //    // Step 3. Matrix
        //    this.StartingMatrix = this.Matrix;
        //    this.Matrix = this.StartingMatrix * this.Host;
        //    //this.Invert();

        //    indicator.ChangeXYWHRS(this.Triangle, mode);
        //}
        //public void SetSkew(IIndicator indicator, BoxMode mode, float skewAngleInDegrees, float minimum = -85f, float maximum = 85f)
        //{
        //    // Step 1. Transformer
        //    this.StartingTriangle = this.Triangle;
        //    this.Triangle = indicator.CreateSkew(this.StartingTriangle, mode, skewAngleInDegrees, minimum, maximum);

        //    // Step 2. Homography Matrix
        //    // Step 3. Matrix
        //    this.Find();

        //    // Step 4. Host
        //    //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
        //    //this.FindHomography();

        //    indicator.ChangeXYWHRS(this.Triangle, mode);
        //}
        #endregion
    }
}