using System.Collections.Generic;
using UnityEngine;
using FSM;

public class TriggerTest : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> triggeredObjs = new List<Transform>();

    [HideInInspector]
    public FSM_Object targetFsm;

    private void OnTriggerEnter(Collider other)
    {
        if (OnTriggerEnterEvent != null)
        {

            OnTriggerEnterEvent.Invoke(other);
        }

        if (other.tag == "HandItem")
        {
            if (!triggeredObjs.Contains(other.transform))
            {
                triggeredObjs.Insert(0, other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HandItem")
        {
            if (triggeredObjs.Contains(other.transform))
            {
                triggeredObjs.Remove(other.transform);
            }
        }
    }
    public delegate void OnTriggerEnterFun(Collider collider);
    public event OnTriggerEnterFun OnTriggerEnterEvent;
}