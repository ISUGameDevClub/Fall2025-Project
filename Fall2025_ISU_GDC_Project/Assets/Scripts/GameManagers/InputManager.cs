using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //i(jake) will probably delete this class, as it has no purpose. All input storage logic should be handled in InputConnectionManager
    //keeping it now for debugging purposes

    [SerializeField] private List<GameObject> curPlayers;
    [SerializeField] private List<PlayerInput> curPlayerInputs;

    public void PlayerJoined(PlayerInput pi)
    {
        curPlayers.Add(pi.gameObject);
        curPlayerInputs.Add(pi);
    }

    public void PlayerLeft(PlayerInput pi)
    {
        curPlayers.Remove(pi.gameObject);
        curPlayerInputs.Remove(pi);
    }

    public List<GameObject> GetPlayersCurrentlyInGame()
    {
        return curPlayers;
    }
}
