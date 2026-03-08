using FanKit.Transformer.Mathematics;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using FanKit.Transformer.Cache;
using FanKit.Transformer.Indicators;

namespace FanKit.Transformer.Curves
{
    public partial class Path2
    {
        // Step 0. Initialize
        //public int Count;
        public Bounds SourceBounds;
        public Rectangle SourceRect;
        public RectMatrix SourceNormalize;

        // Step 1. Transformer
        TransformedBounds TransformedBounds;
        public Triangle StartingTriangle;
        public Triangle Triangle;

        // Step 2. Homography Matrix
        Matrix3x2 DestNorm;

        // Step 3. Matrix
        public Matrix3x2 StartingMatrix;
        public Matrix3x2 Matrix;
        public Matrix3x2 InverseMatrix;

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

        void Invert()
        {
            Matrix3x2.Invert(this.Matrix, out this.InverseMatrix);
        }

        void Find()
        {
            this.DestNorm = this.Triangle.Normalize();
            this.Matrix = this.SourceNormalize.Affine(this.DestNorm);
            this.Invert();
        }

        /*
        void FindHomography()
        {
            this.HostDestNorm = this.Triangle.Normalize();
            this.Host = this.HostSourceNorm * this.HostDestNorm;
        }
         */

        #region Triangles.Initialize
        private void BeginExtend()
        {
            this.SourceBounds = Bounds.Infinity;
        }
        private void Extend(Bounds source)
        {
            this.SourceBounds = Bounds.Union(this.SourceBounds, source);
        }
        private void EndExtend()
        {
            // Step 0. Initialize
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            // Step 4. Host
            this.Host = Matrix3x2.Identity;

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }

        /*
        public void Reset()
        {
            // Step 0. Initialize
            this.Count = 0;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }
         */

        public void Reset(Triangle triangle)
        {
            // Step 0. Initialize
            //this.Count = 1;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle = triangle;

            // Step 2. Homography Matrix
            // Step 3. Matrix
            this.Find();
            this.RawToMap();

            // Step 4. Host
            this.Host = Matrix3x2.Identity;
        }

        /*
        public void Reset(IEnumerable<IGetTriangle> items)
        {
            this.Count = 0;

            foreach (IGetTriangle item in items)
            {
                if (item.HasTriangle())
                {
                    if (this.Count == 0)
                        this.StartingTriangle = this.Triangle = item.GetTriangle();

                    this.Count++;
                }
            }

            switch (this.Count)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    // Step 0. Initialize
                    this.SourceBounds = new Bounds(items);

                    // Step 1. Transformer
                    this.StartingTriangle = this.Triangle = new Triangle(this.SourceBounds);

                    // Step 2. Homography Matrix
                    // Step 3. Matrix
                    this.Find();

                    // Step 4. Host
                    this.Host = Matrix3x2.Identity;
                    break;
            }
        }
         */
        #endregion

        #region Triangles.SelectedItems
        public List<Figure2> Data;

        public PathSetting Setting { get; } = new PathSetting();

        public Path2(List<Figure2> items)
        {
            this.Data = items;
            this.Matrix = Matrix3x2.Identity;
            this.InverseMatrix = Matrix3x2.Identity;

            this.BeginExtend();
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    figure.Data[i] = new Segment2
                    {
                        Map = item.Raw,
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        //Map = item.Map,
                    };
                }

                figure.Extend();
                this.Extend(figure.SourceBounds);
            }

