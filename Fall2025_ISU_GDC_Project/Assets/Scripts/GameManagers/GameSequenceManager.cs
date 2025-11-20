using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using Image = UnityEngine.UI.Image;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.Events;

/*
===================================================================================

GameSequenceManager

===================================================================================
*/
/// <summary>
/// 
/// </summary>

public class GameSequenceManager : MonoBehaviour
{
	private static WaitForSeconds _waitForSeconds_1 = new WaitForSeconds(.1f);
	private static WaitForSeconds _waitForSeconds1_5 = new WaitForSeconds(1.5f);
	private static WaitForSeconds _waitForSeconds3 = new WaitForSeconds(3f);
	private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1);
	private static WaitForSeconds _waitForSeconds_5 = new WaitForSeconds(.5f);

	//this script handles all the logic for controlling the sequence of a normal round
	//(player input connection -> character select -> battle -> victory screen)

	public enum Sequence : uint
	{
		PlayerInputConnection,
		CharacterSelect,
		Battle,
		VictoryScreen
	};

	[SerializeField] private Canvas victoryCanvas;
	[SerializeField] private GameObject podiumStuff;
	[SerializeField] private GameObject sprite;
	[SerializeField] private Animation winAnimation;
	[SerializeField] private CinemachineCamera cmc;
	[SerializeField] private InputManager inputManager;
	[SerializeField] private TextMeshProUGUI victoryText;
	[SerializeField] private int countdownStartNum = 3; //The number the countdown starts at
	[SerializeField] private int countdownStartSize = 100; //The font size each number starts at
	[SerializeField] private int countdownFinalSize = 1000; //The font size each number ends at
	[SerializeField] private string endOfCountdownText = "GO!"; //The word displayed when countdown reaches 0

	[SerializeField] private TextMeshProUGUI countdownText;
	[SerializeField] private RectTransform countdownTransform;

	public static Sequence State { get; private set; } = Sequence.PlayerInputConnection;

	private Image spriteImage;
	private bool changeSize = false;
	private bool shake = false;
	private int count = 0;

	private static readonly Dictionary<Sequence, UnityEvent> sequenceActions = new Dictionary<Sequence, UnityEvent>();

	private void Awake()
	{
		sequenceActions.Clear();

		//disable victory canvas on awake
		victoryCanvas.GetComponent<Canvas>().enabled = false;
		podiumStuff.SetActive(false);
		spriteImage = sprite.GetComponent<Image>();

		RegisterSequenceChangeCallback(Sequence.VictoryScreen, FindFirstObjectByType<GameSequenceManager>().doVictoryStuff);
		RegisterSequenceChangeCallback(Sequence.Battle, StartCountdown);
	}

	public static void RegisterSequenceChangeCallback(Sequence sequence, UnityAction callback)
	{
		if (!sequenceActions.TryGetValue(sequence, out UnityEvent @event))
		{
			@event = new UnityEvent();
			sequenceActions.Add(sequence, @event);
		}
		Debug.LogFormat($"GameSequenceManager.RegisterSequenceChangeCallback: callback {callback.Method.Name} registered for sequence {sequence}");
		@event.AddListener(callback);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AdvanceSequence()
	{
		Debug.LogFormat("GameSequenceManager.AdvanceSequence: advancing game state...");
		SetSequence(State + 1);
	}

	public static void SetSequence(Sequence sequence)
	{
		Debug.LogFormat($"GameSequenceManager.SetSequence: Changing from sequence {State} to {sequence}...");
		if (!sequenceActions.TryGetValue(sequence, out UnityEvent callbackBatch))
		{
			Debug.LogWarningFormat($"GameSequenceManager.SetSequence: no callbacks for sequence {sequence}");
			return;
		}
		State = sequence;
		callbackBatch?.Invoke();
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

		//hardcoded right now as default, will be overwritten by ScriptableObject Data later on
		string winText = "Eat Justice";
		StartCoroutine(DisplayTextStaggered(winText));

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
		winPlayerSR = winPlayerSR != null ? winPlayerSR : currPlayers[0].GetComponent<SpriteRenderer>();
		spriteImage.sprite = winPlayerSR.sprite;
		spriteImage.color = winPlayerSR.color;
		//These values work best for camera zoom in. 
		StartCoroutine(VictoryAnims(.01f, cmc, .05f, 1f));
	}



	private IEnumerator DisplayTextStaggered(string textToShow)
	{
		string textShown = String.Empty;

		for (int i = 0; i < textToShow.Length; i++)
		{
			textShown += textToShow[i];
			victoryText.text = textShown;
			yield return _waitForSeconds_1;
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
		yield return _waitForSeconds3;
		victoryText.gameObject.transform.parent.gameObject.SetActive(false);
		winAnimation.Play();
		yield return _waitForSeconds1_5;
		podiumStuff.SetActive(true);
	}

	public void StartCountdown()
	{
		StartCoroutine(runCountdown());
	}

	private IEnumerator runCountdown()
	{
		while (countdownStartNum > 0)
		{
			countdownText.fontSize = countdownStartSize;
			countdownText.text = countdownStartNum.ToString();

			changeSize = true;
			yield return _waitForSeconds1;
			countdownStartNum--;
		}
		changeSize = false;
		countdownText.text = endOfCountdownText;
		countdownText.fontSize = countdownFinalSize;
		shake = true;
		yield return _waitForSeconds_5;
		countdownText.text = String.Empty;
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
