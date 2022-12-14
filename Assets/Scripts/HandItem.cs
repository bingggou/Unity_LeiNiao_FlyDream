using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//可以手拿的道具
public class HandItem : Item
{
    public HandPosAndRot leftHandPR;
    public HandPosAndRot rightHandPR;
    public HandPosAndRot pc1HandPR;
    protected HandItemType handItemType;
    private Rigidbody rigBody;

    protected virtual void Start()
    {
        handItemType = HandItemType.Common;
        rigBody = transform.GetComponent<Rigidbody>();

        //测试用
        rigBody.constraints = RigidbodyConstraints.FreezeAll;
    }


    

    public void OnDragStart(Transform handTrans,HandPosAndRot localTransState)
    {
        rigBody.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = handTrans;
        localTransState.ChangeTrans(transform);
    }

    public void OnDragEnd()
    {
        rigBody.constraints = RigidbodyConstraints.None;
        transform.parent = null;   
    }



    [ContextMenu("定位右手")]
    private void LoadRightHandLocalPos()
    {
        rightHandPR.ChangeTrans(transform);
    }




    [ContextMenu("左手掌位确定")]
    private void SetLeftHandLocalPos()
    {
        leftHandPR.Setup(transform.localPosition,transform.localRotation);
    }
    [ContextMenu("右手掌位确定")]
    private void SetRightHandLocalPos()
    {
        rightHandPR.Setup(transform.localPosition, transform.localRotation);
    }



    [ContextMenu("第一人称电脑掌位确定")]
    private void SetPC1HandLocalPos()
    {
        pc1HandPR.Setup(transform.localPosition, transform.localRotation);
    }
}


[System.Serializable]
public class HandPosAndRot
{
  public  Vector3 localPosition=Vector3.zero;
  public  Quaternion localRotation=Quaternion.identity;

    public void Setup(Vector3 localPos,Quaternion localRot)
    {
        localPosition = localPos;
        localRotation = localRot;
    }

    public void ChangeTrans(Transform trans)
    {
        trans.localPosition = localPosition;
        trans.localRotation = localRotation;
    }
}

public enum HandItemType
{
    Common,
    Weapon
}


