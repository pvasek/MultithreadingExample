using System;
using MultithreadingExample.Entities;

namespace MultithreadingExample.Services
{
    public interface IProcessorFactory
    {
        IProcessor Create(string id, Action<Message> forwardMessage);
    }
}
