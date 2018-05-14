namespace App.Agent
{
    public interface ICardInstance : IAgent<Model.ICardInstance>
    {
        int Health { get; }
    }
}
