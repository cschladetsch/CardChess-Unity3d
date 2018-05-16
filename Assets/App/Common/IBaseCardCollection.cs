using System.Collections.Generic;

namespace App.Common
{
    interface IBaseCardCollection<TCard> where TCard : class, IHasId, IHasName
    {
        int MaxCards { get; }
        IList<TCard> Cards { get; }
    }
}
