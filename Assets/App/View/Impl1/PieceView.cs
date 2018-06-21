using UnityEngine;

using CoLib;
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

        protected override void Begin()
        {
            base.Begin();
            Coord.Subscribe(c => Move());
            SquareOver.Subscribe(sq =>
            {
                if (sq != null)
                    BoardView.ShowSquares(Agent.Model.Card, sq);
            }).AddTo(this);
        }

        public override void SetAgent(IPlayerView view, IPieceAgent agent)
        {
            base.SetAgent(view, agent);

            Assert.IsNotNull(agent);
            agent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            agent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            //agent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);

            FindPiece().GetComponent<Renderer>().material
                = Owner.Value.Color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;

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
            BoardView.Remove(this);
        }

        private void Move()
        {
            var coord = Coord.Value;
            var go = GameObject;
            var pos = new Vector3(coord.x - BoardView.Width.Value/2 + 0.5f, coord.y - BoardView.Height.Value/2 + 0.5f, -1);
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.MoveTo(go, pos, 0.1, Ease.InOutBounce(), true),
                    Commands.ScaleTo(go, 1, 0.1)
                )
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
            Info($"PieceView Response: {response}");
            if (response.Failed)
            {
                ReturnToStart();
                return;
            }
            var battle = response.Request as Battle;
            if (battle != null)
            {
                ReturnToStart();
                return;
            }

            var move = response.Request as MovePiece;
            if (move != null)
            {
                BoardView.MovePiece(this, Coord.Value);
            }
        }
    }
}
