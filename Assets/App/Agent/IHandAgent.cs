using UniRx;

namespace App.Agent
{
    public interface IHandAgent
        : IAgent<Model.IHandModel>
    {
        IReadOnlyReactiveCollection<ICardAgent> Cards { get; }
    }
}
