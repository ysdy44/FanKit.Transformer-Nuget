namespace FanKit.Transformer
{
    internal static partial class Constants
    {
        internal const byte None = 0;

        #region Channel
        internal const byte LeftTopX = 1;
        internal const byte LeftTopY = 2;

        internal const byte RightTopX = 3;
        internal const byte RightTopY = 4;

        internal const byte LeftBottomX = 5;
        internal const byte LeftBottomY = 6;

        internal const byte RightBottomX = 7;
        internal const byte RightBottomY = 8;
        #endregion

        #region Line
        internal const byte Handle0 = 1;
        internal const byte Handle1 = 2;

        internal const byte Handle = 3;

        internal const byte Center = 4;

        internal const byte Point0 = 5;
        internal const byte Point1 = 6;
        #endregion

        #region Box
        internal const byte Contains = 1;

        internal const byte HandleLeftTop = 2; // UpperLeft
        internal const byte HandleRightTop = 3; // UpperRight
        internal const byte HandleLeftBottom = 4; // LowerLeft
        internal const byte HandleRightBottom = 5; // LowerRight

        internal const byte HandleLeft = 6;
        internal const byte HandleTop = 7;
        internal const byte HandleRight = 8;
        internal const byte HandleBottom = 9;

        internal const byte CenterLeft = 10;
        internal const byte CenterTop = 11; // Upper
        internal const byte CenterRight = 12;
        internal const byte CenterBottom = 13; // Lower

        internal const byte LeftTop = 14; // UpperLeft
        internal const byte RightTop = 15; // UpperRight
        internal const byte LeftBottom = 16; // LowerLeft
        internal const byte RightBottom = 17; // LowerRight
        #endregion
    }
}