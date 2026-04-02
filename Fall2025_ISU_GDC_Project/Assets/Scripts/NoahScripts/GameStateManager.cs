using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable

/// <summary>
/// Enumeration marker for the current status of the game
/// </summary>
/// <remarks>
/// Add to this when needed
/// </remarks>
public enum GameState : uint {
	/// <summary>
	/// We're in the title screen
	/// </summary>
	TitleScreen,

	/// <summary>
	/// We're in a level
	/// </summary>
	Level,

	/// <summary>
	/// The game's <see cref="PauseMenu"/> is being shown, and we've completely halted the main game loop
	/// </summary>
	Paused,

	Count
};

/// <summary>
/// Event types correlated with state changes, add to this if needed
/// </summary>
public enum StateEventType : uint {
	/// <summary>
	/// The event that triggers when entering a specific state.
	/// </summary>
	Entered,

	/// <summary>
	/// The event that triggers when the state's value has changed.
	/// </summary>
	Modified,

	/// <summary>
	/// The event that triggers when exiting a specific state.
	/// </summary>
	Exited,

	Count
};

/*
===================================================================================

GameStateManager

===================================================================================
*/
/// <summary>
/// <para>Handles state across the game's ui and level scenes</para>
/// <para>This class is basically an automated event bus (something that does all the heavy lifting with event subscription) for different game states.</para>
/// <para>This class handles both global <see cref="GameState"/> management, and also allows custom dynamically defined states that are cleared upon new scene instantiation.</para>
/// <para>To register an event catcher for global game events (part of the <see cref="GameState"/> enum), use the <see cref="GameStateManager.SubscribeToGameStateEvent(GameState, StateEventType, Action{GameState}?)"/>
/// method.</para>
/// </summary>

//
// NOTE: keep this file under ideally 800 lines, and avoid breaches of SRP
//
// IDEAS FOR EXTENDING THIS:
// - use ObjectPool for scene-based state variables for more efficient memory management
// - allow the creator of a dynamic state variable to have a custom state validator callback when changing the state's internals
// - allow the creator of a dynamic state variable to modify an internal Dictionary hosting variable for an object's state
// - thread safety... maybe
// - let the event callback do the state validation itself (so that we don't have manual state validation here)
//

public class GameStateManager : MonoBehaviour {
	private class StateEventSubscriptionSet<StateType> {
		/// <summary>
		/// The state we are subscribing to, either a <see cref="GameState"/>, or dynamically named state
		/// </summary>
		public readonly StateType? State;

		/// <summary>
		/// A lookup table for specific event transitions
		/// </summary>
		public readonly ConcurrentDictionary<StateEventType, List<Action<StateType>>?>? Subscriptions;

		/*
		===============
		StateEventSubscriptionSet
		===============
		*/
		public StateEventSubscriptionSet( StateType state ) {
			State = state;
			Subscriptions = new ConcurrentDictionary<StateEventType, List<Action<StateType>>?>();
		}

		/*
		===============
		AddSubscription
		===============
		*/
		/// <summary>
		/// Adds a subscription to the provided event of <paramref name="type"/>
		/// </summary>
		/// <param name="type">The <see cref="StateEventType"/> to hook the <paramref name="callback"/> to</param>
		/// <param name="callback">The method that will be called when the <paramref name="type"/> is triggered</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="type"/> isn't a valid <see cref="StateEventType"/></exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/></exception>
		public void AddSubscription( StateEventType type, Action<StateType>? callback ) {
			// sanity checks
			if ( Subscriptions == null ) {
				throw new ArgumentNullException( nameof( Subscriptions ) );
			}
			if ( State == null ) {
				throw new ArgumentNullException( nameof( State ) );
			}
			if ( type < StateEventType.Entered || type >= StateEventType.Count ) {
				throw new ArgumentOutOfRangeException( nameof( type ) );
			}
			if ( callback == null ) {
				throw new ArgumentNullException( nameof( callback ) );
			}

			if ( !Subscriptions.TryGetValue( type, out List<Action<StateType>>? callbackList ) ) {
				callbackList = new List<Action<StateType>>();

				try {
					if ( !Subscriptions.TryAdd( type, callbackList ) ) {
						throw new Exception( $"Error adding subscription set for game state '{State}' and event type '{type}', TryAdd failed, race condition?" );
					}
				} catch ( OverflowException ) {
					throw new Exception( "StateEventSubscriptionSets.Subscriptions overflowed (OverflowException)... somehow?" );
				} catch ( Exception e ) {
					Debug.LogErrorFormat( $"Exception thrown while adding subscriptionSet to GameStateManager: {e.Message}" );
				}
			}
			if ( callbackList == null ) {
				throw new ArgumentNullException( nameof( callbackList ) );
			}
			callbackList.Add( callback );

			Debug.LogFormat( $"...Successfully registered event subscription for state '{State}' and event '{type}'" );
		}

