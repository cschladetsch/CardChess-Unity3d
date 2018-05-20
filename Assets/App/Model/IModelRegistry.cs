using System;
using App.Common;

namespace App.Model
{
    public interface IModelRegistry
    {
        IModel Get(Guid id);
        TModel New<TModel>(params object[] args) where TModel : class, IModel, new();
    }
}
