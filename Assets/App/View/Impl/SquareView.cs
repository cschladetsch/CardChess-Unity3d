#pragma warning disable 649

namespace App.View.Impl
{
    using Dekuple.View.Impl;
    using Common;

    /// <inheritdoc cref="ISquareView" />
    /// <summary>
    /// View of a square on the play board
    /// </summary>
    public class SquareView
        : ViewBase
        , ISquareView
    {
        public EColor Color { get; set; }
        public Coord Coord { get; internal set; }
        public float Length;
        public override bool IsValid => true;

        public override int GetHashCode()
        {
            // this is only set once, I promise.
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Coord.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var sq = other as SquareView;
            if (sq == null)
                return false;
            return Coord == sq.Coord;
        }

        public override string ToString() => $"{Color} {Coord}";
    }
}
