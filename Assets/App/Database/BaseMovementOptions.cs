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

        #region Private Fields
        private static readonly Vector2[] KingOffsets =
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
        #endregion
    }
}
