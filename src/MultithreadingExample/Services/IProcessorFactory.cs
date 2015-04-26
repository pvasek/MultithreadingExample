namespace MultithreadingExample.Services
{
    public interface IProcessorFactory
    {
        IProcessor Create(string id, IMessageDispatcher messageDispatcher);
    }
}
