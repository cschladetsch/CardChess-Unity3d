using App.Model;

namespace App.Action
{
    public class AcceptCards : ActionBase
    {
        public AcceptCards(IPlayerModel player)
            : base(player, EActionType.AcceptCards)
        {
        }
    }
}
