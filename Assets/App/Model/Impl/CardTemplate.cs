using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A description of a card. Used to make Agent.CardInstances that are
    /// stored in Agent.Player's Hands and Decks.
    /// </summary>
    public class CardTemplate :
        ModelBase,
        ICardTemplate
    {
        public ECardType Type { get; private set; }
        public string FlavourText { get; private set; }
        public int ManaCost { get; private set; }
        public int Attack { get; private set; }
        public int Health { get; private set; }
        public IEnumerable<EAbility> Abilities { get; private set; }

        public void Set(ECardType type, string name, int manaCost, int attack, int health, IEnumerable<EAbility> abilities = null,
            string flavourText = "")
        {
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
