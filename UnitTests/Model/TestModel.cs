using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;

using App.Common;
using App.Model;
using App.Registry;

namespace App.Model.Test
{
    [TestFixture]
    class TestBoard : TestBase
    {
        [Test]
        public void TestBoardCreation()
        {
            Assert.IsNotNull(_board);
            Assert.IsTrue(_board.Width == 8);
            Assert.IsTrue(_board.Height == 8);
            Assert.IsTrue(!_board.Pieces.Any());

            Assert.IsNotNull(_arbiter);
            Assert.AreSame(_reg.New<IBoardModel>(), _board);
            Assert.AreSame(_reg.New<IArbiterModel>(), _board.Arbiter);
            Assert.AreSame(_reg.New<IArbiterModel>(), _arbiter);

            Assert.IsNotNull(_white);
            Assert.IsNotNull(_black);

            _arbiter.NewGame(_white, _black);
            //Assert.AreSame(_white, _arbiter.WhitePlayer);
            //Assert.AreEqual(_white, _arbiter.WhitePlayer);
            //Assert.AreSame(_black, _arbiter.BlackPlayer);
            Assert.AreSame(_white.Arbiter, _black.Arbiter);
            Assert.AreSame(_white.Board, _black.Board);
            //Assert.AreSame(_arbiter.WhitePlayer, _white);
            //Assert.AreSame(_arbiter.BlackPlayer, _black);
            //Assert.AreSame(_board.WhitePlayer, _black.Arbiter.Board.WhitePlayer);
            //Assert.AreNotSame(_board.WhitePlayer, _black.Arbiter.Board.BlackPlayer);
            //Assert.AreNotSame(_arbiter.Board.Arbiter.WhitePlayer, _board.Arbiter.Board.BlackPlayer);
            //Assert.AreSame(_arbiter.Board.Arbiter.WhitePlayer, _board.Arbiter.Board.BlackPlayer.Board.WhitePlayer);
        }

        [Test]
        public void TestGame()
        {
            _arbiter.NewGame(_white, _black);
            Trace.WriteLine(_board.Print());

            Assert.IsTrue(_arbiter.Arbitrate(_white.NextAction()).Success);
            Assert.IsTrue(_arbiter.Arbitrate(_black.NextAction()).Success);
            Trace.WriteLine(_board.Print());

            Assert.IsTrue(_arbiter.Arbitrate(_white.NextAction()).Success);
            Assert.IsTrue(_arbiter.Arbitrate(_black.NextAction()).Success);
            Trace.WriteLine(_board.Print());

            _arbiter.Arbitrate(_white.NextAction());
            _arbiter.Arbitrate(_black.NextAction());
            Trace.WriteLine(_board.Print());

            _arbiter.Arbitrate(_white.NextAction());
            _arbiter.Arbitrate(_black.NextAction());
            Trace.WriteLine(_board.Print());

        }
    }
}
