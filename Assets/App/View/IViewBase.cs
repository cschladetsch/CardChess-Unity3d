using App.Common;
using UnityEngine;

namespace App.View
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
        GameObject GameObject { get;}
    }

    public interface IView<TIAgent>
        : IViewBase
        where TIAgent : IAgent
    {
        TIAgent Agent { get; set; }
        void SetAgent(TIAgent agent);
    }
}
