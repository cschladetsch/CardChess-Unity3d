namespace UIWidgets.Examples
{
	using UIWidgets;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// TableRow component.
	/// </summary>
	public class TableRowComponent : ListViewItem, IViewData<TableRow>
	{
		/// <summary>
		/// Cell01Text.
		/// </summary>
		[SerializeField]
		public Text Cell01Text;

		/// <summary>
		/// Cell02Text.
		/// </summary>
		[SerializeField]
		public Text Cell02Text;

		/// <summary>
		/// Cell03Image.
		/// </summary>
		[SerializeField]
		public Image Cell03Image;

		/// <summary>
		/// Cell04Text.
		/// </summary>
		[SerializeField]
		public Text Cell04Text;

		TableRow Item;

		/// <summary>
		/// Gets foreground graphics for coloring.
		/// </summary>
		public override Graphic[] GraphicsForeground
		{
			get
			{
				return new Graphic[] { Cell01Text, Cell02Text, Cell04Text, };
			}
		}

		/// <summary>
		/// Background graphics for coloring.
		/// </summary>
		public override Graphic[] GraphicsBackground
		{
			get
			{
				return new Graphic[] { };
			}
		}

		/// <summary>
		/// Gets the objects to resize.
		/// </summary>
		/// <value>The objects to resize.</value>
		public GameObject[] ObjectsToResize
		{
			get
			{
				return new GameObject[]
				{
					Cell01Text.transform.parent.gameObject,
					Cell02Text.transform.parent.gameObject,
					Cell03Image.transform.parent.gameObject,
					Cell04Text.transform.parent.gameObject,
				};
			}
		}

		/// <summary>
		/// Set data.
		/// </summary>
		/// <param name="item">Item.</param>
		public void SetData(TableRow item)
		{
			Item = item;

			Cell01Text.text = Item.Cell01;
			Cell02Text.text = Item.Cell02.ToString();
			Cell03Image.sprite = Item.Cell03;
			Cell04Text.text = Item.Cell04.ToString();

			// set transparent color if no icon
			Cell03Image.color = (Cell03Image.sprite == null) ? Color.clear : Color.white;
		}

		/// <summary>
		/// Handle cell clicked event.
		/// </summary>
		/// <param name="cellName">Cell name.</param>
		public void CellClicked(string cellName)
		{
			Debug.Log(string.Format("clicked row {0}, cell {1}", Index, cellName));
			switch (cellName)
			{
				case "Cell01":
					Debug.Log("cell value: " + Item.Cell01);
					break;
				case "Cell02":
					Debug.Log("cell value: " + Item.Cell02);
					break;
				case "Cell03":
					Debug.Log("cell value: " + Item.Cell03);
					break;
				case "Cell04":
					Debug.Log("cell value: " + Item.Cell04);
					break;
				default:
					Debug.Log("cell value: <unknown cell>");
					break;
			}
		}
	}
}