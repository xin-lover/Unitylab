using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Luwu.Net
{
    public class NetEasy
    {
        protected int mHostId;
        protected int mChannelId;
        protected int connectionId;

        public virtual void Init()
        {
            // Init Transport using default values.
            NetworkTransport.Init();

            // Create a connection config and add a Channel.
            ConnectionConfig config = new ConnectionConfig();
            mChannelId = config.AddChannel(QosType.Reliable);

            // Create a topology based on the connection config.
            HostTopology topology = new HostTopology(config, 10);

            // Create a host based on the topology we just created, and bind the socket to port 12345.
            mHostId = NetworkTransport.AddHost(topology, 8888);
        }

        public void HandleMessageReceive()
        {
            int outHostId;
            int outConnectionId;
            int outChannelId;
            byte[] buffer = new byte[1024];
            int bufferSize = 1024;
            int receiveSize;
            byte error;

            NetworkEventType evnt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, bufferSize, out receiveSize, out error);
            switch (evnt)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected hotid:" + outHostId);
                    break;
                case NetworkEventType.DisconnectEvent:
                    Debug.Log("Connected, error:" + error.ToString());
                    break;
            }
        }

        public void Connect(string ip,int port)
        {
            // Connect to the host with IP 10.0.0.42 and port 54321
            byte error;
            connectionId = NetworkTransport.Connect(mHostId, ip, port, 0, out error);
            if ((NetworkError)error != NetworkError.Ok)
            {
                Logger.Error("connect error");
            }
        }

        void Disconnect()
        {
            byte error;
            NetworkTransport.Disconnect(mHostId, connectionId, out error);
        }
    }

    public class NetServer : MonoBehaviour
    {
        NetEasy ne = new NetEasy();

        private void Start()
        {
            ne.Init();
        }
        private void Update()
        {
            ne.HandleMessageReceive();
        }
    }
}


