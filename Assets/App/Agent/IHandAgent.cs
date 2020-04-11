namespace App.Agent
{
    using UniRx;
    using Dekuple.Agent;

    /// <summary>
    /// Behaviour for a Player's Hand.
    /// </summary>
    public interface IHandAgent
        : IGameAgent<Model.IHandModel>
    {
        IReadOnlyReactiveCollection<ICardAgent> Cards { get; }
    }
}
