using UnityEngine;
using UnityEngine.UI;

using UniRx;
using CoLib;

namespace App.View.Impl1
{
    using Agent;
    using Common;
    using Common.Message;
    using Registry;

    public class CardView
        : ViewBase<ICardAgent>
        , ICardView
    {
        public TMPro.TextMeshProUGUI Mana;
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public Image Image;
        public IReadOnlyReactiveProperty<ICardView> MouseOver => _mouseOver;
        [Inject] private IBoardView BoardView { get; set; }
        [Inject] private IArbiterView ArbiterView { get; set; }

        public override void Create()
        {
            _backgroundColor = new Ref<Color>(() => Image.color, c => Image.color = c);
        }

        public override void SetAgent(IPlayerView view, ICardAgent agent)
        {
            base.SetAgent(view, agent);

            agent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            agent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            agent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);

            // TODO: how to not have to do this
            Power.text = $"{agent.Power}";
            Health.text = $"{agent.Health}";
            Mana.text = $"{agent.Model.ManaCost}";
        }

        #region UnityCallbacks

        private float _scaleTime = 0.230f;
        private void OnMouseEnter()
        {
            if (ScaleTo(1.5f))
                _mouseOver.Value = this;
        }

        private void OnMouseExit()
        {
            if (ScaleTo(1.0f))
                _mouseOver.Value = null;
        }

        private bool ScaleTo(float scale)
        {
            if (_dragging)
                return false;
            _Queue.RunToEnd();
            _Queue.Enqueue(
                Commands.ScaleTo(
                    gameObject,
                    scale,
                    _scaleTime
                )
            );
            return true;
        }

        private void OnMouseOver()
        {
        }

        private void OnMouseDown()
        {
            _dragging = true;
            _startLocation = transform.position;
            _screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            _offset = transform.position - Camera.main.ScreenToWorldPoint(
                          new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));

            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.ScaleTo( gameObject, 1, _scaleTime ),
                    Commands.AlphaTo(_backgroundColor, 0, _imageAlphaAnimDuration, Ease.Smooth())
                )
            );
        }

        private void OnMouseDrag()
        {
            _dragging = true;
            var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            var cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + _offset;
            transform.position = cursorPosition;
            transform.SetZ(-0.5f);
        }

        private void OnMouseUp()
        {
            // complete animating to zero-alpha background
            _Queue.RunToEnd();

            var square = BoardView.TestRayCast(Input.mousePosition);
            if (square == null)
            {
                ReturnToHand();
                return;
            }

            Assert.IsTrue(IsValid && Player.IsValid && Agent.IsValid);

            Player.NewRequest(
                new PlacePiece(Player.Agent.Model, Agent.Model, square.Coord)
                , Response);
        }

        private void Response(IResponse obj)
        {
            Info($"{obj}");
        }

        public void ReturnToHand()
        {
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.AlphaTo(_backgroundColor, 1, _imageAlphaAnimDuration, Ease.Smooth()),
                    Commands.MoveTo(
                        transform,
                        _startLocation,
                        0.34,
                        Ease.Smooth()
                    )
                ),
                Commands.Do(() => _dragging = false)
            );
        }

        #endregion

        private bool _dragging;
        private Vector3 _startLocation;
        private Vector3 _screenPoint;
        private Vector3 _offset;
        private double _imageAlphaAnimDuration = 0.5;
        private Ref<Color> _backgroundColor;
        private readonly ReactiveProperty<ICardView> _mouseOver = new ReactiveProperty<ICardView>();
    }
}
