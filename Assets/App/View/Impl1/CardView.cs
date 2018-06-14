using App.Model;
using App.Registry;
using UniRx;
using UnityEngine;

namespace App.View.Impl1
{
    using Agent;
    using Common;
    using Common.Message;

    public class CardView
        : Draggable<ICardAgent>
        , ICardView
    {
        #region Unity Properties
        public TMPro.TextMeshProUGUI Mana;
        public TMPro.TextMeshProUGUI Health;
        public TMPro.TextMeshProUGUI Power;
        public GameObject PieceGameObject;
        #endregion

        public new IReadOnlyReactiveProperty<ICardView> MouseOver => _mouseOver;

        public override void Create()
        {
            base.Create();
            base.MouseOver.Subscribe(
                v => _mouseOver.Value = v as ICardView
            ).AddTo(this);
        }

        public override void SetAgent(IPlayerView view, ICardAgent agent)
        {
            base.SetAgent(view, agent);

            Assert.IsNotNull(agent);
            agent.Power.Subscribe(p => Power.text = $"{p}").AddTo(this);
            agent.Health.Subscribe(p => Health.text = $"{p}").AddTo(this);
            agent.Model.ManaCost.Subscribe(p => Mana.text = $"{p}").AddTo(this);

            PieceGameObject.GetComponent<Renderer>().material
                = Owner.Value.Color == EColor.Black ? BoardView.BlackMaterial : BoardView.WhiteMaterial;
        }

        protected override bool MouseDown()
        {
            return IsCurrentPlayer();
        }

        protected override void MouseHover()
        {
        }

        protected override void MouseUp(IBoardView board, Coord coord)
        {
            Assert.IsTrue(IsValid && PlayerView.IsValid && Agent.IsValid);
            PlayerView.Agent.PushRequest(new PlacePiece(PlayerModel, Agent.Model, coord), Response);
        }

        private void Response(IResponse response)
        {
            _Queue.RunToEnd();
            Info($"CardViewPlaced {response.Request}, response {response.Type}:{response.Error}");
            if (response.Failed)
            {
                ReturnToStart();
                return;
            }

            var place = response.Request as PlacePiece;
            Assert.IsNotNull(place);
            BoardView.PlacePiece(this, place.Coord);
            PlayerModel.Hand.Remove(Agent.Model);
        }

        // used just to downcast from base Draggable.MouseOver<IViewBase>
        private readonly ReactiveProperty<ICardView> _mouseOver = new ReactiveProperty<ICardView>();

    }
}
