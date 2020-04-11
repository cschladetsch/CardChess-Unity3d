#pragma warning disable 649

namespace App.Mock.Model
{
    using Dekuple;
    using Common;
    using App.Model;

    /// <summary>
    /// A mock deck for testing.
    /// </summary>
    public class MockDeck
        : DeckModel
    {
        [Inject] public Service.ICardTemplateService CardTemplateService;

        public MockDeck(ITemplateDeck templateDeck, IPlayerModel owner)
            : base(templateDeck, owner)
        {
        }

        public override void StartGame()
        {
            Clear();
            foreach (var pt in _pieceTypes)
            {
                Add(CardTemplateService.NewCardModel(Player, pt));
            }
        }

        public override void Shuffle()
        {
            // intentionally do not shuffle
        }

        private readonly EPieceType[] _pieceTypes =
        {
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Archer,
            EPieceType.Archer,
            EPieceType.Archer,
            EPieceType.Archer,
            EPieceType.Archer,
            EPieceType.Archer,
            EPieceType.Gryphon,
            EPieceType.Gryphon,
            EPieceType.Gryphon,
            EPieceType.Gryphon,
            EPieceType.Gryphon,
            EPieceType.Queen,
            EPieceType.Queen,
            EPieceType.Queen,
            EPieceType.Queen,
            EPieceType.Queen,
        };
    }
}
