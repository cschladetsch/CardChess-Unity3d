using System;
using System.Collections.Generic;
using App.Common;
using App.Common.Message;

namespace App.Model.Test
{
    class BlackPlayer
        : MockPlayerBase, IBlackPlayer
    {
        public BlackPlayer()
            : base(EColor.Black)
        {
        }

        protected override void CreateActionList()
        {
            _requests = new List<Func<IRequest>>()
            {
                () => new AcceptCards(this),
                () => new PlayCard(this, King, new Coord(4, 5)),
                () => _endTurn,
                () =>
                {
                    var peon = MakePiece(EPieceType.Peon);
                    return new PlayCard(this, peon, new Coord(3, 3));
                },
                () => _pass,
            };
        }
    }
}
