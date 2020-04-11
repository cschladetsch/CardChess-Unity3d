namespace App.Agent
{
<<<<<<< HEAD
    using Dekuple;
    using Flow;
    using UniRx;
=======
    using UniRx;
    using Flow;
    using Dekuple;
    using Dekuple.Agent;
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
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
