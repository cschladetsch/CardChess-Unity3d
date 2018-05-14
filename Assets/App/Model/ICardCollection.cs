using System;
using System.Collections.Generic;

namespace App.Model
{
    /// <summary>
    ///     A generic collection of cards: can be CardInstances or CardTemplates
    /// </summary>
    /// <typeparam name="TCard"></typeparam>
    public interface ICardCollection<TCard>
    {
        #region Properties
        string Name { get; }
        int MaxCards { get; }
        IList<TCard> Cards { get; }
        #endregion

        #region Methods
        bool Add(TCard card);
        bool Remove(TCard card);
        bool Has(Guid id);
        TCard Get(Guid id);
        #endregion
    }
}
