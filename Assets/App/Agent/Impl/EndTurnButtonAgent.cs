namespace App.Agent.Impl
{
    using Model;

    /// <summary>
    /// Pass-through for agent; nothing to do between view and model at the moment.
    /// </summary>
    public class EndTurnButtonAgent
        : AgentBase<IEndTurnButtonModel>
        , IEndTurnButtonAgent
    {
        public EndTurnButtonAgent(IEndTurnButtonModel model)
            : base(model)
        {
        }
    }
}
