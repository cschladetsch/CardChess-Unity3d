using System;
using App.Model;
using UnityEngine.Assertions;

namespace App.Agent
{
    public abstract class Base<TModel> :
        Logger, IAgent<TModel>
        where TModel : class, IModel
    {
        public Guid Id { get; private set;}
        public IModel BaseModel { get; private set; }
        public TModel Model { get; private set; }

        public virtual bool Create(TModel a0)
        {
            Assert.IsNotNull(a0);
            Id = Guid.NewGuid();
            BaseModel = Model = a0;
            return true;
        }
    }
}
