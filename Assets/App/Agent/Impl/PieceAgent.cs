using App.Common;
using UniRx;

namespace App.Agent
{
    using Model;

    public class PieceAgent
        : AgentBase<IPieceModel>
        , IPieceAgent
    {
        public IReactiveProperty<Coord> Coord => Model.Coord;

        public PieceAgent(IPieceModel model)
            : base(model)
        {
        }
    }
}
