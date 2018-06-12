using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

// field not assigned - because it is assigned in Unity3d editor
#pragma warning disable 649

namespace App
{
    using Common;
    using Agent;
    using Model;
    using View;
    using View.Impl1;

    /// <inheritdoc />
    /// <summary>
    /// The intended root of all non-canvas objects in the scene.
    /// </summary>
    public class GameRoot
        : ViewBase
    {
        public ModelRegistry Models;
        public AgentRegistry Agents;
        public IViewRegistry Views;

        public IPlayerAgent WhitePlayerAgent;
        public IPlayerAgent BlackPlayerAgent;
        public IBoardAgent BoardAgent;
        public IArbiterAgent ArbiterAgent;
        public BoardView BoardView;
        public ArbiterView ArbiterView;

        protected override void Begin()
        {
            Registry = Views;

            base.Begin();
            CreateModels();
            CreateAgents();
            RegisterViews();
            PrepareViews(transform);

            BoardView.SetAgent(null, BoardAgent);
            ArbiterAgent.PrepareGame(WhitePlayerAgent, BlackPlayerAgent);
            ArbiterAgent.StartGame();
            ArbiterView.SetAgent(null, ArbiterAgent);

            IsGood();
        }

        [ContextMenu("GameRoot-IsValid")]
        public void IsGood()
        {
            Assert.IsNotNull(Models);
            Assert.IsNotNull(Agents);
            Assert.IsNotNull(Views);

            TestValidity("Models", Models.Instances);
            TestValidity("Agent", Agents.Instances);
            TestValidity("Views", Views.Instances);
        }

        private void TestValidity(string what, IEnumerable<IEntity> things)
        {
            Info($"TestValidity: {what}");

            foreach (var entity in things)
            {
                if (entity is GameRoot)
                    continue;
                if (entity is BoardOverlayView)
                    continue;
                if (entity is SquareView)
                    continue;

                var valid = entity.IsValid;
                if (!valid)
                {
                    Warn($"NotValid: {entity}: {entity.GetType()}");
                    var secondTest = entity.IsValid;
                    Info($"{secondTest}");
                    var pr = entity as IPrintable;
                    try
                    {
                        if (pr != null)
                            Warn($"\tNotValid:\n\t{pr.Print()}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                Assert.IsTrue(valid);
            }
        }

        protected override void Step()
        {
            base.Step();
        }

        [ContextMenu("GameRoot-Trace")]
        public void Trace()
        {
            Info($"Models: {Models.Print()}");
            Info($"Agents: {Agents.Print()}");
            Info($"Views: {Views.Print()}");
        }

        // Required to prepare views that were made at design time
        // in the editor. These have not been internally wired up yet.
        private void PrepareViews(Transform tr)
        {
            foreach (var c in tr.GetComponents<Component>())
            {
                var v = c as IViewBase;
                if (v == null)
                    continue;
                Views.Prepare(v);
            }

            foreach (Transform ch in tr)
                PrepareViews(ch);
        }

        private void CreateModels()
        {
            Models = new ModelRegistry();
            Models.Bind<Service.ICardTemplateService, Service.Impl.CardTemplateService>();
            Models.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
            Models.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
            Models.Bind<ICardModel, CardModel>();
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
                model.PrepareModels();
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

        private void RegisterViews()
        {
            Views = new ViewRegistry();
            Views.Bind<IBoardView, BoardView>(BoardView);
            Views.Bind<IArbiterView, ArbiterView>(ArbiterView);
            Views.Bind<ICardView, CardView>();
            Views.Bind<IDeckView, DeckView>();
            Views.Bind<IHandView, HandView>();
            Views.Bind<IPieceView, PieceView>();
            Views.Bind<IPlayerView, PlayerView>();

            Views.Resolve();
        }

        [ContextMenu("TraceKernel")]
        public void TraceKernel()
        {
            Info(WhitePlayerAgent.Kernel.Root.ToString());
        }

        [ContextMenu("CheckValidHands")]
        public void CheckValidHands()
        {
            Assert.AreEqual(2, Models.Instances.OfType<IHandModel>().Count());
            Assert.AreEqual(2, Agents.Instances.OfType<IHandAgent>().Count());
            Assert.AreEqual(2, Views.Instances.OfType<IHandView>().Count());
            Info("Ok");
        }

        [ContextMenu("Check")]
        public void Check()
        {
            CheckModels();
            CheckAgents();
            //CheckViews(); // some views made at design time are invalid
        }

        [ContextMenu("CheckAgents")]
        public void CheckAgents()
        {
            foreach (var agent in Agents.Instances)
            {
                Assert.IsTrue(agent.IsValid);
                Assert.IsNotNull(agent.BaseModel);
            }
            Info($"{Models.Instances.Count()} Agents Valid");
        }

        [ContextMenu("CheckModels")]
        public void CheckModels()
        {
            foreach (var agent in Models.Instances)
            {
                Assert.IsTrue(agent.IsValid);
            }
            Info($"{Models.Instances.Count()} Models Valid");
        }

        [ContextMenu("TraceBoard")]
        public void TraceBoard()
        {
            Info(_boardModel.Print());
        }

        [ContextMenu("TraceArbiter")]
        public void TraceArbiter()
        {
            Info(_arbiterModel.ToString());
        }

        [ContextMenu("TracePlayer-White")]
        public void TraceWhite()
        {
            Info(WhitePlayerAgent.Model.ToString());
        }

        [ContextMenu("TracePlayer-Black")]
        public void TraceBlack()
        {
            Info(BlackPlayerAgent.Model.ToString());
        }

        private IBoardModel _boardModel;
        private IArbiterModel _arbiterModel;
        private IPlayerModel _whitePlayerModel;
        private IPlayerModel _blackPlayerModel;
    }
}
