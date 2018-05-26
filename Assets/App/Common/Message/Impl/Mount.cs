using App.Model.Card;

namespace App.Common.Message
{
    using Model;

    public class MountPiece : RequestBase
    {
        public ICardModel Mount;
        public ICardModel Rider;

        public MountPiece(IPlayerModel player, ICardModel mount, ICardModel rider)
            : base(player, EActionType.Mount)
        {
            Mount = mount;
            Rider = rider;
        }

        public override string ToString()
        {
            return $"{Player} {Rider} mounts {Mount}";
        }
    }
}
