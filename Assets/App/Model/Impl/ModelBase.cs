using System;

namespace App.Model
{
    using Common;

    /// <summary>
    /// Common for all Models
    /// </summary>
    [Persistent]
    public class ModelBase :
        Flow.Impl.Logger,
        IModel,
        ICreateWith<IOwner>
    {
        public string Name { get; set; }
        public Guid Id { get; private set; }
        public Common.IOwner Owner { get; private set; }

        public bool Create(IOwner a0)
        {
            Id = Guid.NewGuid();
            Owner = a0;
            return true;
        }

        protected ModelBase()
        {
            Subject = this;
            LogPrefix = "Model";
            Id = Guid.NewGuid();
        }

        protected Arbiter Arbiter => Arbiter.Instance;
    }
}
