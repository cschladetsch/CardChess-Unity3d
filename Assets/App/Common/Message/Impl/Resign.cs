using App.Model;

namespace App.Common.Message
{
    public class Resign : RequestBase
    {
        public Resign(IPlayerModel player)
            : base(player, EActionType.Resign)
        {
        }

        public override string ToString()
        {
            return $"{Player} resigns";
        }
    }
}
