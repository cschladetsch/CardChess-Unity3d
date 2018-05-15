using System;

namespace App.Model
{
    /// <inheritdoc />
    /// <summary>
    /// Common for all Models
    /// </summary>
    public class ModelBase : Flow.Impl.Logger, IModel
    {
        #region Public Fieleds
        public string Name { get; set; }
        public Guid Id { get; }
        #endregion

        #region Protected Methods
        protected ModelBase()
        {
            Subject = this;
            LogPrefix = "Model";
            Id = Guid.NewGuid();
        }
        #endregion

        #region Protected Fields
        protected Arbiter Arbiter => Arbiter.Instance;
        #endregion
    }
}
