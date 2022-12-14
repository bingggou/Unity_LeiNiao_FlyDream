using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjPool
{
    public static Dictionary<string, List<GameObject>> itemPool = new Dictionary<string, List<GameObject>>();
   
    public static void ClearPool()
    {
        itemPool.Clear();
    }

    public static bool InPool(GameObject gameObj)
    {

      
        if (itemPool.ContainsKey(gameObj.name) == false)
        {
            itemPool.Add(gameObj.name, new List<GameObject>());
        }

        gameObj.SetActive(false);

        if (!itemPool[gameObj.name].Contains(gameObj))
        {
            itemPool[gameObj.name].Add(gameObj);
        }
        return true;

    }

    public static GameObject OutPool(string itemName)
    {
        if (itemPool.ContainsKey(itemName) == false)
        {
            itemPool.Add(itemName, new List<GameObject>());
        }

        if (itemPool[itemName].Count == 0)
        {
            return null;
        }
        else
        {
            GameObject outGo = itemPool[itemName][0];
            itemPool[itemName].RemoveAt(0);
            outGo.SetActive(true);
            return outGo;
        }
    }
}
