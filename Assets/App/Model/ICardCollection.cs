using System;
using System.Collections.Generic;
using UnityEditor;

namespace App.Model
{
    /// <summary>
    /// A generic collection of cards: can be CardInstances or CardTemplates
    /// </summary>
    /// <typeparam name="TCard"></typeparam>
    public interface ICardCollection<TCard>
    {
        string Name { get; }
        int MaxCards { get; }
        IList<TCard> Cards { get; }

        bool Add(TCard card);
        bool Remove(TCard card);
        bool Has(Guid id);
        TCard Get(Guid id);
    }
}
