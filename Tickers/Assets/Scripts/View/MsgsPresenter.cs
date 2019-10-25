using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MsgsPresenter : MonoBehaviour
{
    [SerializeField]
    private Channel channelClientServer;
    [SerializeField]
    private Channel channelServerClient;
    [SerializeField]
    private Client client;
    [SerializeField]
    private Server server;

    [SerializeField]
    private MsgView msgPrefab;

    private Dictionary<Msg, MsgView> msgViews = new Dictionary<Msg,MsgView>();
    
    private void Awake()
    {
        channelClientServer.MessagePost+= ChannelClientServerOnMessagePost;
        channelClientServer.MessageGet += ChannelClientServerOnMessageGet;
    }

    private void Update()
    {
        foreach (var msgView in msgViews.Values)
        {
            var y = msgView.transform.position.y;
            var m = msgView.Msg;
            msgView.transform.position =
                Vector3.Lerp(client.transform.position, server.transform.position, (Time.time - m.TimestampInChannel) / m.TravelTime);// + new Vector3(0, y, 0);
                

        }
    }

    private void ChannelClientServerOnMessageGet(Msg msg)
    {
        DeleteMessageView(msg);
    }

    private void ChannelClientServerOnMessagePost(Msg msg)
    {
        CreateMessageView(msg);
    }

    private void CreateMessageView(Msg msg)
    {
        var msgView = Instantiate<MsgView>(msgPrefab);
        msgView.Setup(msg);

        msgView.transform.position = client.transform.position;// - new Vector3(0, msgViews.Count, 0);

        msgViews[msg] = msgView;
    }
    
    private void DeleteMessageView(Msg msg)
    {
        Destroy(msgViews[msg].gameObject);
        msgViews.Remove(msg);
    }
}
