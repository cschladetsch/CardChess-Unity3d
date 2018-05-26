using System.Collections.Generic;

namespace App.Model
{
    using Common;
    using Common.Message;

    /// <summary>
    /// A Card in a Deck, Hand, Graveyard or on a Board
    /// </summary>
    public interface ICardModel
        : IModel
        , ICard
    {
        ICardTemplate Template { get; }
        IEnumerable<ICardModel> Items { get; }
        IEnumerable<EAbility> Abilities { get; }
        IPlayerModel Player { get; }

        Response TakeDamage(IPieceModel self, IPieceModel attacker);
    }
}
