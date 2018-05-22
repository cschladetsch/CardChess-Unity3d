using System.Collections.Generic;
using App.Common;

namespace App.Model
{
    public interface ICardModel :
        IModel,
        ICard
    {
        ICardTemplate Template { get; }
        IEnumerable<ICardModel> Items { get; }
        IEnumerable<EAbility> Abilities { get; }
        IPlayerModel Player { get; }

        Response ChangeHealth(int amount, ICardModel cause);
    }
}
