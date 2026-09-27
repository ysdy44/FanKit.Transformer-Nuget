using FanKit.Transformer.Mathematics;
using FanKit.Transformer.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Plane = FanKit.Transformer.UI.Plane;

namespace FanKit.Transformer.TestApp
{
    public class PlaneItem
    {
        public SizeMatrix SourceNormalize;
        public CanvasBitmap Bitmap;

        public Quadrilateral ActualBox;
        public Vector3 ActualCenter;
        public Matrix4x4 ActualMatrix;
    }

    public class PlaneObject
    {
        public Plane Plane;
        public Vector3 Center;

        public Plane ActualPlane;
        public Vector3 ActualCenter;

        public readonly PlaneItem Core = new PlaneItem();

        public PlaneObject(Plane plane)
        {
            this.Init(plane);
        }

        public void Init(Plane plane)
        {
            this.Plane = plane;
            this.Center = plane.Center();
        }

        public void UpdateCamera(Camera camera, Vector2 translation)
        {
            this.ActualPlane = camera.Transform(this.Plane);
            this.ActualCenter = camera.Transform(this.Center);

            this.Core.ActualBox = this.ActualPlane.Rasterize(translation);
            this.Core.ActualMatrix = this.Core.SourceNormalize.Persp(this.Core.ActualBox);
        }
    }

    public class CubeObject
    {
        public readonly Cube RawCube;

        public Cube MapCube;
        public Vector3 MapCenter;

        public Cube ActualCube;
        public Vector3 ActualCenter;

        public Vector3 Translation;

        public readonly PlaneItem Back = new PlaneItem();
        public readonly PlaneItem Front = new PlaneItem();

        public readonly PlaneItem Left = new PlaneItem();
        public readonly PlaneItem Right = new PlaneItem();

        public readonly PlaneItem Top = new PlaneItem();
        public readonly PlaneItem Bottom = new PlaneItem();

        public bool IsPlayer { get; set; }

        public CubeObject(Vector3 translation, Cube cube)
        {
            this.RawCube = cube;
            this.Translation = translation;
            this.Init();
        }

        public void Init()
        {
            this.MapCube = this.RawCube.Add(this.Translation);
            this.MapCenter = this.MapCube.Center();
        }

        public void GoFront(float step) { this.Translation.Z += step; this.Init(); }
        public void GoBack(float step) { this.Translation.Z -= step; this.Init(); }

        public void GoLeft(float step) { this.Translation.X -= step; this.Init(); }
        public void GoRight(float step) { this.Translation.X += step; this.Init(); }

        public void GoTop(float step) { this.Translation.Y -= step; this.Init(); }
        public void GoBottom(float step) { this.Translation.Y += step; this.Init(); }

        public void UpdateCamera(Camera camera, Vector2 translation)
        {
            this.ActualCenter = camera.Transform(this.MapCenter);
            this.ActualCube = camera.Transform(this.MapCube);

            this.Back.ActualCenter = this.ActualCube.CenterOfBack();
            this.Front.ActualCenter = this.ActualCube.CenterOfFront();

            this.Left.ActualCenter = this.ActualCube.CenterOfLeft();
            this.Right.ActualCenter = this.ActualCube.CenterOfRight();

            this.Top.ActualCenter = this.ActualCube.CenterOfTop();
            this.Bottom.ActualCenter = this.ActualCube.CenterOfBottom();

            this.Back.ActualBox = this.ActualCube.RasterizeBack(translation);
            this.Front.ActualBox = this.ActualCube.RasterizeFront(translation);

            this.Left.ActualBox = this.ActualCube.RasterizeLeft(translation);
            this.Right.ActualBox = this.ActualCube.RasterizeRight(translation);

            this.Top.ActualBox = this.ActualCube.RasterizeTop(translation);
            this.Bottom.ActualBox = this.ActualCube.RasterizeBottom(translation);

            this.Back.ActualMatrix = this.Back.SourceNormalize.Persp(this.Back.ActualBox);
            this.Front.ActualMatrix = this.Front.SourceNormalize.Persp(this.Front.ActualBox);

            this.Left.ActualMatrix = this.Left.SourceNormalize.Persp(this.Left.ActualBox);
            this.Right.ActualMatrix = this.Right.SourceNormalize.Persp(this.Right.ActualBox);

            this.Top.ActualMatrix = this.Top.SourceNormalize.Persp(this.Top.ActualBox);
            this.Bottom.ActualMatrix = this.Bottom.SourceNormalize.Persp(this.Bottom.ActualBox);
        }
    }

