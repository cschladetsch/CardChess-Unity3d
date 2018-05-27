using NUnit.Framework;

namespace App.Agent.Test
{
    using App;
    using App.Test;
    using Model;

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
        protected IPlayerModel _whiteAgent;
        protected IPlayerModel _blackAgent;
        protected IArbiterModel _arbiterAgent;

        protected override void SetupTest()
        {
            base.SetupTest();
        }

        protected override void TearDownTest()
        {
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
