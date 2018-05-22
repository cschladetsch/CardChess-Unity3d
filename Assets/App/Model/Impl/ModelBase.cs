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
        IModel
    {
        public bool Destroyed { get; private set; } = false;
        public IBaseRegistry<IModel> Registry { get; set; }
        public string Name { get; set; }
        public Guid Id { get; /*private*/ set; }
        public IOwner Owner { get; protected set; }

        #region Public Methods
        public bool Construct(IOwner owner)
        {
            Owner = owner;
            Subject = this;
            LogPrefix = "Model";
            Verbosity = 100;
            return true;
        }

        public bool SameOwner(IOwned other)
        {
            return Owner == other.Owner;
        }

        public virtual void Destroy()
        {
            if (Destroyed)
            {
                Warn($"Attempt to destroy {this} twice");
                return;
            }

            OnDestroy?.Invoke(this);
            Destroyed = true;
            Id = Guid.Empty;
        }
        #endregion

        public event DestroyedHandler<IModel> OnDestroy;
    }
}
