using System.Collections.Generic;

namespace App.Model
{
    using Common;

    /// <summary>
    /// A Persistent user.
    /// </summary>
    public interface IUserModel : IModel
    {
        #region Properties
        string Handle { get; }
        string Email { get; }

        IDictionary<CardCollectionDesc, ICardCollection<ICardModelTemplate>> Decks { get; }
        IEnumerable<ICardModelTemplate> AllCards { get; }
        IGameHistory GameHistory { get; }
        #endregion
    }
}