    public sealed partial class CubePage : Page
    {
        const float S = 64f;
        const float Y = -S;
        const float X1 = -S;
        const float Z1 = -3 * S;
        const float X2 = 5 * S;
        const float Z2 = 3 * S;
        const float X3 = -5 * S;
        const float Z3 = 3 * S;

        const int GridSize = 64;
        const int GridCount = PaneRequestedSize / GridSize;
        const int PaneRequestedSize = 512;
        const int CubeRequestedSize = 128;

        Vector2 StartingPoint;
        Vector2 Point = Vector2.Zero;

        Vector3 StartingRadians;
        Vector3 Radians = new Vector3(-0.4675389f, -0.6213063f, 0f);

        Vector2 SceneCenter;
        Camera Camera = new Camera(new Vector3(-0.4675389f, -0.6213063f, 0f));

        readonly Color White = Color.FromArgb(255, 86, 176, 241);
        readonly Color Gray = Color.FromArgb(255, 63, 141, 196);
        readonly Color Black = Color.FromArgb(255, 72, 125, 167);

        readonly CanvasOperator1 CanvasOperator;
        readonly List<PlaneItem> ZBuffer = new List<PlaneItem>();
        readonly PlaneObject[] Panes = new PlaneObject[]
        {
            new PlaneObject(new Plane
            {
                LeftTop = new Vector3(-PaneRequestedSize, 0f, -PaneRequestedSize),
                RightTop = new Vector3(PaneRequestedSize, 0f, -PaneRequestedSize),
                RightBottom = new Vector3(PaneRequestedSize, 0f, PaneRequestedSize),
                LeftBottom = new Vector3(-PaneRequestedSize, 0f, PaneRequestedSize),
            }),
        };
        readonly CubeObject[] Cubes = new CubeObject[]
        {
            new CubeObject(Vector3.Zero, new Cube
            {
                BackLeftTop = new Vector3(X1 - S, Y - S, Z1 - S),
                BackRightTop = new Vector3(X1 + S, Y - S, Z1 - S),
                BackRightBottom = new Vector3(X1 + S, Y + S, Z1 - S),
                BackLeftBottom = new Vector3(X1 - S, Y + S, Z1 - S),

                FrontLeftTop = new Vector3(X1 - S, Y - S, Z1 + S),
                FrontRightTop = new Vector3(X1 + S, Y - S, Z1 + S),
                FrontRightBottom = new Vector3(X1 + S, Y + S, Z1 + S),
                FrontLeftBottom = new Vector3(X1 - S, Y + S, Z1 + S),
            })
            {
                IsPlayer = true,
            },
            new CubeObject(Vector3.Zero, new Cube
            {
                BackLeftTop = new Vector3(X2 - S,  Y - S, Z2 - S),
                BackRightTop = new Vector3(X2 + S, Y - S, Z2 - S),
                BackRightBottom = new Vector3(X2 + S, Y + S, Z2 - S),
                BackLeftBottom = new Vector3(X2 - S, Y + S, Z2 - S),

                FrontLeftTop = new Vector3(X2 - S,  Y - S, Z2 + S),
                FrontRightTop = new Vector3(X2 + S, Y - S, Z2 + S),
                FrontRightBottom = new Vector3(X2 + S, Y + S, Z2 + S),
                FrontLeftBottom = new Vector3(X2 - S, Y + S, Z2 + S),
            }),
            new CubeObject(Vector3.Zero, new Cube
            {
                BackLeftTop = new Vector3(X3 - S,  Y - S, Z3 - S),
                BackRightTop = new Vector3(X3 + S, Y - S, Z3 - S),
                BackRightBottom = new Vector3(X3 + S, Y + S, Z3 - S),
                BackLeftBottom = new Vector3(X3 - S, Y + S, Z3 - S),

                FrontLeftTop = new Vector3(X3 - S,  Y - S, Z3 + S),
                FrontRightTop = new Vector3(X3 + S, Y - S, Z3 + S),
                FrontRightBottom = new Vector3(X3 + S, Y + S, Z3 + S),
                FrontLeftBottom = new Vector3(X3 - S, Y + S, Z3 + S),
            }),
        };

