using System;
using System.Diagnostics;
using App.Model;
using UnityEngine.Assertions;

namespace App.Agent
{
    using Common;

    public abstract class AgentBase<TModel> :
        AgentLogger, IAgent<TModel>
        where TModel : class, IModel
    {
        public Guid Id { get; private set;}
        public IModel BaseModel { get; private set; }
        public Arbiter Arbiter { get; set; }
        public TModel Model { get; private set; }
		public TModel MyModel { get { return Model; } }
        public IOwner Owner => Model.Owner;

        public virtual bool Create(TModel a0)
        {
            Assert.IsNotNull(a0);
            Id = Guid.NewGuid();
            BaseModel = Model = a0;
            return true;
        }
    }
}
