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
            var random = new Random();

            var scheduler = new Scheduler(numberOfProcessors, new ProcessorFactory());
            
            var messages = Enumerable
                .Range(0, numberOfProcessors * numberOfMessages)
                .Select(i => new Message
                {
                    Id = i.ToString(), 
                    RecepientId = random.Next(numberOfProcessors).ToString()
                })
                .ToList();

            scheduler.Start(messages);

            Console.ReadLine();
        }
    }    
}
