using App.Model.Card;

namespace App.Common.Message
{
    using Model;

    public class MountPiece : RequestBase
    {
        public IPieceModel Mount;
        public IPieceModel Rider;

        public MountPiece(IPlayerModel player, IPieceModel mount, IPieceModel rider)
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
