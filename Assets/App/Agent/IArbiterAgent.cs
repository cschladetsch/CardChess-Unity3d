using App.Model;
using Flow;
using UniRx;

namespace App.Agent
{
    public interface IArbiterAgent
        : IAgent<IArbiterModel>
    {
        IReadOnlyReactiveProperty<IPlayerAgent> PlayerAgent { get; }
        IReadOnlyReactiveProperty<IPlayerModel> Player { get; }
        IBoardAgent BoardAgent { get; }

        void StartGame(IPlayerAgent p0, IPlayerAgent p1);
        ITransient NewGame();
        void Step();
    }
}
