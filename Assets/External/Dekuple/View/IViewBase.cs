using UnityEngine;

namespace Dekuple.View
{
    using Registry;
    using Agent;

    /// <summary>
    /// Common interface for all views
    /// </summary>
    public interface IViewBase
        : IEntity
        , IHasDestroyHandler<IViewBase>
        , IHasRegistry<IViewBase>
    {
        IAgent AgentBase { get; set; }
        GameObject GameObject { get; }
        //IPlayerView PlayerView { get; set; }
        //IViewRegistry ViewRegistry { get; }

        void SetAgent(IOwner owner, IAgent agent);
    }

    public interface IView<out TIAgent>
        : IViewBase
        where TIAgent : IAgent
    {
        TIAgent Agent { get; }
    }
}
