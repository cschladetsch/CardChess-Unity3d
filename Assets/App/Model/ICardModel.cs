using System.Collections.Generic;

namespace App.Model
{
    using Common;
    using Common.Message;

    public interface ICardModel
        : IModel
        , ICard
    {
        ICardTemplate Template { get; }
        IEnumerable<ICardModel> Items { get; }
        IEnumerable<EAbility> Abilities { get; }
        IPlayerModel Player { get; }

        Response ChangeHealth(int amount, ICardModel cause);
    }
}
