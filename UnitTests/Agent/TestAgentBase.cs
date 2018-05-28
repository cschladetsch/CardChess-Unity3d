using NUnit.Framework;

namespace App.Agent.Test
{
    using App;
    using App.Test;
    using Model;
    using Agent;

    [TestFixture]
    class TestAgentBase : App.Model.Test.TestBaseModel
    {
        protected TestAgentBase()
        {
            LogSubject = this;
            LogPrefix = "AgentTest";
        }

        protected AgentRegistry _agency;
        protected IBoardAgent _boardAgent;
        protected IPlayerAgent _whiteAgent;
        protected IPlayerAgent _blackAgent;
        protected IArbiterAgent _arbiterAgent;

        protected override void SetupTest()
        {
            base.SetupTest();
            _boardAgent = _agency.New<IBoardAgent>();
            _arbiterAgent = _agency.New<IArbiterAgent>();
            _whiteAgent = _agency.New<IWhitePlayerAgent>();
            _blackAgent = _agency.New<IBlackPlayerAgent>();
        }

        protected override void TearDownTest()
        {
            _boardAgent.Destroy();
            _arbiterAgent.Destroy();
            _blackAgent.Destroy();
            _whiteAgent.Destroy();
            base.TearDownTest();
        }

        protected override void PrepareBindings()
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
            _agency.Resolve();
        }

    }
}
