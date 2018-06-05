using System;
using App.Registry;
using CoLib;
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
        , IHasId
        , IHasName
        , IHasDestroyHandler<IViewBase>
        , IHasRegistry<IViewBase>
    {
        //public ViewBase PrefabBase;
        public IAgent AgentBase { get; set; }

        private void Awake()
        {
            _constructed = Create();
        }

        private void Start()
        {
            if (!_constructed)
            {
                Warn($"Construction failed for {this}");
                return;
            }

            Begin();
        }

        private void Update()
        {
            if (_paused)
                return;

            Step();
            _localTime += Time.deltaTime;
        }

        protected virtual bool Create()
        {
            return true;
        }

        protected virtual void Begin()
        {
            Info($"{this} Begin");
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

        protected virtual void Destroy()
        {
            Verbose(20, $"{this} destroyed");
        }

        public override string ToString()
        {
            return $"View {name} of type {GetType()}";
        }

        private bool _paused;
        private bool _constructed;
        private float _localTime;

        CoLib.CommandQueue _queue = new CommandQueue();
        public Guid Id { get; set; }
        public string Name { get; set; }
        public event DestroyedHandler<IViewBase> OnDestroy;
        public IRegistry<IViewBase> Registry { get; set; }
    }

    public class ViewBase<TIAgent>
        : ViewBase
        , IConstructWith<TIAgent>
        where TIAgent : IAgent
    {
        public ViewBase<TIAgent> Pefab;
        public TIAgent Agent { get; set; }

        public virtual bool Construct(TIAgent agent)
        {
            Assert.IsNotNull(agent);
            if (agent == null)
                return false;
            Info($"{this} Construct");
            AgentBase = Agent = agent;
            return true;
        }
    }
}
