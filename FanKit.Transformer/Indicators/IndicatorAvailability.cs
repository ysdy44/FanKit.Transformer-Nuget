using System;
using static FanKit.Transformer.Indicators.ParameterAvailability;

namespace FanKit.Transformer.Indicators
{
    [Flags]
    public enum ParameterAvailability : short
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
        const ParameterAvailability Empty = Unavailable;
        const ParameterAvailability Point = Unavailable | XValue | YValue;
        const ParameterAvailability RowLine = Unavailable | XValue | YValue | WidthValue | RotationValue;
        const ParameterAvailability ColumnLine = Unavailable | XValue | YValue | HeightValue | RotationValue;
        const ParameterAvailability Crop = Unavailable | XValue | YValue | WidthValue | HeightValue;
        const ParameterAvailability Transform = Available;

        // 2
        const ParameterAvailability Empty2Empty = 0;
        const ParameterAvailability Empty2Point = XHasValue | YHasValue | XValue | YValue;
        const ParameterAvailability Empty2RowLine = XHasValue | YHasValue | WidthHasValue | RotationHasValue | XValue | YValue | WidthValue | RotationValue;
        const ParameterAvailability Empty2ColumnLine = XHasValue | YHasValue | HeightHasValue | RotationHasValue | XValue | YValue | HeightValue | RotationValue;
        const ParameterAvailability Empty2Crop = XHasValue | YHasValue | WidthHasValue | HeightHasValue | XValue | YValue | WidthValue | HeightValue;
        const ParameterAvailability Empty2Transform = Available;

        const ParameterAvailability Point2Empty = XHasValue | YHasValue;
        const ParameterAvailability Point2Point = 0;
        const ParameterAvailability Point2RowLine = WidthHasValue | RotationHasValue | WidthValue | RotationValue;
        const ParameterAvailability Point2ColumnLine = HeightHasValue | RotationHasValue | HeightValue | RotationValue;
        const ParameterAvailability Point2Crop = WidthHasValue | HeightHasValue | WidthValue | HeightValue;
        const ParameterAvailability Point2Transform = WidthHasValue | HeightHasValue | RotationHasValue | SkewHasValue | WidthValue | HeightValue | RotationValue | SkewValue;

        const ParameterAvailability RowLine2Empty = XHasValue | YHasValue | WidthHasValue | RotationHasValue;
        const ParameterAvailability RowLine2Point = WidthHasValue | RotationHasValue;
        const ParameterAvailability RowLine2RowLine = 0;
        const ParameterAvailability RowLine2ColumnLine = WidthHasValue | HeightHasValue | HeightValue;
        const ParameterAvailability RowLine2Crop = HeightHasValue | RotationHasValue | HeightValue;
        const ParameterAvailability RowLine2Transform = HeightHasValue | SkewHasValue | HeightValue | SkewValue;

        const ParameterAvailability ColumnLine2Empty = XHasValue | YHasValue | HeightHasValue | RotationHasValue;
        const ParameterAvailability ColumnLine2Point = HeightHasValue | RotationHasValue;
        const ParameterAvailability ColumnLine2RowLine = WidthHasValue | HeightHasValue | WidthValue;
        const ParameterAvailability ColumnLine2ColumnLine = 0;
        const ParameterAvailability ColumnLine2Crop = WidthHasValue | RotationHasValue | WidthValue;
        const ParameterAvailability ColumnLine2Transform = WidthHasValue | SkewHasValue | WidthValue | SkewValue;

        const ParameterAvailability Crop2Empty = XHasValue | YHasValue | WidthHasValue | HeightHasValue;
        const ParameterAvailability Crop2Point = WidthHasValue | HeightHasValue;
        const ParameterAvailability Crop2RowLine = HeightHasValue | RotationHasValue | RotationValue;
        const ParameterAvailability Crop2ColumnLine = WidthHasValue | RotationHasValue | RotationValue;
        const ParameterAvailability Crop2Crop = 0;
        const ParameterAvailability Crop2Transform = RotationHasValue | SkewHasValue | RotationValue | SkewValue;

        const ParameterAvailability Transform2Empty = Unavailable;
        const ParameterAvailability Transform2Point = WidthHasValue | HeightHasValue | RotationHasValue | SkewHasValue;
        const ParameterAvailability Transform2RowLine = HeightHasValue | SkewHasValue;
        const ParameterAvailability Transform2ColumnLine = WidthHasValue | SkewHasValue;
        const ParameterAvailability Transform2Crop = RotationHasValue | SkewHasValue;
        const ParameterAvailability Transform2Transform = 0;

        public static ParameterAvailability ToAvailability(IndicatorSizeType sizeType)
        {
            switch (sizeType)
            {
                case IndicatorSizeType.Empty: return Empty;
                case IndicatorSizeType.Point: return Point;
                case IndicatorSizeType.RowLine: return RowLine;
                case IndicatorSizeType.ColumnLine: return ColumnLine;
                case IndicatorSizeType.Cropper: return Crop;
                case IndicatorSizeType.Transformer: return Transform;
                default: return Empty;
            }
        }

        public static ParameterAvailability ToAvailabilityChange(IndicatorSizeType oldSizeType, IndicatorSizeType newSizeType)
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
                        case IndicatorSizeType.Cropper: return Empty2Crop;
                        case IndicatorSizeType.Transformer: return Empty2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.Point:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Point2Empty;
                        case IndicatorSizeType.Point: return Point2Point;
                        case IndicatorSizeType.RowLine: return Point2RowLine;
                        case IndicatorSizeType.ColumnLine: return Point2ColumnLine;
                        case IndicatorSizeType.Cropper: return Point2Crop;
                        case IndicatorSizeType.Transformer: return Point2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.RowLine:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return RowLine2Empty;
                        case IndicatorSizeType.Point: return RowLine2Point;
                        case IndicatorSizeType.RowLine: return RowLine2RowLine;
                        case IndicatorSizeType.ColumnLine: return RowLine2ColumnLine;
                        case IndicatorSizeType.Cropper: return RowLine2Crop;
                        case IndicatorSizeType.Transformer: return RowLine2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.ColumnLine:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return ColumnLine2Empty;
                        case IndicatorSizeType.Point: return ColumnLine2Point;
                        case IndicatorSizeType.RowLine: return ColumnLine2RowLine;
                        case IndicatorSizeType.ColumnLine: return ColumnLine2ColumnLine;
                        case IndicatorSizeType.Cropper: return ColumnLine2Crop;
                        case IndicatorSizeType.Transformer: return ColumnLine2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.Cropper:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Crop2Empty;
                        case IndicatorSizeType.Point: return Crop2Point;
                        case IndicatorSizeType.RowLine: return Crop2RowLine;
                        case IndicatorSizeType.ColumnLine: return Crop2ColumnLine;
                        case IndicatorSizeType.Cropper: return Crop2Crop;
                        case IndicatorSizeType.Transformer: return Crop2Transform;
                        default: return Empty;
                    }
                case IndicatorSizeType.Transformer:
                    switch (newSizeType)
                    {
                        case IndicatorSizeType.Empty: return Transform2Empty;
                        case IndicatorSizeType.Point: return Transform2Point;
                        case IndicatorSizeType.RowLine: return Transform2RowLine;
                        case IndicatorSizeType.ColumnLine: return Transform2ColumnLine;
                        case IndicatorSizeType.Cropper: return Transform2Crop;
                        case IndicatorSizeType.Transformer: return Transform2Transform;
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