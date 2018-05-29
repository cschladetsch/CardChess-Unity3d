using System.Collections;
using App.Common.Message;
using App.Model;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common;

    public abstract class CardAgent
        : AgentBaseCoro<Model.ICardModel>
        , ICardAgent
    {
        #region Public Fields
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
        #endregion

        #region Public Methods

        public override bool Construct(ICardModel model)
        {
            base.Construct(model);
            return true;
        }

        public Response TakeDamage(ICardAgent other)
        {
            return Model.TakeDamage(other.Model);
        }

        #endregion

        #region Protected Methods
        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
        #endregion

        #region Private Fields
        #endregion
    }
}
