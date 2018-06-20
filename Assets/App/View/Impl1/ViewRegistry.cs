using UnityEngine;

namespace App.View
{
    using Agent;
    using Common;
    using Model;
    using Registry;

    public class ViewRegistry
        : Registry<IViewBase>
        , IViewRegistry
    {
        public override IViewBase Prepare(IViewBase view)
        {
            //?? view.OnDestroyed += v => view.AgentBase.Destroy();
            return base.Prepare(view);
        }

        public TIView FromPrefab<TIView>(Object prefab)
            where TIView : class, IViewBase
        {
            Assert.IsNotNull(prefab);
            var view = Object.Instantiate(prefab) as TIView;
            Assert.IsNotNull(view);
            return Prepare(Prepare(typeof(TIView), view)) as TIView;
        }

        public TIView FromPrefab<TIView, TIAgent, TModel>(IPlayerView player, Object prefab, TModel model)
            where TIView : class , IViewBase
            where TIAgent : class, IAgent, IHasDestroyHandler<IAgent>
            where TModel : IModel
        {
            var view = FromPrefab<TIView>(prefab);
            Assert.IsNotNull(view);
            var agent = player.Agent.Registry.New<TIAgent>(model);
            view.SetAgent(player, agent);
            Assert.IsTrue(view.IsValid);
            return view;
        }
    }
}
