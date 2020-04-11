namespace App.View.Impl1
{
    using UnityEngine;
    using UnityEngine.UI;
    using CoLib;

    /// <summary>
    /// View of a board overlay (not a piece).
    ///
    /// TODO: Use an object pool.
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
            // fancier but slower
            //return Commands.ChangeTo(_alphaRef, 0.5f, 0.03);
            _alphaRef.Value = 0.5f;
            return null;
        }

        public CommandDelegate Clear()
        {
            // fancier but slower
            // return Commands.ChangeTo(_alphaRef, 0, 0.03);
            _alphaRef.Value = 0;
            return null ;
        }
    }
}
