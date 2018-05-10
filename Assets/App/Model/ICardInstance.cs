using System.Collections.Generic;
using System.ComponentModel;

namespace App.Model
{
    public interface IOwned
    {
        IPlayer Owner { get; }
    }

    public interface ICardInstance : IHasId, IOwned
    {
        int Attack { get; set; }
        int Health { get; set; }
        ICard Template { get; }
        IList<ICardInstance> Items { get; }
    }
}
