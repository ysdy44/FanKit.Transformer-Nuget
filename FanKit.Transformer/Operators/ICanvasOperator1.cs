namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator1
    {
        event OperatorSingleStartingEventHandler Single_Start;
        event OperatorSingleEventHandler Single_Delta;
        event OperatorSingleEventHandler Single_Complete;

        event OperatorWheelEventHandler Wheel_Changed;

        bool IsDisableTouch { get; set; }

        bool IsInContact { get; }
    }
}