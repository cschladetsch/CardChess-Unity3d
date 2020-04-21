namespace UIWidgets
{
	using System;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Text adapter to work with both Unity text and TMPro text.
	/// </summary>
	public class TextAdapter : MonoBehaviour, ITextProxy
	{
		ITextProxy proxy;

		/// <summary>
		/// Proxy.
		/// </summary>
		protected ITextProxy Proxy
		{
			get
			{
				if (proxy == null)
				{
					proxy = GetProxy();
				}

				return proxy;
			}
		}

		/// <summary>
		/// Proxy gameobject.
		/// </summary>
		public GameObject GameObject
		{
			get
			{
				return Proxy.GameObject;
			}
		}

		/// <summary>
		/// Proxy graphic component.
		/// </summary>
		public Graphic Graphic
		{
			get
			{
				return Proxy.Graphic;
			}
		}

		/// <summary>
		/// Proxy value.
		/// </summary>
		public string Value
		{
			get
			{
				return Proxy.Text;
			}

			set
			{
				Proxy.Text = value;
			}
		}

		/// <summary>
		/// Proxy value.
		/// Alias for Value property.
		/// </summary>
		public string Text
		{
			get
			{
				return Proxy.Text;
			}

			set
			{
				Proxy.Text = value;
			}
		}

		/// <summary>
		/// Get text proxy.
		/// </summary>
		/// <returns>Proxy instance.</returns>
		protected virtual ITextProxy GetProxy()
		{
			var text_unity = GetComponent<Text>();
			if (text_unity != null)
			{
				return new TextUnity(text_unity);
			}

#if UIWIDGETS_TMPRO_SUPPORT
			var text_tmpro = GetComponent<TMPro.TextMeshProUGUI>();
			if (text_tmpro != null)
			{
				return new TextTMPro(text_tmpro);
			}
#endif

			Debug.LogWarning("Not found any Text component.", this);

			return new TextNull();
		}

#if UNITY_EDITOR
		/// <summary>
		/// Update layout when parameters changed.
		/// </summary>
		protected virtual void OnValidate()
		{
			GetProxy();
		}
#endif
	}
}