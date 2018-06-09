using System;
using App.Common.Message;
using UniRx;

namespace App.Model
{
    using Common;
    using Registry;

    /// <summary>
    /// Common for all Models.
    ///
    /// Models are created from a Registry, have an OnDestroyed event, and are persistent by default.
    /// </summary>
    public abstract class ModelBase
        : Flow.Impl.Logger
        , IModel
    {
        public event Action<IModel> OnDestroyed;
        public bool Prepared { get; protected set; }
        public IRegistry<IModel> Registry { get; set; }
        public string Name { get; set; }
        public Guid Id { get; /*private*/ set; }
        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public IReadOnlyReactiveProperty<IOwner> Owner => _owner;
        public bool IsValid
        {
            get
            {
                if (Registry == null) return false;
                if (Id == Guid.Empty) return false;
                return true;
            }
        }

        protected ModelBase(IOwner owner)
        {
            LogSubject = this;
            LogPrefix = "Model";
            _owner = new ReactiveProperty<IOwner>(owner);
            _destroyed = new BoolReactiveProperty(false);
            Verbosity = Parameters.DefaultLogVerbosity;
            ShowStack = Parameters.DefaultShowTraceStack;
            ShowSource = Parameters.DefaultShowTraceSource;
        }

        public bool SameOwner(IOwned other)
        {
            return Owner.Value == other.Owner.Value;
        }

        public virtual void Create()
        {
        }

        public virtual void Prepare()
        {
            Assert.IsFalse(_prepared);
            if (_prepared)
            {
                Error($"{this} has already been prepared");
                return;
            }
            _prepared = true;
        }

        public virtual void Destroy()
        {
            if (Destroyed.Value)
            {
                Warn($"Attempt to destroy {this} twice");
                return;
            }

            _destroyed.Value = true;
            Id = Guid.Empty;
        }

        public void SetOwner(IOwner owner)
        {
            if (_owner.Value == owner)
                return;

            //Verbose(20, $"{this} changes ownership from {Owner.Value} to {owner}");
            _owner.Value = owner;
        }

        protected void NotImplemented(string text)
        {
            Error($"Not {text} implemented");
        }

        private readonly BoolReactiveProperty _destroyed;
        private readonly ReactiveProperty<IOwner> _owner;
        private bool _prepared;
    }
}

static class ModelExt
{
    public static T AddTo<T>(this T disposable, App.Model.IModel model)
        where T : IDisposable
    {
        model.Destroyed.Subscribe(m => disposable.Dispose());
        return disposable;
    }
}
