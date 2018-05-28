
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
        ReactiveProperty<int> Health { get; }
        ReactiveProperty<int> Power { get; }
        ReactiveProperty<IPlayerModel> Player { get; }
        ReactiveCollection<IEnumerable<IEffect>> Effects { get; }
        ReactiveCollection<IEnumerable<ItemModel>> Items { get; }
        ReactiveProperty<IEnumerable<EAbility>> Abilities { get; }

        ITransient TakeDamage(IPieceAgent self, IPieceAgent attacker);
    }
}
