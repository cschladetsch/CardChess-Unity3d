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
        #region Public Methods

        public override void RequestFailed(IRequest req)
        {
        }

        public override void RequestSuccess(IRequest req)
        {
            Verbose(10, $"{this} action success: {req.Action}");
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

        public override Response NewGame()
        {
            var response = base.NewGame();
            if (response.Success)
                CreateActionList();
            return response;
        }

        public void RequestFailed(Guid req)
        {
            Error("Not Implemented");
        }

        #endregion // Public Methods

        #region Protected Methods

        protected abstract void CreateActionList();

        protected MockModelPlayerBase(EColor color)
            : base(color)
        {
            _pass = new Pass(this);
            _endTurn = new TurnEnd(this);
        }

        protected ICardModel MakePiece(EPieceType type)
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
        #endregion

        private int _next = 0;

        #region protected Fields
        protected IRequest _pass;
        protected IRequest _endTurn;
        protected List<Func<IRequest>> _requests;
        protected List<Guid> _requestIds;
        #endregion
    }
}
