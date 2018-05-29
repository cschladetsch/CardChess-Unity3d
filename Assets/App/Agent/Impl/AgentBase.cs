using System;
using App.Model;
using UniRx;

namespace App.Agent
{
    using App.Registry;
    using Common;

    /// <summary>
    /// Common for all agents that manage models in the system.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class AgentBase<TModel>
        : AgentLogger
        , IAgent<TModel>
        where TModel : class, IModel
    {
        #region Public Properties
        public IRegistry<IAgent> Registry { get; set; }
        public Guid Id { get; /*private*/ set; }
        public IModel BaseModel { get; private set; }
        public TModel Model { get; private set; }
        public IReadOnlyReactiveProperty<IOwner> Owner => Model.Owner;
        public bool Destroyed { get; private set; }
        public event DestroyedHandler<IAgent> OnDestroy;
        #endregion

        #region Public Methods
        public bool SameOwner(IOwned other)
        {
            return Owner == other.Owner;
        }

        public virtual bool Construct(TModel a0)
        {
            Assert.IsNotNull(a0);
            Id = Guid.NewGuid();
            BaseModel = Model = a0;
            return true;
        }

        public void Destroy()
        {
            TransientCompleted();
        }
        #endregion
    }
}
