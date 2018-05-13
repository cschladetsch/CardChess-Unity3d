using System.Collections.Generic;

namespace App.Model
{
    public interface IBoard : IModel, ICreateWith<int, int>
    {
        int Width { get; }
        int Height { get; }

        void NewGame();
        ICardInstance GetContents(Action.Coord coord);
        ICardInstance At(Action.Coord coord);
        bool IsValidCoord(Action.Coord coord);
        IEnumerable<ICardInstance> GetContents();
    }
}
