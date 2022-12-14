
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public class CameraTransPair
{
    public Vector3 localPos;
    public Quaternion localRotation;
}

public class Player : MonoBehaviour
{


    protected int isFirstPerson = 0;
    public Toggle isChengJing;
    public Toggle isMotionBluer;
    public Volume volume;
    GameObject chengJingBKG;

    //public CameraTransPair cameraTP_ThirdPerson;
    //public CameraTransPair cameraTP_FirstPerson;
    MutiButtonAimOne viewButtons;


    [Header("目标UI画布")]
    public Transform canvas;

    [Header("log输出文本")]
    public Text logText;
    [Header("是否模拟眼镜测试")]
    public bool glassTest = false;

    protected bool isRunInGlass = true;

    private int starNum;

    protected int StarNum
    {
        get { return starNum; }
        set
        {
            starNum = value;
            UpdateStarText();
            PlayerPrefs.SetInt(nameof(starNum), starNum);
        }
    }


    [Header("初始/当前速度")]
    public float moveSpeed = 20;
    protected float startSpeed;


    [Header("失控速度,要小于MoveSpeed")]
    public float loseControllSpeed = 5;
    [Header("封顶速度,要大于moveSpeed")]
    public float maxSpeed = 60;


    [Header("空气阻力系数")]
    [Range(0, 1)]
    public float airForceRate = 0.15f;
    [Header("重力加速度系数")]
    [Range(0, 10)]
    public float downForceRate = 0.15f;


    [Header("最大身体角度")]
    public float maxBodyAngle = 80;
    [Header("最小身体角度")]
    public float minBodyAngle = -60;
    [Header("身体旋转速度")]
    [Range(0, 1)]
    public float rotateSpeed = 2;
    //[Header("身体旋转的插值速度")]
    //[Range(0, 0.2f)]
    //public float bodyAngleLerpRange = 0.1f;

    [Header("范围圆球")]
    public Transform sphere;


    public static Player instance;

    protected Rigidbody rigbody;

    float[] speed2Level = new float[2];

    private float originalLocalEulerX;

    protected Vector3 originalPos;
    protected Quaternion originalRot;

    public Transform cameraTrans;

    float disFromSphere;
    float nearOutDis;
    float outDis;
    Vector3 startPos;

    //[ContextMenu("设置第三人称位置")]
    //private void Set3()
    //{
    //    cameraTP_ThirdPerson.localPos = cameraTrans.localPosition;
    //    cameraTP_ThirdPerson.localRotation = cameraTrans.localRotation;
    //}
    //[ContextMenu("设置第一人称位置")]
    //private void Set1()
    //{
    //    cameraTP_FirstPerson.localPos = cameraTrans.localPosition;
    //    cameraTP_FirstPerson.localRotation = cameraTrans.localRotation;
    //}
    //[ContextMenu("到第三人称位置")]
    //private void In3()
    //{
    //    cameraTrans.localPosition = cameraTP_ThirdPerson.localPos;
    //    cameraTrans.localRotation = cameraTP_ThirdPerson.localRotation;
    //}
    //[ContextMenu("到第一人称位置")]
    //private void In1()
    //{
    //    cameraTrans.localPosition = cameraTP_FirstPerson.localPos;
    //    cameraTrans.localRotation = cameraTP_FirstPerson.localRotation;
    //}

     

    // Start is called before the first frame update
    protected virtual void Awake()
    {

#if UNITY_EDITOR
        if (glassTest == false)
        {
            isRunInGlass = false;

        }
        else
        {
            isRunInGlass = true;
        }

#endif
        mainUi = TransformHelper.GetChild(canvas, nameof(mainUi)).gameObject;
        viewButtons = mainUi.GetComponentInChildren<MutiButtonAimOne>(true);
        viewButtons.Start();
        gameUi = TransformHelper.GetChild(canvas, nameof(gameUi)).gameObject;
        chengJingBKG = TransformHelper.GetChild(gameUi.transform, nameof(chengJingBKG)).gameObject;

        startText = TransformHelper.GetChild(mainUi.transform, nameof(startText)).GetComponent<Text>();

        douSpeed = maxSpeed - maxSpeed * douRate;

        cameraTrans = TransformHelper.GetChild(transform, nameof(cameraTrans));

        StarNum = PlayerPrefs.GetInt(nameof(starNum));

        startPos = transform.position;
        outDis = sphere.localScale.x / 2;
        nearOutDis = outDis - 88;
        float tempSpeed = maxSpeed / 3;
        speed2Level[0] = tempSpeed;
        speed2Level[1] = tempSpeed * 2;
        startSpeed = moveSpeed;
        offsetValue = maxValue - zeroValue;
        originalPos = transform.position;
        originalRot = transform.rotation;
        rigbody = transform.GetComponent<Rigidbody>();
        originalLocalEulerX = transform.localEulerAngles.x;

        Collider[] colliders = transform.GetComponentsInChildren<Collider>();

        foreach (var item in colliders)
        {
            item.isTrigger = true;
        }




        isFirstPerson = PlayerPrefs.GetInt(nameof(isFirstPerson));
        SetView(isFirstPerson);
        viewButtons.SetAimPosByIndex(isFirstPerson);

        SetChengJing(PlayerPrefs.GetInt(nameof(isChengJing)));
        isChengJing.onValueChanged.AddListener((chengjing) =>
        {
            bool aimBool = chengjing;
            if (aimBool == false)
            {
                chengJingBKG.SetActive(true);
            }
            else
            {
                chengJingBKG.SetActive(false);
            }
        });

        bool blurTemp= PlayerPrefs.GetInt(nameof(isMotionBluer)) == 0 ? false : true; ;
        volume.profile.components[6].active = blurTemp;
        isMotionBluer.isOn = blurTemp;
        isMotionBluer.onValueChanged.AddListener((isBlur)=> {
            volume.profile.components[6].active = isBlur;
        });


        
        instance = this;
    }


