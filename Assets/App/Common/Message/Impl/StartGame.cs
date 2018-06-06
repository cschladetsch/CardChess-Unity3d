using App.Model;

namespace App.Common.Message
{
    public class StartGame : RequestBase
    {
        public StartGame(IPlayerModel player)
            : base(player, EActionType.StartGame)
        {
        }
    }
}
