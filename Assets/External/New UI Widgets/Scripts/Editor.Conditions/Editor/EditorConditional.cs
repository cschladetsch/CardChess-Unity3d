#if UNITY_EDITOR
namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using UIWidgets.Attributes;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>
	/// Conditional editor.
	/// Fields displayed only if match attribute conditions.
	/// </summary>
	public class EditorConditional : Editor
	{
		/// <summary>
		/// Conditions to display field.
		/// </summary>
		protected class DisplayConditions
		{
			/// <summary>
			/// Field name.
			/// </summary>
			public string Name;

			/// <summary>
			/// List of the conditions to display field.
			/// </summary>
			public List<IEditorCondition> Conditions = new List<IEditorCondition>();

			/// <summary>
			/// Initializes a new instance of the <see cref="DisplayConditions"/> class.
			/// </summary>
			/// <param name="name">Field name.</param>
			/// <param name="conditions">List of the conditions to display field.</param>
			public DisplayConditions(string name, List<IEditorCondition> conditions)
			{
				Name = name;
				Conditions = conditions;
			}

			/// <summary>
			/// Check if field can be displayed.
			/// </summary>
			/// <param name="properties">Properties.</param>
			/// <returns>true if field should be displayed; otherwise false.</returns>
			public bool IsValid(Dictionary<string, SerializedProperty> properties)
			{
				foreach (var condition in Conditions)
				{
					if (!properties.ContainsKey(condition.Field))
					{
						Debug.LogWarning(string.Format("Field \"{0}\" referenced to non existing field \"{1}\"; conditional check ignored.", Name, condition.Field));
						continue;
					}

					var property = properties[condition.Field];
					if (!condition.IsValid(property))
					{
						return false;
					}
				}

				return true;
			}
		}

		/// <summary>
		/// Type of the editable object.
		/// </summary>
		protected Type TargetType;

		/// <summary>
		/// Serialized properties.
		/// </summary>
		protected Dictionary<string, SerializedProperty> SerializedProperties = new Dictionary<string, SerializedProperty>();

		/// <summary>
		/// Serialized events.
		/// </summary>
		protected Dictionary<string, SerializedProperty> SerializedEvents = new Dictionary<string, SerializedProperty>();

		/// <summary>
		/// Display conditions for the properties.
		/// </summary>
		protected Dictionary<string, DisplayConditions> PropertyDisplayConditions = new Dictionary<string, DisplayConditions>();

		/// <summary>
		/// Properties.
		/// </summary>
		protected List<string> Properties = new List<string>();

		/// <summary>
		/// Events.
		/// </summary>
		protected List<string> Events = new List<string>();

		/// <summary>
		/// Fill properties list.
		/// </summary>
		protected virtual void FillProperties()
		{
			var property = serializedObject.GetIterator();
			property.NextVisible(true);
			while (property.NextVisible(false))
			{
				if (IsEvent(property))
				{
					Events.Add(property.name);
				}
				else
				{
					Properties.Add(property.name);
					var condition = GetCondition(property.name);
					PropertyDisplayConditions[property.name] = condition;
				}
			}
		}

		/// <summary>
		/// Get field info from the specified type by name.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="name">Field name.</param>
		/// <returns>Field info.</returns>
		protected static FieldInfo GetField(Type type, string name)
		{
			FieldInfo field;

			var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			while ((field = type.GetField(name, flags)) == null && (type = type.BaseType) is Type)
			{
			}

			return field;
		}

		/// <summary>
		/// Get conditions for the specified field.
		/// </summary>
		/// <param name="name">Field name.</param>
		/// <returns>Conditions.</returns>
		protected DisplayConditions GetCondition(string name)
		{
			var field = GetField(TargetType, name);
			var conditions = new List<IEditorCondition>();

			if (field != null)
			{
				var attrs = field.GetCustomAttributes(typeof(IEditorCondition), true);
				foreach (var a in attrs)
				{
					conditions.Add(a as IEditorCondition);
				}
			}
			else
			{
				Debug.LogWarning("field " + name + " is null");
			}

			return new DisplayConditions(name, conditions);
		}

		/// <summary>
		/// Is it event?
		/// </summary>
		/// <param name="property">Property</param>
		/// <returns>true if property is event; otherwise false.</returns>
		protected virtual bool IsEvent(SerializedProperty property)
		{
			var object_type = property.serializedObject.targetObject.GetType();
			var property_type = object_type.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if (property_type == null)
			{
				return false;
			}

			return typeof(UnityEventBase).IsAssignableFrom(property_type.FieldType);
		}

		/// <summary>
		/// Init.
		/// </summary>
		protected virtual void OnEnable()
		{
			TargetType = serializedObject.targetObject.GetType();

			FillProperties();

			Properties.ForEach(x =>
			{
				var property = serializedObject.FindProperty(x);
				if (property != null)
				{
					SerializedProperties[x] = property;
				}
			});

			Events.ForEach(x =>
			{
				var property = serializedObject.FindProperty(x);
				if (property != null)
				{
					SerializedEvents[x] = property;
				}
			});
		}

		/// <summary>
		/// Toggle events block.
		/// </summary>
		protected bool ShowEvents;

		/// <summary>
		/// Draw inspector GUI.
		/// </summary>
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperties.ForEach(property =>
			{
				var conditions = PropertyDisplayConditions[property.Key];
				var is_visible = conditions.IsValid(SerializedProperties);
				if (is_visible)
				{
					var indent = conditions.Conditions.Count;
					EditorGUI.indentLevel += indent;
					EditorGUILayout.PropertyField(property.Value, true);
					EditorGUI.indentLevel -= indent;
				}
			});

			if (Events.Count > 0)
			{
				EditorGUILayout.BeginVertical();

				ShowEvents = GUILayout.Toggle(ShowEvents, "Events", "Foldout", GUILayout.ExpandWidth(true));
				if (ShowEvents)
				{
					SerializedEvents.ForEach(x => EditorGUILayout.PropertyField(x.Value, true));
				}

				EditorGUILayout.EndVertical();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif