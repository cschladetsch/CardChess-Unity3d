namespace App.View
{
    using Dekuple.View;
    using Common;

    /// <inheritdoc />
    /// <summary>
    /// View of a Square on the Board.
    /// </summary>
    public interface ISquareView
        : IViewBase
    {
        EColor Color { get; }
        Coord Coord { get; }
    }
}
