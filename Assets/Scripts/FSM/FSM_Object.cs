using System;
using System.Collections;
using System.Collections.Generic;
using FSM.AboutPlayer;
using UnityEngine;
namespace FSM
{
    public abstract class FSM_Object : MonoBehaviour
    {

      

       


        protected virtual void IniData()
        {
            
          
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }


        public virtual void Reborn()
        {
          
           
        }

        public virtual void GetHurt(int reduceValue)
        {
          
        }
    }


    public  class FSM_GameObject<T_StateType, T_TriggerType> : FSM_Object
    {

        //命名空间头设置，FSM.Aboutxxx.
        [HideInInspector]
        public string nameSpaceStr = "";
        [Header("初始默认状态")]
        public T_StateType nowState;
        [HideInInspector]
        [Header("当前状态所经过的2种时间")]
        public PassingTime passingTime;
        private StateWithTriggers<T_StateType, TriggerInfoBase> nowStateWithTriggers;
        public T_StateType NowState
        {
            set
            {
                if (nowState.ToString() != value.ToString())
                {
                    nowStateWithTriggers.stateEntity.OnLostState();
                    OnStateChange(nowState, value);
                    nowState = value;
                    nowStateWithTriggers = allStates[nowState];
                    nowStateWithTriggers.stateEntity.OnGetState();
                }
            }
            get { return nowState; }
        }

        protected virtual void OnStateChange(T_StateType lastType, T_StateType nowType)
        {

        }


        protected List<StateWithTriggers<T_StateType, TriggerInfoBase>> allStateWithTriggers = new List<StateWithTriggers<T_StateType, TriggerInfoBase>>();




        public Dictionary<T_StateType, StateWithTriggers<T_StateType, TriggerInfoBase>> allStates = new Dictionary<T_StateType, StateWithTriggers<T_StateType, TriggerInfoBase>>();

       
        protected Dictionary<string, FSM_Trigger> allTriggers = new Dictionary<string, FSM_Trigger>();

        protected virtual void Start()
        {
        
            nowStateWithTriggers.stateEntity.OnGetState();
        }

        List<T_StateType> aimStateTemps = new List<T_StateType>();
        private void Update()
        {
            nowStateWithTriggers.stateEntity.Update();

            aimStateTemps.Clear();
            //StateWithTriggers<T_StateType, TriggerInfo<T_TriggerType>>
            //TriggerStatePair<TriggerInfo<T_TriggerType>, T_StateType>
            foreach (TriggerStatePair<T_StateType, TriggerInfoBase> item in nowStateWithTriggers.tsPair)
            {
                
                if (item.triggerEntity.Testing())
                {
                    //NowState = item.resultStateType;
                    //break;
                    if (item.resultStateType.ToString() == "Dead")
                    {
                        NowState = item.resultStateType;
                        return;
                    }
                    aimStateTemps.Add(item.resultStateType);
                }
            }
            if (aimStateTemps.Count != 0)
            {
                int aimIndex = UnityEngine.Random.Range(0, aimStateTemps.Count);
                NowState = aimStateTemps[aimIndex];
            }

            MoreUpdate();
        }
        public virtual void MoreUpdate()
        {

        }

        private void FixedUpdate()
        {
            nowStateWithTriggers.stateEntity.FixedUpdate();
        }

        private void LateUpdate()
        {
            nowStateWithTriggers.stateEntity.LateUpdate();
        }


       

        private void Awake()
        {
            //赋值给allTriggers,allStateWithTriggers
            IniData();

            passingTime = new PassingTime();
            foreach (StateWithTriggers<T_StateType, TriggerInfoBase> item in allStateWithTriggers)
            {
                
                if (!allStates.ContainsKey(item.stateType))
                {
                    allStates.Add(item.stateType, item);
                   
                }
                else
                {
                    Debug.LogError("错误:状态重复-----" + transform.name);
                }
            }

            nowStateWithTriggers = allStates[NowState];


            foreach (StateWithTriggers<T_StateType, TriggerInfoBase> item in allStateWithTriggers)
            {              
                item.Ini(allTriggers, transform, nameSpaceStr);
            }
        }
    }

    [System.Serializable]
    public class PassingTime
    {
        //当前状态所经过的2种时间  
        public float uPassTime = 0;
        public float fuPassTime = 0;

        public void ResetTime()
        {
            uPassTime = 0;
            fuPassTime = 0;

        }
        public PassingTime()
        {
            ResetTime();
        }

    }
    public class StateWithTriggers<T_StateType, T_TriggerType>
    {
        public T_StateType stateType;
        public string fsmName;
        public FSM_State stateEntity;
        public List<TriggerStatePair<T_StateType, T_TriggerType>> tsPair = new List<TriggerStatePair<T_StateType, T_TriggerType>>();
        public void Ini(Dictionary<string, FSM_Trigger> triggerDic, Transform target, string nameSpaceStr)
        {

            string stateClassName = "State_" + stateType.ToString();
            if (fsmName != "")
            {
                stateClassName += "_" + fsmName;
            }
            stateClassName = nameSpaceStr + stateClassName;
            Type aimType = Type.GetType(stateClassName);

            Debug.Log(stateClassName);
            stateEntity = aimType.Assembly.CreateInstance(stateClassName) as FSM_State;
            stateEntity.target = target;
            Debug.Log(stateEntity);
            stateEntity.Ini();
            foreach (var item in tsPair)
            {
               // item.triggerEntity.otherTriggers=item.triggerInfoBase.otherTypes;
                item.Ini(triggerDic, target, nameSpaceStr);
            }
        }
    }



    public class TriggerStatePair<T_StateType, T_TriggerType>
    {
        public TriggerInfoBase triggerInfoBase;
        public FSM_Trigger triggerEntity;
        public T_StateType resultStateType;
        public List<string> otherTriggerNames=new List<string>();

        public void Ini(Dictionary<string, FSM_Trigger> triggerDic, Transform aiTrans, string nameSpaceStr)
        {
            if (triggerEntity == null)
            {
                //if (triggerDic.ContainsKey(TriggerType))
                //{
                //    triggerEntity = triggerDic[TriggerType];
                //}
                //else
                //{


                string triggerClassName = ("Trigger_" + triggerInfoBase.typeName);

                if (triggerInfoBase.tail != "")
                {
                    triggerClassName = triggerClassName + "_" + triggerInfoBase.tail;
                }

                string aimTriggerTypeStr = nameSpaceStr + triggerClassName;
                Debug.Log(aimTriggerTypeStr);
                Type aimType = Type.GetType(aimTriggerTypeStr);

                
                Debug.Log(aimType);
                triggerEntity = aimType.Assembly.CreateInstance(aimTriggerTypeStr) as FSM_Trigger;
                triggerEntity.otherTriggers = otherTriggerNames;
                Debug.Log(triggerEntity);

                triggerEntity.Ini(aiTrans);

                triggerDic.Add(triggerInfoBase.ToString() + triggerDic.Count, triggerEntity);

                // }
            }
        }
    }

    public class TriggerInfoBase
    {
        public string tail = "";
        public string typeName;



        public TriggerInfoBase(string _typeName, string _tail)
        {
            tail = _tail;
            typeName = _typeName;
        }

        
    }


}