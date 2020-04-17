namespace App.Agent.Impl
{
    using Dekuple.Agent;
    using Model;

    /// <inheritdoc cref="IPopupAgent" />
    public class PopupAgent
        : AgentBase<IPopupModel>
        , IPopupAgent
    {
        public PopupAgent(IPopupModel model)
            : base(model)
        {
        }
    }
}
