using System.Collections;
using Flow;

namespace App.Agent
{
    public class Board : AgentBaseCoro<Model.IBoard>, IBoard
    {
        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }

        public IFuture<bool> NewGame()
        {
            return null;
        }
    }
}
