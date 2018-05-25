
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model.Test
{
    using App.Action;
    using Common;
    using Model;

    public class MockPlayerWhite
        : PlayerModelBase, IWhitePlayer
    {
        public MockPlayerWhite()
            : base(EColor.White)
        {
            _pass = new Action.Pass(this);
            _actions = new List<Func<IRequest>>()
            {
                () => new Action.AcceptCards(this),
                () => new Action.PlayCard(this, King, new Coord(4, 2)),

                () =>
                {
                    var peon = GetACardPiece(EPieceType.Peon);
                    return new PlayCard(this, peon, new Coord(4, 3));
                },
                () => _pass,
            };
        }

        public void RequestFailed(IRequest req)
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

        public void RequestSuccess(IRequest req)
        {
            Verbose(10, $"{this} action success: {req.Action}");
        }

        ICardModel GetACardPiece(EPieceType type)
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
            return new Action.Pass(this);
        }

        public override IRequest NextAction()
        {
            if (_next == _actions.Count)
                return null;

            return _actions[_next++]();
        }

        public override IRequest StartTurn()
        {
            return new Action.Pass(this);
        }

        private int _next = 0;
        private IRequest _pass;
        private List<Func<IRequest>> _actions;
    }
}

