using App.Common;
using App.Common.Message;
using Flow;
using UniRx;

namespace App.Agent
{
    using Model;

    public interface IArbiterAgent
        : IAgent<IArbiterModel>
    {
        IReadOnlyReactiveProperty<IResponse> LastResponse { get; }
        IReadOnlyReactiveProperty<IPlayerAgent> CurrentPlayerAgent { get; }
        IBoardAgent BoardAgent { get; }
        IPlayerAgent WhitePlayerAgent { get; }
        IPlayerAgent BlackPlayerAgent { get; }

        ITransient PrepareGame(IPlayerAgent p0, IPlayerAgent p1);
        void Step();
    }
}
