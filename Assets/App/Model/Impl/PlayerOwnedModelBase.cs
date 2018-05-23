using App.Common;

namespace App.Model
{
    public class PlayerOwnedModelBase
        : ModelBase
        , IPlayerOwnedModel
        , IConstructWith<IPlayerModel>
    {
        public IPlayerModel Player { get; set; }
        public EColor Color => Player.Color;
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;

        public bool Construct(IPlayerModel player)
        {
            Player = player;
            return true;
        }
    }
}
