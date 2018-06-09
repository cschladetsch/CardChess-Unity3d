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
        public event Action<IAgent> OnDestroyed;
        public IRegistry<IAgent> Registry { get; set; }
        public Guid Id { get; /*private*/ set; }
        public bool IsValid { get; private set;}
        public IModel BaseModel { get; }
        public TModel Model { get; }
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public IReadOnlyReactiveProperty<IOwner> Owner => Model.Owner;

        protected AgentBase(TModel model)
        {
            Assert.IsNotNull(model);
            Verbose(30, $"{this} Construct with {model}");
            BaseModel = model;
            Model = model;
        }

        public virtual void Create()
        {
            IsValid = Model != null && Id != Guid.Empty;
        }

        public void SetOwner(IOwner owner)
        {
            Model.SetOwner(owner);
        }

        public bool SameOwner(IOwned other)
        {
            return Owner.Value == other.Owner.Value;
        }

        public void Destroy()
        {
            TransientCompleted();
            if (!_destroyed.Value)
                _destroyed.Value = true;
            OnDestroyed?.Invoke(this);
        }

        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty(false);
    }
}
