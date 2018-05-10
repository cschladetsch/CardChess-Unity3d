namespace App.Model
{
    public interface IAction
    {
        IPlayer Player { get; }
        EAction Action { get; }
    }
}