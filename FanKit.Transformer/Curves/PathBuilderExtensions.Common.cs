using FanKit.Transformer.Controllers;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformer.Curves
{
    public static partial class PathBuilderExtensions
    {
        const bool Closed = true;
        const bool Open = false;

        // Radians
        const float PI = Constants.PI;
        const float PITwice = Constants.PITwice;
        const float PIOver2 = Constants.PIOver2;

        const float R360 = Constants.PI + Constants.PI;
        const float R270 = Constants.PI + Constants.PIOver2;
        const float R180 = Constants.PI;
        const float R90 = Constants.PIOver2;
        const float R0 = 0f;

        // Ellipse
        const float Z276 = 0.276114f;
        const float Z552 = 0.55228f;

        // Radians Quadrant
        const float RQ = R90;

        // Radians Positive
        const float RP360 = R360;
        const float RP270 = R270;
        const float RP180 = R180;
        const float RP90 = R90;

        // Radians Zero
        const float RZ0 = R0;

        // Radians Negative
        const float RN90 = -R90;
        const float RN180 = -R180;
        const float RN270 = -R270;
        const float RN360 = -R360;

        // Arc Positive
        const byte P360 = 0;
        const byte P270T360 = 1;
        const byte P270 = 2;
        const byte P180T270 = 3;
        const byte P180 = 4;
        const byte P90T180 = 5;
        const byte P90 = 6;
        const byte P0T90 = 7;

        // Arc Zero
        const byte Z0 = 8;

        // Arc Negative
        const byte N0T90 = 9;
        const byte N90 = 10;
        const byte N90T180 = 11;
        const byte N180 = 12;
        const byte N180T270 = 13;
        const byte N270 = 14;
        const byte N270T360 = 15;
        const byte N360 = 16;

        // Arrow
        private static Vector2 GetArrowFocusVector(float verticalLength, float horizontalLength, Vector2 horizontal)
        {
            if (verticalLength < horizontalLength)
                return 0.5f * (verticalLength / horizontalLength) * horizontal;
            else
                return 0.5f * horizontal;
        }

        private static Vector2 GetArrowWidthVector(bool isAbsolute, float width2, float value, Vector2 vertical, float verticalLength)
        {
            float width = isAbsolute ? width2 : value * verticalLength;
            return vertical * (width / verticalLength) / 2;
        }

        // Heart
        private static Vector2 HeartTopSpread(float spread)
        {
            // Rang
            //   x: 0~1
            //   y: 1.0~ - 0.8
            //  y=1 - 1.8x
            float topSpread = 1f - spread * 1.8f;
            return new Vector2(0, topSpread);
        }

        // Arc
        private static byte GetArcMode(float sweepAngle)
        {
            switch (sweepAngle.CompareTo(RZ0))
            {
                case 1:
                    switch (sweepAngle.CompareTo(RP90))
                    {
                        case 1:
                            switch (sweepAngle.CompareTo(RP180))
                            {
                                case 1:
                                    switch (sweepAngle.CompareTo(RP270))
                                    {
                                        case 1:
                                            switch (sweepAngle.CompareTo(RP360))
                                            {
                                                case 1: case 0: return P360;
                                                case -1: return P270T360;
                                                default: return Z0;
                                            }
                                        case 0: return P270;
                                        case -1: return P180T270;
                                        default: return Z0;
                                    }
                                case 0: return P180;
                                case -1: return P90T180;
                                default: return Z0;
                            }
                        case 0: return P90;
                        case -1: return P0T90;
                        default: return Z0;
                    }
                case 0: return Z0;
                case -1:
                    switch (sweepAngle.CompareTo(RN90))
                    {
                        case 1: return N0T90;
                        case 0: return N90;
                        case -1:
                            switch (sweepAngle.CompareTo(RN180))
                            {
                                case 1: return N90T180;
                                case 0: return N180;
                                case -1:
                                    switch (sweepAngle.CompareTo(RN270))
                                    {
                                        case 1: return N180T270;
                                        case 0: return N270;
                                        case -1:
                                            switch (sweepAngle.CompareTo(RN360))
                                            {
                                                case 1: return N270T360;
                                                case 0: case -1: return N360;
                                                default: return Z0;
                                            }
                                        default: return Z0;
                                    }
                                default: return Z0;
                            }
                        default: return Z0;
                    }
                default: return Z0;
            }
        }
    }
}