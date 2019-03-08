using System.Collections.Generic;
using Dekuple.Model;

namespace App.Model
{
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// An immutable card template. Used to make other cards at runtime.
    /// </summary>
    public interface ICardTemplate
        : IModel
    {
        ECardType Type { get; }
        EPieceType PieceType { get; }

        int ManaCost { get; }
        int Power { get; }
        int Health { get; }

        string Name { get; }
        string FlavourText { get; }
        IEnumerable<IItemModel> Items { get; }
        IEnumerable<EAbility> Abilities { get; }
        IEnumerable<IEffectModel> Effects { get; }

        // make a new card from this template
        ICardModel New(IPlayerModel player);
    }
}
