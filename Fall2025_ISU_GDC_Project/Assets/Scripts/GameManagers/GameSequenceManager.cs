using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Cinemachine;
using Image = UnityEngine.UI.Image;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;

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
    //[SerializeField] private bool gameOver = false;
    [SerializeField] private CinemachineCamera cmc;

    [SerializeField] private InputManager inputManager;

    //Runs before start
    void Awake()
    {
        //disable victory canvas on awake
        victoryCanvas.GetComponent<Canvas>().enabled = false;
        //spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        podiumStuff.SetActive(false);
        spriteImage = sprite.GetComponent<Image>();
    }

    private void Start()
    {

    }

    /* 
    /  Method that handles all victory stuff. Subscribed to playerDeath Event
    /  playerDeath event is located in PlayerHealth.cs
    */
    public void doVictoryStuff()
    {
        victoryCanvas.GetComponent<Canvas>().enabled = true;
        //Need to find winning player(s)
        List<GameObject> currPlayers = inputManager.GetPlayersCurrentlyInGame();

        //Set podium sprite to winner; Currently only will work for 1v1
        for (int i = 0; i < currPlayers.Count; i++)
        {
            if (currPlayers[i].GetComponent<PlayerHealth>().GetStocks() != 0)
            {
                SpriteRenderer winPlayerSR = currPlayers[i].GetComponent<SpriteRenderer>();
                spriteImage.sprite = winPlayerSR.sprite;
                spriteImage.color = winPlayerSR.color;
            }
        }
        
        StartCoroutine(VictoryAnims(.01f, cmc, .05f, 1f));
    }

    /* Zoom in on winning player(s) <- (not working right) | Play UI Animation | Enable Podium (need more work later)
    /  waitTime - time between decrements of lens size
    /  @param camera - camera to alter
    /  @param decrementAmount - amount to decrement c ,amera lens size after each period of waitTime 
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
