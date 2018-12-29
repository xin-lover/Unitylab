using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Luwu.Net
{
    public class NetClient : MonoBehaviour
    {
        NetEasy ne = new NetEasy();

        private void Start()
        {
            ne.Init();
        }

        void Update()
        {
            //int outHostId;
            //int outConnectionId;
            //int outChannelId;
            //byte[] buffer = new byte[1024];
            //int bufferSize = 1024;
            //int receiveSize;
            //byte error;

            //NetworkEventType evnt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, bufferSize, out receiveSize, out error);
            //switch (evnt)
            //{
            //    case NetworkEventType.ConnectEvent:
            //        Debug.Log("connected hotid:" + outHostId);
            //        break;
            //    case NetworkEventType.DisconnectEvent:
            //        Debug.Log("Connected, error:" + error.ToString());
            //        break;
            //}
        }

        private void OnGUI()
        {
            if(GUI.Button(new Rect(20,20,100,50), "connect"))
            {
                ne.Connect("127.0.0.1", 8888);
            }
        }
    }

}
