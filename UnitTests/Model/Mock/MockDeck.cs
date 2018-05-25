using System;

namespace App.Model.Test
{
    using Common;
    using Model;
    using Registry;

    public class MockDeck
        : DeckModel
    {
        [Inject] private Service.ICardTemplateService _cardTemplateService;

        public MockDeck(Guid a0, IOwner owner)
            : base(a0, owner)
        {
        }

        public override void NewGame()
        {
            foreach (var pt in pieceTypes)
            {
                Add(_cardTemplateService.NewCardModel(Player, pt));
            }
        }

        public override void Shuffle()
        {
        }

        public override int ShuffleIn(params ICardModel[] models)
        {
            int n = 0;
            foreach (var card in models)
            {
                AddToBottom(card);
                ++n;
            }
            return n;
        }

        private readonly EPieceType[] pieceTypes =
        {
            EPieceType.Peon,
            EPieceType.Peon,
            EPieceType.Peon,
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
