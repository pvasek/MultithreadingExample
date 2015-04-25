using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MultithreadingExample.Entities;
using MultithreadingExample.Services;
using NUnit.Framework;

namespace MultithreadingExample.Tests
{
    [TestFixture]
    public class SchedulerTest
    {
        [Test]
        public void StartTest()
        {
            var processorFactory = new Mock<IProcessorFactory>();
            var processors = new List<TestProcessor>();
            processorFactory
                .Setup(i => i.Create(It.IsAny<string>(), It.IsAny<Action<Message>>()))
                .Returns(() =>
                {
                    var processor = new TestProcessor();
                    processors.Add(processor);
                    return processor;
                });
                
            var target = new Scheduler(10, processorFactory.Object);
            var messages = Enumerable
                .Range(0, 100)
                .Select(i => new Message())
                .ToList();

            target.Start(messages);
            Assert.AreEqual(10, processors.Count);
            foreach (var processor in processors)
            {
                Assert.AreEqual(10, processor.ProcessCount);
            }
        }

        private class TestProcessor: IProcessor
        {
            public string Id { get; set; }
            public int QueueCount { get; set; }
            public int ProcessCount { get; set; }

            public void Process(Message message)
            {
                ProcessCount++;
            }
        }
    }
}
