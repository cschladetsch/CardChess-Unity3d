using UnityEngine;
using UnityEngine.UI;

using CoLib;
using UniRx;

// Resharper doesn't know about Unity's stupid use of reflection
// ReSharper disable UnusedMember.Local

namespace App.View.Impl1
{
    using Agent;
    using Common;

    public abstract class Draggable<TIAgent>
        : ViewBase<TIAgent>
        where TIAgent : class, IAgent
    {
        public Image Image;
        public AudioClip ReturnToStartClip;
        public IReadOnlyReactiveProperty<IViewBase> MouseOver => _mouseOver;
        public IReadOnlyReactiveProperty<ISquareView> SquareOver => _squareOverFiltered;

        protected abstract bool MouseDown();
        protected abstract void MouseHover();
        protected abstract void MouseUp(IBoardView board, Coord coord);

        #region UnityCallbacks

        public override void Create()
        {
            base.Create();
            if (Image != null)
                _backgroundColor = new Ref<Color>(() => Image.color, c => Image.color = c);
            else
                _backgroundColor = new Ref<Color>(() => Color.cyan, c => { });

            _squareOver.DistinctUntilChanged().Subscribe(s => _squareOverFiltered.Value = s);
        }

        private float _lastPickTime;
        private float _minPickDifference = 0.3f;
        private void OnMouseEnter()
        {
            if (Time.time - _lastPickTime < _minPickDifference)
                return;
            _lastPickTime = Time.time;
            if (ScaleTo(1.5f))
                _mouseOver.Value = this;
        }

        private void OnMouseExit()
        {
            if (ScaleTo(1.0f))
                _mouseOver.Value = null;
        }

        private float _oldZ;

        private bool ScaleTo(float scale)
        {
            if (_dragging)
                return false;
            _Queue.RunToEnd();
            var pos = GameObject.transform.position;
            if (scale > 1)
            {
                _oldZ = pos.z;
                pos.z = -10;
            }
            else
                pos.z = _oldZ;

            _Queue.Enqueue(
                Commands.ScaleTo(gameObject, scale, ScaleTime),
                Commands.MoveTo(gameObject, pos, 0)
            );
            return true;
        }

        private void OnMouseOver()
        {
            MouseHover();
        }

        private void OnMouseDown()
        {
            //_startLocation = transform.position;
            _startLocation = transform.localPosition;
            if (!MouseDown())
                return;

            _dragging = true;
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
            if (!_dragging)
                return;

            var mp = Input.mousePosition;
            var cursorPoint = new Vector3(mp.x, mp.y, _screenPoint.z);
            var cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint);
            transform.position = cursorPosition + _cursorOffset;
            transform.SetZ(-0.5f);
            _squareOver.Value = BoardView.TestRayCast(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            _dragging = false;

            _Queue.RunToEnd();
            if (SquareOver.Value == null)
            {
                ReturnToStart();
                return;
            }

            Assert.IsTrue(IsValid && PlayerView.IsValid && Agent.IsValid);
            var coord = SquareOver.Value.Coord;
            MouseUp(BoardView, coord);
        }

        #endregion

        protected void ReturnToStart()
        {
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.Do(() => _AudioSource.PlayOneShot(ReturnToStartClip)),
                    Commands.AlphaTo(_backgroundColor, 1, ImageAlphaAnimDuration, Ease.Smooth()),
                    Commands.ScaleTo(transform, 1, ScaleTime),
                    Commands.MoveTo(
                        transform,
                        _startLocation,
                        0.34,
                        Ease.Smooth(),
                        true
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
        private readonly Vector3 _cursorOffset = new Vector3(0, -0.15f, 0);
        private const double ImageAlphaAnimDuration = 0.5;
        private readonly ReactiveProperty<IViewBase> _mouseOver = new ReactiveProperty<IViewBase>();
        private readonly ReactiveProperty<ISquareView> _squareOver = new ReactiveProperty<ISquareView>();
        private readonly ReactiveProperty<ISquareView> _squareOverFiltered = new ReactiveProperty<ISquareView>();
    }
}
