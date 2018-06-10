using System;
using Flow;

namespace App.Agent
{
    using Common.Message;

    /// <summary>
    /// Controller for a Deck model.
    /// </summary>
    public interface IDeckAgent
        : IAgent<Model.IDeckModel>
    {
        event Action<ICardAgent> OnDraw;

        IFuture<ICardAgent> Draw();
        void AddToBottom(ICardAgent card);
    }
}
