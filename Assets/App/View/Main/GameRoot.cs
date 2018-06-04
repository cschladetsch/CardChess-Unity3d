using UnityEngine;
using App.Common;
using App.Agent;
using App.Model;
using App.View;

// field not assigned - because it is assigned in Unity3d editor
#pragma warning disable 649

namespace App
{
    /// <inheritdoc />
    /// <summary>
    /// The intended root of all non-canvas objects in the scene.
    /// </summary>
    public class GameRoot
        : View.Impl1.ViewBase
    {
        public IPlayerAgent WhitePlayerAgent;
        public IPlayerModel BlackPlayerAgent;

        public App.Model.ModelRegistry Models;
        public App.Agent.AgentRegistry Agents;
        public App.View.ViewRegistry Views;

        public IBoardAgent BoardAgent;
        public IArbiterAgent ArbiterAgent;
        public IPlayerAgent WhiteAgent;
        public IPlayerAgent BlackAgent;

        /// <summary>
        /// What makes the decisions.
        /// </summary>
        public GameObject[] Startup;

        protected override bool Create()
        {
            base.Create();

            CreateModels();

            _arbiterModel.NewGame(_whitePlayerModel, _blackPlayerModel);

            CreateAgents();

            ArbiterAgent.StartGame(WhiteAgent, BlackAgent);

            return true;
        }

        protected override void Begin()
        {
            base.Begin();


            CreateViews();
        }

        void CreateModels()
        {
            Models = new ModelRegistry();
            Models.Bind<Service.ICardTemplateService, Service.Impl.CardTemplateService>();
            Models.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
            Models.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
            Models.Bind<IPlayerModel, PlayerModel>();
            Models.Bind<ICardModel, CardModel>();
            Models.Bind<IDeckModel, DeckModel>();
            Models.Bind<IHandModel, HandModel>();
            Models.Bind<IPieceModel, PieceModel>();
            Models.Resolve();

            _boardModel = Models.New<IBoardModel>();
            _arbiterModel = Models.New<IArbiterModel>();
            _whitePlayerModel = Models.New<IPlayerModel>(EColor.White);
            _blackPlayerModel = Models.New<IPlayerModel>(EColor.Black);
        }

        private void CreateAgents()
        {
            Agents = new AgentRegistry();

            Agents.Bind<IBoardAgent, BoardAgent>(new BoardAgent(_boardModel));
            Agents.Bind<IArbiterAgent, ArbiterAgent>(new ArbiterAgent(_arbiterModel));
            Agents.Bind<ICardAgent, CardAgent>();
            Agents.Bind<IHandAgent, HandAgent>();
            Agents.Bind<IPieceAgent, PieceAgent>();
            Agents.Bind<IPlayerAgent, PlayerAgent>();
            Agents.Resolve();

            BoardAgent = Agents.New<IBoardAgent>();
            ArbiterAgent = Agents.New<IArbiterAgent>();
            WhiteAgent = Agents.New<IPlayerAgent>(_whitePlayerModel);
            BlackAgent = Agents.New<IPlayerAgent>(_blackPlayerModel);
        }

        void CreateViews()
        {
            Views = new View.ViewRegistry();
            Views.Bind<IBoardView, App.View.Impl1.BoardView>();
            Views.Bind<ICardView, View.Impl1.CardView>();
            Views.Bind<IDeckView, View.Impl1.DeckView>();
            Views.Bind<IHandView, View.Impl1.HandView>();
            Views.Bind<IPieceView, View.Impl1.PieceView>();
            Views.Bind<IPlayerView, View.Impl1.PlayerView>();

            Views.Resolve();
        }

        protected override void Step()
        {
            base.Step();
        }

        public Model.IBoardModel _boardModel;
        public Model.IArbiterModel _arbiterModel;
        public IPlayerModel _whitePlayerModel;
        public IPlayerModel _blackPlayerModel;
    }
}
