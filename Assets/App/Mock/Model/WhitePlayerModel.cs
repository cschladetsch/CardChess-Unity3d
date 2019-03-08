using System;
using System.Collections.Generic;
using Dekuple;

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
            _Requests = new List<Func<IGameRequest>>()
            {
                //() => new StartGame(this),
                //() => new RejectCards(this),
                () => new PlacePiece(this, GetCardFromHand(EPieceType.King), new Coord(4, 2)),
                () => _EndTurn,
                () => new PlacePiece(this, GetCardFromHand(EPieceType.Peon), new Coord(4, 3)),
                () => _EndTurn,
                () => new Battle(this, Board.At(4,2), Board.At(3,3)),
                () => _EndTurn,
                () => new MovePiece(this, Board.At(4,3), new Coord(4,4)),
                () => _EndTurn,
                () => new Battle(this, Board.At(4,4), Board.At(4,5)),
                () => _EndTurn,
                () => new PlacePiece(this, GetCardFromHand(EPieceType.Archer), new Coord(2,2)),
                () => _EndTurn,
                () => new PlacePiece(this, GetCardFromHand(EPieceType.Gryphon), new Coord(5,4)),
                () => _EndTurn,
                () => new MovePiece(this, Board.At(2,2), new Coord(1,3)),
                () => new Resign(this),
                () => _EndTurn,
                () => _EndTurn,
            };
        }
    }
}
