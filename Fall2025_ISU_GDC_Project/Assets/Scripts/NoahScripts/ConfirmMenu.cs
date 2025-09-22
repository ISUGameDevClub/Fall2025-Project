using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace UserInterface {
	/*
	===================================================================================
	
	ConfirmMenu
		
	===================================================================================
	*/
	/// <summary>
	/// Handler class for the ConfirmMenu Canvas object
	/// </summary>

	public class ConfirmMenu : MonoBehaviour {
		[SerializeField]
		private CanvasGroup ConfirmMenuGroup;

		[SerializeField]
		/// <summary>
		/// The Canvas object of the ConfirmMenu
		/// </summary>
		private GameObject ConfirmMenuObject;
		
		/// <summary>
		/// _callback handler for both buttons
		/// </summary>
		private Action<bool> _callback;

		/*
		===============
		Show
		===============
		*/
		/// <summary>
		/// Called when we want to show the confirmation prompt
		/// </summary>
		/// <param name="callback">The callback handler for the buttons</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is null</exception>
		public void Show( Action<bool> callback ) {
			Debug.Log( "Showing ConfirmMenu..." );

			ConfirmMenuGroup.alpha = 1.0f;
			ConfirmMenuObject.SetActive( true );
			_callback = callback ?? throw new ArgumentNullException( nameof( callback ) );
		}

		/*
		===============
		OnYesButtonClicked
		===============
		*/
		/// <summary>
		/// Callback function for YesButton OnClick event
		/// </summary>
		public void OnYesButtonClicked() {
			OnButtonClicked( true );
		}

		/*
		===============
		OnNoButtonClicked
		===============
		*/
		/// <summary>
		/// Callback function for NoButton OnClick event
		/// </summary>
		public void OnNoButtonClicked() {
			OnButtonClicked( false );
		}

		/*
		===============
		OnButtonClicked
		===============
		*/
		/// <summary>
		/// Called upon a button's OnClick event firing
		/// </summary>
		/// <param name="status">True if yes was pressed, false if no was pressed</param>
		/// <exception cref="ArgumentNullException">Thrown if <see cref="ConfirmMenuObject"/> or <see cref="_callback"/> are null</exception>
		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		private void OnButtonClicked( bool status ) {
			if ( ConfirmMenuObject == null ) {
				throw new ArgumentNullException( nameof( ConfirmMenuObject ) );
			}
			if ( _callback == null ) {
				throw new ArgumentNullException( nameof( _callback ) );
			}
			// hide it
			ConfirmMenuGroup.alpha = 0.0f;

			ConfirmMenuObject.SetActive( false );
			_callback.Invoke( status );
		}
	};
};
