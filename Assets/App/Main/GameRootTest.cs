using System.Collections;
using System.Collections.Generic;
using App.Agent;
using App.Model;
using UnityEngine;
using UnityEngine.UI;

using CoLib;
using Flow;

namespace App
{
    using Common;
    using Common.Message;

    public class GameRootTest
    {
        public Button Give;
        public Button Take;

        private App.Model.ModelRegistry _models;
        private App.Agent.AgentRegistry _agents;

        private Model.ICardModel _cardModel;
        private Agent.ICardAgent _cardAgent;
        private View.ICardAgentView _cardView;

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

