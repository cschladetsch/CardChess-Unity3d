using App.Agent;

namespace App.Action
{
    /// <inheritdoc />
    /// <summary>
    /// Play a card from a Player's Hand onto the Board
    /// </summary>
    public class PlayCard : ActionBase
    {
        public ICard Card;
        public Coord Coord;

        public PlayCard()
        {
        }

        public PlayCard(ICard card, Coord coord)
        {
            // TODO Assert.IsNotNull(card);
            Card = card;
            Coord = coord;
        }

        public override string ToString()
        {
            return $"PlayCard {Card} to {Coord}";
        }
    }
}
