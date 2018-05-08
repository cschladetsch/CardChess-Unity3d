using UnityEngine;
using Flow;

namespace App
{
    /// <summary>
    /// Common for all actors in the game. This is to replace
    /// MonoBehavior and make it more rational, as well as to 
    /// conform with Flow.ITransient.
    /// </summary>
    public /*abstract*/ class Agent/*Base*/ : Logger, ITransient
    {
        private void Awake()
        {
            _constructed = Construct();
        }

        private void Start()
        {
            if (!_constructed)
            {
                Warn("Construction failed");
                MonoBehaviour.Destroy(this);
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

        protected virtual void Destroy()
        {
        }

        protected virtual void Begin()
        {
        }

        protected virtual void Step()
        {
        }

        public void Pause()
        {
            _paused = true;
        }

        public float LifeTime()
        {
            return _localTime;
        }

#region ITransient Implementation
        public event TransientHandler Completed;
        public bool Active { get; private set; }
        public IKernel Kernel { get; set; }
        public void Complete()
        {
            if (!Active)
                return;
            if (Completed != null)
                Completed(this);
            Active = false;
        }
#endregion

        private bool _paused;
        private bool _constructed;
        private float _localTime;
    }
}
