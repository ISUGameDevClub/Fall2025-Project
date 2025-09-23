using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UserInterface {
	/*
	===================================================================================
	
	PauseMenu

	===================================================================================
	*/
	/// <summary>
	/// PauseMenu handler class for the game
	/// </summary>

	public class PauseMenu : MonoBehaviour {
		public enum Status : byte {
			Hidden,
			Active,
			QuitToTitle,
			QuitGame
		};

		/// <summary>
		/// The <see cref="CanvasGroup"/> of the PauseMenu User Interface element.
		/// </summary>
		/// <remarks>
		/// Used to hide and show the PauseMenu
		/// </remarks>
		[SerializeField]
		private CanvasGroup PauseMenuGroup;

		[SerializeField]
		private InputActionReference PauseActionReference;

		[SerializeField]
		private Canvas PapaPauseMenu;
		[SerializeField]
		private Canvas ConfirmMenu;
		
		private Status State = Status.Hidden;

		private static Action<bool> _cachedCallback = null;

		/*
		===============
		Pause
		===============
		*/
		public void Pause() {
			// ... it works, but this right here for some reason is "unassigned"
			PauseMenuGroup.alpha = 1.0f;
			
			ConfirmMenu.enabled = false;
			PapaPauseMenu.enabled = true;

			State = Status.Active;

			Time.timeScale = 0.0f;
			Time.fixedDeltaTime = 0.0f;
		}

		/*
		===============
		UnPause
		===============
		*/
		/// <summary>
		/// Hides the PauseMenu's canvas group and the ConfirmMenu's canvas group
		/// </summary>
		public void UnPause() {
			PauseMenuGroup.alpha = 0.0f;

			// ensure we don't have a hanging ConfirmMenu
			ConfirmMenu.enabled = false;
			PapaPauseMenu.enabled = false;

			State = Status.Hidden;

			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 1.0f;
		}

		/*
		===============
		OnResumeButtonClicked
		===============
		*/
		/// <summary>
		/// Callback function for ResumeButton OnClick event
		/// </summary>
		public void OnResumeButtonClicked() {
			if ( GameStateManager.Instance.GameState != GameState.Paused ) {
				return;
			}
			GameStateManager.Instance.UnPauseGame();
		}

		/*
		===============
		OnQuitToTitleButtonClicked
		===============
		*/
		/// <summary>
		/// Callback for when QuitToTitleButton is pressed
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <see cref="ConfirmMen"/> is nulls</exception>
		public void OnQuitToTitleButtonClicked() {
			if ( GameStateManager.Instance.GameState != GameState.Paused ) {
				return;
			}
			OnExitButtonClicked( Status.QuitToTitle );
		}

		/*
		===============
		OnQuitGameButtonClicked
		===============
		*/
		/// <summary>
		/// Callback for when QuitGameButton is pressed
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <see cref="ConfirmMenu"/> is null</exception>
		public void OnQuitGameButtonClicked() {
			if ( GameStateManager.Instance.GameState != GameState.Paused ) {
				return;
			}
			OnExitButtonClicked( Status.QuitGame );
		}

		/*
		===============
		OnExitButtonClicked
		===============
		*/
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		private void OnExitButtonClicked( Status newStatus ) {
			if ( PapaPauseMenu == null ) {
				throw new ArgumentNullException( nameof( PapaPauseMenu ) );
			}
			if ( ConfirmMenu == null ) {
				throw new ArgumentNullException( nameof( ConfirmMenu ) );
			}
			PapaPauseMenu.enabled = false;
			ConfirmMenu.enabled = true;
			State = newStatus;
			ConfirmMenu.GetComponent<ConfirmMenu>().Show( _cachedCallback );
		}

		/*
		===============
		OnEnable
		===============
		*/
		private void OnEnable() {
			if ( PauseActionReference != null ) {
				PauseActionReference.action.performed += OnPauseInput;
				PauseActionReference.action.Enable();
			}
		}

		/*
		===============
		OnDisable
		===============
		*/
		private void OnDisable() {
			if ( PauseActionReference != null ) {
				PauseActionReference.action.performed -= OnPauseInput;
				PauseActionReference.action.Disable();
			}
		}

		/*
		===============
		OnPauseInput
		===============
		*/
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
			if ( GameStateManager.Instance.GameState == GameState.Paused ) {
				GameStateManager.Instance.UnPauseGame();
			} else {
				GameStateManager.Instance.PauseGame();
			}
		}

		/*
		===============
		Start
		===============
		*/
		/// <summary>
		/// Ensures <see cref="_cachedCallback"/> is properly allocated
		/// </summary>
		private void Start() {
			_cachedCallback ??= ( status ) => OnConfirmMenuButtonSelected( status );

			GameStateManager.Instance.SubscribeToGameStateEvent( GameState.Paused, StateEventType.Entered, ( state ) => { Pause(); } );
			GameStateManager.Instance.SubscribeToGameStateEvent( GameState.Level, StateEventType.Entered, ( state ) => { UnPause(); } );
		}

		/*
		===============
		OnConfirmMenuButtonSelected
		===============
		*/
		/// <summary>
		/// Handles the ConfirmMenu button callbacks
		/// </summary>
		/// <param name="option">True if YesButton is pressed</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="State"/> is out of range</exception>
		private void OnConfirmMenuButtonSelected( bool option ) {
			// cancel the exit
			if ( !option ) {
				Debug.Log( "Showing PauseMenu." );
				State = Status.Active;
				PapaPauseMenu.enabled = true;
				return;
			}

			switch ( State ) {
				case Status.QuitToTitle:
					Debug.Log( "Loading titlescreen..." );
					State = Status.Hidden;

					GameStateManager.Instance.ActivateTitleScreen();
					break;
				case Status.QuitGame:
					Debug.Log( "Exiting application..." );
					Application.Quit( 1 );
					break;
				default:
					throw new ArgumentOutOfRangeException( nameof( State ) );
			}
		}
	};
};