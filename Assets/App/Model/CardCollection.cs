using System;
using System.Collections.Generic;

namespace App.Model
{
    public abstract class CardCollection : ModelBase, ICardCollection
    {
        public abstract int MaxCards { get; }

        public IList<ICard> Cards { get; private set; }

        public void Add(ICard card)
        {
            throw new NotImplementedException();
        }

        public void Remove(ICard card)
        {
            throw new NotImplementedException();
        }
    }
}
