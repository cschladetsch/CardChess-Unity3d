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
        public Image Image;
        public TMPro.TextMeshProUGUI Mana;
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;

        public IReadOnlyReactiveProperty<ICardView> MouseOver => _mouseOver;
        public IReadOnlyReactiveProperty<ISquareView> SquareOver => _squareOverFiltered;

        [Inject] private IBoardView BoardView { get; set; }

        public override void Create()
        {
            _backgroundColor = new Ref<Color>(() => Image.color, c => Image.color = c);
            _squareOver.DistinctUntilChanged().Subscribe(s => _squareOverFiltered.Value = s);
        }

        public override void SetAgent(IPlayerView view, ICardAgent agent)
        {
            base.SetAgent(view, agent);

            Assert.IsNotNull(agent);
            agent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            agent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            agent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);
        }

        #region UnityCallbacks

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
                Commands.ScaleTo(gameObject, scale, ScaleTime)
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
            var mp = Input.mousePosition;
            _offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(mp.x, mp.y, _screenPoint.z));

            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.ScaleTo(gameObject, 1, ScaleTime),
                    Commands.AlphaTo(_backgroundColor, 0, ImageAlphaAnimDuration, Ease.Smooth())
                )
            );
        }

        private void OnMouseDrag()
        {
            _dragging = true;
            var mp = Input.mousePosition;
            var cursorPoint = new Vector3(mp.x, mp.y, _screenPoint.z);
            var cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + _offset;
            transform.position = cursorPosition;
            transform.SetZ(-0.5f);
            _squareOver.Value = BoardView.TestRayCast(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            _Queue.RunToEnd();
            if (SquareOver.Value == null)
            {
                ReturnToHand();
                return;
            }

            Assert.IsTrue(IsValid && PlayerView.IsValid && Agent.IsValid);
            PlayerView.Agent.PushRequest(new PlacePiece(PlayerModel, Agent.Model, SquareOver.Value.Coord), Response);
        }

        #endregion

        private void Response(IResponse response)
        {
            _Queue.RunToEnd();
            Info($"CardViewPlaced {response.Request}, response {response.Type}:{response.Error}");
            if (response.Failed)
            {
                ReturnToHand();
                return;
            }

            var place = response.Request as PlacePiece;
            Assert.IsNotNull(place);
            BoardView.PlacePiece(this, place.Coord);
            // TODO: this Destroys the object, and it should not
            // OR, we create a IPieceView/Agent/Model from this CardView
            //PlayerModel.Hand.Remove(Agent.Model);
        }

        public void ReturnToHand()
        {
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.AlphaTo(_backgroundColor, 1, ImageAlphaAnimDuration, Ease.Smooth()),
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

        private bool _dragging;
        private Vector3 _offset;
        private Vector3 _screenPoint;
        private Vector3 _startLocation;
        private Ref<Color> _backgroundColor;
        private const float ScaleTime = 0.230f;
        private const double ImageAlphaAnimDuration = 0.5;
        private readonly ReactiveProperty<ICardView> _mouseOver = new ReactiveProperty<ICardView>();
        private readonly ReactiveProperty<ISquareView> _squareOver = new ReactiveProperty<ISquareView>();
        private readonly ReactiveProperty<ISquareView> _squareOverFiltered = new ReactiveProperty<ISquareView>();
    }
}