		/*
		===============
		PumpStateEvent
		===============
		*/
		/// <summary>
		/// Takes in <paramref name="type"/> and calls all the callback methods in <see cref="Subscriptions"/>
		/// </summary>
		/// <param name="type">The type of the event that is being triggered</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="type"/> is an invalid <see cref="StateEventType"/>.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <see cref="Subscriptions"/> is null (hasn't been instantiated yet).</exception>
		public void PumpStateEvent( StateEventType type ) {
			// sanity checks
			if ( type < StateEventType.Entered || type >= StateEventType.Count ) {
				throw new ArgumentOutOfRangeException( nameof( type ) );
			}
			if ( Subscriptions == null ) {
				throw new ArgumentNullException( nameof( Subscriptions ) );
			}

			// make sure we've got a valid StateEventType, and log it
			// NOTE: might not be necessary for dynamic states, but its verbose, so I'm keeping it
			LogEventType( type );

			// no subscriptions, just keep 'em moving
			if ( !Subscriptions.TryGetValue( type, out List<Action<StateType>>? callbackList ) ) {
				return;
			}
			if ( callbackList == null ) {
				throw new ArgumentNullException( nameof( callbackList ) );
			}

			// pump it!
			for ( int i = 0; i < callbackList.Count; i++ ) {
				if ( State == null ) {
					// just make absolutely sure
					throw new ArgumentNullException( nameof( State ) );
				}

				// call the callback method subscribed to the event
				callbackList[ i ]?.Invoke( State );
			}
		}

		/*
		===============
		LogEventType
		===============
		*/
		/// <summary>
		/// Prints the given state event (<paramref name="type"/>) to the console and throws an error if it isn't a valid event type.
		/// </summary>
		/// <param name="type">The <see cref="StateEventType"/> to print.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="type"/> isn't a valid <see cref="StateEventType"/>.</exception>
		private void LogEventType( StateEventType type ) {
			Debug.LogFormat( type switch {
				StateEventType.Entered => $"GameStateManager.SetState: entering state '{State}'...",
				StateEventType.Exited => $"GameStateManager.SetState: exiting state '{State}'...",
				StateEventType.Modified => $"GameStateManager.SetState: modifying state '{State}'...",
				_ => throw new ArgumentOutOfRangeException( $"Invalid StateEventType '{type}'." )
			} );
		}
	};

	/// <summary>
	/// The singleton handler for the global <see cref="GameStateManager"/>, access is filtered directly through this
	/// </summary>
	public static GameStateManager Instance {
		get {
			// create the instance if we haven't already
			if ( _instance == null ) {
				GameObject gameObject = new GameObject( nameof( GameStateManager ) );
				DontDestroyOnLoad( gameObject );
				_instance = gameObject.AddComponent<GameStateManager>();
			}
			return _instance;
		}
	}

	/// <summary>
	/// The game's current state, can only be changed through <see cref="SetState"/>
	/// </summary>
	/// <remarks>
	/// The <see cref="GameStateChanged"/> event is fire whenever this variable is changed
	/// </remarks>
	public GameState GameState { get; private set; } = GameState.TitleScreen;

