using MultithreadingExample.Entities;

namespace MultithreadingExample.Services
{
    public interface IMessageDispatcher
    {
        void Dispatch(Message message);
    }
}
