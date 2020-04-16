namespace App.Agent
{
    using System.Collections;
    using Flow;
    using Model;
    using Common.Message;

    /// <summary>
    /// TODO
    /// </summary>
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

            Info($"Adding GameLoop for {Model}");
            _Node.Add(New.Coroutine(Coro).Named($"GameLoop for {Model}"));
        }

        public override void EndGame()
        {
            _end = true;
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
