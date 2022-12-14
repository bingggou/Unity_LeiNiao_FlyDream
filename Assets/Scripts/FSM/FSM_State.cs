using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class FSM_State
    {
        public Transform target;

        public abstract void Ini();
        public abstract void OnGetState();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void LateUpdate();
        public abstract void OnLostState();
    }
}