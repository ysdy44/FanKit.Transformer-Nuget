using System.Numerics;

namespace FanKit.Transformer.UI
{
    public readonly struct FlyoutMetrics
    {
        private readonly FlyoutPlacementPriorityIndex Index;
        internal readonly FlyoutPlacementMode Mode;

        private readonly float tailX;
        private readonly float contentX;
        private readonly float bodyX;

        private readonly float tailY;
        private readonly float contentY;
        private readonly float bodyY;

        private readonly float tailZ;
        private readonly float contentZ;
        private readonly float bodyZ;

        public readonly Vector2 Arrow;
        public readonly Vector2 TailLeft;
        public readonly Vector2 TailRight;

        public readonly Rectangle Content;
        public readonly Rectangle Body;
        public readonly Bounds Inner; // Body of Inner without CornerRadius

        public bool IsFixed => this.Mode is FlyoutPlacementMode.Fixed;

        public FlyoutMetrics(FlyoutLocation location)
        {
            this.Index = FlyoutPlacementPriorityIndex.Priority1;
            this.Mode = FlyoutPlacementMode.Fixed;

            this.Arrow = default;
            this.TailLeft = default;
            this.TailRight = default;

            this.Content = default;
            this.Body = default;
            this.Inner = default;

            // 1. Priority -> Mode
            Unknown:
            switch (this.Index)
            {
                case FlyoutPlacementPriorityIndex.Priority1:
                    this.Index = FlyoutPlacementPriorityIndex.Priority2;

                    switch (location.Priority)
                    {
                        case FlyoutPlacementPriority.Fixed:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                        case FlyoutPlacementPriority.LeftTopRightBottom:
                        case FlyoutPlacementPriority.LeftTopBottomRight:
                        case FlyoutPlacementPriority.LeftRightTopBottom:
                        case FlyoutPlacementPriority.LeftRightBottomTop:
                        case FlyoutPlacementPriority.LeftBottomTopRight:
                        case FlyoutPlacementPriority.LeftBottomRightTop:
                            this.Mode = FlyoutPlacementMode.Left;
                            break;
                        case FlyoutPlacementPriority.TopLeftRightBottom:
                        case FlyoutPlacementPriority.TopLeftBottomRight:
                        case FlyoutPlacementPriority.TopRightLeftBottom:
                        case FlyoutPlacementPriority.TopRightBottomLeft:
                        case FlyoutPlacementPriority.TopBottomLeftRight:
                        case FlyoutPlacementPriority.TopBottomRightLeft:
                            this.Mode = FlyoutPlacementMode.Top;
                            break;
                        case FlyoutPlacementPriority.RightLeftTopBottom:
                        case FlyoutPlacementPriority.RightLeftBottomTop:
                        case FlyoutPlacementPriority.RightTopLeftBottom:
                        case FlyoutPlacementPriority.RightBottomLeftTop:
                        case FlyoutPlacementPriority.RightTopBottomLeft:
                        case FlyoutPlacementPriority.RightBottomTopLeft:
                            this.Mode = FlyoutPlacementMode.Right;
                            break;
                        case FlyoutPlacementPriority.BottomLeftTopRight:
                        case FlyoutPlacementPriority.BottomLeftRightTop:
                        case FlyoutPlacementPriority.BottomTopLeftRight:
                        case FlyoutPlacementPriority.BottomRightLeftTop:
                        case FlyoutPlacementPriority.BottomTopRightLeft:
                        case FlyoutPlacementPriority.BottomRightTopLeft:
                            this.Mode = FlyoutPlacementMode.Bottom;
                            break;
                        default:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                    }
                    break;
                case FlyoutPlacementPriorityIndex.Priority2:
                    this.Index = FlyoutPlacementPriorityIndex.Priority3;

                    switch (location.Priority)
                    {
                        case FlyoutPlacementPriority.Fixed:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                        case FlyoutPlacementPriority.TopLeftRightBottom:
                        case FlyoutPlacementPriority.TopLeftBottomRight:
                        case FlyoutPlacementPriority.RightLeftTopBottom:
                        case FlyoutPlacementPriority.RightLeftBottomTop:
                        case FlyoutPlacementPriority.BottomLeftTopRight:
                        case FlyoutPlacementPriority.BottomLeftRightTop:
                            this.Mode = FlyoutPlacementMode.Left;
                            break;
                        case FlyoutPlacementPriority.LeftTopRightBottom:
                        case FlyoutPlacementPriority.LeftTopBottomRight:
                        case FlyoutPlacementPriority.RightTopLeftBottom:
                        case FlyoutPlacementPriority.RightTopBottomLeft:
                        case FlyoutPlacementPriority.BottomTopLeftRight:
                        case FlyoutPlacementPriority.BottomTopRightLeft:
                            this.Mode = FlyoutPlacementMode.Top;
                            break;
                        case FlyoutPlacementPriority.LeftRightTopBottom:
                        case FlyoutPlacementPriority.LeftRightBottomTop:
                        case FlyoutPlacementPriority.TopRightLeftBottom:
                        case FlyoutPlacementPriority.TopRightBottomLeft:
                        case FlyoutPlacementPriority.BottomRightLeftTop:
                        case FlyoutPlacementPriority.BottomRightTopLeft:
                            this.Mode = FlyoutPlacementMode.Right;
                            break;
                        case FlyoutPlacementPriority.LeftBottomTopRight:
                        case FlyoutPlacementPriority.LeftBottomRightTop:
                        case FlyoutPlacementPriority.TopBottomLeftRight:
                        case FlyoutPlacementPriority.TopBottomRightLeft:
                        case FlyoutPlacementPriority.RightBottomLeftTop:
                        case FlyoutPlacementPriority.RightBottomTopLeft:
                            this.Mode = FlyoutPlacementMode.Bottom;
                            break;
                        default:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                    }
                    break;
                case FlyoutPlacementPriorityIndex.Priority3:
                    this.Index = FlyoutPlacementPriorityIndex.Priority4;

                    switch (location.Priority)
                    {
                        case FlyoutPlacementPriority.Fixed:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                        case FlyoutPlacementPriority.TopRightLeftBottom:
                        case FlyoutPlacementPriority.TopBottomLeftRight:
                        case FlyoutPlacementPriority.RightTopLeftBottom:
                        case FlyoutPlacementPriority.RightBottomLeftTop:
                        case FlyoutPlacementPriority.BottomTopLeftRight:
                        case FlyoutPlacementPriority.BottomRightLeftTop:
                            this.Mode = FlyoutPlacementMode.Left;
                            break;
                        case FlyoutPlacementPriority.LeftRightTopBottom:
                        case FlyoutPlacementPriority.LeftBottomTopRight:
                        case FlyoutPlacementPriority.RightLeftTopBottom:
                        case FlyoutPlacementPriority.RightBottomTopLeft:
                        case FlyoutPlacementPriority.BottomLeftTopRight:
                        case FlyoutPlacementPriority.BottomRightTopLeft:
                            this.Mode = FlyoutPlacementMode.Top;
                            break;
                        case FlyoutPlacementPriority.LeftTopRightBottom:
                        case FlyoutPlacementPriority.LeftBottomRightTop:
                        case FlyoutPlacementPriority.TopLeftRightBottom:
                        case FlyoutPlacementPriority.TopBottomRightLeft:
                        case FlyoutPlacementPriority.BottomLeftRightTop:
                        case FlyoutPlacementPriority.BottomTopRightLeft:
                            this.Mode = FlyoutPlacementMode.Right;
                            break;
                        case FlyoutPlacementPriority.LeftTopBottomRight:
                        case FlyoutPlacementPriority.LeftRightBottomTop:
                        case FlyoutPlacementPriority.TopLeftBottomRight:
                        case FlyoutPlacementPriority.TopRightBottomLeft:
                        case FlyoutPlacementPriority.RightLeftBottomTop:
                        case FlyoutPlacementPriority.RightTopBottomLeft:
                            this.Mode = FlyoutPlacementMode.Bottom;
                            break;
                        default:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                    }
                    break;
                case FlyoutPlacementPriorityIndex.Priority4:
                    this.Index = FlyoutPlacementPriorityIndex.Priority5;

                    switch (location.Priority)
                    {
                        case FlyoutPlacementPriority.Fixed:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                        case FlyoutPlacementPriority.TopRightBottomLeft:
                        case FlyoutPlacementPriority.TopBottomRightLeft:
                        case FlyoutPlacementPriority.RightTopBottomLeft:
                        case FlyoutPlacementPriority.RightBottomTopLeft:
                        case FlyoutPlacementPriority.BottomTopRightLeft:
                        case FlyoutPlacementPriority.BottomRightTopLeft:
                            this.Mode = FlyoutPlacementMode.Left;
                            break;
                        case FlyoutPlacementPriority.LeftRightBottomTop:
                        case FlyoutPlacementPriority.LeftBottomRightTop:
                        case FlyoutPlacementPriority.RightLeftBottomTop:
                        case FlyoutPlacementPriority.RightBottomLeftTop:
                        case FlyoutPlacementPriority.BottomLeftRightTop:
                        case FlyoutPlacementPriority.BottomRightLeftTop:
                            this.Mode = FlyoutPlacementMode.Top;
                            break;
                        case FlyoutPlacementPriority.LeftTopBottomRight:
                        case FlyoutPlacementPriority.LeftBottomTopRight:
                        case FlyoutPlacementPriority.TopLeftBottomRight:
                        case FlyoutPlacementPriority.TopBottomLeftRight:
                        case FlyoutPlacementPriority.BottomLeftTopRight:
                        case FlyoutPlacementPriority.BottomTopLeftRight:
                            this.Mode = FlyoutPlacementMode.Right;
                            break;
                        case FlyoutPlacementPriority.LeftTopRightBottom:
                        case FlyoutPlacementPriority.LeftRightTopBottom:
                        case FlyoutPlacementPriority.TopLeftRightBottom:
                        case FlyoutPlacementPriority.TopRightLeftBottom:
                        case FlyoutPlacementPriority.RightLeftTopBottom:
                        case FlyoutPlacementPriority.RightTopLeftBottom:
                            this.Mode = FlyoutPlacementMode.Bottom;
                            break;
                        default:
                            this.Mode = FlyoutPlacementMode.Fixed;
                            break;
                    }
                    break;
                case FlyoutPlacementPriorityIndex.Priority5:
                default:
                    this.Index = FlyoutPlacementPriorityIndex.Priority5;
                    this.Mode = FlyoutPlacementMode.Fixed;
                    break;
            }

            // 2. Mode -> Content and Body
            switch (this.Mode)
            {
                case FlyoutPlacementMode.Fixed:
                    this.tailX = 0;
                    this.contentX = 0;
                    this.bodyX = 0;

                    this.tailY = 0;
                    this.contentY = 0;
                    this.bodyY = 0;

                    this.tailZ = 0;
                    this.contentZ = 0;
                    this.bodyZ = 0;

                    this.Arrow = default;
                    this.TailLeft = default;
                    this.TailRight = default;

                    switch (location.FixedHorizontalAlignment)
                    {
                        case FlyoutLocation.Center:
                            this.contentX = location.Bounds.Left + location.Bounds.Width / 2 - location.ContentWidth / 2;
                            this.bodyX = this.contentX - location.ContentMargin.Left;
                            break;
                        case FlyoutLocation.Right:
                            this.contentX = location.Bounds.Right - location.BoundsPadding.Right - location.ContentMargin.Right - location.ContentWidth;
                            this.bodyX = this.contentX - location.ContentMargin.Left;
                            break;
                        default:
                            this.bodyX = location.Bounds.Left + location.BoundsPadding.Left;
                            this.contentX = this.bodyX + location.ContentMargin.Left;
                            break;
                    }

                    switch (location.FixedVerticalAlignment)
                    {
                        case FlyoutLocation.Center:
                            this.contentY = location.Bounds.Top + location.Bounds.Height / 2 - location.ContentHeight / 2;
                            this.bodyY = this.contentY - location.ContentMargin.Top;
                            break;
                        case FlyoutLocation.Bottom:
                            this.contentY = location.Bounds.Bottom - location.BoundsPadding.Bottom - location.ContentMargin.Bottom - location.ContentHeight;
                            this.bodyY = this.contentY - location.ContentMargin.Top;
                            break;
                        default:
                            this.bodyY = location.Bounds.Top + location.BoundsPadding.Top;
                            this.contentY = this.bodyY + location.ContentMargin.Top;
                            break;
                    }

                    switch (location.FixedHorizontalAlignment)
                    {
                        case FlyoutLocation.Stretch:
                            switch (location.FixedVerticalAlignment)
                            {
                                case FlyoutLocation.Stretch:
                                    this.Body = new Rectangle
                                    {
                                        X = this.bodyX,
                                        Y = this.bodyY,
                                        Width = location.Bounds.Width - location.BoundsPadding.Left - location.BoundsPadding.Right,
                                        Height = location.Bounds.Height - location.BoundsPadding.Top - location.BoundsPadding.Bottom,
                                    };
                                    this.Content = new Rectangle
                                    {
                                        X = this.contentX,
                                        Y = this.contentY,
                                        Width = this.Body.Width - location.ContentMargin.Left - location.ContentMargin.Right,
                                        Height = this.Body.Height - location.ContentMargin.Top - location.ContentMargin.Bottom,
                                    };
                                    break;
                                default:
                                    this.Body = new Rectangle
                                    {
                                        X = this.bodyX,
                                        Y = this.bodyY,
                                        Width = location.Bounds.Width - location.BoundsPadding.Left - location.BoundsPadding.Right,
                                        Height = location.ContentHeight + location.ContentMargin.Top + location.ContentMargin.Bottom,
                                    };
                                    this.Content = new Rectangle
                                    {
                                        X = this.contentX,
                                        Y = this.contentY,
                                        Width = this.Body.Width - location.ContentMargin.Left - location.ContentMargin.Right,
                                        Height = location.ContentHeight,
                                    };
                                    break;
                            }
                            break;
                        default:
                            switch (location.FixedVerticalAlignment)
                            {
                                case FlyoutLocation.Stretch:
                                    this.Body = new Rectangle
                                    {
                                        X = this.bodyX,
                                        Y = this.bodyY,
                                        Width = location.ContentWidth + location.ContentMargin.Left + location.ContentMargin.Right,
                                        Height = location.Bounds.Height - location.BoundsPadding.Top - location.BoundsPadding.Bottom,
                                    };
                                    this.Content = new Rectangle
                                    {
                                        X = this.contentX,
                                        Y = this.contentY,
                                        Width = location.ContentWidth,
                                        Height = this.Body.Height - location.ContentMargin.Top - location.ContentMargin.Bottom,
                                    };
                                    break;
                                default:
                                    this.Content = new Rectangle
                                    {
                                        X = this.contentX,
                                        Y = this.contentY,
                                        Width = location.ContentWidth,
                                        Height = location.ContentHeight,
                                    };
                                    this.Body = new Rectangle
                                    {
                                        X = this.bodyX,
                                        Y = this.bodyY,
                                        Width = location.ContentWidth + location.ContentMargin.Left + location.ContentMargin.Right,
                                        Height = location.ContentHeight + location.ContentMargin.Top + location.ContentMargin.Bottom,
                                    };
                                    break;
                            }
                            break;
                    }
                    break;
                default:
                    switch (this.Mode)
                    {
                        case FlyoutPlacementMode.Left:
                            this.tailX = location.Target.X - location.ArrowHeight;
                            this.contentX = 0;
                            this.bodyX = this.tailX - location.ContentMargin.Right - location.ContentWidth - location.ContentMargin.Left;

                            this.tailY = 0;
                            this.contentY = 0;
                            this.bodyY = 0;

                            this.tailZ = 0;
                            this.contentZ = 0;
                            this.bodyZ = 0;

                            if (this.bodyX < location.Bounds.X + location.BoundsPadding.Left)
                                goto Unknown;
                            else
                                break;
                        case FlyoutPlacementMode.Right:
                            this.tailX = location.Target.X + location.Target.Width;
                            this.contentX = 0;
                            this.bodyX = this.tailX + location.ArrowHeight;

                            this.tailY = 0;
                            this.contentY = 0;
                            this.bodyY = 0;

                            this.tailZ = 0;
                            this.contentZ = 0;
                            this.bodyZ = 0;

                            if (this.bodyX > location.Bounds.Width - location.BoundsPadding.Right - location.ContentMargin.Right - location.ContentWidth - location.ContentMargin.Left)
                                goto Unknown;
                            else
                                break;
                        case FlyoutPlacementMode.Top:
                            this.tailX = 0;
                            this.contentX = 0;
                            this.bodyX = 0;

                            this.tailY = location.Target.Y - location.ArrowHeight;
                            this.contentY = 0;
                            this.bodyY = this.tailY - location.ContentMargin.Bottom - location.ContentHeight - location.ContentMargin.Top;

                            this.tailZ = 0;
                            this.contentZ = 0;
                            this.bodyZ = 0;

                            if (this.bodyY < location.BoundsPadding.Top)
                                goto Unknown;
                            else
                                break;
                        case FlyoutPlacementMode.Bottom:
                            this.tailX = 0;
                            this.contentX = 0;
                            this.bodyX = 0;

                            this.tailY = location.Target.Y + location.Target.Height;
                            this.contentY = 0;
                            this.bodyY = this.tailY + location.ArrowHeight;

                            this.tailZ = 0;
                            this.contentZ = 0;
                            this.bodyZ = 0;

                            if (this.bodyY > location.Bounds.Height - location.BoundsPadding.Bottom - location.ContentMargin.Bottom - location.ContentHeight - location.ContentMargin.Top)
                                goto Unknown;
                            else
                                break;
                        default:
                            this.tailX = 0;
                            this.contentX = 0;
                            this.bodyX = 0;

                            this.tailY = 0;
                            this.contentY = 0;
                            this.bodyY = 0;

                            this.tailZ = 0;
                            this.contentZ = 0;
                            this.bodyZ = 0;

                            this.Arrow = default;
                            this.TailLeft = default;
                            this.TailRight = default;

                            this.Content = Rectangle.Empty;
                            this.Body = Rectangle.Empty;
                            break;
                    }

                    switch (this.Mode)
                    {
                        case FlyoutPlacementMode.Left:
                        case FlyoutPlacementMode.Right:
                            this.contentY = location.Target.Y + location.Target.Height / 2 - location.ContentHeight / 2;
                            this.bodyY = this.contentY - location.ContentMargin.Top;

                            this.bodyZ = location.Bounds.Y + location.BoundsPadding.Top;
                            this.contentZ = this.bodyZ + location.ContentMargin.Top;

                            if (this.bodyY < this.bodyZ)
                            {
                                this.contentY = this.contentZ;
                                this.bodyY = this.bodyZ;
                            }
                            else
                            {
                                this.contentZ = location.Bounds.Y + location.Bounds.Height - location.BoundsPadding.Bottom - location.ContentMargin.Bottom - location.ContentHeight;
                                this.bodyZ = this.contentZ - location.ContentMargin.Top;

                                if (this.bodyY > this.bodyZ)
                                {
                                    this.contentY = this.contentZ;
                                    this.bodyY = this.bodyZ;
                                }
                            }

                            this.Content = new Rectangle
                            {
                                X = this.bodyX + location.ContentMargin.Left,
                                Y = this.contentY,
                                Width = location.ContentWidth,
                                Height = location.ContentHeight
                            };
                            this.Body = new Rectangle
                            {
                                X = this.bodyX,
                                Y = this.bodyY,
                                Width = location.ContentWidth + location.ContentMargin.Left + location.ContentMargin.Right,
                                Height = location.ContentHeight + location.ContentMargin.Top + location.ContentMargin.Bottom,
                            };
                            break;
                        case FlyoutPlacementMode.Top:
                        case FlyoutPlacementMode.Bottom:
                            this.contentX = location.Target.X + location.Target.Width / 2 - location.ContentWidth / 2;
                            this.bodyX = this.contentX - location.ContentMargin.Left;

                            this.bodyZ = location.Bounds.X + location.BoundsPadding.Left;
                            this.contentZ = this.bodyZ + location.ContentMargin.Left;

                            if (this.bodyX < this.bodyZ)
                            {
                                this.contentX = this.contentZ;
                                this.bodyX = this.bodyZ;
                            }
                            else
                            {
                                this.contentZ = location.Bounds.X + location.Bounds.Width - location.BoundsPadding.Right - location.ContentMargin.Right - location.ContentWidth;
                                this.bodyZ = this.contentZ - location.ContentMargin.Left;

                                if (this.bodyX > this.bodyZ)
                                {
                                    this.contentX = this.contentZ;
                                    this.bodyX = this.bodyZ;
                                }
                            }

                            this.Content = new Rectangle
                            {
                                X = this.contentX,
                                Y = this.bodyY + location.ContentMargin.Top,
                                Width = location.ContentWidth,
                                Height = location.ContentHeight
                            };
                            this.Body = new Rectangle
                            {
                                X = this.bodyX,
                                Y = this.bodyY,
                                Width = location.ContentWidth + location.ContentMargin.Left + location.ContentMargin.Right,
                                Height = location.ContentHeight + location.ContentMargin.Top + location.ContentMargin.Bottom,
                            };
                            break;
                        default:
                            this.Body = Rectangle.Empty;
                            break;
                    }
                    break;
            }

            // 3. Body -> Inner
            this.Inner = new Bounds(new Rectangle(
                this.Body.X + location.CornerRadius,
                this.Body.Y + location.CornerRadius,
                this.Body.Width - location.CornerRadius - location.CornerRadius,
                this.Body.Height - location.CornerRadius - location.CornerRadius
                ));

            // 4. Inner -> Tail
            switch (this.Mode)
            {
                case FlyoutPlacementMode.Fixed:
                    break;
                default:
                    switch (this.Mode)
                    {
                        case FlyoutPlacementMode.Left:
                            this.tailY = location.Target.Y + location.Target.Height / 2;
                            this.Arrow = new Vector2
                            {
                                X = this.tailX + location.ArrowHeight,
                                Y = this.tailY,
                            };
                            break;
                        case FlyoutPlacementMode.Right:
                            this.tailY = location.Target.Y + location.Target.Height / 2;
                            this.Arrow = new Vector2
                            {
                                X = this.tailX,
                                Y = this.tailY,
                            };

                            this.tailX = this.tailX + location.ArrowHeight;
                            break;
                        case FlyoutPlacementMode.Top:
                            this.tailX = location.Target.X + location.Target.Width / 2;
                            this.Arrow = new Vector2
                            {
                                X = this.tailX,
                                Y = this.tailY + location.ArrowHeight,
                            };
                            break;
                        case FlyoutPlacementMode.Bottom:
                            this.tailX = location.Target.X + location.Target.Width / 2;
                            this.Arrow = new Vector2
                            {
                                X = this.tailX,
                                Y = this.tailY,
                            };

                            this.tailY = this.tailY + location.ArrowHeight;
                            break;
                        default:
                            break;
                    }

                    switch (this.Mode)
                    {
                        case FlyoutPlacementMode.Left:
                        case FlyoutPlacementMode.Right:
                            if (this.tailY - location.ArrowWidth / 2 <= this.Inner.Top)
                            {
                                this.TailLeft = new Vector2
                                {
                                    X = this.tailX,
                                    Y = this.Inner.Top,
                                };
                                this.TailRight = new Vector2
                                {
                                    X = this.tailX,
                                    Y = this.Inner.Top + location.ArrowWidth,
                                };
                            }
                            else if (this.tailY + location.ArrowWidth / 2 >= this.Inner.Bottom)
                            {
                                this.TailLeft = new Vector2
                                {
                                    X = this.tailX,
                                    Y = this.Inner.Bottom - location.ArrowWidth,
                                };
                                this.TailRight = new Vector2
                                {
                                    X = this.tailX,
                                    Y = this.Inner.Bottom,
                                };
                            }
                            else
                            {
                                this.TailLeft = new Vector2
                                {
                                    X = this.tailX,
                                    Y = this.tailY - location.ArrowWidth / 2,
                                };
                                this.TailRight = new Vector2
                                {
                                    X = this.tailX,
                                    Y = this.tailY + location.ArrowWidth / 2,
                                };
                            }
                            break;
                        case FlyoutPlacementMode.Top:
                        case FlyoutPlacementMode.Bottom:
                            if (this.tailX - location.ArrowWidth / 2 <= this.Inner.Left)
                            {
                                this.TailLeft = new Vector2
                                {
                                    X = this.Inner.Left,
                                    Y = this.tailY,
                                };
                                this.TailRight = new Vector2
                                {
                                    X = this.Inner.Left + location.ArrowWidth,
                                    Y = this.tailY,
                                };
                            }
                            else if (this.tailX + location.ArrowWidth / 2 >= this.Inner.Right)
                            {
                                this.TailLeft = new Vector2
                                {
                                    X = this.Inner.Right - location.ArrowWidth,
                                    Y = this.tailY,
                                };
                                this.TailRight = new Vector2
                                {
                                    X = this.Inner.Right,
                                    Y = this.tailY,
                                };
                            }
                            else
                            {
                                this.TailLeft = new Vector2
                                {
                                    X = this.tailX - location.ArrowWidth / 2,
                                    Y = this.tailY,
                                };
                                this.TailRight = new Vector2
                                {
                                    X = this.tailX + location.ArrowWidth / 2,
                                    Y = this.tailY,
                                };
                            }
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }
    }
}