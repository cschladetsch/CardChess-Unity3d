namespace CoLib
{

public interface IInterpolator<T>
{
	/// <summary>
	/// Get an interpolated value between startValue and endValue.
	/// When t = 0, startValue should be returned, and when t = 1, 
	/// endValue should be returned. T is typically between 0 - 1,
	/// but the implementor should be able to accept values outside 
	/// that range.
	/// </summary>
	T Interpolate(T startValue, T endValue, double t);
}

}
