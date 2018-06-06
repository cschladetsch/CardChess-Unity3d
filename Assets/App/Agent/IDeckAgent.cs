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
        IFuture<Response> NewGame();
        IFuture<ICardAgent> Draw();
    }
}
