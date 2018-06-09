using System.Linq;
using UnityEngine;
using App.Common;
using App.Agent;
using App.Model;
using App.View;
using App.View.Impl1;

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
        public IPlayerAgent BlackPlayerAgent;

        public App.Model.ModelRegistry Models;
        public App.Agent.AgentRegistry Agents;
        public App.View.ViewRegistry Views;

        public IBoardAgent BoardAgent;
        public IArbiterAgent ArbiterAgent;

        public BoardView BoardView;
        public ArbiterView ArbiterView;

        /// <summary>
        /// What makes the decisions.
        /// </summary>
        public GameObject[] Startup;

        protected override void Begin()
        {
            base.Begin();

            // create *all* models required for startup here
            CreateModels();

            // create *all* agents
            CreateAgents();
            RegisterViews();

            PrepareViews(transform);

            BoardView.SetAgent(null, BoardAgent);
            WhitePlayerAgent.Create();
            BlackPlayerAgent.Create();
            ArbiterAgent.PrepareGame(WhitePlayerAgent, BlackPlayerAgent);
            ArbiterAgent.StartGame();
            ArbiterView.SetAgent(null, ArbiterAgent);

            Trace();
        }

        [ContextMenu("GameRoot-Trace")]
        void Trace()
        {
            Info($"Models: {Models.Print()}");
            foreach (var model in Agents.Instances)
            {
                Assert.IsNotNull(model);
                Assert.IsTrue(model.IsValid);
            }

            Info($"Agents: {Agents.Print()}");
            foreach (var agent in Agents.Instances)
            {
                Assert.IsNotNull(agent.BaseModel);
                Assert.IsNotNull(agent.Arbiter);
            }

            Info($"Views: {Views.Print()}");
            foreach (var view in Views.Instances)
            {
                Assert.IsNotNull(view);
                Assert.IsNotNull(view.AgentBase);
                Assert.IsNotNull(view.AgentBase.BaseModel);
            }

        }
        // Required to prepare views that were made at design time
        // in the editor. These have not been internally wired up yet.
        void PrepareViews(Transform tr)
        {
            foreach (var c in tr.GetComponents<Component>().Cast<IViewBase>())
                ViewRegistry.Prepare(c);

            foreach (Transform ch in tr)
                PrepareViews(ch);
        }

        void CreateModels()
        {
            Models = new ModelRegistry();
            Models.Bind<Service.ICardTemplateService, Service.Impl.CardTemplateService>();
            Models.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
            Models.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
            Models.Bind<Model.ICardModel, Model.CardModel>();
            Models.Bind<IDeckModel, DeckModel>();
            Models.Bind<IHandModel, HandModel>();
            Models.Bind<IPieceModel, PieceModel>();
            Models.Bind<IPlayerModel, PlayerModel>();
            Models.Resolve();

            _boardModel = Models.New<IBoardModel>();
            _arbiterModel = Models.New<IArbiterModel>();
            _whitePlayerModel = Models.New<IPlayerModel>(EColor.White);
            _blackPlayerModel = Models.New<IPlayerModel>(EColor.Black);

            // make all models required
            foreach (var model in Models.Instances.ToList())
                model.Create();

            Info($"Models: {Models.Print()}");
        }

        private void CreateAgents()
        {
            Agents = new AgentRegistry();

            Agents.Bind<IBoardAgent, BoardAgent>(new BoardAgent(_boardModel));
            Agents.Bind<IArbiterAgent, ArbiterAgent>(new ArbiterAgent(_arbiterModel));
            Agents.Bind<ICardAgent, CardAgent>();
            Agents.Bind<IDeckAgent, DeckAgent>();
            Agents.Bind<IHandAgent, HandAgent>();
            Agents.Bind<IPieceAgent, PieceAgent>();
            Agents.Bind<IPlayerAgent, PlayerAgent>();
            Agents.Resolve();

            BoardAgent = Agents.New<IBoardAgent>();
            ArbiterAgent = Agents.New<IArbiterAgent>();
            WhitePlayerAgent = Agents.New<IPlayerAgent>(_whitePlayerModel);
            BlackPlayerAgent = Agents.New<IPlayerAgent>(_blackPlayerModel);
        }

        void RegisterViews()
        {
            Views = new View.ViewRegistry();
            Views.Bind<IBoardView, App.View.Impl1.BoardView>(BoardView);
            Views.Bind<IArbiterView, ArbiterView>(ArbiterView);
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
