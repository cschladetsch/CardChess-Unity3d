using App.Common;
using App.Model;

namespace App.Action
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
