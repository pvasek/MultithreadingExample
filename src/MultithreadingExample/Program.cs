using System;
using System.Linq;
using MultithreadingExample.Entities;
using MultithreadingExample.Services;

namespace MultithreadingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var numberOfProcessors = Int32.Parse(args[0]);
            var numberOfMessages = Int32.Parse(args[1]);
            var random = new Random(Environment.TickCount);

            var scheduler = new MessageDispatcher(numberOfProcessors, new ProcessorFactory());
            
            var messages = Enumerable
                .Range(0, numberOfProcessors * numberOfMessages)
                .Select(i => new Message
                {
                    Id = i.ToString(), 
                    RecepientId = random.Next(numberOfProcessors).ToString()
                })
                .ToList();

            scheduler.DispatchAll(messages);
            scheduler.WaitToFinish();

            var averageForwarding = messages.Average(i => i.DispatchingCount);
            
            var histogram = messages
                .GroupBy(i => i.DispatchingCount)
                .OrderBy(i => i.Key)
                .Select(i => new {Number = i.Key, Count = i.Count()})
                .ToList();

            Console.WriteLine("{0:0.000}", averageForwarding);
            foreach (var histogramItem in histogram)
            {
                Console.WriteLine("{0}\t{1}", histogramItem.Number, histogramItem.Count);
            }
            
            Console.ReadLine();
        }
    }    
}
