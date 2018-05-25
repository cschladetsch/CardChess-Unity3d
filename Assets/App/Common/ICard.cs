using System.Collections.Generic;

namespace App.Common
{
    /// <summary>
    /// A card in a Hand or Deck or Collection for both Agents and Models
    /// </summary>
    public interface ICard :
        IHasId,
        IOwned,
        IHasName
    {
        string Description { get; }
        int ManaCost { get; }
        int Attack { get; }
        int Health { get; }
        IEnumerable<IEffect> Effects { get; }
        ECardType Type { get; }
        EPieceType PieceType { get; }
    }
}
