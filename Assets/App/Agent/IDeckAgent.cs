using System;
using Flow;
using UniRx;

namespace App.Agent
{
    /// <summary>
    /// Controller for a Deck model.
    /// </summary>
    public interface IDeckAgent
        : IAgent<Model.IDeckModel>
    {
        event Action<ICardAgent> OnDraw;

        IReadOnlyReactiveCollection<ICardAgent> Cards { get; }

        IFuture<ICardAgent> Draw();
        //void AddToBottom(ICardAgent card);
    }
}
