using UnityEngine;

namespace App.View
{
    using Agent;
    using Common;
    using Model;
    using Registry;

    /// <summary>
    /// Common registry for all objects that are in the Unity3d scene (or canvas)
    /// </summary>
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
