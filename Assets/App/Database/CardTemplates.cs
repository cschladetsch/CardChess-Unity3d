using System;
using System.Collections.Generic;
using System.Linq;
using App.Model;
using Dekuple;
using Dekuple.Model;
using Dekuple.Registry;

namespace App.Database
{
    using Common;

    static class CardTemplates
    {
        private static readonly Dictionary<Guid, ICardTemplate> _templates = new Dictionary<Guid, ICardTemplate>();

        static CardTemplates()
        {
            AddBasicCards();
        }

        public static ICardModel NewCardModel(IRegistry<IModel> reg, string name, IOwner owner = null)
        {
            var tmpl = _templates.Values.FirstOrDefault(t => t.Name == name);
            return tmpl == null ? null : NewCardModel(reg, tmpl.Id, owner);
        }

        public static ICardModel NewCardModel(IRegistry<IModel> reg, Guid id, IOwner owner)
        {
            var tmp = _templates[id];
            var card = reg.Get<CardModel>(tmp, owner);
            return card;
        }

        public static ICardTemplate Get(Guid id)
        {
            return _templates.ContainsKey(id) ? _templates[id] : null;
        }

        public static IEnumerable<ICardTemplate> OfType(EPieceType type)
        {
            return _templates.Select(kv => kv.Value).Where(template => template.PieceType == type);
        }

		private static void AddBasicCards()
        {
            _templates.Clear();
            CardTemplate[] cards =
            {                                                                        //M, A, H
                new CardTemplate(ECardType.Piece, EPieceType.Peon, "Peon",           1, 1, 1),       // pawn
                new CardTemplate(ECardType.Piece, EPieceType.King, "King",           1, 1, 20),       // king
                new CardTemplate(ECardType.Piece, EPieceType.Queen, "Queen",         5, 5, 5),
                //new CardTemplate(ECardType.Piece, EPieceType.Paladin, "Paladin",     1, 1, 2),        // pawn after Barracks
                new CardTemplate(ECardType.Piece, EPieceType.Gryphon, "Gryphon",     2, 1, 2, new[] {EAbility.Mountable, EAbility.Lethal}),      // knight
                new CardTemplate(ECardType.Piece, EPieceType.Archer, "Archer",       3, 2, 3),        // bishop
                //new CardTemplate(ECardType.Piece, EPieceType.Castle, "Cannon",       4, 5, 6, new[] {EAbility.Guard}),
                //new CardTemplate(ECardType.Piece, EPieceType.Barricade, "Barricade", 2, 0, 3, new[] {EAbility.Guard, EAbility.Static}),
                //new CardTemplate(ECardType.Piece, EPieceType.Siege, "Siege",         3, 2, 1, new[] {EAbility.Static}),
                //new CardTemplate(ECardType.Piece, EPieceType.Dragon, "Dragon",       9, 8, 5, new[] {EAbility.Mountable}),
            };

            foreach (var card in cards)
            {
                _templates[card.Id] = card;
            }
        }

        public static ICardTemplate GetRandom()
        {
            ICardTemplate tmpl;
            do
            {
                tmpl = _templates[_templates.Keys.ElementAt(Math.RandomRanged(0, _templates.Count))];
            } while (tmpl.PieceType == EPieceType.King);
            return tmpl;
        }
    }
}
