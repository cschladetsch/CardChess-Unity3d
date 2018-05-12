namespace App.Action
{
    public interface IAction
    {
        Agent.IPlayer Player { get; }
        EAction Action { get; }
    }
}
