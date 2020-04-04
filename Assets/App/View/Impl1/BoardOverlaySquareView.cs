namespace App.View.Impl1
{
    using UnityEngine;
    using UnityEngine.UI;
    using CoLib;

    /// <summary>
    /// View of a board overlay (not a piece).
    /// </summary>
    public class BoardOverlaySquareView
        : GameViewBase
    {
        private Image _image;
        private Ref<float> _alphaRef;

        public override void Create()
        {
            _image = GetComponent<Image>();
            _alphaRef = _image.ToAlphaRef();
            _alphaRef.Value = 0.5f;
        }

        public CommandDelegate SetColor(Color color)
        {
            _image.color = color;
            return Commands.ChangeTo(_alphaRef, 0.5f, 0.03);
        }

        public CommandDelegate Clear()
        {
            return Commands.ChangeTo(_alphaRef, 0, 0.03);
        }
    }
}
