using MultithreadingExample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultithreadingExample.Services
{
    public class Scheduler
    {
        private readonly List<IProcessor> _processors;
        private readonly Random _random = new Random(Environment.TickCount);
        private readonly object _syncRoot = new object();

        public Scheduler(int numberOfProcessors, IProcessorFactory processorFactory)
        {
            _processors = Enumerable
                .Range(0, numberOfProcessors)
                .Select(i => processorFactory.Create(i.ToString(), ForwardMessage))
                .ToList();
        }

        private void ForwardMessage(Message message)
        {
            message.ForwardingCount += 1;    
            //Console.WriteLine("forwarding " + message.Id);
            var processor = GetRandomProcessor();           
            processor.Process(message);
        }

        private IProcessor GetRandomProcessor()
        {
            lock (_syncRoot)
            {
                return _processors[_random.Next(_processors.Count)];
            }
        }
        
        public void Start(List<Message> messages)
        {
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
    }
}
