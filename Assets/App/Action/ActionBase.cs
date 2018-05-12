namespace App.Action
{
    /// <summary>
    /// Common to all Actions
    /// </summary>
    public abstract class ActionBase : IAction
    {
        public Agent.IPlayer Player { get; protected set; }
        public EAction Action { get; protected set; }
    }
}
