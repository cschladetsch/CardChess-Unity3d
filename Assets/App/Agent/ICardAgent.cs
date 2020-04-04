namespace App.Agent
{
    using Dekuple.Agent;
    using Model;

    public interface ICardAgent
        : IAgent<ICardModel>
        , ICardProperties
    {
    }
}
