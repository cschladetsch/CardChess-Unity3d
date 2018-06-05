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
        IPlayerAgent WhitePlayer { get; }
        IPlayerAgent BlackPlayer { get; }

        ITransient NewGame(IPlayerAgent p0, IPlayerAgent p1);
        void StartGame();
        void Step();
    }
}
