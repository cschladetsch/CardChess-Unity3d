using System;
using System.Collections.Generic;
using System.Linq;
using App.Model;

namespace App.Database
{
    using Common;

    static class CardTemplates
    {
        static CardTemplates()
        {
            MockConstruct();
        }

        public static ICardModel New(string name, IOwner owner = null)
        {
            var tmpl = _templates.Values.FirstOrDefault(t => t.Name == name);
            return tmpl == null ? null : New(tmpl.Id, owner);
        }

        public static ICardModel New(Guid id, IOwner owner)
        {
            var tmp = _templates[id];
            var card = new CardModel(tmp, owner);
            return card;
        }

        public static ICardTemplate Get(Guid id)
        {
            return _templates.ContainsKey(id) ? _templates[id] : null;
        }

        public static IEnumerable<ICardTemplate> OfType(ECardType type)
        {
            return _templates.Select(kv => kv.Value).Where(template => template.Type == type);
        }

        private static void MockConstruct()
        {
            _templates.Clear();
            var X = int.MaxValue;
            CardTemplate[] cards =
            {                                                                        //M, A, H
                new CardTemplate(ECardType.Piece, EPieceType.King, "King",           X, 1, 20),       // king
                new CardTemplate(ECardType.Piece, EPieceType.Queen, "Queen",         6, 6, 7),
                new CardTemplate(ECardType.Piece, EPieceType.Paladin, "Paladin",     1, 1, 2),        // pawn
                new CardTemplate(ECardType.Piece, EPieceType.Gryphon, "Gryphon",     2, 1, 2, new[] {EAbility.Mountable, EAbility.Lethal}),      // knight
                new CardTemplate(ECardType.Piece, EPieceType.Archer, "Archer",       3, 2, 3),        // bishop
                new CardTemplate(ECardType.Piece, EPieceType.Castle, "Cannon",       4, 5, 6, new[] {EAbility.Guard}),
                new CardTemplate(ECardType.Piece, EPieceType.Barricade, "Barricade", 2, 0, 3, new[] {EAbility.Guard, EAbility.Static}),
                new CardTemplate(ECardType.Piece, EPieceType.Siege, "Siege",         3, 2, 1, new[] {EAbility.Static}),
                new CardTemplate(ECardType.Piece, EPieceType.Dragon, "Dragon",       9, 8, 5, new[] {EAbility.Mountable}),
            };

            foreach (var card in cards)
            {
                _templates[card.Id] = card;
            }
        }

        public static ICardTemplate GetRandom()
        {
            var numCards = _templates.Count;
            var r = new System.Random();
            var rand = r.Next(0, numCards - 1);
            var key = _templates.Keys.ElementAt(rand);
            return _templates[key];
        }

        private static readonly Dictionary<Guid, ICardTemplate> _templates = new Dictionary<Guid, ICardTemplate>();
    }
}
