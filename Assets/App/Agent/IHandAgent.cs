using Flow;
using UniRx;

namespace App.Agent
{
    using Common.Message;

    public interface IHandAgent
        : IAgent<Model.IHandModel>
    {
        IReadOnlyReactiveCollection<ICardAgent> Cards { get; }
    }
}
