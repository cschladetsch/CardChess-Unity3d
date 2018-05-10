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
            CardTemplate[] cards =
            {
                new CardTemplate(ECardType.King, "King", 1, 20),
                new CardTemplate(ECardType.Pawn, "Pawn", 1, 1),
                new CardTemplate(ECardType.Queen, "Queen", 4, 7),
                new CardTemplate(ECardType.Bishop, "Bishop", 2, 3),
                new CardTemplate(ECardType.Knight, "Knight", 2, 2, new[] {EAbility.Flying}),
                new CardTemplate(ECardType.Rook, "Rook", 5, 5, new[] {EAbility.Guard}),
            };

            foreach (var card in cards)
            {
                _templates[card.Id] = card;
            }
        }

        private readonly Dictionary<Guid, ICardTemplate> _templates = new Dictionary<Guid, ICardTemplate>();
    }
}
