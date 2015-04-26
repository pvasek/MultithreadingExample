using System;
using System.Collections.Concurrent;
using System.Threading;
using MultithreadingExample.Entities;

namespace MultithreadingExample.Services
{
    public class Processor: IProcessor
    {
        public Processor(string id, IMessageDispatcher messageDispatcher)
        {
            _id = id;
            _messageDispatcher = messageDispatcher;
        }

        private readonly ConcurrentQueue<Message> _queue = new ConcurrentQueue<Message>();
        private Thread _thread;
        private readonly AutoResetEvent _continueEvent = new AutoResetEvent(false);
        private readonly IMessageDispatcher _messageDispatcher;
        private bool _stopped;
        private readonly string _id;

        private void Done(Message message)
        {
            if (OnDone != null)
            {
                OnDone(this, message);
            }
        }

        public event EventHandler<Message> OnDone; 

        public void Process(Message message)
        {
            _queue.Enqueue(message);
            _continueEvent.Set();                        
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
                        if (_id == message.RecepientId)
                        {
                            Done(message);
                        }
                        else
                        {
                            _messageDispatcher.Dispatch(message);
                        }
                    }
                    if (_queue.Count == 0)
                    {
                        _continueEvent.WaitOne();
                    }
                    if (_stopped)
                    {
                        break;
                    }
                }
            });
            _thread.Start();
        }

        public void Stop()
        {
            _stopped = true;
            _continueEvent.Set();
            _thread = null;
        }
    }
}
