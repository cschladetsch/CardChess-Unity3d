using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Agent
{
    using Common;
    using Model;

    public class BoardAgent :
        AgentBaseCoro<Model.IBoardModel>,
        IBoardAgent
    {
        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }

        public void NewGame()
        {
            Model.NewGame();
        }

        public IPieceAgent At(Coord coord)
        {
            throw new NotImplementedException();
        }
    }
}
