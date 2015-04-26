namespace MultithreadingExample.Services
{
    public class ProcessorFactory: IProcessorFactory
    {
        public IProcessor Create(string id, IMessageDispatcher messageDispatcher)
        {
            return new Processor(id, messageDispatcher);
        }
    }
}
