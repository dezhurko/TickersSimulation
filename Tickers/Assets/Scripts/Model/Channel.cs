using System;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    private Queue<Msg> messages = new Queue<Msg>();

    public event Action<Msg> MessagePost;
    public event Action<Msg> MessageGet;

    public void Post(Msg m)
    {
        m.TimestampInChannel = Time.time;
        m.TravelTime = GetTravelTime();
        messages.Enqueue(m);
        
        MessagePost?.Invoke(m);
    }

    public IEnumerable<Msg> Get()
    {
        var messagesToGet = new List<Msg>();
        
        if (messages.Count == 0)
        {
            return messagesToGet;
        }
        
        var m = messages.Peek();
        while (m != null && Time.time - m.TimestampInChannel > m.TravelTime)
        {
            messagesToGet.Add(messages.Dequeue());
            MessageGet?.Invoke(m);
            
            m = messages.Count > 0 ? messages.Peek() : null;
        }

        return messagesToGet.ToArray();
    }


    private float GetTravelTime()
    {
        return 10f;
    }
}