using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// An immutable card template. Used to make other cards at runtime.
    /// </summary>
    public interface ICardTemplate
        : IModel
        , ICard
    {
        string FlavourText { get; }
        IEnumerable<ICardModel> Items { get; }
        IEnumerable<EAbility> Abilities { get; }

        // make a new card from this template
        ICardModel New();
    }
}
