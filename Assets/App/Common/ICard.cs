using System.Collections.Generic;

namespace App.Common
{
    /// <summary>
    /// A card in a Hand or Deck or Collection for both Agents and Models
    /// </summary>
    public interface ICard : IHasId
    {
        string Name { get; }
        string Description { get; }
        int Attack { get; }
        int Health { get; }
        IList<IEffect> Effects { get; }
        ECardType Type { get; }

        bool SameOwner(IOwner other);
    }
}
