using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A description of a card. Used to make other cards in the game.
    /// These are stored in the Players' CardLibrary.
    /// </summary>
    public class CardTemplate :
        ModelBase,
        ICardTemplate
    {
        public ECardType Type { get; }
        public EPieceType PieceType { get; }
        public string Description { get; }
        public string FlavourText { get; }
        public int ManaCost { get; }
        public int Attack { get; }
        public int Health { get; }
        public IEnumerable<ICardModel> Items { get; } = new List<ICardModel>();
        public IEnumerable<EAbility> Abilities { get; } = new List<EAbility>();
        public IEnumerable<IEffect> Effects { get; } = new List<IEffect>();

        public CardTemplate(ECardType type, EPieceType pieceType, string name, int manaCost, int attack,
            int health, IEnumerable<EAbility> abilities = null,
            string flavourText = "")
        {
            Type = type;
            PieceType = pieceType;
            Id = Guid.NewGuid();
            Name = name;
            ManaCost = manaCost;
            Attack = attack;
            Health = health;
            if (abilities != null)
                Abilities = abilities.ToList();
            FlavourText = flavourText;
        }

        public ICardModel New(IPlayerModel player)
        {
            return Registry.New<ICardModel>(this, player);
        }
    }
}
