using System;
using System.Collections.Generic;
using System.Text;
using App.Action;
using Flow;

namespace App
{
    class MockBlackPlayer : Agent.Player
    {
        public override IFuture<PlayCard> PlaceKing()
        {
            var pc = new PlayCard
            {
                Coord = new Coord(3, 7),
                Card = King
            };
            var f = New.Future<PlayCard>();
            f.Value = pc;
            return f;
        }
    }
}
