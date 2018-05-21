using System;
using System.Diagnostics;
using App.Model;
using UnityEngine.Assertions;

namespace App.Agent
{
    using Common;

    /// <summary>
    /// Common for all agents that manage models in the system.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class AgentBase<TModel> :
        AgentLogger, IAgent<TModel>
        where TModel : class, IModel
    {
        #region Public Properties
        public Guid Id { get; /*private*/ set;}
        public IModel BaseModel { get; private set; }
        public new IArbiterAgent Arbiter { get; set; }
        public TModel Model { get; private set; }
        public IOwner Owner => Model.Owner;
        #endregion

        #region Public Methods
        public bool SameOwner(IOwned other)
        {
            return Owner == other.Owner;
        }

        public virtual bool Construct(TModel a0)
        {
            Assert.IsNotNull(a0);
            Id = Guid.NewGuid();
            BaseModel = Model = a0;
            return true;
        }
        #endregion
    }
}
