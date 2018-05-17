using System;
using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// All cards owned by a player
    /// </summary>
    public abstract class CardLibrary :
        ModelBase,
        ICardCollection<ICardTemplate>,
        ICreateWith<Guid>
    {
        public int MaxCards => int.MaxValue;

        public IList<ICardTemplate> Cards { get; } = new List<ICardTemplate>();

        public bool Add(ICardInstance card)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ICardInstance card)
        {
            throw new NotImplementedException();
        }

        public bool Add(ICardTemplate card)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ICardTemplate card)
        {
            throw new NotImplementedException();
        }

        public bool Has(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICardTemplate Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Create(Guid id)
        {
            return true;
        }
    }
}
