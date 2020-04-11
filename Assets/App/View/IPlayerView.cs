<<<<<<< HEAD
﻿using System;

using Dekuple;
using Dekuple.Agent;
using Dekuple.View;

namespace App.View
=======
﻿namespace App.View
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
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
<<<<<<< HEAD
=======
        // void SetAgent(IPlayerView view, IPlayerAgent agent);
        
        /// <summary>
        /// TODO: Remove
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
        void PushRequest(IRequest request, Action<IResponse> response);
    }
}
