using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;



public class DevicesManager : MonoBehaviour
{
    [Header("左手按键")]
    public GamePadInfo leftPadInfo;

    [Header("右手按键")]
    public GamePadInfo rightPadInfo;


    public static DevicesManager Instance;

    Transform body;
    InputDevice headDevice;
    InputDevice leftPad;
    InputDevice rightPad;
    //public Text debugText;

    private void Start()
    {
        body = transform;
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.characteristics.ToString()));
        }
        if (inputDevices.Count == 3)
        {
            headDevice = inputDevices[0];
            leftPad = inputDevices[1];
            rightPad = inputDevices[2];
        }
        else
        {
            Debug.Log("设备不存在了");
            Debug.Log(inputDevices.Count);
            Destroy(this);
        }
        Instance = this;
    }


    private void Update()
    {
        //刷新左手信息
        CheckPadButtons(leftPad,leftPadInfo);
        //刷新右手信息
        CheckPadButtons(rightPad, rightPadInfo);
    }


    void CheckPadButtons(InputDevice _Pad,GamePadInfo _PadInfo)
    {
        if (GamepadFactory.IsTriggerPressed(_Pad))
        {
            if (_PadInfo.isTriggerButtonDown == false && _PadInfo.isTriggerButtonHolding == false)
            {
                _PadInfo.isTriggerButtonDown = true;
            }
            else
            {
                _PadInfo.isTriggerButtonDown = false;
            }
            _PadInfo. isTriggerButtonHolding = true;
            _PadInfo.isTriggerButtonUp = false;
        }
        else
        {
            if (_PadInfo.isTriggerButtonUp == false && _PadInfo.isTriggerButtonHolding == true)
            {
                _PadInfo.isTriggerButtonUp = true;
            }
            else
            {
                _PadInfo.isTriggerButtonUp = false;
            }
            _PadInfo.isTriggerButtonDown = false;
            _PadInfo.isTriggerButtonHolding = false;
        }
        if (GamepadFactory.IsGripPressed(_Pad))
        {
            if (_PadInfo.isGripButtonDown == false && _PadInfo.isGripButtonHolding == false)
            {
                _PadInfo.isGripButtonDown = true;
            }
            else
            {
                _PadInfo.isGripButtonDown = false;
            }
            _PadInfo.isGripButtonHolding = true;
            _PadInfo.isGripButtonUp = false;
        }
        else
        {
            if (_PadInfo.isGripButtonUp == false && _PadInfo.isGripButtonHolding == true)
            {
                _PadInfo.isGripButtonUp = true;
            }
            else
            {
                _PadInfo.isGripButtonUp = false;
            }
            _PadInfo.isGripButtonDown = false;
            _PadInfo.isGripButtonHolding = false;
        }
        if (GamepadFactory.IsAXPressed(_Pad))
        {
            if (_PadInfo.isAXButtonDown == false && _PadInfo.isAXButtonHolding == false)
            {
                _PadInfo.isAXButtonDown = true;
            }
            else
            {
                _PadInfo.isAXButtonDown = false;
            }
            _PadInfo.isAXButtonHolding = true;
            _PadInfo.isAXButtonUp = false;
        }
        else
        {
            if (_PadInfo.isAXButtonUp == false && _PadInfo.isAXButtonHolding == true)
            {
                _PadInfo.isAXButtonUp = true;
            }
            else
            {
                _PadInfo.isAXButtonUp = false;
            }
            _PadInfo.isAXButtonDown = false;
            _PadInfo.isAXButtonHolding = false;

        }

        if (GamepadFactory.IsBYPressed(_Pad))
        {
            if (_PadInfo.isBYButtonDown == false && _PadInfo.isBYButtonHolding == false)
            {
                _PadInfo.isBYButtonDown = true;
            }
            else
            {
                _PadInfo.isBYButtonDown = false;
            }
            _PadInfo.isBYButtonHolding = true;
            _PadInfo.isBYButtonUp = false;
        }
        else
        {
            if (_PadInfo.isBYButtonUp == false && _PadInfo.isBYButtonHolding == true)
            {
                _PadInfo.isBYButtonUp = true;
            }
            else
            {
                _PadInfo.isBYButtonUp = false;
            }
            _PadInfo.isBYButtonDown = false;
            _PadInfo.isBYButtonHolding = false;
        }

        _PadInfo.joyStickValue = GamepadFactory.GetJoystickValue(_Pad);
    }
}

[System.Serializable]
public class GamePadInfo
{

    public bool isTriggerButtonDown = false;
    public bool isTriggerButtonUp = false;
    public bool isTriggerButtonHolding = false;

    public bool isGripButtonDown = false;
    public bool isGripButtonUp = false;
    public bool isGripButtonHolding = false;

    public bool isAXButtonDown = false;
    public bool isAXButtonUp = false;
    public bool isAXButtonHolding = false;

    public bool isBYButtonDown = false;
    public bool isBYButtonUp = false;
    public bool isBYButtonHolding = false;

    public Vector2 joyStickValue = Vector3.zero;
}

public class GamepadFactory
{
    //扳机键
    public static bool IsTriggerPressed(InputDevice inputDevice)
    {

        bool isPressed = false;
        inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out isPressed);
        return isPressed;
    }
    public static float GetTriggerValue(InputDevice inputDevice)
    {

        float value = 0;
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out value);
        return value;
    }
    //抓握键
    public static bool IsGripPressed(InputDevice inputDevice)
    {

        bool isPressed = false;
        inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out isPressed);
        return isPressed;
    }
    public static float GetGripValue(InputDevice inputDevice)
    {

        float value = 0;
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out value);
        return value;
    }
    //摇杆
    public static bool IsJoystickPressed(InputDevice inputDevice)
    {

        bool isPressed = false;
        inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out isPressed);
        return isPressed;
    }
    public static Vector2 GetJoystickValue(InputDevice inputDevice)
    {

        Vector2 value = Vector2.zero;
        inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out value);
        return value;
    }
    //ax键
    public static bool IsAXPressed(InputDevice inputDevice)
    {

        bool isPressed = false;
        inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);
        return isPressed;
    }
    //by键
    public static bool IsBYPressed(InputDevice inputDevice)
    {

        bool isPressed = false;
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out isPressed);
        return isPressed;
    }

}
/* 按键	输入事件
Menu	CommonUsages.menuButton
Trigger	CommonUsages.TriggerButton
Grip	CommonUsages.GripButton
Joystick	CommonUsages.primary2DAxisClick
X/A	CommonUsages.primaryButton
Y/B	CommonUsages.secondaryButton
 
  一体机按键
  返回键	KeyCode.Escape
确认键	KeyCode.JoystickButton0
Home 键	KeyCode.Home（系统占用，默认不开放）
音量增加键	Android标准VOLUME_UP（系统占用，默认不开放）
音量减小键	Android标准VOLUME_DOWN（系统占用，默认不开放）
*/
