using System;

namespace App.Model
{
    using Common;

    /// <summary>
    /// Common for all Models
    /// </summary>
    public class ModelBase :
        Flow.Impl.Logger,
        IModel,
        ICreateWith<IOwner>
    {
        public string Name { get; set; }
        public Guid Id { get; private set; }
        public IOwner Owner { get; private set; }

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

        protected Arbiter Arbiter => Arbiter.Instance;
    }
}
