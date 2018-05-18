using System;
using System.Collections.Generic;
using System.Linq;

// event not used
#pragma warning disable 67

namespace App.Model
{
    using Common;

    public class CardModel :
        ModelBase,
        ICardModel
    {
        public CardModel(ICardModelTemplate modelTemplate, IOwner owner)
        {
            ModelTemplate = modelTemplate;

            // TODO
            base.Create(owner);

            Attack = modelTemplate.Attack;
            Health = modelTemplate.Health;

            if (modelTemplate.Abilities != null)
                Abilities = modelTemplate.Abilities.ToList();
        }

        public IEnumerable<Common.IEffect> Effects { get; }
        public ECardType Type => ModelTemplate.Type;

        public ICardModelTemplate ModelTemplate { get; }
        public string Description { get; }
        public int Attack { get; }

        public int Health { get; private set; }

        public IEnumerable<ICardModel> Items { get; } = new List<ICardModel>();
        public IEnumerable<EAbility> Abilities { get; } = new List<EAbility>();

        public Response ChangeHealth(int value, ICardModel cause)
        {
            if (Health == value)
                return new Response(EResponse.Ok, EError.NoChange);

            Health = value;

            //HealthChanged?.Invoke(this, cause);

            if (Health <= 0)
                Die();

            return Response.Ok;
        }

        public static ICardModel New(ICardModelTemplate modelTemplate, IOwner owner)
        {
            return new CardModel(modelTemplate, owner);
        }

        public void ChangeAttack(int value, ICardModel cause)
        {
            throw new NotImplementedException("ChangeAttack");
        }

        private void Die()
        {
            //Died?.Invoke(this, this);
        }
    }
}
