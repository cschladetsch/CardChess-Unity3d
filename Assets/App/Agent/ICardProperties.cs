using UniRx;

namespace App.Agent
{
    using Common;
    using Model;

    public interface ICardProperties
    {
        ICardTemplate Template { get; }
        IReadOnlyReactiveProperty<int> Health { get; }
        IReadOnlyReactiveProperty<int> Power { get; }
        IReadOnlyReactiveProperty<bool> Dead { get; }
        IReactiveCollection<IEffectModel> Effects { get; }
        IReactiveCollection<IItemModel> Items { get; }
        IReactiveCollection<EAbility> Abilities { get; }
    }
}
