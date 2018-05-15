using System;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace CoLib.Editor
{

[CustomEditor(typeof(PathBehaviour))]
[Serializable]
public class PathBehaviourEditor : UnityEditor.Editor 
{
	#region Editor overrides

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		DrawPathProperties();
		DrawPointList();
	}

	#endregion

	#region Editor methods

	private void OnSceneGUI()
	{
		var pathBehaviour = target as PathBehaviour;

		int index = 0;
		bool dirty = false;
		foreach (var segment in pathBehaviour.Knots) {
			dirty = dirty || DrawPointHandles(pathBehaviour, segment, index);
			++index;
		}
		DrawSegments(pathBehaviour);

		if (dirty) {
			pathBehaviour.RecalculateControlPoints();
		}
	}

	private void OnEnable()
	{
		_list = new ReorderableList(serializedObject,
			serializedObject.FindProperty("_knots"), 
			true, true, true, true
		);

		_list.drawHeaderCallback += (rect) => {
			EditorGUI.LabelField(rect, "Points");
		};

		_list.drawElementCallback = DrawPointElementName;
		_list.onSelectCallback = (list) => {
			SceneView.RepaintAll();
		};
	}

	#endregion

	#region Private methods

	private void DrawSegments(PathBehaviour pathBehaviour)
	{
		for (int segmentIndex = 0; segmentIndex < pathBehaviour.NumberOfSegments; ++segmentIndex) {
			var curKnot = pathBehaviour.Knots[segmentIndex % pathBehaviour.Knots.Count];
			var nexKnot = pathBehaviour.Knots[(segmentIndex + 1) % pathBehaviour.Knots.Count];

			Vector3 start = pathBehaviour.PathSpaceToWorldSpace(curKnot.Position);
			Vector3 end = pathBehaviour.PathSpaceToWorldSpace(nexKnot.Position);
			Vector3 startControl = pathBehaviour.PathSpaceToWorldSpace(curKnot.OutControlPoint + curKnot.Position);
			Vector3 endControl = pathBehaviour.PathSpaceToWorldSpace(nexKnot.InControlPoint +nexKnot.Position);

			Handles.DrawBezier(start,end, startControl, endControl, Color.white, null, 2f);
		}
	}

	private bool DrawPointHandles(PathBehaviour pathBehaviour, CubicBezierKnot segment, int index)
	{
		// Check for changes in the path position.
		Vector3 position = pathBehaviour.PathSpaceToWorldSpace(segment.Position);

		bool selected = _list.index == index;
		bool dirty = false;

		// Show a label to indicate which point is which.
		string pointName = string.Format("P{0}", index);
		Handles.color = Color.gray;
		Handles.Label(position + new Vector3(0, HandleUtility.GetHandleSize(position) * 0.3f, 0), pointName);

		bool moved = DrawHandle(ref position, position, pathBehaviour, index * 3 + 0, false, "Move Bezier Point", selected);

		if (moved) {
			position = pathBehaviour.WorldSpaceToPathSpace(position);
			_list.index = index;
			// Keep the tangents at relative positions to the bezier point.
			segment.Position = position;
			dirty = true;
		}

		if (segment.Type == CubicBezierKnotType.Singular || pathBehaviour.AutoControlPoints) { return dirty; }

		// Check for changes in to the in control point.
		Vector3 inControlPoint = pathBehaviour.PathSpaceToWorldSpace(segment.InControlPoint + segment.Position);
		Handles.color = selected ? Color.yellow : Color.blue;
		Handles.DrawLine(position, inControlPoint);

		moved = DrawHandle(ref inControlPoint, position, pathBehaviour, index * 3 + 1, true, "Move Bezier Control Point", selected);
		if (moved) {
			inControlPoint = pathBehaviour.WorldSpaceToPathSpace(inControlPoint) - segment.Position;
			_list.index = index;
			segment.InControlPoint = inControlPoint;
			segment.RecalculateControlPoint(true, true);
			dirty = true;
		}

		// Check for changes to the out control point
		Vector3 outControlPoint = pathBehaviour.PathSpaceToWorldSpace(segment.OutControlPoint + segment.Position);
		Handles.color = selected ? Color.yellow : Color.blue;
		Handles.DrawLine(position, outControlPoint);

		moved = DrawHandle(ref outControlPoint, position, pathBehaviour, index * 3 + 2, true, "Move Bezier Control Point", selected);
		if (moved) {
			outControlPoint = pathBehaviour.WorldSpaceToPathSpace(outControlPoint) - segment.Position;
			_list.index = index;

			segment.OutControlPoint = outControlPoint;
			segment.RecalculateControlPoint(false,true);
			dirty = true;
		}
		return dirty;
	}

	private bool DrawHandle(ref Vector3 point, Vector3 basePoint, PathBehaviour pathBehaviour, int index, bool useCircleCap, 
		string undoMessage, bool selected)
	{
		const float HandleSize = 0.05f;

		Handles.CapFunction cap = Handles.DotHandleCap;
		float size = HandleSize * HandleUtility.GetHandleSize(point);
		bool isExtendMode = false; 

		if (useCircleCap)
		{
			isExtendMode = (Event.current.modifiers & EventModifiers.Shift) != 0;

			if (isExtendMode) {
				cap = Handles.CubeHandleCap;
			} else {
				_lastSafeTangent = Vector3.zero;
				cap = Handles.SphereHandleCap;
			}
			size *= 3;
		}
		if (selected) {
			Handles.color = useCircleCap ? new Color(1f, 172 / 255f, 97f / 255f) : Color.yellow;
		} else {
			Handles.color = useCircleCap ? new Color(13f / 255f, 63f / 255f, 147f / 255f) : Color.blue;
		}

		EditorGUI.BeginChangeCheck();
		bool changed = false;
		Vector3 oldPoint = point;
		point = Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.zero, cap);

		if (isExtendMode) {
			Vector3 v1 = (oldPoint - basePoint);
			Vector3 v2 = (point - basePoint);
			if (v1.magnitude > 0f) { 
				_lastSafeTangent = v1.normalized; 
			} else { 
				v1 = _lastSafeTangent; 
			}
			point = Vector3.Project(v2, v1.normalized) + basePoint;
		}
		if (EditorGUI.EndChangeCheck()) {
			changed = true;
		}

		if (changed) {
			Undo.RecordObject(pathBehaviour, undoMessage);
			EditorUtility.SetDirty(pathBehaviour);
		}
		return changed;
	}

	private void DrawPointElementName(Rect rect, int index, bool isFocused, bool isActive)
	{
		SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);
		SerializedProperty prop = element.FindPropertyRelative("Position");
		Vector3 position =  prop.vector3Value;

		string label = string.Format("Point {0} ({1:0.00}, {2:0.00}, {3:0.00})", index, position.x, position.y, position.z);
		EditorGUI.LabelField(rect, label);
	}

	private void DrawPathProperties()
	{

		EditorGUILayout.PropertyField(serializedObject.FindProperty("_normalized"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_looped"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_worldSpace"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_overshootMode"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_rotationMode"));

		var rotationMode = (RotationMode)serializedObject.FindProperty("_rotationMode").enumValueIndex;
		if (rotationMode == RotationMode.AutoWind) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_startUp"));
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("_autoControlPoints"));

		bool autoControlPointsMode = serializedObject.FindProperty("_autoControlPoints").boolValue;

		if (autoControlPointsMode) {
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_tension"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_bias"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_continuity"));
			EditorGUILayout.EndVertical();
		}
	}

	private void DrawPointProperty(SerializedProperty property, int index)
	{
		var pathBehaviour = target as PathBehaviour;
		var segment = pathBehaviour.Knots[index];
		bool dirty = false;

		EditorGUI.indentLevel++;
		EditorGUI.BeginChangeCheck();

		EditorGUI.BeginChangeCheck();
		Vector3 position = EditorGUILayout.Vector3Field("Position", segment.Position);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(pathBehaviour, "Move Bezier Point");
			dirty = true;
			segment.Position = position;
		}

		if (pathBehaviour.RotationMode == RotationMode.PerKnotUp) {
			EditorGUI.BeginChangeCheck();
			Vector3 up = EditorGUILayout.Vector3Field("Up", segment.Up);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(pathBehaviour, "Change Up Vector");
				dirty = true;
				segment.Up = up;
			}
		}

		GUI.enabled = !pathBehaviour.AutoControlPoints;

		var type = (CubicBezierKnotType) EditorGUILayout.EnumPopup("LoggingType", segment.Type);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(pathBehaviour, "Changed Point LoggingType");
			dirty = true;
			segment.Type = type;
			segment.RecalculateControlPoint(true, true);
		}

		if (segment.Type != CubicBezierKnotType.Singular)
		{
			EditorGUI.BeginChangeCheck();
			Vector3 inControlPosition = EditorGUILayout.Vector3Field("In Control Point", segment.InControlPoint);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(pathBehaviour, "Move Control Point");
				dirty = true;
				segment.InControlPoint = inControlPosition;
				segment.RecalculateControlPoint(true, true);
			}

			EditorGUI.BeginChangeCheck();
			Vector3 outControlPosition = EditorGUILayout.Vector3Field("Out Control Point", segment.OutControlPoint);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(pathBehaviour, "Move Control Point");
				dirty = true;
				segment.OutControlPoint = outControlPosition;
				segment.RecalculateControlPoint(false, true);
			}
		}


		GUI.enabled = true;
		EditorGUI.indentLevel--;
		if (dirty) {
			EditorUtility.SetDirty(pathBehaviour);
			pathBehaviour.RecalculateControlPoints();
		}
	}

	private void DrawPointList()
	{
		if (_list.count >= 0 && _list.index >= 0) {
			var selectedProperty = _list.serializedProperty.GetArrayElementAtIndex(_list.index);

			if (selectedProperty != null && _list.count > 0) {
				string label = string.Format("Point {0}", _list.index);
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				EditorGUILayout.LabelField(label, GUILayout.ExpandWidth(false));
				DrawPointProperty(selectedProperty, _list.index);
				EditorGUILayout.EndVertical();
			}
		}

		EditorGUILayout.Separator();
		_scrollView = EditorGUILayout.BeginScrollView(_scrollView, false, false, GUILayout.MaxHeight(400f) );

		_list.DoLayoutList();

		serializedObject.ApplyModifiedProperties();

		EditorGUILayout.EndScrollView();
	}

	#endregion

	#region Private fields

	private ReorderableList _list;
	private Vector2 _scrollView;
	private Vector3 _lastSafeTangent = Vector3.zero;

	#endregion
}

}
