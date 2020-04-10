using Dekuple.Agent;
using UniRx;

namespace App.Agent
{
    public interface IHandAgent
        : IGameAgent<Model.IHandModel>
    {
        IReadOnlyReactiveCollection<ICardAgent> Cards { get; }
    }
}
