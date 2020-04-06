namespace App.View
{
    using Agent;
    using Dekuple.View;

    /// <summary>
    /// View of an end-turn button
    /// </summary>
    public interface IEndTurnButtonView
        : IView<IEndTurnButtonAgent>
    {
        // void SetAgent(IViewBase player, IEndTurnButtonAgent agent);
    }
}
