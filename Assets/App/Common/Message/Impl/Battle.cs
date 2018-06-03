using App.Model;

namespace App.Common.Message
{
    public class Battle
        : RequestBase
    {
        public IPieceModel Attacker;
        public IPieceModel Defender;

        public Battle(IPlayerModel player, IPieceModel attacker, IPieceModel defender)
            : base(player, EActionType.Battle)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public override string ToString()
        {
            return $"{Attacker} attacks {Defender}";
        }
    }
}
