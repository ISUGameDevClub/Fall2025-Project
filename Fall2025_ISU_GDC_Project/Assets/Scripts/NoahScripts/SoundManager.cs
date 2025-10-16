using UnityEngine;
using System.Collections.Concurrent;
using System;
using UnityEngine.Audio;
using System.Collections;
using System.Runtime.CompilerServices;

#nullable enable

/*
===================================================================================

SoundSourceData

===================================================================================
*/

/*
===================================================================================

SoundManager

===================================================================================
*/
/// <summary>
/// <para>The sound manager for the game, call into to play a sound.</para>
/// <para>NOTICE: DO NOT ATTACH THIS TO A SCENE! JUST CALL INTO IT</para>
/// </summary>
/// <remarks>
/// This has a lot of potential for extension and modification. The current idea
/// is just a rapid fire performance and simplicity focused prototype. Maybe add object pooling for the audio sources?
/// </remarks>
/// IDEAS FOR EXTENDING
/// - Using a configurable option for number of audio channels (AudioSource essentially) that can be processed at the same time.
/// - Passing a stack-bound readonly struct instead of multiple parameters for better maintainability
/// 
public sealed class SoundManager : MonoBehaviour {
	private static SoundManager Instance {
		get {
			if ( _instance == null ) {
				GameObject gameObject = new GameObject( nameof( SoundManager ) );
				DontDestroyOnLoad( gameObject );
				_instance = gameObject.AddComponent<SoundManager>();
			}
			return _instance;
		}
	}
	private static SoundManager? _instance;

	/// <summary>
	/// A cache for storing already used AudioClip objects, to reduce load overhead. Concurrent for now for scalability reasons,
	/// it's not hurting anyone, and I probably will make this thing multithreaded at some point.
	/// </summary>
	private ConcurrentDictionary<string, AudioResource> AudioFileCache = new ConcurrentDictionary<string, AudioResource>();

	/// <summary>
	/// The object pool for the audio sources, reduces possible allocation overhead when playing a ton of audio sources
	/// </summary>
	private ComponentPool<AudioSource> SourcePool;

	/*
	===============
	SoundManager
	===============
	*/
	public SoundManager() {
		// I know this ain't best practice (for Unity MonoBehaviour classes that is), but... IDGAF
		SourcePool = new ComponentPool<AudioSource>( this );
	}

	/*
	===============
	PlaySound
	===============
	*/
	/// <summary>
	/// Plays the audio file at <paramref name="soundPath"/>.
	/// </summary>
	/// <param name="soundPath">The path to the sound file.</param>
	/// <param name="volume">The volume of the sound effect.</param>
	/// /// <param name="loop">Whether to loop the <see cref="AudioSource"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown if the <see cref="SoundManager"/> instance hasn't been created yet.</exception>
	/// <exception cref="ArgumentException">Thrown if <paramref name="soundPath"/> is null or empty.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="volume"/> is less than or equal to 0.</exception>
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static void PlaySound( string? soundPath, float? volume = 0.0f, bool? loop = false ) {
		if ( Instance == null ) {
			throw new ArgumentNullException( "Do not call into the SoundManager before instantiation" );
		}
		if ( soundPath == null || soundPath.Length <= 0 ) {
			throw new ArgumentException( "soundPath is null or empty" );
		}
		PlaySound( LoadAudioFile( soundPath ), volume, loop );
	}

	/*
	===============
	PlaySound
	===============
	*/
	/// <summary>
	/// Plays the audio data provided in <paramref name="soundStream"/>.
	/// </summary>
	/// <param name="soundStream">The resource to the stream.</param>
	/// <param name="volume">The volume of the sound effect.</param>
	/// <param name="loop">Whether to loop the <see cref="AudioSource"/>.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="soundStream"/> or <see cref="Instance"/> are null.</exception>
	public static void PlaySound( AudioResource? soundStream, float? volume = 0.0f, bool? loop = false ) {
		if ( Instance == null ) {
			throw new ArgumentNullException( "Do not call into the SoundManager before instantiation" );
		}
		if ( soundStream == null ) {
			throw new ArgumentNullException( nameof( soundStream ) );
		}
		if ( volume.HasValue && volume.Value <= 0.0f ) {
			Debug.LogWarningFormat( $"SoundManager.PlaySound: volume is being used, but its less than 0. Cancelling audio query." );
			return;
		}

		AudioSource source = Instance.SourcePool.Rent();
		source.resource = soundStream;
		if ( volume.HasValue ) {
			source.volume = volume.Value;
		}
		if ( loop.HasValue ) {
			source.loop = loop.Value;
		}
		source.Play();

		Instance.StartCoroutine( WaitForAudioSourceFinished( source ) );
	}

	/*
	===============
	LoadAudioFile
	===============
	*/
	/// <summary>
	/// Loads an <see cref="AudioResource"/> from the provided audio file path.
	/// </summary>
	/// <param name="soundPath">The path to the audio file.</param>
	/// <returns>The loaded audio stream.</returns>
	private static AudioResource LoadAudioFile( string? soundPath ) {
		if ( soundPath == null || soundPath.Length <= 0 ) {
			throw new ArgumentException( "soundPath is null or empty" );
		}
		if ( !Instance.AudioFileCache.TryGetValue( soundPath, out AudioResource stream ) ) {
			stream = Resources.Load<AudioResource>( soundPath );
			if ( !Instance.AudioFileCache.TryAdd( soundPath, stream ) ) {
				throw new Exception( "AudioFileCache.TryGetValue failed... then TryAdd failed... race condition?" );
			}

			Debug.LogFormat( $"SoundManager.LoadAudioFile: loading audio file '{soundPath}'..." );
		}
		return stream;
	}

	/*
	===============
	WaitForAudioSourceFinished
	===============
	*/
	/// <summary>
	/// 
	/// </summary>
	/// <param name="source"></param>
	/// <returns></returns>
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	private static IEnumerator WaitForAudioSourceFinished( AudioSource source ) {
		yield return new WaitUntil( () => !source.isPlaying );
		Debug.LogFormat( $"SoundManager.WaitForAudioSourceFinished: returning AudioSource to ComponentPool '{source.name}'..." );
	}
};