        public CubePage()
        {
            this.InitializeComponent();
            this.CanvasOperator = new CanvasOperator1(this.CanvasControl);
            base.Unloaded += delegate
            {
                // Explicitly remove references to allow the Win2D controls to get garbage collected
                this.CanvasControl.RemoveFromVisualTree();
                this.CanvasControl = null;
            };

            this.CanvasControl.CreateResources += (s, args) =>
            {
                ICanvasResourceCreator resourceCreator = s;

                foreach (PlaneObject item in this.Panes)
                {
                    CanvasRenderTarget renderTarget = new CanvasRenderTarget(s, PaneRequestedSize, PaneRequestedSize, 96f);
                    using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
                    {
                        ds.Clear(Colors.Gray);
                        for (int y = 0; y < GridCount; y++)
                        {
                            for (int x = 0; x < GridCount; x++)
                            {
                                if ((x + y) % 2 == 0)
                                {
                                    ds.FillRectangle(x * GridSize, y * GridSize, GridSize, GridSize, Colors.LightGray);
                                }
                            }
                        }
                    }

                    SizeMatrix norm = new SizeMatrix(PaneRequestedSize, PaneRequestedSize);

                    item.Core.SourceNormalize = norm;
                    item.Core.Bitmap = renderTarget;
                }

                using (CanvasTextFormat textFormat = new CanvasTextFormat
                {
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    FontWeight = Windows.UI.Text.FontWeights.Medium,
                    FontSize = 32f,
                })
                {
                    SizeMatrix norm = new SizeMatrix(CubeRequestedSize, CubeRequestedSize);

                    for (int k = 0; k < this.Cubes.Length; k++)
                    {
                        CubeObject item = this.Cubes[k];

                        item.Back.SourceNormalize = norm;
                        item.Front.SourceNormalize = norm;

                        item.Left.SourceNormalize = norm;
                        item.Right.SourceNormalize = norm;

                        item.Top.SourceNormalize = norm;
                        item.Bottom.SourceNormalize = norm;

                        item.Back.Bitmap = CreateTexture(resourceCreator, $"Back", textFormat, Black);
                        item.Front.Bitmap = CreateTexture(resourceCreator, $"Front", textFormat, Black);

                        item.Left.Bitmap = CreateTexture(resourceCreator, $"Left", textFormat, Gray);
                        item.Right.Bitmap = CreateTexture(resourceCreator, $"Right", textFormat, Gray);

                        item.Top.Bitmap = CreateTexture(resourceCreator, $"Top", textFormat, White);
                        item.Bottom.Bitmap = CreateTexture(resourceCreator, $"Bottom", textFormat, White);
                    }
                }

                this.UpdateCamera();
                this.UpdateZBuffer();
            };
            this.CanvasControl.Draw += (s, e) =>
            {
                CanvasDrawingSession drawingSession = e.DrawingSession;

                foreach (PlaneObject item in this.Panes)
                {
                    drawingSession.DrawImage(new Transform3DEffect
                    {
                        TransformMatrix = item.Core.ActualMatrix,
                        Source = item.Core.Bitmap
                    });
                }

                foreach (CubeObject obj in this.Cubes)
                {
                    if (obj.ActualCenter.Z > 600f)
                    {
                        continue;
                    }

                    if (obj.IsPlayer)
                    {
                        DrawBounds(drawingSession, obj.Back.ActualBox);
                        DrawBounds(drawingSession, obj.Front.ActualBox);

                        DrawBounds(drawingSession, obj.Left.ActualBox);
                        DrawBounds(drawingSession, obj.Right.ActualBox);

                        DrawBounds(drawingSession, obj.Top.ActualBox);
                        DrawBounds(drawingSession, obj.Bottom.ActualBox);
                    }
                }

                foreach (PlaneItem item in this.ZBuffer)
                {
                    if (item.ActualCenter.Z > 600f)
                    {
                        continue;
                    }

                    e.DrawingSession.DrawImage(new Transform3DEffect
                    {
                        TransformMatrix = item.ActualMatrix,
                        Source = item.Bitmap
                    });
                }
            };
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                float viewportWidth = (float)e.NewSize.Width;
                float viewportHeight = (float)e.NewSize.Height;

                this.SceneCenter = new Vector2
                {
                    X = 0.5f * viewportWidth,
                    Y = 0.5f * viewportHeight,
                };

                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };

