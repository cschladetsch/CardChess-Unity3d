using System.Collections.Generic;

namespace App.Model
{
    public interface ICardTemplate : IHasId
    {
        ECardType Type { get; }
        string Name { get; }
        string FlavourText { get; }
        int ManaCost { get; }
        int Attack { get; }
        int Health { get; }
        IList<EAbility> Abilities { get; }
    }
}
