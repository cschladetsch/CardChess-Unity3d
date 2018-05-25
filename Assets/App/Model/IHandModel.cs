using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    using Common;

    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
    {
        ICardModel this[int index] { get; set; }
        Response NewGame();
        //new IEnumerable<ICardModel> Cards => Cards.Cast<ICardModel>();
    }
}
