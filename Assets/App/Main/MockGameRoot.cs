using App.Agent;
using App.Common;
using App.Mock;
using App.Mock.Model;
using App.Model;
using UnityEngine;

// field not assigned - because it is assigned in Unity3d editor
#pragma warning disable CS0649
namespace App
{
    /// <inheritdoc />
    /// <summary>
    /// The intended root of all non-canvas objects in the scene.
    /// </summary>
    class MockGameRoot : View.Impl.ViewBase
    {
        /// <summary>
        /// What makes the decisions.
        /// </summary>
        public ArbiterGameObject Arbiter;

        public GameObject[] Startup;

        protected override bool Create()
        {
            base.Create();

            PrepareModels();
            PrepareAgents();
            PrepareViews();

            return true;
        }

        void PrepareModels()
        {
            _models = new ModelRegistry();
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

        private void PrepareAgents()
        {
            _agents = new AgentRegistry();
            _agents.Bind<IArbiterAgent, ArbiterAgent>();
            _agents.Bind<IBoardAgent, BoardAgent>();
            _agents.Bind<ICardAgent, CardAgent>();
            _agents.Bind<IDeckAgent, DeckAgent>();
        }

        private void PrepareViews()
        {
        }

        private App.Model.ModelRegistry _models;
        private App.Agent.AgentRegistry _agents;
        private App.View.ViewRegistry _views;


    }
}
