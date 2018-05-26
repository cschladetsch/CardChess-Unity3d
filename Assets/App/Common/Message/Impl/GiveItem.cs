using App.Model.Card;

namespace App.Common.Message
{
    using Model;

    public class GiveItem : RequestBase
    {
        public IItemModel Item;
        public ICardModel Target;

        public GiveItem(IPlayerModel player, IItemModel item, ICardModel target)
            : base(player, EActionType.Resign)
        {
            Item = item;
            Target = target;
        }
    }
}
