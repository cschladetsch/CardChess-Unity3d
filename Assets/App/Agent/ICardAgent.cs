
using System.Collections.Generic;
using App.Common;
using Flow;
using UniRx;

namespace App.Agent
{
	using Model;

    public interface ICardAgent
        : IAgent<ICardModel>
    {
        ICardTemplate Template { get; }
        IReactiveProperty<int> Health { get; }
        IReactiveProperty<int> Power { get; }
        IReactiveProperty<IPlayerModel> Player { get; }
        IReactiveCollection<IEnumerable<IEffectModel>> Effects { get; }
        IReactiveCollection<IEnumerable<ItemModel>> Items { get; }
        IReactiveProperty<IEnumerable<EAbility>> Abilities { get; }

        ITransient TakeDamage(IPieceAgent self, IPieceAgent attacker);
    }
}
