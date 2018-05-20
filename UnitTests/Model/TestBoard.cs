using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using App.Common;
using App.Model;

namespace UnitTests
{
    [TestFixture]
    class TestBoard
    {
        [Test]
        public void TestBoardPiecePlacement()
        {
            var reg = new Registry();
            // TODO: use DI and map ifaces to concrete types
            var board = reg.New<BoardModel>(8, 8);
            var w = reg.New<PlayerModel>(EColor.White);
            var b = reg.New<PlayerModel>(EColor.Black);
            var a = reg.New<ArbiterModel>(board, w, b);

            a.NewGame();

            w.DrawHand();
            b.DrawHand();

            w.AcceptHand();
            b.AcceptHand();

            var wk = w.King;
            var bk = b.King;

            w.PlayCard(wk, new Coord(3, 2));
            w.Pass();

            b.PlayCard(bk, new Coord(4, 6));
            b.Pass();

            Console.WriteLine(board.Print());
        }
    }
}
