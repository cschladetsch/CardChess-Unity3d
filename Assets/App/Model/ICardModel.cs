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
        IReactiveProperty<int> ManaCost { get; }
        IReactiveProperty<int> Power { get; }
        IReactiveProperty<int> Health { get; }
        IReactiveCollection<IItemModel> Items { get; }
        IReactiveCollection<EAbility> Abilities { get; }
        IReactiveCollection<IEffectModel> Effects { get; }

        //Response TakeDamage(IPieceModel self, IPieceModel attacker);
    }
}
