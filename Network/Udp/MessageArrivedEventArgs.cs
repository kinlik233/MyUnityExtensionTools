using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    ///<summary>
    ///消息事件参数类
    ///</summary>
    public class MessageArrivedEventArgs : EventArgs
    {
        public ChatMessage Message { get; set; }
        public DateTime MessageTime { get; set; }
    }
}