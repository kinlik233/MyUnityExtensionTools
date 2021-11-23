using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	/// <summary>
	/// 动画事件行为
	/// </summary>
	public class AnimationEventBehaviour : MonoBehaviour
    {
        /*
         策划负责在Unity编辑器中，注册动画片段事件(取消动画、攻击)。
         程序负责定义攻击逻辑，注册到攻击事件。
         */
        private Animator anim;

        public event Action OnAttackHandler;

        public void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        //取消动画(由Unity引擎调用)
        public void OnCancelAnim(string animName)
        {
            anim.SetBool(animName, false);
        }

        //攻击(由Unity引擎调用)
        public void OnAttack()
        {
            if (OnAttackHandler != null)
            {
                OnAttackHandler();
            }
        }


    }
}
