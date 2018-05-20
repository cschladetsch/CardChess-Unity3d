using App.Common;

namespace App.Model
{
    public class PlayerOwnedModelBase
        : ModelBase
            , IPlayerOwnedModel
    {
        public IPlayerModel Player { get; }
        public EColor Color => Player.Color;
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;

        public PlayerOwnedModelBase(IPlayerModel player)
        {
            Player = player;
        }
    }
}