	/// <summary>
	/// The event that fires whenever <see cref="GameState"/> changes
	/// </summary>
	/// <remarks>
	/// A much simpler way of hooking to the GameStateManager, but less controllable and is triggered every time a
	/// state change happens, so less performant as well.
	/// </remarks>
	public static event Action<GameState>? GameStateChanged = null;

	/// <summary>
	/// Callback table for state-change events subscriptions. Specifically for global game state
	/// </summary>
	private readonly StateEventSubscriptionSet<GameState>?[]? _gameStateEventSubscriptionSets = new StateEventSubscriptionSet<GameState>?[ (int)GameState.Count ];

	/// <summary>
	/// The internal collection of dynamically defined state event subscriptions. This list will be completely emptied each time
	/// the scene is changed, so be mindful of that and don't make dynamic state events that have a non scene based lifespan.
	/// </summary>
	/// <remarks>
	/// Could be an ObjectPool for more memory efficiency, but the current implementation is fine for now
	/// </remarks>
	private readonly Dictionary<string, StateEventSubscriptionSet<string>?>? _sceneStateEventSubscriptionSets = new Dictionary<string, StateEventSubscriptionSet<string>?>();

	/// <summary>
	/// The internal singleton handle
	/// </summary>
	private static GameStateManager? _instance;

	/*
	===============
	PauseGame
	===============
	*/
	/// <summary>
	/// </summary>
	/// <remarks>
	/// A lose wrapper around calling <see cref="SetGameState"/> with <see cref="GameState.Paused"/>, effectively the same as writing
	/// "Instance.SetGameSet( GameState.Paused )"
	/// </remarks>
	/// <returns>True if state was changed to <see cref="GameState.Paused"/></return>
	public bool PauseGame() {
		// make sure we actually have valid state to do the transition
		switch ( GameState ) {
			case GameState.TitleScreen:
				Debug.LogWarningFormat( "GameStateManager.PauseGame: attempted to pause game from title screen." );
				break;
			case GameState.Paused:
				Debug.LogWarningFormat( "GameStateManager.PauseGame: game is already paused." );
				break;
			case GameState.Level:
				SetGameState( GameState.Paused );
				return true;
			default: // uh-oh
				throw new ArgumentOutOfRangeException( "GameStateManager has an invalid game state!" );
		}
		return false;
	}

	/*
	===============
	UnPauseGame
	===============
	*/
	/// <summary>
	/// </summary>
	/// <remarks>
	/// A lose wrapper around calling <see cref="SetGameState"/> with <see cref="GameState.Level"/>, effectively the same as writing
	/// "SetGameSet( GameState.Level )" when the pause menu is active
	/// </remarks>
	/// <returns>True if state was changed to <see cref="GameState.Level"/></return>
	public bool UnPauseGame() {
		// make sure we actually have valid state to do the transition
		switch ( GameState ) {
			case GameState.TitleScreen:
				Debug.LogWarningFormat( "GameStateManager.UnPauseGame: attempted to unpause game from title screen." );
				break;
			case GameState.Level:
				Debug.LogWarningFormat( "GameStateManager.UnPauseGame: game isn't paused, but function is called." );
				break;
			case GameState.Paused:
				SetGameState( GameState.Level );
				return true;
			default: // uh-oh
				throw new ArgumentOutOfRangeException( "GameStateManager has an invalid game state!" );
		}
		return false;
	}

	/*
	===============
	ActivateTitleScreen
	===============
	*/
	/// <summary>
	/// </summary>
	/// <remarks>
	/// A lose wrapper around calling <see cref="SetGameState"/> with <see cref="GameState.Titlescreen"/>, effectively the same as writing
	/// "SetGameSet( GameState.Titlescreen )" when the pause menu is active
	/// </remarks>
	/// <returns>True if state was changed to <see cref="GameState.TitleScreen"/></return>
	public bool ActivateTitleScreen() {
		// make sure we actually have valid state to do the transition
		switch ( GameState ) {
			case GameState.Level:
				Debug.LogWarningFormat( "GameStateManager.ActivateTitleScreen: attempted to activate title screen from a level without using the pause menu." );
				break;
			case GameState.TitleScreen:
				Debug.LogWarningFormat( "GameStateManager.ActivateTitleScreen: title screen state reactivated." );
				break;
			case GameState.Paused:
				SetGameState( GameState.TitleScreen );
				return true;
			default: // uh-oh
				throw new ArgumentOutOfRangeException( "GameStateManager has an invalid game state!" );
		}
		return false;
	}

