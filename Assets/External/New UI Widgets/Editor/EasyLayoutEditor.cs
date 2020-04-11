namespace EasyLayoutNS
{
	using System;
	using System.Collections.Generic;
	using UnityEditor;

	/// <summary>
	/// EasyLayout editor.
	/// </summary>
	[CustomEditor(typeof(EasyLayout), true)]
	public class EasyLayoutEditor : ConditionalEditor
	{
		void Upgrade()
		{
			Array.ForEach(targets, x =>
			{
				var l = x as EasyLayout;
				if (l != null)
				{
					l.Upgrade();
				}
			});
		}

		static Dictionary<string, Func<SerializedProperty, bool>> isCompactOrGrid = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Compact || (LayoutTypes)x.enumValueIndex == LayoutTypes.Grid },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isCompact = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Compact },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isCompactNotFlexible = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Compact },
			{ "compactConstraint", x => (CompactConstraints)x.enumValueIndex != CompactConstraints.Flexible },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isGrid = new Dictionary<string, Func<SerializedProperty, bool>>()
			{
				{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Grid },
			};

		static Dictionary<string, Func<SerializedProperty, bool>> isGridNotFlexible = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Grid },
			{ "gridConstraint", x => (GridConstraints)x.enumValueIndex != GridConstraints.Flexible },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isFlex = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Flex },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isStaggered = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Staggered },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isEllipse = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex == LayoutTypes.Ellipse },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isNotEllipse = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "layoutType", x => (LayoutTypes)x.enumValueIndex != LayoutTypes.Ellipse },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isSymmetric = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "symmetric", x => x.boolValue },
		};

		static Dictionary<string, Func<SerializedProperty, bool>> isNotSymmetric = new Dictionary<string, Func<SerializedProperty, bool>>()
		{
			{ "symmetric", x => !x.boolValue },
		};

		/// <summary>
		/// Init this instance.
		/// </summary>
		protected override void Init()
		{
			Upgrade();

			Fields = new List<ConditionalFieldInfo>()
			{
				new ConditionalFieldInfo("mainAxis"),
				new ConditionalFieldInfo("layoutType"),

				new ConditionalFieldInfo("groupPosition", 1, isCompactOrGrid),
				new ConditionalFieldInfo("rowAlign", 1, isCompact),
				new ConditionalFieldInfo("innerAlign", 1, isCompact),
				new ConditionalFieldInfo("compactConstraint", 1, isCompact),
				new ConditionalFieldInfo("compactConstraintCount", 2, isCompactNotFlexible),

				new ConditionalFieldInfo("cellAlign", 1, isGrid),
				new ConditionalFieldInfo("gridConstraint", 1, isGrid),
				new ConditionalFieldInfo("gridConstraintCount", 2, isGridNotFlexible),

				new ConditionalFieldInfo("flexSettings", 1, isFlex),
				new ConditionalFieldInfo("staggeredSettings", 1, isStaggered),
				new ConditionalFieldInfo("ellipseSettings", 1, isEllipse),

				new ConditionalFieldInfo("spacing", 0, isNotEllipse),
				new ConditionalFieldInfo("symmetric"),

				new ConditionalFieldInfo("margin", 1, isSymmetric),
				new ConditionalFieldInfo("marginTop", 1, isNotSymmetric),
				new ConditionalFieldInfo("marginBottom", 1, isNotSymmetric),
				new ConditionalFieldInfo("marginLeft", 1, isNotSymmetric),
				new ConditionalFieldInfo("marginRight", 1, isNotSymmetric),

				new ConditionalFieldInfo("topToBottom", 0, isNotEllipse),

				new ConditionalFieldInfo("rightToLeft"),
				new ConditionalFieldInfo("skipInactive"),
				new ConditionalFieldInfo("resetRotation", 0, isNotEllipse),
				new ConditionalFieldInfo("childrenWidth"),
				new ConditionalFieldInfo("childrenHeight"),
			};

			IgnoreFields = new List<string>()
			{
				"m_Padding",
				"m_ChildAlignment",
			};
		}

		/// <summary>
		/// Display additional GUI.
		/// </summary>
		protected override void AdditionalGUI()
		{
			if (targets.Length == 1)
			{
				var script = (EasyLayout)target;

				EditorGUILayout.LabelField("Block size", string.Format("{0}x{1}", script.BlockSize.x, script.BlockSize.y));
				EditorGUILayout.LabelField("UI size", string.Format("{0}x{1}", script.UISize.x, script.UISize.y));
				EditorGUILayout.LabelField("Size", string.Format("{0}x{1}", script.Size.x, script.Size.y));
			}
		}
	}
}