using App.Model;

namespace App.Action
{
    public class Resign : ActionBase
    {
        public Resign(IPlayerModel player)
            : base(player, EActionType.Resign)
        {
        }
    }
}
