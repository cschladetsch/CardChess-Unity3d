using App.Common.Message;
using App.Model.Card;

namespace App.Common.Message
{
    using Model;

    public class CastSpell : RequestBase
    {
        public ISpellModel Spell;

        // can be one or the other
        public ICard Target;
        public Coord Coord;

        public CastSpell(IPlayerModel player, ISpellModel spell, ICardModel target)
            : base(player, EActionType.Resign)
        {
            Spell = spell;
            Target = target;
        }
        public CastSpell(IPlayerModel player, ISpellModel spell, Coord coord)
            : base(player, EActionType.Resign)
        {
            Spell = spell;
            Coord = coord;
        }
    }
}
