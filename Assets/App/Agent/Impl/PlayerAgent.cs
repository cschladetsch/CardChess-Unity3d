using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.Message;
using App.Model;
using Flow;

namespace App.Agent
{
    public class PlayerAgent
        : PlayerAgentBase
    {
        public PlayerAgent(IPlayerModel model)
            : base(model)
        {
        }

        public override IFuture<RejectCards> Mulligan()
        {
            return null;
        }

        public override IFuture<PlacePiece> PlaceKing()
        {
            return null;
        }

        public override ITransient TurnStart()
        {
            return null;
        }

        public override ITimedFuture<IRequest> NextRequest(float seconds)
        {
            var future = New.TimedFuture<IRequest>(TimeSpan.FromSeconds(seconds));
            _futures.Add(future);
            future.TimedOut += f => _futures.RemoveRef(future);
            return future;
        }

        protected override IEnumerator Next(IGenerator self)
        {
            if (_Requests.Count > 0 && _futures.Count > 0)
            {
                var future = _futures[0];
                var req = _Requests[0];
                _futures.RemoveAt(0);
                _Requests.RemoveAt(0);

                future.Value = req.Request;
            }

            return base.Next(self);
        }

        public override ITransient TurnEnd()
        {
            return null;
        }

        private readonly List<IFuture<IRequest>> _futures = new List<IFuture<IRequest>>();
    }
}
