using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    public class Server : MonoBehaviour
    {
        [SerializeField] 
        private Channel toClient;
        [SerializeField] 
        private Channel fromClient;

        private LocalTime time;
        private Ticker ticker;
        private int bufferSize = 0;
        
        private readonly Dictionary<int, Msg> buffer = new Dictionary<int, Msg>();

        public event Action<List<Msg>> BufferChanged;

        private void Awake()
        {
            time = new LocalTime();
            ticker = new Ticker(time);
        }

        private void Update()
        {
            var ticks = ticker.TryTick();


            var msgs = fromClient.Get();

            foreach (var msg in msgs)
            {
                msg.TimeReceivedServer = time.Current;
                buffer[msg.Tick] = msg;

                bufferSize = msg.Tick - ticker.Current;

                BufferChanged?.Invoke(buffer.Values.ToList());

                Debug.Log($"S[{ticker.Current}]. Receive tick: {msg.Tick}. Buffer size {bufferSize}");
            }

            for (int t = 0; t < ticks; t++)
            {
                var tickToProcess = ticker.Current - ticks + t + 1;
                if (buffer.ContainsKey(tickToProcess))
                {
                    var m = buffer[tickToProcess];
                    m.TimeSentServer = time.Current;
                    m.BufferSizeOnServer = bufferSize;
                    toClient.Post(m);

                    buffer.Remove(tickToProcess);

                    BufferChanged?.Invoke(buffer.Values.ToList());

                    Debug.Log($"S[{ticker.Current}]. Send tick: {m.Tick}. Buffer size {bufferSize}");
                }
            }
        }
    }
}