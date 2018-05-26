using App.Model;

namespace App.Common.Message
{
    public class AcceptCards : RequestBase
    {
        public AcceptCards(IPlayerModel player)
            : base(player, EActionType.AcceptCards)
        {
        }
    }
}
