using System;

namespace CoLib
{

/// <summary>
/// The Ref class is used for binding properties/fields for functional tweening.
/// </summary>
/// <example>
/// <code>
/// 	int x = 0;
/// 	Ref<int> xRef = new Ref<int>( () => x, val => {x = val } );
/// 	xRef.Value += 43;
/// 	Debug.Log(x); // Prints 43
/// </code>
/// </example>
public struct Ref<T> where T : struct
{
	#region Public properties

	public T Value
	{
		get 
		{ 
			return _getter == null ? default(T) : _getter();
		}
		set 
		{ 
			if (_setter != null) { _setter(value); } 
		}
	}

	#endregion

	#region Public static methods

	public static Ref<T> Create(T value = default(T))
	{
		return new Ref<T>( 
			() => value,
			t => value = t
		);
	}

	#endregion
	
	#region Public methods

	/// <summary>
	/// Creates a Reference. A reference class encapsulates a getter and
	/// setter for modifying an external variable.
	/// </summary>
	public Ref(Func<T> getter, Action<T> setter)
	{
		if (getter == null) {
			throw new ArgumentNullException("getter");
		}
		if (setter == null) {
			throw new ArgumentNullException("setter");
		}
		_getter = getter;
		_setter = setter;
	}

	#endregion
	
	#region Private fields

	private readonly Func<T> _getter;
	private readonly Action<T> _setter;

	#endregion
}
	
}

