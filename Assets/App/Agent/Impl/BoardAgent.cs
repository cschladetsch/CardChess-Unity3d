using System;
using System.Collections.Generic;
using App.Common.Message;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common;
    using Model;

    public class BoardAgent
        : AgentBaseCoro<IBoardModel>
        , IBoardAgent
    {
        public IReadOnlyReactiveProperty<int> Width => _width;
        public IReadOnlyReactiveProperty<int> Height => _height;

        public BoardAgent(IBoardModel model)
            : base(model)
        {
        }

        public override void StartGame()
        {
            base.StartGame();
            Model.StartGame();
            _idToPiece.Clear();
        }

        public override void EndGame()
        {
            Info($"BoardAgent EndGame");
            _idToPiece.Clear();
        }

        public IResponse Remove(IPieceAgent agent)
        {
            Assert.IsTrue(_idToPiece.ContainsKey(agent.Id));
            _idToPiece.Remove(agent.Id);
            return Model.Remove(agent.Model);
        }

        public IResponse Add(IPieceAgent agent)
        {
            Assert.IsFalse(_idToPiece.ContainsKey(agent.Id));
            _idToPiece[agent.Id] = agent;
            return Model.Add(agent.Model);
        }

        public IPieceAgent At(Coord coord)
        {
            return GetAgent(Model.At(coord));
        }

        public ITransient PerformNewGame()
        {
            return null;
        }

        private IPieceAgent GetAgent(IPieceModel model)
        {
            if (model == null)
                return null;
            IPieceAgent piece;
            if (_idToPiece.TryGetValue(model.Id, out piece))
                return piece;
            return _idToPiece[model.Id] = Registry.New<IPieceAgent>(model);
        }

        private readonly Dictionary<Guid, IPieceAgent> _idToPiece = new Dictionary<Guid, IPieceAgent>();
        private readonly IntReactiveProperty _width = new IntReactiveProperty(8);
        private readonly IntReactiveProperty _height = new IntReactiveProperty(8);
    }
}
