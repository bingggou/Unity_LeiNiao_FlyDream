
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{

    public Transform targetCameraTrans;
    [Range(0, 1)]
    public float cameraRotateLerp = 0.05f;
    [Range(0, 1)]
    public float cameraFollowLerp = 0.05f;
    public static MyCamera instance;

    public Camera targetCamera;

    private void Awake()
    {
        Debug.Log("zhixing");
        targetCamera = transform.GetComponentInChildren<Camera>();
        transform.parent = null;
        instance = this;



    }
    private void Start()
    {

        //transform.position = targetCamera.position;
        // transform.rotation = targetCamera.rotation;
        ChangeFov(60);
    }

    private void Update()
    {
      
    }



    public void ChangeFov(int value)
    {
        targetCamera.fieldOfView = value;
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetCameraTrans.position, cameraFollowLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetCameraTrans.rotation, cameraRotateLerp);


    }


}
