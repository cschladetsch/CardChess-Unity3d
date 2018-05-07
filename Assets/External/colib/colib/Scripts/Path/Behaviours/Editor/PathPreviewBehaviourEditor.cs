using System;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace CoLib.Editor
{

[CustomEditor(typeof(PathPreviewBehaviour))]
[Serializable]
public class PathPreviewBehaviourEditor : UnityEditor.Editor 
{	
	#region Editor methods

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		DrawPlayControls();

		PathPreviewBehaviour previewBehaviour = target as PathPreviewBehaviour;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Mode"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("PreviewSize"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ShouldOverrideUp"));
		if (previewBehaviour.ShouldOverrideUp) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("OverrideUp"));
		}

		serializedObject.ApplyModifiedProperties();
	}



	private void OnSceneGUI()
	{
		var pathPreviewBehaviour = target as PathPreviewBehaviour;

		if (pathPreviewBehaviour.enabled) {
			SceneView.RepaintAll();
		}
	}

	#endregion

	#region Private methods

	private void DrawPlayControls()
	{
		PathPreviewBehaviour previewBehaviour = target as PathPreviewBehaviour;

		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		GUILayout.Space(3f);

		EditorGUILayout.BeginHorizontal();

		GUILayout.Label("Playback", GUILayout.Width(EditorGUIUtility.labelWidth));

		if (_playTex == null) {
			_playTex = EditorGUIUtility.Load("icons/d_Animation.Play.png") as Texture2D;
		}
		if (_pauseTex == null) {
			_pauseTex = EditorGUIUtility.Load("icons/d_PauseButton.png") as Texture2D;
		}
		if (_nextTex == null) {
			_nextTex = EditorGUIUtility.Load("icons/d_Animation.NextKey.png") as Texture2D;
		}
		if (_prevTex == null) {
			_prevTex = EditorGUIUtility.Load("icons/d_Animation.PrevKey.png") as Texture2D;
		}
		Texture2D playIcon = previewBehaviour.IsPlaying ? _pauseTex : _playTex;
			
		bool wasPressed = GUILayout.Button(_prevTex, EditorStyles.miniButtonLeft, GUILayout.Height(23), GUILayout.Width(29));
		if (wasPressed) {
			previewBehaviour.ChangeTime(- 1 / (60.0 * previewBehaviour.PreviewDuration));
		}
		wasPressed = GUILayout.Button(playIcon, EditorStyles.miniButtonMid, GUILayout.Height(23), GUILayout.Width(29));
		if (wasPressed) {
			previewBehaviour.IsPlaying = !previewBehaviour.IsPlaying;
		}
		wasPressed = GUILayout.Button(_nextTex, EditorStyles.miniButtonRight,GUILayout.Height(23), GUILayout.Width(29));
		if (wasPressed) {
			previewBehaviour.ChangeTime(1 / (60.0 * previewBehaviour.PreviewDuration));
		}

		GUILayout.EndHorizontal();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("PreviewDuration"));
		GUILayout.Space(3f);
		GUILayout.EndVertical();
	}

	#endregion

	#region Private fields

	private Texture2D _playTex;
	private Texture2D _pauseTex;
	private Texture2D _nextTex;
	private Texture2D _prevTex;

	#endregion
}


}