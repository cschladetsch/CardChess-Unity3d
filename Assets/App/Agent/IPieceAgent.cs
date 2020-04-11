namespace App.Agent
{
    using UniRx;
    using Dekuple.Agent;
    using Model;
    using Common;

    /// <summary>
    /// Agents act on behalf of models
    /// </summary>
    public interface IPieceAgent
        : IAgent<IPieceModel>
        , ICardProperties
    {
        IReactiveProperty<Coord> Coord { get; }
    }
}
