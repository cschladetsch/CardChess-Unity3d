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
        public IModel BaseModel { get; }
        public TModel Model { get; }
        public IReadOnlyReactiveProperty<IOwner> Owner => Model.Owner;
        public bool Destroyed { get; private set; }
        public event DestroyedHandler<IAgent> OnDestroy;
        #endregion

        protected AgentBase(TModel a0)
        {
            Assert.IsNotNull(a0);
            Info($"{this} Construct with {a0}");
            Id = Guid.NewGuid();
            BaseModel = a0;
            Model = a0;
        }

        public virtual void Construct()
        {
        }

        #region Public Methods
        public bool SameOwner(IOwned other)
        {
            return Owner == other.Owner;
        }

        public void Destroy()
        {
            TransientCompleted();
            OnDestroy?.Invoke(this);
        }
        #endregion
    }
}
