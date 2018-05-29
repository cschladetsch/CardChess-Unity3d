namespace App.Model
{
	using Common;

    /// <summary>
    /// Common for all models that are owned by a Player
    /// </summary>
    public class PlayerOwnedModelBase
        : ModelBase
    {
        public IPlayerModel Player => Owner.Value as IPlayerModel;
        public EColor Color => Player.Color;
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;
    }
}
