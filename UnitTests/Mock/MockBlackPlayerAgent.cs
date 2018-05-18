using App.Action;
using Flow;

namespace App
{
    /// <inheritdoc />
    /// <summary>
    /// PlayerAgent used to test as Black pieces.
    /// </summary>
    internal class MockBlackPlayerAgent : Agent.PlayerAgent
    {
        //public override IFuture<PlayCard> PlaceKing()
        //{
        //    var pc = new PlayCard
        //    {
        //        Coord = new Coord(3, 7),
        //        Card = King
        //    };
        //    var f = New.Future<PlayCard>();
        //    f.Value = pc;
        //    return f;
        //}
    }
}
