using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUnityExtensionTools
{
    /// <summary>
    /// 脚本单例类
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        //T 表示子类类型
        //按需加载
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //在场景中查找指定类型引用
                    instance = FindObjectOfType<T>();
                    //如果没有，则自行创建
                    if (instance == null)
                    {
                        //当对象被创建时 立即调用Awake方法
                        new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                    }
                    else
                    { 
                          instance.Init();                        
                    }
                }
                return instance;
            }
        }

        protected void Awake()
        {//this  父类型引用 指向 子类对象  
            if (instance == null)
            {
                instance = this as T;
                Init();
            }
        }

        public virtual void Init()
        {
            
        }
    }
}
