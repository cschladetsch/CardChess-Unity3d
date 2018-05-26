using App.Common.Message;
using App.Model.Card;

namespace App.Common.Message
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
