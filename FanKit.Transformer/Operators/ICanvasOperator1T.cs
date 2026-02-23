namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator1<T>
    {
        event OperatorSingleStartingEventHandler<T> Single_Start;
        event OperatorSingleEventHandler<T> Single_Delta;
        event OperatorSingleEventHandler<T> Single_Complete;

        event OperatorWheelEventHandler Wheel_Changed;

        bool IsDisableTouch { get; set; }

        bool IsInContact { get; }
    }
}