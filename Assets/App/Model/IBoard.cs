using System.Collections.Generic;

namespace App.Model
{
    /// <summary>
    /// The NxN playing board. Typically 6x6, 10x10, or 12x12 (Desktop only)
    /// </summary>
    public interface IBoard :
        IModel,
        ICreateWith<int, int>
    {
        #region Properties
        int Width { get; }
        int Height { get; }
        #endregion

        #region Methods
        void NewGame();
        ICardInstance GetContents(Action.Coord coord);
        ICardInstance At(Action.Coord coord);
        bool IsValidCoord(Action.Coord coord);
        IEnumerable<ICardInstance> GetContents();
        #endregion
    }
}
