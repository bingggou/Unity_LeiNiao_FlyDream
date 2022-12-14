using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Token: 0x02000084 RID: 132
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour
{
    // Token: 0x17000019 RID: 25
    // (get) Token: 0x060001CD RID: 461 RVA: 0x0000E0FE File Offset: 0x0000C4FE
    public static PlayerMovement Singleton
    {
        get
        {
            return PlayerMovement.singleton;
        }
    }

    // Token: 0x060001CE RID: 462 RVA: 0x0000E105 File Offset: 0x0000C505
    private void Awake()
    {
        if (PlayerMovement.singleton != null)
        {
            UnityEngine.Object.Destroy(PlayerMovement.singleton);
        }
        PlayerMovement.singleton = this;
    }

    // Token: 0x060001CF RID: 463 RVA: 0x0000E127 File Offset: 0x0000C527
    private void Start()
    {
        this.rb = base.GetComponent<Rigidbody>();
       jiegou= SetJieGou(transform);
        GUIUtility.systemCopyBuffer=(jiegou);
    }

 

    // Token: 0x060001D0 RID: 464 RVA: 0x0000E135 File Offset: 0x0000C535
    private void FixedUpdate()
    {
        //if (LocalGameManager.Singleton.playerState == LocalGameManager.PlayerState.Dead)
        //{
        //	return;
        //}
        this.GetInput();
        this.Move();
    }

    // Token: 0x060001D1 RID: 465 RVA: 0x0000E154 File Offset: 0x0000C554
    private void OnCollisionEnter(Collision c)
    {
        //if (LocalGameManager.Singleton.playerState == LocalGameManager.PlayerState.Flying && !c.gameObject.CompareTag("Player"))
        //{
        //	this.Die();
        //}
    }

    // Token: 0x060001D2 RID: 466 RVA: 0x0000E180 File Offset: 0x0000C580
    private void GetInput()
    {
        float num = Input.GetAxis("Horizontal");
        float num2 = Input.GetAxis("Vertical");

        num *= Mathf.Abs(Mathf.Cos(num2 * 3.1415927f / 4f));
        num2 *= Mathf.Abs(Mathf.Cos(num * 3.1415927f / 4f));
        Vector2 keyboardInput = new Vector2(num, num2);

        Vector2 vector = this.ManageKeyboardAndController(keyboardInput);

        this.horizontalInput = vector.x * this.inputSensitivity * SettingsManager.xAxisSensibility;
        Debug.Log("!!!:" + horizontalInput);
        this.verticalInput = vector.y * this.inputSensitivity * SettingsManager.yAxisSensibility;
        if (SettingsManager.xAxisInverted)
        {
            this.horizontalInput *= -1f;
        }
        if (SettingsManager.yAxisInverted)
        {
            this.verticalInput *= -1f;
        }
        Debug.Log(verticalInput);
    }

    // Token: 0x060001D3 RID: 467 RVA: 0x0000E2C0 File Offset: 0x0000C6C0
    private Vector2 ManageKeyboardAndController(Vector2 keyboardInput)
    {
        Vector2 zero = Vector2.zero;

        zero.x = keyboardInput.x;


        zero.y = keyboardInput.y;

        return zero;
    }

    // Token: 0x060001D4 RID: 468 RVA: 0x0000E338 File Offset: 0x0000C738
    private void Move()
    {
        float num = Mathf.InverseLerp(this.forwardSpeedLimits.min, this.forwardSpeedLimits.max, this.currentSpeed);
        float time = 1f - num;
        float num2 = this.stallSpeedRelationCurve.Evaluate(time);
        float d = 1f - num2;
        float num3 = Vector3.Dot(Vector3.up, base.transform.forward) * -1f;
        Vector3 a = Vector3.Cross(Vector3.up, new Vector3(base.transform.forward.x, 0f, base.transform.forward.z));
        Vector3 vector = Vector3.zero;
        Vector3 b = Vector3.down * this.maxStallForce * num2;
        vector += b;
        if (num3 > 0f)
        {
            this.currentSpeed += this.acceleration.down * num3;
        }
        else
        {
            this.currentSpeed += this.acceleration.up * num3;
        }
        this.currentSpeed -= this.dragConstant;
        this.currentSpeed = Mathf.Clamp(this.currentSpeed, this.forwardSpeedLimits.min, this.forwardSpeedLimits.max);
        Vector3 b2 = base.transform.forward * this.currentSpeed;
        vector += b2;
        float num4 = Vector3.Angle(Vector3.up, base.transform.forward);
        float num5 = this.yaw.rotationSpeed.Evaluate(num4 / 180f);
        Vector3 b3 = base.transform.right * this.yaw.sideForce * this.horizontalInput * (1f - num5);
        vector += b3;
        this.rb.AddForce(vector);
        float num6 = 90f - num4;
        if (this.verticalInput > 0f && num6 > -this.pitch.maxRotationForPlayerInput.y)
        {
            this.rb.AddTorque(a * Mathf.Clamp(this.verticalInput, 0f, 1f) * this.pitch.torque.down);
        }
        if (num6 < this.pitch.maxRotationForPlayerInput.x && this.verticalInput < 0f)
        {
            this.rb.AddTorque(base.transform.right * Mathf.Clamp(this.verticalInput, -1f, 0f) * this.pitch.torque.up * d);
        }
        this.rb.AddTorque(a * num2 * this.speedCutOffForcedRotationStrengh);
        this.rb.AddTorque(Vector3.up * this.horizontalInput * this.yaw.torque * (1f - num5));
        float num7 = this.yaw.maxRotation * this.horizontalInput * -1f;
        Vector3 euler = new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z * num5 + num7);
        float num8 = Mathf.Lerp(3f, 2f, num5);
        base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(euler), num8 * Time.deltaTime);
        if (num4 < this.pitch.RotationLimits.x)
        {
            float t = this.pitch.RotationLimits.x / num4;
            Vector3 forward = Vector3.LerpUnclamped(Vector3.up, base.transform.forward, t);
            Vector3 upwards = Vector3.LerpUnclamped(Vector3.up, base.transform.up, t);
            this.rb.angularVelocity = Vector3.zero;
            Quaternion rotation = Quaternion.LookRotation(forward, upwards);
            base.transform.rotation = rotation;
        }
        Vector3 rhs = base.transform.forward + Vector3.up;
        float num9 = Vector3.Dot(base.transform.up, rhs);
        if (num4 > this.pitch.RotationLimits.y && num9 > 0f)
        {
            float t2 = this.pitch.RotationLimits.y / 180f;
            Vector3 forward2 = Vector3.Lerp(Vector3.up, -Vector3.up, t2);
            forward2 = new Vector3(base.transform.forward.x, forward2.y, base.transform.forward.z);
            Vector3 vector2 = Vector3.Lerp(Vector3.up, base.transform.up, t2);
            if (this.verticalInput >= 0f)
            {
                this.rb.angularVelocity = Vector3.zero;
            }
            Quaternion quaternion = Quaternion.LookRotation(forward2);
        }
    }

    // Token: 0x060001D5 RID: 469 RVA: 0x0000E898 File Offset: 0x0000CC98
    public void Die()
    {
        //LocalGameManager.Singleton.OnPlayerDeath();
        this.bodyParts.SetActive(true);

        IEnumerator enumerator = this.bodyParts.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                Transform transform = (Transform)obj;
                transform.gameObject.AddComponent<Rigidbody>();
            }
        }
        finally
        {
            IDisposable disposable;
            if ((disposable = (enumerator as IDisposable)) != null)
            {
                disposable.Dispose();
            }
        }
        this.PlayerDeathAnimation();
        this.bodyParts.transform.parent = null;
        UnityEngine.Object.Destroy(this.bodyParts, 7f);
    }

    // Token: 0x060001D6 RID: 470 RVA: 0x0000E960 File Offset: 0x0000CD60
    public void PlayerDeathAnimation()
    {
        //this.rb.useGravity = true;
        //this.rb.drag = 1f;
        //this.rb.angularDrag = 1f;
        //this.rb.angularVelocity = Vector3.zero;
        //int childCount = BodyPartsReference.singelton.gameObject.transform.childCount;
        //Vector3 position = base.transform.position;
        //for (int i = 1; i < childCount; i++)
        //{
        //	if ((double)UnityEngine.Random.value < 0.2)
        //	{
        //		UnityEngine.Object.Destroy(BodyPartsReference.singelton.transform.GetChild(i).GetComponent<Joint>());
        //	}
        //	BodyPartsReference.singelton.transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        //	BodyPartsReference.singelton.transform.GetChild(i).GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //	BodyPartsReference.singelton.transform.GetChild(i).GetComponent<Rigidbody>().mass = 1f;
        //	BodyPartsReference.singelton.transform.GetChild(i).GetComponent<Rigidbody>().angularDrag = 0f;
        //	BodyPartsReference.singelton.transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
        //}
    }

    // Token: 0x060001D7 RID: 471 RVA: 0x0000EAA4 File Offset: 0x0000CEA4
    public void ResetPlayerRigidbodies()
    {
        foreach (Rigidbody rigidbody in this.allPlayerRBs)
        {
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
        }
    }

    // Token: 0x060001D8 RID: 472 RVA: 0x0000EAE8 File Offset: 0x0000CEE8
    public void SetPlayerRotation(Quaternion rotation)
    {
        foreach (Rigidbody rigidbody in this.allPlayerRBs)
        {
            rigidbody.rotation = rotation;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
        }
    }

    // Token: 0x060001D9 RID: 473 RVA: 0x0000EB31 File Offset: 0x0000CF31
    public void ApplyExternalForce(Vector3 externalForce)
    {
        this.rb.AddForce(externalForce);
    }

    // Token: 0x060001DA RID: 474 RVA: 0x0000EB3F File Offset: 0x0000CF3F
    public void ApplyExternalTorque(Vector3 externalTorque)
    {
        this.rb.AddTorque(externalTorque);
    }

    // Token: 0x040002EF RID: 751
    private static PlayerMovement singleton;


    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 300, 600), jiegou);
    }
    string jiegou;
    string SetJieGou(Transform trans)
    {
        string str = trans.name + GetTransInfo(trans);
        for (int i = 0; i < trans.childCount; i++)
        {
            Transform temp = trans.GetChild(i);

            str += trans.name + "->" + SetJieGou(temp);

        }
        return str;
    }
    string GetTransInfo(Transform trans)
    {
        return ":" + trans.localPosition.ToString() + trans.localEulerAngles.ToString() + trans.localScale.ToString() + "\n";
    }


    void ShowCanshu()
    {
        string aimStr = "";
        aimStr += "\n" + nameof(inputSensitivity) + ":" + inputSensitivity;
        aimStr += "\n" + nameof(acceleration.up) + ":" + acceleration.up;
        aimStr += "\n" + nameof(acceleration.down) + ":" + acceleration.down;
        aimStr += "\n" + nameof(dragConstant) + ":" + dragConstant;
        aimStr += "\n" + nameof(currentSpeed) + ":" + currentSpeed;
        aimStr += "\n" + nameof(forwardSpeedLimits.min) + ":" + forwardSpeedLimits.min;
        aimStr += "\n" + nameof(forwardSpeedLimits.max) + ":" + forwardSpeedLimits.max;
        aimStr += "\n" + nameof(pitch.torque.up) + ":" + pitch.torque.up;
        aimStr += "\n" + nameof(pitch.torque.down) + ":" + pitch.torque.down;
        aimStr += "\n" + nameof(pitch.maxRotationForPlayerInput) + ":" + pitch.maxRotationForPlayerInput;
        aimStr += "\n" + nameof(pitch.RotationLimits) + ":" + pitch.RotationLimits;
        aimStr += "\n" + nameof(yaw.torque) + ":" + yaw.torque;
        aimStr += "\n" + nameof(yaw.sideForce) + ":" + yaw.sideForce;
        aimStr += "\n" + nameof(yaw.maxRotation) + ":" + yaw.maxRotation;
        aimStr += "\n" + nameof(yaw.rotationSpeed) + ":" + yaw.rotationSpeed;
        aimStr += "\n" + nameof(maxStallForce) + ":" + maxStallForce;
        aimStr += "\n" + nameof(stallSpeedRelationCurve) + ":" + stallSpeedRelationCurve;
        aimStr += "\n" + nameof(speedCutOffForcedRotationStrengh) + ":" + speedCutOffForcedRotationStrengh;

        GUI.Box(new Rect(10, 10, 300, 600), "看这里！" + aimStr);
    }



    // Token: 0x040002F0 RID: 752
    [Header("Input Settings:")]
    [SerializeField]
    [Range(0f, 1f)]
    private float inputSensitivity = 1f;

    // Token: 0x040002F1 RID: 753
    [Header("速度设置:")]
    [Tooltip("这因子是箭率以下垂的角度来决定…")]
    [SerializeField]
    private PlayerMovement.TwoDirectionalFloat acceleration;

    // Token: 0x040002F2 RID: 754
    [Tooltip("减慢速度的因子不管现场球员的位置")]
    [SerializeField]
    private float dragConstant;

    // Token: 0x040002F3 RID: 755
    [Tooltip("当前移动速度是设定")]
    public float currentSpeed;

    // Token: 0x040002F4 RID: 756
    [Tooltip("最小及最大的前进速度.")]
    public PlayerMovement.Limits forwardSpeedLimits;

    // Token: 0x040002F5 RID: 757
    [Header("旋转设置:")]
    [Tooltip("控制设置")]
    [SerializeField]
    private PlayerMovement.PitchStruct pitch;

    // Token: 0x040002F6 RID: 758
    [Tooltip("偏航输入")]
    [SerializeField]
    private PlayerMovement.YawStruct yaw;

    // Token: 0x040002F7 RID: 759
    [Header("摊位设置:")]
    [Tooltip("我们身体中的引力作用于玩家.")]
    [SerializeField]
    private float maxStallForce;

    // Token: 0x040002F8 RID: 760
    [SerializeField]
    [Tooltip("控制重力与前进速度的关系。x轴是作为反向单位间隔的前进速度(0是最快的1是最慢的)，y轴是施加在玩家身上的重力.")]
    private AnimationCurve stallSpeedRelationCurve;

    // Token: 0x040002F9 RID: 761
    [SerializeField]
    [Tooltip("当玩家开始失去速度和下降时，他会旋转回来，这个参数决定了旋转的强度.")]
    private float speedCutOffForcedRotationStrengh;

    // Token: 0x040002FA RID: 762
    [Header("其他设置:")]
    [Tooltip("玩家的身体部位。用于玩家死亡.")]
    [SerializeField]
    private GameObject bodyParts;

    // Token: 0x040002FB RID: 763
    [Tooltip("所有的刚体连接到玩家预制件。用于传送门重置.")]
    [SerializeField]
    private Rigidbody[] allPlayerRBs;

    // Token: 0x040002FC RID: 764
    [HideInInspector]
    public float horizontalInput;

    // Token: 0x040002FD RID: 765
    [HideInInspector]
    public float verticalInput;

    // Token: 0x040002FE RID: 766
    private Rigidbody rb;

    // Token: 0x040002FF RID: 767
    private Quaternion quaternionLimitTarget;

    // Token: 0x02000085 RID: 133
    [SerializeField]
    [Serializable]
    private struct TwoDirectionalFloat
    {
        // Token: 0x04000300 RID: 768
        [Tooltip("Wert für Up-Richtung.")]
        public float up;

        // Token: 0x04000301 RID: 769
        [Tooltip("Wert für Down-Richtung.")]
        public float down;
    }

    // Token: 0x02000086 RID: 134
    [Serializable]
    public struct Limits
    {
        // Token: 0x04000302 RID: 770
        [Tooltip("Wert für Minimum.")]
        public float min;

        // Token: 0x04000303 RID: 771
        [Tooltip("Wert für Maximum.")]
        public float max;
    }

    // Token: 0x02000087 RID: 135
    [SerializeField]
    [Serializable]
    private struct PitchStruct
    {
        // Token: 0x04000304 RID: 772
        [Tooltip("根据电子仪器在x轴旋转")]
        public PlayerMovement.TwoDirectionalFloat torque;

        // Token: 0x04000305 RID: 773
        [Tooltip("设备以x轴旋转.X为上，Y为下.")]
        public Vector2 maxRotationForPlayerInput;

        // Token: 0x04000306 RID: 774
        [Tooltip("X值表示玩家前方与世界Y轴之间的天使alpha的下限和Y值的上限")]
        public Vector2 RotationLimits;
    }

    // Token: 0x02000088 RID: 136
    [SerializeField]
    [Serializable]
    private struct YawStruct
    {
        // Token: 0x04000307 RID: 775
        [Tooltip("通过电子仪器在y轴旋转.")]
        public float torque;

        // Token: 0x04000308 RID: 776
        [Tooltip("保持垂直于托克.")]
        public float sideForce;

        // Token: 0x04000309 RID: 777
        [Tooltip("设备以x轴旋转.")]
        public float maxRotation;

        // Token: 0x0400030A RID: 778
        [Tooltip("玩家绕z轴旋转的速度有多快.")]
        public AnimationCurve rotationSpeed;
    }
}
