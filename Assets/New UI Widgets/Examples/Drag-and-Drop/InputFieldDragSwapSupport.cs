namespace UIWidgets.Examples
{
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <summary>
	/// Drag support with content swap for the InputField.
	/// </summary>
	[RequireComponent(typeof(InputField))]
	public class InputFieldDragSwapSupport : InputFieldDragSupport
	{
		/// <summary>
		/// Called by a BaseInputModule when a drag is ended.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		protected override void OnEndDrag(PointerEventData eventData)
		{
			if (!IsDragged)
			{
				return;
			}

			var target = FindTarget(eventData);
			if (target != null)
			{
				target.Drop(Data, eventData);
				Dropped(true);

				// replace dragged text with drop target text
				GetComponent<InputField>().text = (target as InputFieldDropSupport).OriginalData;
			}
			else
			{
				Dropped(false);
			}

			IsDragged = false;
			Cursor.SetCursor(DefaultCursorTexture, DefaultCursorHotSpot, Compatibility.GetCursorMode());
		}
	}
}