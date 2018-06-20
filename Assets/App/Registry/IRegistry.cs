using System;
using System.Collections.Generic;

namespace App.Registry
{
    using Common;

    public interface IRegistry
        : IPrintable
    {
        int NumInstances { get; }
        bool Has(Guid id);
        bool Resolve();
    }

    public interface IRegistry<IBase>
        : IRegistry
        where IBase
            : class
            , IHasId
            , IHasDestroyHandler<IBase>
    {
        IEnumerable<IBase> Instances { get; }

        string Save();
        void Load(string text);

        bool Has(IBase instance);
        IBase Get(Guid id);

        // bind an interface to an implementation, which can be abstract
        bool Bind<TInterface, TImpl>()
            where TInterface : IBase where TImpl : TInterface;

        // bind an interface to a singleton
        bool Bind<TInterface, TImpl>(TImpl single)
            where TInterface : IBase where TImpl : TInterface;

        // make a new instance given interface
        TIBase New<TIBase>(params object[] args)
            where TIBase : class, IBase, IHasRegistry<IBase>, IHasDestroyHandler<IBase>;

        IBase Inject(IBase model, Inject inject, Type iface, IBase single);

        IBase Prepare(IBase model);
    }
}
