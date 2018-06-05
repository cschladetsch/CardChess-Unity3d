using App.Model;

namespace App.Agent
{
    public class PieceAgent
        : AgentBase<Model.IPieceModel>
        , IPieceAgent
    {
        public PieceAgent(IPieceModel model) : base(model)
        {
        }
    }
}
