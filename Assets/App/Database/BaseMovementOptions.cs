using System.Collections.Generic;
using App.Action;
using App.Model;

namespace App.Database
{
    using Common;

    /// <summary>
    /// Options for a card on a boardAgent without considering other items etc.
    /// </summary>
    public class BaseMovementOptions
    {
        public IEnumerable<Coord> Options;

        public BaseMovementOptions(ECardType type, Coord start, Model.IBoardModel boardModel)
        {
            _start = start;
            _boardModel = boardModel;

            //Options = Filter(_potentials[type]);
        }

        #region Private Fields
		private static readonly Coord[] KingOffsets =
        {
			new Coord(-1, 0),
			new Coord(-1, 1),
			new Coord(0, 1),
			new Coord(1, 1),
			new Coord(1, 0),
			new Coord(1, -1),
			new Coord(0, -1),
			new Coord(-1, -1),
        };

		private Coord _start;
        private Model.IBoardModel _boardModel;
		private static Dictionary<ECardType, IList<Coord>> _potentials;
        #endregion
    }
}
