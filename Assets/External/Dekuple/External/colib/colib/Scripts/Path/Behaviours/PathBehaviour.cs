using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace CoLib
{

public enum RotationMode
{
	/// <summary>
	/// Every knot has a predefined up vector.
	/// </summary>
	PerKnotUp,
	/// <summary>
	/// The winding is handled automatically. This is a semi-expensive operation.
	/// </summary>
	AutoWind,
	/// <summary>
	/// Calculates a cheap and very simple rotation. This may involve some flipping.
	/// </summary>
	Simple
}

[Serializable]
/// <summary>
/// Behaviour used to create paths in the editor.
/// </summary>
public class PathBehaviour : MonoBehaviour 
{
	#region Public properties

	public int NumberOfSegments
	{
		get { return Looped ? _knots.Count : _knots.Count - 1; }
	}

	#endregion

	#region MonoBehaviour fields

	public bool Looped { get { return _looped; } set { _looped = value; _path = null; } }

	public bool WorldSpace { get { return _worldSpace; } set { _worldSpace = value; _path = null; } }

	public bool Normalized { get { return _normalized; } set { _normalized = value; _path = null; } }

	public bool AutoControlPoints { get { return _autoControlPoints; } set { _autoControlPoints = value; _path = null; } }

	public float Tension { get { return _tension; } set { _tension = value; _path = null; } }

	public float Bias { get { return _bias; } set { _bias = value; _path = null; } }

	public float Continuity { get { return _continuity; } set { _continuity = value; _path = null; } }

	public PathOvershootMode OvershootMode { get { return _overshootMode; } set { _overshootMode = value; _path = null; } }

	public RotationMode RotationMode { get { return _rotationMode; } set { _rotationMode = value; _path = null; } }

	public Vector3 StartUp { get { return _startUp; } set { _startUp = value; _path = null; } }

	/// <summary>
	/// A list describing the knot data.
	/// </summary>
	/// <value>The knots.</value>
	public List<CubicBezierKnot> Knots { get { return _knots; } }

	#endregion

	#region Public methods

	/// <summary>
	/// Makes sure every control point follows the rules of the knot they belong to, or re-auto generates the control
	/// points. 
	/// </summary>
	public void RecalculateControlPoints()
	{
		_path = null;
		if (AutoControlPoints) {
			IEnumerable<Vector3> points = _knots.Select( knot => knot.Position);
			var newKnots = CubicBezierKnotUtils.GeneratePathKnots( Looped, points, Tension, Bias, Continuity, true);
			// Copy up positions over to knew knots.
			for (int i = 0; i < _knots.Count; ++i) {
				newKnots[i].Up = _knots[i].Up;
			}
			_knots = newKnots;
		} else {
			for (int i = 0; i < _knots.Count; ++i) {
				_knots[i].RecalculateControlPoint(true, true);
			}
		}
	}

	/// <summary>
	/// Gets the path
	/// </summary>
	/// <returns>
	/// The path. Note that the WorldSpace field only applies to how the path is previewed and 
	/// edited. The path is defined in an arbitary space.
	/// </returns>
	public IPath GetPath()
	{
		if (_path == null) { _path = CalculatePath(); }
		return _path;
	}

	/// <summary>
	/// Gets the path of a segment. A segment is any curve connecting two knots in the path.
	/// </summary>
	/// <returns>The path segment.</returns>
	/// <param name="segmentIndex">
	/// The index of the path segment. Must be less than NumberOfSegments.
	/// </param>
	public IPath GetSegmentPath(int segmentIndex)
	{
		Assert.IsTrue( (Looped && segmentIndex <= _knots.Count) || (!Looped && segmentIndex < _knots.Count) );

		int nextIndex = (segmentIndex + 1) % _knots.Count;

		CubicBezierKnot firstPoint = _knots[segmentIndex];
		CubicBezierKnot secondPoint = _knots[nextIndex];

		var startPoint = firstPoint.Position;
		var startControlPoint = firstPoint.OutControlPoint + firstPoint.Position;
		var endControlPoint = secondPoint.InControlPoint + secondPoint.Position;
		var endPoint = secondPoint.Position;

		if (Normalized)
		{
			return new NormalizedBezierPath(startPoint, startControlPoint, endControlPoint, endPoint);
		}
		else
		{
			return new BezierPath(startPoint, startControlPoint, endControlPoint, endPoint);
		}
	}

	/// <summary>
	/// Converts a point from path space to world space. This is effected by the WorldSpace field.
	/// </summary>
	/// <returns>The point in world space</returns>
	/// <param name="pathPosition">The point in path space.</param>
	public Vector3 PathSpaceToWorldSpace(Vector3 pathPosition)
	{
		return WorldSpace ? pathPosition :  transform.TransformPoint(pathPosition);
	}

	/// <summary>
	/// Converts a rotation from path space to world space. This is effected by the WorldSpace field.
	/// </summary>
	/// <returns>The rotation in world space</returns>
	/// <param name="pathPosition">The rotation in path space.</param>
	public Quaternion PathSpaceToWorldSpace(Quaternion pathRotation)
	{
		return WorldSpace ? pathRotation :  transform.rotation * pathRotation;
	}

	/// <summary>
	/// Converts a point from world space to path space. This is effected by the WorldSpace field.
	/// </summary>
	/// <returns>The point in path space</returns>
	/// <param name="worldPosition">The point in world space.</param>
	public Vector3 WorldSpaceToPathSpace(Vector3 worldPosition)
	{
		return WorldSpace ? worldPosition : transform.InverseTransformPoint(worldPosition);
	}

	/// <summary>
	/// Converts a rotation from world space to path space. This is effected by the WorldSpace field.
	/// </summary>
	/// <returns>The rotation in path space</returns>
	/// <param name="worldPosition">The rotation in world space.</param>
	public Quaternion WorldSpaceToRotationSpace(Quaternion worldRotation)
	{
		return WorldSpace ? worldRotation : Quaternion.Inverse(transform.rotation) * worldRotation;
	}

	#endregion

	#region MonoBehaviour events

	private void OnValidate()
	{
		RecalculateControlPoints();
	}

	#endregion

	#region Private methods

	private IPath CalculatePath()
	{
		if (_knots.Count == 0) { return null; }
	
		RecalculateControlPoints();

		List<IPath> paths = new List<IPath>();

		Vector3 startUp = StartUp;
		Vector3 up = startUp;
		for (int i = 0; i < NumberOfSegments; ++i) {
			IPath path = GetSegmentPath(i);
			if (RotationMode == RotationMode.AutoWind) {
				AutoWindingPath rotationPath = null;

				if (i == NumberOfSegments - 1) {
					rotationPath = new AutoWindingPath(path, up, startUp);
				} else {
					rotationPath = new AutoWindingPath(path, up);
					if (i == 0) {
						startUp = rotationPath.StartUpVector;
					}
				}
				path = rotationPath;
				up = rotationPath.EndUpVector;

			} else if (RotationMode == RotationMode.PerKnotUp) {
				int index = i % _knots.Count;
				int nextIndex = (i + 1) % _knots.Count;
				path = new AutoWindingPath(path, _knots[index].Up, _knots[nextIndex].Up);  
			}
			paths.Add(path);
		}

		return new ChainedPath(OvershootMode, paths);
	}

	#endregion

	#region Private fields

	[Tooltip("Whether the first and last knot's in the path have a joining segment.")]
	[FormerlySerializedAs("Looped")]
	[SerializeField]
	private bool _looped = false;

	[Tooltip(
		"Whether the path is edited in local space or world space. Note, this doesn't change the generated path's data, " + 
		"(the path is defined in an arbitary space), but does affects how it is edited in the scene."
	)]
	[FormerlySerializedAs("WorldSpace")]
	[SerializeField]
	private bool _worldSpace = false;

	[Tooltip(
		"Whether the path should be length normalized or not. Turning this option on is a little more expensive, but will " +
		"result in constant speed motion when interpolating across the path."
	)]
	[FormerlySerializedAs("Normalized")]
	[SerializeField]
	private bool _normalized = false;

	[Tooltip("Whether the editor should automatically create control points, for smooth interpolation between knots.")]
	[FormerlySerializedAs("AutoControlPoints")]
	[SerializeField]
	private bool _autoControlPoints = false;

	[Tooltip(
		"Tension affects how smooth the automatic control points should be. A value of 0 will make the path smooth, a " +
		"value of 1 will make the path sharp. Values outside that range will cause the path fold over itself."
	)]
	[FormerlySerializedAs("Tension")]
	[SerializeField]
	private float _tension = 0f;

	[Range(-1f,1f)]
	[Tooltip(
		"Bias affects how automatic control points should be generated. For a given knot, a positive bias means there " + 
	 	"will be a strong bend at the start of the outgoing segment, and a negative bias means there will be a strong " +
		"bend at the end of the incomming segment. The default bias of 0 gives a neutral weighting.")]
	[FormerlySerializedAs("Bias")]
	[SerializeField]
	private float _bias = 0f;

	[Range(-1f,1f)]
	[Tooltip(
		"Continuity affects how automatic control points should be generated. If this value is 0, control points will " +
		"be C1 continious, (no instant changes in velocity, unless tension is >= 1). This can be though to change how " +
		"sharp the shape is."
	)]
	[FormerlySerializedAs("Continuity")]
	[SerializeField]
	private float _continuity = 0f;

	[Tooltip(
		"The overshoot mode effects how the path handles values outside the range 0-1. " + 
		"Wrapped mode immediately repeats the start of the path when overshooting. " +
		"Extrapolate mode continues along the path the bezier would have followed."
	)]
	[FormerlySerializedAs("OvershootMode")]
	[SerializeField]
	private PathOvershootMode _overshootMode = PathOvershootMode.Clamped;

	[Tooltip(
		"The rotation method to use. Per Knot Mode gives every knot has a predefined up vector." +
		"AutoWind mode will create an automatic rotation based on the curve of the path, using Start Up as the up " +
		"vector for the first knot. This is a semi-expensive operation. Simple mode will use the tangent of the path " +
		"to create select an arbitary up vector. This is cheap, but there may be popping/flipping."
	)]
	[FormerlySerializedAs("RotationMode")]
	[SerializeField]
	private RotationMode _rotationMode = RotationMode.AutoWind;

	[Tooltip(
		"The up vector to use for the first knot, when the AutoWind rotation mode is selected."
	)]
	[FormerlySerializedAs("StartUp")]
	[SerializeField]
	private Vector3 _startUp = new Vector3(0f, 1f, 0f);

	[SerializeField]
	private List<CubicBezierKnot> _knots = new List<CubicBezierKnot>();
	private IPath _path;

	#endregion

}

}
