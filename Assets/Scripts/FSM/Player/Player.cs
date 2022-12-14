using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.AboutPlayer
{
    public class Player : FSM_GameObject<PlayerStateType, PlayerTriggerType>
    {
        [Header("所有状态设置")]
        public StateInfo[] StateSetting;
        
        [Header("视野旋转速度")]
        public float viewRotateSpeed = 10;
        
     

        public MusicManager playSound;

        public static Player Instance;

        public Rigidbody rigBody;
        Transform weaponPoint;
        public string[] weaponArr = new string[] { "JiQiang", "JiPao", "JiaTeLing", "HandGun" };
        private int weaponIndex = 0;
        private GameObject weaponObject;
     
        [HideInInspector]
        public Transform DaoDanPoint;



        public enum GameEffectType
        {
            Game_None,
            Game_Success,
            Game_Lose,
            Game_Click,
            Game_UI_Close,
            Game_UI_Change,
            Game_UI_Open,
            Game_JiPaoEmpty,
            Game_JiQiangEmpty,
            Game_Ex_JiPao,
            Game_Ex_JiQiang,
            Game_Pick_Goods,
            Game_BeHited,
        }       

        public GameEffectType gameEffectType;
        protected override void IniData()
        {
            base.IniData();

            DaoDanPoint = TransformHelper.GetChild(transform, "DaoDanPoint");
         
            Collider Collider = transform.GetComponentInChildren<Collider>();
            TriggerTest tTest = Collider.gameObject.GetComponent<TriggerTest>();
            if (tTest == null)
            {
                tTest = Collider.gameObject.AddComponent<TriggerTest>();
            }
            tTest.targetFsm = this;
            tTest.OnTriggerEnterEvent += OnTouchSth;



            rigBody = transform.GetComponent<Rigidbody>();
        
            weaponPoint = TransformHelper.GetChild(transform, "WeaponPoint");

            //get child of Audio  获取声音组件
      

       

            if (Instance == null)
            {
                Instance = this;
               
            }
            nameSpaceStr = "FSM.AboutPlayer.";


            foreach (StateInfo item in StateSetting)
            {

                StateWithTriggers<PlayerStateType, TriggerInfoBase> temp = new StateWithTriggers<PlayerStateType, TriggerInfoBase>();



                temp.stateType = item.state;
                temp.fsmName = item.note;
                temp.tsPair = new List<TriggerStatePair<PlayerStateType, TriggerInfoBase>>();

                foreach (TriggerInfo triggerInfo in item.triggers)
                {
                    TriggerInfoBase xx = new TriggerInfoBase(triggerInfo.trigger.ToString(), triggerInfo.tail);
                    TriggerStatePair<PlayerStateType, TriggerInfoBase> tempTrigger = new TriggerStatePair<PlayerStateType, TriggerInfoBase>();

                    tempTrigger.triggerInfoBase = xx;
                    tempTrigger.resultStateType = triggerInfo.resultState;
                    temp.tsPair.Add(tempTrigger);
                }
                allStateWithTriggers.Add(temp);
            }

        }

        private string getSoundEffectName()
        {
            string soundName = null;
            if (gameEffectType == GameEffectType.Game_None)
            {
                soundName = null;
                return soundName;
            }
            else if (gameEffectType == GameEffectType.Game_Success)
            {
                soundName = "成功通关";
            }
            else if (gameEffectType == GameEffectType.Game_Lose)
            {
                soundName = "游戏失败";
            }
            else if (gameEffectType == GameEffectType.Game_Click)
            {
                int romd = Random.Range(1, 3);
                soundName = "点击" + romd.ToString();
            }
            else if (gameEffectType == GameEffectType.Game_UI_Close)
            {
                soundName = "画面关闭";
            }
            else if (gameEffectType == GameEffectType.Game_UI_Open)
            {
                soundName = "画面启动";
            }
            else if (gameEffectType == GameEffectType.Game_UI_Change)
            {
                soundName = "画面切换";
            }
            else if (gameEffectType == GameEffectType.Game_JiPaoEmpty)
            {
                soundName = "机炮弹夹空";
            }
            else if (gameEffectType == GameEffectType.Game_JiQiangEmpty)
            {
                soundName = "机枪弹夹空";
            }
            else if (gameEffectType == GameEffectType.Game_Ex_JiPao)
            {
                soundName = "切换至机炮";
            }
            else if (gameEffectType == GameEffectType.Game_Ex_JiQiang)
            {
                soundName = "切换至机枪";
            }
            else if (gameEffectType == GameEffectType.Game_Pick_Goods)
            {
                soundName = "拾取 获得物品";
            }
            else if (gameEffectType == GameEffectType.Game_BeHited)
            {
                int hitindex = Random.Range(1, 5);
                soundName = "受击" + hitindex.ToString();
            }
            return soundName;
        }

        public void PlayNowWeaponSound()
        {
            string soundName = getSoundEffectName();
          
        }

    

        public void OnAddIndex()
        {
            PoolTool.PutInPool(weaponObject);
            if ((weaponIndex + 1) >= weaponArr.Length)
            {
                weaponIndex = 0;
            }
            else
            {
                weaponIndex = weaponIndex + 1;
            }
        
        }

        public void OnReduceIndex()
        {
            PoolTool.PutInPool(weaponObject);

            if ((weaponIndex - 1) < 0)
            {
                weaponIndex = weaponArr.Length - 1;
            }
            else
            {
                weaponIndex = weaponIndex - 1;
            }
           
        }


        private void OnTriggerEnter(Collider other)
        {
            OnTouchSth(other);
        }

        private void OnTouchSth(Collider other)
        {

            
        }

       


    }

    [System.Serializable]
    public class StateInfo
    {
        public PlayerStateType state;
        public string note = "";

        public TriggerInfo[] triggers;
    }
    [System.Serializable]
    public class TriggerInfo
    {
        public PlayerTriggerType trigger;
        public PlayerStateType resultState;
        public string tail = "";
        public PlayerTriggerType[] otherTriggers;
    }

}