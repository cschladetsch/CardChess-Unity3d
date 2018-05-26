using App.Common.Message;
using App.Model;

namespace App.Common.Message
{
    public class Battle
        : RequestBase
    {
        public Coord Attacker;
        public Coord Defender;
        public Battle(IPlayerModel player, Coord attacker, Coord defender)
            : base(player, EActionType.Battle)
        {
            Attacker = attacker;
            Defender = defender;
        }
    }
}
