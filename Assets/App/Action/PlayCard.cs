namespace App.Action
{
    /// <summary>
    /// Play a card from a Player's Hand onto the Board
    /// </summary>
    public class PlayCard : ActionBase
    {
        public Agent.ICardInstance Card;
        public Coord Coord;
    }
}
