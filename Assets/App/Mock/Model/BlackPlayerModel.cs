using System;
using System.Collections.Generic;
using App.Model;

namespace App.Mock.Model
{
    using Common;
    using Common.Message;

    public class BlackPlayerModel
        : MockModelPlayerBase
        , IBlackPlayerModel
    {
        public BlackPlayerModel()
            : base(EColor.Black)
        {
        }

        protected override void CreateActionList()
        {
            _Requests = new List<Func<IRequest>>()
            {
                //() => new StartGame(this),
                //() => new RejectCards(this),
                () => new PlacePiece(this, King, new Coord(4, 5)),
                () => new PlacePiece(this, GetCardFromHand(EPieceType.Peon), new Coord(3, 3)),
                () => new PlacePiece(this, GetCardFromHand(EPieceType.Archer), new Coord(5,2)),
                () => new PlacePiece(this, GetCardFromHand(EPieceType.Gryphon), new Coord(4,4)),
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
