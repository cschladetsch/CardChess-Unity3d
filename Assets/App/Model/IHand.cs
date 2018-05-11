using System.Collections.Generic;

namespace App.Model
{
    public interface IHand : ICardCollection
    {
        void NewGame();
        void Add(ICardInstance card);
        bool Remove(ICardInstance card);
    }
}
