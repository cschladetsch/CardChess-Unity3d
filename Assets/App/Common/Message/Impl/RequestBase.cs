using System;
using Dekuple;

namespace App.Common.Message
{
    using Model;

    /// <summary>
    /// Common to all Actions requested by player
    /// </summary>
    public class RequestBase
        : IGameRequest
    {
        public IPlayerModel Player { get; }
        public EActionType Action { get; }
        public Guid Id { get; set; }

        public RequestBase(IPlayerModel player, EActionType actionType)
        {
            Id = Guid.NewGuid();
            Player = player;
            Owner = Player;
            Action = actionType;
        }

        public IOwner Owner { get; private set; }
    }
}
