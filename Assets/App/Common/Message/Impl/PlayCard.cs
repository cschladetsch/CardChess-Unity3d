using UnityEngine.Assertions;

namespace App.Common.Message
{
    using Model;
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// Play a card from a PlayerAgent's Hand onto the Board
    /// </summary>
    public class PlayCard
        : RequestBase
    {
        public ICardModel Card { get; }
        public Coord Coord { get; }

        public PlayCard()
        {
        }

        public PlayCard(IPlayerModel player, ICardModel card, Coord coord)
            : base(player, EActionType.PlayCard)
        {
            Assert.IsNotNull(card);
            Card = card;
            Coord = coord;
        }

        public override string ToString()
        {
            return $"{Player} plays {Card.PieceType} @{Coord}";
        }
    }
}
