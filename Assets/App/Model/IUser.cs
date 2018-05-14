using System.Collections.Generic;

namespace App.Model
{
    /// <summary>
    /// A Persistent user.
    /// </summary>
    public interface IUser : IHasId
    {
        #region Properties
        string Name { get; }
        string Handle { get; }
        string Email { get; }

        IDictionary<CardCollectionDesc, ICardCollection<ICardTemplate>> Decks { get; }
        IEnumerable<ICardTemplate> AllCards { get; }
        IGameHistory GameHistory { get; }
        #endregion
    }
}
