using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Cinemachine;
using Image = UnityEngine.UI.Image;
using System.Runtime.CompilerServices;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class GameSequenceManager : MonoBehaviour
{
    //this script handles all the logic for controlling the sequence of a normal round
    //(player input connection -> character select -> battle -> victory screen)

    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private GameObject podiumStuff;
    [SerializeField] private GameObject sprite;
    //private SpriteRenderer spriteRenderer;
    private Image spriteImage;
    [SerializeField] private Animation winAnimation;
    [SerializeField] private CinemachineCamera cmc;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private TextMeshProUGUI victoryText;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private RectTransform countdownTransform;

    [SerializeField] private GameObject playerInfoGroup;

    [SerializeField] private AudioSource levelAudioSource;
    [SerializeField] private AudioClip charSelectSong;
    [SerializeField] private AudioClip levelFightSong;

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

        levelAudioSource.clip = charSelectSong;
        levelAudioSource.Play();
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
        //Debug.Log("do char select now");
        DisableAllMenus();
        charSelectMenu.SetActive(true);
    }

    private void InitiateFighting()
    {
        levelAudioSource.clip = levelFightSong;
        levelAudioSource.Play();

        //Each player has made their character selection by now
        //We need to add a prefab instance of the PlayerVariant that corresponds to their character choice
        var charSelectManager = FindFirstObjectByType<CharacterSelectManager>();
        var inputManager = FindFirstObjectByType<InputManager>(); //InputManager stores references to PlayerTemplate GameObjects (this should be changed soon)
        foreach (GameObject playerObj in inputManager.GetPlayerInputsCurrentlyInGame().ToList())
        {
            var template = playerObj.GetComponentInParent<PlayerSpawningTemplate>();
            PlayerCharacter playerCharacter = charSelectManager.GetCharacterSelectionFromPlayerInput(playerObj.GetComponent<PlayerInput>());
            template.EnablePlayerObjectFromPlayerCharacter(playerCharacter);
        }

        //set all players spawn points (the points at which they start in the game)
        List<GameObject> currPlayers = FindFirstObjectByType<InputConnectionManager>().GetCurrentPlayerObjectsInGame();
        FindFirstObjectByType<PlayerSpawnPointManager>().SetPlayerSpawnPoints(currPlayers);


        //set the target group for the camera, now that players are spawned in
        List<GameObject> players = FindFirstObjectByType<InputConnectionManager>().GetCurrentPlayerObjectsInGame();
        FindFirstObjectByType<CameraTarget>().SetCameraTargets(players);

        DisableAllMenus();


        //make all players dormant
        var playerStates = FindObjectsByType<PlayerState>(FindObjectsSortMode.None);
        foreach (var playerState in playerStates)
        {
            playerState.ChangePlayerState(PlayerState.PlayerStateEnum.Dormant);
        }


        //set all playercursors to inactive
        var playerCursors = FindObjectsByType<PlayerCursor>(FindObjectsSortMode.None);
        foreach (var cursor in playerCursors)
        {
            cursor.gameObject.SetActive(false);
        }

        //initialize ultimate tracking
        List<PlayerInput> playerInputList = new List<PlayerInput>();
        foreach (var playerInputObj in inputManager.GetPlayerInputsCurrentlyInGame())
        {
            Debug.Log(playerInputObj.GetComponent<PlayerInput>() == null);
            playerInputList.Add(playerInputObj.GetComponent<PlayerInput>());
        }
        FindFirstObjectByType<UltimateTrackerManager>().InitializeDictionary(playerInputList);

        //create player info UI
        if (FindFirstObjectByType<PlayerUI_Manager>() != null)
        {
            FindFirstObjectByType<PlayerUI_Manager>().CreateAllPlayerUI();
        }

        //Initiate countdown
        StartCoroutine(runCountdown());

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
        List<GameObject> currPlayers = FindFirstObjectByType<InputConnectionManager>().GetCurrentPlayerObjectsInGame();
        GameObject winner = null;
        foreach (var player in currPlayers) { 
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph.GetTotalStocks() > 0) { 
                winner = player;
            }
        }

        List<PlayerCharacter> playerCharacterList = FindFirstObjectByType<CharacterSelectManager>().getPlayerCharacterList();
        PlayerCharacter winningCharacter = null;
        foreach (var character in playerCharacterList) {
            if (winner.name == character.characterName) { 
                winningCharacter = character;
            }
        }

        //hardcoded right now as default, will be overwritten by ScriptableObject Data later on
        string winText = winningCharacter.victoryText;
        StartCoroutine(DisplayTextStaggered(winText));

        playerInfoGroup.SetActive(false);

        //Set podium sprite to winner; Currently only will work for 1v1
        SpriteRenderer winPlayerSR = null;
        /*        for (int i = 0; i < currPlayers.Count; i++)
                {
                    if (currPlayers[i].GetComponent<PlayerHealth>().GetStocks() != 0)
                    {
                        winPlayerSR = currPlayers[i].GetComponent<SpriteRenderer>();
                    }

                }*/
        

        //Should ensure someone always displays
        if (winPlayerSR == null)
        {
            winPlayerSR = currPlayers[0].GetComponent<SpriteRenderer>();
        }
        spriteImage.sprite = winningCharacter.victorySprite;
        //spriteImage.color = winPlayerSR.color;
        //These values work best for camera zoom in. 
        StartCoroutine(VictoryAnims(.01f, cmc, .05f, 1f));
    }

    private IEnumerator DisplayTextStaggered(string textToShow)
    {
        string textShown = "";

        for (int i = 0; i < textToShow.Length; i++)
        {
            textShown += textToShow[i];
            victoryText.text = textShown;
            yield return new WaitForSeconds(.05f);
        }
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
        yield return new WaitForSeconds(4.5f);
        victoryText.gameObject.transform.parent.gameObject.SetActive(false);
        winAnimation.Play();
        yield return new WaitForSeconds(1.5f);
        podiumStuff.SetActive(true);
    }

    [SerializeField] private int countdownStartNum = 3; //The number the countdown starts at
    [SerializeField] private int countdownStartSize = 100; //The font size each number starts at
    [SerializeField] private int countdownFinalSize = 1000; //The font size each number ends at
    [SerializeField] private string endOfCountdownText = "GO!"; //The word displayed when countdown reaches 0
    private bool changeSize = false;
    private bool shake = false;
    private int count = 0;

    private IEnumerator runCountdown()
    {
        while (countdownStartNum > 0)
        {
            countdownText.fontSize = countdownStartSize;
            countdownText.text = countdownStartNum.ToString();

            changeSize = true;
            yield return new WaitForSeconds(1);
            countdownStartNum--;
        }
        changeSize = false;
        countdownText.text = endOfCountdownText;
        countdownText.fontSize = countdownFinalSize;
        shake = true;
        yield return new WaitForSeconds(.5f);
        countdownText.text = "";
        shake = false;

        //make all currently join players active
        var playerStates = FindObjectsByType<PlayerState>(FindObjectsSortMode.None);
        foreach (var playerState in playerStates)
        {
            playerState.ChangePlayerState(PlayerState.PlayerStateEnum.Active);
        }

    }

    private void Update()
    {
        //effects for countdown at round start
        if (changeSize)
        {
            countdownText.fontSize += (countdownFinalSize - countdownStartSize) * Time.deltaTime;
        }

        if (shake)
        {
            if (count == 10)
            {
                countdownTransform.localPosition = new Vector3(UnityEngine.Random.Range(countdownTransform.position.x - 10, countdownTransform.position.x + 10), UnityEngine.Random.Range(countdownTransform.position.y - 10, countdownTransform.position.y + 10), 0);
                count = 0;
            }
            else
            {
                count++;
            }
        }
    }


}
