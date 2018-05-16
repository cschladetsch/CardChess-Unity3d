using System;

namespace App.Model
{
    public class Hand : Common.CardCollection<ICardInstance>, IHand
    {
        public override int MaxCards => 9;

        public void NewGame()
        {
            Cards.Clear();
        }

        public void RemoveRandom()
        {
            if (Cards.Count == 0)
                return;
            Cards.RemoveAt(new Random().Next(0, Cards.Count - 1));
        }
    }
}
