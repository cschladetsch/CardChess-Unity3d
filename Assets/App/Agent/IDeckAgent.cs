namespace App.Agent
{
    using System;
    using UniRx;
    using Flow;
    using Dekuple.Agent;
    
    /// <inheritdoc />
    /// <summary>
    /// Controller for a Deck model.
    /// </summary>
    public interface IDeckAgent
        : IGameAgent<Model.IDeckModel>
    {
        event Action<ICardAgent> OnDraw;

        IReadOnlyReactiveCollection<ICardAgent> Cards { get; }

        IFuture<ICardAgent> Draw();
    }
}
