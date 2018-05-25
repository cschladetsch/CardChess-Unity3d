using App.Common;
using App.Model.Card;

namespace App.Action
{
    using Model;

    public class CastSpell : ActionBase
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
