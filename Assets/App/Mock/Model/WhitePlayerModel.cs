using System;
using System.Collections.Generic;
using App.Mock;
using App.Model;

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
            _Requests = new List<Func<IRequest>>()
            {
                () => new RejectCards(this),
                () => new PlacePiece(this, King, new Coord(4, 2)),
                () => _EndTurn,
                () => new PlacePiece(this, MakePiece(EPieceType.Peon), new Coord(4, 3)),
                () => new Battle(this, KingPiece, Board.At(3,3)),
                () => new MovePiece(this, Board.At(4,3), new Coord(4,4)),
                () => new Battle(this, Board.At(4,4), Board.At(4,5)),
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
            };
        }
    }
}
