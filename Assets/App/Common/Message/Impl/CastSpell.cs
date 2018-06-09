using App.Agent;

namespace App.Common.Message
{
    using Model;

    public class CastSpell : RequestBase
    {
        public ISpellModel Spell;

        // can be one or the other
        public IPieceAgent Target;
        public Coord Coord;

        public CastSpell(IPlayerModel player, ISpellModel spell, IPieceAgent target)
            : base(player, EActionType.CastSpell)
        {
            Spell = spell;
            Target = target;
        }

        public CastSpell(IPlayerModel player, ISpellModel spell, Coord coord)
            : base(player, EActionType.CastSpell)
        {
            Spell = spell;
            Coord = coord;
        }
    }
}
