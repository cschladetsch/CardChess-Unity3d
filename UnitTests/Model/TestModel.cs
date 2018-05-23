using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;

using App.Common;
using App.Model;
using App.Registry;

namespace UnitTests
{
    [TestFixture]
    class TestModel : Flow.Impl.Logger
    {
        private IRegistry<IModel> _reg;
        private IBoardModel _board;
        private IPlayerModel _white;
        private IPlayerModel _black;
        private IArbiterModel _arbiter;

        // this is only place concrete types are needed
        private void PrepareBindings()
        {
            _reg = new Registry<IModel>();
            _reg.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
            _reg.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
            _reg.Bind<IPlayerModel, PlayerModel>();

            _reg.Bind<ICardModel, CardModel>();
            _reg.Bind<IDeckModel, DeckModel>();
            _reg.Bind<IHandModel, HandModel>();

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
            _arbiter.NewGame(_white, _black);

            Info(_reg.Print());
        }

        [TearDown]
        public void TearDown()
        {
            Info($"{_reg.NumInstances}");
            _reg.Print();
            _arbiter.Destroy();
            _white.Destroy();
            _black.Destroy();
            _arbiter.Destroy();
            _reg.Print();
            Info($"{_reg.NumInstances}");
        }

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

            Assert.AreSame(_white.Arbiter, _black.Arbiter);
            Assert.AreSame(_white.Board, _black.Board);
            Assert.AreSame(_arbiter.WhitePlayer, _white);
            Assert.AreSame(_arbiter.BlackPlayer, _black);
            Assert.AreSame(_board.WhitePlayer, _black.Arbiter.Board.WhitePlayer);
            Assert.AreNotSame(_board.WhitePlayer, _black.Arbiter.Board.BlackPlayer);
            Assert.AreNotSame(_arbiter.Board.Arbiter.WhitePlayer, _board.Arbiter.Board.BlackPlayer);
            Assert.AreSame(_arbiter.Board.Arbiter.WhitePlayer, _board.Arbiter.Board.BlackPlayer.Board.WhitePlayer);
        }

        [Test]
        public void TestBoardPiecePlacement()
        {
            var wk = _white.King;
            var bk = _black.King;

			Assert.AreSame(_arbiter.WhitePlayer, _white);

			Assert.IsTrue(_arbiter.RequestPlayCard(_white, wk, new Coord(4, 1)).Type == EResponse.Ok);

            //_white.Pass();

            //_black.PlayCard(bk, new Coord(4, 6));
            //_black.Pass();

            //Console.WriteLine(board.Print());
        }
    }
}
