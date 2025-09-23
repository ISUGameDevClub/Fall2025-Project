using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSequenceManager : MonoBehaviour
{
    //this script handles all the logic for controlling the sequence of a normal round
    //(player input connection -> character select -> battle)

    [SerializeField] private GameObject inputConnectionMenu;
    [SerializeField] private GameObject charSelectMenu;

    private List<Action> sequenceActions = new List<Action>();
    int curAction = 0;

    private void Start()
    {
        sequenceActions.Add(InitiateCharacterSelect);
        sequenceActions.Add(InitiateFighting);
    }

    public void CallAndForwardSequenceAction()
    {
        sequenceActions[curAction].Invoke();
        curAction++;
    }
    

    private void InitiateInputConnection()
    {

    }

    private void InitiateCharacterSelect()
    {
        Debug.Log("do char select now");
        DisableAllMenus();
        charSelectMenu.SetActive(true);
    }

    private void InitiateFighting()
    {
        DisableAllMenus();

        //make all currently join players active
        var playerStates = FindObjectsByType<PlayerState>(FindObjectsSortMode.None);
        foreach (var playerState in playerStates)
        {
            playerState.ChangePlayerState(PlayerState.PlayerStateEnum.Active);
        }

        //set all playercursors to inactive
        var playerCursors = FindObjectsByType<PlayerCursor>(FindObjectsSortMode.None);
        foreach (var cursor in playerCursors)
        {
            cursor.gameObject.SetActive(false);
        }
    }

    private void DisableAllMenus()
    {
        inputConnectionMenu.SetActive(false);
        charSelectMenu.SetActive(false);
    }
}
