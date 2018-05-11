using System;

namespace App.Model
{
    public class Hand : CardCollection
    {
        public void RemoveRandom()
        {
            throw new NotImplementedException();
        }

        public override int MaxCards => 9;
    }
}
