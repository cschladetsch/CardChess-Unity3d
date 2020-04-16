using UnityEngine;

namespace App.Model
{
    using UniRx;
    using Dekuple.Model;
    using Common;
    using Common.Message;

    /// <inheritdoc />
    /// <summary>
    /// A Card in a Deck, Hand, Graveyard or on a Board
    /// </summary>
    public interface ICardModel
        : IModel
    {
        ICardTemplate Template { get; }
        ECardType Type { get; }
        EPieceType PieceType { get; }

        IReactiveProperty<IPlayerModel> Player { get; }
        IReadOnlyReactiveProperty<int> ManaCost { get; }
        IReadOnlyReactiveProperty<int> Power { get; }
        IReadOnlyReactiveProperty<int> Health { get; }
        IReactiveCollection<IItemModel> Items { get; }
        IReactiveCollection<EAbility> Abilities { get; }
        IReactiveCollection<IEffectModel> Effects { get; }
        IReadOnlyReactiveProperty<bool> Dead { get; }

        EColor Color { get; }

        void ChangeHealth(int change);
        void ChangeManaCost(int change);
        void ChangePower(int change);

        Response TakeDamage(ICardModel other);
    }
}
