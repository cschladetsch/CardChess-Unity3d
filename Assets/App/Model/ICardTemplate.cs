using System.Collections.Generic;

namespace App.Model
{
    using Common;

    public interface ICardTemplate :
        ICard
    {
        int ManaCost { get; }
		string FlavourText { get; }
    }
}
