namespace UIWidgets.Examples
{
	using System;
	using UIWidgets;
	using UnityEngine;

	/// <summary>
	/// Test dialog with different buttons.
	/// </summary>
	public class TestDialogWithDifferentButtons : MonoBehaviour
	{
		/// <summary>
		/// Dialog template.
		/// </summary>
		[SerializeField]
		public Dialog DialogTemplate;

		/// <summary>
		/// Show dialog.
		/// </summary>
		public void ShowDialog()
		{
			var actions = new DialogButton[]
			{
				new DialogButton("Cancel Button", Dialog.AlwaysClose, 1),
				new DialogButton("Main Button", Dialog.AlwaysClose, 0),
			};

			DialogTemplate.Clone().Show(
				title: "Dialog With Different Buttons",
				message: "Test",
				buttons: actions,
				focusButton: "Close",
				modal: true,
				modalColor: new Color(0, 0, 0, 0.8f));
		}
	}
}