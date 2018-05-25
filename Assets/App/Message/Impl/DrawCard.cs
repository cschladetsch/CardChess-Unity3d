using App.Common;
using App.Model.Card;

namespace App.Action
{
    using Model;

    public class DrawCard : RequestBase
    {
        public DrawCard(IPlayerModel player)
            : base(player, EActionType.DrawCard)
        {
        }
    }
}
