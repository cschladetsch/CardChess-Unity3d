using App.Common.Message;
using App.Model;

namespace App.Common.Message
{
    public class Battle
        : RequestBase
    {
        public ICardModel Attacker;
        public ICardModel Defender;

        public Battle(IPlayerModel player, ICardModel attacker, ICardModel defender)
            : base(player, EActionType.Battle)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public override string ToString()
        {
            return $"{Player} {Attacker} attacks {Defender}";
        }
    }
}
