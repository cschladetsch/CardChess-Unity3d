using UnityEngine;
using UniRx;

// Reflection deals with this
// ReSharper disable PublicConstructorInAbstractClass

namespace App.Model
{
    using Common;
    using Common.Message;
    using Registry;

    /// <summary>
    /// Common for all PlayerModels.
    /// </summary>
    public abstract class PlayerModelBase
        : ModelBase
        , IPlayerModel
    {
        public EColor Color { get; }
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;
        public bool AcceptedHand { get; private set; }
        public IReactiveProperty<int> MaxMana => _maxMana;
        public IReactiveProperty<int> Mana => _mana;
        public IReactiveProperty<int> Health => _health;
        [Inject] public IBoardModel Board { get; set; }
        [Inject] public IArbiterModel Arbiter { get; set; }
        public IHandModel Hand { get; private set; }
        public IDeckModel Deck { get; private set; }
        public ICardModel King { get; private set; }
        public IPieceModel KingPiece { get; set; }
        [Inject] public Service.ICardTemplateService _CardtemplateService;

        #region Public Methods

        public override string ToString()
        {
            return $"{Color}";//: {MaxMana.Value} MaxMana, {Mana.Value} Mana, {Deck.NumCards} in Deck, {Hand.NumCards} in Hand";
        }

        public PlayerModelBase(EColor color)
            : base(null)
        {
            Color = color;
            SetOwner(this);
        }

        public override void Create()
        {
            base.Create();

            AcceptedHand = false;
            _mana.Value = 0;
            _maxMana.Value = 0;
            King = _CardtemplateService.NewCardModel(this, EPieceType.King);

            // TODO: pass a deck template
            Deck = Registry.New<IDeckModel>(null, this);
            Hand = Registry.New<IHandModel>(this);

            King.Create();
            Deck.Create();
            Hand.Create();
        }

        public override void Prepare()
        {
            base.Prepare();
            Deck.Prepare();
            Hand.Prepare();
        }

        public void NewGame()
        {
            Mana.Value = 0;
            MaxMana.Value = Parameters.MaxManaCap;
            Deck.NewGame();
            Hand.NewGame();
        }

        public void EndGame()
        {
            Info($"{this} EndGame");
        }

        public void CardExhaustion()
        {
            King.ChangeHealth(Parameters.CardExhaustionHealthLoss);
        }

        public virtual void StartTurn()
        {
            MaxMana.Value = (MaxMana.Value + 1) % Parameters.MaxManaCap;
            Mana.Value = MaxMana.Value;
            Info($"{this} starts turn with {Mana.Value} mana");
        }

        public void EndTurn()
        {
            Verbose(20, $"{this} ends turn with {Mana} mana");
        }

        public Response CardDrawn(ICardModel card)
        {
            if (Hand.NumCards.Value == Hand.MaxCards)
                return Response.Fail;
            return Hand.Add(card) ? Response.Ok : Response.Fail;
        }

        public virtual void Result(IRequest req, IResponse response)
        {
            Verbose(5, $"{this}: {req} -> {response}");

            // if these actions failed, return the cards to Hand
            switch (req.Action)
            {
                case EActionType.RejectCards:
                    // TODO:
                    //var cards = (req as IRequest<IList<ICardModel>>)
                    Info("Got cards back from mulligan request (somehow)....");
                    break;
                case EActionType.CastSpell:
                    var cast = req as CastSpell;
                    Assert.IsNotNull(cast);
                    Hand.Add(cast.Spell);
                    ChangeMana(cast.Spell.ManaCost.Value);
                    break;
                case EActionType.PlacePiece:
                    var place = req as PlacePiece;
                    Assert.IsNotNull(place);
                    Hand.Add(place.Card);
                    ChangeMana(place.Card.ManaCost.Value);
                    break;
                case EActionType.GiveItem:
                    var item = req as GiveItem;
                    Assert.IsNotNull(item);
                    Hand.Add(item.Item);
                    ChangeMana(item.Item.ManaCost.Value);
                    break;
            }
        }

        public virtual IRequest Mulligan()
        {
            return null;
        }

        public abstract void StartGame();

        public virtual IRequest NextAction()
        {
            return null;
        }

        public Response ChangeMana(int change)
        {
            var result = Mana.Value + change;
            if (result < 0)
                return new Response(EResponse.Fail, EError.NotEnoughMana);
            Mana.Value = Math.Clamp(0, Parameters.MaxManaCap, result);
            return Common.Message.Response.Ok;
        }

        public Response ChangeMaxMana(int change)
        {
            // TODO: set this via Rx
            MaxMana.Value = Mathf.Clamp(0, Parameters.MaxManaCap, MaxMana.Value + change);
            return Response.Ok;
        }

        public Response DrawHand()
        {
            Hand.Add(Deck.Draw(Parameters.StartHandCardCount));
            Hand.Add(King);

            return Response.Ok;
        }

        #endregion

        #region Private Fields
        private readonly IntReactiveProperty _maxMana = new IntReactiveProperty(Parameters.MaxManaCap);
        private readonly IntReactiveProperty _mana = new IntReactiveProperty(0);
        private readonly IntReactiveProperty _health = new IntReactiveProperty(0);
        #endregion

    }
}
