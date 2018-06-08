using UnityEngine;
using UnityEngine.UI;

using UniRx;
using CoLib;

namespace App.View.Impl1
{
    using Agent;

    public class CardView
        : ViewBase<ICardAgent>
        , ICardView
    {
        public TMPro.TextMeshProUGUI Mana;
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public Image Image;
        public IReadOnlyReactiveProperty<ICardView> MouseOver => _mouseOverFilter;

        public override void Create()
        {
            _backgroundColor = new Ref<Color>(() => Image.color, c => Image.color = c);
            _mouseOver.AsObservable().DistinctUntilChanged().Subscribe(p => _mouseOverFilter.Value = p).AddTo(this);
        }

        public override void SetAgent(ICardAgent agent)
        {
            base.SetAgent(agent);

            agent.Power.DistinctUntilChanged().Subscribe(p => Power.text = $"{p}");
            agent.Health.DistinctUntilChanged().Subscribe(p => Health.text = $"{p}");
            agent.Model.ManaCost.DistinctUntilChanged().Subscribe(p => Mana.text = $"{p}");
        }

        #region UnityCallbacks

        private void OnMouseEnter()
        {
            Info($"Enter {Agent.Model}");
        }

        private void OnMouseOver()
        {
            _mouseOver.Value = this;
        }

        private void OnMouseExit()
        {
            Info($"Exit {Agent.Model}");
        }

        private void OnMouseDown()
        {
            _startLocation = transform.position;
            _screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            _offset = transform.position - Camera.main.ScreenToWorldPoint(
                          new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));

            _Queue.Enqueue(
                Commands.AlphaTo(_backgroundColor, 0, _imageAlphaAnimDuration, Ease.Smooth())
            );
        }

        private void OnMouseDrag()
        {
            var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            var cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + _offset;
            transform.position = cursorPosition;
            transform.SetZ(-0.5f);
        }

        private void OnMouseUp()
        {
            // complete animating to zero-alpha background
            _Queue.RunToEnd();

            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.AlphaTo(_backgroundColor, 1, _imageAlphaAnimDuration, Ease.Smooth()),
                    Commands.MoveTo(
                        transform,
                        _startLocation,
                        0.23,
                        Ease.Smooth()
                    )//,
                    //Commands.Do(() => _dragging = false)
                )
            );
        }
        #endregion

        private Vector3 _startLocation;
        private Vector3 _screenPoint;
        private Vector3 _offset;
        private double _imageAlphaAnimDuration = 0.5;
        private Ref<Color> _backgroundColor;
        private ReactiveProperty<ICardView> _mouseOver = new ReactiveProperty<ICardView>();
        private ReactiveProperty<ICardView> _mouseOverFilter = new ReactiveProperty<ICardView>();
    }
}
