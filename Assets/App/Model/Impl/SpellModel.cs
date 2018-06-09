namespace App.Model
{
    using Common;
    using Common.Message;

    /// <summary>
    /// A spell that can be cast by a player on a coordinate, a piece, an area, or the other player.
    /// </summary>
    public class SpellModel
        : CardModel
        , ISpellModel
    {
        public SpellModel(IOwner owner, ICardTemplate template)
            : base(owner, template)
        {
        }

        public Response Cast(Coord coord)
        {
            throw new System.NotImplementedException();
        }

        public Response Cast()
        {
            throw new System.NotImplementedException();
        }
    }
}
