using App.Agent;
using Dekuple.View;

namespace App.View
{
    public interface IEndTurnButtonView
        : IView<IEndTurnButtonAgent>
    {
        void SetAgent(IPlayerView player, IEndTurnButtonAgent agent);
    }
}
