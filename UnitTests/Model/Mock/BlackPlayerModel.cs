using System;
using System.Collections.Generic;
using App.Common;
using App.Common.Message;

namespace App.Model.Test
{
    using App.Test;

    class BlackPlayerModel
        : MockPlayerBase
        , IBlackPlayerModel
    {
        public BlackPlayerModel()
            : base(EColor.Black)
        {
        }

        protected override void CreateActionList()
        {
            _requests = new List<Func<IRequest>>()
            {
                () => new AcceptCards(this),
                () => new PlacePiece(this, King, new Coord(4, 5)),
                () => _endTurn,
                () =>
                {
                    var peon = MakePiece(EPieceType.Peon);
                    return new PlacePiece(this, peon, new Coord(3, 3));
                },
                () => _endTurn,
                () => _endTurn,
                () => _endTurn,
                () => _endTurn,
            };
        }
    }
}
