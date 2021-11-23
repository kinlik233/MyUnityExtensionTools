using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Main
{
    ///<summary>
    ///服务端网络服务类
    ///</summary>
    public class ServerUdpNetworkService : MonoSingleton<ServerUdpNetworkService>
    {
        private UdpClient udpServer;
        public event EventHandler<MessageArrivedEventArgs> MessageArrivedHandler;
        private Thread threadReceive;
        private List<IPEndPoint> allClientEP;
        //private UdpClient service;

        //由登录窗口传递服务端地址、端口
        public void Initialized(string ip,int port)
        {
            DontDestroyOnLoad(gameObject);

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(ip),port);
            udpServer = new UdpClient(serverEP);

            //service = new UdpClient(serverEP);
            threadReceive = new Thread(ReceiveChatMessage);
            threadReceive.Start();

            allClientEP = new List<IPEndPoint>();
        }

        public void SendChatMessage(ChatMessage msg, IPEndPoint remote)
        {
            byte[] buffer = msg.ObjectToBytes();
            int count = udpServer.Send(buffer, buffer.Length, remote);
        }

        private void ReceiveChatMessage()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {

                byte[] data = udpServer.Receive(ref remote);
                ChatMessage msg = ChatMessage.BytesToObject(data);
                OnMessageArrived(msg, remote);
                if (MessageArrivedHandler == null) continue;
                MessageArrivedEventArgs args = new MessageArrivedEventArgs()
                {
                    Message = msg,
                    MessageTime = DateTime.Now
                };
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() => { MessageArrivedHandler?.Invoke(this, args); });


            }
        }

        //服务端收到消息处理
        private void OnMessageArrived(ChatMessage msg, IPEndPoint remote)
        {
            switch (msg.Type)
            {
                case MessageType.Online:
                    allClientEP.Add(remote);
                    break;
                case MessageType.Offline:
                    allClientEP.Remove(remote);
                    break;
                case MessageType.General:
                    //allClientEP.ForEach(item => SendChatMessage(msg, remote));
                    for (int i = 0; i < allClientEP.Count; i++)
                    {
                        SendChatMessage(msg, allClientEP[i]);
                    }
                    break;
            }
        }
        private void OnApplicationQuit()
        {
            udpServer.Close();
            //service.Close();
            threadReceive.Abort();
        }
    }
}