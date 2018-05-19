using App.Agent;
using App.Model;
using UnityEngine.Assertions;

namespace App.Common
{
    /// <summary>
    /// An Entity is a pair of Agent and Model.
    /// Note that there can be more than one Agent for a given Model.
    /// </summary>
    /// <typeparam name="TModel">The Model used by the Agent</typeparam>
    /// <typeparam name="TAgent">The Agent in the Entity</typeparam>
    public class Entity<TModel, TAgent> :
        IEntity<TModel, TAgent>
        where TModel : class, IModel
        where TAgent : class, IAgent<TModel>
    {
        #region Public Fields
        public IModel BaseModel => Model;
        public IAgent BaseAgent => Agent;
        public TModel Model { get; private set; }
        public TAgent Agent { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Construct an Agent by binding a Model to an Agent.
        /// Note that there can be more than one Agent for a given Model.
        /// </summary>
        /// <param name="model">The Model to bind from</param>
        /// <param name="agent">The Agent to bind to</param>
        /// <returns></returns>
        public bool Construct(TModel model, TAgent agent)
        {
            Assert.IsNotNull(model);
            Assert.IsNotNull(agent);
            Assert.IsTrue(Agent.Model == model);

            Model = model;
            Agent = agent;

            return true;
        }
        #endregion
    }
}
