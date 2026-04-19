namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator1
    {
        event SingleStartingEventHandler Single_Start;
        event SingleEventHandler Single_Delta;
        event SingleEventHandler Single_Complete;

        event WheelEventHandler Wheel_Changed;

        bool IsDisableTouch { get; set; }

        bool IsInContact { get; }
    }
}