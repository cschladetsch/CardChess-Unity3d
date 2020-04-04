using UnityEngine;

namespace App.Model
{
    using System.Collections.Generic;
    using Dekuple.Model;
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// An immutable card template. Used to make other cards at runtime.
    /// </summary>
    public interface ICardTemplate
        : IModel
    {
        string Name { get; }
        string FlavourText { get; }
        int ManaCost { get; }
        int Power { get; }
        int Health { get; }
        ECardType Type { get; }
        EPieceType PieceType { get; }
        GameObject MeshPrefab { get; }

        IEnumerable<IItemModel> Items { get; }
        IEnumerable<EAbility> Abilities { get; }
        IEnumerable<IEffectModel> Effects { get; }
    }
}
