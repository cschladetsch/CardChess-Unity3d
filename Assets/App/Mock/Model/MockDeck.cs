using System;

#pragma warning disable 649

namespace App.Mock.Model
{
    using Common;
    using Registry;
    using App.Model;

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
            foreach (var pt in _pieceTypes)
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

        private readonly EPieceType[] _pieceTypes =
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
