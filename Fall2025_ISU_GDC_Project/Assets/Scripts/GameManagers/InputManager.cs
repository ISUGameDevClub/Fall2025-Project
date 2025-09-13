using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> curPlayers;

    public void PlayerJoined(PlayerInput pi)
    {
        curPlayers.Add(pi.gameObject);
    }

    public void PlayerLeft(PlayerInput pi)
    {
        curPlayers.Remove(pi.gameObject);
    }

    public List<GameObject> GetPlayersCurrentlyInGame()
    {
        return curPlayers;
    }
}
