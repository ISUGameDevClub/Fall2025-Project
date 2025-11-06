using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //This class is used to store the literal PlayerInputs components that are active in the game

    [SerializeField] private List<GameObject> curPlayerInputGameObjects;
    [SerializeField] private List<PlayerInput> curPlayerInputs;

    public void PlayerJoined(PlayerInput pi)
    {
        curPlayerInputGameObjects.Add(pi.gameObject);
        curPlayerInputs.Add(pi);
    }

    public void PlayerLeft(PlayerInput pi)
    {
        curPlayerInputGameObjects.Remove(pi.gameObject);
        curPlayerInputs.Remove(pi);
    }

    public List<GameObject> GetPlayerInputsCurrentlyInGame()
    {
        return curPlayerInputGameObjects;
    }
}
