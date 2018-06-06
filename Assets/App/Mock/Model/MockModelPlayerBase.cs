using System;
using System.Linq;
using System.Collections.Generic;

namespace App.Mock.Model
{
    using Common;
    using Common.Message;
    using App.Model;

    /// <summary>
    /// Common to all Mock players
    /// </summary>
    public abstract class MockModelPlayerBase
        : App.Model.PlayerModelBase
    {
        public override void StartGame()
        {
        }

        public override IRequest Mulligan()
        {
            return _Pass;
        }

        public override IRequest NextAction()
        {
            return _next == _Requests.Count ? null : _Requests[_next++]();
        }

        public override void StartTurn()
        {
            base.StartTurn();
            Hand.Add(Deck.Draw());
        }

        public override void Prepare()
        {
            base.Prepare();
            CreateActionList();
        }

        public void RequestFailed(Guid req)
        {
            Error("Not Implemented");
        }

        protected abstract void CreateActionList();

        protected MockModelPlayerBase(EColor color)
            : base(color)
        {
            _Pass = new Pass(this);
            _EndTurn = new TurnEnd(this);
        }

        protected ICardModel GetCardFromHand(EPieceType type)
        {
            var card = Hand.Cards.FirstOrDefault(c => c.PieceType == type);
            if (card == null)
            {
                Error($"Don't have a {type} in hand!");
                return null;
            }
            Hand.Remove(card);
            return card;
        }

        private int _next = 0;

        protected IRequest _Pass;
        protected IRequest _EndTurn;
        protected List<Func<IRequest>> _Requests;
        protected List<Guid> _RequestIds;
    }
}
