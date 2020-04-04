namespace App.Database.Data.Scriptable
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Collection of CardTemplates used by the game.
    /// </summary>
    [CreateAssetMenu(fileName = "CardTemplateDatabase", menuName = "Cards/Collection", order = 1)]
    public class CardTemplateDatabase
        : ScriptableObject
    {
        public List<CardTemplateData> Cards;
    }
}
