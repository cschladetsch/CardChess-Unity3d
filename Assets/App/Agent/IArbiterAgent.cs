namespace App.Agent
{
    using Flow;
    using UniRx;
    using Model;

    /// <summary>
    /// Behaviour of the game Arbiter, which makes all decisions and applies the rules.
    /// </summary>
    public interface IArbiterAgent
        : IGameAgent<IArbiterModel>
    {
        IReadOnlyReactiveProperty<RequestResponse> LastResponse { get; }
        IReadOnlyReactiveProperty<string> Log { get; }
        IReadOnlyReactiveProperty<IPlayerAgent> CurrentPlayerAgent { get; }
        IBoardAgent BoardAgent { get; }
        IPlayerAgent WhitePlayerAgent { get; }
        IPlayerAgent BlackPlayerAgent { get; }

        ITransient PrepareGame(IPlayerAgent white, IPlayerAgent black);
        void Step();
    }
}
