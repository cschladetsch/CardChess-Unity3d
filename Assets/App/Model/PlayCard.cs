namespace App.Model
{
    /// <summary>
    /// Play a card from a Player's Hand onto the Board
    /// </summary>
    public class PlayCard : ActionBase
    {
        public ICard Card;
        public Coord Coord;
    }
}
