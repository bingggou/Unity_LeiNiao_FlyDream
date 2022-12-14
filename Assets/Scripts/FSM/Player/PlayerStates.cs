using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace FSM.AboutPlayer
{
    public enum PlayerStateType
    {
        LostControll,
        Talk,
        Dead,
        Attack,
        TwoHandsUiClick,
        TwoHandsItemGrab
    }


    public class PlayerState : FSM_State
    {
        protected Player targetPlayer;
        protected Rigidbody rigBody;

        protected virtual void ControlMove()
        {

        }
        public override void FixedUpdate()
        {

        }

        public override void Ini()
        {

            targetPlayer = target.GetComponent<Player>();
            Debug.Log(targetPlayer);
            rigBody = targetPlayer.rigBody;
        }

        public override void LateUpdate()
        {

        }

        public override void OnGetState()
        {
        }

        public override void OnLostState()
        {
        }

        public override void Update()
        {
            // ControlMove();           
        }


        protected void Move(float h, float v)
        {

            Vector3 aimVocity = (target.right * h + target.forward * v);
            aimVocity = aimVocity * Time.deltaTime * 20f;
            //aimVocity.y = 0;
            //rigBody.velocity = aimVocity;
            target.position += aimVocity;
        }
    }

    public class PlayerState4PC : PlayerState
    {

        protected Camera mainCamera;
        protected Transform mainCameraTrans;

        public override void Ini()
        {
            base.Ini();
            mainCamera = Camera.main;
            mainCameraTrans = mainCamera.transform;
        }


    }

    public class PlayerState4PC_FirstPerson : PlayerState4PC
    {
        private float xLeft = 60;
        private float xRight = 300;


        public override void Update()
        {
            base.Update();
            MouseCamera();
        }

        //镜头函数
        private void MouseCamera()
        {
            //if (!Input.GetMouseButton(1))
            //{
            //    return;
            //}
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            if (x != 0 || y != 0)
            {
                mainCameraTrans.Rotate(Vector3.up, targetPlayer.viewRotateSpeed * x * Time.deltaTime, Space.World);
                float addValue = -targetPlayer.viewRotateSpeed * y * Time.deltaTime;
                float xTemp = mainCameraTrans.eulerAngles.x + addValue;

                xTemp = xTemp % 360;
                if (xTemp < 0)
                {
                    xTemp = 360 + xTemp;
                }

                if (!(xTemp > xLeft && xTemp < xRight))
                {
                    Vector3 aimEuler = mainCameraTrans.localEulerAngles;
                    aimEuler.x = xTemp;
                    mainCameraTrans.localEulerAngles = aimEuler;
                }

                //mainCameraTrans.Rotate(Vector3.right, -player.viewRotateSpeed * y * Time.deltaTime, Space.Self);
            }
        }

        protected override void ControlMove()
        {

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            //Move(h, v);
            Vector3 rightDir = new Vector3(mainCameraTrans.forward.x, 0, mainCameraTrans.forward.z);
            Vector3 dir = (mainCameraTrans.right * h + rightDir * v);

            targetPlayer.transform.position += dir * 20f * Time.deltaTime;

        }

        public override void OnGetState()
        {
            base.OnGetState();
            // Cursor.visible = false;
        }


        public override void OnLostState()
        {
            base.OnLostState();
            // Cursor.visible = true;
        }

    }

    public class PlayerState4VR : PlayerState
    {

        protected XRRayInteractor leftUiRay;
        protected XRRayInteractor rightUiRay;

        public override void Ini()
        {
            base.Ini();
            Transform leftHand = TransformHelper.GetChild(targetPlayer.transform, "LeftHand Controller");
            leftUiRay = leftHand.GetComponent<XRRayInteractor>();
            rightUiRay = TransformHelper.GetChild(targetPlayer.transform, "RightHand Controller").GetComponent<XRRayInteractor>();
        }

        public override void Update()
        {
            base.Update();
            //RotateView();
        }

        protected virtual void RotateView()
        {
            Vector2 joyStickR = DevicesManager.Instance.rightPadInfo.joyStickValue;
            float h = joyStickR.x;

            target.Rotate(target.up, h * Time.deltaTime * targetPlayer.viewRotateSpeed);
        }

        protected override void ControlMove()
        {
            Vector2 joyStickR = DevicesManager.Instance.leftPadInfo.joyStickValue;
            float h = joyStickR.x;
            float v = joyStickR.y;
            Move(h, v);
        }

    }

}