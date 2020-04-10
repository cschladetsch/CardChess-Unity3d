using App.Model;
using UnityEngine;

using CoLib;
using Dekuple;
using Dekuple.Agent;
using Dekuple.View;
using UniRx;

namespace App.View.Impl1
{
    using Agent;
    using Common;
    using Common.Message;

    /// <summary>
    /// View of a Piece on the Board in the scene.
    /// </summary>
    public class PieceView
        : Draggable<IPieceAgent>
        , IPieceView
    {
        #region Unity Properties
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public AudioClip CancelClip;
        public AudioClip MoveClip;
        public AudioClip HitClip;
        public AudioClip HitBothClip;
        #endregion

        public IReactiveProperty<Coord> Coord => Agent.Coord;
        public IReadOnlyReactiveProperty<bool> Dead => Agent.Dead;

        public override bool IsValid
        {
            get
            {
                if (!base.IsValid) return false;
                if (Health == null) return false;
                if (Power == null) return false;
                return true;
            }
        }

        protected override bool Begin()
        {
            if (!base.Begin())
                return false;
            
            Coord.Subscribe(c => Move());
            SquareOver.Subscribe(sq =>
            {
                if (sq != null)
                    BoardView.ShowSquares(Agent.Model.Card, sq);
            }).AddTo(this);

            return true;
        }

        //public override void SetAgent(IPlayerView view, IPieceAgent agent)
        public void SetAgent(IViewBase view, IAgent agent)
        {
            // base.SetAgent(view, agent);

            Assert.IsNotNull(agent);
            var pieceAgent = agent as IPieceAgent;
            Assert.IsNotNull(pieceAgent.Power);
            Assert.IsNotNull(pieceAgent.Health);
            Assert.IsNotNull(Power);
            Assert.IsNotNull(Health);

            pieceAgent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            pieceAgent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            //agent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);

            var player = Owner.Value as IPlayerModel;
            var color = player.Color;
            FindPiece().GetComponent<Renderer>().material
                = color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;

            MouseOver.DistinctUntilChanged().Subscribe(
                v =>
                {
                    //BoardView.ShowSquares(this);
                }
            );
            Dead.Subscribe(d => { if (d) Die(); });
        }

        GameObject FindPiece()
        {
            return transform.GetComponentInChildren<MeshRenderer>().gameObject;
        }

        void Die()
        {
            Info($"{Agent.Model} died");
            _AudioSource.PlayOneShot(HitBothClip);
            BoardView.Remove(this);
        }

        private void Move()
        {
            var coord = Coord.Value;
            var go = GameObject;
            var pos = new Vector3(coord.x - BoardView.Width.Value/2 + 0.5f, coord.y - BoardView.Height.Value/2 + 0.5f, -1);
            _Queue.Sequence(
                Cmd.Parallel(
                    Cmd.MoveTo(go, pos, 0.1, Ease.InOutBounce(), true),
                    Cmd.ScaleTo(go, 1, 0.1)
                ),
                Cmd.Do(() => _AudioSource.PlayOneShot(MoveClip))
            );
        }

        public override string ToString()
        {
            return $"{PlayerView} {Agent}";
        }

        protected override bool MouseDown()
        {
            Verbose(30, $"MouseDown {this}");
            return IsCurrentPlayer();
        }

        protected override void MouseHover()
        {
            Verbose(30, $"MouseHover {this}");
        }

        protected override void MouseUp(IBoardView board, Coord coord)
        {
            var player = PlayerView.Agent;
            var existing = BoardView.Get(coord);
            if (existing == null)
            {
                player.PushRequest(new MovePiece(PlayerModel, Agent.Model, coord), Response);
                return;
            }

            if (ReferenceEquals(existing, this))
                return;

            // TODO: allow for mounting
            if (existing.SameOwner(this))
                return;

            player.PushRequest(new Battle(PlayerModel, Agent.Model, existing.Agent.Model), Response);
        }

        private void Response(IResponse response)
        {
            Verbose(3, $"PieceView Response: {response}");
            if (response.Failed)
            {
                _AudioSource.PlayOneShot(CancelClip);
                ReturnToStart();
                return;
            }

            switch (response.Request)
            {
                case Battle battle:
                    _AudioSource.PlayOneShot(HitClip);
                    ReturnToStart();
                    return;
                case MovePiece move:
                    BoardView.MovePiece(this, Coord.Value);
                    break;
            }
        }
    }
}
