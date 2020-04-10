using CoLib;
using UnityEngine;
using UnityEngine.UI;

namespace App.View.Impl1
{
    /// <summary>
    /// View of a board overlay (not a piece)
    /// </summary>
    public class BoardOverlaySquareView
        : GameViewBase
    {
        private Image _image;
        private Ref<float> _alphaRef;

        protected override bool Create()
        {
            if (!base.Create())
                return false;
            
            _image = GetComponent<Image>();
            _alphaRef = _image.ToAlphaRef();
            _alphaRef.Value = 0.5f;

            return true;
        }

        public CommandDelegate SetColor(Color color)
        {
            _image.color = color;
            return Cmd.ChangeTo(_alphaRef, 0.5f, 0.03);
        }

        public CommandDelegate Clear()
        {
            return Cmd.ChangeTo(_alphaRef, 0, 0.03);
        }
    }
}
