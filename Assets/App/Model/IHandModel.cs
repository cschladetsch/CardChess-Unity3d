using System.Collections.Generic;

namespace App.Model
{
    using Common;

    public interface IHandModel
        : IModel
        , ICardCollection<ICardModel>
    {
        Response NewGame();
    }
}
