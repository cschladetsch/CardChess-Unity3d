using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// An immutable card template. Used to make other cards at runtime.
    /// </summary>
    public interface ICardTemplate
        : IModel
    {
        ECardType Type { get; }
        int ManaCost { get; }
        int Attack { get; }
        int Health { get; }
        string FlavourText { get; }
        string Description { get; }
        IEnumerable<ICardModel> Items { get; }
        IEnumerable<IEffect> Effects { get; }
        IEnumerable<EAbility> Abilities { get; }

        // make a new card from this template
        ICardModel New();
    }
}
