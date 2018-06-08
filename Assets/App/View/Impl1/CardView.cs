using App.Agent;
using CoLib;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.View.Impl1
{
    public class CardView
        : ViewBase<ICardAgent>
        , ICardView
    {
        public TMPro.TextMeshProUGUI Mana;
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public Image Image;

        public override void Create()
        {
            _backgroundColor = new Ref<Color>(() => Image.color, c => Image.color = c);
        }

        public override void SetAgent(ICardAgent agent)
        {
            base.SetAgent(agent);

            agent.Power.DistinctUntilChanged().Subscribe(p => Power.text = $"{p}");
            agent.Health.DistinctUntilChanged().Subscribe(p => Health.text = $"{p}");
            agent.Model.ManaCost.DistinctUntilChanged().Subscribe(p => Mana.text = $"{p}");
        }


        void OnMouseDown()
        {
            _startLocation = transform.position;
            _screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            _offset = transform.position - Camera.main.ScreenToWorldPoint(
                          new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));

            _Queue.Enqueue(
                Commands.AlphaTo(_backgroundColor, 0, _imageAlphaAnimDuration, Ease.Smooth())
            );
        }

        void OnMouseDrag()
        {
            var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            var cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + _offset;
            transform.position = cursorPosition;
            transform.SetZ(-0.5f);
        }

        void OnMouseUp()
        {
            _Queue.RunToEnd();
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.AlphaTo(_backgroundColor, 1, _imageAlphaAnimDuration, Ease.Smooth()),
                    Commands.MoveTo(
                        transform,
                        _startLocation,
                        0.23,
                        Ease.Smooth()
                    )
                )
            );
        }

        private Vector3 _startLocation;
        private Vector3 _screenPoint;
        private Vector3 _offset;
        private double _imageAlphaAnimDuration = 0.5;
        private Ref<Color> _backgroundColor;
    }
}
