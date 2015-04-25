using MultithreadingExample.Entities;

namespace MultithreadingExample.Services
{
    public interface IProcessor
    {
        string Id { get; set; }
        int QueueCount { get; }
        void Process(Message message);
        void Start();
    }
}
