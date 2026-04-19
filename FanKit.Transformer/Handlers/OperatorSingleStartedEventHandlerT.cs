namespace FanKit.Transformer
{
    public delegate void SingleStartedEventHandler<T>(double startingX, double startingY, double startedX, double startedY, T properties);
}