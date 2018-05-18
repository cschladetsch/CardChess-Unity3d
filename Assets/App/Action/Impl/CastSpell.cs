using App.Common;
using App.Model.Card;

namespace App.Action
{
    public class CastSpell : ActionBase
    {
        public ISpellModel Spell;
        public ICard Target;
    }
}