namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator2
    {
        event SingleStartingEventHandler Single_Start;
        event SingleEventHandler Single_Delta;
        event SingleEventHandler Single_Complete;

        event RightEventHandler Right_Start;
        event RightEventHandler Right_Delta;
        event RightEventHandler Right_Complete;

        event WheelEventHandler Wheel_Changed;

        TouchMode TouchMode { get; set; }

        bool IsInContact { get; }
    }
}