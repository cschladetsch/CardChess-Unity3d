using System;
using System.Collections;
using Flow;

namespace App.Agent
{
    using Common;

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
