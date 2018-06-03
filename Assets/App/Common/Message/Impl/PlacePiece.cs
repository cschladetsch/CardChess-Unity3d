namespace App.Common.Message
{
    using Model;
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// Play a card from a PlayerAgent's Hand onto the Board
    /// </summary>
    public class PlacePiece
        : RequestBase
    {
        public ICardModel Card { get; }
        public Coord Coord { get; }

        public PlacePiece()
        {
        }

        public PlacePiece(IPlayerModel player, ICardModel card, Coord coord)
            : base(player, EActionType.PlacePiece)
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
