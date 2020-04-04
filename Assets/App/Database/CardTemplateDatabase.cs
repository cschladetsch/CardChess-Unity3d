using System.Collections.Generic;
using UnityEngine;

namespace App.Model
{
    [CreateAssetMenu(fileName = "CardTemplateDatabase", menuName = "Inventory/CardTemplates", order = 1)]
    public class CardTemplateDatabase : ScriptableObject
    {
        public List<CardTemplateData> Cards;
    }
}