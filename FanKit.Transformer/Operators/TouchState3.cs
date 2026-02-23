namespace FanKit.Transformer.Operators
{
    public enum TouchState3 : byte
    {
        None,

        // None -> Indeterminate -> Single
        // None -> Indeterminate -> Double
        Indeterminate, // Stopwatch start

        SingleFinger,
        SingleFingerToRightButton,

        DoubleFingers,

        // Double -> DoubleOnly0 -> None
        // Double -> DoubleOnly0 -> Double ->
        DoubleFingersOnly0,
        // Double -> DoubleOnly1 -> None
        // Double -> DoubleOnly1 -> Double ->
        DoubleFingersOnly1,

        Pen,

        LeftButton,
        MiddleButton,
        RightButton,
    }
}