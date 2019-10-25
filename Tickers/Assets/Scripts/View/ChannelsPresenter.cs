using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelsPresenter : MonoBehaviour
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

        msgViews[msg] = msgView;
    }
    
    private void DeleteMessageView(Msg msg)
    {
        Destroy(msgViews[msg]);
    }
}
