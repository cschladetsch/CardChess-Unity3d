namespace App.Agent
{
    using Model;

    public class PieceAgent
        : AgentBase<Model.IPieceModel>
        , IPieceAgent
    {
        public PieceAgent(IPieceModel model) : base(model)
        {
        }
    }
}
