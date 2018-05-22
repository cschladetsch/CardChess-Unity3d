namespace App.Action
{
    using Model;

    /// <summary>
    /// Pass the turn
    /// </summary>
    public class Pass : ActionBase
    {
        public Pass(IPlayerModel player)
            : base(player, EActionType.Pass)
        {
        }
    }

}
