using App.Common;
using UnityEngine;
using App.View.Impl1;
using CoLib;

namespace App.View
{
    public class CardBackgroundViewXxxx
        : ViewBase
    {
        protected override void Begin()
        {
            base.Begin();
            _cardView = transform.parent.GetComponent<CardView>();
            Assert.IsNotNull(_cardView);
            _cardTransform = _cardView.transform;
            _renderer = GetComponent<Renderer>();
            _backgroundColor = new Ref<Color>(() => _renderer.material.color, c => _renderer.material.color = c);

            _renderer.material.color = new Color(0,0,0,0);
        }

        double duration = 0.23;

        void OnMouseOver()
        {
        }
        void OnMouseDown()
        {
            var c = _renderer.material.color;
            c.a = 0;
            _renderer.material.color = c;
            startLocation = _cardTransform.position;
            screenPoint = Camera.main.WorldToScreenPoint(_cardTransform.position);
            offset = _cardTransform.position -
                     Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                         screenPoint.z));
            _Queue.Enqueue(
                Commands.AlphaTo(_backgroundColor, 0, duration, Ease.Smooth())
            );
        }

        void OnMouseDrag()
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            _cardTransform.position = cursorPosition;
        }

        void OnMouseUp()
        {
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.AlphaTo(_backgroundColor, 1, duration, Ease.Smooth()),
                    Commands.MoveTo(
                        _cardView.transform,
                        startLocation,
                        0.23,
                        Ease.Smooth()
                    )
                )
            );
        }

        private Vector3 startLocation;
        private Vector3 screenPoint;
        private Vector3 offset;
        private CardView _cardView;
        private Transform _cardTransform;

        private Ref<Color> _backgroundColor;
        private Renderer _renderer;
    }
}

