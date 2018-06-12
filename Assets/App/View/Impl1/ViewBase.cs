using System;
using App.Model;
using App.Registry;
using CoLib;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.View.Impl1
{
    using Common;
    using Agent;

    /// <summary>
    /// Common for all Views in the game. This is to replace MonoBehavior and make it more rational, as well as to
    /// conform with Flow.ITransient.
    /// </summary>
    public abstract class ViewBase
        : LoggingBehavior
        , IHasName
        , IViewBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IRegistry<IViewBase> Registry { get; set; }
        public IViewRegistry ViewRegistry => Registry as IViewRegistry;
        public IReadOnlyReactiveProperty<IOwner> Owner => AgentBase.Owner;
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public event Action<IViewBase> OnDestroyed;
        public IAgent AgentBase { get; set; }
        public IPlayerView PlayerView { get; set; }
        public IPlayerModel PlayerModel => Owner.Value as IPlayerModel;
        public GameObject GameObject => gameObject;
        public bool IsValid
        {
            get
            {
                if (Id == Guid.Empty) return false;
                if (Registry == null) return false;
                if (ViewRegistry == null) return false;
                if (AgentBase?.BaseModel?.IsValid == null) return false;
                if (GameObject == null) return false;
                if (!AgentBase.IsValid) return false;
                if (!AgentBase.BaseModel.IsValid) return false;
                return true;
            }
        }

        private void Awake()
        {
            Create();
        }

        public virtual void SetAgent(IPlayerView player, IAgent agent)
        {
            PlayerView = player;
            Assert.IsNotNull(agent);
            AgentBase = agent;
        }

        private void Start()
        {
            Begin();
        }

        private void Update()
        {
            if (_paused)
                return;

            Step();
            _localTime += Time.deltaTime;
        }

        public virtual void Create()
        {
            Assert.IsFalse(_created);
            _created = true;
        }

        protected virtual void Begin()
        {
        }

        protected virtual void Step()
        {
            _queue?.Update(Time.deltaTime);
        }

        public void Pause(bool pause = true)
        {
            _paused = pause;
        }

        public float LifeTime()
        {
            return _localTime;
        }

        public void SetOwner(IOwner owner)
        {
            AgentBase.SetOwner(owner);
        }

        public virtual void Destroy()
        {
            Verbose(4, $"{this} destroyed");
            if (Destroyed.Value)
            {
                Warn($"Object {Id} of type {GetType()} already destoyed");
                return;
            }
            OnDestroyed?.Invoke(this);
            _destroyed.Value = true;
            Object.Destroy(GameObject);
        }

        public override string ToString()
        {
            return $"View {name} of type {GetType()}";
        }

        protected CommandQueue _Queue => _queue ?? (_queue = new CommandQueue());

        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty(false);
        private bool _paused;
        private float _localTime;
        private CommandQueue _queue;
        private bool _created;
    }

    public class ViewBase<TIAgent>
        : ViewBase
        , IView<TIAgent>
        where TIAgent : class, IAgent
    {
        public ViewBase<TIAgent> Pefab;
        public TIAgent Agent => AgentBase as TIAgent;

        public virtual void SetAgent(IPlayerView player, TIAgent agent)
        {
            base.SetAgent(player, agent);
        }
    }
}
