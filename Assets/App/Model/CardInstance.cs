using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    public class CardInstance : ICardInstance
    {
        public event CardInstanceDelegate Born;
        public event CardInstanceDelegate Died;
        public event CardInstanceDelegate Reborn;
        public event CardInstanceDelegate Moved;
        public event CardInstanceDelegate AppliedToPiece;
        public event CardInstanceDelegate RemovedFromPiece;
        public event CardInstanceDelegate HealthChanged;
        public event CardInstanceDelegate AttackChanged;
        public event CardInstanceDelegate ItemAdded;
        public event CardInstanceDelegate ItemRemoved;
        public event CardInstanceDelegate Attacked;
        public event CardInstanceDelegate Defended;

        public Guid Id { get; }
        public ICardTemplate Template { get; }
        public Agent.IPlayer Owner { get; }
        public ECardType Type => Template.Type;
        public int Attack { get; set; }
        public int Health => _health;

        public IList<ICardInstance> Items { get; } = new List<ICardInstance>();
        public IList<EAbility> Abilities { get; } = new List<EAbility>();

        public CardInstance(ICardTemplate template, Agent.IPlayer owner)
        {
            Id = Guid.NewGuid();
            Template = template;
            Owner = owner;

            Attack = template.Attack;
            _health = template.Health;

            Abilities = template.Abilities.ToList();
        }

        public static ICardInstance New(ICardTemplate template, Agent.IPlayer owner)
        {
            return new CardInstance(template, owner);
        }

        public void ChangeHealth(int value, ICardInstance cause)
        {
            if (_health == value)
                return;

            _health = value;

            HealthChanged?.Invoke(this, cause);

            if (_health <= 0)
                Die();
        }

        private void Die()
        {
            Died?.Invoke(this, this);
        }

        private int _health;
    }
}
