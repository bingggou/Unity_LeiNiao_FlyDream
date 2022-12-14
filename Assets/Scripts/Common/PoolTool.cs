
using UnityEngine;

public class PoolTool : MonoBehaviour
{
    public static GameObject GetFromPool(string path, string name)
    {
        GameObject aimGameObj;
        aimGameObj = GameObjPool.OutPool(name);
        if (aimGameObj == null)
        {
            GameObject temp = Resources.Load<GameObject>(path + name);
            Debug.Log(path + name);
            aimGameObj = Instantiate(temp);
        }
        aimGameObj.transform.parent = null;
        return aimGameObj;
    }

    public static GameObject GetFromPool(string path, string name, Vector3 pos)
    {
        GameObject aimGameObj;
        aimGameObj = GameObjPool.OutPool(name);
        if (aimGameObj == null)
        {
            GameObject temp = Resources.Load<GameObject>(path + name);
       
            Debug.Log(temp.name);
            aimGameObj = Instantiate(temp,pos,Quaternion.identity);
            Debug.Log(aimGameObj);
        }
        aimGameObj.transform.parent = null;
        aimGameObj.transform.position = pos;
        return aimGameObj;
    }


    public static GameObject GetFromPool(string path, string name, Transform tempTrans, bool isfather = false)
    {
        GameObject aimGameObj;
        aimGameObj = GameObjPool.OutPool(name);
        if (aimGameObj == null)
        {
            GameObject temp = Resources.Load<GameObject>(path + name);
            Debug.Log("!!!!!!!!!!!!!" + path + name);
            Debug.Log(temp.name);
            aimGameObj = Instantiate(temp);
            Debug.Log(aimGameObj);
        }
        aimGameObj.transform.parent = null;
        if (isfather == true)
        {
            aimGameObj.transform.parent = tempTrans;
            aimGameObj.transform.position = Vector3.zero;
            aimGameObj.transform.rotation = Quaternion.identity;
        }
        else
        {
            aimGameObj.transform.position = tempTrans.position;
            aimGameObj.transform.rotation = tempTrans.rotation;
        }
        return aimGameObj;
    }

    public static void PutInPool(GameObject gameObj)
    {
        GameObjPool.InPool(gameObj);
    }
}
