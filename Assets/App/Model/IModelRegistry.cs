using System;
using System.Collections.Generic;
using App.Common;

namespace App.Model
{
    public interface IModelRegistry
        : IPrintable
    {
        IEnumerable<IModel> Models { get; }
        int NumModels { get; }

        bool HasModel(IModel model);
        bool HasModel(Guid id);
        IModel Get(Guid id);

        TIModel New<TIModel>(params object[] args) where TIModel : class, IModel;

        bool Bind<TInterface, TImpl>(Func<TImpl> creator) where TInterface : IModel where TImpl  : TInterface;
        bool Bind<TInterface, TImpl, A0>(Func<A0, TImpl> creator) where TInterface : IModel where TImpl  : TInterface;
        bool Bind<TInterface, TImpl, A0, A1>(Func<A0, A1, TImpl> creator) where TInterface : IModel where TImpl  : TInterface;
        bool Bind<TInterface, TImpl>(TImpl single) where TInterface : IModel where TImpl  : TInterface;
        bool Bind<TInterface, TImpl>() where TInterface : IModel where TImpl  : TInterface;
        bool Resolve();
    }
}
