using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;

using App.Common;
using App.Model;

namespace UnitTests
{
    [TestFixture]
    class TestBoard : Flow.Impl.Logger
    {
        private IModelRegistry _reg;
        private IBoardModel _board;
        private IPlayerModel _white;
        private IPlayerModel _black;
        private IArbiterModel _arbiter;

        // this is only place concrete types are needed
        private void PrepareBindings()
        {
            _reg = new ModelRegistry();
            _reg.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
            _reg.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
            _reg.Bind<IPlayerModel, PlayerModel>();

            _reg.Resolve();
        }

        [SetUp]
        public void Setup()
        {
            PrepareBindings();

            _board = _reg.New<IBoardModel>();
            _arbiter = _reg.New<IArbiterModel>();
            _white = _reg.New<IPlayerModel>(EColor.White);
            _black = _reg.New<IPlayerModel>(EColor.Black);

            _arbiter.SetPlayers(_white, _black);

            Info(_reg.Print());
        }

        [TearDown]
        public void TearDown()
        {
            Info($"{_reg.NumModels}");
            _reg.Print();
            _arbiter.Destroy();
            _white.Destroy();
            _black.Destroy();
            _arbiter.Destroy();
            _reg.Print();
            Info($"{_reg.NumModels}");
        }

        [Test]
        public void TestBoardCreation()
        {
            Assert.IsNotNull(_board);
            Assert.IsTrue(_board.Width == 8);
            Assert.IsTrue(_board.Height == 8);
            Assert.IsTrue(!_board.GetContents().Any());

            Assert.IsNotNull(_arbiter);
            Assert.AreSame(_reg.New<IBoardModel>(), _board);
            Assert.AreSame(_reg.New<IBoardModel>(), _board);
            Assert.AreNotSame(_reg.New<IArbiterModel>(), _arbiter);

            Assert.IsNotNull(_white);
            Assert.IsNotNull(_black);
        }

        [Test]
        public void TestBoardPiecePlacement()
        {
            //var reg = new ModelRegistry();
            //var board = reg.New<BoardModel>(8, 8);
            //var w = reg.New<PlayerModel>(EColor.White);
            //var b = reg.New<PlayerModel>(EColor.Black);
            //var a = reg.New<ArbiterModel>(board, w, b);

            //a.NewGame();

            //w.DrawHand();
            //b.DrawHand();

            //w.AcceptHand();
            //b.AcceptHand();

            //var wk = w.King;
            //var bk = b.King;

            //w.PlayCard(wk, new Coord(3, 2));
            //w.Pass();

            //b.PlayCard(bk, new Coord(4, 6));
            //b.Pass();

            //Console.WriteLine(board.Print());
        }
    }
}
