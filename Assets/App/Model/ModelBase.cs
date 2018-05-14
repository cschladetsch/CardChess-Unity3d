using System;

namespace App.Model
{
    /// <summary>
    /// Common for all Models
    /// </summary>
    public class ModelBase : Logger, IModel
    {
        public Guid Id { get; }

        protected ModelBase()
        {
            Id = Guid.NewGuid();
        }

        protected Main.Arbiter Arbiter => Main.Arbiter.Instance;
    }
}