            // Step 0. Initialize
            this.SourceRect = new Rectangle(this.SourceBounds);
            this.SourceNormalize = new RectMatrix(this.SourceRect);

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }
        public Path2(List<Figure2> items, Matrix3x2 matrix)
        {
            this.Data = items;
            this.Matrix = matrix;
            this.Invert();

            this.BeginExtend();
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    figure.Data[i] = new Segment2
                    {
                        Map = Node.Transform(item.Raw, this.Matrix),
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        //Map = item.Map,
                    };
                }

                figure.Extend();
                this.Extend(figure.SourceBounds);
            }
            this.EndExtend();
        }

        public void Complete(Figure2 figure = null)
        {
            this.BeginExtend();
            if (figure == null)
            {
                foreach (Figure2 item in this.Data)
                {
                    item.Extend();
                    this.Extend(item.SourceBounds);
                }
            }
            else
            {
                figure.Extend();

                foreach (Figure2 item in this.Data)
                {
                    this.Extend(item.SourceBounds);
                }
            }
            this.EndExtend();
        }

        public void Select(int index1, int index2)
        {
            for (int j = 0; j < this.Data.Count; j++)
            {
                Figure2 figure = this.Data[j];

                if (index1 == j)
                {
                    for (int i = 0; i < figure.Data.Count; i++)
                    {
                        Segment2 item = figure.Data[i];

                        if (item.IsChecked)
                        {
                            if (index2 != i)
                            {
                                figure.Data[i] = new Segment2
                                {
                                    IsChecked = false,
                                    // C# 9.0 : var a = item with { ... }

                                    //IsChecked = item.IsChecked,
                                    IsSmooth = item.IsSmooth,
                                    Starting = item.Starting,
                                    Raw = item.Raw,
                                    Map = item.Map,
                                };
                            }
                        }
                        else
                        {
                            if (index2 == i)
                            {
                                figure.Data[i] = new Segment2
                                {
                                    IsChecked = true,
                                    // C# 9.0 : var a = item with { ... }

                                    //IsChecked = item.IsChecked,
                                    IsSmooth = item.IsSmooth,
                                    Starting = item.Starting,
                                    Raw = item.Raw,
                                    Map = item.Map,
                                };
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < figure.Data.Count; i++)
                    {
                        Segment2 item = figure.Data[i];
                        if (item.IsChecked)
                        {
                            figure.Data[i] = new Segment2
                            {
                                IsChecked = false,
                                // C# 9.0 : var a = item with { ... }

                                //IsChecked = item.IsChecked,
                                IsSmooth = item.IsSmooth,
                                Starting = item.Starting,
                                Raw = item.Raw,
                                Map = item.Map,
                            };
                        }
                    }
                }
            }
        }

        public void DeselectAll()
        {
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];

                    if (item.IsChecked)
                    {
                        figure.Data[i] = new Segment2
                        {
                            IsChecked = false,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            Raw = item.Raw,
                            Map = item.Map,
                        };
                    }
                }
            }
        }

        public void RectChooseItems(Bounds bounds)
        {
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];

                    if (bounds.ContainsPoint(item.Map.Point))
                    {
                        if (item.IsChecked is false)
                        {
                            figure.Data[i] = new Segment2
                            {
                                IsChecked = true,
                                // C# 9.0 : var a = item with { ... }

                                //IsChecked = item.IsChecked,
                                IsSmooth = item.IsSmooth,
                                Starting = item.Starting,
                                Raw = item.Raw,
                                Map = item.Map,
                            };
                        }
                    }
                    else
                    {
                        if (item.IsChecked)
                        {
                            figure.Data[i] = new Segment2
                            {
                                IsChecked = false,
                                // C# 9.0 : var a = item with { ... }

                                //IsChecked = item.IsChecked,
                                IsSmooth = item.IsSmooth,
                                Starting = item.Starting,
                                Raw = item.Raw,
                                Map = item.Map,
                            };
                        }
                    }
                }
            }
        }

        public void RawToMap()
        {
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    figure.Data[i] = new Segment2
                    {
                        Map = Node.Transform(item.Raw, this.Matrix),
                        // C# 9.0 : var a = item with { ... }

                        IsChecked = item.IsChecked,
                        IsSmooth = item.IsSmooth,
                        Starting = item.Starting,
                        Raw = item.Raw,
                        //Map = item.Map,
                    };
                }
            }

            // Step 1. Transformer
            this.TransformedBounds = new TransformedBounds(this.SourceBounds, this.Matrix);
            this.StartingTriangle = this.Triangle = this.TransformedBounds.ToTriangle();
        }
        #endregion

        #region Triangles.SelectedItems.Set
        public void SetTranslationSelectedItems(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate.X, translate.Y, this.InverseMatrix));

            this.TranslateRaw();
        }
        public void SetTranslationSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate.X, translate.Y, this.InverseMatrix));

            this.TranslateRaw();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void SetTranslationXSelectedItems(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateRaw();
        }
        public void SetTranslationXSelectedItems(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateRaw();
            indicator.ChangeX(this.Triangle, mode);
        }

        public void SetTranslationYSelectedItems(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateRaw();
        }
        public void SetTranslationYSelectedItems(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateRaw();
            indicator.ChangeY(this.Triangle, mode);
        }

        public void SetTransformSelectedItems(Matrix3x2 matrix)
        {
            this.Host = matrix * this.InverseMatrix;

            this.TransformMap();
        }
        public void SetTransformSelectedItems(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            this.Host = matrix * this.InverseMatrix;

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
        //    this.Invert();

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
            this.BeginExtend();
            foreach (Figure2 figure in this.Data)
            {
                bool has = false;

                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        has = true;

                        Node p = Node.Translate(item.Raw, this.HostTranslateX, this.HostTranslateY);
                        figure.Data[i] = new Segment2
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Node.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                }

                if (has) figure.Extend();
                this.Extend(figure.SourceBounds);
            }
            this.EndExtend();
        }

        private void TransformMap()
        {
            this.BeginExtend();
            foreach (Figure2 figure in this.Data)
            {
                bool has = false;

                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        has = true;

                        Node p = Node.Transform(item.Map, this.HostMatrix);
                        figure.Data[i] = new Segment2
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Node.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                }

                if (has) figure.Extend();
                this.Extend(figure.SourceBounds);
            }
            this.EndExtend();
        }
        #endregion

        #region Triangles.SelectedItems.Set.Index
        public void SetTranslation(float translateX, float translateY, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));
            SI(point, index);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, float translateX, float translateY, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));
            SI(point, index);
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void SetTranslation(Vector2 translate, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));
            SI(point, index);
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate, Vector2 point, int index)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate, this.InverseMatrix));
            SI(point, index);
            indicator.ChangeXY(this.Triangle, mode);
        }

        private void SI(Vector2 point, int index)
        {
            this.BeginExtend();
            foreach (Figure2 figure in this.Data)
            {
                Segment2 item = figure.Data[index];
                Node node = item.Map.MovePoint(point);

                figure.Data[index] = new Segment2
                {
                    IsChecked = true,
                    Map = node,
                    Raw = Node.Transform(node, this.InverseMatrix),
                    // C# 9.0 : var a = item with { ... }

                    //IsChecked = item.IsChecked,
                    IsSmooth = item.IsSmooth,
                    Starting = item.Starting,
                    //Raw = item.Raw,
                    //Map = item.Map,
                };

                figure.Extend();
                this.Extend(figure.SourceBounds);
            }
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
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(point.X - startingPoint.X, point.Y - startingPoint.Y, this.InverseMatrix));
            this.TranslateStarting();
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(point.X - startingPoint.X, point.Y - startingPoint.Y, this.InverseMatrix));
            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateSelectedItems(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));

            this.TranslateStarting();
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translateX, translateY, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateSelectedItems(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate.X, translate.Y, this.InverseMatrix));

            this.TranslateStarting();
        }
        public void TranslateSelectedItems(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormal(translate.X, translate.Y, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateXSelectedItems(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateStarting();
        }
        public void TranslateXSelectedItems(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalX(translateX, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateYSelectedItems(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateStarting();
        }
        public void TranslateYSelectedItems(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(Math.TransformNormalY(translateY, this.InverseMatrix));

            this.TranslateStarting();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TransformSelectedItems(Matrix3x2 matrix)
        {
            this.Host = matrix * this.InverseMatrix;

            this.TransformStarting();
        }

        private void CacheRaw()
        {
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        figure.Data[i] = new Segment2
                        {
                            IsChecked = true,
                            Starting = item.Raw,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            //Starting = item.Starting,
                            Raw = item.Raw,
                            Map = item.Map,
                        };
                    }
                }
            }
        }

        private void CacheMap()
        {
            foreach (Figure2 figure in this.Data)
            {
                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        figure.Data[i] = new Segment2
                        {
                            IsChecked = true,
                            Starting = item.Map,
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            //Starting = item.Starting,
                            Raw = item.Raw,
                            Map = item.Map,
                        };
                    }
                }
            }
        }

        private void TranslateStarting()
        {
            this.BeginExtend();
            foreach (Figure2 figure in this.Data)
            {
                bool has = false;

                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        has = true;

                        Node p = Node.Translate(item.Starting, this.HostTranslateX, this.HostTranslateY);
                        figure.Data[i] = new Segment2
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Node.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                }

                if (has) figure.Extend();
                this.Extend(figure.SourceBounds);
            }
            this.EndExtend();
        }

        private void TransformStarting()
        {
            this.BeginExtend();
            foreach (Figure2 figure in this.Data)
            {
                bool has = false;

                for (int i = 0; i < figure.Data.Count; i++)
                {
                    Segment2 item = figure.Data[i];
                    if (item.IsChecked)
                    {
                        has = true;

                        Node p = Node.Transform(item.Starting, this.HostMatrix);
                        figure.Data[i] = new Segment2
                        {
                            IsChecked = true,
                            Raw = p,
                            Map = Node.Transform(p, this.Matrix),
                            // C# 9.0 : var a = item with { ... }

                            //IsChecked = item.IsChecked,
                            IsSmooth = item.IsSmooth,
                            Starting = item.Starting,
                            //Raw = item.Raw,
                            //Map = item.Map,
                        };
                    }
                }

                if (has) figure.Extend();
                this.Extend(figure.SourceBounds);
            }
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
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            this.Invert();
            this.RawToMap();
        }
        public void SetTranslation(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translate);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            this.Invert();
            this.RawToMap();

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
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            this.Invert();
            this.RawToMap();
        }
        public void SetTranslationX(IIndicator indicator, BoxMode mode, float translateX)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            this.Invert();
            this.RawToMap();

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
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            this.Invert();
            this.RawToMap();
        }
        public void SetTranslationY(IIndicator indicator, BoxMode mode, float translateY)
        {
            // Step 4. Host
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            this.Invert();
            this.RawToMap();

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
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            this.Invert();
            this.RawToMap();
        }
        public void SetTransform(IIndicator indicator, BoxMode mode, Matrix3x2 matrix)
        {
            // Step 4. Host
            this.Host = matrix;

            // Step 1. Transformer
            this.StartingTriangle = this.Triangle;
            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);

            // Step 3. Matrix
            this.StartingMatrix = this.Matrix;
            this.Matrix = this.StartingMatrix * this.Host;
            this.Invert();
            this.RawToMap();

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
        //    this.Invert();

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

        #region Triangles.Transform
        public void CacheTranslation()
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void CacheTransform()
        {
            this.StartingTriangle = this.Triangle;
            this.StartingMatrix = this.Matrix;

            //this.HostSourceNorm = this.StartingTriangle.ToInvertibleMatrix();
            this.Host = Matrix3x2.Identity;
        }

        public void Translate(Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            this.RawToMap();
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 startingPoint, Vector2 point)
        {
            this.Host = Matrix3x2.CreateTranslation(point.X - startingPoint.X, point.Y - startingPoint.Y);
            this.T();
            this.RawToMap();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void Translate(Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
            this.RawToMap();
        }
        public void Translate(IIndicator indicator, BoxMode mode, Vector2 translate)
        {
            this.Host = Matrix3x2.CreateTranslation(translate);
            this.T();
            this.RawToMap();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void Translate(float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            this.RawToMap();
        }
        public void Translate(IIndicator indicator, BoxMode mode, float translateX, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, translateY);
            this.T();
            this.RawToMap();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateX(float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            this.RawToMap();
        }
        public void TranslateX(IIndicator indicator, BoxMode mode, float translateX)
        {
            this.Host = Matrix3x2.CreateTranslation(translateX, 0f);
            this.TX();
            this.RawToMap();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void TranslateY(float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            this.RawToMap();
        }
        public void TranslateY(IIndicator indicator, BoxMode mode, float translateY)
        {
            this.Host = Matrix3x2.CreateTranslation(0f, translateY);
            this.TY();
            this.RawToMap();
            indicator.ChangeXY(this.Triangle, mode);
        }

        public void Transform(Matrix3x2 matrix)
        {
            this.Host = matrix;

            this.Triangle = Triangle.Transform(this.StartingTriangle, this.Host);
            this.Matrix = this.StartingMatrix * this.Host;
            this.Invert();
            this.RawToMap();
        }

        private void T()
        {
            this.Triangle = Triangle.Translate(this.StartingTriangle, this.Host.M31, this.Host.M32);
            this.Matrix = Math.Translate(this.StartingMatrix, this.Host.M31, this.Host.M32);
            this.Invert();
        }
        private void TX()
        {
            this.Triangle = Triangle.TranslateX(this.StartingTriangle, this.Host.M31);
            this.Matrix = Math.TranslateX(this.StartingMatrix, this.Host.M31);
            this.Invert();
        }
        private void TY()
        {
            this.Triangle = Triangle.TranslateY(this.StartingTriangle, this.Host.M32);
            this.Matrix = Math.TranslateY(this.StartingMatrix, this.Host.M32);
            this.Invert();
        }
        #endregion
    }
}