    public void SetView(int type)
    {
        isFirstPerson = type;
        //if (isFirstPerson == 0)
        //{

        //    cameraTrans.localPosition = cameraTP_ThirdPerson.localPos;
        //   cameraTrans.localRotation = cameraTP_ThirdPerson.localRotation;

        //}
        //else
        //{
        //    cameraTrans.localPosition = cameraTP_FirstPerson.localPos;
        //    cameraTrans.localRotation = cameraTP_FirstPerson.localRotation;
        //}
    }


    public void SetChengJing(int type)
    {

        bool aimBool = type == 0 ? false : true;
        isChengJing.isOn = aimBool;
        if (type == 0)
        {
            chengJingBKG.SetActive(true);
        }
        else
        {
            chengJingBKG.SetActive(false);
        }
    }


    public void SaveNowSetting()
    {
        Save_isFirstPerson();
        Save_isChengJing();
        Save_isMotionBluer();
    }

    void Save_isFirstPerson()
    {
        PlayerPrefs.SetInt(nameof(isFirstPerson), isFirstPerson);
    }

    void Save_isChengJing()
    {
        PlayerPrefs.SetInt(nameof(isChengJing), isChengJing.isOn == true ? 1 : 0);
    }
    void Save_isMotionBluer()
    {
        
        PlayerPrefs.SetInt(nameof(isMotionBluer), isMotionBluer.isOn == true ? 1 : 0);
    }















    protected float h;
    protected float v;
    float zeroValue = 5;
    float maxValue = 50;
    float offsetValue;
    float douSpeed;

    float douRate = 0.3f;



    protected GameObject mainUi;
    protected GameObject gameUi;
    protected Text startText;




    protected void UpdateStarText()
    {
        startText.text = StarNum.ToString();
    }


    protected void ResetGlass(bool whinMobile = false)
    {
       
    }

    void UpdateCameraLocalEuler()
    {
        if (moveSpeed > douSpeed)
        {
            Vector3 aimEuler = cameraTrans.localEulerAngles;
            float value = 0.4f * ((moveSpeed - douSpeed) / (maxSpeed - douSpeed));
            aimEuler.x = Random.Range(-value, value);
            aimEuler.y = Random.Range(-value, value);
            cameraTrans.localEulerAngles = aimEuler;
        }
        else
        {
            cameraTrans.localEulerAngles = Vector3.zero;
        }
    }


    protected virtual void UpdatePos()
    {

        float aimDis = Vector3.Distance(transform.position, sphere.position);
        disFromSphere = aimDis;
        if (disFromSphere > outDis)
        {
            startPos.y = transform.position.y;
            transform.position = startPos;
            MusicManager.instance.ChangeAndPlaySound(1, "portal_enter_a_02");
        }
    }

