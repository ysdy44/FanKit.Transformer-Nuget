namespace FanKit.Transformer.Operators
{
    internal enum TouchState2 : byte
    {
        None,

        SingleFinger,
        SingleFingerToRightButton,

        Pen,

        LeftButton,
        MiddleButton,
        RightButton,
    }
}