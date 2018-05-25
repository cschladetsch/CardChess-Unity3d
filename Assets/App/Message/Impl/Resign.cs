using App.Model;

namespace App.Action
{
    public class Resign : RequestBase
    {
        public Resign(IPlayerModel player)
            : base(player, EActionType.Resign)
        {
        }
    }
}
