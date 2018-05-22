using App.Common;
using App.Model;

namespace App.Action
{
    public class PlaceKing : ActionBase
    {
        public Coord Coord { get;  }
        public PlaceKing(IPlayerModel player, Coord coord)
            : base(player, EActionType.Pass)
        {
            Coord = coord;
        }
    }
}
