using System;
using MultithreadingExample.Entities;

namespace MultithreadingExample.Services
{
    public interface IProcessor
    {
        void Process(Message message);
        void Start();
        void Stop();
        event EventHandler<Message> OnDone;
    }
}
