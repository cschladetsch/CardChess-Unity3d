using System;
using App.Registry;
using CoLib;
using UniRx;
using UnityEngine;

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
        public bool IsValid { get; protected set; }
        public IRegistry<IViewBase> Registry { get; set; }
        public IViewRegistry ViewRegistry => Registry as IViewRegistry;
        public IReadOnlyReactiveProperty<IOwner> Owner => AgentBase.Owner;
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public event Action<IViewBase> OnDestroyed;
        public IAgent AgentBase { get; set; }
        public IPlayerView Player { get; set; }
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            Create();
        }

        public virtual void SetAgent(IPlayerView player, IAgent agent)
        {
            Player = player;
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
            Verbose(20, $"{this} destroyed");
            if (Destroyed.Value)
                return;
            OnDestroyed?.Invoke(this);
            _destroyed.Value = true;
        }

        public override string ToString()
        {
            return $"View {name} of type {GetType()}";
        }

        protected CoLib.CommandQueue _Queue => _queue ?? (_queue = new CommandQueue());

        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty(false);
        private bool _paused;
        private float _localTime;
        private CoLib.CommandQueue _queue;
    }

    public class ViewBase<TIAgent>
        : ViewBase
            , IView<TIAgent>
        where TIAgent : IAgent
    {
        public ViewBase<TIAgent> Pefab;
        public TIAgent Agent { get; set; }

        public virtual void SetAgent(IPlayerView player, TIAgent agent)
        {
            base.SetAgent(player, agent);
            Agent = agent;
        }
    }
}
