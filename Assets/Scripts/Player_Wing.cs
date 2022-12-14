using FfalconXR.Native;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Player_Wing : Player
{

    [Header("距离检测的层级类别设定")]
    public LayerMask disCheckLayerMask;

    [Header("翅膀上下摆动减值")]
    [Range(0, 60)]
    public float wingAngleReduce = 0;
    [Header("翅膀摆动插值速度")]
    [Range(0, 0.2f)]
    public float wingAngleLerpRange = 0.1f;


    private int totalScore;
    int comboScore;
    int lastComboScore;
    DistenceCheckTool dcTool;
    private bool couldPlay = false;
    private bool CouldPlay
    {
        set
        {
            if (couldPlay != value)
            {
                if (value == true)
                {

                    couldPlay = value;
                    OnGameStart();
                }
                else
                {
                    couldPlay = value;
                    StartCoroutine(OnGameOver());
                }
            }
        }
        get
        {
            return couldPlay;
        }
    }
    private Transform childMain;
    float maxWingAngle;
    float middleWingAngle;
    Transform armL;
    Transform armR;
    Transform jianTou;
    SpriteRenderer jianTouSR;

    GameObject addStarBKG;
    Text scoreTextForAnimation;


    Text scoreText;
    Text speedText;
    GameObject laserBeam;
    ParticleSystem dieFx;
    float nowDistenceFromBuilding;
    Vector3 lastScorePos;
    float maxScoreDis = 12;
    GameObject starPic;
    Transform starAimTrans;
    Vector3 starBornPos;


    MeshRenderer[] allmr;
    List<BodyPart> allbp = new List<BodyPart>();

    void OnGameStart()
    {
        if (isFirstPerson == 1)
        {
            SetMeshRenderVisiable(false);

        }
        else
        {
            SetMeshRenderVisiable(true);
        }
        mainUi.SetActive(false);
        gameUi.SetActive(true);
        //laserBeam
        laserBeam.SetActive(false);
        SetDeadBodyVisiable(false);
        ResetGlass();
    }

    IEnumerator OnGameOver()
    {
        MusicManager.instance.ChangeAndPlayLoopSound("");
        MusicManager.instance.ChangeAndPlaySound(1, "playerCrash(破碎)");
        SetDeadBodyVisiable(true);
        SetMeshRenderVisiable(false);
        rigbody.velocity = Vector3.zero;
        //dieFx.Play();
        if (comboScore != 0)
        {
            totalScore += comboScore;
            comboScore = 0;
            lastComboScore = 0;
        }
        scoreText.text = totalScore.ToString();
        jianTou.gameObject.SetActive(false);


        yield return new WaitForSeconds(2);
        laserBeam.SetActive(true);
        mainUi.SetActive(true);
        gameUi.SetActive(false);
        transform.position = originalPos;
        transform.rotation = originalRot;
        moveSpeed = startSpeed;
        scoreText.text = "";
       // SetMeshRenderVisiable(true);
        yield return StartCoroutine(AddScoreRoutine());

    }

    private int oneStarScore = 1000;

    IEnumerator AddScoreRoutine()
    {
        scoreTextForAnimation.text = totalScore.ToString();
        if (totalScore >= oneStarScore)
        {
            addStarBKG.SetActive(true);
            int needScore = totalScore / oneStarScore;


            int aimNum = 0;
            while (totalScore != aimNum)
            {
                float temp = Mathf.Lerp(totalScore, aimNum, 0.3f);
                totalScore = (int)Mathf.Floor(temp);
                scoreTextForAnimation.text = totalScore.ToString();
                yield return new WaitForSeconds(Time.deltaTime);
            }


            while (needScore > 0)
            {
                needScore -= 1;
                starPic.transform.position = starBornPos;
                starPic.SetActive(true);
                while (Vector3.Distance(starPic.transform.position, starAimTrans.position) >= 0.666f)
                {
                    starPic.transform.position = Vector3.Lerp(starPic.transform.position, starAimTrans.position, 0.3f);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                starPic.SetActive(false);
                StarNum += 1;
                MusicManager.instance.ChangeAndPlaySound("combo_complete_b_01");
            }

            addStarBKG.SetActive(false);
        }
        totalScore = 0;
        yield return null;
    }





    void UpdateScoreText(int nowScore4Show)
    {
        if (nowScore4Show != lastComboScore)
        {
            scoreText.text = totalScore + "+" + nowScore4Show;
            lastComboScore = nowScore4Show;
        }
        else
        {
            scoreText.text = totalScore.ToString();
        }
    }

    void UpdateSpeedText()
    {
        speedText.text = (moveSpeed * 3.6f).ToString("0.00") + "km/h";
    }

    public void SetCouldPlay(bool _couldPlay)
    {
        CouldPlay = _couldPlay;
    }

    protected override void Awake()
    {
        base.Awake();

        dcTool = new DistenceCheckTool(transform, disCheckLayerMask);

        dieFx = transform.GetComponentInChildren<ParticleSystem>();
        childMain = transform.GetChild(0);


        armL = TransformHelper.GetChild(transform, nameof(armL));
        armR = TransformHelper.GetChild(transform, nameof(armR));

        //scoreText
        scoreText = TransformHelper.GetChild(gameUi.transform, nameof(scoreText)).GetComponent<Text>();

        speedText = TransformHelper.GetChild(gameUi.transform, nameof(speedText)).GetComponent<Text>();

        //addStartBKG

        addStarBKG = TransformHelper.GetChild(mainUi.transform, nameof(addStarBKG)).gameObject;
        starAimTrans = TransformHelper.GetChild(mainUi.transform, "星星");



        starPic = TransformHelper.GetChild(addStarBKG.transform, nameof(starPic)).gameObject;
        starBornPos = starPic.transform.position;
        scoreTextForAnimation = TransformHelper.GetChild(addStarBKG.transform, nameof(scoreTextForAnimation)).GetComponent<Text>();

        UpdateSpeedText();

        Debug.Log(armL.localEulerAngles);
        maxWingAngle = 120 - wingAngleReduce;
        middleWingAngle = wingAngleReduce + (maxWingAngle - wingAngleReduce) / 2;
        jianTou = TransformHelper.GetChild(transform, nameof(jianTou));
        if (jianTou)
        {
            jianTouSR = jianTou.GetComponentInChildren<SpriteRenderer>();
        }

        allmr = childMain.GetComponentsInChildren<MeshRenderer>(true);

        foreach (var item in allmr)
        {
            GameObject newPart = Instantiate(item.gameObject, null);
            newPart.name = "零件";
            BodyPart bpTemp = new BodyPart(item.transform, newPart.transform);
            bpTemp.Hide();
            allbp.Add(bpTemp);
        }

        SetMeshRenderVisiable(false);

    }


    void SetDeadBodyVisiable(bool couldShow)
    {
        foreach (var item in allbp)
        {
            if (couldShow)
            {
                item.Show();
            }
            else
            {
                item.Hide();
            }
        }
    }



    private void SetMeshRenderVisiable(bool couldSee)
    {

        foreach (var item in allmr)
        {
            item.enabled = couldSee;
        }
    }


    private void Start()
    {
        laserBeam = TransformHelper.GetChild(MyCamera.instance.targetCamera.transform, nameof(laserBeam)).gameObject;
    }


    private void UpdateJianTou()
    {
        if (jianTouSR == null)
        {
            return;
        }

        float value = (new Vector2(h, v)).magnitude;
        if (value == 0)
        {
            jianTou.transform.gameObject.SetActive(false);
        }
        else
        {

            jianTou.transform.gameObject.SetActive(true);
        }

        if (isRunInGlass)
        {
            // float aimPercent = value - 1;
            JianTouChangePerUpdate(value, value, 0.01f);

        }
        else
        {
            JianTouChangePerUpdate(value, value, 0.01f);
        }


    }

    public float GetJianTouAngle()
    {
        Vector2 to = new Vector2(h, -v);
        float angle = Vector2.Angle(Vector2.up, to);
        if (to.x < 0)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    float NowDistenceFromBuilding
    {
        set
        {
            if (value <= maxScoreDis)
            {
                if (nowDistenceFromBuilding > maxScoreDis)
                {
                    lastScorePos = transform.position;
                }
                else
                {
                    float nowDisFromlastScorePos = Vector3.Distance(transform.position, lastScorePos);
                    if (nowDisFromlastScorePos > 1)
                    {
                        // MusicManager.instance.ChangeAndPlaySound(1, "multiplyer_d_01", true);
                        //score_base_i_01
                        MusicManager.instance.ChangeAndPlaySound(1, "score_base_i_01");
                        int needAddScore = 20;
                        if (value <= 2)
                        {
                            needAddScore = 120;

                        }
                        else if (value <= 6)
                        {
                            needAddScore = 50;
                        }
                        lastComboScore = comboScore;
                        comboScore += needAddScore;
                        lastScorePos = transform.position;
                    }
                }
            }
            else
            {
                if (nowDistenceFromBuilding <= maxScoreDis)
                {
                    if (comboScore != 0)
                    {
                        totalScore += comboScore;
                        comboScore = 0;
                        lastComboScore = 0;
                    }
                }

            }


            nowDistenceFromBuilding = value;
        }
        get { return nowDistenceFromBuilding; }
    }

    void UpdateScore()
    {
        NowDistenceFromBuilding = dcTool.CheckOneTime();
        float aimScore = Mathf.Lerp(lastComboScore, comboScore, 0.1f);
        aimScore = Mathf.Ceil(aimScore);
        UpdateScoreText((int)aimScore);

    }

    void JianTouChangePerUpdate(float aimAlpha, float aimScale, float lerpRange)
    {
        float lerpR = Mathf.Clamp01(lerpRange);
        jianTouSR.color = new Color(1, 1, 1, Mathf.Lerp(jianTouSR.color.a, aimAlpha, lerpR));
        float scale = Mathf.Lerp(jianTou.localScale.x, aimScale, lerpR);
        jianTou.localScale = new Vector3(scale, scale, scale);
        Vector3 nowEuler = jianTou.transform.localEulerAngles;
        nowEuler.z = Mathf.Lerp(nowEuler.z, GetJianTouAngle(), 0.1f);
        jianTou.transform.localEulerAngles = nowEuler;

    }

    protected override void Update()
    {
        if (couldPlay)
        {
            base.Update();
            UpdateJianTou();
            UpdateScore();
            UpdateSpeedText();

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetCouldPlay(true);
        }

    }

    protected override void FixedUpdate()
    {
        if (couldPlay)
        {
            UpdateWingAngle(h, v);
            base.FixedUpdate();
        }
    }

    void UpdateWingAngle(float leftRightValue, float upDownValue)
    {
        if (leftRightValue == 0 && upDownValue == 0)
        {
            Set2WingAngle(middleWingAngle, middleWingAngle);

        }
        else
        {
            if (upDownValue != 0)
            {


                if (upDownValue > 0)
                {
                    if (leftRightValue != 0)
                    {
                        if (leftRightValue > 0)
                        {

                            Set2WingAngle(maxWingAngle, wingAngleReduce);
                        }
                        else
                        {
                            Set2WingAngle(wingAngleReduce, maxWingAngle);
                        }
                    }
                    else
                    {
                        Set2WingAngle(wingAngleReduce, wingAngleReduce);
                    }
                }
                else
                {
                    if (leftRightValue != 0)
                    {
                        if (leftRightValue > 0)
                        {

                            Set2WingAngle(maxWingAngle, middleWingAngle);
                        }
                        else
                        {
                            Set2WingAngle(middleWingAngle, maxWingAngle);
                        }
                    }
                    else
                    {
                        Set2WingAngle(maxWingAngle, maxWingAngle);
                    }
                }
            }
            else
            {
                if (leftRightValue > 0)
                {

                    Set2WingAngle(maxWingAngle, wingAngleReduce);
                }
                else
                {
                    Set2WingAngle(wingAngleReduce, maxWingAngle);
                }

            }
        }
    }

    void Set2WingAngle(float lAngle, float rAngle)
    {
        Set1WingAngle(armL, lAngle);
        Set1WingAngle(armR, rAngle);
    }

    void Set1WingAngle(Transform wingTrans, float angle)
    {
        //Debug.Log(GetInspectorRotationValueMethod(wingTrans));
        //Vector3 inspectLocalEuler = GetInspectorRotationValueMethod(wingTrans);
        float lerpZangle = Mathf.LerpUnclamped(wingTrans.localEulerAngles.z, angle, wingAngleLerpRange);
        //Debug.Log(wingTrans.name+wingTrans.localEulerAngles.z);
        wingTrans.localEulerAngles = new Vector3(wingTrans.localEulerAngles.x, wingTrans.localEulerAngles.y, lerpZangle);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        SetCouldPlay(false);
    }

    protected override void UpdateLog()
    {
        base.UpdateLog();
        logText.text += "当前fov:" + MyCamera.instance.targetCamera.fieldOfView + "\n";
        logText.text += "当前帧率:" + (1 / Time.deltaTime).ToString() + "\n";
    }


    protected override void UpdatePos()
    {
        if (transform.position.y < sphere.position.y)
        {
            SetCouldPlay(false);
        }
        else
        {
            base.UpdatePos();

        }

    }





}


public class BodyPart
{
    Transform modelPart;
    Transform nowPart;
    Rigidbody nowRigbody;
    BoxCollider nowCollider;
    public BodyPart(Transform _modelPart, Transform _nowPart)
    {
        modelPart = _modelPart;
        nowPart = _nowPart;
        nowCollider = nowPart.gameObject.GetComponent<BoxCollider>();
        if (nowCollider == null)
        {
            nowCollider = nowPart.gameObject.AddComponent<BoxCollider>();
        }
        nowRigbody = nowPart.gameObject.AddComponent<Rigidbody>();
    }

    public void Hide()
    {
        nowPart.gameObject.SetActive(false);
    }
    public void Show()
    {
        nowRigbody.velocity = Vector3.zero;
        ResetPosAndRot();
        nowPart.gameObject.SetActive(true);
        nowRigbody.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
    }



    void ResetPosAndRot()
    {
        nowPart.rotation = modelPart.rotation;
        nowPart.position = modelPart.position;
    }
}


public class DistenceCheckTool
{
    LayerMask layerMask;
    float[] distenceValues;
    float rayLength = 1000;
    Transform target;
    public DistenceCheckTool(Transform _target, LayerMask _layerMask)
    {
        target = _target;
        layerMask = _layerMask;
        distenceValues = new float[4];
        for (int i = 0; i < distenceValues.Length; i++)
        {
            distenceValues[i] = rayLength;
        }
    }

    float GetMinValue()
    {
        float minValue = distenceValues[0];
        for (int i = 1; i < distenceValues.Length; i++)
        {
            if (minValue > distenceValues[i])
            {
                minValue = distenceValues[i];
            }
        }
        return minValue;
    }

    public float CheckOneTime()
    {
        distenceValues[0] = CheckOneRayByDir(-target.right);
        distenceValues[1] = CheckOneRayByDir(target.right);
        distenceValues[2] = CheckOneRayByDir(target.up);
        distenceValues[3] = CheckOneRayByDir(-target.up);
        return GetMinValue();
    }

    float CheckOneRayByDir(Vector3 dir)
    {
        Vector3 direction = dir.normalized;
        Ray ray = new Ray(target.position, dir);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, rayLength, layerMask))
        {
            Debug.DrawLine(target.position, hitInfo.point);
            return hitInfo.distance;
        }
        else
        {
            return rayLength;
        }

    }


}

