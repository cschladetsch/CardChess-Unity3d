using System;

namespace App.Model
{
    /// <inheritdoc />
    /// <summary>
    /// Common for all Models
    /// </summary>
    public class ModelBase : Logger, IModel
    {
        #region Public Fieleds
        public Guid Id { get; }
        #endregion

        #region Protected Methods
        protected ModelBase()
        {
            Id = Guid.NewGuid();
        }
        #endregion

        #region Protected Fields
        protected Arbiter Arbiter => Arbiter.Instance;
        #endregion
    }
}
