using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputConnectionManager : MonoBehaviour
{
    [SerializeField] private GameObject deviceIdGroup;
    [SerializeField] private GameObject deviceIdPrefab;

    [SerializeField] private Sprite keyboardIcon;
    [SerializeField] private Sprite controllerIcon;

    [SerializeField] private GameObject controllerCursorPrefab;
    [SerializeField] private GameObject canvasObj;
    private int numConnected = 0;

    private Dictionary<int, PlayerInput> playerIDMapToPlayerInput = new Dictionary<int, PlayerInput>();
    private Dictionary<PlayerInput, Color> playerInputMapToColor = new Dictionary<PlayerInput, Color>();

    public List<GameObject> realPlayers = new List<GameObject>(); //used to store the literal GameObjects for active players, not the PlayerSpawningTemplate

    private enum DeviceType
    {
        Keyboard,
        Controller
    }

    public void OnPlayerJoined(PlayerInput pi)
    {
        pi.actions.Enable();

        int playerID = Random.Range(1000, 9999);
        while (playerIDMapToPlayerInput.ContainsKey(playerID))
        {
            //try again, we got the same ID (small chance, but very possible edge case)
            playerID = Random.Range(1000, 9999);
        }
        playerIDMapToPlayerInput.Add(playerID, pi);
        pi.gameObject.name = "Player | ID: " + playerID;

        Color playerColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
        playerInputMapToColor.Add(pi, playerColor);
        if (pi.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            pi.gameObject.GetComponent<SpriteRenderer>().color = playerColor;
        }

        //make the player Inactive by default, so that they may become active when character select is over
        //pi.gameObject.GetComponent<PlayerState>().ChangePlayerState(PlayerState.PlayerStateEnum.Inactive);

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
        //spawn a cursor
        GameObject playerCursor = Instantiate(controllerCursorPrefab, canvasObj.transform);
        playerCursor.GetComponent<PlayerCursor>().AssignPlayerInput(pi);
        playerCursor.GetComponent<PlayerCursor>().AssignColor(playerColor);
    }

    public void OnPlayerLeft(PlayerInput pi)
    {
        //this will be empty for now, but we likely will need logic here eventually
    }

    public Color GetColorFromPlayerInput(PlayerInput pi)
    {
        return playerInputMapToColor[pi];
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
