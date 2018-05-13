using App.Agent;
using App.Model;
using UnityEngine.Assertions;

namespace App
{
    public class Entity<TModel, TAgent> : IEntity<TModel, TAgent>
        where TModel : class, IModel
        where TAgent : class, IAgent<TModel>
    {
        public IModel BaseModel => Model;
        public IAgent BaseAgent => Agent;
        public TModel Model { get; private set; }
        public TAgent Agent { get; private set; }

        public bool Create(TModel model, TAgent agent)
        {
            Assert.IsNotNull(model);
            Assert.IsNotNull(agent);
            Assert.IsTrue(Agent.Model == model);

            Model = model;
            Agent = agent;

            return true;
        }
    }
}
