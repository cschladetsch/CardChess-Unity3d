using System;

namespace App.Model
{
    using Common;

    /// <summary>
    /// Common for all Models.
    ///
    /// Models are created from a Registry, have an OnDestroyed event, and are persistent by default.
    /// </summary>
    public class ModelBase :
        Flow.Impl.Logger,
        IModel,
        ICreateWith<IOwner>
    {
        public bool Destroyed { get; private set; } = false;
        public Registry Registry { get; set; }
        public string Name { get; set; }
        public Guid Id { get; private set; }
        public IOwner Owner { get; private set; }

        public event ModelDestroyedHandler OnDestroy;

        protected ModelBase()
        {
            Id = Guid.NewGuid();
            Subject = this;
            LogPrefix = "Model";
        }

        public bool Create(IOwner a0)
        {
            Owner = a0;
            return true;
        }

        public bool SameOwner(IOwner other)
        {
            return Owner == other;
        }

        public virtual void Destroy()
        {
            Destroyed = true;
            Id = Guid.Empty;
        }
    }
}
