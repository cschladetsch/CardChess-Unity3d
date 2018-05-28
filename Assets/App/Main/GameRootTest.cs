using UnityEngine.UI;

namespace App
{
    using Mock;
    using Model;
    using Agent;
    using Mock.Model;
    using Mock.Agent;

    public class GameRootTest
    {
        public Button Give;
        public Button Take;

        private App.Registry.Registry<IModel> _models;
        private App.Agent.AgentRegistry _agents;

        private Model.ICardModel _cardModel;
        private Agent.ICardAgent _cardAgent;
        private View.ICardAgentView _cardView;

        void PrepareModel()
        {
            _models = new App.Registry.Registry<IModel>();
            _models.Bind<Service.ICardTemplateService, Service.Impl.CardTemplateService>();
            _models.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
            _models.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
            _models.Bind<IWhitePlayerModel, WhitePlayerModel>();
            _models.Bind<IBlackPlayerModel, BlackPlayerModel>();
            _models.Bind<ICardModel, CardModel>();
            _models.Bind<IDeckModel, MockDeck>();
            _models.Bind<IHandModel, MockHand>();
            _models.Bind<IPieceModel, PieceModel>();
            _models.Resolve();

        }
        void Start()
        {
            _models = new ModelRegistry();
            _agents = new AgentRegistry();

            _models.Bind<ICardModel, CardModel>();
            _agents.Bind<ICardAgent, CardAgent>();

            //_cardModel = _models.New<ICardModel>()

        }
    }
}

