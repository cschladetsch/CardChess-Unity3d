namespace UIWidgets
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Assembly definitions editor.
	/// </summary>
	public static class AssemblyDefinitionsEditor
	{
#if UNITY_2018_1_OR_NEWER
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
		[Serializable]
		class AssemblyDefinition
		{
			/// <summary>
			/// Path.
			/// </summary>
			[NonSerialized]
			public string Path;

			/// <summary>
			/// Assembly name.
			/// </summary>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Reviewed.")]
			public string name = string.Empty;

			/// <summary>
			/// References.
			/// </summary>
			public List<string> references = new List<string>();

			/// <summary>
			/// Optional references.
			/// </summary>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Reviewed.")]
			public List<string> optionalUnityReferences = new List<string>();

			/// <summary>
			/// Include platforms.
			/// </summary>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Reviewed.")]
			public List<string> includePlatforms = new List<string>();

			/// <summary>
			/// Exclude platforms.
			/// </summary>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Reviewed.")]
			public List<string> excludePlatforms = new List<string>();

			/// <summary>
			/// Allow unsafe code.
			/// </summary>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Reviewed.")]
			public bool allowUnsafeCode = false;
		}

		static readonly Dictionary<string, string[]> cachedGUIDs = new Dictionary<string, string[]>()
		{
			{ "l:UiwidgetsTMProRequiredAssemblyDefinition", new string[] {
				"9b890130495c8904b9dcfaf28273b376", // New UI Widgets/Editor/UIWidgets.Editor.asmdef
				"6db589689499ad14c8b33ef4478bb29d", // New UI Widgets/Examples/Editor/UIWidgets.Examples.Editor.asmdef
				"ce661a99e9e51a34eb04bd42897d8e4c", // New UI Widgets/Examples/UIWidgets.Examples.asmdef
				"8158a7b0409ed1b49a27393c5d96ca43", // New UI Widgets/Scripts/Style/Editor/UIWidgets.Styles.Editor.asmdef
				"77b640ebe03a1194598db26ced37dd6e", // New UI Widgets/Scripts/UIWidgets.asmdef
			} },
		};

		static List<AssemblyDefinition> Get(string search)
		{
			var guids = cachedGUIDs.ContainsKey(search)
				? cachedGUIDs[search]
				: AssetDatabase.FindAssets(search);

			var result = new List<AssemblyDefinition>(guids.Length);
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				if (string.IsNullOrEmpty(path))
				{
					Debug.LogWarning("Path not found for the GUID: " + guid);
					continue;
				}

				var asset = Compatibility.LoadAssetAtPath<TextAsset>(path);
				if (asset != null)
				{
					var json = JsonUtility.FromJson<AssemblyDefinition>(asset.text);
					json.Path = path;

					result.Add(json);
				}
			}

			return result;
		}
#endif

		/// <summary>
		/// Add reference to assemblies.
		/// </summary>
		/// <param name="search">Search string for assemblies.</param>
		/// <param name="reference">Reference.</param>
#if !UNITY_2018_1_OR_NEWER
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "search", Justification = "Reviewed.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reference", Justification = "Reviewed.")]
#endif
		public static void Add(string search, string reference)
		{
#if UNITY_2018_1_OR_NEWER
			var assemblies = Get(search);

			foreach (var assembly in assemblies)
			{
				if (!assembly.references.Contains(reference))
				{
					assembly.references.Add(reference);
					File.WriteAllText(assembly.Path, JsonUtility.ToJson(assembly, true));
				}
			}
#endif
		}

		/// <summary>
		/// Check if matched assemblies contains specified reference.
		/// </summary>
		/// <param name="search">Search string for assemblies.</param>
		/// <param name="reference">Reference.</param>
		/// <returns>true if all assemblies contains specified reference; otherwise false</returns>
#if !UNITY_2018_1_OR_NEWER
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "search", Justification = "Reviewed.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reference", Justification = "Reviewed.")]
#endif
		public static bool Contains(string search, string reference)
		{
#if UNITY_2018_1_OR_NEWER
			var assemblies = Get(search);

			foreach (var assembly in assemblies)
			{
				if (!assembly.references.Contains(reference))
				{
					return false;
				}
			}
#endif

			return true;
		}

		/// <summary>
		/// Remove reference from assemblies.
		/// </summary>
		/// <param name="search">Search string for assemblies.</param>
		/// <param name="reference">Reference.</param>
#if !UNITY_2018_1_OR_NEWER
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "search", Justification = "Reviewed")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reference", Justification = "Reviewed")]
#endif
		public static void Remove(string search, string reference)
		{
#if UNITY_2018_1_OR_NEWER
			var assemblies = Get(search);

			foreach (var assembly in assemblies)
			{
				assembly.references.Remove(reference);
				File.WriteAllText(assembly.Path, JsonUtility.ToJson(assembly, true));
			}
#endif
		}
	}
}