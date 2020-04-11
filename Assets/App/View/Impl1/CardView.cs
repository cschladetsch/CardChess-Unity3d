namespace App.View.Impl1
{
    using UnityEngine;
    using UniRx;
<<<<<<< HEAD
    using App.Model;
    using Dekuple;
    using Dekuple.Agent;
    using Dekuple.View;
    using Agent;
=======
    using Dekuple;
    using Dekuple.View;
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
    using Common;
    using Common.Message;
    using Agent;

    /// <inheritdoc cref="ICardView" />
    /// <summary>
    /// View of a card that is <b>not</b> on the Board. This includes the Hand, Deck, Graveyard.
    /// A view of a Card on the Board is a <see cref="IPieceView"/>.
    /// </summary>
    public class CardView
        : Draggable<ICardAgent>
        , ICardView
    {
        public TMPro.TextMeshProUGUI Mana;
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public AudioClip LeaveHandClip;
        public new IReadOnlyReactiveProperty<ICardView> MouseOver => _mouseOver;

        public override bool IsValid
        {
            get
            {
                if (!base.IsValid) return false;
                if (Mana == null) return false;
                if (Health == null) return false;
                if (Power == null) return false;
                return true;
            }
        }

        private readonly ReactiveProperty<ICardView> _mouseOver = new ReactiveProperty<ICardView>();

<<<<<<< HEAD
        public override void SetAgent(IAgent agent)
        {
            var cardAgent = agent as ICardAgent;
            Assert.IsNotNull(cardAgent);
            base.SetAgent(cardAgent);
             
            _mana = FindTextChild("Mana");
            _health = FindTextChild("Health");
            _power = FindTextChild("Power");

            if (_mana == null)
            {
                Error("No Mana text child for {0}", this);
                return;
            }
            if (_health == null)
            {
                Error("No Health text child for {0}", this);
                return;
            }
            if (_power == null)
            {
                Error("No Power text child for {0}", this);
                return;
            }
            
            MouseOver.Subscribe(v => _mouseOver.Value = v as ICardView).AddTo(this);
            cardAgent.Power.Subscribe(p => _power.text = $"{p}").AddTo(this);
            cardAgent.Health.Subscribe(p => _health.text = $"{p}").AddTo(this);
            cardAgent.Model.ManaCost.Subscribe(p => _mana.text = $"{p}").AddTo(this);

            FindPiece().material
                = (Owner.Value as IPlayerModel)?.Color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;

=======
        public void SetAgent(IViewBase view, ICardAgent agent)
        //public override void SetAgent(IViewBase view, IAgent agent)
        {
            Assert.IsNotNull(agent);
            base.SetAgent(view, agent);
            Agent = agent;

            AddSubscriptions();
            AddMesh();
            Assert.IsTrue(IsValid);
        }

        private void AddMesh()
        {
            var root = Instantiate(Agent.Model.Template.MeshPrefab, transform);
            var mesh = root.GetComponentInChildren<MeshRenderer>();
            mesh.material = PlayerModel.Color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;
        }

        private void AddSubscriptions()
        {
            base.MouseOver.Subscribe(v => _mouseOver.Value = v as ICardView).AddTo(this);
            Agent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            Agent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            Agent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);
            
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
            SquareOver.Subscribe(sq =>
            {
                if (sq != null)
                    BoardView.ShowSquares(Agent.Model, sq);
            }).AddTo(this);
        }

        protected override bool MouseDown()
        {
            var inPlay = IsCurrentPlayer();
            if (inPlay)
                _AudioSource.PlayOneShot(LeaveHandClip);
            return inPlay;
        }

        protected override void MouseHover()
        {
            // TODO: popup info
        }

        protected override void MouseUp(IBoardView board, Coord coord)
        {
            Assert.IsTrue(IsValid && PlayerView.IsValid && Agent.IsValid);
            Verbose(30, $"MouseUp: Requesting new piece {this} owned by {PlayerModel} @{coord}");
            PlayerView.Agent.PushRequest(new PlacePiece(PlayerModel, Agent.Model, coord), Response);
        }

        private void Response(IResponse response)
        {
            _Queue.RunToEnd();
            Verbose(10, $"CardViewPlaced {response.Request}, response {response.Type}:{response.Error}");
            if (response.Failed)
            {
                ReturnToStart();
                return;
            }

            Verbose(10, $"Removing {Agent.Model} from {PlayerModel.Hand}");
            PlayerModel.Hand.Remove(Agent.Model);
        }
    }
}
