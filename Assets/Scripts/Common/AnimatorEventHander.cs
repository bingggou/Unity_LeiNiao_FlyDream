using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEventHander : MonoBehaviour
{
    public UnityEvent OnAttackEventHandler;//攻击事件
    void OnAttack() { OnAttackEventHandler.Invoke(); }//攻击事件

    public UnityEvent OnAttack2EventHandler;//攻击事件
    void OnAttack2() { OnAttack2EventHandler.Invoke(); }//攻击事件



    // Start is called before the first frame update
    // private AnimationClip[] clips;

    //public UnityEvent OnMoveEventHandler;  //移动
    // public UnityEvent OnRunEventHandler;   //跑动
    //public UnityEvent OnLookEventHandler;  //看
    // public UnityEvent OnDeadEventHandler;  //死亡
    //public UnityEvent OnBreathEventHandler;//呼吸
    // public UnityEvent OnBoomEventHandler;  //爆炸

    //void OnMove() { OnMoveEventHandler.Invoke(); }//移动
    //void OnRunBack() { OnRunEventHandler.Invoke(); }//跑动
    // void OnLookBack() { OnLookEventHandler.Invoke(); }//看
    //void OnDead() { OnDeadEventHandler.Invoke(); }//死亡



    //public void SetAnimatorEvent(Animator _animator, int _index, float _time, string _backFunction)
    //{
    //    clips = _animator.runtimeAnimatorController.animationClips;
    //    foreach (var item in clips)
    //    {
    //        Debug.Log(item.name + ";;;;;;;;;;;;;");
    //    }

    //    AnimationClip _clip = clips[_index];
    //    AddAnimationEvent(_animator, _clip.name, _backFunction, _clip.length * _time);
    //}

    //private void AddAnimationEvent(Animator _animator, string _clipname, string _eventFunctionName, float _time)
    //{
    //    AnimationClip[] _clips = _animator.runtimeAnimatorController.animationClips;
    //    for (int i = 0; i < _clips.Length; i++)
    //    {
    //        if (_clips[i].name == _clipname)
    //        {
    //            AnimationEvent _event = new AnimationEvent();
    //            _event.functionName = _eventFunctionName;
    //            _event.time = _time;
    //            _clips[i].AddEvent(_event);
    //            break;
    //        }
    //    }
    //    _animator.Rebind();
    //}
}
