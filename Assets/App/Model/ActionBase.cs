namespace App.Model
{
    /// <summary>
    /// Common to all Actions
    /// </summary>
    public abstract class ActionBase : IAction
    {
        public IPlayer Player { get; protected set; }
        public EAction Action { get; protected set; }
    }
}