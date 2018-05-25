using System;
using NUnit.Framework;

using App;
using App.Common;
using App.Model;
using App.Registry;

namespace App.Model.Test
{
	[TestFixture]
	class TestBase : Flow.Impl.Logger
	{
		protected IRegistry<IModel> _reg;
		protected IBoardModel _board;
		protected IPlayerModel _white;
		protected IPlayerModel _black;
		protected IArbiterModel _arbiter;

		// this is only place concrete types are needed
		private void PrepareBindings()
		{
			_reg = new Registry<IModel>();
			_reg.Bind<Service.ICardTemplateService, Service.Impl.CardTemplateService>();
			_reg.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
			_reg.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
			_reg.Bind<IWhitePlayer, Test.WhitePlayer>();
			_reg.Bind<Test.IBlackPlayer, Test.BlackPlayer>();

			_reg.Bind<ICardModel, CardModel>();
			_reg.Bind<IDeckModel, DeckModel>();
			_reg.Bind<IHandModel, HandModel>();
			_reg.Bind<IPieceModel, PieceModel>();

			_reg.Resolve();
		}

		[SetUp]
		public void Setup()
		{
			PrepareBindings();

			_board = _reg.New<IBoardModel>();
			_arbiter = _reg.New<IArbiterModel>();
			_white = _reg.New<Test.IWhitePlayer>();
			_black = _reg.New<Test.IBlackPlayer>();
		}

		[TearDown]
		public void TearDown()
		{
			_arbiter.Destroy();
			_white.Destroy();
			_black.Destroy();
			_board.Destroy();
		}
	}
}
