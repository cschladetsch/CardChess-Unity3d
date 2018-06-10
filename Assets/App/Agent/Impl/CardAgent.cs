using System;
using System.Collections;

using Flow;
using UniRx;

namespace App.Agent
{
    using Common;
    using Model;

    public class CardAgent
        : AgentBaseCoro<Model.ICardModel>
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

        public CardAgent(Model.ICardModel model)
            : base(model)
        {
        }

        //protected override IEnumerator Next(IGenerator self)
        //{
        //    yield break;
        //}

        public bool SetModel(Model.ICardModel model)
        {
            Assert.IsNotNull(model);
            return true;
        }

        public override string ToString()
        {
            try
            {
                return $"{Owner.Value}s {PieceType} CardAgent, Id={Id}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e} Printing {GetType()}");
                throw;
            }
        }
    }
}
