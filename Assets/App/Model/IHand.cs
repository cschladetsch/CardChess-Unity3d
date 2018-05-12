using System.Collections.Generic;

namespace App.Model
{
    public interface IHand : ICardCollection<ICardInstance>
    {
        void NewGame();
    }
}
