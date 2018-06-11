namespace App.Agent
{
    using Model;

    public class PieceAgent
        : AgentBase<IPieceModel>
        , IPieceAgent
    {
        public PieceAgent(IPieceModel model)
            : base(model)
        {
        }
    }
}
