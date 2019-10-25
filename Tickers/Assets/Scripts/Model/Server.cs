using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField]
    private Channel toClient;
    [SerializeField]
    private Channel fromClient;

    private LocalTime time;
    private Ticker ticker;
    private Dictionary<int, Msg> buffer = new Dictionary<int, Msg>();

    private int bufferSize = 0;

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
                
                Debug.Log($"S[{ticker.Current}]. Send tick: {m.Tick}. Buffer size {bufferSize}");
            }
        }
    }
}
