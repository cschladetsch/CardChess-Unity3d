using App.Model;

namespace App.Mock.Agent
{
    using App.Agent;

    public class BlackPlayerAgent
        : PlayerAgentBase
        , IBlackPlayerAgent
    {
        public BlackPlayerAgent(IPlayerModel model) : base(model)
        {
        }
    }
}
