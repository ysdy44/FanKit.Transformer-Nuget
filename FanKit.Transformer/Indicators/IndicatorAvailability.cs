using System;
using static FanKit.Transformer.Indicators.IndicatorAvailability;

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
        Unavailable = XHasValue | YHasValue | WidthHasValue | HeightHasValue | RotationHasValue | SkewHasValue,

        XValue = 64,
        YValue = 128,
        WidthValue = 256,
        HeightValue = 512,
        RotationValue = 1024,
        SkewValue = 2048,
        Available = XHasValue | YHasValue | WidthHasValue | HeightHasValue | RotationHasValue | SkewHasValue | XValue | YValue | WidthValue | HeightValue | RotationValue | SkewValue,
    }

    partial class Indicator
    {
        // 1
        const IndicatorAvailability Empty = Unavailable;
        const IndicatorAvailability Point = Unavailable | XValue | YValue;
        const IndicatorAvailability RowLine = Unavailable | XValue | YValue | WidthValue | RotationValue;
        const IndicatorAvailability ColumnLine = Unavailable | XValue | YValue | HeightValue | RotationValue;
        const IndicatorAvailability Crop = Unavailable | XValue | YValue | WidthValue | HeightValue;
        const IndicatorAvailability Transform = Available;

        // 2
        const IndicatorAvailability Empty2Empty = 0;
        const IndicatorAvailability Empty2Point = XHasValue | YHasValue | XValue | YValue;
        const IndicatorAvailability Empty2RowLine = XHasValue | YHasValue | WidthHasValue | RotationHasValue | XValue | YValue | WidthValue | RotationValue;
        const IndicatorAvailability Empty2ColumnLine = XHasValue | YHasValue | HeightHasValue | RotationHasValue | XValue | YValue | HeightValue | RotationValue;
        const IndicatorAvailability Empty2Crop = XHasValue | YHasValue | WidthHasValue | HeightHasValue | XValue | YValue | WidthValue | HeightValue;
        const IndicatorAvailability Empty2Transform = Available;

        const IndicatorAvailability Point2Empty = XHasValue | YHasValue;
        const IndicatorAvailability Point2Point = 0;
        const IndicatorAvailability Point2RowLine = WidthHasValue | RotationHasValue | WidthValue | RotationValue;
        const IndicatorAvailability Point2ColumnLine = HeightHasValue | RotationHasValue | HeightValue | RotationValue;
        const IndicatorAvailability Point2Crop = WidthHasValue | HeightHasValue | WidthValue | HeightValue;
        const IndicatorAvailability Point2Transform = WidthHasValue | HeightHasValue | RotationHasValue | SkewHasValue | WidthValue | HeightValue | RotationValue | SkewValue;

        const IndicatorAvailability RowLine2Empty = XHasValue | YHasValue | WidthHasValue | RotationHasValue;
        const IndicatorAvailability RowLine2Point = WidthHasValue | RotationHasValue;
        const IndicatorAvailability RowLine2RowLine = 0;
        const IndicatorAvailability RowLine2ColumnLine = WidthHasValue | HeightHasValue | HeightValue;
        const IndicatorAvailability RowLine2Crop = HeightHasValue | RotationHasValue | HeightValue;
        const IndicatorAvailability RowLine2Transform = HeightHasValue | SkewHasValue | HeightValue | SkewValue;

        const IndicatorAvailability ColumnLine2Empty = XHasValue | YHasValue | HeightHasValue | RotationHasValue;
        const IndicatorAvailability ColumnLine2Point = HeightHasValue | RotationHasValue;
        const IndicatorAvailability ColumnLine2RowLine = WidthHasValue | HeightHasValue | WidthValue;
        const IndicatorAvailability ColumnLine2ColumnLine = 0;
        const IndicatorAvailability ColumnLine2Crop = WidthHasValue | RotationHasValue | WidthValue;
        const IndicatorAvailability ColumnLine2Transform = WidthHasValue | SkewHasValue | WidthValue | SkewValue;

        const IndicatorAvailability Crop2Empty = XHasValue | YHasValue | WidthHasValue | HeightHasValue;
        const IndicatorAvailability Crop2Point = WidthHasValue | HeightHasValue;
        const IndicatorAvailability Crop2RowLine = HeightHasValue | RotationHasValue | RotationValue;
        const IndicatorAvailability Crop2ColumnLine = WidthHasValue | RotationHasValue | RotationValue;
        const IndicatorAvailability Crop2Crop = 0;
        const IndicatorAvailability Crop2Transform = RotationHasValue | SkewHasValue | RotationValue | SkewValue;

        const IndicatorAvailability Transform2Empty = Unavailable;
        const IndicatorAvailability Transform2Point = WidthHasValue | HeightHasValue | RotationHasValue | SkewHasValue;
        const IndicatorAvailability Transform2RowLine = HeightHasValue | SkewHasValue;
        const IndicatorAvailability Transform2ColumnLine = WidthHasValue | SkewHasValue;
        const IndicatorAvailability Transform2Crop = RotationHasValue | SkewHasValue;
        const IndicatorAvailability Transform2Transform = 0;

        public static IndicatorAvailability ToAvailability(IndicatorSizeType sizeType)
        {
            switch (sizeType)
            {
                case IndicatorSizeType.Empty: return Empty;
                case IndicatorSizeType.Point: return Point;
                case IndicatorSizeType.RowLine: return RowLine;
                case IndicatorSizeType.ColumnLine: return ColumnLine;
                case IndicatorSizeType.Crop: return Crop;
                case IndicatorSizeType.Transform: return Transform;
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
                        case IndicatorSizeType.Crop: return Empty2Crop;
                        case IndicatorSizeType.Transform: return Empty2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.Point:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Point2Empty;
                        case IndicatorSizeType.Point: return Point2Point;
                        case IndicatorSizeType.RowLine: return Point2RowLine;
                        case IndicatorSizeType.ColumnLine: return Point2ColumnLine;
                        case IndicatorSizeType.Crop: return Point2Crop;
                        case IndicatorSizeType.Transform: return Point2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.RowLine:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return RowLine2Empty;
                        case IndicatorSizeType.Point: return RowLine2Point;
                        case IndicatorSizeType.RowLine: return RowLine2RowLine;
                        case IndicatorSizeType.ColumnLine: return RowLine2ColumnLine;
                        case IndicatorSizeType.Crop: return RowLine2Crop;
                        case IndicatorSizeType.Transform: return RowLine2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.ColumnLine:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return ColumnLine2Empty;
                        case IndicatorSizeType.Point: return ColumnLine2Point;
                        case IndicatorSizeType.RowLine: return ColumnLine2RowLine;
                        case IndicatorSizeType.ColumnLine: return ColumnLine2ColumnLine;
                        case IndicatorSizeType.Crop: return ColumnLine2Crop;
                        case IndicatorSizeType.Transform: return ColumnLine2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.Crop:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Crop2Empty;
                        case IndicatorSizeType.Point: return Crop2Point;
                        case IndicatorSizeType.RowLine: return Crop2RowLine;
                        case IndicatorSizeType.ColumnLine: return Crop2ColumnLine;
                        case IndicatorSizeType.Crop: return Crop2Crop;
                        case IndicatorSizeType.Transform: return Crop2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.Transform:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Transform2Empty;
                        case IndicatorSizeType.Point: return Transform2Point;
                        case IndicatorSizeType.RowLine: return Transform2RowLine;
                        case IndicatorSizeType.ColumnLine: return Transform2ColumnLine;
                        case IndicatorSizeType.Crop: return Transform2Crop;
                        case IndicatorSizeType.Transform: return Transform2Transform;
                        default: return Empty;
                    }
                default: return Empty;
            }
        }

        public static ComposerSizeType ToComposerSizeType(SizeType sizeType, IndicatorKind kind)
        {
            switch (sizeType)
            {
                case Indicators.SizeType.Empty:
                    switch (kind)
                    {
                        default: return ComposerSizeType.Empty;
                    }
                case Indicators.SizeType.Point:
                    switch (kind)
                    {
                        case IndicatorKind.X: return ComposerSizeType.PointX;
                        case IndicatorKind.Y: return ComposerSizeType.PointY;
                        default: return ComposerSizeType.Empty;
                    }
                case Indicators.SizeType.RowLine:
                    switch (kind)
                    {
                        case IndicatorKind.X: return ComposerSizeType.RowLineX;
                        case IndicatorKind.Y: return ComposerSizeType.RowLineY;
                        case IndicatorKind.Width: return ComposerSizeType.RowLineWidth;
                        case IndicatorKind.Rotation: return ComposerSizeType.RowLineRotation;
                        default: return ComposerSizeType.Empty;
                    }
                case Indicators.SizeType.ColumnLine:
                    switch (kind)
                    {
                        case IndicatorKind.X: return ComposerSizeType.ColumnLineX;
                        case IndicatorKind.Y: return ComposerSizeType.ColumnLineY;
                        case IndicatorKind.Height: return ComposerSizeType.ColumnLineHeight;
                        case IndicatorKind.Rotation: return ComposerSizeType.ColumnLineRotation;
                        default: return ComposerSizeType.Empty;
                    }
                case Indicators.SizeType.Panel:
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