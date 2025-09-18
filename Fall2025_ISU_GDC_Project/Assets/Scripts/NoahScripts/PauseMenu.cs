using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		private enum Status : byte {
			Valid,
			QuitToTitle,
			QuitGame
		};

		/// <summary>
		/// TODO: change this later when needed
		/// </summary>
		private static readonly string PAUSE_MENU_SCENE_PATH = "Assets/Scenes/NoahScenes/PauseMenu";

		public Canvas PapaPauseMenu;
		public Canvas ConfirmMenu;

		private static Status _status = Status.Valid;
		private static Action<bool> _cachedCallback = null;

		/*
		===============
		Pause
		===============
		*/
		/// <summary>
		/// The only way to instantiate the PauseMenu
		/// </summary>
		public static void Pause() {
			SceneManager.LoadScene( PAUSE_MENU_SCENE_PATH, LoadSceneMode.Additive );
		}

		/*
		===============
		Start
		===============
		*/
		public void Start() {
			Time.timeScale = 0.0f;
			_status = Status.Valid;
			_cachedCallback ??= ( status ) => OnConfirmMenuButtonSelected( status );
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
			Debug.Log( "Resuming game..." );

			Time.timeScale = 1.0f;
			PapaPauseMenu.enabled = false;
			Destroy( this );
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
			_status = Status.QuitGame;
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
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="_status"/> is out of range</exception>
		private void OnConfirmMenuButtonSelected( bool option ) {
			// cancel the exit
			if ( !option ) {
				Debug.Log( "Showing PauseMenu." );
				_status = Status.Valid;
				PapaPauseMenu.enabled = true;
				return;
			}

			switch ( _status ) {
				case Status.QuitToTitle:
					// TODO: instantiate the stitlescreen here
					Debug.Log( "Loading titlescreen..." );
					break;
				case Status.QuitGame:
					Debug.Log( "Exiting application..." );
					Application.Quit( 1 );
					break;
				default:
					throw new ArgumentOutOfRangeException( nameof( _status ) );
			}
		}
	};
};