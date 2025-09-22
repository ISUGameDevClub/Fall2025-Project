using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

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

		public CanvasGroup PauseMenuGroup;

		public Canvas PapaPauseMenu;
		public Canvas ConfirmMenu;

		public event Action ResumePressed;
		public event Action QuitToTitlePressed;

		private Status State = Status.Hidden;

		private static Action<bool> _cachedCallback = null;

		/*
		===============
		Awake
		===============
		*/
		/// <summary>
		/// Ensures <see cref="_cachedCallback"/> is properly allocated
		/// </summary>
		private void Awake() {
			_cachedCallback ??= ( status ) => OnConfirmMenuButtonSelected( status );
		}

		/*
		===============
		Pause
		===============
		*/
		public void Pause() {
			PauseMenuGroup.alpha = 1.0f;

			ConfirmMenu.enabled = false;
			PapaPauseMenu.enabled = true;

			State = Status.Active;
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
			UnPause();
			ResumePressed?.Invoke();
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
			State = Status.QuitGame;
			ConfirmMenu.GetComponent<ConfirmMenu>().Show( _cachedCallback );
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
					QuitToTitlePressed?.Invoke();
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