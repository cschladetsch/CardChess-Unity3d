using System;
using System.Collections.Generic;
using System.Linq;

// event not used
#pragma warning disable 67

namespace App.Model
{
    using Common;
    using Common.Message;

    public class CardModel :
        ModelBase,
        ICardModel
    {
        #region Public Properties
        public ECardType Type => Template.Type;
        public ICardTemplate Template { get; }
        public string Description => Template.FlavourText;
        public int ManaCost { get; }
        public int Attack { get; }
        public int Health { get; private set; }
        public IEnumerable<ICardModel> Items { get; } = new List<ICardModel>();
        public IEnumerable<IEffect> Effects { get; } = new List<IEffect>();
        public IEnumerable<EAbility> Abilities { get; } = new List<EAbility>();
        public IPlayerModel Player => Owner as IPlayerModel;
        public EPieceType PieceType { get; }
        #endregion

        #region Public Methods
        public CardModel()
        {
        }

        public CardModel(ICardTemplate template, IOwner owner)
        {
            Template = template;
            Construct(owner);

            PieceType = template.PieceType;
            Attack = template.Attack;
            Health = template.Health;
            ManaCost = template.ManaCost;

            if (template.Effects != null)
                Effects = template.Effects.ToList();
            if (template.Items != null)
                Items = template.Items.ToList();
            if (template.Abilities != null)
                Abilities = template.Abilities.ToList();
        }


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

        public static ICardModel New(ICardTemplate template, IOwner owner)
        {
            return new CardModel(template, owner);
        }

        public void ChangeAttack(int value, ICardModel cause)
        {
            throw new NotImplementedException("ChangeAttack");
        }

        public override string ToString()
        {
            return $"CardModel: name={Name}, type={PieceType}";
        }

        #endregion

        #region Private Methods
        private void Die()
        {
            //Died?.Invoke(this, this);
        }
        #endregion
    }
}
