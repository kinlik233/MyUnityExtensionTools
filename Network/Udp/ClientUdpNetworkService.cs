using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Main
{
    ///<summary>
    ///Udp客户端网络服务
    ///</summary>

    #region
    public class ClientUdpNetworkService : MonoSingleton<ClientUdpNetworkService>
    {
        private UdpClient udpServer;
        public event EventHandler<MessageArrivedEventArgs> MessageArrivedHandler;
        private Thread threadReceive;

        //由登录窗口传递服务端端地址、端口
        public void Initialized(string ip,int port)
        {
            DontDestroyOnLoad(gameObject);
            //随机分配可以使用的端口
            udpServer = new UdpClient();
            //与服务端建立连接，只配置自身Socket
            IPEndPoint remote = new IPEndPoint(IPAddress.Parse(ip),port);
            udpServer.Connect(remote);

            //connect报错bug，连接失败时报错
            //const uint IOC_IN = 0x80000000;
            //int IOC_VENDOR = 0x18000000;
            //int SIO_UDP_CONNRESET = (int)(IOC_IN | IOC_VENDOR | 12);
            //udpServer.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
 
            threadReceive = new Thread(ReceiveChatMessage);
            threadReceive.Start();

            NotifyServer(MessageType.Online);
        }

        //上下线状态通知
        public void NotifyServer(MessageType type)
        {
            SendChatMessage(new ChatMessage() { Type = type, SenderName = DoGetHostEntry(), Content = "" });
        }
        public void SendChatMessage(ChatMessage msg)
        {
            byte[] buffer = msg.ObjectToBytes();
            //发送时不能指定终端，已和服务端建立了连接
            int count = udpServer.Send(buffer, buffer.Length);
        }

        private void ReceiveChatMessage()
        {

            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {

                byte[] data = udpServer.Receive(ref remote);
                ChatMessage msg = ChatMessage.BytesToObject(data);
                if (MessageArrivedHandler == null) continue;
                MessageArrivedEventArgs args = new MessageArrivedEventArgs()
                {
                    Message = msg,
                    MessageTime = DateTime.Now
                };
                ThreadCrossHelper.Instance.ExecuteOnMainThread(() => { MessageArrivedHandler?.Invoke(this, args); });
            }
        }

        /// 获得本机IP地址
        public static string DoGetHostEntry()
        {
            System.Net.IPHostEntry IpEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            string localhostipv4Address = "";

            for (int i = 0; i != IpEntry.AddressList.Length; i++)
            {
                if (!IpEntry.AddressList[i].IsIPv6LinkLocal)
                {
                    localhostipv4Address = IpEntry.AddressList[i].ToString();
                    break;
                }
            }

            return localhostipv4Address;
        }

        private void OnApplicationQuit()
        {
            NotifyServer(MessageType.Offline);
            udpServer.Close();
            //service.Close();
            threadReceive.Abort();
        }
    }
    #endregion


}