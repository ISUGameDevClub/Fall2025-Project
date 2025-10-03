using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Cinemachine;
using Image = UnityEngine.UI.Image;
using System.Runtime.CompilerServices;

public class GameSequenceManager : MonoBehaviour
{
    //this script handles all the logic for controlling the sequence of a normal round
    //(player input connection -> character select -> battle -> victory screen -> character select)

    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private GameObject podiumStuff;
    [SerializeField] private GameObject sprite;
    //private SpriteRenderer spriteRenderer;
    private Image spriteImage;
    [SerializeField] private Animation winAnimation;
    [SerializeField] private CinemachineCamera cmc;

    [SerializeField] private InputManager inputManager;

    //Runs before start
    void Awake()
    {
        //disable victory canvas on awake
        victoryCanvas.GetComponent<Canvas>().enabled = false;
        podiumStuff.SetActive(false);
        spriteImage = sprite.GetComponent<Image>();
    }

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

    /* 
    /  Listener Method that handles all victory stuff. Subscribed to playerDeath Event
    /  playerDeath event is located in PlayerHealth.cs
    */
    public void doVictoryStuff()
    {
        victoryCanvas.GetComponent<Canvas>().enabled = true;
        //Need to find winning player(s)
        List<GameObject> currPlayers = inputManager.GetPlayersCurrentlyInGame();

        //Set podium sprite to winner; Currently only will work for 1v1
        SpriteRenderer winPlayerSR = null;
        for (int i = 0; i < currPlayers.Count; i++)
        {
            if (currPlayers[i].GetComponent<PlayerHealth>().GetStocks() != 0)
            {
                winPlayerSR = currPlayers[i].GetComponent<SpriteRenderer>();
            }
            
        }
        //Should ensure someone always displays
        if (winPlayerSR == null)
        {
            winPlayerSR = currPlayers[0].GetComponent<SpriteRenderer>();
        }
        spriteImage.sprite = winPlayerSR.sprite;
        spriteImage.color = winPlayerSR.color;
        //These values work best for camera zoom in. 
        StartCoroutine(VictoryAnims(.01f, cmc, .05f, 1f));
    }

    /* Zoom in on winning player(s) <- (not working right so disabled) | Play UI Animation | Enable Podium (need more work later)
    /  @param waitTime - time between decrements of lens size
    /  @param camera - camera to alter
    /  @param decrementAmount - amount to decrement camera lens size after each period of waitTime 
    /  @param finalSize - final lens size (loop will stop once this size is reached) (lens size starts at 5f, I recc stopping at 1f)
    */
    IEnumerator VictoryAnims(float waitTime, CinemachineCamera camera, float decrementAmount, float finalSize)
    {
        // while (camera.Lens.OrthographicSize > finalSize)
        // {
        //     camera.Lens.OrthographicSize -= decrementAmount;
        //     yield return new WaitForSeconds(waitTime);
        // }
        //reset camera OS
        yield return new WaitForSeconds(3f);
        winAnimation.Play();
        yield return new WaitForSeconds(1.5f);
        podiumStuff.SetActive(true);
    }




    
}
