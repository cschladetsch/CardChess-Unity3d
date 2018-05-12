using System;

namespace App.Model
{
    public class Deck : CardCollection<ICardInstance>, IDeck
    {
        public override int MaxCards => 50;

        public bool Create()
        {
            throw new NotImplementedException();
        }

        public void NewGame()
        {
            throw new NotImplementedException();
        }

        public void Shuffle()
        {
            throw new NotImplementedException();
        }

        public ICardInstance Draw()
        {
            throw new NotImplementedException();
        }

        public void AddToBottom(ICardInstance card)
        {
            throw new NotImplementedException();
        }

        public void AddToRandom(ICardInstance card)
        {
            throw new NotImplementedException();
        }
    }
}
