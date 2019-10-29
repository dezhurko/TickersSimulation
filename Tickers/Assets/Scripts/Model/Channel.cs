using System;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    [SerializeField]
    private float latency = 3f;
    
    private List<Msg> messages = new List<Msg>();

    public event Action<Msg> MessagePost;
    public event Action<Msg> MessageGet;

    public void Post(Msg m)
    {
        m.TimestampInChannel = Time.time;
        m.TravelTime = GetLatencyTime();
        messages.Add(m);
        
        MessagePost?.Invoke(m);
    }

    public IEnumerable<Msg> Get()
    {
        var messagesToGet = new List<Msg>();
        
        if (messages.Count == 0)
        {
            return messagesToGet;
        }

        foreach (var m in messages)
        {
            if (m != null && Time.time - m.TimestampInChannel > m.TravelTime)
            {
                messagesToGet.Add(m);
            }
        }

        foreach (var m in messagesToGet)
        {
            messages.Remove(m);
            MessageGet?.Invoke(m);
        }

        return messagesToGet.ToArray();
    }

    private float GetLatencyTime()
    {
        return latency;
    }
}