namespace App.Model
{
	using Common;
=======
        : ModelBase
        , IPlayerOwnedModel
        , IConstructWith<IPlayerModel>
    {
        public IPlayerModel Player => Owner as IPlayerModel;
        public EColor Color => Player.Color;
        public bool IsWhite => Color == EColor.White;
        public bool IsBlack => Color == EColor.Black;

        public bool Construct(IPlayerModel player)
        {
            Owner = player;
            return true;
        }
    }
}
