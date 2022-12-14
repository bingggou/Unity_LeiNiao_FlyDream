
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationManager : MonoBehaviour
{
    private float radius;
    public static NavigationManager Instance;
    [HideInInspector]
   public List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        radius = transform.localScale.x / 2;


        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
      
         Debug.Log("顶点数量:" + navMeshData.vertices.Length);
     

        for (int i = 0; i < navMeshData.vertices.Length; i++)
        {
            //判断当前的顶点是否在列表中，并且该顶点的另外两个顶点是否在列表中（如果不在，表示已经被判断过，并且不符合条件）
            
            if (Vector3.Distance(transform.position, navMeshData.vertices[i]) < radius)
                points.Add(navMeshData.vertices[i]);
        }

        Debug.Log("--------" + points.Count);


        Instance = this;
    }


    /// <summary>
    /// 获取球体半径内高精度随机点
    /// </summary>
    public Vector3 GetSphereRaduisPoint()
    {
        Vector3 point = transform.position;
       
        if (points.Count > 0)
        {
          
            point =points[Random.Range(0, points.Count)];
            Debug.Log("获取随机点成功");
        }
        return point;
    }

    public bool IsPointInRange(Vector3 point)
    {
        if (Vector3.Distance(transform.position, new Vector3(point.x, transform.position.y, point.z)) < radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }




}

