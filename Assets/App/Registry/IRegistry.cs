using System;
using System.Collections.Generic;
using App.Common;

namespace App.Registry
{
    public interface IRegistry
    {
        int NumInstances { get; }
        bool Has(Guid id);
        bool Resolve();
    }

    public interface IRegistry<IBase>
        : IPrintable, IRegistry
        where IBase : class, IKnown, IHasDestroyHandler<IBase>
    {
        IEnumerable<IBase> Instances { get; }

        bool Has(IBase instance);
        IBase Get(Guid id);

        TIBase New<TIBase>(params object[] args)
            where TIBase
            : class, IBase,
            IHasRegistry<IBase>,
            IHasDestroyHandler<IBase>;

        bool Bind<TInterface, TImpl>(Func<TImpl> creator) where TInterface : IBase where TImpl : TInterface;
        bool Bind<TInterface, TImpl, A0>(Func<A0, TImpl> creator) where TInterface : IBase where TImpl : TInterface;
        bool Bind<TInterface, TImpl, A0, A1>(Func<A0, A1, TImpl> creator) where TInterface : IBase where TImpl : TInterface;
        bool Bind<TInterface, TImpl>(TImpl single) where TInterface : IBase where TImpl : TInterface;
        bool Bind<TInterface, TImpl>() where TInterface : IBase where TImpl : TInterface;

        IBase Inject(IBase model, Inject inject, Type iface, IBase single);
    }
}
