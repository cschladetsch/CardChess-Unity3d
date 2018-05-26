using System;
using System.Collections.Generic;
using App.Common;

namespace App.Model.Test
{
    using Common;
    using Common.Message;

    class WhitePlayer
        : MockPlayerBase
        , IWhitePlayer
    {
        public WhitePlayer()
            : base(EColor.White)
        {
        }

        protected override void CreateActionList()
        {
            _requests = new List<Func<IRequest>>()
            {
                () => new AcceptCards(this),
                () => new PlayCard(this, King, new Coord(4, 2)),
                () => _endTurn,
                () =>
                {
                    var peon = MakePiece(EPieceType.Peon);
                    return new PlayCard(this, peon, new Coord(4, 3));
                },
                () => _pass,
            };
        }
    }
}
