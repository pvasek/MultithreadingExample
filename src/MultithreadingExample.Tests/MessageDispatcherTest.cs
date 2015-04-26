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
    public class MessageDispatcherTest
    {
        [Test]
        public void ShoudDispatchAllMessages()
        {
            var processorFactory = new Mock<IProcessorFactory>();
            var processors = new List<TestProcessor>();
            processorFactory
                .Setup(i => i.Create(It.IsAny<string>(), It.IsAny<IMessageDispatcher>()))
                .Returns(() =>
                {
                    var processor = new TestProcessor();
                    processors.Add(processor);
                    return processor;
                });
                
            var target = new MessageDispatcher(10, processorFactory.Object);
            var messages = Enumerable
                .Range(0, 100)
                .Select(i => new Message())
                .ToList();

            
            target.DispatchAll(messages);
            Assert.AreEqual(10, processors.Count);

            foreach (var processor in processors)
            {
                Assert.AreEqual(10, processor.ProcessCount);
                Assert.AreEqual(1, processor.StartCount);
                Assert.AreEqual(0, processor.StopCount);
            }
            foreach (var processor in processors)
            {
                for (var i = 0; i < 10; i++)
                {
                    processor.Done();
                }
            }
            foreach (var processor in processors)
            {
                Assert.AreEqual(10, processor.ProcessCount);
                Assert.AreEqual(1, processor.StartCount);
                Assert.AreEqual(1, processor.StopCount);
            }          
        }

        [Test]
        public void ShouldDispatchSingleMessage()
        {
            var processorFactory = new Mock<IProcessorFactory>();
            var processor = new TestProcessor();
            processorFactory
                .Setup(i => i.Create(It.IsAny<string>(), It.IsAny<IMessageDispatcher>()))
                .Returns(processor);

            var target = new MessageDispatcher(1, processorFactory.Object);

            target.Dispatch(new Message());
            Assert.AreEqual(1, processor.ProcessCount);
        }

        private class TestProcessor: IProcessor
        {
            public int ProcessCount { get; set; }
            public int StartCount { get; set; }
            public int StopCount { get; set; }

            public void Done()
            {
                if (OnDone != null)
                {
                    OnDone(this, new Message());
                }
            }

            public void Process(Message message)
            {
                ProcessCount++;
            }

            public void Start()
            {
                StartCount++;
            }

            public void Stop()
            {
                StopCount++;
            }

            public event EventHandler<Message> OnDone;
        }
    }
}
