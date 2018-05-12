using System.Collections.Generic;
using App.Action;
using App.Model;

namespace App.Database
{
    /// <summary>
    /// Options for a card on a board without considering other items etc.
    /// </summary>
    public class BaseMovementOptions
    {
        public IEnumerable<Coord> Options;

        public BaseMovementOptions(ECardType type, Vector2 start, Model.IBoard board)
        {
            _start = start;
            _board = board;

            //Options = Filter(_potentials[type]);
        }

        IEnumerable<Coord> Filter(Vector2[] offsets)
        {
            foreach (var v in offsets)
            {
                var dest = _start + v;
                if (dest.X < 0 || dest.Y < 0)
                    continue;
                if (dest.X >= _board.Width || dest.Y >= _board.Height)
                    continue;

                yield return new Coord(dest.X, dest.Y);
            }
        }

        static readonly Vector2[] KingOffsets =
        {
            new Vector2(-1, 0),
            new Vector2(-1, 1),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(1, -1),
            new Vector2(0, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),
        };

        private Vector2 _start;
        private Model.IBoard _board;
        private static Dictionary<ECardType, IList<Vector2>> _potentials;
    }
}
