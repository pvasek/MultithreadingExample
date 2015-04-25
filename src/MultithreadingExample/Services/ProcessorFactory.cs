using System;
using MultithreadingExample.Entities;

namespace MultithreadingExample.Services
{
    public class ProcessorFactory: IProcessorFactory
    {
        public IProcessor Create(string id, Action<Message> forwardMessage)
        {
            return new Processor(id, forwardMessage);
        }
    }
}
