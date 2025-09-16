using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputConnectionManager : MonoBehaviour
{
    [SerializeField] private GameObject deviceIdGroup;
    [SerializeField] private GameObject deviceIdPrefab;

    [SerializeField] private Sprite keyboardIcon;
    [SerializeField] private Sprite controllerIcon;
    private int numConnected = 0;

    private Dictionary<PlayerInput, Color> playerInputMapToColor = new Dictionary<PlayerInput, Color>();

    private enum DeviceType
    {
        Keyboard,
        Controller
    }

    public void OnPlayerJoined(PlayerInput pi)
    {
        //this is really bad and hard coded. will fix later
        playerInputMapToColor.Add(pi, Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f));

        numConnected++;

        foreach(var device in pi.devices)
        {
            //Debug.Log("device -> " + device.name);
            if (device.name.Contains("Keyboard"))
            {
                //spawn an instance of device identifier with attributes of keyboard
                AddDeviceIdentifierToGroup(DeviceType.Keyboard, pi);

            }
            else if (device.name.Contains("XInputControllerWindows"))
            {
                //spawn an instance of device identifier with attributes of controller
                AddDeviceIdentifierToGroup(DeviceType.Controller, pi);
            }
        }
    }

    public void OnPlayerLeft(PlayerInput pi)
    {

    }

    private void AddDeviceIdentifierToGroup(DeviceType deviceType, PlayerInput pi)
    {
        var deviceIdentifier = Instantiate(deviceIdPrefab, deviceIdGroup.transform);

        string deviceText = "err: text not found";
        Sprite deviceImage = null;

        if (deviceType == DeviceType.Keyboard)
        {
            deviceText = "Player " + numConnected + "\nKeyboard\nConnected";
            deviceImage = keyboardIcon;
        }
        else if (deviceType == DeviceType.Controller)
        {
            deviceText = "Player " + numConnected + "\nController\nConnected";
            deviceImage = controllerIcon;
        }

        deviceIdentifier.GetComponent<InputDeviceIdentifier>().UpdateIdentifierData(deviceImage, deviceText, playerInputMapToColor[pi]);
    }
}
