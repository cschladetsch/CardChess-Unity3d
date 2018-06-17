using App.Common;
using App.Model;
using UniRx;

namespace App.Agent
{
    public interface ICardProperties
    {
        ICardTemplate Template { get; }
        IReadOnlyReactiveProperty<int> Health { get; }
        IReadOnlyReactiveProperty<int> Power { get; }
        IReadOnlyReactiveProperty<bool> Dead { get; }
        IReactiveProperty<IPlayerModel> Player { get; }
        IReactiveCollection<IEffectModel> Effects { get; }
        IReactiveCollection<IItemModel> Items { get; }
        IReactiveCollection<EAbility> Abilities { get; }
    }
}
