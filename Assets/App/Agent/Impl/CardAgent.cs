using System.Collections;
using System.Collections.Generic;
using App.Common.Message;
using App.Model;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common;

    public abstract class CardAgent :
        AgentBaseCoro<Model.ICardModel>,
        ICardAgent
    {
        #region Public Fields
        public int ManaCost => Model.ManaCost;
        public string Description => Model.Description;
        public ECardType Type => Model.Type;
        public EPieceType PieceType => Model.PieceType;
        public ICardTemplate Template => Model.Template;

        public ReactiveProperty<int> Power { get; }
        public ReactiveProperty<int> Health { get; private set; }
        public ReactiveProperty<IPlayerModel> Player { get; }
        public ReactiveCollection<IEnumerable<IEffect>> Effects { get; }
        public ReactiveCollection<IEnumerable<ItemModel>> Items { get; }
        public ReactiveProperty<IEnumerable<EAbility>> Abilities { get; }
        #endregion

        #region Public Methods

        public override bool Construct(ICardModel model)
        {
            base.Construct(model);
            Health = new ReactiveProperty<int>(model.Health);
            return true;
        }

        public ITransient TakeDamage(IPieceAgent self, IPieceAgent attacker)
        {
            var response = Model.TakeDamage(self.Model, attacker.Model);
            return null;
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
