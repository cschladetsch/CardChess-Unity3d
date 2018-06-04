using App.Common;
using App.Common.Message;
using UniRx;

namespace App.Agent
{
    using Model;

    public interface ICardAgent
        : IAgent<ICardModel>
    {
        ICardTemplate Template { get; }

        IReadOnlyReactiveProperty<int> Health { get; }
        IReadOnlyReactiveProperty<int> Power { get; }
        IReactiveProperty<IPlayerModel> Player { get; }
        IReactiveCollection<IEffectModel> Effects { get; }
        IReactiveCollection<IItemModel> Items { get; }
        IReactiveCollection<EAbility> Abilities { get; }
    }
}
