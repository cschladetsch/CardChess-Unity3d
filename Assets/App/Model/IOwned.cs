namespace App.Model
{
    public interface IOwned
    {
        Agent.IPlayer Owner { get; }
    }
}
