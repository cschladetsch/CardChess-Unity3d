using System;
using System.Collections.Generic;
using System.Linq;
using App.Agent.Impl;
using App.Model.Impl;
using Dekuple;
using Dekuple.Agent;
using Dekuple.Model;
using Dekuple.View;
using Dekuple.View.Impl;
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
    using Service.Impl;

    /// <inheritdoc />
    /// <summary>
    /// The intended root of all non-canvas objects in the scene.
    /// </summary>
    public class GameRoot
        : ViewBase
    {
        public IPlayerAgent WhitePlayerAgent;
        public IPlayerAgent BlackPlayerAgent;
        public IBoardAgent BoardAgent;
        public IArbiterAgent ArbiterAgent;
        public BoardView BoardView;
        public ArbiterView ArbiterView;
        public float SKyRotationSpeedMultiplier = 2;

        protected override void Begin()
        {
            Registry = _views;

            base.Begin();
            CreateModels();
            CreateAgents();
            RegisterViews();
            PrepareViews(transform);

            BoardView.SetAgent(null, BoardAgent);
            ArbiterAgent.PrepareGame(WhitePlayerAgent, BlackPlayerAgent);
            ArbiterAgent.StartGame();
            ArbiterView.SetAgent(null, ArbiterAgent);

            CheckAllValid();
        }

        [ContextMenu("GameRoot-IsValid")]
        public void CheckAllValid()
        {
            Assert.IsNotNull(_models);
            Assert.IsNotNull(_agents);
            Assert.IsNotNull(_views);

            TestValidity("Models", _models.Instances);
            TestValidity("Agent", _agents.Instances);
            TestValidity("Views", _views.Instances);
        }

        protected override void Step()
        {
            base.Step();
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * SKyRotationSpeedMultiplier);

        }

        [ContextMenu("GameRoot-Trace")]
        public void Trace()
        {
            Info($"Models: {_models.Print()}");
            Info($"Agents: {_agents.Print()}");
            Info($"Views: {_views.Print()}");
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
                _views.Prepare(v);
            }

            foreach (Transform ch in tr)
                PrepareViews(ch);
        }

        [ContextMenu("TestWhiteHand")]
        public void TestWhiteHand()
        {
        }

        private void CreateModels()
        {
            _models = new ModelRegistry();
            _models.Bind<Service.ICardTemplateService, CardTemplateService>(new CardTemplateService());
            _models.Bind<IBoardModel, BoardModel>(new BoardModel(8, 8));
            _models.Bind<IArbiterModel, ArbiterModel>(new ArbiterModel());
            _models.Bind<IEndTurnButtonModel, EndTurnButtonModel>();
            _models.Bind<ICardModel, CardModel>();
            _models.Bind<IDeckModel, DeckModel>();
            _models.Bind<IHandModel, HandModel>();
            _models.Bind<IPieceModel, PieceModel>();
            _models.Bind<IPlayerModel, PlayerModel>();
            _models.Resolve();

            _boardModel = _models.New<IBoardModel>();
            _arbiterModel = _models.New<IArbiterModel>();
            _whitePlayerModel = _models.New<IPlayerModel>(EColor.White);
            _blackPlayerModel = _models.New<IPlayerModel>(EColor.Black);

            // make all models required. this resolves any cycles of dependancy for
            // singletons, as well as creates models used internally by other models.
            foreach (var model in _models.Instances.ToList())
                model.PrepareModels();
        }

        private void CreateAgents()
        {
            _agents = new AgentRegistry();
            _agents.Bind<IBoardAgent, BoardAgent>(new BoardAgent(_boardModel));
            _agents.Bind<IArbiterAgent, ArbiterAgent>(new ArbiterAgent(_arbiterModel));
            _agents.Bind<IEndTurnButtonAgent, EndTurnButtonAgent>();
            _agents.Bind<ICardAgent, CardAgent>();
            _agents.Bind<IDeckAgent, DeckAgent>();
            _agents.Bind<IHandAgent, HandAgent>();
            _agents.Bind<IPieceAgent, PieceAgent>();
            _agents.Bind<IPlayerAgent, PlayerAgent>();
            _agents.Resolve();
            BoardAgent = _agents.New<IBoardAgent>();
            ArbiterAgent = _agents.New<IArbiterAgent>();
            WhitePlayerAgent = _agents.New<IPlayerAgent>(_whitePlayerModel);
            BlackPlayerAgent = _agents.New<IPlayerAgent>(_blackPlayerModel);
        }

        private void RegisterViews()
        {
            _views = new ViewRegistry();
            _views.Bind<IBoardView, BoardView>(BoardView);
            _views.Bind<IArbiterView, ArbiterView>(ArbiterView);
            _views.Bind<IEndTurnButtonView, EndTurnButtonView>();
            _views.Bind<ICardView, CardView>();
            _views.Bind<IDeckView, DeckView>();
            _views.Bind<IHandView, HandView>();
            _views.Bind<IPieceView, PieceView>();
            _views.Bind<IPlayerView, PlayerView>();
            _views.Resolve();
        }

        [ContextMenu("TraceKernel")]
        public void TraceKernel()
        {
            Info(WhitePlayerAgent.Kernel.Root.ToString());
        }

        [ContextMenu("CheckValidHands")]
        public void CheckValidHands()
        {
            Assert.AreEqual(2, _models.Instances.OfType<IHandModel>().Count());
            Assert.AreEqual(2, _agents.Instances.OfType<IHandAgent>().Count());
            Assert.AreEqual(2, _views.Instances.OfType<IHandView>().Count());
            Info("Ok");
        }

        [ContextMenu("Check")]
        public void Check()
        {
            CheckModels();
            CheckAgents();
            CheckViews();
        }

        [ContextMenu("CheckModels")]
        public void CheckModels()
        {
            TestValidity("Models", _models.Instances);
        }

        [ContextMenu("CheckAgents")]
        public void CheckAgents()
        {
            TestValidity("Agents", _agents.Instances);
        }

        [ContextMenu("CheckViews")]
        public void CheckViews()
        {
            TestValidity("Views", _views.Instances);
        }

        /// <summary>
        /// Test that a collection of entities are valid.
        /// </summary>
        /// <param name="what">the name of the collection</param>
        /// <param name="entities">the set of entities to test</param>
        private void TestValidity(string what, IEnumerable<IEntity> entities)
        {
            var list = entities.ToList();
            Verbose(10, $"TestValidity: {what}, count={list.Count}");
            int n = 0;
            foreach (var entity in list)
            {
                // these are special cases
                if (entity is GameRoot)
                    continue;
                if (entity is BoardOverlayView)
                    continue;

                var valid = entity.IsValid;
                if (!valid)
                {
                    // test again so we can see what exactly is invalid in the debugger
                    Warn($"NotValid: {n}th {entity}: {entity.GetType()}");
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

                ++n;
            }
        }

        [ContextMenu("TraceBoard")]
        public void TraceBoard()
        {
            Info(BoardView.Print());
            Info(BoardAgent.Print());
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

        private ModelRegistry _models;
        private AgentRegistry _agents;
        private IViewRegistry _views;
    }
}
