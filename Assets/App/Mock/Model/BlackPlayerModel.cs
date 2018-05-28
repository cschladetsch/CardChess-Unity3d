using System;
using System.Collections.Generic;

namespace App.Mock.Model
{
    using App.Agent;
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
                () => new AcceptCards(this),
                () => new PlacePiece(this, King, new Coord(4, 5)),
                () => _EndTurn,
                () =>
                {
                    var peon = MakePiece(EPieceType.Peon);
                    return new PlacePiece(this, peon, new Coord(3, 3));
                },
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
                () => _EndTurn,
            };
        }
    }
}
