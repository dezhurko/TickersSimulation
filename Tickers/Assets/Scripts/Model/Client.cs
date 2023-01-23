using UnityEngine;

namespace Model
{
    public class Client : MonoBehaviour
    {
        [SerializeField] 
        private Channel toServer;
        [SerializeField] 
        private Channel fromServer;

        private LocalTime time;
        private Ticker ticker;

        private void Awake()
        {
            time = new LocalTime();
            ticker = new Ticker(time)
            {
                Current = 10
            };
        }

        private void Update()
        {
            var ticks = ticker.TryTick();

            for (int t = 0; t < ticks; t++)
            {
                var m = new Msg()
                {
                    Tick = ticker.Current,
                    TimeSentClient = time.Current,
                };

                toServer.Post(m);

                Debug.Log($"C[{ticker.Current}] send message.");
            }

            var msgs = fromServer.Get();

            foreach (var msg in msgs)
            {
                Debug.Log($"C[{ticker.Current}] receive msg tick {msg.Tick}. buffer size {msg.BufferSizeOnServer}");
            }
        }
    }
}