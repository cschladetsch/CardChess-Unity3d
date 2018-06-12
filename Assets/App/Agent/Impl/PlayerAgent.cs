using Flow;

namespace App.Agent
{
    using Common.Message;
    using Model;

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
    }
}
