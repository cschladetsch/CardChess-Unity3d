namespace App.Agent
{
    using System.Collections;
    using Flow;
    using Model;
    using Common.Message;

    public class PlayerAgent
        : PlayerAgentBase
    {
        public PlayerAgent(IPlayerModel model)
            : base(model)
        {
        }

        public override void StartGame()
        {
            base.StartGame();
            _Node.Add(New.Coroutine(Coro));
        }

        public override void EndGame()
        {
            _end = true;
        }

        IEnumerator Coro(IGenerator self)
        {
            Name = "PlayerAgentCoro";
            while (!_end)
            {
                while (_Requests.Count > 0 && _Futures.Count > 0)
                {
                    var req = _Requests[0];
                    _Futures[0].Value = req;

                    _Requests.RemoveAt(0);
                    _Futures.RemoveAt(0);
                    yield return null;
                }

                yield return null;
            }
        }

        public override IFuture<RejectCards> Mulligan()
        {
            return null;
        }

        public override ITransient TurnStart()
        {
            return null;
        }

        public override ITransient TurnEnd()
        {
            return null;
        }

        private bool _end = false;
    }
}
