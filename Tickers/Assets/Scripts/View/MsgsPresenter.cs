using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace View
{
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

        private readonly Dictionary<Msg, MsgView> msgViewsInChanelToServer = new Dictionary<Msg, MsgView>();
        private readonly Dictionary<Msg, MsgView> msgViewsInChanelToClient = new Dictionary<Msg, MsgView>();
        private readonly Dictionary<Msg, MsgView> msgViewsOnServer = new Dictionary<Msg, MsgView>();

        private void Awake()
        {
            channelClientServer.MessagePost += ChannelClientServerOnMessagePost;
            channelClientServer.MessageGet += ChannelClientServerOnMessageGet;
            channelServerClient.MessagePost += ChannelServerClientOnMessagePost;
            channelServerClient.MessageGet += ChannelServerClientOnMessageGet;
            server.BufferChanged += ServerOnBufferChanged;
        }

        private void Update()
        {
            foreach (var msgView in msgViewsInChanelToServer.Values)
            {
                var y = msgView.transform.position.y;
                var m = msgView.Msg;
                msgView.transform.position =
                    Vector3.Lerp(client.transform.position, server.transform.position,
                        (Time.time - m.TimestampInChannel) / m.TravelTime) + new Vector3(0, y, 0);
            }

            foreach (var msgView in msgViewsInChanelToClient.Values)
            {
                var y = msgView.transform.position.y;
                var m = msgView.Msg;
                msgView.transform.position =
                    Vector3.Lerp(server.transform.position, client.transform.position,
                        (Time.time - m.TimestampInChannel) / m.TravelTime) + new Vector3(0, y, 0);
            }
        }

        private void ServerOnBufferChanged(List<Msg> msgs)
        {
            foreach (var msg in msgViewsOnServer.Values)
            {
                Destroy(msg.gameObject);
            }

            msgs = msgs.OrderBy(m => m.Tick).ToList();

            msgViewsOnServer.Clear();

            for (int i = 0; i < msgs.Count; i++)
            {
                var msg = msgs[i];
                var msgView = Instantiate<MsgView>(msgPrefab);
                msgView.Setup(msg);

                msgView.transform.position = server.transform.position + new Vector3(1, (i + 1) / 2.0f, 0);

                msgViewsOnServer[msg] = msgView;
            }
        }

        private void ChannelServerClientOnMessageGet(Msg msg)
        {
            DeleteMessageView(msg, msgViewsInChanelToClient);
        }

        private void ChannelServerClientOnMessagePost(Msg msg)
        {
            CreateMessageView(msg, msgViewsInChanelToClient, 0);
        }

        private void ChannelClientServerOnMessageGet(Msg msg)
        {
            DeleteMessageView(msg, msgViewsInChanelToServer);
        }

        private void ChannelClientServerOnMessagePost(Msg msg)
        {
            CreateMessageView(msg, msgViewsInChanelToServer, 1);
        }

        private void CreateMessageView(Msg msg, Dictionary<Msg, MsgView> views, float pos)
        {
            var msgView = Instantiate<MsgView>(msgPrefab);
            msgView.Setup(msg);

            msgView.transform.position = client.transform.position + new Vector3(0, pos, 0);

            views[msg] = msgView;
        }

        private void DeleteMessageView(Msg msg, Dictionary<Msg, MsgView> views)
        {
            Destroy(views[msg].gameObject);
            views.Remove(msg);
        }
    }
}