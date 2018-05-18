using UnityEngine.Assertions;

namespace App.Action
{
    using Model;
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// Play a card from a PlayerAgent's Hand onto the Board
    /// </summary>
    public class PlayCard : ActionBase
    {
        public ICardModel Card { get; }
        public Coord Coord { get; }

        public PlayCard()
        {
        }

        public PlayCard(ICardModel card, Coord coord)
        {
            Assert.IsNotNull(card);
            Card = card;
            Coord = coord;
        }

        public override string ToString()
        {
            return $"Play {Card.Type} @{Coord}";
        }
    }
}