            this.CanvasOperator.Single_Start += (startingX, startingY, p) =>
            {
                this.StartingPoint = this.Point = new Vector2((float)startingX, (float)startingY);
                this.StartingRadians = this.Radians;
            };
            this.CanvasOperator.Single_Delta += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                float horizontalOffset = this.Point.X - this.StartingPoint.X;
                float verticalOffset = this.Point.Y - this.StartingPoint.Y;
                this.Radians = Camera.ScrollTo(this.StartingRadians, 200f, horizontalOffset, verticalOffset);

                this.Camera = new Camera(this.Radians);

                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Single_Complete += (x, y, p) =>
            {
                this.Point = new Vector2((float)x, (float)y);

                if (System.Math.Abs(this.StartingPoint.X - this.Point.X) < 4d)
                {
                    if (System.Math.Abs(this.StartingPoint.Y - this.Point.Y) < 4d)
                    {
                        bool hasChanged = false;

                        foreach (CubeObject item in this.Cubes)
                        {
                            if (hasChanged == false
                                && item.ActualCenter.Z <= 600f
                                && (item.Back.ActualBox.ContainsPoint(this.Point)
                                || item.Front.ActualBox.ContainsPoint(this.Point)
                                || item.Left.ActualBox.ContainsPoint(this.Point)
                                || item.Right.ActualBox.ContainsPoint(this.Point)
                                || item.Top.ActualBox.ContainsPoint(this.Point)
                                || item.Bottom.ActualBox.ContainsPoint(this.Point)))
                            {
                                hasChanged = true;
                                item.IsPlayer = true;
                            }
                            else
                            {
                                item.IsPlayer = false;
                            }
                        }

                        this.CanvasControl.Invalidate();
                    }
                }
            };

            this.LeftButton.Click += delegate
            {
                foreach (CubeObject item in this.Cubes)
                {
                    if (item.IsPlayer)
                    {
                        item.GoLeft(16f);
                        item.UpdateCamera(this.Camera, this.SceneCenter);
                        this.UpdateZBuffer();
                        this.CanvasControl.Invalidate();
                        break;
                    }
                }
            };
            this.BackButton.Click += delegate
            {
                foreach (CubeObject item in this.Cubes)
                {
                    if (item.IsPlayer)
                    {
                        item.GoBack(16f);
                        item.UpdateCamera(this.Camera, this.SceneCenter);
                        this.UpdateZBuffer();
                        this.CanvasControl.Invalidate();
                        break;
                    }
                }
            };
            this.RightButton.Click += delegate
            {
                foreach (CubeObject item in this.Cubes)
                {
                    if (item.IsPlayer)
                    {
                        item.GoRight(16f);
                        item.UpdateCamera(this.Camera, this.SceneCenter);
                        this.UpdateZBuffer();
                        this.CanvasControl.Invalidate();
                        break;
                    }
                }
            };
            this.FrontButton.Click += delegate
            {
                foreach (CubeObject item in this.Cubes)
                {
                    if (item.IsPlayer)
                    {
                        item.GoFront(16f);
                        item.UpdateCamera(this.Camera, this.SceneCenter);
                        this.UpdateZBuffer();
                        this.CanvasControl.Invalidate();
                        break;
                    }
                }
            };

