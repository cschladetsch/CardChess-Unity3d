namespace App.Agent
{
    using Dekuple;
    using Flow;
    using UniRx;
    using Model;

    public interface IArbiterAgent
        : IGameAgent<IArbiterModel>
    {
        IReadOnlyReactiveProperty<IResponse> LastResponse { get; }
        IReadOnlyReactiveProperty<IPlayerAgent> CurrentPlayerAgent { get; }
        IBoardAgent BoardAgent { get; }
        IPlayerAgent WhitePlayerAgent { get; }
        IPlayerAgent BlackPlayerAgent { get; }

        ITransient PrepareGame(IPlayerAgent white, IPlayerAgent black);
        void Step();
    }
}
