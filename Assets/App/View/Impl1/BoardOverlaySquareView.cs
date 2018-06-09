using UnityEngine;
using UnityEngine.UI;

namespace App.View.Impl1
{
    public class BoardOverlaySquareView
        : MonoBehaviour
    {
        public Color Color
        {
            set { GetComponent<Image>().color = value; }
        }

        public void Awake()
        {
            _image = GetComponent<Image>();
        }

        private Image _image;
    }
}
