using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    public class CardTemplate : ModelBase, ICardTemplate
    {
        public Guid Id { get; }
        public ECardType Type { get; }
        public string FlavourText { get; }
        public int ManaCost { get; }
        public int Attack { get; }
        public int Health { get; }
        public string Name { get;}
        public IList<EAbility> Abilities { get; }

        public CardTemplate(ECardType type, string name, int manaCost, int attack, int health, IEnumerable<EAbility> abilities = null,
            string flavourText = "")
        {
            Id = Guid.NewGuid();
            Type = type;
            Name = name;
            ManaCost = manaCost;
            Attack = attack;
            Health = health;
            if (abilities != null)
                Abilities = abilities.ToList();
            FlavourText = flavourText;
        }

    }
}
