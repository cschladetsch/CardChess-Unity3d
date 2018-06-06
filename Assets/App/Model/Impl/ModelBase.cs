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
        #region Public Properties
        public bool IsValid { get; protected set; }

        public IRegistry<IModel> Registry { get; set; }
        public string Name { get; set; }
        public Guid Id { get; /*private*/ set; }

        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
        public event Action<IModel> OnDestroyed;
        public IReadOnlyReactiveProperty<IOwner> Owner => _owner;
        #endregion

        #region Public Methods

        protected ModelBase(IOwner owner)
        {
            LogSubject = this;
            LogPrefix = "Model";
            _owner = new ReactiveProperty<IOwner>(owner);
            _destroyed = new BoolReactiveProperty(false);
            Verbosity = Parameters.DefaultLogVerbosity;
            ShowStack = Parameters.DefaultShowTraceStack;
            ShowSource = Parameters.DefaultShowTraceSource;
            Id = Guid.NewGuid();
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
        #endregion

        #region Protected Methods
        protected void NotImplemented(string text)
        {
            Error($"Not {text} implemented");
        }
        #endregion

        #region Private Fields
        private readonly BoolReactiveProperty _destroyed;
        private readonly ReactiveProperty<IOwner> _owner;
        #endregion
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
