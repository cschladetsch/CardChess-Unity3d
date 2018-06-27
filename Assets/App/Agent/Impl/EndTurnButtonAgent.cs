namespace App.Agent.Impl
{
    using Model;

    public class EndTurnButtonAgent
        : AgentBase<IEndTurnButtonModel>
        , IEndTurnButtonAgent
    {
        public EndTurnButtonAgent(IEndTurnButtonModel model) : base(model)
        {
        }
    }
}
