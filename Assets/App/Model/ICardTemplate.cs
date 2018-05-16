using System.Collections.Generic;

namespace App.Model
{
    using Common;

    public interface ICardTemplate : IHasId, IHasName
    {
        ECardType Type { get; }
        string FlavourText { get; }
        int ManaCost { get; }
        int Attack { get; }
        int Health { get; }
        IList<EAbility> Abilities { get; }
    }
}
