using System;
using App.Model;
using UniRx;

namespace App.Agent
{
    using Registry;
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
        public IModel BaseModel { get; }
        public TModel Model { get; }
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public IReadOnlyReactiveProperty<IOwner> Owner => Model.Owner;
        public bool IsValid
        {
            get
            {
                if (Registry == null) return false;
                if (Id == Guid.Empty) return false;
                if (BaseModel == null) return false;
                if (Model != BaseModel) return false;
                if (!Model.IsValid) return false;
                return true;
            }
        }

        protected AgentBase(TModel model)
        {
            Assert.IsNotNull(model);
            BaseModel = model;
            Model = model;
        }

        public virtual void Create()
        {
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
