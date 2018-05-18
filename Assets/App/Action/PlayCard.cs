using App.Agent;
using UnityEngine.Assertions;

namespace App.Action
{
    using Model;

    /// <inheritdoc />
    /// <summary>
    /// Play a card from a PlayerAgent's HandModel onto the BoardAgent
    /// </summary>
    public class PlayCard : ActionBase
    {
        public ICardModel Card;
        public Coord Coord;

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
