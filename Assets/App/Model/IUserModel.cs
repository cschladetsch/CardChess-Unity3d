namespace App.Model
{
    using System.Collections.Generic;
    using Dekuple.Model;

    /// <inheritdoc />
    /// <summary>
    /// TODO: Multi player. A Persistent user.
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
