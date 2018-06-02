using System.Linq;
using App.Common;

namespace App.Mock.Model
{
    using App.Model;

    public class MockHand
        : HandModel
    {
        public MockHand(IPlayerModel owner)
            : base(owner)
        {
        }

        protected override void DrawInitialCards(int count)
        {
            foreach (var ty in _pieceTypes)
            {
                var card = Deck.Cards.FirstOrDefault(c => c.PieceType == ty);
                Assert.IsNotNull(card);
                Add(card);
                Deck.Remove(card);
            }
        }

        private readonly EPieceType[] _pieceTypes =
        {
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Archer,
            EPieceType.Archer,
            EPieceType.Gryphon,
            EPieceType.Gryphon,
        };
    }
}
