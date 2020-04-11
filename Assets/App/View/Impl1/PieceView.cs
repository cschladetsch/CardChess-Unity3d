namespace App.View.Impl1
{
    using UnityEngine;
    using UniRx;
    using CoLib;
    using Dekuple;
    using Dekuple.View;
    using Common;
    using Common.Message;
    using Agent;
    using Model;

    /// <inheritdoc cref="IPieceView"/>
    /// <summary>
    /// View of a Piece on the Board in the scene.
    /// </summary>
    public class PieceView
        : Draggable<IPieceAgent>
        , IPieceView
    {
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public AudioClip CancelClip;
        public AudioClip MoveClip;
        public AudioClip HitClip;
        public AudioClip HitBothClip;
        public AudioClip DiedClip;
        public IReactiveProperty<Coord> Coord => Agent.Coord;

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

<<<<<<< HEAD
        protected override bool Begin()
=======
        public IReadOnlyReactiveProperty<bool> Dead => Agent.Dead;

        public override string ToString() => $"{PlayerView} {Agent}";

        protected override void Begin()
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
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

<<<<<<< HEAD
        //public override void SetAgent(IPlayerView view, IPieceAgent agent)
        public void SetAgent(IViewBase view, IAgent agent)
        {
            // base.SetAgent(view, agent);

=======
        public void SetAgent(IViewBase view, IPieceAgent agent)
        //public override void SetAgent(IViewBase view, IAgent agent)
        {
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
            Assert.IsNotNull(agent);
            base.SetAgent(view, agent);
            Agent = agent;

            AddSubscriptions();
            AddMesh();
            Assert.IsTrue(IsValid);
        }

        private void AddSubscriptions()
        {
            Agent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            Agent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            //agent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);
            
            MouseOver.DistinctUntilChanged().Subscribe(
                v =>
                {
                    //BoardView.ShowSquares(this);
                }
            );
            
            Dead.Subscribe(dead =>
            {
                if (!dead)
                    return;
                    
                Info($"{Agent.Model} died");
                _Queue.Enqueue(Commands.Do(() => _AudioSource.PlayOneShot(DiedClip)));
                BoardView.Remove(this);
            });
        }

        private void AddMesh()
        {
<<<<<<< HEAD
            Info($"{Agent.Model} died");
            _AudioSource.PlayOneShot(HitBothClip);
            BoardView.Remove(this);
=======
            var root = Instantiate(Agent.Model.Card.Template.MeshPrefab, transform);
            root.transform.localScale *= 0.6f;    // pieces on board are smaller than in Hand/Deck
            root.transform.SetLocalZ(App.Parameters.PieceZOffset);
            var mesh = root.GetComponentInChildren<MeshRenderer>();
            mesh.material = PlayerModel.Color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
        }

        private void Move()
        {
            var coord = Coord.Value;
            var go = GameObject;
            var pos = new Vector3(coord.x - BoardView.Width.Value/2 + 0.5f, coord.y - BoardView.Height.Value/2 + 0.5f, -1);
<<<<<<< HEAD
            _Queue.Sequence(
                Cmd.Parallel(
                    Cmd.MoveTo(go, pos, 0.1, Ease.InOutBounce(), true),
                    Cmd.ScaleTo(go, 1, 0.1)
=======
            _Queue.Enqueue(
                Commands.Parallel(
                    Commands.MoveTo(go, pos, 0.3, Ease.InOutBounce(), true),
                    Commands.ScaleTo(go, 1, 0.3)
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
                ),
                Cmd.Do(() => _AudioSource.PlayOneShot(MoveClip))
            );
        }

        protected override bool MouseDown()
        {
            Verbose(30, $"MouseDown {this}");
            return IsCurrentPlayer();
        }

        protected override void MouseHover()
        {
            // TODO: Start a popup
            // Verbose(30, $"MouseHover {this}");
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
            {
                ReturnToStart();
                return;
            }

            player.PushRequest(new Battle(PlayerModel, Agent.Model, existing.Agent.Model), Response);
        }

        private void Response(IResponse response)
        {
            Verbose(0, $"PieceView: {response}");
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
                    if (battle.Defender.Dead.Value)
                        MoveTo(Coord.Value);
                    else
                        ReturnToStart();
                    return;
 
                case MovePiece move:
                    if (response.Failed)
                        ReturnToStart();
                    else
                        MoveTo(move.Coord);
                    break;
            }
        }

        private void MoveTo(Coord coord)
        {
            BoardView.MovePiece(this, coord);
        }
    }
}
