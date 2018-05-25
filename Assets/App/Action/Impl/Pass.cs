namespace App.Action
{
    using Model;

    /// <summary>
    /// Pass the turn
    /// </summary>
    public class Pass : RequestBase
    {
        public Pass(IPlayerModel player)
            : base(player, EActionType.Pass)
        {
        }
    }

}
