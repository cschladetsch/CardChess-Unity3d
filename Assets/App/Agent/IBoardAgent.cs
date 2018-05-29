using System;

namespace App.Agent
{
    using Common;
    using Model;

    public class TimeFrame
    {
        public DateTime Last { get; internal set; }
        public DateTime Now { get; internal set; }
        public TimeSpan Delta { get; internal set; }
    }

    /// <summary>
    /// An agent that acts on behalf of a model board.
    /// </summary>
    public interface IBoardAgent
        : IAgent<IBoardModel>
    {
        void NewGame();
        //Response PlaceCard(TimeFrame when, IPieceAgent cardAgent, Coord where);
        IPieceAgent At(Coord coord);
        //IFuture<IList<IPieceAgent>> AdjacentTo(TimeFrame when, Coord coord, int dist = 1);
        //IObservable
        //  <IList<Coord>> GetMovements(TimeFrame when, Coord coord);

        //Model.BoardModel BoardAt(TimeFrame time);
    }
}
