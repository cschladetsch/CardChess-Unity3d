using System;

namespace App.Common.Message
{
    using Model;

    /// <summary>
    /// Common to all Actions requested by player
    /// </summary>
    public class RequestBase
        : IRequest
    {
        public IPlayerModel Player { get; }
        public EActionType Action { get; }
        public Guid Id { get; set; }

        public RequestBase()
        {
            Id = Guid.NewGuid();
        }

        public RequestBase(IPlayerModel player, EActionType actionType)
        {
            Player = player;
            Action = actionType;
        }
    }
}
