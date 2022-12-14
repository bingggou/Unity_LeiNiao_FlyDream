using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//物品的总父类
public class Item : MonoBehaviour
{
    public string itemName;
   
    public FSM_Object owner;


    [ContextMenu("设置道具名字")]
    private void SetName()
    {
        itemName = gameObject.name;
    }

    public virtual void Reset()
    {
        
    }

    private void OnEnable()
    {
        Reset();
    }


    protected virtual void Awake()
    {
        if (itemName == "")
        {
            itemName = transform.name;
        }
    }

}
