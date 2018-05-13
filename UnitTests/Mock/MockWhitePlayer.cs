using System;
using System.Collections.Generic;
using System.Text;
using App.Action;
using Flow;

namespace App
{
    class MockWhitePlayer : Agent.Player
    {
        public override IFuture<PlayCard> PlaceKing()
        {
            var pc = new PlayCard();
            pc.Coord = new Coord(4, 2);
            pc.Card = King;
            var f = New.Future<PlayCard>();
            f.Value = pc;
            return f;
        }
    }
}
