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
        public IEnumerable<Common.IEffect> Effects { get; }
        public ECardType Type { get; private set; }
        public string FlavourText { get; private set; }
        public int ManaCost { get; private set; }
        public string Description { get; }
        public int Attack { get; private set; }
        public int Health { get; private set; }
        //public event CardDelegate Born;
        //public event CardDelegate Died;
        //public event CardDelegate Reborn;
        //public event CardDelegate Moved;
        //public event CardDelegate AppliedToPiece;
        //public event CardDelegate RemovedFromPiece;
        //public event CardDelegate HealthChanged;
        //public event CardDelegate AttackChanged;
        //public event CardDelegate ItemAdded;
        //public event CardDelegate ItemRemoved;
        //public event CardDelegate Attacked;
        //public event CardDelegate Defended;
        public ICardTemplate Template { get; }
        public IEnumerable<ICard> Items { get; }
        public IEnumerable<EAbility> Abilities { get; private set; }
        public void ChangeHealth(int amount, ICard cause)
        {
            throw new NotImplementedException();
        }

        public CardTemplate(ECardType type, string name, int manaCost, int attack, int health, IEnumerable<EAbility> abilities = null,
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
