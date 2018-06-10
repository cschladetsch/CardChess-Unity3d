using UnityEngine;

namespace App.View
{
    using Registry;
    using Agent;
    using Common;

    /// <summary>
    /// Common interface for all views
    /// </summary>
    public interface IViewBase
        : IEntity
        , IHasDestroyHandler<IViewBase>
        , IHasRegistry<IViewBase>
    {
        IAgent AgentBase { get; set; }
        IPlayerView Player { get; set; }
        GameObject GameObject { get; }
        IViewRegistry ViewRegistry { get; }

        void SetAgent(IPlayerView player, IAgent agent);
    }

    public interface IView<TIAgent>
        : IViewBase
        where TIAgent : IAgent
    {
        TIAgent Agent { get; }
        //void SetAgentTyped(IPlayerView player, TIAgent agent);
    }
}
