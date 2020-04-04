using System;
using System.Linq;
using System.Collections.Generic;

using Dekuple.Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A description of a card. Used to make other cards in the game.
    /// These are stored in the Players' CardLibrary.
    /// </summary>
    public class CardTemplate
        : ModelBase
        , ICardTemplate
    {
        public ECardType Type { get; }
        public EPieceType PieceType { get; }
        public string Description { get; }
        public string FlavourText { get; }
        public int ManaCost { get; }
        public int Power { get; }
        public int Health { get; }
        public GameObject Prefab { get; }
        public IEnumerable<IItemModel> Items { get; } = new List<IItemModel>();
        public IEnumerable<EAbility> Abilities { get; } = new List<EAbility>();
        public IEnumerable<IEffectModel> Effects { get; } = new List<IEffectModel>();

        public CardTemplate(ECardType type, EPieceType pieceType, string name, int manaCost, int attack,
            int health, GameObject model = null, IEnumerable<EAbility> abilities = null,
            string flavourText = "")
            : base(null)
        {
            Type = type;
            PieceType = pieceType;
            Id = Guid.NewGuid();
            Name = name;
            ManaCost = manaCost;
            Power = attack;
            Health = health;
            if (abilities != null)
                Abilities = abilities.ToList();
            FlavourText = flavourText;
            Prefab = model;
        }

        /// <summary>
        /// Make a new card from this template, owned by the given player
        /// </summary>
        /// <param name="player">The owner of new created card</param>
        /// <returns>A new card from this template</returns>
        public ICardModel New(IPlayerModel player)
        {
            var model = Registry.New<ICardModel>(this, player);
            model.MeshObject = Object.Instantiate(Prefab);
            return model;
        }
    }
}
