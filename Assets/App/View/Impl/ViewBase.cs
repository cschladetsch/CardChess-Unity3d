using UnityEngine;

namespace App.View.Impl
{
    using Common;
    using Agent;

    /// <summary>
    /// Common for all Views in the game. This is to replace MonoBehavior and make it more rational, as well as to
    /// conform with Flow.ITransient.
    /// </summary>
    public abstract class ViewBase
        : LoggingBehavior
    {
        private void Awake()
        {
            _constructed = Construct();
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

        protected virtual bool Construct()
        {
            return true;
        }

        protected virtual void Begin()
        {
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
            return $"Monobehavior '{name}' of type {GetType()}";
        }

        private bool _paused;
        private bool _constructed;
        private float _localTime;
    }

    public class ViewBase<TIAgent>
        : ViewBase
        where TIAgent : IAgent
    {
        public TIAgent Agent { get; set; }
    }
}
