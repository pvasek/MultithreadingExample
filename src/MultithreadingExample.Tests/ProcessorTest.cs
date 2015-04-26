using System.Threading;
using Moq;
using MultithreadingExample.Entities;
using MultithreadingExample.Services;
using NUnit.Framework;

namespace MultithreadingExample.Tests
{
    [TestFixture]
    public class ProcessorTest
    {
        [Test]
        public void ShouldProcessMessageThatIsAddressedToTheProcessor()
        {
            var message = new Message { RecepientId = "A" };
            var doneCount = 0;
            var dispatchCount = 0;

            var dispatcher = new Mock<IMessageDispatcher>();
            
            var target = new Processor("A", dispatcher.Object);

            target.OnDone += (sender, m) => doneCount++;

            target.Process(message);

            Assert.AreEqual(0, doneCount);
            Assert.AreEqual(0, dispatchCount);

            target.Start();
            Thread.Sleep(200);

            Assert.AreEqual(1, doneCount);
            Assert.AreEqual(0, dispatchCount);

            target.Stop();
            target.Stop();
        }

        [Test]
        public void ShouldDispatchMessageThatIsAddressedToAnotherProcessor()
        {
            var message = new Message { RecepientId = "B" };
            var doneCount = 0;
            var dispatchCount = 0;

            var dispatcher = new Mock<IMessageDispatcher>();
            dispatcher.Setup(i => i.Dispatch(message)).Callback(() => dispatchCount++);

            var target = new Processor("A", dispatcher.Object);
            
            target.OnDone += (sender, m) => doneCount++;
            
            target.Process(message);

            Assert.AreEqual(0, doneCount);
            Assert.AreEqual(0, dispatchCount);

            target.Start();
            Thread.Sleep(200);

            Assert.AreEqual(0, doneCount);
            Assert.AreEqual(1, dispatchCount);

            target.Stop();
        }
    }
}
