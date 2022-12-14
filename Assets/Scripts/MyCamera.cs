using FfalconXR.Native;
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

#if !UNITY_EDITOR
        ChangeFov(-66);

#endif

    }

    private void Update()
    {
        if (Google.XR.Cardboard.Api.HasNewDeviceParams())
        {

            Google.XR.Cardboard.Api.ReloadDeviceParams();
        }
    }



    public void ChangeFov(int value)
    {


        targetCamera.fieldOfView = value;
        if (NativeModule.Instance != null)
        {
            NativeModule.Instance.ChangeFov(value);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetCameraTrans.position, cameraFollowLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetCameraTrans.rotation, cameraRotateLerp);


    }


}
