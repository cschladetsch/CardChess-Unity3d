namespace UIWidgets
{
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Forced recompilation if compilation was not done after Scripting Define Symbols was changed.
	/// </summary>
	public static class ScriptsRecompile
	{
		/// <summary>
		/// Text label for initial state.
		/// </summary>
		public const string StatusInitial = "initial";

		/// <summary>
		/// Text label for state after symbols added.
		/// </summary>
		public const string StatusSymbolsAdded = "symbols added";

		/// <summary>
		/// Text label for recompilation started state.
		/// </summary>
		public const string StatusRecompiledAdded = "recompiled label added";

		/// <summary>
		/// Text label for recompilation labels removed state.
		/// </summary>
		public const string StatusRecompileRemoved = "recompiled label removed";

		const string FolderSuffix = "Folder";
		const string FileSuffix = "RecompilationStatus";

		/// <summary>
		/// Check if forced recompilation required.
		/// </summary>
		[UnityEditor.Callbacks.DidReloadScripts]
		public static void Run()
		{
			#if UIWIDGETS_TMPRO_SUPPORT
			Check("TMPro");
			#endif

			#if UIWIDGETS_DATABIND_SUPPORT
			Check("DataBind");
			#endif
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Conditional compilation.")]
		static bool TypeExists(string fileLabel)
		{
			var path = Utilites.GetAssetPath(fileLabel);
			var script = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript)) as MonoScript;
			if ((script == null) || (script.GetClass() != null))
			{
				return false;
			}

			return true;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Conditional compilation.")]
		static void Check(string folderLabel)
		{
			var status = GetStatus(folderLabel);
			Log("check " + folderLabel + "; status: " + status);

			switch (status)
			{
				case StatusInitial:
					break;
				case StatusSymbolsAdded:
					Compatibility.ForceRecompileByLabel(folderLabel + FolderSuffix);

					SetStatus(folderLabel, StatusRecompiledAdded);

					Log("Forced recompilation started.");
					break;
				case StatusRecompiledAdded:
					Compatibility.RemoveForceRecompileByLabel(folderLabel + FolderSuffix);

					SetStatus(folderLabel, StatusRecompileRemoved);

					Log("Forced recompilation done; labels removing started");
					break;
				case StatusRecompileRemoved:
					SetStatus(folderLabel, StatusInitial);

					Log("Labels removed.");
					break;
				default:
					Debug.LogWarning("Unknown recompile status: " + status);
					break;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Conditional compilation.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "message", Justification = "For the debug purposes.")]
		static void Log(string message)
		{
		}

		/// <summary>
		/// Get forced recompilation status from file with label.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <returns>Status.</returns>
		public static string GetStatus(string label)
		{
			var path = Utilites.GetAssetPath(label + FileSuffix);
			if (path == null)
			{
				return StatusInitial;
			}

			return File.ReadAllText(path);
		}

		/// <summary>
		/// Set forced recompilation status to file with label.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="status">Status.</param>
		public static void SetStatus(string label, string status)
		{
			var path = Utilites.GetAssetPath(label + FileSuffix);
			if (path == null)
			{
				return;
			}

			File.WriteAllText(path, status);
		}
	}
}