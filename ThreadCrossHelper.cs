using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    ///<summary>
    ///线程交叉访问助手
    ///</summary>
    public class ThreadCrossHelper : MonoSingleton<ThreadCrossHelper>
    {
        class DelayedItem
        {
            public Action CurrentAction { get; set; }
            public DateTime Time { get; set; }
        }
        private List<DelayedItem> actionList;

        public override void Init()
        {
            base.Init();
            actionList = new List<DelayedItem>();
        }

        //主线程中执行
        private void Update()
        {
            lock (actionList)
            {
                for (int i = actionList.Count - 1; i >= 0; i--)
                {
                    if (actionList[i].Time <= DateTime.Now)
                    {
                        actionList[i].CurrentAction();
                        actionList.RemoveAt(i);
                    }
                }
            }

        }
        //辅助线程中执行，把方法交给主线程
        public void ExecuteOnMainThread(Action action, float delay = 0)
        {
            lock (actionList)
            {
                var item = new DelayedItem()
                {
                    CurrentAction = action,
                    Time = DateTime.Now.AddSeconds(delay)
                };
                actionList.Add(item);
            }

        }
    }
}