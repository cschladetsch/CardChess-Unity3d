using App.Common;

namespace App.Model
{
    /// <summary>
    /// A spell that can be cast by a player on a coordinate, a piece, an area, or the other player.
    /// </summary>
    public class Spell
        : ModelBase
    {
        public Spell(IOwner owner)
            : base(owner)
        {
        }
    }
}
