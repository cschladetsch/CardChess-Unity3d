namespace App.Common.Message
{
    using Model;

    /// <summary>
    /// Pass the turn
    /// </summary>
    public class Pass
        : RequestBase
        , IRequest
    {
        public Pass(IPlayerModel player)
            : base(player, EActionType.Pass)
        {
        }
    }

}
