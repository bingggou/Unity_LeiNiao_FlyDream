using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    private PXR_Hand lhand;
    private PXR_Hand rhand;
    Transform lIndexFinger;
    Transform rIndexFinger;
    // Start is called before the first frame update
    void Start()
    {
        PXR_Hand[] hands = transform.GetComponentsInChildren<PXR_Hand>();
        lhand = hands[0];
        lIndexFinger = TransformHelper.GetChild(lhand.transform, "p_l_index_null");
        rhand = hands[1];
        rIndexFinger = TransformHelper.GetChild(rhand.transform, "p_r_index_null");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MoveForward()
    {
        //if ()
        //{
        //    transform.Translate();
        //}
    }

   
}
