using NUnit.Framework;

namespace App.Agent.Test
{
    using App;
    using App.Test;
    using Model;

    class TestAgentBase : App.Model.Test.TestBaseModel
    {
        protected TestAgentBase()
        {
            LogSubject = this;
            LogPrefix = "AgentTest";
        }

        protected AgentRegistry _agency;
        protected IBoardAgent _boardAgent;
        protected IPlayerModel _whiteAgent;
        protected IPlayerModel _blackAgent;
        protected IArbiterModel _arbiterAgent;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            PrepareBindings();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        public override void PrepareBindings()
        {
            base.PrepareBindings();
            _agency = new AgentRegistry();

            _agency.Bind<IBoardAgent, BoardAgent>(new BoardAgent());
            _agency.Bind<IArbiterAgent, ArbiterAgent>(new ArbiterAgent());
            _agency.Bind<IWhitePlayerAgent, WhitePlayerAgent>();
            _agency.Bind<IBlackPlayerAgent, BlackPlayerAgent>();
            //_reg.Bind<ICardModel, CardModel>();
            //_reg.Bind<IDeckModel, MockDeck>();
            //_reg.Bind<IHandModel, MockHand>();
            //_reg.Bind<IPieceModel, PieceModel>();
            _reg.Resolve();
        }

    }
}
