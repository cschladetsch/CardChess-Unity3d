using Dekuple.Model;
using UniRx;

namespace App.Model
{
    using Common;
    using Common.Message;

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

        void ChangeHealth(int change);
        void ChangeManaCost(int change);
        void ChangePower(int change);

        Response TakeDamage(ICardModel other);
    }
}
