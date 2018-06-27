using App.Agent;

namespace App.View
{
    public interface IEndTurnButtonView
        : IView<IEndTurnButtonAgent>
    {
        void SetAgent(IPlayerView player, IEndTurnButtonAgent agent);
    }
}
