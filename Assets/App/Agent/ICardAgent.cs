using Dekuple.Agent;

namespace App.Agent
{
    using Model;

    public interface ICardAgent
        : IAgent<ICardModel>
        , ICardProperties
    {
    }
}
