using App.Model;
using Dekuple.Agent;

namespace App.View.Impl1
{
    using UnityEngine;
    using UniRx;
    using Dekuple;
    using Dekuple.View;
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

        public override void SetAgent(IAgent agent)
        {
            var cardAgent = agent as ICardAgent;
            Assert.IsNotNull(cardAgent);
            base.SetAgent(cardAgent);
             
            MouseOver.Subscribe(v => _mouseOver.Value = v as ICardView).AddTo(this);
            cardAgent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            cardAgent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            cardAgent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);
                
            //TODO
                // .material
                // = (Owner.Value as IPlayerModel)?.Color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;

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
