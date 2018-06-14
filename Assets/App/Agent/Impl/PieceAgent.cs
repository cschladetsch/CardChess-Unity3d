using App.Common;
using UniRx;

namespace App.Agent
{
    using Model;

    public class PieceAgent
        : AgentBase<IPieceModel>
        , IPieceAgent
    {
        public IReactiveProperty<Coord> Coord => Model.Coord;
        public ECardType Type { get; }
        public EPieceType PieceType => Model.PieceType;
        public ICardTemplate Template { get; }
        public IReactiveProperty<IPlayerModel> Player { get; }
        public IReadOnlyReactiveProperty<int> ManaCost => Model.Power;
        public IReadOnlyReactiveProperty<int> Power => Model.Power;
        public IReadOnlyReactiveProperty<int> Health => Model.Health;
        public IReactiveCollection<IItemModel> Items { get; }
        public IReactiveCollection<EAbility> Abilities { get; }
        public IReactiveCollection<IEffectModel> Effects { get; }

        public PieceAgent(IPieceModel model)
            : base(model)
        {
        }
    }
}
