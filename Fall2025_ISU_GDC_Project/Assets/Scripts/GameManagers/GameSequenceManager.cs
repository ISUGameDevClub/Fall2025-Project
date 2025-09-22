using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSequenceManager : MonoBehaviour
{
    //this script handles all the logic for controlling the sequence of a normal round
    //(player input connection -> character select -> battle)

    [SerializeField] private GameObject inputConnectionCanvas;
    [SerializeField] private GameObject charSelectCanvas;

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
        charSelectCanvas.SetActive(true);
    }

    private void InitiateFighting()
    {

    }

    private void DisableAllMenus()
    {
        inputConnectionCanvas.SetActive(false);
        charSelectCanvas.SetActive(false);
    }
}
