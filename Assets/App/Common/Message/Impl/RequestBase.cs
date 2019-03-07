using System;

namespace App.Common.Message
{
    using Model;
    using Dekuple.Common.Message;
    using Dekuple.Model;
    using IPlayerModel = Model.IPlayerModel;

    /// <summary>
    /// Common to all Actions requested by player
    /// </summary>
    public class RequestBase
        : IRequest
    {
        public App.Model.IPlayerModel Player { get; }
        public EActionType Action { get; }
        public Guid Id { get; set; }

        Dekuple.Model.IPlayerModel IRequest.Player => throw new NotImplementedException();

        Dekuple.Common.Message.EActionType IRequest.Action => throw new NotImplementedException();

        public RequestBase(Dekuple.Model.IPlayerModel player, EActionType actionType)
        {
            Id = Guid.NewGuid();
            Player = (IPlayerModel)player;
            Action = actionType;
        }
    }
}
