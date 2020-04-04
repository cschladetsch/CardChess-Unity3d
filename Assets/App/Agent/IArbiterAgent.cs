namespace App.Agent
{
    using UniRx;
    using Flow;
    using Dekuple;
    using Dekuple.Agent;
    using Model;

    /// <summary>
    /// Behaviour of the game Arbiter, which makes all decisions and applies the rules.
    /// </summary>
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
