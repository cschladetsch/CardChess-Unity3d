using System;
using System.Linq;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoLib
{

[ExecuteInEditMode]
[RequireComponent(typeof(PathBehaviour))]
public class PathPreviewBehaviour : MonoBehaviour 
{
	#region Public properties

	/// <summary>
	/// Preview animation modes.
	/// </summary>
	public enum PreviewMode 
	{
		/// <summary>
		/// Linearly interpolate t from 0 to 1.
		/// </summary>
		Linear,
		/// <summary>
		/// Linearly interpolate t from 0 to 1, then 1 to 0. 
		/// </summary>
		YoYo,
		/// <summary>
		/// Interpolate using an InOutBack ease.
		/// </summary>
		InOutBack,
		/// <summary>
		/// Interpolate using a smooth ease.
		/// </summary>
		Smooth
	}

	/// <summary>
	/// The animation mode the preview should use.
	/// </summary>
	public PreviewMode Mode = PreviewMode.Linear;

	/// <summary>
	/// How many seconds should the preview take.
	/// </summary>
	[Range(0f, 100f)]
	public double PreviewDuration = 10f;

	/// <summary>
	/// The size of the preview gizmo.
	/// </summary>
	[Range(0f, 100f)]
	public float PreviewSize = 0.3f;

	public bool IsPlaying = true;

	public bool ShouldOverrideUp = false;

	public Vector3 OverrideUp = Vector3.up;

	#endregion

	#if UNITY_EDITOR

	#region Public methods

	public void ChangeTime(double delta)
	{
		_t += delta;
		while (_t < 0f) { _t = PreviewDuration + _t; }
		UpdatePreview();
	}

	#endregion

	#region Unity events

	private void OnEnable()
	{
		_enabled = true;
		EditorApplication.update += UpdatePreview;
	}

	private void OnDisable()
	{
		EditorApplication.update -= UpdatePreview;
		_enabled = false;
	}

	private void OnDrawGizmosSelected()	
	{
		if (!_enabled) { return; }
		var pathBehaviour = GetComponent<PathBehaviour>();
		if (pathBehaviour == null) { return; }

		float previewSize = PreviewSize;
		Vector3 right = (_previewSphereRotation * Vector3.right) * previewSize * 1.5f + _previewSpherePosition;
		Vector3 left = (_previewSphereRotation * Vector3.left) * previewSize * 1.5f + _previewSpherePosition;
		Vector3 forward = (_previewSphereRotation * Vector3.forward) * previewSize * 1.5f + _previewSpherePosition;
		Vector3 up = (_previewSphereRotation * Vector3.up) * previewSize * 1.5f + _previewSpherePosition;

		// Because gizmos don't respect depth sorting, we have to manually sort them, based on their distance to
		// the camera. We start by storing all the sphere gizmos in a data structure.

		DirectionGizmoData centerGizmo = new DirectionGizmoData() {
			Position = _previewSpherePosition,
			Size = previewSize,
			Color = Color.white
		};

		DirectionGizmoData rightGizmo = new DirectionGizmoData() {
			Position = right,
			Size = previewSize / 2,
			Color = Color.green
		};

		DirectionGizmoData leftGizmo = new DirectionGizmoData() {
			Position = left,
			Size = previewSize / 2,
			Color = Color.green
		};

		DirectionGizmoData forwardGizmo = new DirectionGizmoData() {
			Position = forward,
			Size = previewSize / 2,
			Color = Color.red
		};

		DirectionGizmoData upGizmo = new DirectionGizmoData() {
			Position = up,
			Size = previewSize / 2,
			Color = Color.blue
		};

		var gizmos = new DirectionGizmoData[] {
			centerGizmo, rightGizmo, leftGizmo, forwardGizmo, upGizmo
		};

		Camera camera = SceneView.currentDrawingSceneView.camera;
		// Sort from furthest to closest to SceneView camera.
		var sortedGizmos = gizmos.OrderBy( (data) => {
			return -camera.worldToCameraMatrix.MultiplyPoint(data.Position).magnitude;
		});

		// Draw each gizmo.
		foreach (DirectionGizmoData gizmo in sortedGizmos) {
			Gizmos.color = gizmo.Color;
			Gizmos.DrawSphere(gizmo.Position, gizmo.Size);
		}
	}

	#endregion

	#region Private types

	private struct DirectionGizmoData
	{
		public Vector3 Position;
		public float Size;
		public Color Color;
	}

	#endregion

	#region Private methods

	private void UpdatePreview()
	{
		UpdatePreview(true);
	}

	private void UpdatePreview(bool addDelta)
	{
		var pathBehaviour = GetComponent<PathBehaviour>();
		if (pathBehaviour == null ) { return; }

		IPath path = pathBehaviour.GetPath();

		double deltaTime = 0.0;
		if (addDelta) {
			deltaTime = EditorApplication.timeSinceStartup - _lastPreviewTime;
			_lastPreviewTime = EditorApplication.timeSinceStartup;

			if (!IsPlaying) {
				deltaTime = 0.0;
			}
		}
		if (PreviewDuration > 0.0) {
			_t = (_t + deltaTime / PreviewDuration) % 1.0;
		}
		double  t = _t;
		switch (Mode) {
			case PreviewMode.InOutBack:
				t = Ease.InOutBack()(t);
				break;
			case PreviewMode.Smooth:
				t = Ease.Smooth()(t);
				break;
			case PreviewMode.YoYo:
				t = (t > 0.5 ? 1.0 - t : t) * 2; 
				break;
		}

		_previewSpherePosition = pathBehaviour.PathSpaceToWorldSpace(path.GetPoint((float)t));
		if (ShouldOverrideUp) {
			Vector3 up = OverrideUp.magnitude > 0f ? OverrideUp : Vector3.up;
			_previewSphereRotation = pathBehaviour.PathSpaceToWorldSpace(path.GetRotation((float) t, up));
		} else { 
			_previewSphereRotation = pathBehaviour.PathSpaceToWorldSpace(path.GetRotation((float) t));
		}
	}

	#endregion

	#endif

	#region Private fields

	//private Vector3 _previewSpherePosition;
	//private Quaternion _previewSphereRotation;
	//private bool _enabled = false;
	//private double _lastPreviewTime = 0f;
	//private double _t = 0f;

	#endregion
}

}
