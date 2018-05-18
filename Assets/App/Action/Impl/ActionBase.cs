using App.Model;

namespace App.Action
{
    /// <summary>
    /// Common to all Actions
    /// </summary>
    public class ActionBase : IAction
    {
        public IPlayerModel Player { get; }
        public EAction Type { get; }
    }
}
