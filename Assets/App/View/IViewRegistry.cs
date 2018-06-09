using App.Agent;
using App.Common;
using App.Model;
using App.Registry;
using UnityEngine;

namespace App.View
{
    public interface IViewRegistry
        : IRegistry<IViewBase>
    {
        TIView FromPrefab<TIView>(Object prefab)
            where TIView : class, IViewBase;

        TIView FromPrefab<TIView, TIAgent, TModel>(
            IPlayerView player, Object prefab, TModel model)
            where TIView : class, IViewBase
            where TIAgent : class, IAgent, IHasDestroyHandler<IAgent>
            where TModel : IModel;
    }
}
