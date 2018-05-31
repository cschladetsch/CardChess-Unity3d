using App.Model;

namespace App.Mock.Agent
{
    using App.Agent;

    public class WhitePlayerAgent
        : PlayerAgentBase
        , IWhitePlayerAgent
    {
        public WhitePlayerAgent(IPlayerModel model) : base(model)
        {
        }
    }
}
