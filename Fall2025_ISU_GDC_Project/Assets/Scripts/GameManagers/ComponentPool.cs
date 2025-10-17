using System;
using System.Collections.Concurrent;
using UnityEngine;

/*
===================================================================================

ComponentPool

===================================================================================
*/
/// <summary>
/// An object pool specifically made for Unity GameObject components
/// </summary>
/// <typeparam name="T"></typeparam>

public class ComponentPool<T> where T : Component {
	private readonly ConcurrentBag<T> Pool = new ConcurrentBag<T>();
	private readonly MonoBehaviour Owner;

	/*
	===============
	ComponentPool
	===============
	*/
	public ComponentPool( MonoBehaviour owner ) {
		if ( owner == null ) {
			throw new ArgumentNullException( nameof( owner ) );
		}
		Owner = owner;
	}

	/*
	===============
	ComponentPool
	===============
	*/
	private ComponentPool() {
		throw new InvalidOperationException();
	}

	/*
	===============
	Rent
	===============
	*/
	/// <summary>
	/// Retrieves an object of type <typeparamref name="T"/> from the pool, allocating a new object if needed.
	/// </summary>
	/// <returns>The object retrieved from the pool, or freshly created from the <see cref="Owner"/></returns>
	public T Rent() {
		if ( Pool.TryTake( out T result ) ) {
			return result;
		}
		result = Owner.gameObject.AddComponent<T>();
		return result;
	}

	/*
	===============
	Return
	===============
	*/
	/// <summary>
	/// Returns the <paramref name="value"/> back to the pool.
	/// </summary>
	/// <param name="value">The value to return to the pool.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
	public void Return( T value ) {
		if ( value == null ) {
			throw new ArgumentNullException( nameof( value ) );
		}
		Pool.Add( value );
	}
};