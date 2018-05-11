using System;
using System.Collections.Generic;

namespace App.Model
{
    public class Deck : CardCollection
    {
        public IEnumerable<ICard> Draw(int n = 1)
        {
            throw new NotImplementedException();
        }

        public override int MaxCards => 30;
    }
}
