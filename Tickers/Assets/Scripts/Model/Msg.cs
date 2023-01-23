namespace Model
{
    public class Msg
    {
        public int Tick { get; set; }
        public float TimestampInChannel { get; set; }
        public float TravelTime { get; set; }
        public int BufferSizeOnServer { get; set; }
        public float TimeReceivedServer { get; set; }
        public float TimeSentServer { get; set; }
        public float TimeSentClient { get; set; }
    }
}