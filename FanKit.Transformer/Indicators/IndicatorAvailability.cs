using System;

namespace FanKit.Transformer.Indicators
{
    [Flags]
    public enum IndicatorAvailability : short
    {
        None = 0,

        XHasValue = 1,
        YHasValue = 2,
        WidthHasValue = 4,
        HeightHasValue = 8,
        RotationHasValue = 16,
        SkewHasValue = 32,
        Unavailable = 47,

        XValue = 64,
        YValue = 128,
        WidthValue = 256,
        HeightValue = 512,
        RotationValue = 1024,
        SkewValue = 2048,
        Available = 3055,
    }

    partial class Indicator
    {
        // 1
        const IndicatorAvailability Empty = (IndicatorAvailability)63;
        const IndicatorAvailability Point = (IndicatorAvailability)255;
        const IndicatorAvailability RowLine = (IndicatorAvailability)1535;
        const IndicatorAvailability ColumnLine = (IndicatorAvailability)1791;
        const IndicatorAvailability Panel = (IndicatorAvailability)4095;

        // 2
        const IndicatorAvailability Empty2Empty = 0;
        const IndicatorAvailability Empty2Point = (IndicatorAvailability)195;
        const IndicatorAvailability Empty2RowLine = (IndicatorAvailability)1495;
        const IndicatorAvailability Empty2ColumnLine = (IndicatorAvailability)1755;
        const IndicatorAvailability Empty2Panel = (IndicatorAvailability)4095;

        const IndicatorAvailability Point2Empty = (IndicatorAvailability)3;
        const IndicatorAvailability Point2Point = 0;
        const IndicatorAvailability Point2RowLine = (IndicatorAvailability)1300;
        const IndicatorAvailability Point2ColumnLine = (IndicatorAvailability)1560;
        const IndicatorAvailability Point2Panel = (IndicatorAvailability)3900;

        const IndicatorAvailability RowLine2Empty = (IndicatorAvailability)23;
        const IndicatorAvailability RowLine2Point = (IndicatorAvailability)20;
        const IndicatorAvailability RowLine2RowLine = 0;
        const IndicatorAvailability RowLine2ColumnLine = (IndicatorAvailability)524;
        const IndicatorAvailability RowLine2Panel = (IndicatorAvailability)2600;

        const IndicatorAvailability ColumnLine2Empty = (IndicatorAvailability)27;
        const IndicatorAvailability ColumnLine2Point = (IndicatorAvailability)24;
        const IndicatorAvailability ColumnLine2RowLine = (IndicatorAvailability)268;
        const IndicatorAvailability ColumnLine2ColumnLine = 0;
        const IndicatorAvailability ColumnLine2Panel = (IndicatorAvailability)2340;

        const IndicatorAvailability Panel2Empty = (IndicatorAvailability)63;
        const IndicatorAvailability Panel2Point = (IndicatorAvailability)60;
        const IndicatorAvailability Panel2RowLine = (IndicatorAvailability)40;
        const IndicatorAvailability Panel2ColumnLine = (IndicatorAvailability)36;
        const IndicatorAvailability Panel2Panel = 0;

        public static IndicatorAvailability ToAvailability(IndicatorSizeType sizeType)
        {
            switch (sizeType)
            {
                case IndicatorSizeType.Empty: return Empty;
                case IndicatorSizeType.Point: return Point;
                case IndicatorSizeType.RowLine: return RowLine;
                case IndicatorSizeType.ColumnLine: return ColumnLine;
                case IndicatorSizeType.Panel: return Panel;
                default: return Empty;
            }
        }

