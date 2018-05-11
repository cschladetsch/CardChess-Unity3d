using System;
using System.Collections.Generic;
using System.Linq;
using App.Model;

namespace App.Database
{
    class CardTemplates
    {
        public CardTemplates()
        {
            MockConstruct();
        }

        public ICardTemplate Get(Guid id)
        {
            return _templates.ContainsKey(id) ? _templates[id] : null;
        }

        public IEnumerable<ICardTemplate> OfType(ECardType type)
        {
            return _templates.Select(kv => kv.Value).Where(template => template.Type == type);
        }

        private void MockConstruct()
        {
            _templates.Clear();
            var X = int.MaxValue;
            CardTemplate[] cards =
            {                                                    //M, A, H
                new CardTemplate(ECardType.King, "King",           X, 1, 20),
                new CardTemplate(ECardType.Peon, "Peon",           1, 1, 2),
                new CardTemplate(ECardType.Gryphon, "Gryphon",     2, 1, 2, new[] {EAbility.Flying, EAbility.Lethal}),
                new CardTemplate(ECardType.Bishop, "Bishop",       3, 2, 3),
                new CardTemplate(ECardType.Castle, "Castle",       4, 5, 6, new[] {EAbility.Guard}),
                new CardTemplate(ECardType.Queen, "Queen",         6, 6, 7),

                new CardTemplate(ECardType.Barricade, "Barricade", 2, 0, 3, new[] {EAbility.Guard, EAbility.Static}),
                new CardTemplate(ECardType.Siege, "Siege",         3, 2, 1, new[] {EAbility.Static}),
                new CardTemplate(ECardType.Dragon, "Dragon",       9, 8, 5, new[] {EAbility.Flying}),
            };

            foreach (var card in cards)
            {
                _templates[card.Id] = card;
            }
        }

        private readonly Dictionary<Guid, ICardTemplate> _templates = new Dictionary<Guid, ICardTemplate>();
    }
}
