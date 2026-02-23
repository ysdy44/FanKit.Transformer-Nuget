namespace FanKit.Transformer.Operators
{
    public interface ICanvasOperator2
    {
        event OperatorSingleStartingEventHandler Single_Start;
        event OperatorSingleEventHandler Single_Delta;
        event OperatorSingleEventHandler Single_Complete;

        event OperatorRightEventHandler Right_Start;
        event OperatorRightEventHandler Right_Delta;
        event OperatorRightEventHandler Right_Complete;

        event OperatorWheelEventHandler Wheel_Changed;

        TouchMode TouchMode { get; set; }

        bool IsInContact { get; }
    }
}