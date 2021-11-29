using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using UIWidgetsSamples;
using UnityEngine;

namespace Common
{
    ///<summary>
    ///通过tcp协议发送消息
    ///</summary>
    public class ChatTcpSeverTest : MonoBehaviour
    {
        public string ip;
        public int port;
        private TcpListener listener;
        private Thread receiveThread;
        //private ChatView chatView;
        //创建监听器
        private void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new TcpListener(ep);
            listener.Start();
            receiveThread = new Thread(ReceiveChatMessage);
            receiveThread.Start();
            //chatView = transform.FindChildByName("ChatView").GetComponent<ChatView>();
        }
        //接受消息
        private void ReceiveChatMessage()
        {
            //接收客户端
            //如果没有监听到客户端连接，则阻塞线程
            //监听到则继续执行
            TcpClient client = listener.AcceptTcpClient();
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[512];
                int count;
                //如果读取到数据，则解析
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //读取数据
                    //返回值表示实际读取到的字节数,如果为0表示客户端下线
                    //如果没有读取到数据，则阻塞线程
                    //读取到则继续执行
                    //如果需要监听多个客户端连接，则需要再开启线程                 
                    string msg = Encoding.Unicode.GetString(buffer, 0, count);
                    if (msg == "Quit") break;
                    //ThreadCrossHelper.Instance.ExecuteOnMainThread(() => { DisplayChatMessage(msg); });
                }
            }
        }
        //private void DisplayChatMessage(string msg)
        //{
        //    ChatLine line = new ChatLine()
        //    {
        //        UserName = "kinlik",
        //        Message = msg,
        //        Time = DateTime.Now,
        //        Type = ChatLineType.User,
        //    };

        //    chatView.DataSource.Add(line);

        //}
        //关闭连接
        private void OnApplicationQuit()
        {
            listener.Stop();
            receiveThread.Abort();
        }
    }
}