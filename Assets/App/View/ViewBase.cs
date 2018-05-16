using UnityEngine;
using Flow;

namespace App.View
{
    using Common;

    /// <summary>
    /// Common for all actors in the game. This is to replace
    /// MonoBehavior and make it more rational, as well as to
    /// conform with Flow.ITransient.
    /// </summary>
    public abstract class ViewBase : LoggingBehavior
    {
        private void Awake()
        {
            _constructed = Construct();
        }

        private void Start()
        {
            if (!_constructed)
            {
                Warn("Construction failed for {0}", this);
                //Object.Destroy(this);
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
        }

        private bool _paused;
        private bool _constructed;
        private float _localTime;
        public ITransient _transient;
    }
}
