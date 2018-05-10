using System;
using System.Collections.Generic;
using Flow;
using UnityEngine.EventSystems;

namespace App.Model
{
    public class CardCollectionDesc : IHasId
    {
        public Guid Id { get; }
        public string Name { get; }
    }

    /// <summary>
    /// A Player in the game.
    /// Hopefully, these could be bots, or remote players as well
    /// as simple hotseat players at the same local device.
    /// </summary>
    public interface IPlayer
    {
        EColor Color { get; }
        int Mana { get; set; }
        int Health { get; set; }
        IHand Hand { get; }
        IDeck Deck { get; }
        IDictionary<CardCollectionDesc, ICardCollection> Collections { get; }
        void NewGame();
    }
}
