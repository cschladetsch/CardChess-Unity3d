using App.Agent;

namespace App.Action
{
    /// <inheritdoc />
    /// <summary>
    /// Play a card from a Player's Hand onto the Board
    /// </summary>
    public class PlayCard : ActionBase
    {
        public ICardInstance Card;
        public Coord Coord;

        public PlayCard()
        {
        }

        public PlayCard(ICardInstance card, Coord coord)
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
