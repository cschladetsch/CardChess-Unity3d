using UnityEngine;
using UnityEngine.UI;

namespace App.View.Impl1
{
    public class BoardOverlaySquareView
        : MonoBehaviour
    {
        public Color Color
        {
            set
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                _image.color = value;
            }
        }

        public void Awake()
        {
            _image = GetComponent<Image>();
        }

        private Image _image;
    }
}
