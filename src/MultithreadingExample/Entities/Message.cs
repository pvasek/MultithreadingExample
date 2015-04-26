namespace MultithreadingExample.Entities
{
    public class Message
    {
        public Message()
        {
            DispatchingCount = 1;
        }

        public string Id { get; set; }
        public string RecepientId { get; set; }
        public int DispatchingCount { get; set; }
    }
}
