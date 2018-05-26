using App.Common.Message;
using App.Model;

namespace App.Common.Message
{
    using Common;

    public class PlaceKing : RequestBase
    {
        public Coord Coord { get;  }
        public PlaceKing(IPlayerModel player, Coord coord)
            : base(player, EActionType.Pass)
        {
            Coord = coord;
        }
    }
}
