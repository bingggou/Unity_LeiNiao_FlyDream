using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSM
{
    public abstract class FSM_Trigger
    {
        protected Transform target;
        protected FSM_Object targetFsm;
        public List<string> otherTriggers=new List<string>();

        public virtual void Ini(Transform aiTrans)
        {
            target = aiTrans;
            targetFsm = aiTrans.GetComponent<FSM_Object>();

        }
        public abstract bool Testing();         
    }
}