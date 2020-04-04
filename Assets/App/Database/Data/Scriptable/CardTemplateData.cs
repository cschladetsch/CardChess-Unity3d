namespace App.Database.Data.Scriptable
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Common;
    
    [CreateAssetMenu(fileName = "CardTemplateData", menuName = "Cards/Template", order = 1)]
    public class CardTemplateData
        : ScriptableObject
    {
        public ECardType Type;
        public EPieceType PieceType;
        public string Title;
        public string FlavourText;
        public int ManaCost;
        public int Power;
        public int Health;
        public string Id = Guid.NewGuid().ToString();

        public GameObject Model;
        // public List<IItemModel> Items;
        public List<EAbility> Abilities;
        // public List<IEffectModel> Effects;

        [ContextMenu("New Guid")]
        public void NewGuid()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}