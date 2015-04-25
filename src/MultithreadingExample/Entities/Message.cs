namespace MultithreadingExample.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string RecepientId { get; set; }
        public int ForwardingCount { get; set; }
    }
}
