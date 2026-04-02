using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UserInterface {
	/*
	===================================================================================
	
	PauseManager
	
	===================================================================================
	*/
	/// <summary>
	/// A manager for the <see cref="PauseMenu"/>
	/// </summary>
	/// <remarks>
	/// This should really be a GameStateManager...
	/// This will need some refactoring later on
	/// </remarks>

	public sealed class PauseManager : MonoBehaviour {
		[Header( "Dependencies" )]
		[SerializeField]
		private GameObject PauseMenuPrefab;
		[SerializeField]
		private InputActionReference PauseActionReference;

		/// <summary>
		/// Event fired when the game enters the paused state.
		/// </summary>
		public static event Action OnGamePaused;

		/// <summary>
		/// Event fired when the game resumes from the paused state.
		/// </summary>
		public static event Action OnGameResumed;

		/// <summary>
		/// Event fired when the "Quit to Title" request is confirmed from the pause menu.
		/// </summary>
		public static event Action OnQuitToTitleRequest;

		/// <summary>
		/// Whether the game is paused or not
		/// </summary>
		public static bool IsGamePaused { get; private set; } = false;

		private PauseMenu PauseMenu;
		private InputAction PauseAction;

		private static PauseManager Instance;

		/*
		===============
		Awake
		===============
		*/
		private void Awake() {
			if ( Instance != null ) {
				Debug.LogError( "PauseManager singleton created twice." );
				Destroy( gameObject );
				return;
			}
			Instance = this;

			if ( PauseActionReference == null ) {
				throw new UnassignedReferenceException( nameof( PauseActionReference ) );
			}

			PauseAction = PauseActionReference.action;
			if ( PauseAction == null ) {
				throw new ArgumentNullException( nameof( PauseAction ) );
			}

			Debug.Log( $"PauseManager initialized on '{gameObject.name}'. Pause key binding: '{PauseAction.bindings[ 0 ].path}'." );
		}

		/*
		===============
		OnEnable
		===============
		*/
		private void OnEnable() {
			if ( PauseAction != null ) {
				PauseAction.performed += OnPauseInput;
				PauseAction.Enable();
			}
		}

		/*
		===============
		OnDisable
		===============
		*/
		private void OnDisable() {
			if ( PauseAction != null ) {
				PauseAction.performed -= OnPauseInput;
				PauseAction.Disable();
			}
		}

		/*
		===============
		OnDestroy
		===============
		*/
		/// <summary>
		/// Called upon object destruction
		/// </summary>
		private void OnDestroy() {
			if ( Instance == this ) {
				Instance = null;
				OnGamePaused = null;
				OnGameResumed = null;
				OnQuitToTitleRequest = null;
				Debug.Log( "PauseManager static events cleared." );
			}
		}

		/*
		===============
		OnPauseInput
		===============
		*/
		/// <summary>
		/// Called whenever we receive valid pause action input
		/// </summary>
		/// <param name="context"></param>
		private void OnPauseInput( InputAction.CallbackContext context ) {
			if ( !context.performed ) {
				return;
			}
			TogglePauseState();
		}

		/*
		===============
		TogglePauseState
		===============
		*/
		/// <summary>
		/// Toggles the <see cref="PauseMenu"/>'s state
		/// </summary>
		private void TogglePauseState() {
			if ( IsGamePaused ) {
				ResumeGame();
			} else {
				PauseGame();
			}
		}

		/*
		===============
		PauseGame
		===============
		*/
		private void PauseGame() {
			if ( IsGamePaused ) {
				Debug.LogWarning( "Attempted to pause the game, but it is already paused." );
				return;
			}

			Debug.Log( "=== GAME PAUSING ===" );

			if ( PauseMenu == null ) {
				PauseMenu = PauseMenuPrefab.GetComponent<PauseMenu>();
				if ( PauseMenu == null ) {
					throw new MissingComponentException( nameof( PauseMenu ) );
				}
				PauseMenu.ResumePressed += ResumeGame;
				PauseMenu.QuitToTitlePressed += HandleQuitToTitleRequest;
			}

			IsGamePaused = true;

			Time.timeScale = 0.0f;
			Time.fixedDeltaTime = 0.0f;
			PauseMenu.Pause();

			OnGamePaused?.Invoke();
		}

		/*
		===============
		ResumeGame
		===============
		*/
		private void ResumeGame() {
			if ( !IsGamePaused ) {
				Debug.LogWarning( "Attempted to resume the game, but it is not paused." );
				return;
			}

			Debug.Log( "=== GAME RESUMING ===" );

			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 1.0f;

			PauseMenu.UnPause();
			IsGamePaused = false;

			OnGameResumed?.Invoke();
		}

		/*
		===============
		HandleQuitToTitleRequest
		===============
		*/
		private void HandleQuitToTitleRequest() {
			OnQuitToTitleRequest?.Invoke();
		}
	}
};