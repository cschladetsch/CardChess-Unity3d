using System.Collections;
using App.Model;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common;

    public class CardAgent
        : AgentBaseCoro<ICardModel>
        , ICardAgent
    {
        public ECardType Type => Model.Type;
        public EPieceType PieceType => Model.PieceType;
        public ICardTemplate Template => Model.Template;
        public IReactiveProperty<IPlayerModel> Player => Model.Player;
        public IReadOnlyReactiveProperty<int> ManaCost => Model.Power;
        public IReadOnlyReactiveProperty<int> Power => Model.Power;
        public IReadOnlyReactiveProperty<int> Health => Model.Health;
        public IReactiveCollection<IItemModel> Items => Model.Items;
        public IReactiveCollection<EAbility> Abilities => Model.Abilities;
        public IReactiveCollection<IEffectModel> Effects => Model.Effects;

        public CardAgent(ICardModel model)
            : base(model)
        {
        }

        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
    }
}
