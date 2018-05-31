using App.Model;

namespace App.Agent
{
    class PieceAgent
        : AgentBase<Model.IPieceModel>
        , IPieceAgent
    {
        public PieceAgent(PieceModel model) : base(model)
        {
        }
    }
}