	/*
	===============
	ActivateTitleScreen
	===============
	*/
	/// <summary>
	/// </summary>
	/// <remarks>
	/// A lose wrapper around calling <see cref="SetGameState"/> with <see cref="GameState.Level"/>, effectively the same as writing
	/// "SetGameSet( GameState.Level )" when the pause menu is active
	/// </remarks>
	/// <returns>True if state was changed to <see cref="GameState.Level"/></return>
	public bool ActivateLevel() {
		// make sure we actually have valid state to do the transition
		switch ( GameState ) {
			case GameState.Paused:
				Debug.LogWarningFormat( "GameStateManager.ActivateLevel: attempted to activate level state from pause menu, use GameStateManager.UnPauseGame instead." );
				break;
			case GameState.Level:
				Debug.LogWarningFormat( "GameStateManager.ActivateLevel: level state reactivated." );
				break;
			case GameState.TitleScreen:
				SetGameState( GameState.Level );
				return true;
			default: // uh-oh
				throw new ArgumentOutOfRangeException( "GameStateManager has an invalid game state!" );
		}
		return false;
	}

	/*
	===============
	SubscribeToGameStateEvent
	===============
	*/
	/// <summary>
	/// <para>Adds the lamda/method reference <paramref name="callback"/> to the GameStateManager's system.</para>
	/// <para>The parameter <paramref name="state"/> is the name of the scene-based state variable</para>
	/// </summary>
	/// <param name="state">The name of the state variable, a "State Key" (this is talked about in other parts of this API).</param>
	/// <param name="type">The type of event that you want the <paramref name="callback"/> to be called when the event occurs.</param>
	/// <param name="callback">The lamda/method that will be called when the event <paramref name="type"/> occurs.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="type"/> isn't a valid <see cref="StateEventType"/> enum,
	/// or if <paramref name="state"/> isn't a valid <see cref="global::GameState"/> enum.</exception>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is null.</exception>
	public void SubscribeToGameStateEvent( GameState state, StateEventType type, Action<GameState>? callback ) {
		/// sanity checks
		if ( state < GameState.TitleScreen || state >= GameState.Count ) {
			throw new ArgumentOutOfRangeException( $"Provided state '{Enum.GetName( typeof( GameState ), state )}' is not a valid GameState" );
		} else if ( type < StateEventType.Entered || type >= StateEventType.Count ) {
			throw new ArgumentOutOfRangeException( $"Provided event type '{type}' is not a valid StateEventTyoe" );
		} else if ( callback == null ) {
			throw new ArgumentNullException( nameof( callback ) );
		}

		// make sure we're instantiating the state subscriber before using it
		if ( _gameStateEventSubscriptionSets[ (int)state ] == null ) {
			_gameStateEventSubscriptionSets[ (int)state ] = new StateEventSubscriptionSet<GameState>( state );
		}

		// add the subscription
		AddStateEventSubscriptionSet( in _gameStateEventSubscriptionSets[ (int)state ], type, callback );
	}

