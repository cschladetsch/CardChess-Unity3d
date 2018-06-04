
namespace App.View
{
    using Common;

    public interface ISquareView
        : IViewBase
    {
        EColor Color { get; }
        Coord Coord { get; }
    }
}
