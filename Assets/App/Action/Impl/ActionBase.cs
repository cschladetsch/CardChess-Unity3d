using App.Model;

namespace App.Action
{
    /// <summary>
    /// Common to all Actions
    /// </summary>
    public class ActionBase : IAction
    {
        public IPlayerModel Player { get; }
        public EActionType Action { get; }

        public ActionBase() {}

        public ActionBase(IPlayerModel player, EActionType actionType)
        {
            Player = player;
            Action = actionType;
        }
    }
}
