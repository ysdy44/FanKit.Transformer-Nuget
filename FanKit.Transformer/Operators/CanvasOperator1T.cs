namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator1<T>
    {
        event SingleStartingEventHandler<T> Single_Start;
        event SingleEventHandler<T> Single_Delta;
        event SingleEventHandler<T> Single_Complete;

        event WheelEventHandler Wheel_Changed;

        bool IsDisableTouch { get; set; }

        bool IsInContact { get; }
    }
}