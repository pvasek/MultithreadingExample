using MultithreadingExample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MultithreadingExample.Services
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly List<IProcessor> _processors;
        private readonly Random _random = new Random(Environment.TickCount);
        private readonly object _syncRoot = new object();
        private int _messagesInProgress;
        private readonly ManualResetEvent _doneEvent = new ManualResetEvent(false);

        public MessageDispatcher(int numberOfProcessors, IProcessorFactory processorFactory)
        {
            _processors = Enumerable
                .Range(0, numberOfProcessors)
                .Select(i => processorFactory.Create(i.ToString(), this))
                .ToList();

            foreach (var processor in _processors)
            {
                processor.OnDone += Done;
            }
        }

        private IProcessor GetRandomProcessor()
        {
            lock (_syncRoot)
            {
                return _processors[_random.Next(_processors.Count)];
            }
        }

        private void FinishProcessing()
        {
            foreach (var processor in _processors)
            {
                processor.Stop();
            }
            _doneEvent.Set();            
        }

        private void Done(object sender, Message message)
        {
            Interlocked.Decrement(ref _messagesInProgress);
            if (_messagesInProgress == 0)
            {
                FinishProcessing();
            }
        }

        public void Dispatch(Message message)
        {
            message.DispatchingCount += 1;
            var processor = GetRandomProcessor();
            processor.Process(message);
        }
    
        public void DispatchAll(List<Message> messages)
        {
            _messagesInProgress = messages.Count;
            var next = 0;
            foreach (var message in messages)
            {
                _processors[next].Process(message);
                next++;
                if (next >= _processors.Count)
                {
                    next = 0;
                }
            }

            foreach (var processor in _processors)
            {
                processor.Start();
            }            
        }

        public void WaitToFinish()
        {
            _doneEvent.WaitOne();
        }
    }
}