        public static IndicatorAvailability ToAvailabilityChange(IndicatorSizeType oldSizeType, IndicatorSizeType newSizeType)
        {
            switch (oldSizeType)
            {
                case IndicatorSizeType.Empty:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Empty2Empty;
                        case IndicatorSizeType.Point: return Empty2Point;
                        case IndicatorSizeType.RowLine: return Empty2RowLine;
                        case IndicatorSizeType.ColumnLine: return Empty2ColumnLine;
                        case IndicatorSizeType.Panel: return Empty2Panel;
                        default: return Empty;
                    }
                case IndicatorSizeType.Point:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Point2Empty;
                        case IndicatorSizeType.Point: return Point2Point;
                        case IndicatorSizeType.RowLine: return Point2RowLine;
                        case IndicatorSizeType.ColumnLine: return Point2ColumnLine;
                        case IndicatorSizeType.Panel: return Point2Panel;
                        default: return Empty;
                    }
                case IndicatorSizeType.RowLine:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return RowLine2Empty;
                        case IndicatorSizeType.Point: return RowLine2Point;
                        case IndicatorSizeType.RowLine: return RowLine2RowLine;
                        case IndicatorSizeType.ColumnLine: return RowLine2ColumnLine;
                        case IndicatorSizeType.Panel: return RowLine2Panel;
                        default: return Empty;
                    }
                case IndicatorSizeType.ColumnLine:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return ColumnLine2Empty;
                        case IndicatorSizeType.Point: return ColumnLine2Point;
                        case IndicatorSizeType.RowLine: return ColumnLine2RowLine;
                        case IndicatorSizeType.ColumnLine: return ColumnLine2ColumnLine;
                        case IndicatorSizeType.Panel: return ColumnLine2Panel;
                        default: return Empty;
                    }
                case IndicatorSizeType.Panel:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Panel2Empty;
                        case IndicatorSizeType.Point: return Panel2Point;
                        case IndicatorSizeType.RowLine: return Panel2RowLine;
                        case IndicatorSizeType.ColumnLine: return Panel2ColumnLine;
                        case IndicatorSizeType.Panel: return Panel2Panel;
                        default: return Empty;
                    }
                default: return Empty;
            }
        }

        public static ComposerSizeType ToComposerSizeType(IndicatorSizeType sizeType, IndicatorKind kind)
        {
            switch (sizeType)
            {
                case IndicatorSizeType.Empty:
                    switch (kind)
                    {
                        default: return ComposerSizeType.Empty;
                    }
                case IndicatorSizeType.Point:
                    switch (kind)
                    {
                        case IndicatorKind.X: return ComposerSizeType.PointX;
                        case IndicatorKind.Y: return ComposerSizeType.PointY;
                        default: return ComposerSizeType.Empty;
                    }
                case IndicatorSizeType.RowLine:
                    switch (kind)
                    {
                        case IndicatorKind.X: return ComposerSizeType.RowLineX;
                        case IndicatorKind.Y: return ComposerSizeType.RowLineY;
                        case IndicatorKind.Width: return ComposerSizeType.RowLineWidth;
                        case IndicatorKind.Rotation: return ComposerSizeType.RowLineRotation;
                        default: return ComposerSizeType.Empty;
                    }
                case IndicatorSizeType.ColumnLine:
                    switch (kind)
                    {
                        case IndicatorKind.X: return ComposerSizeType.ColumnLineX;
                        case IndicatorKind.Y: return ComposerSizeType.ColumnLineY;
                        case IndicatorKind.Height: return ComposerSizeType.ColumnLineHeight;
                        case IndicatorKind.Rotation: return ComposerSizeType.ColumnLineRotation;
                        default: return ComposerSizeType.Empty;
                    }
                case IndicatorSizeType.Panel:
                    switch (kind)
                    {
                        case IndicatorKind.X: return ComposerSizeType.PanelX;
                        case IndicatorKind.Y: return ComposerSizeType.PanelY;
                        case IndicatorKind.Width: return ComposerSizeType.PanelWidth;
                        case IndicatorKind.Height: return ComposerSizeType.PanelHeight;
                        case IndicatorKind.Rotation: return ComposerSizeType.PanelRotation;
                        case IndicatorKind.Skew: return ComposerSizeType.PanelSkew;
                        default: return ComposerSizeType.Empty;
                    }
                default: return ComposerSizeType.Empty;
            }
        }

        public static TransformerSizeType ToTransformerSizeType(int count, IndicatorKind kind)
        {
            switch (count)
            {
                case 0:
                    return TransformerSizeType.None;
                case 1:
                    switch (kind)
                    {
                        case IndicatorKind.X: return TransformerSizeType.X;
                        case IndicatorKind.Y: return TransformerSizeType.Y;
                        case IndicatorKind.Width: return TransformerSizeType.Width;
                        case IndicatorKind.Height: return TransformerSizeType.Height;
                        case IndicatorKind.Rotation: return TransformerSizeType.Rotation;
                        case IndicatorKind.Skew: return TransformerSizeType.Skew;
                        default: return TransformerSizeType.None;
                    }
                default:
                    switch (kind)
                    {
                        case IndicatorKind.X: return TransformerSizeType.MultiX;
                        case IndicatorKind.Y: return TransformerSizeType.MultiY;
                        case IndicatorKind.Width: return TransformerSizeType.MultiWidth;
                        case IndicatorKind.Height: return TransformerSizeType.MultiHeight;
                        case IndicatorKind.Rotation: return TransformerSizeType.MultiRotation;
                        case IndicatorKind.Skew: return TransformerSizeType.MultiSkew;
                        default: return TransformerSizeType.None;
                    }
            }
        }

        public static CropperSizeType ToCropperSizeType(int count, IndicatorKind kind)
        {
            switch (count)
            {
                case 0:
                    return CropperSizeType.None;
                case 1:
                    switch (kind)
                    {
                        case IndicatorKind.X: return CropperSizeType.X;
                        case IndicatorKind.Y: return CropperSizeType.Y;
                        case IndicatorKind.Width: return CropperSizeType.Width;
                        case IndicatorKind.Height: return CropperSizeType.Height;
                        //case IndicatorKind.Rotation: return CropperSizeType.Rotation;
                        //case IndicatorKind.Skew: return CropperSizeType.Skew;
                        default: return CropperSizeType.None;
                    }
                default:
                    switch (kind)
                    {
                        case IndicatorKind.X: return CropperSizeType.MultiX;
                        case IndicatorKind.Y: return CropperSizeType.MultiY;
                        case IndicatorKind.Width: return CropperSizeType.MultiWidth;
                        case IndicatorKind.Height: return CropperSizeType.MultiHeight;
                        //case IndicatorKind.Rotation: return CropperSizeType.MultiRotation;
                        //case IndicatorKind.Skew: return CropperSizeType.MultiSkew;
                        default: return CropperSizeType.None;
                    }
            }
        }
    }
}