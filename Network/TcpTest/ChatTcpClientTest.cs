using MyUnityExtensionTools;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyUnityExtensionTools
{
    ///<summary>
    ///通过tcp协议发送消息
    ///</summary>
    public class ChatTcpClientTest : MonoBehaviour
    {
        public string ip;
        public int port;
        private TcpClient tcpService;
        //创建socket对象
        private void Start()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip), port);
            //客户端绑定端口
            //tcpService = new TcpClient(localEP);
            //客户端使用随机端口
            tcpService = new TcpClient();
            var serverTest = GetComponent<ChatTcpSeverTest>();
            //var serverTest = FindObjectOfType<ChatTcpSeverTest>();


            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverTest.ip), serverTest.port);
            //与服务器建立连接
            tcpService.Connect(serverEP);
        }
        private void OnEnable()
        {
            transform.FindChildByName("Send").GetComponent<Button>().onClick.AddListener(OnSendButtonClick);
        }
        private void OnDisable()
        {
            transform.FindChildByName("Send").GetComponent<Button>().onClick.RemoveListener(OnSendButtonClick);
        }
        private void OnSendButtonClick()
        {
            string msg = transform.FindChildByName("MessageInput").GetComponent<InputField>().text;
            SendChatMessage(msg);
        }
        public void SendChatMessage(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(msg);
            //获取网络流
            NetworkStream stream = tcpService.GetStream();
            //写入数据
            stream.Write(buffer, 0, buffer.Length);
        }
        private void OnApplicationQuit()
        {
            //客户端绑定端口后，短时间内(30秒-4分钟)再次连接，提示端口占用
            //解决，客户端使用随机端口，或者让服务端先断开(先断开的一端会进入等待时间，会延迟释放端口)
            //

            //下线通知，让服务端先断开
            //SendChatMessage("Quit");
            Thread.Sleep(500);
            tcpService.Close();
        }
    }
}