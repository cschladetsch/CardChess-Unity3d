using UnityEngine;

namespace App.View
{
    using Agent;
    using Dekuple.Common;
    using Dekuple.Registry;

    /// <summary>
    /// Common interface for all views
    /// </summary>
    public interface IViewBase
        : IEntity
        , IHasDestroyHandler<IViewBase>
        , IHasRegistry<IViewBase>
    {
        IAgent AgentBase { get; set; }
        IPlayerView PlayerView { get; set; }
        GameObject GameObject { get; }
        IViewRegistry ViewRegistry { get; }

        void SetAgent(IPlayerView player, IAgent agent);
    }

    public interface IView<TIAgent>
        : IViewBase
        where TIAgent : IAgent
    {
        TIAgent Agent { get; }
    }
}
