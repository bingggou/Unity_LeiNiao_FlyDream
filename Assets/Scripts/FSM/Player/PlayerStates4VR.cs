using System.Collections.Generic;
using UnityEngine;
namespace FSM.AboutPlayer
{

    public class State_TwoHandsItemGrab : PlayerState4VR
    {
        //TwoHandsItemGrab
        HandItem grabedItemL;
        HandItem grabedItemR;

        Transform leftHandTrans;
        Transform rightHandTrans;

        TriggerTest leftTriggerTest;
        TriggerTest rightTriggerTest;

        public override void Ini()
        {
            base.Ini();

            leftTriggerTest = leftUiRay.transform.GetComponent<TriggerTest>();
            leftHandTrans = leftTriggerTest.transform;
            rightTriggerTest = rightUiRay.transform.GetComponent<TriggerTest>();
            rightHandTrans = rightTriggerTest.transform;

        }

        public override void OnGetState()
        {
            base.OnGetState();
        }

        public override void Update()
        {
            base.Update();
            CheckGrab();

        }


        public override void OnLostState()
        {
            base.OnLostState();
            if (grabedItemL != null)
            {
                grabedItemL.OnDragEnd();
            }
            if (grabedItemR != null)
            {
                grabedItemR.OnDragEnd();
            }
        }

        void CheckGrab()
        {
            if (DevicesManager.Instance.leftPadInfo.isGripButtonDown)
            {
                if (grabedItemL == null)
                {
                    if (leftTriggerTest.triggeredObjs.Count != 0)
                    {
                        grabedItemL = leftTriggerTest.triggeredObjs[0].GetComponent<HandItem>();
                        if (grabedItemL == grabedItemR)
                        {
                            if (leftTriggerTest.triggeredObjs.Count > 1)
                            {
                                grabedItemL = leftTriggerTest.triggeredObjs[1].GetComponent<HandItem>();
                            }
                            else
                            {
                                grabedItemR = null;
                            }
                        }
                        grabedItemL.OnDragStart(leftHandTrans, grabedItemL.leftHandPR);
                    }
                }
            }
            else if (DevicesManager.Instance.leftPadInfo.isGripButtonUp)
            {
                if (grabedItemL != null)
                {
                    grabedItemL.OnDragEnd();
                    grabedItemL = null;
                }
            }

            if (DevicesManager.Instance.rightPadInfo.isGripButtonDown)
            {
                if (grabedItemR == null)
                {
                    if (rightTriggerTest.triggeredObjs.Count != 0)
                    {
                        grabedItemR = rightTriggerTest.triggeredObjs[0].GetComponent<HandItem>();
                        if (grabedItemL == grabedItemR)
                        {
                            if (rightTriggerTest.triggeredObjs.Count > 1)
                            {
                                grabedItemR = rightTriggerTest.triggeredObjs[1].GetComponent<HandItem>();
                            }
                            else
                            {
                                grabedItemL = null;
                            }
                        }
                        grabedItemR.OnDragStart(rightHandTrans, grabedItemR.rightHandPR);
                    }
                }
            }
            else if (DevicesManager.Instance.rightPadInfo.isGripButtonUp)
            {
                if (grabedItemR != null)
                {
                    grabedItemR.OnDragEnd();
                    grabedItemR = null;
                }
            }
        }
    }

    public class State_TwoHandsUiClick : PlayerState4VR
    {
        //TwoHandsUiClick

        public override void OnGetState()
        {
            base.OnGetState();
            leftUiRay.enabled = true;
            rightUiRay.enabled = true;
        }

        public override void OnLostState()
        {
            base.OnLostState();
            leftUiRay.enabled = false;
            rightUiRay.enabled = false;
        }
    }


    public class State_Attack : PlayerState4VR
    {
        //TwoHandsItemGrab
        HandItem grabedItemL;
        HandItem grabedItemR;

        Transform leftHandTrans;
        Transform rightHandTrans;

        TriggerTest leftTriggerTest;
        TriggerTest rightTriggerTest;
        ColdTimeMachine cdMachine;

        //Weapon nowWeapon;

        public List<GameObject> leftHandsObjects = new List<GameObject>();

        public List<GameObject> rightHandsObjects = new List<GameObject>();

        protected GameObject weaponPoint;

        public override void Ini()
        {
            base.Ini();

            leftTriggerTest = leftUiRay.transform.GetComponent<TriggerTest>();
            leftHandTrans = leftTriggerTest.transform;
            rightTriggerTest = rightUiRay.transform.GetComponent<TriggerTest>();
            rightHandTrans = rightTriggerTest.transform;
            GetHandObj(leftHandTrans, leftHandsObjects);
            GetHandObj(rightHandTrans, rightHandsObjects);
            weaponPoint.SetActive(false);

            cdMachine = new ColdTimeMachine(0);
        }

        private void GetHandObj(Transform handTrans, List<GameObject> handObjs)
        {
            for (int i = 0; i < handTrans.childCount; i++)
            {
                Transform tempTrans = handTrans.GetChild(i);
                if (tempTrans.name == "WeaponPoint")
                {
                    weaponPoint = tempTrans.gameObject;
                }
                else
                {
                    handObjs.Add(handTrans.GetChild(i).gameObject);
                }
            }
        }

        private void HandObjActive(List<GameObject> handObjs, bool ifActive)
        {
            for (int i = 0; i < handObjs.Count; i++)
            {
                handObjs[i].SetActive(ifActive);
            }
        }

        private void CloseNoNeedObject()
        {
            HandObjActive(leftHandsObjects, false);
            HandObjActive(rightHandsObjects, false);
            leftTriggerTest.enabled = false;
            rightTriggerTest.enabled = false;
        }

        private void OpenNoNeedObject()
        {
            HandObjActive(leftHandsObjects, true);
            HandObjActive(rightHandsObjects, true);
            leftTriggerTest.enabled = true;
            rightTriggerTest.enabled = true;
        }

        float lastYvalue;

        public override void Update()
        {
            base.Update();

            float tempY = DevicesManager.Instance.leftPadInfo.joyStickValue.y;
            tempY = Input.GetAxis("Horizontal");
            if (tempY != 0)
            {
                if (lastYvalue == 0)
                {
                    if (tempY > 0)
                    {
                        Player.Instance.OnReduceIndex();
                    }
                    else
                    {
                        Player.Instance.OnAddIndex();
                    }
                }
            }

            lastYvalue = tempY;
          
           
        }

        public override void OnGetState()
        {
            base.OnGetState();
            CloseNoNeedObject();
         
            weaponPoint.SetActive(true);
            lastYvalue = 0;         
        }

        public override void OnLostState()
        {
            base.OnLostState();
            OpenNoNeedObject();
            weaponPoint.SetActive(false);
        }

    }

}