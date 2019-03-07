using Flow;

namespace Dekuple.Agent
{
    using Model;
    using Dekuple.Common;

    /// <summary>
    /// Base for agents that perform actions over time.
    /// </summary>
    /// <typeparam name="TModel">The model that this Agent represents</typeparam>
    public abstract class AgentBaseCoro<IAgent, TModel>
        : AgentBase<IAgent, TModel>
        where TModel : class, IModel
        where IAgent : class, IHasDestroyHandler<IAgent>, IHasId
    {
        protected AgentBaseCoro(TModel model)
            : base(model)
        {
        }

        //protected abstract IEnumerator Next(IGenerator self);

        //protected IGenerator _Coro;

        protected INode _Node
        {
            get
            {
                if (_node != null)
                    return _node;
                _node = New.Node();
                _node.Name = Name;
                Root.Add(_node);
                return _node;
            }
        }

        private INode _node;
    }
}
