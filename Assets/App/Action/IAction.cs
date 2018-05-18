namespace App.Action
{
    public interface IAction
    {
        Agent.IPlayerAgent PlayerAgent { get; }
        EAction Action { get; }
    }
}
