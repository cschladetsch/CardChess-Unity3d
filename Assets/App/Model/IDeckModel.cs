using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// The pool of cards that the player can draw from
    /// during a game.
    /// </summary>
    public interface IDeckModel
        : ICardCollection<ICardModel>
        , IModel
    {
        ICardModel Draw();
        IEnumerable<ICardModel> Draw(int count);
    }
}
