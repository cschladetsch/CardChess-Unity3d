using System.Collections.Generic;

namespace App.Model
{
    using Common;

    public interface ICardTemplate :
        IHasId,
        IHasName,
        IOwned
    {
        ECardType Type { get; }
        string FlavourText { get; }
        int ManaCost { get; }
        int Attack { get; }
        int Health { get; }
        IEnumerable<EAbility> Abilities { get; }
    }
}
