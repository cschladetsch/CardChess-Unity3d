using Dekuple.View;

namespace App.View
{
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// View of a square on the board
    /// </summary>
    public interface ISquareView
        : IViewBase
    {
        EColor Color { get; }
        Coord Coord { get; }
    }
}