	/*
	===============
	SubscribeToSceneStateEvent
	===============
	*/
	/// <summary>
	/// <para>Adds the lamda/method reference <paramref name="callback"/> to the GameStateManager's system.</para>
	/// <para>The parameter <paramref name="state"/> is the name of the scene-based state variable</para>
	/// </summary>
	/// <param name="state">The name of the state variable, a "State Key" (this is talked about in other parts of this API).</param>
	/// <param name="type">The type of event that you want the <paramref name="callback"/> to be called when the event occurs.</param>
	/// <param name="callback">The lamda/method that will be called when the event <paramref name="type"/> occurs.</param>
	/// <exception cref="ArgumentException">Thrown if <paramref name="state"/>'s <see cref="string"/> is empty (length of 0) or if its null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="type"/> isn't a valid <see cref="StateEventType"/> enum.</exception>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> or <see cref="_sceneStateEventSubscriptionSets"/> is null.</exception>
	/// <exception cref="Exception">Thrown if <see cref="Dictionary.TryAdd"/> fails, meaning we've likely either got a race condition
	/// (as we check with <see cref="Dictionary.TryGetValue"/> before adding) or memory corruption (which ideally shouldn't happen in C#).</exception>
	public void SubscribeToSceneStateEvent( string? state, StateEventType type, Action<string>? callback ) {
		/// sanity checks
		if ( state == null || state.Length <= 0 ) {
			throw new ArgumentException( "name is null or empty" );
		} else if ( type < StateEventType.Entered || type >= StateEventType.Count ) {
			throw new ArgumentOutOfRangeException( $"Provided event type '{type}' is not a valid StateEventTyoe" );
		} else if ( callback == null ) {
			throw new ArgumentNullException( nameof( callback ) );
		} else if ( _sceneStateEventSubscriptionSets == null ) {
			throw new ArgumentNullException( nameof( _sceneStateEventSubscriptionSets ) );
		}

		// fetch the subscription object/set, create a new set if one doesn't exist yet
		if ( !_sceneStateEventSubscriptionSets.TryGetValue( state, out StateEventSubscriptionSet<string>? subscription ) ) {
			subscription = new StateEventSubscriptionSet<string>( state );
			try {
				// if this happens, something big has definitely broken
				if ( !_sceneStateEventSubscriptionSets.TryAdd( state, subscription ) ) {
					throw new Exception( $"Error adding subscription set to game state '{state}' and event type '{type}', TryAdd failed, race condition?" );
				}
			} catch ( OverflowException ) {
				throw new Exception( "GameManager._sceneStateEventSubscriptionSets overflowed (OverflowException)... somehow?" );
			} catch ( Exception e ) {
				Debug.LogErrorFormat( $"Exception thrown while adding subscriptionSet to GameStateManager: {e.Message}" );
			}
		}

		// add the subscription
		AddStateEventSubscriptionSet( in subscription, type, callback );
	}

	/*
	===============
	PublishEvent
	===============
	*/
	/// <summary>
	/// <para>Notifies all subscribers of the provided <paramref name="state"/> that <see cref="StateEventType"/> <paramref name="type"/> has occured.</para>
	/// <para>This can publish events for both <see cref="global::GameState"/> and dynamically created state variables.</para>
	/// </summary>
	/// <example>
	/// An example of how to call this method:
	/// <code>
	/// GameStateManager.Instance.PublishEvent( GameState.Titlescreen, StateEventType.Entered );
	/// </code>
	/// </example>
	/// <typeparam name="StateType">The type of the state data, either a <see cref="string"/> or a <see cref="global::GameState"/> enum.</typeparam>
	/// <param name="state">The state variable that is being modified.</param>
	/// <param name="type">The <see cref="StateEventType"/> that has just occured.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="type"/> isn't a valid <see cref="StateEventType"/> enum.</exception>
	/// <exception cref="ArgumentNullException"></exception>
	public void PublishEvent<StateType>( StateType state, StateEventType type ) {
		// sanity checks
		if ( type < StateEventType.Entered || type >= StateEventType.Count ) {
			throw new ArgumentOutOfRangeException( nameof( type ) );
		}

		// check that we aren't getting garbage. If it's null, then just say no and reject the call
		// the lock here is handled by IsValidStateKey
		if ( !IsValidStateKey( state ) ) {
			Debug.LogWarningFormat( $"GameStateManager.PublishEvent: invalid state data given." );
			return;
		}

		if ( state is string strState ) {
			if ( _sceneStateEventSubscriptionSets == null ) {
				throw new ArgumentNullException( nameof( _sceneStateEventSubscriptionSets ) );
			}
			if ( !_sceneStateEventSubscriptionSets.TryGetValue( strState, out StateEventSubscriptionSet<string>? subscriptionSet ) ) {
				// should this just be a debug message? Realistically, this WOULD be a seggy
				throw new KeyNotFoundException( $"Attempted to publish non-existent event for state '{state}', event was '{type}'" );
			}

			// check that nothing has broken
			if ( subscriptionSet == null ) {
				throw new ArgumentNullException( $"StateEventSubscriptionSet for state '{strState}' has a null value." );
			}

			// trigger the event
			subscriptionSet.PumpStateEvent( type );
		} else if ( state is GameState gameState ) {
			if ( gameState < GameState.TitleScreen || gameState >= GameState.Count ) {
				throw new ArgumentOutOfRangeException( nameof( gameState ) );
			}
			// check that nothing has broken
			if ( _gameStateEventSubscriptionSets[ (int)gameState ] != null ) {
				// all good to go, pump it!
				_gameStateEventSubscriptionSets[ (int)gameState ].PumpStateEvent( type );
			} else {
				throw new ArgumentNullException( $"StateEventSubscriptionSet for global game state '{Enum.GetName( typeof( GameState ), gameState )}' has a null value." );
			}
		}
	}

