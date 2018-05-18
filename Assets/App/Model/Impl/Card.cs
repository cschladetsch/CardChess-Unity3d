using System;
using System.Collections.Generic;
using System.Linq;

// event not used
#pragma warning disable 67

namespace App.Model
{
    using Common;

    public class Card :
        ModelBase,
        ICard
    {
        public Card(ICardTemplate template, IOwner owner)
        {
            Template = template;

            // TODO
            base.Create(owner);

            Attack = template.Attack;
            Health = template.Health;

            if (template.Abilities != null)
                Abilities = template.Abilities.ToList();
        }

        public IEnumerable<Common.IEffect> Effects { get; }
        public ECardType Type => Template.Type;

        public ICardTemplate Template { get; }
        public string Description { get; }
        public int Attack { get; }

        public int Health { get; private set; }

        public IEnumerable<ICard> Items { get; } = new List<ICard>();
        public IEnumerable<EAbility> Abilities { get; } = new List<EAbility>();

        public void ChangeHealth(int value, ICard cause)
        {
            if (Health == value)
                return;

            Health = value;

            //HealthChanged?.Invoke(this, cause);

            if (Health <= 0)
                Die();
        }

        public static ICard New(ICardTemplate template, IOwner owner)
        {
            return new Card(template, owner);
        }

        public void ChangeAttack(int value, ICard cause)
        {
            throw new NotImplementedException("ChangeAttack");
        }

        private void Die()
        {
            //Died?.Invoke(this, this);
        }
    }
}
