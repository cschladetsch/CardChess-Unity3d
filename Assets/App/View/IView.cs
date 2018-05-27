using UnityEngine;

namespace App.View
{
    using Registry;
    using Agent;

    /// <summary>
    /// Common interface for all views
    /// </summary>
    public interface IView
        : IKnown
        , IHasDestroyHandler<IView>
        , IHasRegistry<IView>
    {
        IAgent AgentBase { get; set; }
        GameObject GameObject { get; }
    }

    public interface IView<TIAgent>
        : IView
        where TIAgent : IAgent
    {
        TIAgent Agent { get; set; }
    }
}
