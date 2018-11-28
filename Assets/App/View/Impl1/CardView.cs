using UnityEngine;

using UniRx;

namespace App.View.Impl1
{
    using Agent;
    using Common;
    using Common.Message;

    /// <summary>
    /// View of a card that is not on the board. This includes Hand, Deck, Graveyard.
    ///
    /// A view of a card on the board is a PieceView.
    /// </summary>
    public class CardView
        : Draggable<ICardAgent>
        , ICardView
    {
        public EPieceType PieceType;
        public AudioClip LeaveHandClip;
        public new IReadOnlyReactiveProperty<ICardView> MouseOver => _mouseOver;

        // no more manually hooking things up via editor: now we resolve them at creation-time
        private TMPro.TextMeshProUGUI _mana;
        private TMPro.TextMeshProUGUI _health;
        private TMPro.TextMeshProUGUI _power;

        public override bool IsValid
        {
            get
            {
                if (!base.IsValid) return false;
                if (_mana == null) return false;
                if (_health == null) return false;
                if (_power == null) return false;
                return true;
            }
        }

        protected override void Begin()
        {
            //Verbosity = 50;
            base.Begin();
        }

        public override void SetAgent(IPlayerView view, ICardAgent agent)
        {
            base.SetAgent(view, agent);
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


            base.MouseOver.Subscribe(v => _mouseOver.Value = v as ICardView).AddTo(this);

            Assert.IsNotNull(agent);
            agent.Power.Subscribe(p => _power.text = $"{p}").AddTo(this);
            agent.Health.Subscribe(p => _health.text = $"{p}").AddTo(this);
            agent.Model.ManaCost.Subscribe(p => _mana.text = $"{p}").AddTo(this);

            FindPiece().material
                = Owner.Value.Color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;

            SquareOver.Subscribe(sq =>
            {
                if (sq != null)
                    BoardView.ShowSquares(Agent.Model, sq);
            }).AddTo(this);

            Assert.IsTrue(IsValid);
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

        private Renderer FindPiece()
        {
            return transform.GetComponentInChildren<MeshRenderer>();
        }

        // used just to downcast from base Draggable.MouseOver<IViewBase>
        private readonly ReactiveProperty<ICardView> _mouseOver = new ReactiveProperty<ICardView>();
    }
}
