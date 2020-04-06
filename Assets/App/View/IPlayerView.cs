namespace App.View
{
    using System;
    using Dekuple;
    using Dekuple.View;
    using Agent;

    /// <summary>
    /// View of a player in the scene
    /// </summary>
    public interface IPlayerView
        : IView<IPlayerAgent>
    {
        // void SetAgent(IPlayerView view, IPlayerAgent agent);
        
        /// <summary>
        /// TODO: Remove
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void PushRequest(IRequest request, Action<IResponse> response);
    }
}
