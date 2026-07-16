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

        public static ComposerParameterKind ToComposerParameterKind(ComposerPointsDistribution pointsDistribution, ParameterKind kind)
        {
            switch (pointsDistribution)
            {
                case ComposerPointsDistribution.Empty:
                    switch (kind)
                    {
                        default: return ComposerParameterKind.Empty;
                    }
                case ComposerPointsDistribution.Point:
                    switch (kind)
                    {
                        case ParameterKind.X: return ComposerParameterKind.PointX;
                        case ParameterKind.Y: return ComposerParameterKind.PointY;
                        default: return ComposerParameterKind.Empty;
                    }
                case ComposerPointsDistribution.RowLine:
                    switch (kind)
                    {
                        case ParameterKind.X: return ComposerParameterKind.RowLineX;
                        case ParameterKind.Y: return ComposerParameterKind.RowLineY;
                        case ParameterKind.Width: return ComposerParameterKind.RowLineWidth;
                        case ParameterKind.Rotation: return ComposerParameterKind.RowLineRotation;
                        default: return ComposerParameterKind.Empty;
                    }
                case ComposerPointsDistribution.ColumnLine:
                    switch (kind)
                    {
                        case ParameterKind.X: return ComposerParameterKind.ColumnLineX;
                        case ParameterKind.Y: return ComposerParameterKind.ColumnLineY;
                        case ParameterKind.Height: return ComposerParameterKind.ColumnLineHeight;
                        case ParameterKind.Rotation: return ComposerParameterKind.ColumnLineRotation;
                        default: return ComposerParameterKind.Empty;
                    }
                case ComposerPointsDistribution.Panel:
                    switch (kind)
                    {
                        case ParameterKind.X: return ComposerParameterKind.PanelX;
                        case ParameterKind.Y: return ComposerParameterKind.PanelY;
                        case ParameterKind.Width: return ComposerParameterKind.PanelWidth;
                        case ParameterKind.Height: return ComposerParameterKind.PanelHeight;
                        case ParameterKind.Rotation: return ComposerParameterKind.PanelRotation;
                        case ParameterKind.Skew: return ComposerParameterKind.PanelSkew;
                        default: return ComposerParameterKind.Empty;
                    }
                default: return ComposerParameterKind.Empty;
            }
        }

        public static TransformsParameterKind ToTransformsParameterKind(int count, ParameterKind kind)
        {
            switch (count)
            {
                case 0:
                    return TransformsParameterKind.None;
                case 1:
                    switch (kind)
                    {
                        case ParameterKind.X: return TransformsParameterKind.X;
                        case ParameterKind.Y: return TransformsParameterKind.Y;
                        case ParameterKind.Width: return TransformsParameterKind.Width;
                        case ParameterKind.Height: return TransformsParameterKind.Height;
                        case ParameterKind.Rotation: return TransformsParameterKind.Rotation;
                        case ParameterKind.Skew: return TransformsParameterKind.Skew;
                        default: return TransformsParameterKind.None;
                    }
                default:
                    switch (kind)
                    {
                        case ParameterKind.X: return TransformsParameterKind.MultiX;
                        case ParameterKind.Y: return TransformsParameterKind.MultiY;
                        case ParameterKind.Width: return TransformsParameterKind.MultiWidth;
                        case ParameterKind.Height: return TransformsParameterKind.MultiHeight;
                        case ParameterKind.Rotation: return TransformsParameterKind.MultiRotation;
                        case ParameterKind.Skew: return TransformsParameterKind.MultiSkew;
                        default: return TransformsParameterKind.None;
                    }
            }
        }

        public static TransformParameterKind ToTransformParameterKind(ParameterKind kind)
        {
            switch (kind)
            {
                case ParameterKind.X: return TransformParameterKind.X;
                case ParameterKind.Y: return TransformParameterKind.Y;
                case ParameterKind.Width: return TransformParameterKind.Width;
                case ParameterKind.Height: return TransformParameterKind.Height;
                case ParameterKind.Rotation: return TransformParameterKind.Rotation;
                case ParameterKind.Skew: return TransformParameterKind.Skew;
                default: return TransformParameterKind.None;
            }
        }

        public static CropsParameterKind ToCropsParameterKind(int count, ParameterKind kind)
        {
            switch (count)
            {
                case 0:
                    return CropsParameterKind.None;
                case 1:
                    switch (kind)
                    {
                        case ParameterKind.X: return CropsParameterKind.X;
                        case ParameterKind.Y: return CropsParameterKind.Y;
                        case ParameterKind.Width: return CropsParameterKind.Width;
                        case ParameterKind.Height: return CropsParameterKind.Height;
                        //case ParameterKind.Rotation: return CropsParameterKind.Rotation;
                        //case ParameterKind.Skew: return CropsParameterKind.Skew;
                        default: return CropsParameterKind.None;
                    }
                default:
                    switch (kind)
                    {
                        case ParameterKind.X: return CropsParameterKind.MultiX;
                        case ParameterKind.Y: return CropsParameterKind.MultiY;
                        case ParameterKind.Width: return CropsParameterKind.MultiWidth;
                        case ParameterKind.Height: return CropsParameterKind.MultiHeight;
                        //case ParameterKind.Rotation: return CropsParameterKind.MultiRotation;
                        //case ParameterKind.Skew: return CropsParameterKind.MultiSkew;
                        default: return CropsParameterKind.None;
                    }
            }
        }

        public static CropParameterKind ToCropParameterKind(ParameterKind kind)
        {
            switch (kind)
            {
                case ParameterKind.X: return CropParameterKind.X;
                case ParameterKind.Y: return CropParameterKind.Y;
                case ParameterKind.Width: return CropParameterKind.Width;
                case ParameterKind.Height: return CropParameterKind.Height;
                //case ParameterKind.Rotation: return CropParameterKind.Rotation;
                //case ParameterKind.Skew: return CropParameterKind.Skew;
                default: return CropParameterKind.None;
            }
        }
        public static ColumnLineParameterKind ToColumnLineParameterKind(ParameterKind kind)
        {
            switch (kind)
            {
                case ParameterKind.X: return ColumnLineParameterKind.X;
                case ParameterKind.Y: return ColumnLineParameterKind.Y;
                case ParameterKind.Height: return ColumnLineParameterKind.Height;
                case ParameterKind.Rotation: return ColumnLineParameterKind.Rotation;
                default: return ColumnLineParameterKind.None;
            }
        }

        public static RowLineParameterKind ToRowLineParameterKind(ParameterKind kind)
        {
            switch (kind)
            {
                case ParameterKind.X: return RowLineParameterKind.X;
                case ParameterKind.Y: return RowLineParameterKind.Y;
                case ParameterKind.Width: return RowLineParameterKind.Width;
                case ParameterKind.Rotation: return RowLineParameterKind.Rotation;
                default: return RowLineParameterKind.None;
            }
        }

        public static PointParameterKind ToPointParameterKind(ParameterKind kind)
        {
            switch (kind)
            {
                case ParameterKind.X: return PointParameterKind.X;
                case ParameterKind.Y: return PointParameterKind.Y;
                default: return PointParameterKind.None;
            }
        }
    }
}