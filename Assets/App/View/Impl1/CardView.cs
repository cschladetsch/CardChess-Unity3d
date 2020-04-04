namespace App.View.Impl1
{
    using UnityEngine;
    using UniRx;
    using Dekuple;
    using Dekuple.View;
    using Common;
    using Common.Message;
    using Agent;

    /// <inheritdoc cref="Draggable{TIAgent}" />
    /// <summary>
    /// View of a card that is not on the board. This includes Hand, Deck, Graveyard.
    /// A view of a card on the board is a PieceView.
    /// </summary>
    public class CardView
        : Draggable<ICardAgent>
        , ICardView
    {
        public Transform ModelRoot;
        public Transform Pivot;
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

        // used just to downcast from base Draggable.MouseOver<IViewBase>
        private readonly ReactiveProperty<ICardView> _mouseOver = new ReactiveProperty<ICardView>();

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

            var place = response.Request as PlacePiece;
            Assert.IsNotNull(place);
            Verbose(20, $"Removing {Agent.Model} from {PlayerModel.Hand}");
            PlayerModel.Hand.Remove(Agent.Model);
        }

        private TMPro.TextMeshProUGUI FindTextChild(string name)
        {
            return transform.FindChildNamed<TMPro.TextMeshProUGUI>(name);
        }
    }
}
