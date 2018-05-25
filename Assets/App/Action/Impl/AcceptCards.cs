using App.Model;

namespace App.Action
{
    public class AcceptCards : RequestBase
    {
        public AcceptCards(IPlayerModel player)
            : base(player, EActionType.AcceptCards)
        {
        }
    }
}
