using System;
using Dekuple.Agent;
using Flow;
using UniRx;

namespace App.Agent
{
    /// <inheritdoc />
    /// <summary>
    /// Controller for a Deck model.
    /// </summary>
    public interface IDeckAgent
        : IAgent<Model.IDeckModel>
    {
        event Action<ICardAgent> OnDraw;

        IReadOnlyReactiveCollection<ICardAgent> Cards { get; }

        IFuture<ICardAgent> Draw();
    }
}
