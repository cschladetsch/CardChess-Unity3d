namespace App.Agent
{
    using System;
    using UniRx;
    using Dekuple.Agent;
    using Common;
    using Model;

    /// <inheritdoc cref="AgentBaseCoro{TModel}" />
    ///  <summary>
    ///  Representative for a CardModel. Currently this is just a pass-through.
    ///  In future it will deal with networking, and also add extra animation and
    ///  bling to actions.
    ///  </summary>
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
        public IReadOnlyReactiveProperty<bool> Dead => Model.Dead;
        public IReadOnlyReactiveProperty<int> Health => Model.Health;
        public IReactiveCollection<IItemModel> Items => Model.Items;
        public IReactiveCollection<EAbility> Abilities => Model.Abilities;
        public IReactiveCollection<IEffectModel> Effects => Model.Effects;

        public CardAgent(ICardModel model)
            : base(model)
        {
        }

        public override string ToString()
        {
            try
            {
                return $"{Owner.Value}s CardAgent for {PieceType}";
            }
            catch (Exception e)
            {
                Error($"{e.Message} Printing {GetType()}");
                throw;
            }
        }
    }
}
