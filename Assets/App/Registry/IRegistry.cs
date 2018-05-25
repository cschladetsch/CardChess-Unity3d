using System;
using System.Collections.Generic;
using App.Common;

namespace App.Registry
{
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
	        , IKnown
	        , IHasDestroyHandler<IBase>
    {
        IEnumerable<IBase> Instances { get; }

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

        //// override for impmentations of abstract types
        //TIBase New<TIBase, TImpl>(params object[] args)
            //where TIBase : class, IBase, IHasRegistry<IBase>, IHasDestroyHandler<IBase>
            //where TImpl : class, TIBase;

        IBase Inject(IBase model, Inject inject, Type iface, IBase single);

        /*
        bool Bind<TInterface, TImpl>(Func<TImpl> creator)
            where TInterface : IBase where TImpl : TInterface;
        bool Bind<TInterface, TImpl, A0>(Func<A0, TImpl> creator)
            where TInterface : IBase where TImpl : TInterface;
        bool Bind<TInterface, TImpl, A0, A1>(Func<A0, A1, TImpl> creator)
            where TInterface : IBase where TImpl : TInterface;
        */
    }
}
