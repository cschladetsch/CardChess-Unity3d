using System.Collections.Generic;

namespace App.Model
{
    /// <summary>
    /// A Persistent user.
    /// </summary>
    public interface IUserModel
        : IModel
    {
        string Handle { get; }
        string Email { get; }

        IDictionary<CardCollectionDesc, IList<ICardTemplate>> Decks { get; }
        IEnumerable<ICardTemplate> AllCards { get; }
        IGameHistory GameHistory { get; }
    }
}
