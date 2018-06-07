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
        public IReadOnlyReactiveProperty<IOwner> Owner => _owner;
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public event Action<IViewBase> OnDestroyed;
        public IAgent AgentBase { get; set; }

        private void Awake()
        {
            Create();
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
            //Verbose(50, $"{this} Begin");
        }

        protected virtual void Step()
        {
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
            _owner.Value = owner;
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

        private readonly ReactiveProperty<IOwner> _owner = new ReactiveProperty<IOwner>();
        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty(false);
        private bool _paused;
        private float _localTime;
        private CoLib.CommandQueue _queue;
    }

    public class ViewBase<TIAgent>
        : ViewBase
        where TIAgent : IAgent
    {
        public ViewBase<TIAgent> Pefab;
        public TIAgent Agent { get; set; }

        public virtual void SetAgent(TIAgent agent)
        {
            //Info($"{this} SetAgent {agent}");
            AgentBase = Agent = agent;
        }
    }
}
