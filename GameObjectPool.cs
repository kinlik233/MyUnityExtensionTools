using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUnityExtensionTools
{
	/// <summary>
	/// 游戏对象池
	/// </summary>
	public class GameObjectPool : MonoSingleton<GameObjectPool> 
	{
        //1.对象池 
        private Dictionary<string, List<GameObject>> cache;

        private new void Awake()
        {
            base.Awake();
            cache = new Dictionary<string, List<GameObject>>(); 
        }

        /// <summary>
        /// 通过对象池创建对象
        /// </summary>
        /// <param name="key">类别</param>
        /// <param name="prefab">预制件</param>
        /// <param name="pos">位置</param>
        /// <param name="dir">旋转方向</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion dir)
        {
            GameObject tempGO = FindUsableObject(key);
            if (tempGO == null)
            {
                //创建游戏对象
                tempGO = Instantiate(prefab);
                AddCache(key, tempGO);
            }
            //使用
            UseObject(pos, dir, tempGO);
            return tempGO;
            /*
             重构提取方法快捷键：Ctrl + .   +   Enter
                                           Ctrl + R + M
             */
        }

        private static void UseObject(Vector3 pos, Quaternion dir, GameObject tempGO)
        { 
            tempGO.transform.position = pos;
            tempGO.transform.rotation = dir;
            tempGO.SetActive(true);
            //……
            //调用枪的计算目标方法
            //tempGO.GetComponent<ns.Bullet>().计算目标();
            //tempGO.GetComponent<IResetable>().OnReset();
            //获取物体上所有需要被重置的对象
            foreach (var item in tempGO.GetComponents<IResetable>())
            {
                item.OnReset();
            }  
        }

        private void AddCache(string key, GameObject tempGO)
        {
            if (!cache.ContainsKey(key))
                cache.Add(key, new List<GameObject>());
            //加入池中
            cache[key].Add(tempGO);
        }

        private GameObject FindUsableObject(string key)
        {
            //GameObject tempGO = null;
            //if (cache.ContainsKey(key))
            //    tempGO = cache[key].Find(go => !go.activeInHierarchy);
            //return tempGO; 
            return cache.ContainsKey(key) ? cache[key].Find(go => !go.activeInHierarchy) : null;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="delay"></param>
        public void CollectObject(GameObject go, float delay = 0)
        {
            StartCoroutine(DelayCollectObject(go, delay));
        }

        private IEnumerator DelayCollectObject(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }

        /// <summary>
        /// 清空某一类别
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            //销毁游戏对象
            foreach (var item in cache[key])
            {
                Destroy(item);
            }

            //销毁字典中的记录
            cache.Remove(key);
        }

        /// <summary>
        /// 清除全部
        /// </summary>
        public void ClearAll()
        {
            //foreach (var item in cache.Keys)//异常：无效的操作
            //{
            //    Clear(item);
            //}

            //cache.Keys  -->   临时列表
            List<string> keys = new List<string>(cache.Keys);
            //遍历列表
            foreach (var item in keys)//异常：无效的操作
            {
                Clear(item);//移除字典记录   内部：  cache.Remove(key);
            }
        }
    }

    public interface IResetable
    {
        void OnReset();
    }
}
