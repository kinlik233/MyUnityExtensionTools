using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUnityExtensionTools
{
	/// <summary>
	/// 变换组件助手类
	/// </summary>
	public static class TransformHelper
	{
        /// <summary>
        /// 注视目标方向旋转
        /// </summary>
        /// <param name="currentTF">当前物体变换组件引用</param>
        /// <param name="direction">需要注视的方向</param>
        /// <param name="rotateSpeed">旋转速度</param>
        public static void LookDirection(this Transform currentTF, Vector3 direction,float rotateSpeed)
        {
            if (direction == Vector3.zero) return; 
            var targetDir = Quaternion.LookRotation(direction);
            currentTF.rotation = Quaternion.Lerp(currentTF.rotation, targetDir, rotateSpeed * Time.deltaTime);
        }
        
        /// <summary>
        /// 注视目标点旋转
        /// </summary>
        /// <param name="currentTF">当前物体变换组件引用</param>
        /// <param name="direction">需要注视的点</param>
        /// <param name="rotateSpeed">旋转速度</param>
        public static void LookPostion(this Transform currentTF, Vector3 position, float rotateSpeed)
        {
            Vector3 direction = position - currentTF.position;
            currentTF.LookDirection(direction, rotateSpeed);
        }

        /// <summary>
        /// 未知层级查找子物体
        /// </summary>
        /// <param name="currentTF">当前物体变换组件</param>
        /// <param name="childName">子物体名称</param>
        /// <returns></returns>
        public static Transform FindChildByName(this Transform currentTF, string childName)
        { 
            Transform childTF = currentTF.Find(childName);
            if (childTF != null) return childTF;
            //将任务交给子物体
            for (int i = 0; i < currentTF.childCount; i++)
            {
                /*
                 调用过程：……
                 返回过程：……
                 */
                childTF = currentTF.GetChild(i).FindChildByName(childName);
                if (childTF != null) return childTF;
            }
            return null;
        }

        /// <summary>
        /// 查找周围物体
        /// </summary>
        /// <param name="currentTF">当前物体变换组件</param>
        /// <param name="distance">查找距离</param>
        /// <param name="angle">查找角度</param>
        /// <param name="tags">目标标签</param>
        /// <returns></returns>
        public static Transform[] GetAroundObject(this Transform currentTF, float distance, float angle, string[] tags)
        {
            //1.查找所有目标物体
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < tags.Length; i++)
            {
                GameObject[] tempGOArr = GameObject.FindGameObjectsWithTag(tags[i]);
                Transform[] tempTFArr = tempGOArr.Select(g => g.transform);
                list.AddRange(tempTFArr);
            }
            //2.筛选 
            list = list.FindAll(tf =>
                Vector3.Distance(currentTF.position, tf.transform.position) <= distance &&
                Vector3.Angle(currentTF.forward, tf.position - currentTF.position) <= angle / 2
            );
            return list.ToArray(); 
        }
    }
}
