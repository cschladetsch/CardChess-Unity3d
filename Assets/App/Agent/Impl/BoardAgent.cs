using System;
using System.Collections.Generic;
using System.Text;
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

        public string Print()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"BoardAgent: {_modelIdToPiece.Count} pieces:");
            foreach (var kv in _modelIdToPiece)
            {
                sb.AppendLine($"\t{kv.Value.Model}");
            }
            return sb.ToString();
        }
        public override void StartGame()
        {
            base.StartGame();
            Model.StartGame();
            _modelIdToPiece.Clear();
        }

        public override void EndGame()
        {
            Info($"BoardAgent EndGame");
            _modelIdToPiece.Clear();
        }

        public IResponse Remove(IPieceAgent agent)
        {
            Assert.IsTrue(_modelIdToPiece.ContainsKey(agent.Model.Id));
            Assert.IsTrue(Model.Remove(agent.Model).Success);
            Model.Remove(agent.Model);
            _modelIdToPiece.Remove(agent.Model.Id);
            return Response.Ok;
        }

        public IResponse Add(IPieceAgent agent)
        {
            Assert.IsFalse(_modelIdToPiece.ContainsKey(agent.Model.Id));
            _modelIdToPiece[agent.Model.Id] = agent;
            return Response.Ok;
        }

        public IPieceAgent At(Coord coord)
        {
            var model = Model.At(coord);
            if (model == null)
                return null;
            Assert.AreEqual(coord, model.Coord.Value);
            var agent = GetAgent(model);
            Assert.AreSame(agent.Model, model);
            Assert.AreEqual(agent.Coord.Value, coord);
            Assert.AreEqual(agent.Model.Coord.Value, coord);
            return agent;
        }

        public ITransient PerformNewGame()
        {
            return null;
        }

        private IPieceAgent GetAgent(IPieceModel model)
        {
            Assert.IsNotNull(model);
            IPieceAgent piece;
            if (_modelIdToPiece.TryGetValue(model.Id, out piece))
                return piece;
            throw new Exception();
        }

        private readonly Dictionary<Guid, IPieceAgent> _modelIdToPiece = new Dictionary<Guid, IPieceAgent>();
        private readonly IntReactiveProperty _width = new IntReactiveProperty(8);
        private readonly IntReactiveProperty _height = new IntReactiveProperty(8);
    }
}
