using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSM.AboutPlayer
{
    public enum PlayerTriggerType
    {        
        AXButtonDown
    }
    public class PlayerTrigger : FSM_Trigger
    {     
        public override bool Testing()
        {
            return false;
        }
    }



    public class Trigger_AXButtonDown : PlayerTrigger
    {
        public override bool Testing()
        {
            if (DevicesManager.Instance.rightPadInfo.isAXButtonDown || DevicesManager.Instance.leftPadInfo.isAXButtonDown)
            {
                return true;
            }
            else
            {

            return false;
            }
        }
    }
}