    protected virtual void Update()
    {

        if (isRunInGlass)
        {
            if (/*NativeModule.Instance != null*/true)
            {
                //这里给头自身旋转度
                Quaternion headRotation =MyCamera.instance.targetCamera.transform.localRotation;
                Vector3 euler = headRotation.eulerAngles;

                float aimx = CheckAngle(euler.x);
                float aimy = CheckAngle(euler.y);

                float absX = Mathf.Abs(aimx);
                if (absX > zeroValue)
                {

                    if (aimx > 0)
                    {
                        v = 1;
                    }
                    else
                    {
                        v = -1;
                    }
                }
                else
                {
                    v = 0;
                }
                float absY = Mathf.Abs(aimy);
                if (absY > zeroValue)
                {
                    if (aimy > 0)
                    {
                        h = 1;
                    }
                    else
                    {
                        h = -1;
                    }
                }
                else
                {
                    h = 0;
                }


                float nowXvalue = Mathf.Clamp(absX, zeroValue, maxValue);
                float addValueXPercent = (nowXvalue - zeroValue) / offsetValue;
                v += v * addValueXPercent;
                float nowYvalue = Mathf.Clamp(absY, zeroValue, 35);
                float addValueYPercent = (nowYvalue - zeroValue) / offsetValue;
                h += h * addValueYPercent;

            }
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        // transform.position -= new Vector3(0, Time.deltaTime * downForceRate, 0);
        UpdateLog();
        UpdateWindSound();
        UpdatePos();
        UpdateCameraLocalEuler();

    }


    protected virtual void UpdateLog()
    {
        logText.text = "";
       // logText.text += "头的欧拉:" + NativeModule.Instance.GetGlassesQualternion().eulerAngles + "\n";
        //logText.text += "眼镜实例：" + NativeModule.Instance + "\n";
        logText.text += "是否眼镜控制：" + isRunInGlass + "\n";
    }


    protected virtual void FixedUpdate()
    {

        UpdateBodyRotation(h, v);
        UpdateVolocity();
    }

    protected virtual void UpdateVolocity()
    {
        UpdateNowSpeed();
        // float leftAngle = armL.localEulerAngles.z;
        // float rightAngle = armR.localEulerAngles.z;
        rigbody.velocity = transform.forward * moveSpeed;
    }


    private void UpdateNowSpeed()
    {
        Vector3 bodyEulerAngle = transform.localEulerAngles;
        float aimXvalue = CheckAngle(bodyEulerAngle.x);

        float aimSpeed = moveSpeed;
        if (aimXvalue != 0)
        {
            if (aimXvalue > 0)
            {
                //重力影响
                float addValue = downForceRate * aimSpeed * Time.deltaTime;
                //俯角 影响
                addValue *= aimXvalue / 90;
                aimSpeed = aimSpeed + addValue;
            }
            else
            {
                //重力影响
                float reduceValue = downForceRate * aimSpeed * Time.deltaTime;
                //仰角 影响
                reduceValue *= -aimXvalue / 90;
                aimSpeed = aimSpeed - reduceValue / 5f;
            }
        }
        else
        {
            //平飞重力不影响速度，只会下降
        }
        //空气阻力影响
        aimSpeed = aimSpeed - (airForceRate * aimSpeed * Time.deltaTime);
        moveSpeed = Mathf.Clamp(aimSpeed, 0, maxSpeed);
    }


    void UpdateBodyRotation(float leftRightValue, float upDownValue)
    {
        float nowXangle = CheckAngle(transform.localEulerAngles.x);
        nowXangle = Mathf.Abs(nowXangle);
        float scaleNum = 1 - nowXangle / 90;

        float aimUpDownValue = 0;
        if (upDownValue != 0)
        {
            if (moveSpeed > loseControllSpeed)
            {
                aimUpDownValue = upDownValue * (moveSpeed / maxSpeed);
            }
            else
            {
                aimUpDownValue = 1 - (moveSpeed / loseControllSpeed);
            }
        }
        else
        {
            if (moveSpeed > loseControllSpeed)
            {
                aimUpDownValue = downForceRate * Time.deltaTime;
            }
            else
            {
                aimUpDownValue = 1 - (moveSpeed / loseControllSpeed);
            }
        }


        aimUpDownValue -= Mathf.Abs(aimUpDownValue) * downForceRate * Time.deltaTime;

        float aimBodyXvalue = CheckAngle(transform.localEulerAngles.x) + aimUpDownValue;
        if (CheckIfUpDownAngleInRange(aimBodyXvalue))
        {
            //Debug.Log(aimBodyXvalue);
            transform.localEulerAngles = new Vector3(aimBodyXvalue, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }


        scaleNum += scaleNum * (1 - moveSpeed / maxSpeed);

        transform.localEulerAngles += new Vector3(0, rotateSpeed * leftRightValue * scaleNum, 0);
        // transform.localEulerAngles += new Vector3(0, leftRightValue*rotateSpeed, 0);

    }



    bool CheckIfUpDownAngleInRange(float angle)
    {


        if (angle <= maxBodyAngle && angle > minBodyAngle)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    // 将大于180度角进行以负数形式输出
    public float CheckAngle(float value)

    {

        float angle = value - 180;

        if (angle > 0)

        {

            return angle - 180;

        }

        if (value == 0)

        {

            return 0;

        }

        return angle + 180;

    }


    string[] musicStrLowWind = new string[] { "wind_low_b_01", "", "" };
    string[] musicStrMiddleWind = new string[] { "wind_mid_a_01", "wind_mid_b_01", "wind_mid_c_01" };
    string[] musicStrHighWind = new string[] { "wind_high_b_01", "wind_high_c_01", "wind_high_combined_b_01" };
    string[] musicStrOutWind = new string[] { "portal_proximity_a_04", "", "" };

    void UpdateWindSound()
    {
        string[] aimWindSoundNames;
        if (Vector3.Distance(transform.position, sphere.position) < nearOutDis)
        {
            if (moveSpeed > speed2Level[1])
            {
                aimWindSoundNames = musicStrHighWind;
            }
            else if (moveSpeed > speed2Level[0])
            {
                aimWindSoundNames = musicStrMiddleWind;
            }
            else
            {
                aimWindSoundNames = musicStrLowWind;
            }
        }
        else
        {
            aimWindSoundNames = musicStrOutWind;
        }

        AudioClip nowClip = MusicManager.instance.GetLoopSoundClip();
        if (nowClip == null || nowClip.name != aimWindSoundNames[0])
        {
            for (int i = 0; i < aimWindSoundNames.Length; i++)
            {
                MusicManager.instance.ChangeAndPlayLoopSound(i, aimWindSoundNames[i]);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {

    }

}
