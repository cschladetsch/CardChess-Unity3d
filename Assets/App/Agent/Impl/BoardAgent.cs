using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Agent
{
    using Common;

    public class BoardAgent
        : AgentBaseCoro<Model.IBoardModel>
        , IBoardAgent
    {
        public BoardAgent(IBoardModel model)
            : base(model)
        {
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }

        public ITransient NewGame()
        {
            Model.NewGame();
            _idToPiece.Clear();
            return null;
        }

        public IFuture<IPieceAgent> At(Coord coord)
        {
            return New.Future(GetAgent(Model.At(coord)));
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
    }
}
