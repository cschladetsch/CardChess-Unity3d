using System;
using App.Model;
using UniRx;

namespace App.Agent
{
    using Common;
    using Dekuple.Registry;
    using Dekuple.Model;
    using Dekuple.Common;

    /// <summary>
    /// Common for all agents that manage models in the system.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class AgentBase<IAgent, TModel>
        : Dekuple.Agent.AgentBase<IAgent, TModel>
        where TModel : class, Dekuple.Model.IModel
        where IAgent : class, IHasId, IHasDestroyHandler<IAgent>
    {
        [Inject] public IBoardAgent Board { get; set; }
        [Inject] public IArbiterAgent Arbiter { get; set; }

        //public virtual bool IsValid
        //{
        //    get
        //    {
        //        if (Id == Guid.Empty) return false;
        //        if (Registry == null) return false;
        //        if (BaseModel == null) return false;
        //        if (!Model.IsValid) return false;
        //        return true;
        //    }
        //}

        public AgentBase(TModel model)
        {
            if (model == null)
            {
                Error("Model cannot be null");
                return;
            }
            Assert.IsNotNull(model);
            BaseModel = model;
        }

        //public bool SameOwner(IEntity other)
        //{
        //    if (other == null)
        //        return Owner.Value == null;
        //    return other.Owner.Value == Owner.Value;
        //}

        //public void SetOwner(IOwner owner)
        //{
        //    Model.SetOwner(owner);
        //}

        //public bool SameOwner(IOwned other)
        //{
        //    if (other == null)
        //        return Owner.Value == null;
        //    return other.Owner.Value == Owner.Value;
        //}

        //public virtual void StartGame()
        //{
        //    Assert.IsFalse(_started);
        //    _started = true;
        //}

        //public virtual void EndGame()
        //{
        //    _started = false;
        //}

        //public void Destroy()
        //{
        //    TransientCompleted();
        //    if (!_destroyed.Value)
        //        _destroyed.Value = true;
        //    OnDestroyed?.Invoke(this);
        //}

        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty(false);
        private bool _started = false;
    }
}