	/*
	===============
	IsValidStateKey
	===============
	*/
	/// <summary>
	/// Checks if <paramref name="state"/> is a valid State Key
	/// </summary>
	/// <typeparam name="StateType">The datatype of the State Key</typeparam>
	/// <param name="state">The name of the state data to check</param>
	/// <returns>
	/// <para>If <paramref name="state"/> is a <see cref="string"/>, then True if it is a valid string (not null or empty), and if the State Key
	/// has already been created with <see cref="SubscribeToSceneStateEvent"/>.</para>
	/// <para>If <paramref name="state"/> is a <see cref="global::GameState"/> enum, then True if it is a valid <see cref="global::GameState"/> enum
	/// and if the provided <paramref name="state"/> state has already been created with <see cref="SubscribeToGameStateEvent"/>.</para>
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// <para>Thrown if <paramref name="state"/> is a <see cref="string"/> and if the <see cref="_sceneStateEventSubscriptionSets"/> hasn't been initialized yet.</para>
	/// <para>Thrown if <paramref name="state"/> is a <see cref="global::GameState"/> hasn't been initialized yet.</para>
	/// </exception>
	public bool IsValidStateKey<StateType>( StateType state ) {
		if ( state == null ) {
			return false;
		}
		if ( state is string strState ) {
			if ( _sceneStateEventSubscriptionSets == null ) {
				throw new ArgumentNullException( nameof( _sceneStateEventSubscriptionSets ) );
			}

			// its a dynamic state, so just check that its there
			return _sceneStateEventSubscriptionSets.ContainsKey( strState );
		} else if ( state is GameState gameState ) {
			if ( _gameStateEventSubscriptionSets[ (int)gameState ] == null ) {
				return false;
			}

			// if it's been allocated properly, then we have a valid state key
			return true;
		}
		throw new InvalidCastException( nameof( state ) );
	}

	/*
	===============
	SetGameState
	===============
	*/
	/// <summary>
	/// Sets the <see cref="GameState"/>, and fires the <see cref="GameStateChanged"/> event.
	/// </summary>
	/// <remarks>
	/// The <see cref="GameStateChanged"/> event is only triggered if no errors occurred.
	/// </remarks>
	/// <param name="state">The new <see cref="GameState"/>, should ideally be different from the current gamestate.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="state"/> isn't a valid <see cref="GameState"/>.</exception> 
	private void SetGameState( GameState state ) {
		if ( state < GameState.TitleScreen || state >= GameState.Count ) {
			throw new ArgumentOutOfRangeException( $"Provided state '{Enum.GetName( typeof( GameState ), state )}' is not a valid GameState" );
		} else if ( GameState == state ) {
			Debug.LogWarningFormat( $"GameStateManager.SetGameState: same game state." );
		}

		Debug.LogFormat( $"GameStateManager.SetState: changing state to '{Enum.GetName( typeof( GameState ), state )}'..." );

		// notify the system
		TriggerGameStateChange( state );
		GameStateChanged?.Invoke( GameState );
	}


