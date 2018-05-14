namespace App.Agent
{
    public interface ICardInstance : IAgent<Model.ICardInstance>, IHasId
    {
        int Health { get; }
    }
}
