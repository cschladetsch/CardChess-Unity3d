using UnityEngine;
using Flow;

namespace App
{
    //public class TransientBase<Impl> where Impl : ITransient
    //{
    //    Impl Transient { get; protected set; }
    //    public event TransientHandler Completed;
    //    public bool Active { get; private set; }
    //    public IKernel Kernel { get; set; }
    //    public void Complete()
    //    {
    //        if (!Active)
    //            return;
    //        if (Completed != null)
    //            Completed(_transient);
    //        Active = false;
    //    }
    //}

    /// <summary>
    /// Common for all actors in the game. This is to replace
    /// MonoBehavior and make it more rational, as well as to 
    /// conform with Flow.ITransient.
    /// </summary>
    public /*abstract*/ class Agent/*Base*/ : Logger//, TransientBase<Agent>
    {
        public Agent Transient { get; private set; }

        private void Awake()
        {
            _constructed = Construct();
        }

        private void Start()
        {
            if (!_constructed)
            {
                Warn("Construction failed for {0}", this);
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