	/*
	===============
	ClearDynamicStateVars
	===============
	*/
	/// <summary>
	/// Clears the <see cref="_sceneStateEventSubscriptionSets"/> Dictionary as the scene is changed.
	/// </summary>
	/// <param name="currentScene">The current scene.</param>
	/// <param name="nextScene">The scene being transitioned to.</param>
	/// <exception cref="ArgumentNullException">Thrown if <see cref="_sceneStateEventSubscriptionSets"/> is null.</exception>
	private void ClearDynamicStateVars( Scene currentScene, Scene nextScene ) {
		if ( _sceneStateEventSubscriptionSets == null ) {
			throw new ArgumentNullException( nameof( _sceneStateEventSubscriptionSets ) );
		}
		_sceneStateEventSubscriptionSets.Clear();
	}

	/*
	===============
	TriggerGameStateChange
	===============
	*/
	/// <summary>
	/// Publishes and notifies all <see cref="global::GameState"/> subscribers of a game state change.
	/// </summary>
	/// <remarks>
	/// Thread safety is handled from <see cref="SetGameState"/>, no need to lock the lock again.
	/// </remarks>
	/// <param name="newState">The new state that we're changing to.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="newState"/> isn't a valid <see cref="global::GameState"/>.</exception>
	private void TriggerGameStateChange( GameState newState ) {
		if ( newState < GameState.TitleScreen || newState >= GameState.Count ) {
			throw new ArgumentOutOfRangeException( nameof( newState ) );
		}

		// establish the new state BEFORE we notify the rest of the system to avoid state corruption
		GameState oldState = GameState;
		GameState = newState;

		// notify the system now that we've established the new state
		PublishEvent( oldState, StateEventType.Exited );
		PublishEvent( newState, StateEventType.Entered );
	}

	/*
	===============
	AddStateEventSubscriptionSet
	===============
	*/
	/// <summary>
	/// <para>Adds a new state event subscription to the event subscription cache.</para>
	/// <para>This is a utility function to reduce repeating code between the functions <see cref="SubscribeToSceneStateEvent"/> and <see cref="SubscribeToGameStateEvent"/>.</para>
	/// </summary>
	/// <typeparam name="StateType">Type internal type of the state.</typeparam>
	/// <param name="state">The handler struct for the internal subscription data.</param>
	/// <param name="type">The type of event that the subscription object <paramref name="state"/> is handling.</param>
	/// <param name="callback">The callback method invoked when the event <paramref name="type"/> has been triggered.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="type"/> isn't a valid <see cref="StateEventType"/> enum.</exception>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> or <paramref name="state"/> is null.</exception>
	private void AddStateEventSubscriptionSet<StateType>( in StateEventSubscriptionSet<StateType>? state, StateEventType type, Action<StateType>? callback ) {
		if ( type < StateEventType.Entered || type >= StateEventType.Count ) {
			throw new ArgumentOutOfRangeException( nameof( type ) );
		}
		if ( callback == null ) {
			throw new ArgumentNullException( nameof( callback ) );
		}
		if ( state == null ) {
			throw new ArgumentNullException( nameof( state ) );
		}

		//
		// I'm not writing this out in a print statement with the full enum get name thing because I don't want to have an
		// anyeursym, so its here for readability more than anything
		//

		string? stateName;
		if ( state.State is GameState gameState ) {
			stateName = state.ToString();
		} else if ( state.State is string strState ) {
			stateName = strState;
		} else {
			throw new InvalidCastException( nameof( state.State ) );
		}

		// log it, this is to ensure we know exactly what events are created and when they are created
		Debug.Log(
			$"GameStateManager.AddStateEventSubscriptionSet: state event subscription added - [{stateName}:{type}:{callback.Method.HumanName()}]"
		);

		// pin down the object to avoid memory corruption and/or race conditions
		state.AddSubscription( type, callback );
	}

	/*
	===============
	Awake
	===============
	*/
	private void Awake() {
		// hook the scene change clear
		SceneManager.activeSceneChanged += ClearDynamicStateVars;
	}
};