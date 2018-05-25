using System;
using System.Collections.Generic;
using App.Action;
using App.Common;

namespace App.Model.Test
{
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

                () =>
                {
                    var peon = GetACardPiece(EPieceType.Peon);
                    return new PlayCard(this, peon, new Coord(4, 3));
                },
                () => _pass,
            };
        }
    }
}
