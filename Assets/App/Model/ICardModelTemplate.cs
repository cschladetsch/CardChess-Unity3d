using System.Collections.Generic;

namespace App.Model
{
    using Common;

    public interface ICardModelTemplate :
        ICardModel
    {
        int ManaCost { get; }
		string FlavourText { get; }
    }
}
