using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    //Editor For Arrow Object Script Asset
    [CustomEditor(typeof(ArrowObject))]
    public class ArrowObjectEditor : Editor
    {
        SerializedProperty arrowPath;
        SerializedProperty arrowTail;
        SerializedProperty arrowHead;
        SerializedProperty updatesByItself;
        SerializedProperty updateSpeed;
        SerializedProperty permanentScriptableObjects;
        SerializedProperty flatShading;

        void OnEnable()
        {
            arrowPath = serializedObject.FindProperty("arrowPath");
            arrowTail = serializedObject.FindProperty("arrowTail");
            arrowHead = serializedObject.FindProperty("arrowHead");
            updatesByItself = serializedObject.FindProperty("updatesByItself");
            updateSpeed = serializedObject.FindProperty("updateSpeed");
            permanentScriptableObjects = serializedObject.FindProperty("permanentScriptableObjects");
            flatShading = serializedObject.FindProperty("flatShading");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(arrowPath, new GUIContent("Arrow Path"));
            EditorGUILayout.PropertyField(arrowTail, new GUIContent("Arrow Tail"));
            EditorGUILayout.PropertyField(arrowHead, new GUIContent("Arrow Head"));
            EditorGUILayout.PropertyField(updatesByItself, new GUIContent("Dynamic Update"));
            EditorGUILayout.PropertyField(updateSpeed, new GUIContent("Update Speed"));
            EditorGUILayout.PropertyField(permanentScriptableObjects, new GUIContent("Permanent Scriptable Objects"));
            EditorGUILayout.PropertyField(flatShading, new GUIContent("Flat shading"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}