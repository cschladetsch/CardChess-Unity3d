using System;
using System.Linq;
using System.Collections.Generic;

namespace App.Model.Test
{
    using Action;
    using Common;
    using Model;

    /// <summary>
    /// Common to all Mock players
    /// </summary>
    public abstract class MockPlayerBase
        : PlayerModelBase
    {
        protected MockPlayerBase(EColor color)
            : base(color)
        {
            _pass = new Pass(this);
            _endTurn = new TurnEnd(this);
        }

        public override Response NewGame()
        {
            var response = base.NewGame();
            if (response.Success)
                CreateActionList();
            return response;
        }

        protected abstract void CreateActionList();

        public void RequestFailed(Guid req)
        {
            Error("Not Implemented");
        }

        public override void RequestFailed(IRequest req)
        {
            Verbose(10, $"{this} action failed: {req.Action}");
            switch (req.Action)
            {
                case EActionType.CastSpell:
                    Hand.Add((req as CastSpell)?.Spell);
                    break;
                case EActionType.PlayCard:
                    Hand.Add((req as PlayCard)?.Card);
                    break;
            }
        }

        public override void RequestSuccess(IRequest req)
        {
            Verbose(10, $"{this} action success: {req.Action}");
        }

        protected ICardModel MakePiece(EPieceType type)
        {
            var card = Hand.Cards.FirstOrDefault(c => c.PieceType == type);
            if (card == null)
            {
                Error("Don't have a {type} in hand!");
                return null;
            }
            Hand.Remove(card);
            return card;
        }

        public override IRequest Mulligan()
        {
            return _pass;
        }

        public override IRequest NextAction()
        {
            return _next == _requests.Count ? null : _requests[_next++]();
        }

        public override IRequest StartTurn()
        {
            Hand.Add(Deck.Draw());
            return _pass;
        }

        private int _next = 0;
        protected IRequest _pass;
        protected IRequest _endTurn;
        protected List<Func<IRequest>> _requests;
        protected List<Guid> _requestIds;
    }
}
