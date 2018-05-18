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

        public static Model.ICardModel New(string name, IOwner owner = null)
        {
            var tmpl = _templates.Values.FirstOrDefault(t => t.Name == name);
            return tmpl == null ? null : New(tmpl.Id, owner);
        }

        public static Model.ICardModel New(Guid id, IOwner owner)
        {
            var tmp = _templates[id];
            var card = new CardModel(tmp, owner);
            return card;
        }

        public static ICardModelTemplate Get(Guid id)
        {
            return _templates.ContainsKey(id) ? _templates[id] : null;
        }

        public static IEnumerable<ICardModelTemplate> OfType(ECardType type)
        {
            return _templates.Select(kv => kv.Value).Where(template => template.Type == type);
        }

        private static void MockConstruct()
        {
            _templates.Clear();
            var X = int.MaxValue;
            CardModelTemplate[] cardsModel =
            {                                                    //M, A, H
                new CardModelTemplate(ECardType.King, "King",           X, 1, 20),       // king
                new CardModelTemplate(ECardType.Queen, "Queen",         6, 6, 7),
                new CardModelTemplate(ECardType.Paladin, "Paladin",     1, 1, 2),        // pawn
                new CardModelTemplate(ECardType.Gryphon, "Gryphon",     2, 1, 2, new[] {EAbility.Mountable, EAbility.Lethal}),      // knight
                new CardModelTemplate(ECardType.Archer, "Archer",       3, 2, 3),        // bishop
                new CardModelTemplate(ECardType.Castle, "Cannon",       4, 5, 6, new[] {EAbility.Guard}),
                new CardModelTemplate(ECardType.Barricade, "Barricade", 2, 0, 3, new[] {EAbility.Guard, EAbility.Static}),
                new CardModelTemplate(ECardType.Siege, "Siege",         3, 2, 1, new[] {EAbility.Static}),
                new CardModelTemplate(ECardType.Dragon, "Dragon",       9, 8, 5, new[] {EAbility.Mountable}),
            };

            foreach (var card in cardsModel)
            {
                _templates[card.Id] = card;
            }
        }

        public static ICardModelTemplate GetRandom()
        {
            var numCards = _templates.Count;
            var r = new System.Random();
            var rand = r.Next(0, numCards - 1);
            var key = _templates.Keys.ElementAt(rand);
            return _templates[key];
        }

        private static readonly Dictionary<Guid, ICardModelTemplate> _templates = new Dictionary<Guid, ICardModelTemplate>();
    }
}
