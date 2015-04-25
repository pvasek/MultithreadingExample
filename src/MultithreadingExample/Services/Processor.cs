using MultithreadingExample.Entities;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MultithreadingExample.Services
{
    public class Processor: IProcessor
    {
        public Processor(string id, Action<Message> forwardMessage)
        {
            Id = id;
            _forwardMessage = forwardMessage;
        }

        private readonly ConcurrentQueue<Message> _queue = new ConcurrentQueue<Message>();
        private readonly Action<Message> _forwardMessage;
        private Thread _thread;

        public string Id { get; set; }

        public int QueueCount { get { return _queue.Count; } }

        public void Process(Message message)
        {
            _queue.Enqueue(message);
        }

        public void Start()
        {
            _thread = new Thread(() =>
            {
                while (true)
                {
                    Message message;
                    if (_queue.TryDequeue(out message))
                    {
                        if (Id == message.RecepientId)
                        {
                            // process
                            Console.WriteLine("processed " + message.Id);
                        }
                        else
                        {
                            _forwardMessage(message);
                        }
                    }
                    Thread.Sleep(20);
                }
            });
            _thread.Start();
        }
    }
}
