using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Main
{
    ///<summary>
    ///聊天消息
    ///</summary>
    public class ChatMessage 
    {
        public MessageType Type { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }


        #region 仅适合同构平台
        ////序列化：将对象状态存储到相应存储介质(文件、内存、网络)中的过程。
        ////反序列化：取出存储介质中的对象状态的过程。
        //public byte[] ObjectToBytes()
        //{
        //    //object  --->  MemoryStream  --->  byte[]
        //    //          序列化
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        bf.Serialize(ms, this);//将对象存储到内存流中
        //        return ms.ToArray();
        //    }
        //}

        //public static ChatMessage BytesToObject(byte[] bts)
        //{
        //    //byte[]  --->  MemoryStream  --->  object
        //    //                                           反序列化
        //    using (MemoryStream ms = new MemoryStream(bts))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        //取出内存流中的对象
        //        return bf.Deserialize(ms) as ChatMessage;
        //    }
        //}
        #endregion

        #region 支持异构平台
        /// <summary>
        /// 对象转换为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ObjectToBytes()
        {
            //string/int/bool-->二进制写入器BinaryWriter-->内存流MemoryStream-->byte[]
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                WriteString(writer, Type.ToString());
                WriteString(writer, SenderName);
                WriteString(writer, Content);
                return stream.ToArray();
            }
        }

        //将字符串对象写入流中
        private void WriteString(BinaryWriter writer, string str)
        {
            //编码
            byte[] typeBTS = Encoding.Unicode.GetBytes(str);
            //写入长度
            writer.Write(typeBTS.Length);
            //写入内容
            writer.Write(typeBTS);
        }

        /// <summary>
        /// 字节数组转换为对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatMessage BytesToObject(byte[] bytes)
        {
            ChatMessage obj = new ChatMessage();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryReader reader = new BinaryReader(stream);
                //读取按照写入格式
                string strType = ReadString(reader);
                obj.Type = (MessageType)Enum.Parse(typeof(MessageType), strType);
                obj.SenderName = ReadString(reader);
                obj.Content = ReadString(reader);
                return obj;
            }
        }

        private static string ReadString(BinaryReader reader)
        {
            //读取长度
            int typeLength = reader.ReadInt32();
            //读取内容
            byte[] typeBTS = reader.ReadBytes(typeLength);
            //
            return Encoding.Unicode.GetString(typeBTS);

        }
        #endregion
    }
}