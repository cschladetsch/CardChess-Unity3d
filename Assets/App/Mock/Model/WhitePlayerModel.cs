using System;
using System.Collections.Generic;
using App.Mock;

namespace App.Mock.Model
{
    using Common;
    using Common.Message;

    public class WhitePlayerModel
        : MockModelPlayerBase
        , IWhitePlayerModel
    {
        public WhitePlayerModel()
            : base(EColor.White)
        {
        }

        protected override void CreateActionList()
        {
            _requests = new List<Func<IRequest>>()
            {
                () => new AcceptCards(this),
                () => new PlacePiece(this, King, new Coord(4, 2)),
                () => _endTurn,
                () =>
                {
                    var peon = MakePiece(EPieceType.Peon);
                    return new PlacePiece(this, peon, new Coord(4, 3));
                },
                () => new Battle(this, KingPiece, Board.At(3,3)),
                () => _endTurn,
                () => _endTurn,
                () => _endTurn,
                () => _endTurn,
            };
        }
    }
}
