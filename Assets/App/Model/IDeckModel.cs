using System.Collections.Generic;
using Dekuple.Model;

namespace App.Model
{
    using Common;

    /// <inheritdoc cref="IModel" />
    /// <summary>
    /// The pool of cards that the player can draw from during a game.
    /// </summary>
    public interface IDeckModel
        : ICardCollection<ICardModel>
        , IModel
        , IGameActor
    {
        ICardModel Draw();
        IEnumerable<ICardModel> Draw(int count);
    }
}