            this.XMinusButton.Click += delegate
            {
                this.Radians.X -= Mathematics.Math.PI / 60;
                this.Camera = new Camera(this.Radians);
                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };
            this.XPlusButton.Click += delegate
            {
                this.Radians.X += Mathematics.Math.PI / 60;
                this.Camera = new Camera(this.Radians);
                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };

            this.YMinusButton.Click += delegate
            {
                this.Radians.Y -= Mathematics.Math.PI / 60;
                this.Camera = new Camera(this.Radians);
                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };
            this.YPlusButton.Click += delegate
            {
                this.Radians.Y += Mathematics.Math.PI / 60;
                this.Camera = new Camera(this.Radians);
                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };

            this.ZMinusButton.Click += delegate
            {
                this.Radians.Z -= Mathematics.Math.PI / 60;
                this.Camera = new Camera(this.Radians);
                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };
            this.ZPlusButton.Click += delegate
            {
                this.Radians.Z += Mathematics.Math.PI / 60;
                this.Camera = new Camera(this.Radians);
                this.UpdateCamera();
                this.UpdateZBuffer();
                this.CanvasControl.Invalidate();
            };
        }

        private static float OrderBy(PlaneItem item)
        {
            return item.ActualCenter.Z;
        }

        private static void DrawBounds(CanvasDrawingSession drawingSession, Quadrilateral box)
        {
            drawingSession.DrawLine(box.LeftTop, box.RightTop, Colors.OrangeRed, 6f);
            drawingSession.DrawLine(box.RightTop, box.RightBottom, Colors.OrangeRed, 6f);
            drawingSession.DrawLine(box.RightBottom, box.LeftBottom, Colors.OrangeRed, 6f);
            drawingSession.DrawLine(box.LeftBottom, box.LeftTop, Colors.OrangeRed, 6f);
        }

        private static CanvasBitmap CreateTexture(ICanvasResourceCreator resourceCreator, string textString, CanvasTextFormat textFormat, Color color)
        {
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(resourceCreator, CubeRequestedSize, CubeRequestedSize, 96f);

            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            using (CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, textString, textFormat, CubeRequestedSize, CubeRequestedSize))
            {
                drawingSession.Clear(color);
                drawingSession.DrawTextLayout(textLayout, 0f, 0f, Windows.UI.Colors.White);
            }

            return renderTarget;
        }

        private void UpdateCamera()
        {
            foreach (PlaneObject item in this.Panes)
            {
                item.UpdateCamera(this.Camera, this.SceneCenter);
            }

            foreach (CubeObject item in this.Cubes)
            {
                item.UpdateCamera(this.Camera, this.SceneCenter);
            }
        }

        private void UpdateZBuffer()
        {
            this.ZBuffer.Clear();
            foreach (PlaneItem item in this.GetItems().OrderBy(OrderBy))
            {
                this.ZBuffer.Add(item);
            }
        }

        private IEnumerable<PlaneItem> GetItems()
        {
            foreach (CubeObject obj in this.Cubes)
            {
                yield return obj.Back;
                yield return obj.Front;

                yield return obj.Left;
                yield return obj.Right;

                yield return obj.Top;
                yield return obj.Bottom;
            }
        }
    }
}